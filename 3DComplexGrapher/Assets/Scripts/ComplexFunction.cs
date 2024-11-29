using Antlr4.Runtime.Atn;
using ComplexParser;
using System;
using System.Collections.Generic;
using UnityEngine;

public struct ComplexNumber
{
    public static readonly ComplexNumber I = new ComplexNumber(0,1);
    public readonly double real, imaginary;
    public static readonly double[] lanczosCoefficients =
    {
        676.5203681218851,
        -1259.1392167224028,
        771.32342877765313,
        -176.61502916214059,
        12.507343278686905,
        -0.13857109526572012,
        9.9843695780195716e-6,
        1.5056327351493116e-7
    };
    public readonly double Abs()
    {
        return Math.Sqrt(real*real + imaginary*imaginary);
    }

    public readonly double Arg()
    {
        if(imaginary==0)
        {
            return real >= 0 ? 0 : Math.PI;
        }
        return 2 * Math.Atan(imaginary / (real + Abs()));
    }
    public readonly ComplexNumber Conj()
    {
        return new ComplexNumber(real, -imaginary);
    }
    public ComplexNumber(double real, double imaginary)
    {
        this.real = real;
        this.imaginary = imaginary;
    }

    public static ComplexNumber operator +(ComplexNumber z1, ComplexNumber z2)
    {
        return new ComplexNumber(z1.real+z2.real, z1.imaginary+z2.imaginary);
    }

    public static ComplexNumber operator -(ComplexNumber z1, ComplexNumber z2)
    {
        return new ComplexNumber(z1.real - z2.real, z1.imaginary - z2.imaginary);
    }

    public static ComplexNumber operator -(ComplexNumber z)
    {
        return new ComplexNumber(-z.real, -z.imaginary);
    }

    public static ComplexNumber operator *(ComplexNumber z1, ComplexNumber z2)
    {
        return new ComplexNumber(z1.real*z2.real - z1.imaginary*z2.imaginary, z1.real*z2.imaginary + z1.imaginary*z2.real);
    }

    public static ComplexNumber operator /(ComplexNumber z1, ComplexNumber z2)
    {
        return (z1 * z2.Conj()) * (1 / (z2.Abs() * z2.Abs()));
    }

    public static bool operator ==(ComplexNumber z1, ComplexNumber z2)
    {
        return z1.real==z2.real && z1.imaginary==z2.imaginary;
    }

    public static bool operator !=(ComplexNumber z1, ComplexNumber z2)
    {
        return !(z1==z2);
    }

    public override readonly bool Equals(object o)
    {
        return (o is ComplexNumber z) && this == z;
    }


    public static implicit operator ComplexNumber(double value)
    {
        return new ComplexNumber(value, 0);
    }


    public override string ToString()
    {
        return "( " + Round(real) + " ) + ( " + Round(imaginary) + " * i )";
    }

    public static float Round(double value)
    {
        return (float)(Math.Round(value * Math.Pow(10, 13)) * Math.Pow(10, -13));
    }

    public override int GetHashCode()
    {
        return (int) (real+imaginary*29);
    }

    public static ComplexNumber Exp(ComplexNumber z)
    {
        double a = z.real;
        double b = z.imaginary;
        return Math.Exp(a) * (Math.Cos(b) + Math.Sin(b) * I);
    }

    public static ComplexNumber Sin(ComplexNumber z)
    {
        double a = z.real;
        double b = z.imaginary;
        return Math.Cosh(b)*Math.Sin(a) + I * Math.Cos(a) * Math.Sinh(b);
    }

    public static ComplexNumber Cos(ComplexNumber z)
    {
        double a = z.real;
        double b = z.imaginary;
        return Math.Cos(a) * Math.Cosh(b) - I * Math.Sin(a) * Math.Sinh(b);
    }

    public static ComplexNumber Ln(ComplexNumber z)
    {
        return Math.Log(z.Abs()) + I * z.Arg();
    }

    public static ComplexNumber Pow(ComplexNumber z1, ComplexNumber z2)
    {
        return Exp(Ln(z1) * z2);
    }

    public static ComplexNumber Asin(ComplexNumber z)
    {
        if(z==1)
        {
            return Math.PI/2;
        }
        else if(z==-1)
        {
            return -Math.PI/2;
        }
        return -I * Ln(Pow(1 - z * z, 0.5) + I * z);
    }

    public static ComplexNumber Atan(ComplexNumber z)
    {
        return 0.5 * I * Ln((1 - I * z)/(1 + I * z));
    }

