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

            SolveIProblem(problem);
        }

        internal static void SolveAllProblems()
        {
            foreach (Type problemType in LoadAllProblems())
            {
                if (Activator.CreateInstance(problemType) is IProblem problem)
                {
                    SolveIProblem(problem);
                }
                else
                {
                    throw new Exception($"'{problemType} is IProblem' cast has failed unexpectedly");
                }
            }
        }

        internal static IEnumerable<Type> LoadAllProblems()
        {
            return Assembly.GetAssembly(typeof(IProblem)).GetTypes()
                .Where(type => typeof(IProblem).IsAssignableFrom(type) && !type.IsInterface);
        }

        private static void SolveIProblem(IProblem problem)
        {
            problem.Solve_1();
            problem.Solve_2();
        }
    }
}
