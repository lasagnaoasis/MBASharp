using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MBASharp {
    internal class LinearMba {
        internal static string DatasetDirPath { get; private set; }
        internal static string OneVariableDatasetFilePath { get; private set; }
        internal static string TwoVariableDatasetFilePath { get; private set; }
        internal static string ThreeVariableDatasetFilePath { get; private set; }
        internal static string FourVariableDatasetFilePath { get; private set; }

        private int _variables;
        private int[] _coefficientList;
        private int _maxAmountOfTerms;

        private string[] _standardBitList;
        private List<string> _nonStandardBitList;

        private Regex _regexMatchOne;

        static LinearMba() {
            var datasetDirInfo = Directory.CreateDirectory("Dataset");
            DatasetDirPath = datasetDirInfo.FullName;
            OneVariableDatasetFilePath = Path.Combine(DatasetDirPath, "1_var_lmba.txt");
            TwoVariableDatasetFilePath = Path.Combine(DatasetDirPath, "2_var_lmba.txt");
            ThreeVariableDatasetFilePath = Path.Combine(DatasetDirPath, "3_var_lmba.txt");
            FourVariableDatasetFilePath = Path.Combine(DatasetDirPath, "4_var_lmba.txt");
        }

        internal LinearMba(int variables, int[] coefficientList = null, int maxAmountOfTerms = 100) {
            _variables = variables;
            _coefficientList = coefficientList;
            _maxAmountOfTerms = maxAmountOfTerms;

            if (_coefficientList == null) {
                _coefficientList = new int[2 * 19 + 2 * 13 + 10 * 7];
                for (int i = 0; i < 2 * 19; i += 2) {
                    _coefficientList[i] = 1;
                    _coefficientList[i + 1] = -1;
                }
                for (int i = 2 * 19; i < 2 * 19 + 2 * 13; i += 2) {
                    _coefficientList[i] = 2;
                    _coefficientList[i + 1] = -2;
                }
                for (int i = 2 * 19 + 2 * 13; i < 2 * 19 + 2 * 13 + 10 * 7; i += 10) {
                    _coefficientList[i] = 3;
                    _coefficientList[i + 1] = 4;
                    _coefficientList[i + 2] = 5;
                    _coefficientList[i + 3] = 7;
                    _coefficientList[i + 4] = 11;
                    _coefficientList[i + 5] = -3;
                    _coefficientList[i + 6] = -5;
                    _coefficientList[i + 7] = -6;
                    _coefficientList[i + 8] = -7;
                    _coefficientList[i + 9] = -11;
                }
                //_coefficientList = new int[2];
                //_coefficientList[0] = 1;
                //_coefficientList[1] = -1;
            }

            _regexMatchOne = new Regex("1", RegexOptions.Compiled);

            _standardBitList = new string[(int)MathF.Pow(2, variables)];
            _nonStandardBitList = new List<string>(_standardBitList.Length * 3);
            GetTruthtable();

            //Console.WriteLine(_standardBitList.Length);
            //Console.WriteLine(String.Join(", ", _standardBitList));
            //Console.WriteLine();
            //Console.WriteLine(_nonStandardBitList.Count);
            //Console.WriteLine(String.Join(", ", _nonStandardBitList));
            //Console.WriteLine();
            //Console.WriteLine(_coefficientList.Length);
            //Console.WriteLine(String.Join(", ", _coefficientList));
        }

        internal string ComplexGroundtruth(string groundtruth, string partterm = "") {
            var variablesInGroundtruth = Utils.GetVariableList(groundtruth).Count;
            var variables = 0;
            if (!string.IsNullOrEmpty(partterm)) {
                var variablesInPartterm = Utils.GetVariableList(partterm).Count;
                variables = (int)MathF.Max(variablesInGroundtruth, (int)MathF.Max(variablesInPartterm, 2));
            } else
                variables = (int)MathF.Max(variablesInGroundtruth, 2);

            return ProcessComplexGroundtruth(groundtruth, variables, partterm);
        }

        private string ProcessComplexGroundtruth(string groundtruth, int variables, string partterm) {
            var groundtruthList = Utils.GenerateTruthtableFromExpression(groundtruth, variables);
            List<int> parttermTruthlist = null;
            if (string.IsNullOrEmpty(partterm))
                parttermTruthlist = Numpy.ZerosInt((int)MathF.Pow(2, variables)).ToList();
            else
                parttermTruthlist = Utils.GenerateTruthtableFromExpression(partterm, variables);

            var randomCoefficientList = Utils.RandomSample(_coefficientList, variables);
            var randomNonStandardBitList = Utils.RandomSample(_nonStandardBitList, variables);
            //var randomCoefficientList = new int[] { 1, -1 };
            //var randomNonStandardBitList = new string[] { "~(x^y)", "~(x&~y)" };
            if (randomCoefficientList.Length != variables || randomNonStandardBitList.Length != variables)
                throw new InvalidOperationException("randomCoefficientList.Length != 2 || randomNonStandardBitList.Length != 2");

            var termList = new List<string>(8);
            for (int i = 0; i < variables; i++) {
                var coefficient = randomCoefficientList[i];
                var nsbit = randomNonStandardBitList[i];
                var term = string.Empty;
                if (coefficient > 0)
                    term = $"+{coefficient}*{nsbit}";
                else
                    term = $"{coefficient}*{nsbit}";
                termList.Add(term);
            }

            var complexExpression = string.Join("", termList);
            if (complexExpression[0] == '+')
                complexExpression = complexExpression.Substring(1);

            var complexTruthList = Utils.GenerateTruthtableFromExpression(complexExpression, variables);
            //var difTruth = Numpy.Subtract(Numpy.Subtract(groundtruthList.ToArray(), parttermTruthlist.ToArray()), complexTruthList.ToArray());
            var difTruth = Numpy.Subtract(groundtruthList.ToArray(), parttermTruthlist.ToArray());
            difTruth = Numpy.Subtract(difTruth, complexTruthList.ToArray());
            var difTruthList = difTruth.ToList();


            var standardExpressionList = new List<string>();
            var squaredVariables = (int)MathF.Pow(2, variables);
            for (int o = 0; o < squaredVariables; o++) {
                var coefficient = difTruthList[o];
                if (coefficient == 0)
                    continue;
                else {
                    var term = string.Empty;
                    if (coefficient > 0)
                        term = $"+{coefficient}*{_standardBitList[o]}";
                    else
                        term = $"{coefficient}*{_standardBitList[o]}";
                    standardExpressionList.Add(term);
                }
            }
            var standardExpression = string.Join("", standardExpressionList);

            var comExpression = string.Empty;
            if (string.IsNullOrEmpty(partterm))
                comExpression = string.Empty;
            else
                comExpression = partterm;

            if (complexExpression[0] == '-')
                comExpression += complexExpression;
            else
                comExpression += $"+{complexExpression}";

            if (!string.IsNullOrEmpty(standardExpression)) {
                if (standardExpression[0] == '-')
                    comExpression += standardExpression;
                else
                    comExpression += $"+{standardExpression}";
            }

            if (comExpression[0] == '+')
                comExpression = comExpression.Substring(1);

            //var z3Result = Utils.VerifyMBAUnsat(groundtruth, comExpression);
            //if (!z3Result)
            //    throw new InvalidOperationException("var z3Result = Utils.VerifyMBAUnsat(groundtruth, comExpression);");

            //Console.WriteLine($"Pre: {comExpression}");

            return Utils.CombineTerm(comExpression);
        }

        private void GetTruthtable() {
            var filePath = string.Empty;
            switch (_variables) {
                case 1:
                    filePath = TruthtableGenerator.OneVariableTruthtableFilePath;
                    break;
                case 2:
                    filePath = TruthtableGenerator.TwoVariableTruthtableFilePath;
                    break;
                case 3:
                    filePath = TruthtableGenerator.ThreeVariableTruthtableFilePath;
                    break;
                case 4:
                    filePath = TruthtableGenerator.FourVariableTruthtableFilePath;
                    break;
                default:
                    throw new InvalidOperationException("Incorrect number of variables");
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines) {
                var strippedLine = line.Replace(Environment.NewLine, "");
                var splitLine = strippedLine.Split(',');
                var truthtable = splitLine[0];
                var expression = splitLine[1];
                if (!truthtable.Contains("1"))
                    continue;
                else if (_regexMatchOne.Matches(truthtable).Count == 1) {
                    truthtable = truthtable.Substring(1, truthtable.Length - 2);
                    var splitTruthtable = truthtable.Split(' ');
                    var splitTruthtableAsInt = new int[splitTruthtable.Length];
                    for (int i = 0; i < splitTruthtable.Length; i++)
                        splitTruthtableAsInt[i] = int.Parse(splitTruthtable[i]);
                    var indexOfOne = Array.IndexOf(splitTruthtableAsInt, 1);
                    _standardBitList[indexOfOne] = expression;
                } else
                    _nonStandardBitList.Add(expression);
            }

            //Console.WriteLine("STANDARD BL");
            //foreach (var val in _standardBitList) {
            //    Console.WriteLine(val);
            //}
            //Console.WriteLine("===");
            //Console.WriteLine("NONSTANDARD BL");
            //foreach (var val in _nonStandardBitList) {
            //    Console.WriteLine(val);
            //}
            //Console.WriteLine("===");
        }

        private void GenerateLinearMbaDataset() {
            var filePath = string.Empty;
            switch (_variables) {
                case 1:
                    filePath = OneVariableDatasetFilePath;
                    break;
                case 2:
                    filePath = TwoVariableDatasetFilePath;
                    break;
                case 3:
                    filePath = ThreeVariableDatasetFilePath;
                    break;
                case 4:
                    filePath = FourVariableDatasetFilePath;
                    break;
                default:
                    throw new InvalidOperationException("Incorrect number of variables");
            }

            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read);
            using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);


        }
    }
}