    public static ComplexNumber Acot(ComplexNumber z)
    {
        return -0.5 * I * Ln((z + I) / (z - I));
    }

    public static ComplexNumber Gamma(ComplexNumber z)
    {
        // Reflection formula for negative real part
        if (z.real < 0.5)
        {
            return Math.PI / (Sin(Math.PI * z) * Gamma(1 - z));
        }

        // Shift z to be z - 1
        z -= 1;
        ComplexNumber x = new ComplexNumber(0.99999999999980993, 0);

        for (int i = 0; i < lanczosCoefficients.Length; i++)
        {
            x += lanczosCoefficients[i] / (z + i + 1);
        }

        ComplexNumber t = z + lanczosCoefficients.Length - 0.5;

        return Pow(2 * Math.PI, 0.5) * Pow(t, z + 0.5) * Exp(-t) * x;
    }

    public static ComplexNumber Si(ComplexNumber z)
    {
        if (Math.Abs(z.real) < 20)
        {
            ComplexNumber result = 0;
            ComplexNumber nextTermMultiplier = z;
            for (int i = 0; i < 100; i++)
            {
                result += nextTermMultiplier / (2 * i + 1);
                nextTermMultiplier *= -z * z / ((2 * (i + 1) + 1) * (2 * (i + 1)));
            }
            return result;
        }
        else if (z.real >= 20)
        {
            ComplexNumber result = 0;

            ComplexNumber nextTermMultiplier = 1;
            ComplexNumber subResult = 0;
            for (int i = 0; i < 20; i++)
            {
                subResult += nextTermMultiplier;
                nextTermMultiplier *= -(2 * (i + 1)) * (2 * (i + 1) - 1) / (z * z);
            }
            result -= Cos(z) / z * subResult;

            nextTermMultiplier = 1 / z;
            subResult = 0;
            for (int i = 0; i < 20; i++)
            {
                subResult += nextTermMultiplier;
                nextTermMultiplier *= -(2 * (i + 1) + 1) * (2 * (i + 1)) / (z * z);
            }
            result -= Sin(z) / z * subResult;
            return Math.PI / 2 + result;
        }
        return -Si(-z);
    }

    //This is a helper function to calculate the Riemann-Zeta function. For its purposes, implementing z.real > 0 is enough.
    private static ComplexNumber DirichletEta(ComplexNumber z)
    {
        if(z.real<0)
        {
            throw new ArgumentException("z.real must be positive.");
        }
        ComplexNumber result = 0;
        for(int i=1;i<=100;i++)
        {
            result += Pow(-1, i - 1) / Pow(i, z);
        }
        return result;
    }

    public static ComplexNumber RiemannZeta(ComplexNumber z)
    {
        if(z==0)
        {
            return -0.5;
        }
        if(z.real<0.5)
        {
            return Pow(2, z) * Pow(Math.PI, z - 1) * Sin(Math.PI * z / 2.0) * Gamma(1 - z) * RiemannZeta(1 - z);
        }
        return DirichletEta(z) / (1 - Pow(2, 1 - z));
    }

    public static ComplexNumber Beta(ComplexNumber z1, ComplexNumber z2)
    {
        return (Gamma(z1)*Gamma(z2))/Gamma(z1+z2);
    }


    public static ComplexNumber LambertW(ComplexNumber z)
    {
        ComplexNumber w = z.Abs() > 1 ? Ln(z) : z; // Reasonable initial guess

        for (int i = 0; i < 1000; i++)
        {
            // Compute f(w), f'(w), and f''(w)
            ComplexNumber ew = Exp(w);         // e^w
            ComplexNumber f = w * ew - z;     // f(w) = w * e^w - z
            ComplexNumber fp = ew * (1 + w);  // f'(w) = e^w * (1 + w)
            ComplexNumber fpp = ew * (2 + w); // f''(w) = e^w * (2 + w)

            // Halley's method update
            ComplexNumber denominator = fp - f * fpp / (2 * fp);
            ComplexNumber delta = f / denominator;
            w -= delta;

            // Convergence check
            if (delta.Abs() < 0.01)
                return w;
        }

        // If the method didn't converge, throw an exception
        return w;

    }

}
public abstract class ComplexFunction
{
    public abstract ComplexNumber Calculate(ComplexNumber z);
    public abstract bool Defined(ComplexNumber z);

    public static implicit operator ComplexFunction(ComplexNumber value)
    {
        return new Constant(value);
    }

    public static implicit operator ComplexFunction(double value)
    {
        return new Constant(value);
    }

    public static implicit operator ComplexFunction(VariableTypes type)
    {
        return new Variable(type);
    }

