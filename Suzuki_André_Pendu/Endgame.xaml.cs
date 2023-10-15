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
            WindowOpened();
        }

        private void WindowOpened()
        {
            UpdateText();
        }
        public void UpdateText()
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;

            int score = main.Score;
            int wins = main.Wins;
            int losses = main.Losses;

            Score_TB.Text = "Score: "+score.ToString();
            Wins_TB.Text = "Wins: "+wins.ToString();
            Losses_TB.Text = "Losses: "+losses.ToString();
            Lifes_TB.Text = "Lifes: "+main.Vies.ToString();
            Word_TB.Text = "It was: "+main.mot_choisi;
        }
        private void NextGame_click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            Button button = (Button)sender;
            bool Next = false;

            if (button.Name == "NextGame_Btn")
            {
                Next = true;
            }
            main.NewGame(Next);
            this.Close();
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
    }
}
