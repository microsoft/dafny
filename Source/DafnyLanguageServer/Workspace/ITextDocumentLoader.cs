﻿using System;
using System.Collections.Generic;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Boogie;
using Microsoft.Dafny.LanguageServer.Workspace.Notifications;

namespace Microsoft.Dafny.LanguageServer.Workspace {
  /// <summary>
  /// Implementations are responsible to load a specified language server document and generate
  /// a dafny document out of it.
  /// </summary>
  public interface ITextDocumentLoader {

    Task<Program> ParseAsync(ErrorReporter reporter, CompilationInput compilation,
      IReadOnlyDictionary<Uri, DocumentVerificationTree> migratedVerificationTrees, CancellationToken cancellationToken);

    Task<Resolution> ResolveAsync(CompilationInput input,
      Program program,
      CancellationToken cancellationToken);
  }
}
