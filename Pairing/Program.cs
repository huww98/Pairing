using System;
using System.Collections.Generic;
using System.Linq;

namespace Pairing
{
    class Group
    {
        public int ID { get; set; }
        public List<int> WantedSubject { get; set; }

        public static Group BuildFromInput(string line)
        {
            var cells = line.Split(' ').Select(s => int.Parse(s));
            Group group = new Group
            {
                ID = cells.First(),
                WantedSubject = new List<int>(cells.Skip(1))
            };
            return group;
        }
    }

    class Program
    {
        public static void Shuffle<T>(IList<T> list, Random random)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        static int SubjectsCount = 20;
        static void Main(string[] args)
        {
            Console.Write("seed: ");
            int seed;
            unchecked
            { 
                seed = (int)long.Parse(Console.ReadLine());
            }
            Random random = new Random(seed);

            Console.WriteLine("Input data: [groud ID] [subject ID] ...");
            List<Group> groups = new List<Group>();
            for (int i = 0; i < SubjectsCount; i++)
            {
                var line = Console.ReadLine();
                groups.Add(Group.BuildFromInput(line));
            }

            List<int> availableSubjects = new List<int>();
            for (int j = 1; j <= SubjectsCount; j++)
            {
                availableSubjects.Add(j);
            }

            for (int i = 0; i < SubjectsCount; i++)
            {
                Dictionary<int, List<Group>> wantingGroups = new Dictionary<int, List<Group>>();
                foreach (var j in availableSubjects)
                {
                    wantingGroups.Add(j, new List<Group>());
                }
                foreach (var g in groups)
                {
                    if(g.WantedSubject.Count > 0)
                    {
                        wantingGroups[g.WantedSubject[0]].Add(g);
                    }
                }
                foreach (var p in wantingGroups)
                {
                    if (p.Value.Count > 0)
                    {
                        int chosenGroupIndex = random.Next(p.Value.Count);
                        Group chosenGroup = p.Value[chosenGroupIndex];
                        Console.WriteLine($"Group {chosenGroup.ID,2} got subject {p.Key,2}");
                        groups.Remove(chosenGroup);
                        availableSubjects.Remove(p.Key);
                        foreach (var g in groups)
                        {
                            g.WantedSubject.Remove(p.Key);
                        }
                    }
                }
            }

            // 处理没人选的subjects
            Shuffle(availableSubjects, random);
            for (int i = 0; i < availableSubjects.Count; i++)
            {
                Console.WriteLine($"Group {groups[i].ID,2} got subject {availableSubjects[i],2}");
            }

            Console.ReadLine();
        }
    }
}
