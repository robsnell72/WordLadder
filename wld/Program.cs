using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wld
{
    class Program
    {
        class Node
        {
            public int step { get; set; }
            public string word { get; set; }
            public string stopWord { get; set; }
            public List<string> wordList { get; set; }
            public List<Node> children { get; set; }
            public Node parent { get; set; }

            public Node(string word, string stopWord, string[] wordList, int step, Node parent)
            {
                this.word = word;
                this.stopWord = stopWord;
                this.step = step;
                this.wordList = wordList.ToList();
                this.children = new List<Node>();
                this.parent = parent;
            }
        }

        static int wordLadder(string beginWord, string endWord, string[] wordList)
        {
            List<Node> nodes = new List<Node>();
            Stack<Node> ends = new Stack<Node>();
            Node root = new Node(beginWord, endWord, wordList, 1, null);
            Process(root, ends);
            Node shortestRouteEnd = null;
            int shortestStep = 0;
                        
            while(ends.Count > 0)
            {
                Node node = ends.Pop();
                Stack<string> breadcrumbs = new Stack<string>();

                if (shortestRouteEnd == null || (node.step < shortestRouteEnd.step))
                {
                    shortestRouteEnd = node;
                    shortestStep = node.step;
                }

                while(true)
                {
                    breadcrumbs.Push(node.word);

                    if (node.parent == null)
                    {
                        break;
                    }
                    else
                    {
                        node = node.parent;
                    }
                }

                StringBuilder line = new StringBuilder();
                
                while(breadcrumbs.Count > 0)
                {
                    if (breadcrumbs.Count == 1)
                    {
                        line.Append($"{breadcrumbs.Pop()}");
                    }
                    else
                    {
                        line.Append($"{breadcrumbs.Pop()} -> ");
                    }
                }

                Console.WriteLine(line);
            }

            Console.WriteLine($"Shortest route {shortestStep}.");

            return shortestStep;
        }

        private static void Process(Node node, Stack<Node> ends)
        {
            string[] nextSteps = GetEligibleNextSteps(node.word, node.wordList.ToArray());

            //how recursion ends
            //you have reached the goal
            if (node.stopWord == node.word)
            {
                ends.Push(node);
                return;
            }
            
            //there are no eligible words to jump to on this chain
            if (nextSteps.Length == 0)
            {
                return;
            }

            foreach (string word in nextSteps)
            {
                List<string> newWordList = new List<string>(node.wordList);
                newWordList.Remove(word);

                Node newNode = new wld.Program.Node(word, node.stopWord, newWordList.ToArray(), node.step + 1, node);
                node.children.Add(newNode);
                Process(newNode, ends);
            }
        }

        private static string[] GetEligibleNextSteps(string word, string[] wordList)
        {
            List<string> result = new List<string>();

            foreach (string nextStep in wordList)
            {
                char[] wordc = word.ToCharArray();
                char[] nextStepc = nextStep.ToCharArray();
                int charMismatches = 0;

                for(int i=0;i<nextStep.Length;i++)
                {
                    if (wordc[i] != nextStep[i])
                    {
                        charMismatches++;
                    }
                }

                if (charMismatches == 1)
                {
                    result.Add(nextStep);
                }
            }

            return result.ToArray();
        }

        static void Main(string[] args)
        {
            int value = wordLadder("hit", "cog", new string[] { "hot", "dot", "dog", "lot", "log", "cog" });

            Console.ReadLine();
        }
    }
}
