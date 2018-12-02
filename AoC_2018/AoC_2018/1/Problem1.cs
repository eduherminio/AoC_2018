using System;
using System.Collections.Generic;
using System.IO;
using FileParser;

namespace AoC_2018._1
{
    class Problem1
    {
        private readonly string _fileName = Path.Combine("1", "1.in");

        internal void Solve_1()
        {
            ICollection<long> frequencyChanges = ParseInput(_fileName);

            long result = 0;

            foreach (long freq in frequencyChanges)
            {
                result += freq;
            }

            Console.WriteLine($"Day 1, part 1: {result}");
        }

        internal void Solve_2()
        {
            ICollection<long> frequencyChanges = ParseInput(_fileName);
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
                        Console.WriteLine($"Day 1, part 2: {result}");
                        exit = true;
                        break;
                    }
                    uniqueFrequencies.Add(result);
                }
            }
        }

        internal ICollection<long> ParseInput(string inputFile)
        {
            ICollection<long> result = new List<long>();

            IParsedFile parsedFile = new ParsedFile(inputFile);

            while (!parsedFile.Empty)
            {
                IParsedLine parsedLine = parsedFile.NextLine();
                while (!parsedLine.Empty)
                {
                    result.Add(parsedLine.NextElement<long>());
                }
            }

            return result;
        }
    }
}
