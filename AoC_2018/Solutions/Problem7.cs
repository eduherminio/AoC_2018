using AoC_2018.Model;
using FileParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC_2018.Solutions
{
    public class Problem7 : IProblem
    {
        public string FilePath => Path.Combine("Inputs", "7.in");

        public void Solve_1()
        {
            ICollection<Step> unorderedSteps = ParseInput();

            string result = ApplyPart1Algorithm(unorderedSteps);

            Console.Write($"Day 7, part 1: {result}");
        }

        private string ApplyPart1Algorithm(ICollection<Step> unorderedSteps)
        {
            string result = string.Empty;

            while (unorderedSteps.Any())
            {
                Step stepToRemove = unorderedSteps
                    .OrderBy(step => step.NonResolvedDependencies.Count)
                    .ThenBy(step => step.Name)
                    .First();

                if (stepToRemove.NonResolvedDependencies.Any())
                {
                    throw new Exception("Exception in ApplyPart1Algorithm");
                }
                unorderedSteps.Remove(stepToRemove);

                foreach (Step step in unorderedSteps)
                {
                    step.NonResolvedDependencies.Remove(stepToRemove);
                }
                result += stepToRemove.Name;
            }

            return result;
        }

        public void Solve_2()
        {
            throw new NotImplementedException();
        }

        private HashSet<Step> ParseInput()
        {
            HashSet<Step> steps = new HashSet<Step>();

            IParsedFile parsedFile = new ParsedFile(FilePath);

            while (!parsedFile.Empty)
            {
                IParsedLine parsedLine = parsedFile.NextLine();

                parsedLine.NextElement<string>();

                string dependencyName = parsedLine.NextElement<string>();
                Step stepDependency = new Step(dependencyName.Single());
                steps.Add(stepDependency);

                while (!parsedLine.Empty)
                {
                    string word = parsedLine.NextElement<string>();

                    if (word.ToUpperInvariant() == "STEP")
                    {
                        string mainStepName = parsedLine.NextElement<string>();

                        if (steps.TryGetValue(new Step(mainStepName.Single()), out Step existingStep))
                        {
                            existingStep.NonResolvedDependencies.Add(stepDependency);
                        }
                        else
                        {
                            steps.Add(new Step(mainStepName.Single(), stepDependency));
                        }
                        continue;
                    }
                }
            }
            return steps;
        }

        private class Step
        {
            public char Name { get; set; }

            public HashSet<Step> NonResolvedDependencies { get; set; }

            public Step(char name)
            {
                Name = name;
                NonResolvedDependencies = new HashSet<Step>();
            }

            public Step(char name, Step dependency)
            {
                Name = name;
                NonResolvedDependencies = new HashSet<Step> { dependency };
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }

                if (!(obj is Step))
                {
                    return false;
                }

                return Equals((Step)obj);
            }

            public bool Equals(Step other)
            {
                if (other == null)
                {
                    return false;
                }

                return Name == other.Name;
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }

            public static bool operator ==(Step step1, Step step2)
            {
                if (ReferenceEquals(step1, null))
                {
                    return ReferenceEquals(step2, null);
                }

                return step1.Equals(step2);
            }

            public static bool operator !=(Step step1, Step step2)
            {
                if (ReferenceEquals(step1, null))
                {
                    return !ReferenceEquals(step2, null);
                }

                return !step1.Equals(step2);
            }
        }
    }
}
