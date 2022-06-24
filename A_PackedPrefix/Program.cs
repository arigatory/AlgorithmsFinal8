//https://contest.yandex.ru/contest/26133/run-report/69128051/
//https://contest.yandex.ru/contest/26133/run-report/69167145/

//  Комменарий из ревью:
//ок, но здесь вроде можно и без бора если не хранить все строки а только 1 с которой сравниваешь текущую.
// Ответ:
// Спасибо за проверку и замечания. Я на самом деле храню не все строки, а только первую. Все следующие я начинаю помещать в бор,
// но останавливаюсь, когда начинается ветвление. То есть как бы отсекаю остаток. Немного подправил, чтобы и первую строку обрезать,
// после расхождения при сравнении. Ссылку на новую посылку во второй строке разместил.

/*
-- ПРИНЦИП РАБОТЫ --
Сначала распаковываем строку. Это функция f. Затем вносим распакованные строки в бор.
Ответом будет строка, получаемая из бора путем перемещения из корня вниз только тогда, когда
перемещаться можем только по 1 ребру. Это и означает общий префикс.


-- ДОКАЗАТЕЛЬСТВО КОРРЕКТНОСТИ --
Слова, начинающиеся одинаково, имеют общий префикс. Значит в боре, будет только одно ребро. Если имеем 2 ребра, это
означает, что слова в данной позиции различаются.


-- ВРЕМЕННАЯ СЛОЖНОСТЬ --
O(∣Σ∣log∣Σ∣), где ∣Σ∣ — размер алфавита. 
Это классическая сложность алгоритма построения бора по заданному набору слов.


-- ПРОСТРАНСТВЕННАЯ СЛОЖНОСТЬ --
Для хранения бора при большом размере алфавита требуется много памяти. В условии нет ограничений на алфавит, 
поэтому невозможно точно посчитать.

*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace A_PackedPrefix
{
    public class Node
    {
        public char Value { get; set; }
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
                        else if (i != 0)
                        {
                            currentNode.NextChars = new Dictionary<char, Node>();
                            break;
                        }
                        else
                        {
                            var tempNode = new Node(decodedString[j]);
                            currentNode.NextChars[decodedString[j]] = tempNode;
                            currentNode = tempNode;
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