    public static ComplexFunction operator +(ComplexFunction f1, ComplexFunction f2)
    {
        return new Addition(f1, f2);
    }

    public static ComplexFunction operator -(ComplexFunction f1, ComplexFunction f2)
    {
        return new Substraction(f1, f2);
    }

    public static ComplexFunction operator -(ComplexFunction f)
    {
        return new Multiplication(-1, f);
    }

    public static ComplexFunction operator *(ComplexFunction f1, ComplexFunction f2)
    {
        return new Multiplication(f1, f2);
    }

    public static ComplexFunction operator /(ComplexFunction f1, ComplexFunction f2)
    {
        return new Division(f1, f2);
    }

    public ComplexNumber CalculateApproximately(ComplexNumber z)
    {
        if(!Defined(z))
        {
            System.Random rand= new System.Random();
            CalculateApproximately(new ComplexNumber(z.real + rand.NextDouble()*0.1 - 0.05, z.imaginary + rand.NextDouble() * 0.1 - 0.05));
        }
        return Calculate(z);
    }
}

public class Constant : ComplexFunction
{
    ComplexNumber value;
    public Constant(ComplexNumber value)
    {
        this.value = value;
    }
    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return value;
    }

    public override bool Defined(ComplexNumber z)
    {
        return true;
    }

}

public enum VariableTypes
{
    X, Y, Z
}

public class Variable : ComplexFunction
{
    readonly VariableTypes type;

    public Variable(VariableTypes type)
    {
        this.type = type;
    }
    public override ComplexNumber Calculate(ComplexNumber z)
    {
        switch (type)
        {
            case VariableTypes.X:
                return z.real;
            case VariableTypes.Y:
                return z.imaginary;
            default:
                return z;
        }
    }

    public override bool Defined(ComplexNumber z)
    {
        return true;
    }
}

public class Addition : ComplexFunction
{
    readonly ComplexFunction function1;
    readonly ComplexFunction function2;

    public Addition(ComplexFunction function1, ComplexFunction function2)
    {
        this.function1 = function1;
        this.function2 = function2;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return function1.Calculate(z)+function2.Calculate(z);
    }

    public override bool Defined(ComplexNumber z)
    {
        return function1.Defined(z) && function2.Defined(z);
    }
}

public class Substraction : ComplexFunction
{
    readonly ComplexFunction function1;
    readonly ComplexFunction function2;

    public Substraction(ComplexFunction function1, ComplexFunction function2)
    {
        this.function1 = function1;
        this.function2 = function2;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return function1.Calculate(z) - function2.Calculate(z);
    }

    public override bool Defined(ComplexNumber z)
    {
        return function1.Defined(z) && function2.Defined(z);
    }
}

public class Multiplication : ComplexFunction
{
    readonly ComplexFunction function1;
    readonly ComplexFunction function2;

    public Multiplication(ComplexFunction function1, ComplexFunction function2)
    {
        this.function1 = function1;
        this.function2 = function2;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return function1.Calculate(z) * function2.Calculate(z);
    }

    public override bool Defined(ComplexNumber z)
    {
        return function1.Defined(z) && function2.Defined(z);
    }
}

public class Division : ComplexFunction
{
    readonly ComplexFunction function1;
    readonly ComplexFunction function2;

    public Division(ComplexFunction function1, ComplexFunction function2)
    {
        this.function1 = function1;
        this.function2 = function2;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return function1.Calculate(z) / function2.Calculate(z);
    }

    public override bool Defined(ComplexNumber z)
    {
        return function1.Defined(z) && function2.Defined(z) && function2.Calculate(z) != 0;
    }
}

public class Re : ComplexFunction
{
    readonly ComplexFunction function;

    public Re(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return function.Calculate(z).real;
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z);
    }
}

public class Im : ComplexFunction
{
    readonly ComplexFunction function;

    public Im(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return function.Calculate(z).imaginary;
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z);
    }
}

public class Arg : ComplexFunction
{
    readonly ComplexFunction function;

    public Arg(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return function.Calculate(z).Arg();
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z);
    }
}

public class Abs : ComplexFunction
{
    readonly ComplexFunction function;

    public Abs(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return function.Calculate(z).Abs();
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z);
    }
}

public class Sqrt : ComplexFunction
{
    readonly ComplexFunction function;

    public Sqrt(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        if(z==0)
        {
            return 0;
        }
        return ComplexNumber.Pow(function.Calculate(z), 0.5);
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z);
    }
}

public class Cbrt : ComplexFunction
{
    readonly ComplexFunction function;

