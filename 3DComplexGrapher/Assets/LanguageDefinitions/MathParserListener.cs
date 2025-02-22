//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from MathParser.g4 by ANTLR 4.7.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="MathParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
public interface IMathParserListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="MathParser.add_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAdd_expr([NotNull] MathParser.Add_exprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MathParser.add_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAdd_expr([NotNull] MathParser.Add_exprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MathParser.mult_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMult_expr([NotNull] MathParser.Mult_exprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MathParser.mult_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMult_expr([NotNull] MathParser.Mult_exprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MathParser.exp_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExp_expr([NotNull] MathParser.Exp_exprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MathParser.exp_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExp_expr([NotNull] MathParser.Exp_exprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MathParser.final_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFinal_expr([NotNull] MathParser.Final_exprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MathParser.final_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFinal_expr([NotNull] MathParser.Final_exprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MathParser.exp_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExp_op([NotNull] MathParser.Exp_opContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MathParser.exp_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExp_op([NotNull] MathParser.Exp_opContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MathParser.add_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAdd_op([NotNull] MathParser.Add_opContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MathParser.add_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAdd_op([NotNull] MathParser.Add_opContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MathParser.mult_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMult_op([NotNull] MathParser.Mult_opContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MathParser.mult_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMult_op([NotNull] MathParser.Mult_opContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MathParser.int_declaration_for_iteration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterInt_declaration_for_iteration([NotNull] MathParser.Int_declaration_for_iterationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MathParser.int_declaration_for_iteration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitInt_declaration_for_iteration([NotNull] MathParser.Int_declaration_for_iterationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MathParser.function"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunction([NotNull] MathParser.FunctionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MathParser.function"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunction([NotNull] MathParser.FunctionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MathParser.constant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterConstant([NotNull] MathParser.ConstantContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MathParser.constant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitConstant([NotNull] MathParser.ConstantContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MathParser.variable"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariable([NotNull] MathParser.VariableContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MathParser.variable"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariable([NotNull] MathParser.VariableContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="MathParser.sum_or_product"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSum_or_product([NotNull] MathParser.Sum_or_productContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MathParser.sum_or_product"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSum_or_product([NotNull] MathParser.Sum_or_productContext context);
}
