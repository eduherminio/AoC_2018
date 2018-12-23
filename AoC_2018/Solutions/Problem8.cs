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

            long checkSum = _nodes.SelectMany(node => node.Metadata).Sum();

            Console.Write($"Day 8, part 2: {checkSum}");
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
                ParseNode(-1);
            }
        }

        private void ParseNode(int parentId)
        {
            ++_id;

            MetadataNode node = new MetadataNode(_id) { ParentId = parentId };

            int nChildNodes = _intStack.Pop();
            int nMetadataEntries = _intStack.Pop();

            for (int i = 0; i < nChildNodes; ++i)
            {
                ParseNode(node.Id);
            }

            for (int i = 0; i < nMetadataEntries; ++i)
            {
                node.Metadata.Add(_intStack.Pop());
            }

            _nodes.Add(node);
        }

        private readonly static IDictionary<MetadataNode, int> _nodeValuePair = new Dictionary<MetadataNode, int>();

        private int ApplyPart2Algorithm()
        {
            MetadataNode rootNode = _nodes.Single(n => n.Id == 0 && n.ParentId == -1);

            return CalculateNodeValue(rootNode);
        }

        private int CalculateNodeValue(MetadataNode node)
        {
            if (_nodeValuePair.TryGetValue(node, out int value))
            {
                return value;
            }

            var children = _nodes.Where(n => n.ParentId == node.Id).ToList();

            if (children.Any())
            {
                foreach (int metadata in node.Metadata)
                {
                    if (metadata > 0 && metadata <= children.Count)
                    {
                        value += CalculateNodeValue(children.ElementAt(metadata - 1));
                    }
                }
            }
            else
            {
                value = node.Metadata.Sum(_ => _);
            }

            _nodeValuePair.Add(node, value);
            return value;
        }

        private class MetadataNode : Node<int>
        {
            public ICollection<int> Metadata { get; set; } = new List<int>();

            public MetadataNode(int id) : base(id) { }
        }
    }
}
