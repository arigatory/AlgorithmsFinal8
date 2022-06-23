using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace B_CheatSheet
{
    public class Node
    {
        public char Value { get; set; }
        public bool IsTerminal { get; set; } = false;
        public Dictionary<char, Node> NextChars { get; set; } = new Dictionary<char, Node>();

        public Node(Char ch)
        {
            Value = ch;
        }
    }


    public class Solution
    {
        private static TextReader _reader;
        private static TextWriter _writer;

        public static void Main(string[] args)
        {
            InitialiseStreams();
            var s = _reader.ReadLine();
            var n = ReadInt();
 
            Node root = new Node(default(char));

            for (var i = 0; i < n; i++)
            {
                var word =_reader.ReadLine();
                var currentNode = root;
                for (int j = 0; j < word.Length; j++)
                {
                    if (currentNode.NextChars.ContainsKey(word[j]))
                    {
                        currentNode = currentNode.NextChars[word[j]];
                    }
                    else
                    {
                        var tempNode = new Node(word[j]);
                        currentNode.NextChars[word[j]] = tempNode;
                        currentNode = tempNode;
                    }
                }
                currentNode.IsTerminal = true;
            }

            _writer.WriteLine("YES");

            CloseStreams();
        }

        private static void CloseStreams()
        {
            _reader.Close();
            _writer.Close();
        }

        private static void InitialiseStreams()
        {
            _reader = new StreamReader(Console.OpenStandardInput());
            _writer = new StreamWriter(Console.OpenStandardOutput());
        }

        private static int ReadInt()
        {
            return int.Parse(_reader.ReadLine());
        }

        private static List<int> ReadList()
        {
            return _reader.ReadLine()
                .Split(new[] { ' ', '\t', }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
        }
    }

}
