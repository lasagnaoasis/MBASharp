using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.Z3;

namespace MBASharp {
    internal static class Utils {
        internal static string[] BinaryOperators = { "&", "|", "^" }; //AND, OR, XOR
        internal static string[] UnaryOperators = { "~" }; //NOT
        internal static string[] Variables = { "x", "y", "z", "t" };

        internal static char[] BinaryOperatorsAsChars = { '&', '|', '^' }; //AND, OR, XOR
        internal static char[] UnaryOperatorsAsChars = { '~' }; //NOT
        internal static char[] VariablesAsChars = { 'x', 'y', 'z', 't' };

        private static Random _random = new Random();

        internal static string Postfix(string expression) {
            var expressionResultBuilder = new StringBuilder(32);
            var operatorList = new List<char>(32);
            for (int i = 0; i < expression.Length; i++) {
                var @char = expression[i];
                if (@char == '(')
                    operatorList.Add(@char);
                else if (BinaryOperatorsAsChars.Contains(@char))
                    operatorList.Add(@char);
                else if (UnaryOperatorsAsChars.Contains(@char))
                    operatorList.Add(@char);
                else if (@char == ')') {
                    while (operatorList is { Count: > 0 } && operatorList[^1] != '(') {
                        expressionResultBuilder.Append(operatorList[^1]);
                        operatorList.RemoveAt(operatorList.Count - 1);
                    }
                    if (operatorList is { Count: > 0 } && operatorList[^1] != '(') {
                        throw new InvalidOperationException("operatorList is { Count: > 0 } && operatorList[^1] != '('");
                    }
                    operatorList.RemoveAt(operatorList.Count - 1);
                    while (operatorList is { Count: > 0 } && operatorList[^1] == '~') {
                        expressionResultBuilder.Append(operatorList[^1]);
                        operatorList.RemoveAt(operatorList.Count - 1);
                    }
                } else {
                    expressionResultBuilder.Append(@char);
                    while (operatorList is { Count: > 0 } && UnaryOperatorsAsChars.Contains(operatorList[^1])) {
                        expressionResultBuilder.Append(operatorList[^1]);
                        operatorList.RemoveAt(operatorList.Count - 1);
                    }
                }
            }

            if (operatorList.Count > 1)
                throw new InvalidOperationException("operatorList.Count > 1");
            else if (operatorList.Count == 1)
                expressionResultBuilder.Append(operatorList[0]);

            return expressionResultBuilder.ToString();
        }

        //internal static byte[] CalculatePostfix(string expression, int variables = 0) {
        //    byte[] resultBytes = null;
        //    byte[] x = null;
        //    byte[] y = null;
        //    byte[] z = null;
        //    byte[] t = null;
        //    switch (variables) {
        //        case 1:
        //            x = new byte[] { 0, 1 };
        //            break;
        //        case 2:
        //            y = new byte[] { 0, 1, 0, 1 };
        //            x = new byte[] { 0, 0, 1, 1 };
        //            break;
        //        case 3:
        //            z = new byte[] { 0, 0, 0, 0, 1, 1, 1, 1 };
        //            y = new byte[] { 0, 1, 0, 1, 0, 1, 0, 1 };
        //            x = new byte[] { 0, 0, 1, 1, 0, 0, 1, 1 };
        //            break;
        //        case 4:
        //            t = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 };
        //            z = new byte[] { 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1 };
        //            y = new byte[] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };
        //            x = new byte[] { 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1 };
        //            break;
        //        default:
        //            throw new InvalidOperationException("Incorrect number of variables");
        //    }
        //    //var stackList = new List<char>();
        //    //for (int i = 0; i < expression.Length; i++) {
        //    //    var @char = expression[i];
        //    //    if (VariablesAsChars.Contains(@char))
        //    //        stackList.Insert(0, @char);
        //    //    else if (UnaryOperatorsAsChars.Contains(@char)) {

        //    //    } else if (BinaryOperatorsAsChars.Contains(@char)) {

