using System;
using System.Collections.Generic;
using System.IO;

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
        private static HashSet<int>[] _set;

        public static void Main(string[] args)
        {
            InitialiseStreams();
            var s = _reader.ReadLine();
            var n = ReadInt();
            _set = new HashSet<int>[s.Length];

            Node root = new Node(default(char));

            for (var i = 0; i < n; i++)
            {
                var word = _reader.ReadLine();
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


            if (Ok(s, 0, root))
            {
                _writer.WriteLine("YES");
            }
            else
            {
                _writer.WriteLine("NO");
            }

            CloseStreams();
        }


        private static bool Ok(string s, int start, Node root)
        {
            //_writer.WriteLine($"Processing substing: {s.Substring(start)}");
            if (start >= s.Length)
                return false;
            var ends = GetStringEnds(s, start, root);
            if (ends.Contains(s.Length - 1))
            {
                //_writer.WriteLine($"Found end! {s[s.Length - 1]}");
                return true;
            }
            else
            {
                foreach (var end in ends)
                {
                    var tempResult = Ok(s, end + 1, root);
                    if (tempResult)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static HashSet<int> GetStringEnds(string s, int start, Node root)
        {
            //_writer.WriteLine("\nPossible ends: ");
            if (_set[start] != null)
            {
                return _set[start];
            }
            var result = new HashSet<int>();
            var currentNode = root;
            for (int i = start; i < s.Length; i++)
            {
                if (currentNode.NextChars.ContainsKey(s[i]))
                {
                    currentNode = currentNode.NextChars[s[i]];
                    if (currentNode.IsTerminal)
                    {
                        //_writer.WriteLine($" {i} ");
                        result.Add(i);
                    }
                }
                else
                {
                    //_writer.WriteLine($"\nMove after {i} impossible\n");
                    return result;
                }
            }
            _set[start] = result;
            return result;

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
    }

}