    public Cbrt(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        if (z == 0)
        {
            return 0;
        }
        return ComplexNumber.Pow(function.Calculate(z), 1.0/3.0);
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z);
    }
}

public class Sine : ComplexFunction
{
    readonly ComplexFunction function;

    public Sine(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return ComplexNumber.Sin(function.Calculate(z));
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z);
    }
}

public class Cosine : ComplexFunction
{
    readonly ComplexFunction function;

    public Cosine(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return ComplexNumber.Cos(function.Calculate(z));
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z);
    }
}

public class Tangent : ComplexFunction
{
    readonly ComplexFunction function;

    public Tangent(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        ComplexNumber value = function.Calculate(z);
        return ComplexNumber.Sin(value) / ComplexNumber.Cos(value);
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z) && ComplexNumber.Cos(function.Calculate(z)) != 0;
    }
}

public class Cotangent : ComplexFunction
{
    readonly ComplexFunction function;

    public Cotangent(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        ComplexNumber value = function.Calculate(z);
        return ComplexNumber.Cos(value) / ComplexNumber.Sin(value);
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z) && ComplexNumber.Sin(function.Calculate(z)) != 0;
    }
}

public class Secant : ComplexFunction
{
    readonly ComplexFunction function;

    public Secant(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return 1 / ComplexNumber.Cos(function.Calculate(z));
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z) && ComplexNumber.Cos(function.Calculate(z)) != 0;
    }
}

public class Cosecant : ComplexFunction
{
    readonly ComplexFunction function;

    public Cosecant(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return 1 / ComplexNumber.Sin(function.Calculate(z));
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z) && ComplexNumber.Sin(function.Calculate(z)) != 0;
    }
}

public class Exponential : ComplexFunction
{
    readonly ComplexFunction function;

    public Exponential(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return ComplexNumber.Exp(function.Calculate(z));
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z);
    }
}

public class NaturalLogarithm : ComplexFunction
{
    readonly ComplexFunction function;

    public NaturalLogarithm(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return ComplexNumber.Ln(function.Calculate(z));
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z) && function.Calculate(z) != 0;
    }
}

public class Power : ComplexFunction
{
    readonly ComplexFunction function1;
    readonly ComplexFunction function2;

    public Power(ComplexFunction function1, ComplexFunction function2)
    {
        this.function1 = function1;
        this.function2 = function2;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return ComplexNumber.Pow(function1.Calculate(z), function2.Calculate(z));
    }

    public override bool Defined(ComplexNumber z)
    {
        return function1.Defined(z) && function2.Defined(z) && function1.Calculate(z) != 0;
    }
}

public class Arcsine : ComplexFunction
{
    readonly ComplexFunction function;

    public Arcsine(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return ComplexNumber.Asin(function.Calculate(z));
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z);
    }
}

public class Arctangent : ComplexFunction
{
    readonly ComplexFunction function;

    public Arctangent(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return ComplexNumber.Atan(function.Calculate(z));
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z) && function.Calculate(z) != ComplexNumber.I && function.Calculate(z) != -ComplexNumber.I;
    }
}

public class Arccotangent : ComplexFunction
{
    readonly ComplexFunction function;

    public Arccotangent(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return ComplexNumber.Acot(function.Calculate(z));
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z) && function.Calculate(z) != ComplexNumber.I && function.Calculate(z) != -ComplexNumber.I;
    }
}


public class Gamma : ComplexFunction
{
    readonly ComplexFunction function;

    public Gamma(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return ComplexNumber.Gamma(function.Calculate(z));
    }

    public override bool Defined(ComplexNumber z)
    {
        if(!function.Defined(z))
        {
            return false;
        }
        ComplexNumber value=function.Calculate(z);
        return !(value.real<0.1 && Math.Abs(value.real - Math.Round(value.real))<0.0001 && Math.Abs(value.imaginary) < 0.0001);
    }
}

public class SineIntegral : ComplexFunction
{
    readonly ComplexFunction function;

    public SineIntegral(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return ComplexNumber.Si(function.Calculate(z));
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z);
    }
}

public class RiemannZeta : ComplexFunction
{
    readonly ComplexFunction function;

    public RiemannZeta(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return ComplexNumber.RiemannZeta(function.Calculate(z));
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z) && function.Calculate(z) != 1;
    }
}

public class Beta : ComplexFunction
{
    readonly ComplexFunction function1;
    readonly ComplexFunction function2;

    public Beta(ComplexFunction function1, ComplexFunction function2)
    {
        this.function1 = function1;
        this.function2 = function2;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return ComplexNumber.Beta(function1.Calculate(z), function2.Calculate(z));
    }

