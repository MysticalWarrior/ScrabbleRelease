/// Aidan McClung
/// May 15, 2020
/// A way to sort through words and output any that can be played with scrabble letters.
/// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace ScrabbleRelease
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DateTime timerStart;
        private char[] _playerHand;
        /// <summary>
        /// The 7 characters in the player's hand.
        /// Setting the value will trigger an update of the matching words.
        /// </summary>
        public char[] playerHand
        {
            get { return _playerHand; }
            set
            {
                if (validWords != null)
                {
                    timerStart = DateTime.Now;
                    _playerHand = value;
                    updateWords();
                    //MessageBox.Show("updateWords() time: " + (DateTime.Now - timerStart).TotalMilliseconds);
                }
            }
        }
        private ScrabbleGame sg;
        private ScrabbleSorting ss;
        public string[] validWords;
        public List<String> wordsOutput;
        public List<int> wordsPoints;
        public Point outputCapacity;
        public int nextToDisplay;

        public MainWindow()
        {
            InitializeComponent();
            outputCapacity = new Point(9, 20);//The x and y of point are vertical and horizontal capacity.
            playerHand = new char[7];

            ///Sets the dictionary of valid words
            validWords = pullWordsFromWeb();
            ss = new ScrabbleSorting(validWords);
            
            ///Starts program with Tiles (and triggers an update by setting it here)
            cbInputMode.SelectedItem = cbTiles;
        }

        /// <summary>
        /// Stores any word less than 7 characters from the given source
        /// (darcy.rsgc.on.ca/ACES/ICS4U/SourceCode/Words.txt)
        /// </summary>
        /// <returns>An array of the words it stored.</returns>
        private string[] pullWordsFromWeb()
        {
            List<string> Words = new List<string>();
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                System.IO.StreamReader sr = new System.IO.StreamReader(wc.OpenRead("http://darcy.rsgc.on.ca/ACES/ICS4U/SourceCode/Words.txt"));
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    //filters out words that are more than 7 characters long
                    if (line.Length <= 7) { Words.Add(line); }
                }
                sr.Close();
            }
            catch (Exception) { MessageBox.Show("Error initializing dictionary"); }
            return Words.ToArray();
        }

        /// <summary>
        /// Checks if all characters in a word can be removed by charcters in the players' hand.
        /// </summary>
        /// <param name="wordtocheck"></param>
        /// <returns> Returns true if the word can be played with the current tiles.</returns>
        private int checkWord(string wordtocheck)
        {
            string word = wordtocheck.ToUpper();///ensures playerhand and the word's cases match.
            int numberRemovedByWild = 0;
            int points = 0;
            for (int i = 0; i < 7; i++)
            {
                if (word.Length == 0) { break; }
                else if (word.Contains(playerHand[i]))
                {
                    word = word.Remove(word.IndexOf(playerHand[i]), 1);
                    points += ScrabbleLetter.HowManyPoints(playerHand[i]);
                }
                else if (playerHand[i] == ' ')
                {
                    numberRemovedByWild++;
                }
            }
            if (word == "") { return points; }
            else if (word.Length <= numberRemovedByWild) { return points; }
            else { return 0; }
        }

        /// <summary>
        /// Changes playerHand if the text is 7 characters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtInput.Text.Length == 7)
            {
                changePlayerHand(txtInput.Text);//calls a seperate method so the tiles can also change it
            }
        }

        /// <summary>
        /// Changes the playerHand array to the input
        /// </summary>
        /// <param name="input">The string to be converted to a character array.</param>
        /// 
        private void changePlayerHand(string input)
        {
            playerHand = input.ToUpper().ToCharArray();
        }

        /// <summary>
        /// Updates the list of words that can be played by checking each word.
        /// Checks using the CheckWord method.
        /// </summary>
        private void updateWords()
        {
            //resets the list of checked words
            wordsOutput = new List<string>();
            wordsPoints = new List<int>();

            nextToDisplay = 0;
            //then adds any word that can be played.
            for (int i = 0; i < validWords.Length; i++)
            {
                int temp = checkWord(validWords[i]);
                if (temp > 0)
                {
                    wordsOutput.Add(validWords[i]);
                    wordsPoints.Add(temp);
                }
            }
            //resets the sorting mode to the default
            cbSortBy.SelectedIndex = 0;
            displayWords();
            displayDiagnostics((bool)chkHideDiagnostics.IsChecked);
        }

        /// <summary>
        /// Displays data from the last run of UpdateWords().
        /// Data: number of playable words, maximum number of visible words at once, and time to run.
        /// </summary>
        /// <param name="hidden">A boolean representing visibility state (true = hidden)</param>
        private void displayDiagnostics(bool hidden)
        {
            lblDiagnostics.Content = wordsOutput.Count + " playable words  ";
            lblDiagnostics.Content += "Can show " + (outputCapacity.Y * outputCapacity.X) + " items (" + outputCapacity.Y + " rows and " + outputCapacity.X + " columns)  ";
            lblDiagnostics.Content += "Time to run: " + (DateTime.Now - timerStart).TotalMilliseconds + " ms";//the timer starts in the playerHand.get() method.

            if (hidden) { lblDiagnostics.Visibility = Visibility.Collapsed; }
            else { lblDiagnostics.Visibility = Visibility.Visible; }
        }

        /// <summary>
        /// Sets the tiles onscreen to be the next 7 drawn by ScrabbleGame
        /// </summary>
        private void SetTiles()
        {
            TextBox[] UITiles = new TextBox[7];
            //if no tiles exist, the program makes a set.
            if (spTiles.Children.Count == 0)
            {
                for (int i = 0; i < 7; i++)
                {
                    UITiles[i] = new TextBox();
                    UITiles[i].Name = "Tile" + i;
                    UITiles[i].Width = 16; UITiles[i].Height = 20;
                    spTiles.Children.Add(UITiles[i]);
                }
            }
            //otherwise, it sets the existing ones and changes them
            else { UITiles = spTiles.Children.OfType<TextBox>().ToArray(); }

            string newTiles = sg.drawHand();//draws 7 tiles.
            changePlayerHand(newTiles);//other methods handle the checking and updating of the playable words when this is changed.
            for (int i = 0; i < newTiles.Length; i++)//updates the letter on the rendered tile.
            {
                UITiles[i].Text = newTiles[i].ToString();
            }
            //MessageBox.Show(initTiles);//troubleshooting
        }

        /// <summary>
        /// Updates the displayed words. 
        /// Generates a horizontal array of labels, which contain words seperated by line.
        /// Uses NextToDisplay (which is changed here and by the buttons Next and Prev)
        /// </summary>
        private void displayWords()
        {
            if (wordsOutput.Count == 0) { return; }
            spWords.Children.Clear();

            for (int x = 0; x < outputCapacity.X; x++)
            {
                string wordsOut = "";
                for (int y = 0; y < outputCapacity.Y; y++)
                {
                    if (nextToDisplay < wordsOutput.Count)
                    {
                        wordsOut += wordsOutput[nextToDisplay] + '\t' + wordsPoints[nextToDisplay] + '\r';
                        nextToDisplay++;
                    }
                }
                spWords.Children.Add(createColumn(x, wordsOut));
            }

            //Sets visibility of navigation buttons.
            if (nextToDisplay < wordsOutput.Count) { btnNext.Visibility = Visibility.Visible; }
            else { btnNext.Visibility = Visibility.Hidden; }
            if (nextToDisplay - (outputCapacity.Y * outputCapacity.X) > 0) { btnPrev.Visibility = Visibility.Visible; }
            else { btnPrev.Visibility = Visibility.Hidden; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i">The horizontal index of this column</param>
        /// <param name="wordsOut">A '\r' seperated string of the words the column is to display.</param>
        /// <returns>A label containing the output.</returns>
        private Label createColumn(int i, string wordsOut)
        {
            return new Label { Content = wordsOut, Name = "lblWordsOutput" + i.ToString(), Width = 73 };
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            //moves the display back (if the new is negative it sets it to 0)
            nextToDisplay -= (int)(2 * (outputCapacity.Y * outputCapacity.X));
            if (nextToDisplay < 0) { nextToDisplay = 0; }
            displayWords();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            //don't need to update nextToDisplay; it's already at the last item
            displayWords();
        }

        /// <summary>
        /// Changes the input method between Tiles and Textbox, based on which one is selected in the settings menu.
        /// </summary>
        private void cbInputMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbInputMode.SelectedItem == cbTiles)
            {
                spTiles.Visibility = Visibility.Visible;
                txtInput.Visibility = Visibility.Collapsed;

                ///begins a new game when the item is selected
                sg = new ScrabbleGame();
                SetTiles();
            }
            if (cbInputMode.SelectedItem == cbText)
            {
                spTiles.Visibility = Visibility.Collapsed;
                txtInput.Visibility = Visibility.Visible;

                txtInput.Text = "default";
                changePlayerHand(txtInput.Text);//updates in case the text was already "default" when switched. (the text wouldn't have changed or updated)
            }
            ///matches any other relevant ui items to their respective input.
            btnNextTiles.Visibility = spTiles.Visibility;
            //theres only one right now...
        }

        private void btnNextTiles_Click(object sender, RoutedEventArgs e)
        {
            SetTiles();
        }

        /// <summary>
        /// When the checkbox is checked, it hides the data currently being shown.
        /// </summary>
        private void chkHideDiagnostics_Checked(object sender, RoutedEventArgs e)
        {
            lblDiagnostics.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Sorts the ouput words list when the selection changes to the corresponding method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Note: Tried to put a catch system here so this code doesn't run and update the display a second time when a new input is entered ,but that broke it and it works fine so I'm leaving only this note to show that it's the right thing to do.
            if (wordsOutput == null) { return; }
            /*if (cbSortBy.SelectedItem == cbAlpha)
            {
                Tuple<List<string>, List<int>> temp = ss.SortByAlpha(wordsOutput, wordsPoints);
                wordsOutput = temp.Item1;
                wordsPoints = temp.Item2;

                //update words with the newly sorted ones.
                nextToDisplay = 0;
                displayWords();
            }
            if (cbSortBy.SelectedItem == cbAlphaInv)
            {
                Tuple<List<string>, List<int>> temp = ss.SortByAlphaInv(wordsOutput, wordsPoints);
                wordsOutput = temp.Item1;
                wordsPoints = temp.Item2;

                //update words with the newly sorted ones.
                nextToDisplay = 0;
                displayWords();
            }*/
            if (cbSortBy.SelectedItem == cbPoints)
            {
                Tuple<List<string>, List<int>> temp = ss.SortByPoints(wordsOutput, wordsPoints);
                wordsOutput = temp.Item1;
                wordsPoints = temp.Item2;

                //update words with the newly sorted ones.
                nextToDisplay = 0;
                displayWords();
            }
            if (cbSortBy.SelectedItem == cbPointsInv)
            {
                Tuple<List<string>, List<int>> temp = ss.SortByPointsInv(wordsOutput, wordsPoints);
                wordsOutput = temp.Item1;
                wordsPoints = temp.Item2;

                //update words with the newly sorted ones.
                nextToDisplay = 0;
                displayWords();
            }
        }
    }
}