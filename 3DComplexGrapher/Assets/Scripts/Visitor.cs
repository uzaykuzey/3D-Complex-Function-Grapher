using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ComplexParser
{
    public class FunctionBuilderVisitor : MathParserBaseVisitor<ComplexFunction>
    {

        public static readonly double eulerMascheroni = 0.57721566490153286;
        public static readonly double apery = 1.2020569031595942;
        public static readonly double phi = (1 + Math.Sqrt(5)) / 2;
        public override ComplexFunction VisitAdd_expr(MathParser.Add_exprContext context)
        {
            if (context.add_op() != null && context.add_expr() == null)
            {
                ComplexFunction arg = Visit(context.mult_expr());
                switch (context.add_op().GetText())
                {
                    case "+":
                        return arg;
                    case "-":
                        return -arg;
                }
            }

            if (context.add_op() != null)
            {
                var left = Visit(context.add_expr());
                var right = Visit(context.mult_expr());

                return context.add_op().GetText() == "+"
                    ? left + right
                    : left - right;
            }



            return Visit(context.mult_expr());
        }

        public override ComplexFunction VisitMult_expr(MathParser.Mult_exprContext context)
        {
            if (context.mult_op() != null)
            {
                var left = Visit(context.mult_expr());
                var right = Visit(context.exp_expr());

                switch (context.mult_op().GetText())
                {
                    case "*":
                        return left*right;
                    case "/":
                        return left/right;
                }
            }
            try
            {
                var left = Visit(context.mult_expr());
                var right = Visit(context.exp_expr());
                return left * right;
            }
            catch { }

            return Visit(context.exp_expr());
        }

        public override ComplexFunction VisitExp_expr(MathParser.Exp_exprContext context)
        {
            if (context.exp_op() != null)
            {
                var baseFunc = Visit(context.final_expr());
                var exponentFunc = Visit(context.exp_expr());
                return new Power(baseFunc, exponentFunc);
            }

            return Visit(context.final_expr());
        }

        public override ComplexFunction VisitFinal_expr(MathParser.Final_exprContext context)
        {

            if (context.variable() != null)
            {
                return Visit(context.variable());
            }

            if (context.function() != null)
            {
                ComplexFunction arg = Visit(context.final_expr());
                switch (context.function().GetText())
                {
                    case "sin":
                        return new Sine(arg);
                    case "cos":
                        return new Cosine(arg);
                    case "ln":
                        return new NaturalLogarithm(arg);
                    case "exp":
                        return new Exponential(arg);
                    case "re":
                        return new Re(arg);
                    case "real":
                        return new Re(arg);
                    case "im":
                        return new Im(arg);
                    case "imag":
                        return new Im(arg);
                    case "abs":
                        return new Abs(arg);
                    case "arg":
                        return new Arg(arg);
                    case "tan":
                        return new Tangent(arg);
                    case "cot":
                        return new Cotangent(arg);
                    case "sec":
                        return new Secant(arg);
                    case "csc":
                        return new Cosine(arg);
                    case "cosec":
                        return new Cosine(arg);
                    case "arcsin":
                        return new Arcsine(arg);
                    case "arccos":
                        return Math.PI/2 - new Arcsine(arg);
                    case "arctan":
                        return new Arctangent(arg);
                    case "arccot":
                        return new Arccotangent(arg);
                    case "sqrt":
                        return new Sqrt(arg);
                    case "cbrt":
                        return new Cbrt(arg);
                    case "gamma":
                        return new Gamma(arg);
                    case "si":
                        return new SineIntegral(arg);
                    case "zeta":
                        return new RiemannZeta(arg);
                    case "w":
                        return new LambertW(arg);
                    case "lambertw":
                        return new LambertW(arg);
                    case "sign":
                        return new Sign(arg);
                    default:
                        throw new NotImplementedException($"Function '{context.function().GetText()}' is not implemented.");
                }
            }

            if (context.POW() != null)
            {
                ComplexFunction arg1 = Visit(context.add_expr(0));
                ComplexFunction arg2 = Visit(context.add_expr(1));
                return new Power(arg1, arg2);
            }

            if(context.LOG() != null)
            {
                if(context.add_expr(0)!=null)
                {
                    ComplexFunction arg1 = Visit(context.add_expr(0));
                    ComplexFunction arg2 = Visit(context.add_expr(1));
                    return new NaturalLogarithm(arg1) / new NaturalLogarithm(arg2);
                }
                else
                {
                    ComplexFunction arg=Visit(context.final_expr());
                    return new NaturalLogarithm(arg)/new Constant(ComplexNumber.Ln(10));
                }
                
            }

            if (context.BETA() != null)
            {
                ComplexFunction arg1 = Visit(context.add_expr(0));
                ComplexFunction arg2 = Visit(context.add_expr(1));
                return new Beta(arg1, arg2);
            }

            if (context.sum_or_product() != null)
            {
                ComplexFunction rule = Visit(context.add_expr(0));
                char iterativeVariable = context.ITERATIVE_VARIABLE().GetText()[1];
                int startingPoint = int.Parse(context.int_declaration_for_iteration(0).GetText().Replace(" ", ""));
                int endPoint = int.Parse(context.int_declaration_for_iteration(1).GetText().Replace(" ", ""));
                switch (context.sum_or_product().GetText())
                {
                    case "sum":
                        return new Sum(rule, iterativeVariable, startingPoint, endPoint);
                    case "product":
                        return new Product(rule, iterativeVariable, startingPoint, endPoint);
                    default:
                        throw new NotImplementedException($"Function '{context.sum_or_product().GetText()}' is not implemented.");
                }
            }

            if (context.constant() != null)
            {
                return Visit(context.constant());
            }

            if (context.LP() != null && context.RP() != null)
            {
                return Visit(context.add_expr(0));
            }

            if (context.ABSSYMBOL() != null)
            {
                return new Abs(Visit(context.add_expr(0)));
            }



            throw new Exception("Invalid Synthax");
        }

        public override ComplexFunction VisitConstant(MathParser.ConstantContext context)
        {
            if (double.TryParse(context.GetText(), out double value))
            {
                return new Constant(value);
            }

            return context.GetText() switch
            {
                "e" => Math.E,
                "pi" => Math.PI,
                "phi" => phi,
                "i" => ComplexNumber.I,
                "eulerm" => eulerMascheroni,
                "apery" => apery,
                _ => throw new ArgumentException("Unknown constant.")
            };
        }

        public override ComplexFunction VisitVariable(MathParser.VariableContext context)
        {
            switch(context.GetText()[0])
            {
                case 'x':
                    return new Variable(VariableTypes.X);
                case 'y':
                    return new Variable(VariableTypes.Y);
                case 'z':
                    return new Variable(VariableTypes.Z);
                case '$':
                    return new IterativeVariable(context.GetText()[1]);
            }
            throw new Exception("Unknown Variable");
        }
    }
}
