using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScrabbleRelease
{
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
        /*
        public Tuple<List<string>, List<int>> SortByAlpha(List<string> words, List<int> points)
        {
            List<string> OutWords = new List<string>();
            List<int> OutPoints = new List<int>();
            List<string> TempWords = new List<string>();
            List<int> TempPoints = new List<int>();
            for (int indexOfLength = 0; indexOfLength < 7; indexOfLength++)
            {
                for (int l = (int)'A'; l <= (int)'Z'; l++)
                {
                    for (int i = 0; i < words.Count; i++)
                    {
                        if (OutWords[i].Length < indexOfLength) { break; }
                        if (words[i][indexOfLength] == l)
                        {
                            OutWords.Add(words[i]);
                            OutPoints.Add(points[i]);
                        }
                        if (words[i][0] == l + 32)
                        {
                            OutWords.Add(words[i]);
                            OutPoints.Add(points[i]);
                        }
                    }
                }
                for (int length = 7; length > 0; length--)
                {
                    //if it's A add it to 
                }
            }
            return new Tuple<List<string>, List<int>>(OutWords, OutPoints);
        }

        //if the first letter is a; add it to a.
        
        public Tuple<List<string>, List<int>> SortByAlphaInv(List<string> words, List<int> points)
        {
            List<string> OutWords = new List<string>();
            List<int> OutPoints = new List<int>();

            //for (int length = 1; length <= 7; length++)
            for (int l = (int)'Z'; l >= (int)'A'; l--)
            {
                for (int i = 0; i < words.Count; i++)
                {
                    if (words[i][0] == l)
                    {
                        OutWords.Add(words[i]);
                        OutPoints.Add(points[i]);
                    }
                }
            }
            return new Tuple<List<string>, List<int>>(OutWords, OutPoints);
        }*/
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
