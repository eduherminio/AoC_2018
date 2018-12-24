using System;
using System.Collections.Generic;
using FileParser;

namespace AoC_2018.Solutions
{
    public class Problem01: BaseProblem, IProblem
    {
        public void Solve_1()
        {
            IEnumerable<long> frequencyChanges = ParseInput();

            long result = 0;

            foreach (long freq in frequencyChanges)
            {
                result += freq;
            }

            Console.Write($"Day 1, part 1: {result}");
        }

        public void Solve_2()
        {
            IEnumerable<long> frequencyChanges = ParseInput();
            HashSet<long> uniqueFrequencies = new HashSet<long>() { 0 };

            long result = 0;
            bool exit = false;

            while (!exit)
            {
                foreach (long freq in frequencyChanges)
                {
                    result += freq;

                    if (uniqueFrequencies.Contains(result))
                    {
                        Console.Write($"Day 1, part 2: {result}");
                        exit = true;
                        break;
                    }
                    uniqueFrequencies.Add(result);
                }
            }
        }

        private IEnumerable<long> ParseInput()
        {
            IParsedFile parsedFile = new ParsedFile(FilePath);

            while (!parsedFile.Empty)
            {
                IParsedLine parsedLine = parsedFile.NextLine();
                while (!parsedLine.Empty)
                {
                    yield return parsedLine.NextElement<long>();
                }
            }
        }
    }
}
