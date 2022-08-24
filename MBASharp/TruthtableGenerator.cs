using Microsoft.Z3;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBASharp {
    internal static class TruthtableGenerator {
        internal static string TruthtablesDirPath { get; private set; }
        internal static string OneVariableTruthtableFilePath { get; private set; }
        internal static string TwoVariableTruthtableFilePath { get; private set; }
        internal static string ThreeVariableTruthtableFilePath { get; private set; }
        internal static string FourVariableTruthtableFilePath { get; private set; }

        internal static string[] BinaryOperators = { "&", "|", "^" }; //AND, OR, XOR
        internal static string[] UnaryOperators = { "", "~" }; //NOT
        internal static string[] XVariables = { "x", "~x" };
        internal static string[] YVariables = { "y", "~y" };
        internal static string[] ZVariables = { "z", "~z" };
        internal static string[] TVariables = { "t", "~t" };

        internal static string[] XYVariables = XVariables.Concat(YVariables).ToArray();
        //internal static string[] XYZVariables = XVariables.Concat(YVariables).Concat(ZVariables).ToArray();
        //internal static string[] XYZTVariables = XVariables.Concat(YVariables).Concat(ZVariables).Concat(TVariables).ToArray();
        internal static string[] XYZVariables = XYVariables.Concat(ZVariables).ToArray();
        internal static string[] XYZTVariables = XYZVariables.Concat(TVariables).ToArray();

        static TruthtableGenerator() {
            var truthtableDir = Directory.CreateDirectory("Truthtables");
            TruthtablesDirPath = truthtableDir.FullName;

            OneVariableTruthtableFilePath = Path.Combine(TruthtablesDirPath, "1_var_tt.txt");
            TwoVariableTruthtableFilePath = Path.Combine(TruthtablesDirPath, "2_var_tt.txt");
            ThreeVariableTruthtableFilePath = Path.Combine(TruthtablesDirPath, "3_var_tt.txt");
            FourVariableTruthtableFilePath = Path.Combine(TruthtablesDirPath, "4_var_tt.txt");
        }

        internal static void Generate1VariableTruthtable() {
            var variables = 1;
            var truthtableList = new List<string>(32);

            var exprA = "(x&~x)";
            var exprATruthtable = ByteArrayTruthtableToString(Utils.GenerateBitwiseTruthtable(exprA, variables));
            var exprAStringBuilder = new StringBuilder(exprATruthtable.Length + exprA.Length + 3);
            exprAStringBuilder.Append(exprATruthtable);
            exprAStringBuilder.Append(',');
            exprAStringBuilder.AppendLine(exprA);
            truthtableList.Add(exprAStringBuilder.ToString());

            var exprB = "~x";
            var exprBTruthtable = ByteArrayTruthtableToString(Utils.GenerateBitwiseTruthtable(exprB, variables));
            var exprBStringBuilder = new StringBuilder(exprBTruthtable.Length + exprB.Length + 3);
            exprBStringBuilder.Append(exprBTruthtable);
            exprBStringBuilder.Append(',');
            exprBStringBuilder.AppendLine(exprB);
            truthtableList.Add(exprBStringBuilder.ToString());

            var exprC = "x";
            var exprCTruthtable = ByteArrayTruthtableToString(Utils.GenerateBitwiseTruthtable(exprC, variables));
            var exprCStringBuilder = new StringBuilder(exprCTruthtable.Length + exprC.Length + 3);
            exprCStringBuilder.Append(exprCTruthtable);
            exprCStringBuilder.Append(',');
            exprCStringBuilder.AppendLine(exprC);
            truthtableList.Add(exprCStringBuilder.ToString());

            var exprD = "~(x&~x)";
            var exprDTruthtable = ByteArrayTruthtableToString(Utils.GenerateBitwiseTruthtable(exprD, variables));
            var exprDStringBuilder = new StringBuilder(exprDTruthtable.Length + exprD.Length + 3);
            exprDStringBuilder.Append(exprDTruthtable);
            exprDStringBuilder.Append(',');
            exprDStringBuilder.AppendLine(exprD);
            truthtableList.Add(exprDStringBuilder.ToString());

            OutputTruthtable(truthtableList, variables);
        }

        internal static void Generate2VariableTruthtable() {
            var variables = 2;
            //var truthtableList = new string[(int)Math.Pow(2, Math.Pow(2, variables))];
            var truthtableList = new string[(int)MathF.Pow(2, MathF.Pow(2, variables))];

            var expressionList = new List<string>(32);
            for (int i = 0; i < XYVariables.Length; i++) {
                for (int o = 0; o < BinaryOperators.Length; o++) {
                    for (int p = 0; p < XYVariables.Length; p++) {
                        if (XYVariables[i] == XYVariables[p])
                            continue;

                        var expressionStringBuilderA = new StringBuilder(32);
                        expressionStringBuilderA.Append('(');
                        expressionStringBuilderA.Append(XYVariables[i]);
                        expressionStringBuilderA.Append(BinaryOperators[o]);
                        expressionStringBuilderA.Append(XYVariables[p]);
                        expressionStringBuilderA.Append(')');

                        for (int a = 0; a < UnaryOperators.Length; a++) {
                            if (string.IsNullOrEmpty(UnaryOperators[a]))
                                expressionList.Add(expressionStringBuilderA.ToString());
                            else {
                                var expressionStringBuilderB = new StringBuilder(expressionStringBuilderA.Length);
                                expressionStringBuilderB.Append(UnaryOperators[a]);
                                expressionStringBuilderB.Append(expressionStringBuilderA.ToString());
                                expressionList.Add(expressionStringBuilderB.ToString());
                            }
                        }
                    }
                }
            }

            expressionList = XYVariables.Concat(expressionList).ToList();

            foreach (var expression in expressionList) {
                var rawTruthtable = Utils.GenerateBitwiseTruthtable(expression, variables);
                var res = 0;
                for (int s = 0; s < rawTruthtable.Length; s++)
                    //res += rawTruthtable[s] * (int)Math.Pow(2, s);
                    res += rawTruthtable[s] * (int)MathF.Pow(2, s);
                if (truthtableList[res] == null) {
                    var expressionTruthtableStringBuilder = new StringBuilder(32);
                    expressionTruthtableStringBuilder.Append(ByteArrayTruthtableToString(rawTruthtable));
                    expressionTruthtableStringBuilder.Append(',');
                    expressionTruthtableStringBuilder.AppendLine(expression);
                    truthtableList[res] = expressionTruthtableStringBuilder.ToString();
                }
            }

            OutputTruthtable(truthtableList.ToList(), variables);
        }

        internal static void Generate3VariableTruthtable() {
            var variables = 3;
            //var truthtableList = new string[(int)Math.Pow(2, Math.Pow(2, variables))];
            var truthtableList = new string[(int)MathF.Pow(2, MathF.Pow(2, variables))];

            var expressionList2Var = new List<string>(32);
            for (int i = 0; i < XYZVariables.Length; i++) {
                for (int o = 0; o < BinaryOperators.Length; o++) {
                    for (int p = 0; p < XYZVariables.Length; p++) {
                        if (XYZVariables[i] == XYZVariables[p])
                            continue;

                        var expressionStringBuilderA = new StringBuilder(32);
                        expressionStringBuilderA.Append('(');
                        expressionStringBuilderA.Append(XYZVariables[i]);
                        expressionStringBuilderA.Append(BinaryOperators[o]);
                        expressionStringBuilderA.Append(XYZVariables[p]);
                        expressionStringBuilderA.Append(')');

                        for (int a = 0; a < UnaryOperators.Length; a++) {
                            if (string.IsNullOrEmpty(UnaryOperators[a]))
                                expressionList2Var.Add(expressionStringBuilderA.ToString());
                            else {
                                var expressionStringBuilderB = new StringBuilder(expressionStringBuilderA.Length + 1);
                                expressionStringBuilderB.Append(UnaryOperators[a]);
                                expressionStringBuilderB.Append(expressionStringBuilderA.ToString());
                                expressionList2Var.Add(expressionStringBuilderB.ToString());
                            }
                        }
                    }
                }
            }

            var expressionList3Var = new List<string>(32 * 8);
            for (int a = 0; a < ZVariables.Length; a++) {
                for (int s = 0; s < BinaryOperators.Length; s++) {
                    for (int d = 0; d < YVariables.Length; d++) {
                        var expressionStringBuilderA = new StringBuilder(32);
                        expressionStringBuilderA.Append('(');
                        expressionStringBuilderA.Append(YVariables[d]);
                        expressionStringBuilderA.Append(BinaryOperators[s]);
                        expressionStringBuilderA.Append(ZVariables[a]);
                        expressionStringBuilderA.Append(')');

                        for (int f = 0; f < UnaryOperators.Length; f++) {
                            var expressionStringBuilderB = new StringBuilder(expressionStringBuilderA.Length);
                            if (string.IsNullOrEmpty(UnaryOperators[f]))
                                expressionStringBuilderB.Append(expressionStringBuilderA.ToString());
                            else {
                                expressionStringBuilderB.Append(UnaryOperators[f]);
                                expressionStringBuilderB.Append(expressionStringBuilderA.ToString());
                            }
                            for (int g = 0; g < BinaryOperators.Length; g++) {
                                for (int h = 0; h < XVariables.Length; h++) {
                                    var expressionStringBuilderC = new StringBuilder(expressionStringBuilderB.Length);
                                    expressionStringBuilderC.Append('(');
                                    expressionStringBuilderC.Append(XVariables[h]);
                                    expressionStringBuilderC.Append(BinaryOperators[g]);
                                    expressionStringBuilderC.Append(expressionStringBuilderB.ToString());
                                    expressionStringBuilderC.Append(')');
                                    for (int j = 0; j < UnaryOperators.Length; j++) {
                                        if (string.IsNullOrEmpty(UnaryOperators[j]))
                                            expressionList3Var.Add(expressionStringBuilderC.ToString());
                                        else {
                                            var expressionStringBuilderD = new StringBuilder(expressionStringBuilderC.Length);
                                            expressionStringBuilderD.Append(UnaryOperators[j]);
                                            expressionStringBuilderD.Append(expressionStringBuilderC.ToString());
                                            expressionList3Var.Add(expressionStringBuilderD.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var expressionList = XYZVariables.Concat(expressionList2Var).Concat(expressionList3Var).ToList();
            var termList = new List<string>(32 * 8);
            foreach (var expression in expressionList) {
                var rawTruthtable = Utils.GenerateBitwiseTruthtable(expression, variables);
                var res = 0;
                for (int k = 0; k < rawTruthtable.Length; k++)
                    //res += rawTruthtable[k] * (int)Math.Pow(2, k);
                    res += rawTruthtable[k] * (int)MathF.Pow(2, k);
                if (truthtableList[res] == null) {
                    var expressionTruthtableStringBuilder = new StringBuilder(32);
                    expressionTruthtableStringBuilder.Append(ByteArrayTruthtableToString(rawTruthtable));
                    expressionTruthtableStringBuilder.Append(',');
                    expressionTruthtableStringBuilder.AppendLine(expression);
                    truthtableList[res] = expressionTruthtableStringBuilder.ToString();
                    termList.Add(expression);
                }
            }

            var expressionListMore3Var = new List<string>(32 * 8);
            for (int l = 0; l < termList.Count; l++) {
                for (int y = 0; y < termList.Count; y++) {
                    if (termList[l] == termList[y])
                        continue;

                    for (int x = 0; x < BinaryOperators.Length; x++) {
                        var termStringBuilder = new StringBuilder(32);
                        termStringBuilder.Append('(');
                        termStringBuilder.Append(termList[l]);
                        termStringBuilder.Append(BinaryOperators[x]);
                        termStringBuilder.Append(termList[y]);
                        termStringBuilder.Append(')');
                        expressionListMore3Var.Add(termStringBuilder.ToString());
                    }
                }
            }

            foreach (var expression in expressionListMore3Var) {
                var rawTruthtable = Utils.GenerateBitwiseTruthtable(expression, variables);
                var res = 0;
                for (int c = 0; c < rawTruthtable.Length; c++)
                    //res += rawTruthtable[c] * (int)Math.Pow(2, c);
                    res += rawTruthtable[c] * (int)MathF.Pow(2, c);
                if (truthtableList[res] == null) {
                    var expressionTruthtableStringBuilder = new StringBuilder(32);
                    expressionTruthtableStringBuilder.Append(ByteArrayTruthtableToString(rawTruthtable));
                    expressionTruthtableStringBuilder.Append(',');
                    expressionTruthtableStringBuilder.AppendLine(expression);
                    truthtableList[res] = expressionTruthtableStringBuilder.ToString();
                    termList.Add(expression);
                }
            }

            OutputTruthtable(truthtableList.ToList(), variables);
        }

        internal static void Generate4VariableTruthtable() {
            var variables = 4;
            //var truthtableList = new string[(int)Math.Pow(2, Math.Pow(2, variables))];
            var truthtableList = new string[(int)MathF.Pow(2, MathF.Pow(2, variables))];

            var expressionList2Var = new List<string>(32);
            for (int i = 0; i < XYZTVariables.Length; i++) {
                for (int o = 0; o < BinaryOperators.Length; o++) {
                    for (int p = 0; p < XYZTVariables.Length; p++) {
                        if (XYZTVariables[i] == XYZTVariables[p])
                            continue;

                        var expressionStringBuilderA = new StringBuilder(32);
                        expressionStringBuilderA.Append('(');
                        expressionStringBuilderA.Append(XYZTVariables[i]);
                        expressionStringBuilderA.Append(BinaryOperators[o]);
                        expressionStringBuilderA.Append(XYZTVariables[p]);
                        expressionStringBuilderA.Append(')');

                        for (int a = 0; a < UnaryOperators.Length; a++) {
                            if (string.IsNullOrEmpty(UnaryOperators[a]))
                                expressionList2Var.Add(expressionStringBuilderA.ToString());
                            else {
                                var expressionStringBuilderB = new StringBuilder(expressionStringBuilderA.Length + 1);
                                expressionStringBuilderB.Append(UnaryOperators[a]);
                                expressionStringBuilderB.Append(expressionStringBuilderA.ToString());
                                expressionList2Var.Add(expressionStringBuilderB.ToString());
                            }
                        }
                    }
                }
            }

            var expressionList3Var = new List<string>(32 * 8);
            for (int i = 0; i < XYZTVariables.Length; i++) {
                for (int o = 0; o < BinaryOperators.Length; o++) {
                    for (int p = 0; p < XYZTVariables.Length; p++) {
                        if (XYZTVariables[i] == XYZTVariables[p])
                            continue;

                        var expressionStringBuilderA = new StringBuilder(32);
                        expressionStringBuilderA.Append('(');
                        expressionStringBuilderA.Append(XYZTVariables[i]);
                        expressionStringBuilderA.Append(BinaryOperators[o]);
                        expressionStringBuilderA.Append(XYZTVariables[p]);
                        expressionStringBuilderA.Append(')');

                        for (int a = 0; a < UnaryOperators.Length; a++) {
                            var expressionStringBuilderB = new StringBuilder(expressionStringBuilderA.Length + 1);
                            if (string.IsNullOrEmpty(UnaryOperators[a])) {
                                expressionStringBuilderB.Append(expressionStringBuilderA.ToString());
                            } else {
                                expressionStringBuilderB.Append(UnaryOperators[a]);
                                expressionStringBuilderB.Append(expressionStringBuilderA.ToString());
                            }

                            for (int s = 0; s < XYZTVariables.Length; s++) {
                                if (XYZTVariables[s] == XYZTVariables[i] || XYZTVariables[s] == XYZTVariables[p])
                                    continue;

                                for (int d = 0; d < BinaryOperators.Length; d++) {
                                    var expressionStringBuilderC = new StringBuilder(expressionStringBuilderB.Length);
                                    expressionStringBuilderC.Append('(');
                                    expressionStringBuilderC.Append(expressionStringBuilderB.ToString());
                                    expressionStringBuilderC.Append(BinaryOperators[d]);
                                    expressionStringBuilderC.Append(XYZTVariables[s]);
                                    expressionStringBuilderC.Append(')');

                                    for (int f = 0; f < UnaryOperators.Length; f++) {
                                        if (string.IsNullOrEmpty(UnaryOperators[f])) {
                                            expressionList3Var.Add(expressionStringBuilderC.ToString());
                                        } else {
                                            var expressionStringBuilderD = new StringBuilder(expressionStringBuilderC.Length + 1);
                                            expressionStringBuilderD.Append(UnaryOperators[f]);
                                            expressionStringBuilderD.Append(expressionStringBuilderC.ToString());
                                            expressionList3Var.Add(expressionStringBuilderD.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var expressionList4Var = new List<string>(32 * 8);
            for (int g = 0; g < TVariables.Length; g++) {
                for (int h = 0; h < BinaryOperators.Length; h++) {
                    for (int j = 0; j < ZVariables.Length; j++) {
                        var expressionStringBuilderA = new StringBuilder(32);
                        expressionStringBuilderA.Append('(');
                        expressionStringBuilderA.Append(ZVariables[j]);
                        expressionStringBuilderA.Append(BinaryOperators[h]);
                        expressionStringBuilderA.Append(TVariables[g]);
                        expressionStringBuilderA.Append(')');
                        for (int k = 0; k < UnaryOperators.Length; k++) {
                            var expressionStringBuilderB = new StringBuilder(expressionStringBuilderA.Length + 1);
                            if (string.IsNullOrEmpty(UnaryOperators[k])) {
                                expressionStringBuilderB.Append(expressionStringBuilderA.ToString());
                            } else {
                                expressionStringBuilderB.Append(UnaryOperators[k]);
                                expressionStringBuilderB.Append(expressionStringBuilderA.ToString());
                            }

                            for (int l = 0; l < BinaryOperators.Length; l++) {
                                for (int y = 0; y < YVariables.Length; y++) {
                                    var expressionStringBuilderC = new StringBuilder(expressionStringBuilderB.Length);
                                    expressionStringBuilderC.Append('(');
                                    expressionStringBuilderC.Append(YVariables[y]);
                                    expressionStringBuilderC.Append(BinaryOperators[l]);
                                    expressionStringBuilderC.Append(expressionStringBuilderB.ToString());
                                    expressionStringBuilderC.Append(')');

                                    for (int x = 0; x < UnaryOperators.Length; x++) {
                                        var expressionStringBuilderD = new StringBuilder(expressionStringBuilderC.Length + 1);
                                        if (string.IsNullOrEmpty(UnaryOperators[x])) {
                                            expressionStringBuilderD.Append(expressionStringBuilderC.ToString());
                                        } else {
                                            expressionStringBuilderD.Append(UnaryOperators[x]);
                                            expressionStringBuilderD.Append(expressionStringBuilderC.ToString());
                                        }

                                        for (int c = 0; c < BinaryOperators.Length; c++) {
                                            for (int v = 0; v < XVariables.Length; v++) {
                                                var expressionStringBuilderE = new StringBuilder(expressionStringBuilderD.Length);
                                                expressionStringBuilderE.Append('(');
                                                expressionStringBuilderE.Append(XVariables[v]);
                                                expressionStringBuilderE.Append(BinaryOperators[c]);
                                                expressionStringBuilderE.Append(expressionStringBuilderD.ToString());
                                                expressionStringBuilderE.Append(')');

                                                for (int b = 0; b < UnaryOperators.Length; b++) {
                                                    if (string.IsNullOrEmpty(UnaryOperators[b])) {
                                                        expressionList4Var.Add(expressionStringBuilderE.ToString());
                                                    } else {
                                                        var expressionStringBuilderF = new StringBuilder(expressionStringBuilderE.Length + 1);
                                                        expressionStringBuilderF.Append(UnaryOperators[b]);
                                                        expressionStringBuilderF.Append(expressionStringBuilderE.ToString());
                                                        expressionList4Var.Add(expressionStringBuilderF.ToString());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var expressionList = XYZTVariables.Concat(expressionList2Var).Concat(expressionList3Var).Concat(expressionList4Var).ToList();
            foreach (var expression in expressionList) {
                var rawTruthtable = Utils.GenerateBitwiseTruthtable(expression, variables);
                var res = 0;
                for (int n = 0; n < rawTruthtable.Length; n++)
                    //res += rawTruthtable[n] * (int)Math.Pow(2, n);
                    res += rawTruthtable[n] * (int)MathF.Pow(2, n);
                if (truthtableList[res] == null) {
                    var expressionTruthtableStringBuilder = new StringBuilder(32);
                    expressionTruthtableStringBuilder.Append(ByteArrayTruthtableToString(rawTruthtable));
                    expressionTruthtableStringBuilder.Append(',');
                    expressionTruthtableStringBuilder.AppendLine(expression);
                    truthtableList[res] = expressionTruthtableStringBuilder.ToString();
                }
            }

            //foreach (var tt in truthtableList)
            //    if (!string.IsNullOrEmpty(tt))
            //        Console.WriteLine(tt);

            var lastTruthDict = TruthtableToDictionary(variables - 1);
            for (int m = 0; m < truthtableList.Length; m++) {
                var expressionTruthtable = truthtableList[m];
                if (!string.IsNullOrEmpty(expressionTruthtable))
                    continue;

                var remainingBitwiseExpression = GenerateOneNewBitwiseExpression(m, variables, "t", lastTruthDict);
                truthtableList[m] = remainingBitwiseExpression;
            }

            OutputTruthtable(truthtableList.ToList(), variables);
        }

        private static Dictionary<int, string> TruthtableToDictionary(int variables) {
            var truthDict = new Dictionary<int, string>(32 * 8);
            var filePath = string.Empty;
            switch (variables) {
                case 1:
                    filePath = OneVariableTruthtableFilePath;
                    break;
                case 2:
                    filePath = TwoVariableTruthtableFilePath;
                    break;
                case 3:
                    filePath = ThreeVariableTruthtableFilePath;
                    break;
                case 4:
                    filePath = FourVariableTruthtableFilePath;
                    break;
                default:
                    throw new InvalidOperationException("Incorrect number of variables");
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines) {
                var trimmedLine = line.Trim();
                var itemList = trimmedLine.Split(',');
                var truthtable = itemList[0].Replace(" ", "");
                truthtable = truthtable.Substring(1, truthtable.Length - 2);
                var expression = itemList[1];
                var key = 0;
                for (int i = 0; i < truthtable.Length; i++)
                    //key += (int)Math.Pow(2, i) * (int)char.GetNumericValue(truthtable[i]);
                    key += (int)MathF.Pow(2, i) * (int)char.GetNumericValue(truthtable[i]);
                truthDict[key] = expression;
            }

            return truthDict;
        }

        private static string GenerateOneNewBitwiseExpression(int index, int variables, string freeVariable, Dictionary<int, string> lastTruthDict) {
            if (lastTruthDict == null)
                lastTruthDict = TruthtableToDictionary(variables - 1);

            //var amountOfLeadingZeros = (int)Math.Pow(2, variables);
            var amountOfLeadingZeros = (int)MathF.Pow(2, variables);
            var truthtableStr = new string('0', amountOfLeadingZeros) + Convert.ToString(index, 2);
            truthtableStr = Utils.Reverse(truthtableStr).Substring(0, amountOfLeadingZeros);
            //var truthtablePreStr = truthtableStr.Substring(0, (int)Math.Pow(2, variables - 1));
            var truthtablePreStr = truthtableStr.Substring(0, (int)MathF.Pow(2, variables - 1));
            //var truthtablePostStr = truthtableStr.Substring((int)Math.Pow(2, variables - 1));
            var truthtablePostStr = truthtableStr.Substring((int)MathF.Pow(2, variables - 1));
            var preValue = 0;
            var postValue = 0;
            for (int i = 0; i < truthtablePreStr.Length; i++)
                //preValue += (int)Math.Pow(2, i) * (int)char.GetNumericValue(truthtablePreStr[i]);
                preValue += (int)MathF.Pow(2, i) * (int)char.GetNumericValue(truthtablePreStr[i]);
            for (int o = 0; o < truthtablePostStr.Length; o++)
                //postValue += (int)Math.Pow(2, o) * (int)char.GetNumericValue(truthtablePostStr[o]);
                postValue += (int)MathF.Pow(2, o) * (int)char.GetNumericValue(truthtablePostStr[o]);
            //Console.WriteLine(preValue);
            var preExpression = lastTruthDict[preValue];
            var postExpression = lastTruthDict[postValue];
            var newExpression = $"(({preExpression}&~{freeVariable})|({postExpression}&{freeVariable}))";

            var newExpressionTruthtable = ByteArrayTruthtableToString(Utils.GenerateBitwiseTruthtable(newExpression, variables));
            var newExpressionStringBuilder = new StringBuilder(newExpressionTruthtable.Length + newExpression.Length + 3);
            newExpressionStringBuilder.Append(newExpressionTruthtable);
            newExpressionStringBuilder.Append(',');
            newExpressionStringBuilder.AppendLine(newExpression);

            return newExpressionStringBuilder.ToString();
        }

        //private static string ByteArrayTruthtableToString(byte[] truthtable) {
        //    var noSpaceLength = truthtable.Length - 1;

        //    var truthtableStringBuilder = new StringBuilder(32);
        //    truthtableStringBuilder.Append("[");
        //    for (int i = 0; i < truthtable.Length; i++) {
        //        truthtableStringBuilder.Append(truthtable[i]);
        //        if (i != noSpaceLength)
        //            truthtableStringBuilder.Append(" ");
        //    }
        //    truthtableStringBuilder.Append("]");
        //    return truthtableStringBuilder.ToString();
        //}

        private static string ByteArrayTruthtableToString(int[] truthtable) {
            var noSpaceLength = truthtable.Length - 1;

            var truthtableStringBuilder = new StringBuilder(32);
            truthtableStringBuilder.Append("[");
            for (int i = 0; i < truthtable.Length; i++) {
                truthtableStringBuilder.Append(truthtable[i]);
                if (i != noSpaceLength)
                    truthtableStringBuilder.Append(" ");
            }
            truthtableStringBuilder.Append("]");
            return truthtableStringBuilder.ToString();
        }

        private static void OutputTruthtable(List<string> ttl, int variables) {
            var savePath = string.Empty;
            switch (variables) {
                case 1:
                    savePath = OneVariableTruthtableFilePath;
                    break;
                case 2:
                    savePath = TwoVariableTruthtableFilePath;
                    break;
                case 3:
                    savePath = ThreeVariableTruthtableFilePath;
                    break;
                case 4:
                    savePath = FourVariableTruthtableFilePath;
                    break;
                default:
                    throw new InvalidOperationException("Incorrect number of variables");
            }

            using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.Read);
            using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            foreach (var @string in ttl)
                streamWriter.Write(@string);
        }

        private static string ToBinary(int value) {
            var valueAsStr = Convert.ToString(value, 2);
            var sizeOfInt = sizeof(int);
            var sizeOfIntInBits = sizeOfInt * 8;
            var leadingZeros = sizeOfIntInBits - valueAsStr.Length;
            if (leadingZeros == 0)
                return valueAsStr;

            var zeros = new string('0', leadingZeros);
            return zeros + valueAsStr;
        }
    }
}