        //    //    }
        //    //}
        //    var stack = new Stack<object>(32);
        //    for (int i = 0; i < expression.Length; i++) {
        //        var @char = expression[i];
        //        if (VariablesAsChars.Contains(@char)) {
        //            switch (@char) {
        //                case 'x':
        //                    stack.Push(x);
        //                    break;
        //                case 'y':
        //                    stack.Push(y!);
        //                    break;
        //                case 'z':
        //                    stack.Push(z!);
        //                    break;
        //                case 't':
        //                    stack.Push(t!);
        //                    break;
        //            }
        //        } else if (UnaryOperatorsAsChars.Contains(@char)) {
        //            var stackObject = stack.Pop();
        //            if (stackObject is byte[] bytes) {
        //                var inverted = Numpy.Invert(bytes);
        //                var moduloed = Numpy.Modulo(inverted, 2);
        //                stack.Push(moduloed);
        //            }
        //        } else if (BinaryOperatorsAsChars.Contains(@char)) {
        //            var stackObjectA = stack.Pop();
        //            var stackObjectB = stack.Pop();
        //            byte[] result = null;
        //            if (stackObjectA is byte[] bytesA && stackObjectB is byte[] bytesB) {
        //                switch (@char) {
        //                    case '&':
        //                        result = Numpy.BitwiseAnd(bytesA, bytesB);
        //                        stack.Push(result);
        //                        break;
        //                    case '|':
        //                        result = Numpy.BitwiseOr(bytesA, bytesB);
        //                        stack.Push(result);
        //                        break;
        //                    case '^':
        //                        result = Numpy.BitwiseXor(bytesA, bytesB);
        //                        stack.Push(result);
        //                        break;
        //                }
        //            }
        //        }
        //    }

        //    if (stack.Count > 1)
        //        throw new InvalidOperationException("stack.Count > 1");
        //    else {
        //        var result = stack.Peek();
        //        if (result is byte[] bytes) {
        //            resultBytes = bytes;
        //        }
        //    }

        //    return resultBytes!;
        //}

        internal static int[] CalculatePostfix(string expression, int variables = 0) {
            int[] resultBytes = null;
            int[] x = null;
            int[] y = null;
            int[] z = null;
            int[] t = null;
            switch (variables) {
                case 1:
                    x = new int[] { 0, 1 };
                    break;
                case 2:
                    y = new int[] { 0, 1, 0, 1 };
                    x = new int[] { 0, 0, 1, 1 };
                    break;
                case 3:
                    z = new int[] { 0, 0, 0, 0, 1, 1, 1, 1 };
                    y = new int[] { 0, 1, 0, 1, 0, 1, 0, 1 };
                    x = new int[] { 0, 0, 1, 1, 0, 0, 1, 1 };
                    break;
                case 4:
                    t = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 };
                    z = new int[] { 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1 };
                    y = new int[] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };
                    x = new int[] { 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1 };
                    break;
                default:
                    throw new InvalidOperationException("Incorrect number of variables");
            }
            //var stackList = new List<char>();
            //for (int i = 0; i < expression.Length; i++) {
            //    var @char = expression[i];
            //    if (VariablesAsChars.Contains(@char))
            //        stackList.Insert(0, @char);
            //    else if (UnaryOperatorsAsChars.Contains(@char)) {

            //    } else if (BinaryOperatorsAsChars.Contains(@char)) {

            //    }
            //}
            var stack = new Stack<object>(32);
            for (int i = 0; i < expression.Length; i++) {
                var @char = expression[i];
                if (VariablesAsChars.Contains(@char)) {
                    switch (@char) {
                        case 'x':
                            stack.Push(x);
                            //Console.WriteLine(string.Join(", ", x));
                            break;
                        case 'y':
                            stack.Push(y!);
                            //Console.WriteLine(string.Join(", ", y!));
                            break;
                        case 'z':
                            stack.Push(z!);
                            break;
                        case 't':
                            stack.Push(t!);
                            break;
                    }
                } else if (UnaryOperatorsAsChars.Contains(@char)) {
                    var stackObject = stack.Pop();
                    if (stackObject is int[] bytes) {
                        var inverted = Numpy.Invert(bytes);
                        var moduloed = Numpy.Modulo(inverted, 2);
                        stack.Push(moduloed);
                    }
                } else if (BinaryOperatorsAsChars.Contains(@char)) {
                    var stackObjectA = stack.Pop();
                    var stackObjectB = stack.Pop();
                    int[] result = null;
                    if (stackObjectA is int[] bytesA && stackObjectB is int[] bytesB) {
                        switch (@char) {
                            case '&':
                                result = Numpy.BitwiseAnd(bytesA, bytesB);
                                stack.Push(result);
                                break;
                            case '|':
                                result = Numpy.BitwiseOr(bytesA, bytesB);
                                stack.Push(result);
                                break;
                            case '^':
                                result = Numpy.BitwiseXor(bytesA, bytesB);
                                stack.Push(result);
                                break;
                        }
                    }
                }
            }

