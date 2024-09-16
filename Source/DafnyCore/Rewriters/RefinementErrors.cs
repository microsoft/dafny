using System.Collections.Generic;
using static Microsoft.Dafny.ErrorRegistry;

namespace Microsoft.Dafny;

public class RefinementErrors {

  public enum ErrorId {
    // ReSharper disable once InconsistentNaming
    ref_refinement_import_must_match_opened_base,
    ref_refinement_import_must_match_non_opened_base,
    ref_refinement_type_must_match_base,
    ref_refining_notation_needed,
    ref_refining_notation_does_not_refine,
    ref_default_export_unchangeable,
    ref_module_must_refine_module,
    ref_export_must_refine_export,
    ref_base_module_must_be_facade,
    ref_module_must_refine_module_2,
    ref_mismatched_type_characteristics_equality,
    ref_mismatched_type_characteristics_autoinit,
    ref_mismatched_type_characteristics_nonempty,
    ref_mismatched_type_characteristics_noreferences,
    ref_mismatched_type_with_members,
    ref_mismatched_abstractness,
    ref_declaration_must_refine,
    ref_iterator_must_refine_iterator,
    ref_base_type_cannot_be_refined,
    ref_base_module_must_be_abstract_or_alias,
    ref_no_new_iterator_preconditions,
    ref_no_new_iterator_yield_preconditions,
    ref_no_new_iterator_reads,
    ref_no_new_iterator_modifies,
    ref_no_new_iterator_decreases,
    ref_const_must_refine_const,
    ref_no_changed_const_type,
    ref_no_refining_const_initializer,
    ref_mismatched_module_static,
    ref_mismatched_const_ghost,
    ref_refinement_must_add_const_ghost,
    ref_field_must_refine_field,
    ref_mismatched_field_name,
    ref_refinement_field_must_add_ghost,
    ref_mismatched_refinement_kind,
    ref_refinement_no_new_preconditions,
    ref_refinement_no_new_reads,
    ref_no_new_decreases,
    ref_mismatched_function_static,
    ref_mismatched_function_compile,
    ref_no_refinement_function_with_body,
    ref_mismatched_function_return_name,
    ref_mismatched_function_return_type,
    ref_mismatched_refinement_body,
    ref_method_refines_method,
    ref_no_new_method_precondition,
    ref_no_new_method_reads,
    ref_no_new_method_modifies,
    ref_no_new_method_decreases,
    ref_mismatched_method_static,
    ref_mismatched_method_ghost,
    ref_mismatched_method_non_ghost,
    ref_mismatched_type_parameters_count,
    ref_mismatched_type_parameter_name,
    ref_mismatched_type_parameter_equality,
    ref_mismatched_type_parameter_auto_init,
    ref_mismatched_type_parameter_nonempty,
    ref_mismatched_type_parameter_not_reference,
    ref_mismatched_type_parameter_variance,
    ref_mismatched_type_bounds_count,
    ref_mismatched_type_parameter_bound,
    ref_mismatched_kind_count,
    ref_mismatched_kind_name,
    ref_mismatched_kind_ghost,
    ref_mismatched_kind_non_ghost,
    ref_mismatched_kind_non_new,
    ref_mismatched_kind_new,
    ref_mismatched_kind_older,
    ref_mismatched_kind_non_older,
    ref_mismatched_parameter_type,
    ref_refined_formal_may_not_have_default,
    ref_mismatched_skeleton,
    ref_mismatched_assert,
    ref_mismatched_expect,
    ref_mismatched_assume,
    ref_mismatched_if_statement,
    ref_mismatched_while_statement,
    ref_mismatched_while_statement_guard,
    ref_mismatched_modify_statement,
    ref_mismatched_statement_body,
    ref_mismatched_loop_decreases,
    ref_mismatched_while_body,
    ref_misplaced_skeleton,
    ref_misplaced_yield,
    ref_invalid_break_in_skeleton,
    ref_misplaced_assignment,
    ref_misplaced_call,
    ref_invalid_variable_assignment,
    ref_invalid_field_assignment,
    ref_invalid_new_assignments,
    ref_invalid_assignment_rhs,
  }
}
