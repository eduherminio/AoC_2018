using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using FileParser;

namespace AoC_2018._1
{
    class Problem1
    {
        const string _fileName = "1\\1_1.in";

        internal void Solve()
        {
            ICollection<long> frequencyChanges = ParseInput(_fileName);

            long result = 0;

            foreach (long freq in frequencyChanges)
            {
                result += freq;
            }

            Console.WriteLine(result);
            Console.ReadKey();
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
