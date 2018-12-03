using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AoC_2018
{
    static class SolverHelpers
    {
        internal static void Solve<TProblem>()
            where TProblem : IProblem, new()
        {
            IProblem problem = new TProblem();

            problem.Solve_1();
            problem.Solve_2();
        }

        internal static void SolveAllProblems()
        {
            foreach (Type problem in LoadAllProblems())
            {
                if (Activator.CreateInstance(problem) is IProblem problemToSolve)
                {
                    problemToSolve.Solve_1();
                    problemToSolve.Solve_2();
                }
                else
                {
                    throw new Exception($"'{problem} is IProblem' cast has failed unexpectedly");
                }
            }
        }

        internal static IEnumerable<Type> LoadAllProblems()
        {
            return Assembly.GetAssembly(typeof(IProblem)).GetTypes()
                .Where(type => typeof(IProblem).IsAssignableFrom(type) && !type.IsInterface);
        }
    }
}
