using AoC_2018.Model;
using FileParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC_2018.Solutions
{
    class Problem6 : IProblem
    {
        public string FilePath => Path.Combine("Inputs", "6.in");

        public void Solve_1()
        {
            List<Point> allPoints = ParseInput().ToList();

            var pointsAndItsSurroundingArea = CalculateSurroundingArea(allPoints);

            long result = pointsAndItsSurroundingArea.Values.OrderByDescending(surr => surr.Area)
                .First().Area;

            ValidateResult(allPoints, pointsAndItsSurroundingArea);

            Console.WriteLine($"Day 6, part 1: {result}");
        }

        private static void ValidateResult(List<Point> allPoints, Dictionary<Point, SurrondingArea> pointsAndItsSurroundingArea)
        {
            List<Point> pointList = new List<Point>();
            foreach (SurrondingArea area in pointsAndItsSurroundingArea.Values)
            {
                pointList.AddRange(area.SurroundingGrid);
            }

            if (pointList.Count != pointList.ToHashSet().Count)
            {
                var duplicatedItems = pointList.GroupBy(_ => _).Where(item => item.Count() > 1).Select(item => item.Key).ToList();

                string duplicated = string.Join('\n', duplicatedItems.Select(item => item.ToString()));
                string orderedByX = string.Join('\n', allPoints.OrderBy(p => p.X).Select(item => item.ToString()));
                string orderedByY = string.Join('\n', allPoints.OrderBy(p => p.Y).Select(item => item.ToString()));

                duplicatedItems.ForEach(point =>
                {
                    var guilty = pointsAndItsSurroundingArea.Where(pair => pair.Value.SurroundingGrid.Contains(point));
                });

                throw new Exception("Here's the error, repeated points");
            }
        }

        public void Solve_2()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Point> ParseInput()
        {
            IParsedFile parsedFile = new ParsedFile(FilePath);

            while (!parsedFile.Empty)
            {
                IParsedLine parsedLine = parsedFile.NextLine();

                int.TryParse(parsedLine.NextElement<string>().Trim(','), out int x);

                yield return new Point(x, parsedLine.NextElement<int>());
            }
        }

        private Dictionary<Point, SurrondingArea> CalculateSurroundingArea(ICollection<Point> allPoints)
        {
            ICollection<Point> edgePoints = ExtractEdgePoints(allPoints);

            ICollection<Point> candidatePoints = allPoints.Where(p => !edgePoints.Contains(p)).ToList();

            HashSet<Point> areaOfInterest = ExtractAreaOfInterest(edgePoints);

            Dictionary<Point, SurrondingArea> result = new Dictionary<Point, SurrondingArea>();

            foreach (Point point in areaOfInterest)
            {
                Point closestPoint = point.CalculateClosestManhattanPointNotTied(allPoints);

                if (closestPoint != null
                    && !result.TryAdd(closestPoint, new SurrondingArea(point, new[] { point })))
                {
                    result[closestPoint].SurroundingGrid.Add(point);
                }
            }

            foreach (Point point in edgePoints)
            {
                result.Remove(point);
            }

            if (candidatePoints.Count < result.Count)
            {
                throw new Exception("Error in CalculateSurrounderArea method");
            }

            return result;
        }

        private List<Point> ExtractEdgePoints(IEnumerable<Point> allPoints)
        {
            return allPoints.Where(point =>
                allPoints.Max(p => p.X) == point.X
                || allPoints.Max(p => p.Y) == point.Y
                || allPoints.Min(p => p.X) == point.X
                || allPoints.Min(p => p.Y) == point.Y
            ).ToList();
        }

        private HashSet<Point> ExtractAreaOfInterest(ICollection<Point> edgePoints)
        {
            HashSet<Point> areaOfInterest = new HashSet<Point>();

            int minX = edgePoints.Min(p => p.X);
            int minY = edgePoints.Min(p => p.Y);
            int maxX = edgePoints.Max(p => p.X);
            int maxY = edgePoints.Max(p => p.Y);

            var xRange = Enumerable.Range(minX, maxX - minX + 1);
            var yRange = Enumerable.Range(minY, maxY - minY + 1);

            return GeneratePointRange(xRange, yRange).ToHashSet();
        }

        private IEnumerable<Point> GeneratePointRange(IEnumerable<int> xRange, IEnumerable<int> yRange)
        {
            foreach (int x in xRange)
            {
                foreach (int y in yRange)
                {
                    yield return new Point(x, y);
                }
            }
        }

        internal class SurrondingArea
        {
            internal Point Point { get; set; }

            internal HashSet<Point> SurroundingGrid { get; set; } = new HashSet<Point>();

            internal long Area => SurroundingGrid.Count;

            internal SurrondingArea(Point point, ICollection<Point> surroundingGrid)
            {
                Point = point;

                SurroundingGrid = new HashSet<Point>() { point };
                SurroundingGrid.UnionWith(surroundingGrid);
            }
        }
    }
}
