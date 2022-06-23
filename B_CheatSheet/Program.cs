// https://contest.yandex.ru/contest/26133/run-report/69152546/

/*
-- ПРИНЦИП РАБОТЫ --
Я записал шаблоны слов в структуру бор. Далее иду по строке, которую хотим разбить на слова.
При прохождении мы спускаемся одновременно по бору и можем достигнуть конец (терминальный символ).
Если это так, то начинаем снова спускаться по бору, но уже с того индекса строки, на котором остановились.


-- ДОКАЗАТЕЛЬСТВО КОРРЕКТНОСТИ --
Из описания алгоритма следует, что мы проходим строку один раз, причем только тогда, когда в боре есть
соответствующий переход, что означает, что мы идем по исходной строке только если имеется данное слово в словаре.


-- ВРЕМЕННАЯ СЛОЖНОСТЬ --
Пусть длина исходной строки n. Мы проходим строку 1 раз. На каждом шаге мы спускаемся по бору, это O(1)*k,
где k - количество слов в словаре. Построение бора занимает S, где S - сумма всех символов в словаре.
Допустим, в среднем слова, на которые мы разбиваем исходную строку равны n/2, тогда мы выполняем O(n) операций.
Плюс при проверке окончаний слов из словаря в худшем случае у нас будет n окончаний, тогда итоговую сложность
получаем O(n*n/2*n) = O(n^3)


-- ПРОСТРАНСТВЕННАЯ СЛОЖНОСТЬ --
Храним дополнительно 2 массива длины строки - n. Значит, требуется дополнительно памяти O(n)


*/

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
