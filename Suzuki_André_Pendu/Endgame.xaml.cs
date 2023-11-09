using Suzuki_André_Pendu.Classes;
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
using System.Windows.Shapes;

namespace Suzuki_André_Pendu
{
    /// <summary>
    /// Interaction logic for Endgame.xaml
    /// </summary>
    public partial class Endgame : Window
    {
        //---------------------------------- Class Variables -----------------------------------
        private GameManager _GameManager;

        //---------------------------------- Constructor -----------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Endgame"/> class.
        /// </summary>
        /// <param name="gameManager"></param>
        public Endgame(GameManager gameManager)
        {
            InitializeComponent();
            _GameManager = gameManager;
            UpdateScoreboard();
        }

        /// <summary>
        /// Updates the scoreboard.
        /// </summary>
        public void UpdateScoreboard()
        {
            // Variables
            int score = _GameManager.Score;
            int wins = _GameManager.Wins;
            int losses = _GameManager.Losses;
            string word = _GameManager.mot_choisi;

            // Update TextBlocks
            Score_TB.Text = "Score: "+score.ToString();
            Wins_TB.Text = "Wins: "+wins.ToString();
            Losses_TB.Text = "Losses: "+losses.ToString();
            Lifes_TB.Text = "Lifes: "+ _GameManager.Vies.ToString();

            // Reverse Word if necessary
            if (_GameManager.isWordReversed)
            {
                word = ReverseWord(_GameManager.mot_choisi);
            }

            // Update Word TextBlock
            Word_TB.Text = "It was: " + word;
        }
        //---------------------------------- Utility Functions -----------------------------------

        /// <summary>
        /// Returns the specified word in reverse.
        /// </summary>
        /// <param name="Word"></param>
        /// <returns> the reversed word. </returns>
        private string ReverseWord(string Word)
        {
            char[] CharArray = Word.ToCharArray();
            Array.Reverse(CharArray);
            return new string(CharArray);
        }

        /// <summary>
        /// Sets the color of the Word_TB depending on the value.
        /// </summary>
        /// <param name="Value"></param>
        public void IsWin(bool Value)
        {
            if (Value)
            {
                Word_TB.Foreground = this.TryFindResource("Correct_Btn") as SolidColorBrush;
                Message_TB.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                Word_TB.Foreground = this.TryFindResource("Wrong_Btn") as SolidColorBrush;
                Message_TB.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        //---------------------------------- Events -----------------------------------

        /// <summary>
        /// Whenever a player clicks Next Game or New Round, this function is called.
        /// Restarts or plays the next game.
        /// </summary>
        private void NewRound_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            _GameManager.PlaySoundEffect();

            if (button.Name == "NextGame_Btn")
            {
                _GameManager.NewGame(true);
            }
            else
            {
                _GameManager.NewGame(false);
            }

            this.Close();
        }
    }
}
