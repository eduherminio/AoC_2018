using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

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

            Console.WriteLine($"Day 5, part 1: {result.Length}");
        }

        public void Solve_2()
        {
            throw new NotImplementedException();
        }

        private string ParseInput()
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                return reader.ReadToEnd();
            }
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
                if (IsReactionCondition(polymer[index -1], polymer[index]))
                {
                    polymer = RecursiveReactThatCausesStackOverflow(polymer.Remove(index - 1, 2));
                }
            }

            return polymer;
        }

        private bool IsReactionCondition(char a, char b)
        {
            return a != b && char.ToUpperInvariant(a) == char.ToUpperInvariant(b);
        }
    }
}
