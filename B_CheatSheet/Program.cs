// https://contest.yandex.ru/contest/26133/run-report/69152546/
//

// Спасибо за замечания, постарался все участь.
// С рекурсией чуть быстрее алгоритм получился:
// https://contest.yandex.ru/contest/26133/run-report/69167205/
// Зато с помощью динамического программирования меньше памяти расходуется:
// https://contest.yandex.ru/contest/26133/run-report/69167412/

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
        public bool IsTerminal { get; set; } = false;
        public Dictionary<char, Node> NextChars { get; set; } = new Dictionary<char, Node>();
    }


    public class Solution
    {
        private static TextReader _reader;
        private static TextWriter _writer;
        private static bool[] dp;

        public static void Main(string[] args)
        {
            InitialiseStreams();
            var s = _reader.ReadLine();
            var n = ReadInt();
            dp = new bool[s.Length];

            Node root = new Node();

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
                        var tempNode = new Node();
                        currentNode.NextChars[word[j]] = tempNode;
                        currentNode = tempNode;
                    }
                }
                currentNode.IsTerminal = true;
            }


            if (Ok(s, root))
            {
                _writer.WriteLine("YES");
            }
            else
            {
                _writer.WriteLine("NO");
            }

            CloseStreams();
        }


        private static bool Ok(string s, Node root)
        {
            var ends = GetStringEnds(s, 0, root);
            foreach (var end in ends)
            {
                if (end < s.Length)
                {
                    dp[end] = true;
                }
            }
            for (int i = 0; i < s.Length; i++)
            {
                if (dp[i])
                {
                    ends = GetStringEnds(s, i+1, root);
                    foreach (var end in ends)
                    {
                        if (end < s.Length)
                        {
                            dp[end] = true;
                        }
                    }
                }
                
            }
            return dp[s.Length - 1];
        }

        private static List<int> GetStringEnds(string s, int start, Node root)
        {
            var result = new List<int>();
            var currentNode = root;
            for (int i = start; i < s.Length; i++)
            {
                if (currentNode.NextChars.ContainsKey(s[i]))
                {
                    currentNode = currentNode.NextChars[s[i]];
                    if (currentNode.IsTerminal)
                    {
                        result.Add(i);
                        if (i == s.Length - 1)
                        {
                            return result;
                        }
                    }
                }
                else
                {
                    return result;
                }
            }
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
