using AoC_2018.Model;
using FileParser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2018.Solutions
{
    public class Problem06 : BaseProblem, IProblem
    {
        public void Solve_1()
        {
            List<Point> allPoints = ParseInput().ToList();

            var pointsAndItsSurroundingArea = CalculateSurroundingArea(allPoints);

            long result = pointsAndItsSurroundingArea.Values.OrderByDescending(surr => surr.Area)
                .First().Area;

            ValidateResult(allPoints, pointsAndItsSurroundingArea);

            Console.Write($"Day 6, part 1: {result}");
        }

        public void Solve_2()
        {
            List<Point> allPoints = ParseInput().ToList();

            // ~151.000, this can be improved
            HashSet<Point> candidateLocations = ExtractCandidateLocations(allPoints);

            HashSet<Point> desiredRegion = ExtractDesiredRegion(candidateLocations, allPoints).ToHashSet();

            Console.Write($"Day 6, part 2: {desiredRegion.Count}");
        }

        private IEnumerable<Point> ParseInput()
        {
            IParsedFile parsedFile = new ParsedFile(FilePath);

            while (!parsedFile.Empty)
            {
                IParsedLine parsedLine = parsedFile.NextLine();

                int.TryParse(parsedLine.NextElement<string>().Trim(','), out int x);

                yield return new Point(x, parsedLine.NextElement<int>());
            }
        }

        private const int _distanceConstraint = 10000;

        private List<Point> ExtractEdgePoints(IEnumerable<Point> allPoints)
        {
            return allPoints.Where(point =>
                allPoints.Max(p => p.X) == point.X
                || allPoints.Max(p => p.Y) == point.Y
                || allPoints.Min(p => p.X) == point.X
                || allPoints.Min(p => p.Y) == point.Y
            ).ToList();
        }

        #region Part 1
        private Dictionary<Point, SurroundingArea> CalculateSurroundingArea(ICollection<Point> allPoints)
        {
            ICollection<Point> edgePoints = ExtractEdgePoints(allPoints);
            ICollection<Point> candidatePoints = allPoints.Where(p => !edgePoints.Contains(p)).ToList();
            HashSet<Point> areaOfInterest = ExtractAreaOfInterest(edgePoints);

            Dictionary<Point, SurroundingArea> result = new Dictionary<Point, SurroundingArea>();

            foreach (Point point in areaOfInterest)
            {
                Point closestPoint = point.CalculateClosestManhattanPointNotTied(allPoints);

                if (closestPoint != null
                    && !result.TryAdd(closestPoint, new SurroundingArea(point, new[] { point })))
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

        private HashSet<Point> ExtractAreaOfInterest(ICollection<Point> edgePoints)
        {
            HashSet<Point> areaOfInterest = new HashSet<Point>();

            int minX = edgePoints.Min(p => p.X);
            int minY = edgePoints.Min(p => p.Y);
            int maxX = edgePoints.Max(p => p.X);
            int maxY = edgePoints.Max(p => p.Y);

            var xRange = Enumerable.Range(minX, maxX - minX + 1);
            var yRange = Enumerable.Range(minY, maxY - minY + 1);

            return Point.GeneratePointRangeIteratingOverYFirst(xRange, yRange).ToHashSet();
        }

        private static void ValidateResult(List<Point> allPoints, Dictionary<Point, SurroundingArea> pointsAndItsSurroundingArea)
        {
            List<Point> pointList = new List<Point>();
            foreach (SurroundingArea area in pointsAndItsSurroundingArea.Values)
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

        private class SurroundingArea
        {
            internal Point Point { get; set; }

            internal HashSet<Point> SurroundingGrid { get; set; } = new HashSet<Point>();

            internal long Area => SurroundingGrid.Count;

            internal SurroundingArea(Point point, ICollection<Point> surroundingGrid)
            {
                Point = point;

                SurroundingGrid = new HashSet<Point>() { point };
                SurroundingGrid.UnionWith(surroundingGrid);
            }
        }
        #endregion

        #region Part 2
        private HashSet<Point> ExtractCandidateLocations(ICollection<Point> allPoints)
        {
            HashSet<Point> candidateLocations = new HashSet<Point>();

            List<Point> edgePoints = ExtractEdgePoints(allPoints);

            int minX = edgePoints.Min(p => p.X);
            int minY = edgePoints.Min(p => p.Y);
            int maxX = edgePoints.Max(p => p.X);
            int maxY = edgePoints.Max(p => p.Y);

            var xRange = Enumerable.Range(maxX - _distanceConstraint, minX + _distanceConstraint - (maxX - _distanceConstraint) + 1);
            var yRange = Enumerable.Range(maxY - _distanceConstraint, minY + _distanceConstraint - (maxY - _distanceConstraint) + 1);

            // 10_000 x 10_0000, not being able to invoke GeneratePointRange yet

            // Providing all points are in minX, but one that is in maxX:
            ulong extraMinYDistanceToConsider = (ulong)(maxY - minY);   // 160_000 -> ~151_000
            List<int> curatedXRange = new List<int>();
            foreach (int x in xRange)
            {
                ulong xCounter = 0;
                foreach (int pX in allPoints.Select(p => p.X))
                {
                    xCounter += (ulong)Math.Abs(x - pX);
                    if (xCounter > _distanceConstraint)
                    {
                        break;
                    }
                }

                if (xCounter + extraMinYDistanceToConsider <= _distanceConstraint)
                {
                    curatedXRange.Add(x);
                }
            }

            // Providing all points are in minY, but one that is in maxY:
            ulong extraMinXDistanceToConsider = (ulong)(maxX - minX);   // 160_000 -> ~151_000
            List<int> curatedYRange = new List<int>();
            foreach (int y in yRange)
            {
                ulong yCounter = 0;
                foreach (int pY in allPoints.Select(p => p.Y))
                {
                    yCounter += (ulong)Math.Abs(y - pY);
                    if (yCounter > _distanceConstraint)
                    {
                        break;
                    }
                }

                if (yCounter + extraMinXDistanceToConsider <= _distanceConstraint)
                {
                    curatedYRange.Add(y);
                }
            }

            return Point.GeneratePointRangeIteratingOverYFirst(curatedXRange, curatedYRange).ToHashSet();
        }

        private IEnumerable<Point> ExtractDesiredRegion(ICollection<Point> candidateLocations, ICollection<Point> allPoints)
        {
            foreach (Point candidateLocation in candidateLocations)
            {
                int totalDistance = 0;
                foreach (Point point in allPoints)
                {
                    totalDistance += candidateLocation.ManhattanDistance(point);
                }

                if (totalDistance < _distanceConstraint)
                {
                    yield return candidateLocation;
                }
            }
        }
        #endregion
    }
}
