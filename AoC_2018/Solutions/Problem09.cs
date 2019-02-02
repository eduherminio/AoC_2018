using FileParser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2018.Solutions
{
    public class Problem09 : BaseProblem, IProblem
    {
        public void Solve_1()
        {
            var input = ParseInput();
            int numberOfPlayers = input.Item1;
            int numberOfMarbles = input.Item2;

            ulong winner = SolveMarbleGame(numberOfPlayers, numberOfMarbles);

            Console.Write($"Day 9, part 1: {winner}");
        }

        public void Solve_2()
        {
            var input = ParseInput();
            int numberOfPlayers = input.Item1;
            int numberOfMarbles = 100 * input.Item2;

            ulong winner = SolveMarbleGame(numberOfPlayers, numberOfMarbles);

            Console.Write($"Day 9, part 2: {winner}");
        }

        private static ulong SolveMarbleGame(int numberOfPlayers, int numberOfMarbles)
        {
            Dictionary<int, ulong> playerPointPairs = new Dictionary<int, ulong>(numberOfMarbles);
            for (int i = 0; i < numberOfPlayers; ++i)
            {
                playerPointPairs.Add(i, 0);
            }

            List<int> field = new List<int> { 0, 1 };

            int turn = 0;
            int indexOfCurrentMarble = 1;

            for (int marbleIndex = 2; marbleIndex <= numberOfMarbles; ++marbleIndex)
            {
                ++turn;
                turn %= numberOfPlayers;

                if (marbleIndex % 23 == 0)
                {
                    int indexOfMarbleToRemove =
                        (indexOfCurrentMarble - 7) > 0
                        ? indexOfCurrentMarble - 7
                        : field.Count + (indexOfCurrentMarble - 7);

                    playerPointPairs[turn] += (ulong)(marbleIndex + field[indexOfMarbleToRemove]);

                    indexOfCurrentMarble = indexOfMarbleToRemove;

                    field.RemoveAt(indexOfMarbleToRemove);
                }
                else
                {
                    if (indexOfCurrentMarble + 2 <= field.Count)
                    {
                        field.Insert(indexOfCurrentMarble + 2, marbleIndex);
                        indexOfCurrentMarble += 2;
                    }
                    else if (indexOfCurrentMarble + 2 == field.Count + 1)   // Last one
                    {
                        field.Insert(1, marbleIndex);
                        indexOfCurrentMarble = 1;
                    }
                    else
                    {
                        throw new Exception(":(");
                    }
                }
            }

            return playerPointPairs.Max(pair => pair.Value);
        }

        public Tuple<int, int> ParseInput()
        {
            IParsedFile parsedFile = new ParsedFile(FilePath);

            IParsedLine parsedLine = parsedFile.NextLine();

            int nPlayers = parsedLine.NextElement<int>();

            string str = string.Empty;
            do
            {
                str = parsedLine.NextElement<string>();
            } while (str != "worth");

            int maxPoints = parsedLine.NextElement<int>();

            return Tuple.Create(nPlayers, maxPoints);
        }
    }
}