    public override bool Defined(ComplexNumber z)
    {
        if(!function1.Defined(z) || !function2.Defined(z)) 
        {
            return false;
        }
        ComplexNumber value1 = function1.Calculate(z);
        ComplexNumber value2 = function2.Calculate(z);
        return !(value1.real < 0.1 && Math.Abs(value1.real - Math.Round(value1.real)) < 0.0001 && Math.Abs(value1.imaginary) < 0.0001) &&
                    !(value2.real < 0.1 && Math.Abs(value2.real - Math.Round(value2.real)) < 0.0001 && Math.Abs(value2.imaginary) < 0.0001);
    }
}


public class IterativeVariable : ComplexFunction
{
    readonly char name;
    public static Dictionary<char, int> iterativeValues = new Dictionary<char, int>();

    public IterativeVariable(char name)
    {
        this.name = name;
    }
    public override ComplexNumber Calculate(ComplexNumber z)
    {
        if(iterativeValues.ContainsKey(name))
        {
            return iterativeValues[name];
        }
        throw new Exception("Iterative Variable used outside of sum/product notations.");
    }

    public override bool Defined(ComplexNumber z)
    {
        return true;
    }
}

public class Product : ComplexFunction
{
    readonly ComplexFunction rule;
    readonly char name;
    readonly int startingIndex;
    readonly int endIndex;

    public Product(ComplexFunction rule, char name, int startingIndex, int endIndex)
    {
        this.rule = rule;
        this.name = name;
        this.startingIndex = startingIndex;
        this.endIndex = endIndex;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        ComplexNumber result = 1;
        for(IterativeVariable.iterativeValues.Add(name, startingIndex); IterativeVariable.iterativeValues[name] <= endIndex; IterativeVariable.iterativeValues[name]++)
        {
            result *= rule.Calculate(z);
        }
        IterativeVariable.iterativeValues.Remove(name);
        return result;
    }

    public override bool Defined(ComplexNumber z)
    {
        for (IterativeVariable.iterativeValues.Add(name, startingIndex); IterativeVariable.iterativeValues[name] <= endIndex; IterativeVariable.iterativeValues[name]++)
        {
            if(!rule.Defined(z))
            {
                IterativeVariable.iterativeValues.Remove(name);
                return false;
            }
        }
        IterativeVariable.iterativeValues.Remove(name);
        return true;
    }
}

public class Sum : ComplexFunction
{
    readonly ComplexFunction rule;
    readonly char name;
    readonly int startingIndex;
    readonly int endIndex;

    public Sum(ComplexFunction rule, char name, int startingIndex, int endIndex)
    {
        this.rule = rule;
        this.name = name;
        this.startingIndex = startingIndex;
        this.endIndex = endIndex;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        ComplexNumber result = 0;
        for (IterativeVariable.iterativeValues.Add(name, startingIndex); IterativeVariable.iterativeValues[name] <= endIndex; IterativeVariable.iterativeValues[name]++)
        {
            result += rule.Calculate(z);
        }
        IterativeVariable.iterativeValues.Remove(name);
        return result;
    }

    public override bool Defined(ComplexNumber z)
    {
        for (IterativeVariable.iterativeValues.Add(name, startingIndex); IterativeVariable.iterativeValues[name] <= endIndex; IterativeVariable.iterativeValues[name]++)
        {
            if (!rule.Defined(z))
            {
                IterativeVariable.iterativeValues.Remove(name);
                return false;
            }
        }
        IterativeVariable.iterativeValues.Remove(name);
        return true;
    }
}

public class LambertW : ComplexFunction
{
    readonly ComplexFunction function;

    public LambertW(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        return ComplexNumber.LambertW(function.Calculate(z));
    }

    public override bool Defined(ComplexNumber z)
    {
        if(!function.Defined(z))
        {
            return false;
        }
        ComplexNumber value=function.Calculate(z);
        
        return value.real > -1 / Math.E || (Math.Abs(value.Arg()-Math.PI)>0.01 && Math.Abs(value.Arg() + Math.PI) > 0.01);
    }
}

public class Sign : ComplexFunction
{
    readonly ComplexFunction function;

    public Sign(ComplexFunction function)
    {
        this.function = function;
    }

    public override ComplexNumber Calculate(ComplexNumber z)
    {
        ComplexNumber value = function.Calculate(z);
        if(value == 0)
        {
            return 0;
        }
        return ComplexNumber.Exp(ComplexNumber.I * value.Arg());
    }

    public override bool Defined(ComplexNumber z)
    {
        return function.Defined(z);
    }
}