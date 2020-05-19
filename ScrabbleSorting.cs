using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace ScrabbleRelease
{
    public class Letter1FirstAll : Comparer<string>
    {
        public override int Compare(string x, string y)
        {
            x = x.ToUpper();
            y = y.ToUpper();
            int minLength = x.Length;
            if (y.Length < minLength) { minLength = y.Length; }

            for (int i = 0; i < minLength; i++)
            {
                if (x[i].CompareTo(y[i]) != 0)//returns if the letters arent the same
                {
                    return x[i].CompareTo(y[i]);
                }
            }
            return 0;
        }
    }

    class ScrabbleSorting
    {
        private string[] unsortedWords;
        
        public ScrabbleSorting(string[] input)
        {
            unsortedWords = input;
        }

        /// <summary>
        /// Sorts words using a selection sort.
        /// (Highest points to lowest points)
        /// </summary>
        /// <param name="words"></param>
        /// <param name="points"></param>
        /// <returns> A tuple containing both lists</returns>
        public Tuple<List<string>, List<int>> SortByPoints(List<string> words, List<int> points)
        {
            List<string> OutWords = new List<string>();
            List<int> OutPoints = new List<int>();
            int max = 0;
            foreach (int p in points)
            {
                if (p > max) { max = p; }
            }
            for (; max >= 0; max--)
            {
                for (int i = 0; i < words.Count; i++)
                {
                    if (points[i] == max)
                    {
                        OutWords.Add(words[i]);
                        OutPoints.Add(points[i]);
                    }
                }

            }
            return new Tuple<List<string>, List<int>>(OutWords , OutPoints);
        }

        /// <summary>
        /// Sorts words using a selection sort.
        /// (Lowest points to highest points)
        /// </summary>
        /// <param name="words"></param>
        /// <param name="points"></param>
        /// <returns> A tuple containing both lists</returns>
        public Tuple<List<string>, List<int>> SortByPointsInv(List<string> words, List<int> points)
        {
            List<string> OutWords = new List<string>();
            List<int> OutPoints = new List<int>();
            int max = 0;
            foreach (int p in points)
            {
                if (p > max) { max = p; }
            }
            for (int min = 0; min <= max; min++)
            {
                for (int i = 0; i < words.Count; i++)
                {
                    if (points[i] == min)
                    {
                        OutWords.Add(words[i]);
                        OutPoints.Add(points[i]);
                    }
                }
            }
            return new Tuple<List<string>, List<int>>(OutWords, OutPoints);
        }
        
        public Tuple<List<string>, List<int>> SortByAlpha(List<string> words, List<int> points)
        {
            List<string> OutWords;
            List<int> OutPoints = new List<int>();
            Dictionary<string, int> wordPoints = new Dictionary<string, int>();

            for (int i = 0; i < words.Count; i++)
            {
                wordPoints.Add(words[i], points[i]);
            }
            /*
            Console.WriteLine("Word,\t points");
            Console.WriteLine("In dictionary");
            Console.WriteLine("===============");
            foreach (string s in wordPoints.Keys)
            {
                int p = -1;
                wordPoints.TryGetValue(s, out p);
                Console.WriteLine("{0}\t{1}", s, p);
            }*/

            //Console.WriteLine("Word,\t points");
            //Console.WriteLine("Dictionary sorted Letter1FirstAll");
            //Console.WriteLine("===============");
            OutWords = wordPoints.Keys.ToList();
            //OutWords.Sort(new Letter1FirstAll());//Compares and sorts based on all chars in the string x
            OutWords.Sort();
            foreach (string w in OutWords)
            {
                //Console.WriteLine("{0}\t{1}", w, wordPoints[w]);
                OutPoints.Add(wordPoints[w]);
            }

            return new Tuple<List<string>, List<int>>(OutWords, OutPoints);
        }
        
        public Tuple<List<string>, List<int>> SortByAlphaInv(List<string> words, List<int> points)
        {
            List<string> OutWords = new List<string>();
            List<int> OutPoints = new List<int>();

            //pair up the points and words.
            Dictionary<string, int> wordPoints = new Dictionary<string, int>();
            for (int i = 0; i < words.Count; i++) { wordPoints.Add(words[i], points[i]); }

            List<string> tempWords = wordPoints.Keys.ToList();
            tempWords.Sort();

            for (int i = tempWords.Count-1; i >= 0; i--)
            {
                OutWords.Add(tempWords[i]);
                OutPoints.Add(wordPoints[tempWords[i]]);
            }

            return new Tuple<List<string>, List<int>>(OutWords, OutPoints);
        }

        public string outputString(string[] input)
        {
            string output = "";
            foreach(string s in input)
            {
                output += s + Environment.NewLine;
            }
            return output;
        }
    }
}
