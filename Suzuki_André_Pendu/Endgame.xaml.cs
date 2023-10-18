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
        public Endgame()
        {
            InitializeComponent();
            UpdateScoreboard();
        }
        //---------------------------------- Constructor Functions -----------------------------------
        public void UpdateScoreboard()
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;

            int score = main.Score;
            int wins = main.Wins;
            int losses = main.Losses;
            string word = main.mot_choisi;

            Score_TB.Text = "Score: "+score.ToString();
            Wins_TB.Text = "Wins: "+wins.ToString();
            Losses_TB.Text = "Losses: "+losses.ToString();
            Lifes_TB.Text = "Lifes: "+main.Vies.ToString();

            if (main.isWordReversed)
            {
                word = ReverseWord(main.mot_choisi);
            }

            Word_TB.Text = "It was: " + word;
           
        }
        //---------------------------------- Utility Functions -----------------------------------
        private string ReverseWord(string Word)
        {
            char[] CharArray = Word.ToCharArray();
            Array.Reverse(CharArray);
            return new string(CharArray);
        }
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
        //---------------------------------- Click Functions -----------------------------------
        private void NewRound_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            Button button = (Button)sender;

            if (button.Name == "NextGame_Btn")
            {
                main.NewGame(true);
            }
            else
            {
                main.NewGame(false);
            }

            this.Close();
        }
    }
}
