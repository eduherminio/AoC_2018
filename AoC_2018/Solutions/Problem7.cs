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

        public void Solve_2()
        {
            ICollection<Step> unorderedSteps = ParseInput();

            int result = ApplyPart2Algorithm(unorderedSteps);

            Console.Write($"Day 7, part 2: {result}");
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
                Step stepDependency = new Step(char.ToUpperInvariant(dependencyName.Single()));
                steps.Add(stepDependency);

                while (!parsedLine.Empty)
                {
                    string word = parsedLine.NextElement<string>();

                    if (word.ToUpperInvariant() == "STEP")
                    {
                        string mainStepName = parsedLine.NextElement<string>();

                        if (steps.TryGetValue(new Step(char.ToUpperInvariant(mainStepName.Single())), out Step existingStep))
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

        private int ApplyPart2Algorithm(ICollection<Step> unorderedSteps)
        {
            string result = string.Empty;
            const int totalAvailableWorkers = 5;     // Yourself and four Elves

            IDictionary<int, Step> workersTimetable = new Dictionary<int, Step>(totalAvailableWorkers)
            {
                { 1, null },
                { 2, null },
                { 3, null },
                { 4, null },
                { 5, null }
            };

            int secondsSpent = 0;

            while (unorderedSteps.Any())
            {
                ++secondsSpent;

                AssignWorkers(ref unorderedSteps, ref workersTimetable);

                SimulateOneSecond(ref result, ref unorderedSteps, ref workersTimetable);
            }

            return secondsSpent;
        }

        private void AssignWorkers(ref ICollection<Step> unorderedSteps, ref IDictionary<int, Step> workersTimetable)
        {
            List<int> availableWorkers = AvailableWorkers(workersTimetable).ToList();

            var assignableSteps = unorderedSteps
                .Where(step => !step.InProgressOrCompleted)
                .Where(step => !step.NonResolvedDependencies.Any())
                .OrderBy(step => step.Name)
                .ToList();

            var stepsToAssign = assignableSteps.GetRange(0, Math.Min(assignableSteps.Count, availableWorkers.Count()));

            for (int index = 0; index < stepsToAssign.Count(); ++index)
            {
                workersTimetable[availableWorkers.ElementAt(index)] = stepsToAssign.ElementAt(index);
            }
        }

        private void SimulateOneSecond(ref string result, ref ICollection<Step> unorderedSteps, ref IDictionary<int, Step> workersTimetable)
        {
            List<int> workersIdleAfterSimulation = new List<int>();

            foreach (var worker in workersTimetable.Where(pair => pair.Value != null))
            {
                Step step = unorderedSteps.Single(s => s == worker.Value);

                if (step.NonResolvedDependencies.Any())
                {
                    throw new Exception(
                        $"Exception in SimulateStep, step {step.Name} should not be assigned yet since it still has unresolved dependencies");
                }

                step.RunForOneSecond();

                if (step.Completed)
                {
                    unorderedSteps.Remove(step);

                    foreach (Step s in unorderedSteps)
                    {
                        s.NonResolvedDependencies.Remove(step);
                    }

                    workersIdleAfterSimulation.Add(worker.Key);

                    result += step.Name;
                }
            }

            foreach (int key in workersIdleAfterSimulation)
            {
                workersTimetable[key] = null;
            }
        }

        private IEnumerable<int> AvailableWorkers(IDictionary<int, Step> workersTimetable)
        {
            return workersTimetable.Where(pair => pair.Value == null).Select(tuple => tuple.Key);
        }

        private class Step
        {
            public char Name { get; set; }

            public HashSet<Step> NonResolvedDependencies { get; set; }

            public bool InProgressOrCompleted => _restingDuration != (Name - 4);

            public bool Completed => _restingDuration == 0;

            private int _restingDuration;

            public Step(char name)
            {
                Name = name;
                NonResolvedDependencies = new HashSet<Step>();
                _restingDuration = name - 4;
            }

            public Step(char name, Step dependency)
                : this(name)
            {
                NonResolvedDependencies = new HashSet<Step> { dependency };
            }

            public int RunForOneSecond()
            {
                return --_restingDuration;
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