            if (stack.Count > 1)
                throw new InvalidOperationException("stack.Count > 1");
            else {
                var result = stack.Peek();
                if (result is int[] bytes) {
                    resultBytes = bytes;
                }
            }

            return resultBytes!;
        }

        //internal static byte[] GenerateBitwiseTruthtable(string expression, int variables) {
        //    return CalculatePostfix(Postfix(expression), variables);
        //}

        internal static int[] GenerateBitwiseTruthtable(string expression, int variables) {
            var postfixedExpression = Postfix(expression);
            var bwtt = CalculatePostfix(postfixedExpression, variables);
            return bwtt;
        }

        internal static List<string> ExpressionToTerm(string expression) {
            var splitExpression = Regex.Split(expression, @"([\+-])");
            var splitExpression0 = splitExpression[0];
            var termList = new List<string>(4);
            var constantList = new List<string>(4);
            var splitExpressionList = new List<string>(splitExpression);
            if (!string.IsNullOrEmpty(splitExpression0))
                splitExpressionList.Insert(0, string.Empty);
            for (int i = 0; i < splitExpressionList.Count; i++) {
                var splitExpressionItem = splitExpressionList[i];
                if (splitExpressionItem == "+" || splitExpressionItem == "-" || string.IsNullOrEmpty(splitExpressionItem))
                    continue;
                else if (Regex.Match(splitExpressionItem, @"\w+").Success) {
                    var term = splitExpressionList[i - 1] + splitExpressionList[i];
                    termList.Add(term);
                } else if (Regex.Match(splitExpressionItem, @"\d+").Success) {
                    var constant = splitExpressionList[i - 1] + splitExpressionList[i];
                    constantList.Add(constant);
                } else
                    throw new InvalidOperationException("Invalid MBA Expression!");
            }

            return termList.Concat(constantList).ToList();
        }

        internal static List<int> GetTruthtableTermlist(List<string> termList, int variables = 0) {
            if (variables == 0)
                throw new ArgumentNullException(nameof(variables));

            if (termList == null)
                return new List<int>(Enumerable.Repeat(0, (int)MathF.Pow(2, variables))/*.Select(n => n.ToString())*/);

            var result = Numpy.ZerosInt((int)MathF.Pow(2, variables));
            foreach (var term in termList) {
                var splitTerm = Regex.Split(term, @"\*");
                //Console.WriteLine("splitTerm");
                //Console.WriteLine(term);
                //Console.WriteLine(string.Join(", ", splitTerm));
                if (splitTerm.Length == 1) {
                    var expression = splitTerm[0];
                    if (Regex.Match(expression, @"\d").Success) {
                        result = Numpy.Add(result, Numpy.Fill(int.Parse(expression) * -1, (int)MathF.Pow(2, variables)));

                    } else {
                        if (expression[0] == '+') {
                            result = Numpy.Add(result, GenerateBitwiseTruthtable(expression.Substring(1), variables));

                        } else if (expression[0] == '-') {
                            result = Numpy.Subtract(result, GenerateBitwiseTruthtable(expression.Substring(1), variables));

                        } else {
                            result = Numpy.Add(result, GenerateBitwiseTruthtable(expression, variables));

                        }
                    }
                } else if (splitTerm.Length == 2) {
                    var coefficient = int.Parse(splitTerm[0]);
                    var expression = splitTerm[1];

                    var tt = GenerateBitwiseTruthtable(expression, variables);
                    var mulResult = Numpy.Multiply(tt, coefficient);
                    result = Numpy.Add(result, mulResult);

                }
            }



            return result.ToList();
        }

        internal static List<int> GenerateTruthtableFromExpression(string expression, int variables) {
            var termList = ExpressionToTerm(expression);
            var truthtableTermList = GetTruthtableTermlist(termList, variables);
            return truthtableTermList;
        }

        internal static HashSet<string> GetVariableList(string expression) {
            var varList = new HashSet<string>(4);
            var possibleVariables = new HashSet<string>() { "x", "y", "z", "t", "a", "b", "c", "d", "e", "f" };
            foreach (var @char in expression)
                if (possibleVariables.Contains(@char.ToString()))
                    varList.Add(@char.ToString());
            return varList;
        }

        internal static List<(string, string)> GenerateCoefficientList(List<string> termList) {
            var coeList = new List<(string, string)>(8);

            foreach (var term in termList) {
                var splitTerm = Regex.Split(term, @"\*");
                var mayCoefficient = splitTerm[0];
                if (!Regex.Match(mayCoefficient, @"\d").Success) {
                    var expression = splitTerm[0].Replace("+", "");
                    var coefficient = 0;
                    if (!expression.Contains("-"))
                        coefficient = 1;
                    else {
                        coefficient = -1;
                        expression = expression.Replace("-", "");
                    }
                    coeList.Add((coefficient.ToString(), expression));
                } else if (splitTerm.Length >= 2) {
                    var expression = term.Substring(mayCoefficient.Length + 1);
                    coeList.Add((mayCoefficient, expression));
                } else if (splitTerm.Length == 1 && Regex.Match(mayCoefficient, @"\d").Success) {
                    var coeValue = int.Parse(mayCoefficient) * -1;
                    coeList.Add((coeValue.ToString(), "~(x&~x)"));
                } else {
                    throw new InvalidOperationException(nameof(GenerateCoefficientList));
                }
            }

            return coeList;
        }

        internal static string CombineTerm(string expression) {
            var termList = ExpressionToTerm(expression);
            var coefficientExpressionList = GenerateCoefficientList(termList);
            coefficientExpressionList = coefficientExpressionList.OrderBy(ce => ce.Item2).ToList();
            var newCoefficientExpressionList = new List<(string, string)>(8);
            var coefficientExpression = coefficientExpressionList[0];
            foreach (var coeExpr in coefficientExpressionList.Skip(1)) {
                if (coeExpr.Item2 != coefficientExpression.Item2) {
                    if (coefficientExpression.Item1 != "0")
                        newCoefficientExpressionList.Add(coefficientExpression);
                    coefficientExpression = coeExpr;
                } else {
                    var coefficientA = coefficientExpression.Item1;
                    var coefficientB = coeExpr.Item1;
                }
            }

            if (newCoefficientExpressionList[^1].Item2 != coefficientExpression.Item2)
                newCoefficientExpressionList.Add(coefficientExpression);

            var newTermList = new List<string>(8);
            foreach (var newCoefficientExpression in newCoefficientExpressionList) {
                var newCoefficient = newCoefficientExpression.Item1;
                var newExpression = newCoefficientExpression.Item2;
                var newTerm = string.Empty;
                if (newCoefficient[0] == '+' || newCoefficient[0] == '-')
                    newTerm = $"{newCoefficient}*{newExpression}";
                else {
                    newCoefficient = $"+{newCoefficient}";
                    newTerm = $"{newCoefficient}*{newExpression}";
                }
                newTermList.Add(newTerm);
            }

            var newMbaExpr = string.Join("", newTermList);
            if (newMbaExpr[0] == '+')
                newMbaExpr = newMbaExpr.Substring(1);

            //var z3Res = VerifyMBAUnsat(expression, newMbaExpr);

            return newMbaExpr;
        }

        internal static bool VerifyMBAUnsat(string groundtruth, string complexExpression, int bitNumber = 2) {
            groundtruth = groundtruth.Trim().Replace(" ", "");
            complexExpression = complexExpression.Trim().Replace(" ", "");



            Z3Helper.Z3Example();
            
            return true;
        }        

        internal static string Reverse(string input) {
            return string.Create(input.Length, input, (chars, state) =>
            {
                state.AsSpan().CopyTo(chars);
                chars.Reverse();
            });
        }

        internal static T[] RandomSample<T>(T[] provider, int amount) {
            if (provider.Length == 0)
                throw new ArgumentNullException(nameof(provider));

            var result = new T[amount];
            for (int i = 0; i < amount; i++)
                result[i] = provider[_random.Next(provider.Length)];
            return result;
        }

        internal static T[] RandomSample<T>(List<T> provider, int amount) {
            if (provider.Count == 0)
                throw new ArgumentNullException(nameof(provider));

            var result = new T[amount];
            for (int i = 0; i < amount; i++)
                result[i] = provider[_random.Next(provider.Count)];
            return result;
        }

        internal static T RandomElement<T>(T[] provider) {
            if (provider.Length == 0)
                throw new ArgumentNullException(nameof(provider));

            return provider[_random.Next(provider.Length)];
        }

        internal static T RandomElement<T>(List<T> provider) {
            if (provider.Count == 0)
                throw new ArgumentNullException(nameof(provider));

            return provider[_random.Next(provider.Count)];
        }
    }
}
