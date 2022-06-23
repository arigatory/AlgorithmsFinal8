// https://contest.yandex.ru/contest/26133/run-report/69152546/
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
        private static List<int>[] _possibleEnds;
        private static bool?[] _cache;
        private static bool _foundCorrectBreak = false;

        public static void Main(string[] args)
        {
            InitialiseStreams();
            var s = _reader.ReadLine();
            var n = ReadInt();
            _possibleEnds = new List<int>[s.Length];
            _cache = new bool?[s.Length];

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
            if (_foundCorrectBreak)
                return true;
            if (_cache[start].HasValue)
            {
                return _cache[start].Value;
            }
            if (start >= s.Length)
                return false;
            var ends = GetStringEnds(s, start, root);

            foreach (var end in ends)
            {
                var tempResult = Ok(s, end + 1, root);
                if (tempResult)
                {
                    _cache[start] = true;
                    return true;
                }
            }

            _cache[start] = false;
            return false;
        }

        private static List<int> GetStringEnds(string s, int start, Node root)
        {
            if (_possibleEnds[start] != null)
            {
                return _possibleEnds[start];
            }
            var result = new List<int>();
            var currentNode = root;
            for (int i = start; i < s.Length; i++)
            {
                if (currentNode.NextChars.ContainsKey(s[i]))
                {
                    currentNode = currentNode.NextChars[s[i]];
                    if (currentNode.IsTerminal)
                    {
                        _possibleEnds[start] = result;
                        result.Add(i);
                        if (i == s.Length - 1)
                        {
                            _foundCorrectBreak = true;
                            return result;
                        }
                    }
                }
                else
                {
                    return result;
                }
            }
            _possibleEnds[start] = result;
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
