using AoC_2018._1;
using AoC_2018._2;
using System;

namespace AoC_2018
{
    class Program
    {
        static void Main(string[] args)
        {
            SolveProblem2();

            Console.ReadKey();
        }

        private static void SolveProblem1()
        {
            Problem1 problem1 = new Problem1();

            problem1.Solve_1();
            problem1.Solve_2();
        }

        private static void SolveProblem2()
        {
            Problem2 problem = new Problem2();

            problem.Solve_1();
            //problem.Solve_2();
        }
    }
}
