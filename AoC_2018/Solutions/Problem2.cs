using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileParser;

namespace AoC_2018.Solutions
{
    class Problem2 : IProblem
    {
        public string FilePath => Path.Combine("Inputs", "2.in");

        public void Solve_1()
        {
            ICollection<string> ids = ParseInput(FilePath);

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

        public void Solve_2()
        {
            List<string> ids = ParseInput(FilePath).ToList(); ;

            Tuple<string, string> correctBoxes = ExtractCorrectBoxes(ids);

            string commonChars = ExtractCommonChars(correctBoxes);

            Console.WriteLine($"Day 2, part 2: {commonChars}\n");
        }

        private Tuple<string, string> ExtractCorrectBoxes(List<string> ids)
        {
            ids.Sort();

            Tuple<string, string> correctBoxes = Tuple.Create(default(string), default(string));

            for (int idIndex = 1; idIndex < ids.Count; ++idIndex)
            {
                string currentId = ids[idIndex];
                string previousId = ids[idIndex - 1];

                if (currentId.Count() != previousId.Count())
                {
                    continue;
                }

                bool alreadyACoindidence = false;

                for (int charIndex = 0; charIndex < currentId.Count(); ++charIndex)
                {
                    if (currentId[charIndex] != previousId[charIndex])
                    {
                        if (alreadyACoindidence == true)
                        {
                            alreadyACoindidence = false;
                            break;
                        }
                        else
                        {
                            alreadyACoindidence = true;
                        }
                    }
                }

                if (alreadyACoindidence == true)
                {
                    correctBoxes = Tuple.Create(currentId, previousId);

                    if (correctBoxes.Item1 == default(string) || correctBoxes.Item2 == default(string))
                    {
                        throw new Exception("Exception in ExtractCorrectBoxes method");
                    }

                    break;
                }
            }

            return correctBoxes;
        }

        private string ExtractCommonChars(Tuple<string, string> tuple)
        {
            string commonString = default(string);

            for (int charIndex = 0; charIndex < tuple.Item1.Count(); ++charIndex)
            {
                if (tuple.Item1[charIndex] == tuple.Item2[charIndex])
                {
                    commonString += tuple.Item1[charIndex];
                }
            }

            if (commonString.Count() + 1 != tuple.Item1.Count() || commonString.Count() + 1 != tuple.Item2.Count() || commonString == default(string))
            {
                throw new Exception("Exception in ExtractCommonChars method");
            }

            return commonString;
        }

        private ICollection<string> ParseInput(string inputFile)
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
