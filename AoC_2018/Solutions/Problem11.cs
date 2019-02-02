using AoC_2018.Helpers;
using AoC_2018.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2018.Solutions
{
    public class Problem11 : BaseProblem, IProblem
    {
        private ulong _gridSerialNumber;

        public void Solve_1()
        {
            CalculatePowerLevelTest();

            _gridSerialNumber = ParseInput();

            IEnumerable<Point> grid = GenerateGrid();

            Dictionary<Point, int> pointPowerLevel = new Dictionary<Point, int>();
            Dictionary<Point, int> pointSquareTotalPower = new Dictionary<Point, int>();

            const int squareSize = 3;
            EvaluateGridPowerLevel(grid, ref pointPowerLevel, ref pointSquareTotalPower, squareSize);

            var pairWithMaxPower = pointSquareTotalPower.OrderByDescending(pair => pair.Value).First();

            Console.Write($"\nDay 11, part 1: {pairWithMaxPower.Key}");
        }

        public void Solve_2()
        {
            _gridSerialNumber = ParseInput();

            IEnumerable<Point> grid = GenerateGrid();

            Dictionary<Point, int> pointPowerLevel = new Dictionary<Point, int>();

            //const int maxSquareSize = 300;
            //pointPowerLevel = Solve2_BruteForce(maxSquareSize, grid, pointPowerLevel);

            Dictionary<Point, Tuple<int, int>> pointSquareTotalPowerSquareSize = new Dictionary<Point, Tuple<int, int>>();
            foreach (Point point in grid.Reverse())
            {
                EvaluatePointPowerLevelWithDifferentSquareSizes(point, ref pointPowerLevel, ref pointSquareTotalPowerSquareSize);
            }

            var max = pointSquareTotalPowerSquareSize.OrderByDescending(pair => pair.Value.Item1).First();

            Console.WriteLine($"{max.Key}, {max.Value.Item1}, {max.Value.Item2}");
        }

        private Dictionary<Point, int> Solve2_BruteForce(int maxSquareSize, IEnumerable<Point> grid, Dictionary<Point, int> pointPowerLevel)
        {
            Dictionary<Point, Tuple<int, int>> pointSquareTotalPowerSquareSize = new Dictionary<Point, Tuple<int, int>>();

            foreach (int squareSize in RangeHelpers.GenerateRange(1, maxSquareSize))
            {
                Dictionary<Point, int> pointSquareTotalPower = new Dictionary<Point, int>();

                EvaluateGridPowerLevel(grid, ref pointPowerLevel, ref pointSquareTotalPower, squareSize);

                foreach (var pair in pointSquareTotalPower)
                {
                    if (pointSquareTotalPowerSquareSize.TryGetValue(pair.Key, out Tuple<int, int> value))
                    {
                        if (value.Item1 < pair.Value)
                        {
                            pointSquareTotalPowerSquareSize[pair.Key] = Tuple.Create(pair.Value, squareSize);
                        }
                    }
                    else
                    {
                        pointSquareTotalPowerSquareSize[pair.Key] = Tuple.Create(pair.Value, squareSize);
                    }
                }
            }

            var pairWithMaxPower = pointSquareTotalPowerSquareSize.OrderByDescending(pair => pair.Value.Item1).First();

            Console.Write($"\nDay 11, part 2: {pairWithMaxPower.Key}, {pairWithMaxPower.Value.Item2}");
            return pointPowerLevel;
        }

        private ulong ParseInput()
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                return ulong.Parse(reader.ReadToEnd());
            }
        }

        private IEnumerable<Point> GenerateGrid()
        {
            var rangeXAndY = RangeHelpers.GenerateRange(minValue: 1, maxValue: 300);
            return Point.GeneratePointRangeIteratingOverXFirst(rangeXAndY, rangeXAndY);
        }

        private int CalculatePowerLevel(Point point, ulong gridSerialNumber = 0)
        {
            gridSerialNumber = gridSerialNumber == 0 ? _gridSerialNumber : gridSerialNumber;

            ulong rackId = (ulong)(10 + point.X);

            ulong powerLevel = (rackId * (ulong)point.Y + gridSerialNumber) * rackId;

            int realpowerLevel = powerLevel > 99
                ? int.Parse(powerLevel.ToString().Reverse().ElementAt(2).ToString())
                : 0;

            return realpowerLevel - 5;
        }

        private void EvaluateGridPowerLevel(IEnumerable<Point> grid, ref Dictionary<Point, int> pointPowerLevel, ref Dictionary<Point, int> pointSquareTotalPower, int squareSize)
        {
            Console.WriteLine($"Evaluating power level for squares sized {squareSize}");

            foreach (Point point in grid)
            {
                if (point.X + squareSize > 300
                    || point.Y + squareSize > 300)
                {
                    continue;
                }

                IEnumerable<Point> points = Point.GeneratePointRangeIteratingOverXFirst(
                    RangeHelpers.GenerateRange(point.X, point.X + (squareSize - 1)),
                    RangeHelpers.GenerateRange(point.Y, point.Y + (squareSize - 1)));

                int totalPower = 0;
                foreach (Point p in points)
                {
                    if (pointPowerLevel.TryGetValue(p, out int pPower))
                    {
                        totalPower += pPower;
                    }
                    else
                    {
                        pPower = CalculatePowerLevel(p);
                        pointPowerLevel.Add(p, pPower);
                        totalPower += pPower;
                    }
                }
                pointSquareTotalPower.Add(point, totalPower);
            }
        }

        private void EvaluatePointPowerLevelWithDifferentSquareSizes(Point point, ref Dictionary<Point, int> pointPowerLevel, ref Dictionary<Point, Tuple<int, int>> pointSquareTotalPowerSquareSize)
        {
            Console.WriteLine($"Evaluating point {point}");

            int maxSquareSize = Math.Min(Math.Abs(300 - point.X), Math.Abs(300 - point.Y));
            IEnumerable<int> sizeRange = RangeHelpers.GenerateRange(1, maxSquareSize);

            Dictionary<int, int> squareSizeValues = new Dictionary<int, int>();

            HashSet<Point> previousSizePoints = new HashSet<Point>();
            foreach (int squareSize in sizeRange)
            {
                Console.WriteLine($"  Evaluating power level for squares sized {squareSize}");

                if (point.X + squareSize > 300
                    || point.Y + squareSize > 300)
                {
                    continue;
                }

                IEnumerable<Point> points = Point.GeneratePointRangeIteratingOverXFirst(
                    RangeHelpers.GenerateRange(point.X, point.X + (squareSize - 1)),
                    RangeHelpers.GenerateRange(point.Y, point.Y + (squareSize - 1)));

                int totalPower = 0;
                if (previousSizePoints.Any())
                {
                    var newPoints = points.ToHashSet().Except(previousSizePoints);
                    int newPower = 0;
                    foreach (Point p in newPoints)
                    {
                        if (pointPowerLevel.TryGetValue(p, out int pPower))
                        {
                            newPower += pPower;
                        }
                        else
                        {
                            pPower = CalculatePowerLevel(p);
                            pointPowerLevel.Add(p, pPower);
                            newPower += pPower;
                        }
                    }

                    //if (newPower < 0)
                    //{
                    //    continue;
                    //}

                    totalPower = squareSizeValues.Last().Value + newPower;
                    previousSizePoints.UnionWith(newPoints.ToHashSet());
                }
                else
                {
                    foreach (Point p in points)
                    {
                        if (pointPowerLevel.TryGetValue(p, out int pPower))
                        {
                            totalPower += pPower;
                        }
                        else
                        {
                            pPower = CalculatePowerLevel(p);
                            pointPowerLevel.Add(p, pPower);
                            totalPower += pPower;
                        }
                    }

                    if (pointSquareTotalPowerSquareSize.TryGetValue(point, out var value))
                    {
                        if (totalPower > value.Item1)
                        {
                            pointSquareTotalPowerSquareSize[point] = Tuple.Create(totalPower, squareSize);
                        }
                    }
                    else
                    {
                        pointSquareTotalPowerSquareSize[point] = Tuple.Create(totalPower, squareSize);
                    }
                    previousSizePoints = points.ToHashSet();
                }
                squareSizeValues.Add(squareSize, totalPower);
            }

            if (squareSizeValues.Any())
            {
                var max = squareSizeValues.OrderByDescending(pair => pair.Value).First();
                pointSquareTotalPowerSquareSize[point] = Tuple.Create(max.Value, max.Key);
            }
        }

        private void CalculatePowerLevelTest()
        {
            if (4 != CalculatePowerLevel(new Point(3, 5), 8)
                || -5 != CalculatePowerLevel(new Point(122, 79), 57)
                || 0 != CalculatePowerLevel(new Point(217, 196), 39)
                || 4 != CalculatePowerLevel(new Point(101, 153), 71))
            {
                throw new Exception("Error in CalculatePowerLevel method");
            }
        }
    }
}
