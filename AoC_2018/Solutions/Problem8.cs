using AoC_2018.Model;
using FileParser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC_2018.Solutions
{
    public class Problem8 : IProblem
    {
        public string FilePath => Path.Combine("Inputs", "8.in");

        private HashSet<MetadataNode> _nodes = new HashSet<MetadataNode>();
        Stack<int> _intStack;

        public void Solve_1()
        {
            ParseInput();

            long checkSum = 0;

            foreach (int metadata in _nodes.SelectMany(node => node.Metadata))
            {
                checkSum += metadata;
            }

            Console.Write($"Day 8, part 1: {checkSum}");
        }

        public void Solve_2()
        {
            ParseInput();

            int rootValue = ApplyPart2Algorithm();

            Console.Write($"Day 8, part 1: {rootValue}");
        }

        private void ParseInput()
        {
            IParsedFile parsedFile = new ParsedFile(FilePath);

            Stack<int> reversedStack = new Stack<int>(parsedFile.ToList<int>());
            _intStack = new Stack<int>(reversedStack);

            if (!parsedFile.Empty)
            {
                throw new Exception("Assumption of 1-line file is wrong");
            }

            ParseNodeTree();
        }

        private static int _id = -1;

        private void ParseNodeTree()
        {
            while (_intStack.Any())
            {
                ParseNode();
            }
        }

        private int ApplyPart2Algorithm()
        {
            throw new NotImplementedException();
        }

        private void ParseNode()
        {
            ++_id;
            Console.WriteLine(_id);

            MetadataNode node = new MetadataNode(_id) { ParentId = _id - 1 };

            int nChildNodes = _intStack.Pop();
            int nMetadataEntries = _intStack.Pop();

            for (int i = 0; i < nChildNodes; ++i)
            {
                ParseNode();
            }

            for (int i = 0; i < nMetadataEntries; ++i)
            {
                node.Metadata.Add(_intStack.Pop());
            }

            _nodes.Add(node);
        }

        private class MetadataNode : Node<int>
        {
            public ICollection<int> Metadata { get; set; } = new List<int>();

            public MetadataNode(int id) : base(id) { }
        }
    }
}
