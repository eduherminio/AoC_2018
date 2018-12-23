using FileParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC_2018.Solutions
{
    public class Problem9 : IProblem
    {
        public string FilePath => Path.Combine("Inputs", "9.in");

        public void Solve_1()
        {
            var input = ParseInput();
            int numberOfPlayers = input.Item1;
            int numberOfMarbles = input.Item2;

            //int numberOfPlayers = 9;
            //int numberOfMarbles = 25;

            //int numberOfPlayers = 13;
            //int numberOfMarbles = 7999;

            Dictionary<int, int> playerPointPairs = new Dictionary<int, int>(numberOfMarbles);
            for (int i = 0; i < numberOfPlayers; ++i)
            {
                playerPointPairs.Add(i, 0);
            }

            LinkedList<int> field = new LinkedList<int>();

            field.AddFirst(0);
            field.AddLast(1);
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

                    playerPointPairs[turn] += marbleIndex + field.ElementAt(indexOfMarbleToRemove);

                    indexOfCurrentMarble = indexOfMarbleToRemove;

                    field.Remove(field.Find(field.ElementAt(indexOfMarbleToRemove)));
                }
                else
                {
                    if (indexOfCurrentMarble + 2 <= field.Count)
                    {
                        field.AddAfter(field.Find(field.ElementAt(indexOfCurrentMarble + 1)), marbleIndex);
                        indexOfCurrentMarble += 2;
                    }
                    else if (indexOfCurrentMarble + 2 == field.Count + 1)   // Last one
                    {
                        field.AddAfter(field.First, marbleIndex);
                        indexOfCurrentMarble = 1;
                    }
                    else
                    {
                        throw new Exception(":(");
                    }
                }
            }

            int winner = playerPointPairs.Max(pair => pair.Value);

            Console.Write($"Day 9, part 1: {winner}");
        }

        public void Solve_2()
        {
            throw new NotImplementedException();
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
