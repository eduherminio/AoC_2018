using FileParser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2018.Solutions
{
    public class Problem04: BaseProblem, IProblem
    {
        public void Solve_1()
        {
            List<Event> parsedEvents = ParseInput().ToList();

            ICollection<Event> orderedEvents = parsedEvents.OrderBy(e => e.DateTime).ToList();

            FillMissingGuardIds(ref orderedEvents);

            Dictionary<int, RelevantInfo> guardIdRelevantInfoDictionary = ExtractRelevantInfo(orderedEvents);
            ICollection<RelevantInfo> relevantInfo = guardIdRelevantInfoDictionary.Values;

            var relevantInfoFromGuardWithMoreAsleepTime = relevantInfo.OrderByDescending(info => info.SecondsAsleep).First();
            int sleepyGuardId = relevantInfoFromGuardWithMoreAsleepTime.GuardId;
            int mostAsleepMinut = ExtractMostAsleepMinute(relevantInfoFromGuardWithMoreAsleepTime);

            long result = sleepyGuardId * mostAsleepMinut;

            Console.Write($"Day 4, part 1: {result}");
        }

        public void Solve_2()
        {
            List<Event> parsedEvents = ParseInput().ToList();

            ICollection<Event> orderedEvents = parsedEvents.OrderBy(e => e.DateTime).ToList();
            FillMissingGuardIds(ref orderedEvents);

            Dictionary<int, RelevantInfo> guardIdRelevantInfoDictionary = ExtractRelevantInfo(orderedEvents);
            ICollection<RelevantInfo> relevantInfo = guardIdRelevantInfoDictionary.Values;

            var resultTuple = ExtractMostFrequentlyAsleepMinuteByAGuardAndAssociatedGuardId(relevantInfo);
            int mostFrequentlyAsleepMinuteByAGuard = resultTuple.Item1;
            int guardId = resultTuple.Item2;

            long result = mostFrequentlyAsleepMinuteByAGuard * guardId;

            Console.Write($"Day 4, part 2: {result}");
        }

        private ICollection<Event> ParseInput()
        {
            ICollection<Event> events = new List<Event>();

            IParsedFile parsedFile = new ParsedFile(FilePath);

            while (!parsedFile.Empty)
            {
                IParsedLine parsedLine = parsedFile.NextLine();

                string aux = string.Empty;
                while (!parsedLine.Empty)
                {
                    aux += $"{parsedLine.NextElement<string>()} ";
                }
                string line = aux
                    .Trim()
                    .Replace("  ", " ");

                Event evnt = ParseEvent(line);
                evnt.DateTime = ParseDateTime(line);

                events.Add(evnt);
            }

            return events;
        }

        private Event ParseEvent(string rawLine)
        {
            string eventPartString = rawLine.Substring(rawLine.IndexOf(']'))
                .Trim(new char[] { ' ', ']' });

            int guardId = 0;
            EventType eventType = default(EventType);

            if (eventPartString.Contains("wakes up"))
            {
                eventType = EventType.WakesUp;
            }
            else if (eventPartString.Contains("falls asleep"))
            {
                eventType = EventType.FallsAsleep;
            }
            else
            {
                eventType = EventType.BeginsShift;

                string guardIdString = eventPartString.Substring(eventPartString.IndexOf('#'), eventPartString.IndexOf(' '))
                    .Trim(new char[] { ' ', '#' });

                bool success = int.TryParse(guardIdString, out guardId);

                if (!(success && eventPartString.Contains("begins shift")))
                {
                    throw new Exception("Exception parsing event");
                }
            }

            return new Event(guardId, default(DateTime), eventType);
        }

        private DateTime ParseDateTime(string rawLine)
        {
            string dateTimeString = rawLine.Substring(rawLine.IndexOf('['), rawLine.IndexOf(']') - rawLine.IndexOf('['))
                .Trim(new char[] { '[', ']' });

            DateTime.TryParse(dateTimeString, out DateTime dateTime);
            return dateTime;
        }

        private void FillMissingGuardIds(ref ICollection<Event> orderedEvents)
        {
            for (int index = 0; index < orderedEvents.Count; ++index)
            {
                Event evnt = orderedEvents.ElementAt(index);

                if (evnt.EventType != EventType.BeginsShift)
                {
                    ExtractGuardId(ref orderedEvents, index);
                }

                if (evnt.GuardId == default(int))
                {
                    throw new Exception("Badly inferred event's GuardId");
                }
            }
        }

        private void ExtractGuardId(ref ICollection<Event> orderedEvents, int eventIndex)
        {
            for (int backwardsEventIndex = 1; backwardsEventIndex <= eventIndex; ++backwardsEventIndex)
            {
                Event previousEvent = orderedEvents.ElementAt(eventIndex - backwardsEventIndex);
                if (previousEvent.EventType == EventType.BeginsShift)
                {
                    orderedEvents.ElementAt(eventIndex).GuardId = previousEvent.GuardId;
                    break;
                }
            }
        }

        private Dictionary<int, RelevantInfo> ExtractRelevantInfo(ICollection<Event> orderedEvents)
        {
            Dictionary<int, RelevantInfo> guardIdRelevantInfoDictionary = new Dictionary<int, RelevantInfo>();

            foreach (var grouping in orderedEvents.GroupBy(e => e.GuardId))
            {
                for (int eventIndex = 0; eventIndex < grouping.Count(); ++eventIndex)
                {
                    Event evnt = grouping.ElementAt(eventIndex);
                    if (evnt.EventType == EventType.WakesUp)
                    {
                        for (int backwardsEventIndex = 1; backwardsEventIndex <= eventIndex; ++backwardsEventIndex)
                        {
                            Event previousEvent = grouping.ElementAt(eventIndex - backwardsEventIndex);
                            if (previousEvent.EventType == EventType.FallsAsleep)
                            {
                                TimeSpan asleepTime = (evnt.DateTime - previousEvent.DateTime);
                                List<int> minutesAsleep = Enumerable.Range(previousEvent.DateTime.Minute, (int)asleepTime.TotalMinutes).ToList();

                                if (guardIdRelevantInfoDictionary.Keys.Contains(grouping.Key))
                                {
                                    guardIdRelevantInfoDictionary[grouping.Key].SecondsAsleep += asleepTime.TotalSeconds;
                                    minutesAsleep.ForEach(min =>
                                    {
                                        if (guardIdRelevantInfoDictionary[grouping.Key].MinuteTimesAsleepDictionary.Keys.Contains(min))
                                        {
                                            ++guardIdRelevantInfoDictionary[grouping.Key].MinuteTimesAsleepDictionary[min];
                                        }
                                        else
                                        {
                                            guardIdRelevantInfoDictionary[grouping.Key].MinuteTimesAsleepDictionary.Add(min, value: 1);
                                        }
                                    });
                                }
                                else
                                {
                                    guardIdRelevantInfoDictionary.Add(grouping.Key, new RelevantInfo()
                                    {
                                        GuardId = grouping.Key,
                                        SecondsAsleep = asleepTime.TotalSeconds,
                                        MinuteTimesAsleepDictionary = new Dictionary<int, int>() { }
                                    });

                                    minutesAsleep.ForEach(min =>
                                        guardIdRelevantInfoDictionary[grouping.Key].MinuteTimesAsleepDictionary.TryAdd(min, value: 1));
                                }
                                break;
                            }
                        }
                    }
                }
            }

            return guardIdRelevantInfoDictionary;
        }

        private int ExtractMostAsleepMinute(RelevantInfo relevantInfo)
        {
            var orderedMinuteTimesAsleepDictionary = relevantInfo.MinuteTimesAsleepDictionary.OrderByDescending(pair => pair.Value);

            return orderedMinuteTimesAsleepDictionary.First().Key;
        }

        private Tuple<int, int> ExtractMostFrequentlyAsleepMinuteByAGuardAndAssociatedGuardId(ICollection<RelevantInfo> relevantInfo)
        {
            Dictionary<int, Tuple<int, int>> guardId_minute_timesAsleepInThatMinute = new Dictionary<int, Tuple<int, int>>();

            foreach (RelevantInfo info in relevantInfo)
            {
                var orderedMinuteTimesDictionary = info.MinuteTimesAsleepDictionary.OrderByDescending(pair => pair.Value);

                guardId_minute_timesAsleepInThatMinute.Add(info.GuardId, Tuple.Create(orderedMinuteTimesDictionary.First().Key, orderedMinuteTimesDictionary.First().Value));
            }

            var orderedDictionary = guardId_minute_timesAsleepInThatMinute.OrderByDescending(pair => pair.Value.Item2);

            return Tuple.Create(orderedDictionary.First().Value.Item1, orderedDictionary.First().Key);

        }

        private enum EventType
        {
            BeginsShift,
            FallsAsleep,
            WakesUp
        }

        private class Event
        {
            public int GuardId { get; set; }

            public DateTime DateTime { get; set; }

            public EventType EventType { get; set; }

            public Event(int guardId, DateTime dateTime, EventType eventType)
            {
                GuardId = guardId;
                DateTime = dateTime;
                EventType = eventType;
            }
        }

        private class RelevantInfo
        {
            public int GuardId { get; set; }

            public double SecondsAsleep { get; set; }

            public Dictionary<int, int> MinuteTimesAsleepDictionary { get; set; }
        }
    }
}
