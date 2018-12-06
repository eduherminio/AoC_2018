using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AoC_2018
{
    static class ProblemSolver
    {
        internal static void Solve<TProblem>()
            where TProblem : IProblem, new()
        {
            IProblem problem = new TProblem();

            Solve(problem);
        }

        internal static void SolveAllProblems()
        {
            foreach (Type problemType in LoadAllProblems())
            {
                (Activator.CreateInstance(problemType) as IProblem)
                    .SolveWithMetrics();
            }
        }

        internal static IEnumerable<Type> LoadAllProblems()
        {
            return Assembly.GetAssembly(typeof(IProblem)).GetTypes()
                .Where(type => typeof(IProblem).IsAssignableFrom(type) && !type.IsInterface);
        }

        private static void Solve(this IProblem problem)
        {
            problem.Solve_1();
            Console.WriteLine();

            problem.Solve_2();
            Console.WriteLine('\n');
        }

        private static void SolveWithMetrics(this IProblem problem)
        {
            var stopwatch = Stopwatch.StartNew();

            problem.Solve_1();

            stopwatch.Stop();
            PrintElapsedTime(stopwatch);
            stopwatch.Reset();
            stopwatch.Restart();

            problem.Solve_2();

            stopwatch.Stop();
            PrintElapsedTime(stopwatch, newLine: true);
        }

        private static void PrintElapsedTime(Stopwatch stopwatch, bool newLine = false)
        {
            ConsoleColor originalColor = Console.ForegroundColor;

            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            Performance performance = (Performance)Enum.ToObject(
                typeof(Performance),
                (elapsedMilliseconds / 1000)
                    .Clamp(min: 0, max: _actionDictionary.Count - 1));

            _actionDictionary.ChangeForegroundConsoleColor(performance);

            string elapsedTime = elapsedMilliseconds < 1000
                ? $"{elapsedMilliseconds} ms"
                : $"{0.001 * elapsedMilliseconds} s";

            Console.WriteLine($"\t\t\t\t{elapsedTime}");

            if (newLine)
            {
                Console.WriteLine();
            }


            Console.ForegroundColor = originalColor;
        }

        private enum Performance
        {
            Good = 0,
            Average = 1,
            Bad = 2
        }

        private static readonly Dictionary<Performance, Action> _actionDictionary = new Dictionary<Performance, Action>()
        {
            { Performance.Good, ()=> Console.ForegroundColor = ConsoleColor.DarkGreen },
            { Performance.Average, ()=> Console.ForegroundColor = ConsoleColor.DarkYellow },
            { Performance.Bad, ()=> Console.ForegroundColor = ConsoleColor.DarkRed }
        };

        private static void ChangeForegroundConsoleColor(this Dictionary<Performance, Action> dic, Performance key)
        {
            if (dic.TryGetValue(key, out Action action))
            {
                action.Invoke();
            }
        }

        private static long Clamp(this long value, long min, long max)
        {
            return (value <= min)
                ? min
                : (value >= max)
                    ? max
                    : value;
        }
    }
}
