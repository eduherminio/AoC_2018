using System;
using System.Collections.Generic;
using System.Linq;
using FileParser;

namespace AoC_2018.Solutions
{
    public class Problem02 : BaseProblem, IProblem
    {
        public void Solve_1()
        {
            IEnumerable<string> ids = ParseInput();

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

                stringsWithTwoRepeatedLetters += charDic.Values.Any(v => v == 2) ? 1 : 0;
                stringsWithThreeRepeatedLetters += charDic.Values.Any(v => v == 3) ? 1 : 0;
            }

            checkSum = stringsWithTwoRepeatedLetters * stringsWithThreeRepeatedLetters;

            Console.Write($"Day 2, part 1: {checkSum}");
        }

        public void Solve_2()
        {
            List<string> ids = ParseInput().ToList();

            Tuple<string, string> correctBoxes = ExtractCorrectBoxes(ids);

            string commonChars = ExtractCommonChars(correctBoxes);

            Console.Write($"Day 2, part 2: {commonChars}");
        }

        private IEnumerable<string> ParseInput()
        {
            IParsedFile parsedFile = new ParsedFile(FilePath);

            while (!parsedFile.Empty)
            {
                IParsedLine parsedLine = parsedFile.NextLine();
                while (!parsedLine.Empty)
                {
                    yield return parsedLine.NextElement<string>();
                }
            }
        }

        private Tuple<string, string> ExtractCorrectBoxes(List<string> ids)
        {
            ids.Sort();

            Tuple<string, string> correctBoxes = Tuple.Create(default(string), default(string));

            for (int idIndex = 1; idIndex < ids.Count; ++idIndex)
            {
                string currentId = ids[idIndex];
                string previousId = ids[idIndex - 1];

                if (currentId.Length != previousId.Length)
                {
                    continue;
                }

                bool alreadyACoindidence = false;

                for (int charIndex = 0; charIndex < currentId.Length; ++charIndex)
                {
                    if (currentId[charIndex] != previousId[charIndex])
                    {
                        if (alreadyACoindidence)
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

                if (alreadyACoindidence)
                {
                    correctBoxes = Tuple.Create(currentId, previousId);

                    if (correctBoxes.Item1 == default || correctBoxes.Item2 == default)
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
            string commonString = default;

            for (int charIndex = 0; charIndex < tuple.Item1.Length; ++charIndex)
            {
                if (tuple.Item1[charIndex] == tuple.Item2[charIndex])
                {
                    commonString += tuple.Item1[charIndex];
                }
            }

            if (commonString.Length + 1 != tuple.Item1.Length || commonString.Length + 1 != tuple.Item2.Length || commonString == default)
            {
                throw new Exception("Exception in ExtractCommonChars method");
            }

            return commonString;
        }
    }
}
