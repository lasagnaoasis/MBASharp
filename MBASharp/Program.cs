namespace MBASharp {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");
            Console.WriteLine();

            TruthtableGenerator.Generate1VariableTruthtable();
            TruthtableGenerator.Generate2VariableTruthtable();
            TruthtableGenerator.Generate3VariableTruthtable();
            TruthtableGenerator.Generate4VariableTruthtable();

            var term = "x+y-z";
            var vars = Utils.GetVariableList(term);
            var lmba = new LinearMba(vars.Count);
            Console.WriteLine($"Simple expression: {term}");
            Console.WriteLine($"Complex expression: {lmba.ComplexGroundtruth(term)}");

            //Console.WriteLine();
            //var z3Res = Z3Helper.Parse("-10*((3*(x&y))&(+2*(x&~y)))+1*((3*(x&y))|(+2*(x&~y)))+11*(+2*(x&~y))-11*~((3*(x&y))|~(+2*(x&~y)))-1*~(x&~x)+2*~(x|y)+3*~(x|~y)-1*~x");
            //if (z3Res != null)
            //    Console.WriteLine(z3Res.SExpr());
            //Console.WriteLine();

            Console.WriteLine("Press any key to exit..");
            Z3Helper.Dispose();
            Console.ReadKey(true);
        }
    }
}