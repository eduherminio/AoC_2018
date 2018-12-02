using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileParser;

namespace AoC_2018._2
{
    class Problem2
    {
        private readonly string _fileName = Path.Combine("2", "2.in");

        internal void Solve_1()
        {
            ICollection<string> ids = ParseInput(_fileName);

            long checkSum;
            int stringsWithTwoRepeatedLetters = 0, stringsWithThreeRepeatedLetters = 0;

            foreach (string id in ids)
            {
                Dictionary<char, int> charDic = new Dictionary<char, int>();
                foreach (char ch in id)
                {
                    if (!charDic.TryAdd(ch, 1))
                    {
                        charDic[ch]++;
                    }
                }

                stringsWithTwoRepeatedLetters += charDic.Values.Where(v => v == 2).Any() ? 1 : 0;
                stringsWithThreeRepeatedLetters += charDic.Values.Where(v => v == 3).Any() ? 1 : 0;
            }

            checkSum = stringsWithTwoRepeatedLetters * stringsWithThreeRepeatedLetters;

            Console.WriteLine($"Day 2, part 1: {checkSum}");
        }

        internal ICollection<string> ParseInput(string inputFile)
        {
            ICollection<string> result = new List<string>();

            IParsedFile parsedFile = new ParsedFile(inputFile);

            while (!parsedFile.Empty)
            {
                IParsedLine parsedLine = parsedFile.NextLine();
                while (!parsedLine.Empty)
                {
                    result.Add(parsedLine.NextElement<string>());
                }
            }

            return result;
        }
    }
}
