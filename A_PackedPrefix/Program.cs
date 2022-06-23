﻿//https://contest.yandex.ru/contest/26133/run-report/69128051/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace A_PackedPrefix
{
    public class Node
    {
        public char Value { get; set; }
        public Dictionary<char,Node> NextChars { get; set; } = new Dictionary<char,Node>();

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

            Node root = new Node(default(char));


            var n = ReadInt();

            bool goOn = true;
            for (int i = 0; i < n && goOn; i++)
            {
                if (root.NextChars.Count > 1)
                {
                    goOn = false;
                }
                else
                {
                    var s = _reader.ReadLine();
                    var decodedString = f(s);
                    var currentNode = root;
                    for (int j = 0; j < decodedString.Length; j++)
                    {
                        if (currentNode.NextChars.ContainsKey(decodedString[j]))
                        {
                            currentNode = currentNode.NextChars[decodedString[j]];
                        }
                        else
                        {
                            var tempNode = new Node(decodedString[j]);
                            currentNode.NextChars[decodedString[j]] = tempNode;
                            currentNode = tempNode;
                            if (i != 0)
                            {
                                break;
                            }
                        }
                    }
                }
                
            }

            while (root.NextChars.Count == 1)
            {
                char ch = root.NextChars.Keys.First();
                _writer.Write(ch);
                root = root.NextChars[ch];
            }
            _writer.WriteLine();

            CloseStreams();
        }

        private static string f(string s)
        {
            Stack<char> stack = new Stack<char>();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ']')
                {
                    List<char> decodedString = new List<char>();
                    while (stack.Peek() != '[')
                    {
                        decodedString.Add(stack.Pop());
                    }
                    stack.Pop();
                    int baseNum = 1;
                    int k = 0;
                    while (stack.Count > 0 && char.IsDigit(stack.Peek()))
                    {
                        k = k + (stack.Pop() - '0') * baseNum;
                        baseNum *= 10;
                    }
                    while (k != 0)
                    {
                        for (int j = decodedString.Count - 1; j >= 0; j--)
                        {
                            stack.Push(decodedString[j]);
                        }
                        k--;
                    }
                }
                else
                {
                    stack.Push(s[i]);
                }
            }

            char[] result = new char[stack.Count];
            for (int i = result.Length - 1; i >= 0; i--)
            {
                result[i] = stack.Pop();
            }

            return new string(result);
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