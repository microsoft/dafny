﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Dafny.LanguageServer.Handlers;
using Microsoft.Dafny.LanguageServer.Language;
using Microsoft.Dafny.LanguageServer.Workspace;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Server;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Boogie.SMTLib;
using Microsoft.Extensions.Options;
using Action = System.Action;

namespace Microsoft.Dafny.LanguageServer {
  public static class DafnyLanguageServer {
    private static readonly List<string> pluginLoadErrors = new();
    public static IReadOnlyList<string> PluginLoadErrors => pluginLoadErrors;
    private static string DafnyVersion {
      get {
        var version = typeof(DafnyLanguageServer).Assembly.GetName().Version!;
        return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
      }
    }

    public static LanguageServerOptions WithDafnyLanguageServer(this LanguageServerOptions options,
        IConfiguration configuration, Action killLanguageServer) {
      options.ServerInfo = new ServerInfo {
        Name = "Dafny",
        Version = DafnyVersion
      };
      return options
        .WithDafnyLanguage(configuration)
        .WithDafnyWorkspace(configuration)
        .WithDafnyHandlers()
        .OnInitialize((server, @params, token) => InitializeAsync(server, @params, token, killLanguageServer))
        .OnStarted(StartedAsync);
    }

    private static Task InitializeAsync(ILanguageServer server, InitializeParams request, CancellationToken cancelRequestToken,
        Action killLanguageServer) {
      var logger = server.GetRequiredService<ILogger<Program>>();
      logger.LogTrace("initializing service");

      LoadPlugins(logger, server);

      KillLanguageServerIfParentDies(logger, request, killLanguageServer);

      PublishSolverPath(server);

      return Task.CompletedTask;
    }

    private static readonly Regex Z3VersionRegex = new Regex(@"Z3 version (?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)");

    private static void PublishSolverPath(ILanguageServer server) {
      var telemetryPublisher = server.GetRequiredService<ITelemetryPublisher>();
      string solverPath;
      try {
        var proverOptions = new SMTLibSolverOptions(DafnyOptions.O);
        proverOptions.Parse(DafnyOptions.O.ProverOptions);
        solverPath = proverOptions.ExecutablePath();
        HandleZ3Version(telemetryPublisher, proverOptions);
      } catch (Exception e) {
        solverPath = $"Error while determining solver path: {e}";
      }

      telemetryPublisher.PublishSolverPath(solverPath);
    }

    private static void HandleZ3Version(ITelemetryPublisher telemetryPublisher, SMTLibSolverOptions proverOptions) {
      var z3Process = new ProcessStartInfo(proverOptions.ProverPath, "-version") {
        CreateNoWindow = true,
        RedirectStandardError = true,
        RedirectStandardOutput = true,
        RedirectStandardInput = true
      };
      var run = Process.Start(z3Process);
      if (run == null) {
        return;
      }

      var actualOutput = run.StandardOutput.ReadToEnd();
      run.WaitForExit();
      var versionMatch = Z3VersionRegex.Match(actualOutput);
      if (!versionMatch.Success) {
        // Might be another solver.
        return;
      }

      telemetryPublisher.PublishZ3Version(versionMatch.Value);
      var major = int.Parse(versionMatch.Groups["major"].Value);
      var minor = int.Parse(versionMatch.Groups["minor"].Value);
      var patch = int.Parse(versionMatch.Groups["patch"].Value);
      if (major <= 4 && (major != 4 || minor <= 8) && (minor != 8 || patch <= 6)) {
        return;
      }

      var toReplace = "O:model_compress=false";
      var i = DafnyOptions.O.ProverOptions.IndexOf(toReplace);
      if (i == -1) {
        telemetryPublisher.PublishUnhandledException(new Exception($"Z3 version is > 4.8.6 but I did not find {toReplace} in the prover options:" + string.Join(" ", DafnyOptions.O.ProverOptions)));
        return;
      }

      DafnyOptions.O.ProverOptions[i] = "O:model.compact=false";
    }

    /// <summary>
    /// Load the plugins for the Dafny pipeline
    /// </summary>
    private static void LoadPlugins(ILogger<Program> logger, ILanguageServer server) {
      var dafnyPluginsOptions = server.GetRequiredService<IOptions<DafnyPluginsOptions>>();
      var lastPlugin = "";
      try {
        foreach (var pluginPathArgument in dafnyPluginsOptions.Value.Plugins) {
          lastPlugin = pluginPathArgument;
          DafnyOptions.O.Parse(new[] { "-plugin:" + pluginPathArgument });
        }
      } catch (Exception e) {
        logger.LogError(e, $"Error while instantiating plugin {lastPlugin}");
        pluginLoadErrors.Add($"Error while instantiating plugin {lastPlugin}. Please restart the server.\n" + e);
      }
    }

    /// <summary>
    /// As part of the LSP spec, a language server must kill itself if its parent process dies
    /// https://github.com/microsoft/language-server-protocol/blob/gh-pages/_specifications/specification-3-16.md?plain=1#L1713
    /// </summary>
    private static void KillLanguageServerIfParentDies(ILogger<Program> logger, InitializeParams request,
        Action killLanguageServer) {
      if (!(request.ProcessId >= 0)) {
        return;
      }

      void Kill() {
        logger.LogWarning("Shutting down language server because parent process died.");
        killLanguageServer();
      }

      try {
        var hostProcess = Process.GetProcessById((int)request.ProcessId);
        hostProcess.EnableRaisingEvents = true;
        hostProcess.Exited += (_, _) => Kill();
      } catch (ArgumentException) {
        // If the process dies before we get here then request shutdown immediately
        Kill();
      }
    }

    private static Task StartedAsync(ILanguageServer server, CancellationToken cancellationToken) {
      // TODO this currently only sent to get rid of the "Server answer pending" of the VSCode plugin.
      server.SendNotification("serverStarted", DafnyVersion);
      server.SendNotification("dafnyLanguageServerVersionReceived", DafnyVersion);
      return Task.CompletedTask;
    }
  }
}
