using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2018.Solutions
{
    class Problem5 : IProblem
    {
        public string FilePath => Path.Combine("Inputs", "5.in");

        public void Solve_1()
        {
            string input = ParseInput();

            string result = CannotBelieveIveImplementedThisReact(input);

            if (result.Length >= input.Length)
            {
                throw new Exception("Something's wrong inplemented");
            }

            Console.Write($"Day 5, part 1: {result.Length}");
        }

        public void Solve_2()
        {
            string input = ParseInput();

            string unfinishedResult = CannotBelieveIveImplementedThisReact(input);

            int resultLength = ReactRemovingProblematicUnit(unfinishedResult);

            if (resultLength >= unfinishedResult.Length)
            {
                throw new Exception("Something's wrong inplemented");
            }

            Console.Write($"Day 5, part 2: {resultLength}");
        }

        private string ParseInput()
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                return reader.ReadToEnd();
            }
        }

        private bool IsReactionCondition(char a, char b)
        {
            return a != b && char.ToUpperInvariant(a) == char.ToUpperInvariant(b);
        }

        private string CannotBelieveIveImplementedThisReact(string polymer)
        {
            Start:
            for (int index = 1; index < polymer.Length; ++index)
            {
                if (IsReactionCondition(polymer[index - 1], polymer[index]))
                {
                    polymer = polymer.Remove(index - 1, 2);
                    goto Start;
                }
            }

            return polymer;
        }

        private string RecursiveReactThatCausesStackOverflow(string polymer)
        {
            for (int index = 1; index < polymer.Length; ++index)
            {
                if (IsReactionCondition(polymer[index - 1], polymer[index]))
                {
                    polymer = RecursiveReactThatCausesStackOverflow(polymer.Remove(index - 1, 2));
                }
            }

            return polymer;
        }

        private int ReactRemovingProblematicUnit(string polymer)
        {
            HashSet<char> existingUnits = polymer.ToUpperInvariant().ToHashSet();
            Dictionary<char, int> charLength = new Dictionary<char, int>();

            foreach (char unit in existingUnits)
            {
                string modifiedPolymer = polymer
                    .Replace(unit.ToString(), string.Empty)
                    .Replace(unit.ToString().ToLowerInvariant(), string.Empty);

                string result = CannotBelieveIveImplementedThisReact(modifiedPolymer);

                charLength.Add(unit, result.Length);
            }

            return charLength.OrderBy(pair => pair.Value).First().Value;
        }
    }
}
