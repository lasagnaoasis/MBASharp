using Microsoft.Z3;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MBASharp {
    internal static class Z3Helper {
        private static bool _disposed = false;

        internal static string[] BinaryOperators = { "&", "|", "^" }; //AND, OR, XOR
        internal static string[] UnaryOperators = { "~" }; //NOT
        internal static string[] Variables = { "x", "y", "z", "t" };

        internal static char[] BinaryOperatorsAsChars = { '&', '|', '^' }; //AND, OR, XOR
        internal static char[] UnaryOperatorsAsChars = { '~' }; //NOT
        internal static char[] VariablesAsChars = { 'x', 'y', 'z', 't' };

        internal static char BitwiseNotOp = '~';
        internal static char BitwiseAndOp = '&';
        internal static char BitwiseXorOp = '^';
        internal static char BitwiseOrOp = '|';

        internal static char AdditionOp = '+';
        internal static char SubtractionOp = '-';
        internal static char MultiplicationOp = '*';
        internal static char DivisonOp = '/';

        internal static Context GlobalZ3Context;

        internal static Dictionary<string, BitVecExpr> Z3Variables;
        internal static Dictionary<int, BitVecExpr> Z3Numerals;

        internal static int Bits;

        internal static BitVecExpr Parse(string expression, int bits = 32, Context ctx = null) {
            if (ctx == null) {
                ctx = GlobalZ3Context = new Context(new Dictionary<string, string>() { { "proof", "true" } });
                Z3Variables = new Dictionary<string, BitVecExpr>() {
                    { "x", GlobalZ3Context.MkBVConst("x", (uint)bits) },
                    { "y", GlobalZ3Context.MkBVConst("y", (uint)bits) },
                    { "z", GlobalZ3Context.MkBVConst("z", (uint)bits) },
                    { "t", GlobalZ3Context.MkBVConst("t", (uint)bits) }
                };
                var numeralsAmount = 1024;
                Z3Numerals = new Dictionary<int, BitVecExpr>(numeralsAmount * 2);
                for (int i = (-numeralsAmount); i < numeralsAmount; i++) {
                    var a = Z3Numerals[i] = GlobalZ3Context.MkBV(i, (uint)bits);
                }
                Bits = bits;
            }

            expression = expression.Trim().Replace(" ", "");

            BitVecExpr result = null;
            if (expression.Contains("(") && expression.Contains(")")) {
                result = ParseExpressionWithParentheses(expression, ctx);
            } else {
                result = ParseSimpleExpression(expression, ctx);
            }

            return result;
        }

        private static BitVecExpr ParseExpressionWithParentheses(string expression, Context ctx) {
            Console.WriteLine($"Parsing: {expression}");
            Console.WriteLine();

            var variables = GetVariables(expression);
            foreach (var @var in variables)
                Console.WriteLine($"Var: {var.Key} ({var.Value})");

            var parts = SplitIntoParts(expression).OrderBy(p => p.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (var part in parts) {
                Console.WriteLine($"{part.Key} - {part.Value}");
            }

            var parsedParts = ParsePartsIntoBitVecExpressions(parts, ctx);

            return null;
        }

        private static BitVecExpr ParseSimpleExpression(string expression, Context ctx) {
            Console.WriteLine($"Parsing: {expression}");

            if (expression.Count(c => c == '(') > 1)
                throw new ArgumentException($"Parameter {nameof(expression)} should contain max. 1 Parentheses pair!");

            var strippedExpression = string.Empty;
            if (expression.Contains("(") && expression.Contains(")") && expression.IndexOf("(") == 0 && expression.IndexOf(")") == expression.Length - 1)
                strippedExpression = expression.Substring(1, expression.Length - 2);

            Console.WriteLine($"Parsing stripped: {strippedExpression}");
            Console.WriteLine();

            var variables = GetVariables(strippedExpression);
            foreach (var @var in variables)
                Console.WriteLine($"Var: {var.Key} ({var.Value})");

            var expressionMap = new Dictionary<Range, BitVecExpr>(8);
            var maxStrippedExpressionIndex = strippedExpression.Length - 1;
            for (int i = 0; i < strippedExpression.Length; i++) {
                var c = strippedExpression[i];
                if (c == BitwiseNotOp) { // ~
                    ExtractBitwiseNotOpSubExpression(ctx, strippedExpression, expressionMap, maxStrippedExpressionIndex, i);
                } else if (c == AdditionOp) { // +

                } else if (c == SubtractionOp) { // -

                } else if (c == MultiplicationOp) { // *

                } else if (c == DivisonOp) { // /

                } else if (c == BitwiseAndOp) { // &

                } else if (c == BitwiseXorOp) { // ^

                } else if (c == BitwiseOrOp) { // |

                } else if (Regex.Match(c.ToString(), @"\d").Success) {

                } else if (VariablesAsChars.Contains(c)) {

                } else {
                    throw new InvalidOperationException("Invalid char in expression!");
                }
            }

            Console.WriteLine($"Expressions: {string.Join(", ", expressionMap)}");

            Console.WriteLine();

            return null;
        }

        private static void ExtractBitwiseNotOpSubExpression(Context ctx, string strippedExpression, Dictionary<Range, BitVecExpr> expressionMap, int maxStrippedExpressionIndex, int charIndex) {
            int lookAheadIndex = charIndex + 1;
            var isDigit = false;
            var isVariable = false;
            var isPositiveOrNegativeDigitOrVariable = false;
            var lastIndex = -1;
            while (lookAheadIndex <= maxStrippedExpressionIndex) {
                var nextC = strippedExpression[lookAheadIndex];
                if (Regex.Match(nextC.ToString(), @"\w").Success) {
                    if (isDigit)
                        break;
                    isVariable = true;
                    lastIndex = lookAheadIndex;
                    lookAheadIndex++;
                } else if (Regex.Match(nextC.ToString(), @"\d").Success) {
                    if (isVariable)
                        break;
                    isDigit = true;
                    lastIndex = lookAheadIndex;
                    lookAheadIndex++;
                } else if (nextC == SubtractionOp || nextC == AdditionOp) {
                    if (isDigit || isVariable)
                        break;
                    isPositiveOrNegativeDigitOrVariable = true;
                    lastIndex = lookAheadIndex;
                    lookAheadIndex++;
                } else {
                    break;
                }
            }
            var bitwiseNotRange = new Range(charIndex, lastIndex);
            var extractedExpression = strippedExpression.Substring(charIndex, lastIndex - charIndex + 1);
            var extractedVariableOrNumber = strippedExpression.Substring(charIndex + 1, lastIndex - charIndex);
            if (isDigit) {
                var parsedNumber = int.Parse(extractedVariableOrNumber);
                BitVecExpr number = null;
                if (Z3Numerals.ContainsKey(parsedNumber))
                    number = Z3Numerals[parsedNumber];
                else
                    number = ctx.MkBV(parsedNumber, (uint)Bits);
                expressionMap[bitwiseNotRange] = ctx.MkBVNot(number);
            } else if (isVariable) {
                BitVecExpr variable = null;
                if (isPositiveOrNegativeDigitOrVariable && extractedVariableOrNumber[0] == SubtractionOp)
                    variable = ctx.MkBVNeg(Z3Variables[extractedVariableOrNumber.Substring(1)]);
                else if (isPositiveOrNegativeDigitOrVariable && extractedVariableOrNumber[0] == AdditionOp)
                    variable = Z3Variables[extractedVariableOrNumber.Substring(1)];
                else
                    variable = Z3Variables[extractedVariableOrNumber];
                expressionMap[bitwiseNotRange] = ctx.MkBVNot(variable);
            } else {
                throw new InvalidOperationException();
            }
        }

        private static Dictionary<string, BitVecExpr> ParsePartsIntoBitVecExpressions(Dictionary<string, string> parts, Context ctx) {
            var result = new Dictionary<string, BitVecExpr>(parts.Count);

            var maxIndent = parts.Max(p => p.Key.Split('_').Length - 1);
            var indentCounter = maxIndent;
            for (int i = maxIndent; i > 0; i--) {
                foreach (var part in parts) {
                    var partName = part.Key;
                    var expression = part.Value;

                    var splitPartName = partName.Split('_');
                    var indent = splitPartName.Length - 1;
                    if (indent != i)
                        continue;

                    if (expression.Count(e => e == '(') == 1) {
                        var simpleExpression = ParseSimpleExpression(expression, ctx);
                        result[partName] = simpleExpression;
                    }
                }
            }

            return result;
        }

        private static Dictionary<string, string> SplitIntoParts(string expression, string partName = "") {
            var parts = new Dictionary<string, string>(8);

            var openParenthesisCounter = 0;
            var closedParenthesisCounter = 0;
            var indexOfFirstOpenParenthesis = -1;
            var indexOfLastClosedParenthesis = -1;
            var partCounter = 0;

            for (int i = 0; i < expression.Length; i++) {
                var c = expression[i];
                if (c == '(') {
                    if (openParenthesisCounter == 0)
                        indexOfFirstOpenParenthesis = i;
                    openParenthesisCounter++;
                } else if (c == ')') {
                    closedParenthesisCounter++;
                    if (closedParenthesisCounter == openParenthesisCounter)
                        indexOfLastClosedParenthesis = i;
                }

                if (openParenthesisCounter == closedParenthesisCounter && (openParenthesisCounter > 0 && closedParenthesisCounter > 0)) {
                    var part = expression.Substring(indexOfFirstOpenParenthesis, indexOfLastClosedParenthesis - indexOfFirstOpenParenthesis + 1);
                    var key = string.Empty;
                    if (string.IsNullOrEmpty(partName)) {
                        key = $"part_{partCounter++}";
                        parts[key] = part;
                    } else {
                        key = $"{partName}_{partCounter++}";
                        parts[key] = part;
                    }

                    openParenthesisCounter = 0;
                    closedParenthesisCounter = 0;
                    indexOfFirstOpenParenthesis = -1;
                    indexOfLastClosedParenthesis = -1;
                }
            }

            var tempParts = new Dictionary<string, string>(4);
            foreach (var part in parts) {
                if (part.Value.Contains("(") && part.Value.Contains(")")) {
                    var partValue = part.Value;
                    var partValueStripped = partValue.Substring(1, partValue.Length - 2);
                    var additionalParts = SplitIntoParts(partValueStripped, part.Key);
                    foreach (var additionalPart in additionalParts)
                        tempParts[additionalPart.Key] = additionalPart.Value;
                }
            }

            foreach (var tempPart in tempParts)
                parts[tempPart.Key] = tempPart.Value;

            return parts;
        }

        private static Dictionary<string, int> GetVariables(string expression) {
            var variables = new Dictionary<string, int>(4);
            foreach (var @char in expression) {
                var @var = @char.ToString();
                if (VariablesAsChars.Contains(@char)) {
                    if (variables.ContainsKey(@var))
                        variables[@var]++;
                    else
                        variables[@var] = 1;
                }
            }
            return variables;
        }

        internal static void Z3Example() {
            using var z3Context = new Context(new Dictionary<string, string>() { { "proof", "true" } });
            var x = z3Context.MkBVConst("x", 2);
            var y = z3Context.MkBVConst("y", 2);
            var one = z3Context.MkBV(1, 2);

            var exprA = z3Context.MkBVAdd(x, y);
            var exprB = z3Context.MkBVMul(one, exprA);
            var exprC = z3Context.MkBVAdd(x, y);
            var exprD = z3Context.MkBVMul(one, exprC);

            using var z3Solver = z3Context.MkSolver();
            var z3Params = z3Context.MkParams();
            z3Params.Add("mbqi", false);
            z3Solver.Parameters = z3Params;

            var exprAEqExprB = z3Context.MkEq(exprA, exprD);

            z3Solver.Assert(z3Context.MkNot(exprAEqExprB));

            var status = z3Solver.Check();
            Console.WriteLine(status);
            if (status == Status.UNSATISFIABLE)
                Console.WriteLine("OK");
            if (z3Solver.Proof != null)
                Console.WriteLine($"Proof: {z3Solver.Proof}");
        }

        internal static void Dispose() {
            if (_disposed)
                return;

            GlobalZ3Context?.Dispose();

            _disposed = true;
        }
    }
}
