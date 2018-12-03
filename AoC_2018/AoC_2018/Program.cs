using AoC_2018.Solutions;
using System;

namespace AoC_2018
{
    class Program
    {
        static void Main(string[] args)
        {
            Solve<Problem1>();
            Solve<Problem2>();

            Console.ReadKey();
        }

        private static void Solve<TProblem>()
            where TProblem : IProblem, new()
        {
            IProblem problem = new TProblem();

            problem.Solve_1();
            problem.Solve_2();
        }
    }
}
