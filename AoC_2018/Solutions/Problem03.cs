﻿using AoC_2018.Model;
using FileParser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2018.Solutions
{
    public class Problem03 : BaseProblem, IProblem
    {
        public void Solve_1()
        {
            List<Rectangle> rectangles = ParseInput().ToList();

            HashSet<Point> repeatedPoints = ExtractRepeatedPoints(rectangles);

            Console.Write($"Day 3, part 1: {repeatedPoints.Count}");
        }

        public void Solve_2()
        {
            List<Rectangle> rectangles = ParseInput().ToList();

            HashSet<Point> repeatedPoints = ExtractRepeatedPoints(rectangles);

            Rectangle nonOverlappedRectangle_1 = ExtractNonOverlappedRectangle_ManualImplementation(rectangles, repeatedPoints);

            Console.Write($"Day 3, part 2: {nonOverlappedRectangle_1.Id}");
        }

        private IEnumerable<Rectangle> ParseInput()
        {
            ICollection<Rectangle> rectangles = new List<Rectangle>();

            IParsedFile parsedFile = new ParsedFile(FilePath);

            while (!parsedFile.Empty)
            {
                IParsedLine parsedLine = parsedFile.NextLine();

                string idString = parsedLine.NextElement<string>();
                if (!idString.StartsWith("#"))
                {
                    throw new Exception($"{idString} is not #n");
                }
                int.TryParse(idString.Trim('#'), out int id);

                if (parsedLine.NextElement<string>() != "@")
                {
                    throw new Exception($"Exception parsing @");
                }

                string[] x0y0 = parsedLine.NextElement<string>()
                    .Trim(':')
                    .Split(',');
                if (x0y0.Length != 2)
                {
                    throw new Exception($"Length of {x0y0} isn't 2");
                }
                int.TryParse(x0y0.First(), out int x0);
                int.TryParse(x0y0.Last(), out int y0);

                string[] xy = parsedLine.NextElement<string>()
                    .Split('x');
                if (xy.Length != 2)
                {
                    throw new Exception($"Length of {xy} isn't 2");
                }
                int.TryParse(xy.First(), out int x);
                int.TryParse(xy.Last(), out int y);

                if (!parsedLine.Empty)
                {
                    throw new Exception($"Error parsing line, missing at least {parsedLine.PeekNextElement<string>()}");
                }

                rectangles.Add(new Rectangle(x: x, y: y, x0: x0, y0: y0) { Id = id });
            }
            return rectangles;
        }

        private HashSet<Point> ExtractRepeatedPoints(IEnumerable<Rectangle> rectangles)
        {
            HashSet<Point> evaluatedPoints = new HashSet<Point>();
            HashSet<Point> repeatedPoints = new HashSet<Point>();

            foreach (Point point in rectangles.SelectMany(r => r.PointSet))
            {
                if (!evaluatedPoints.Add(point))
                {
                    repeatedPoints.Add(point);
                }
            }

            return repeatedPoints;
        }

        /// <summary>
        /// Faster
        /// </summary>
        /// <param name="rectangles"></param>
        /// <param name="repeatedPoints"></param>
        /// <returns></returns>
        private Rectangle ExtractNonOverlappedRectangle_ManualImplementation(List<Rectangle> rectangles, HashSet<Point> repeatedPoints)
        {
            List<Rectangle> nonOverlappedRectangles = new List<Rectangle>();

            foreach (Rectangle rectangle in rectangles)
            {
                bool isIntact = true;
                foreach (Point point in rectangle.PointSet)
                {
                    if (repeatedPoints.Contains(point))
                    {
                        isIntact = false;
                        break;
                    }
                }

                if (isIntact)
                {
                    nonOverlappedRectangles.Add(rectangle);
                }
            }

            if (nonOverlappedRectangles.Count != 1)
            {
                throw new Exception("Error in ExtractNonOverlappedRectangle method");
            }

            return nonOverlappedRectangles.First();
        }

        /// <summary>
        /// Smarter
        /// </summary>
        /// <param name="rectangles"></param>
        /// <param name="repeatedPoints"></param>
        /// <returns></returns>
        private Rectangle ExtractNonOverlappedRectangle_LinqImplementation(List<Rectangle> rectangles, HashSet<Point> repeatedPoints)
        {
            return rectangles.Single(rectangle =>
                 !repeatedPoints.Any(repeatedPoint => rectangle.PointSet.Contains(repeatedPoint)));
        }

        private class Rectangle
        {
            public int Id { get; set; }
            public HashSet<Point> PointSet { get; set; } = new HashSet<Point>();

            public Rectangle(int x, int y, int x0, int y0)
            {
                for (int xIndex = x0 + 1; xIndex <= x0 + x; ++xIndex)
                {
                    for (int yIndex = y0; yIndex < y0 + y; ++yIndex)
                    {
                        PointSet.Add(new Point(xIndex, yIndex));
                    }
                }
                if (x * y != PointSet.Count)
                {
                    throw new Exception("Error in rectangle constructor");
                }
            }
        }
    }
}
