using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Reflection.Metadata;

namespace Suzuki_André_Pendu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ReadFile();
            NewGame(false);
            KeyDown += new KeyEventHandler(MainWindow_KeyDown);
        }

        //---------------------------------- Class Variables -----------------------------------

        public int Score;
        public int Vies;
        public int Wins;
        public int Losses;
        public string mot_choisi;

        private string mot_cache;
        private int LettersLeft;

        private int StartVies = 7;
        private int StartScore = 0;
       

        private string chemin = @"Resources/MotsPendu.txt";
        private string[] Mots_List;
        //---------------------------------- Events -----------------------------------
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            string letter = e.Key.ToString().ToUpper();

            if (Char.IsLetter(letter[0]))
            {
                ValidateButton(letter);
            }
        }
        //---------------------------------- Constructor Functions -----------------------------------
        public void NewGame(bool IsNextGame)
        {
            ChangeAllButtonState(true);
            Vies = StartVies;

            if (IsNextGame == false)
            {
                Score = StartScore;
                Wins = 0;
                Losses = 0;
            }

            TB_Wins.Text = "W: " + Wins.ToString();
            TB_Losses.Text = "L: " + Losses.ToString();
            mot_choisi = ChoisirMot();
            LettersLeft = mot_choisi.Length;
            UpdateScore();
            UpdateVies();
            CacherMot();
        }
        private void ReadFile()
        {
            Mots_List = File.ReadAllLines(chemin);
        }
        //---------------------------------- Word Related Functions -----------------------------------
        public void RevealMot()
        {
            TB_Display.Text = mot_choisi;
        }
        private string ChoisirMot()
        {
            Random rnd = new Random();
            return Mots_List[rnd.Next(0, Mots_List.Length - 1)];
        }

        private void CacherMot()
        {
            mot_cache = "";

            foreach (char c in mot_choisi)
            {
                mot_cache += "?";
            }
            TB_Display.Text = mot_cache;
        }

        private bool LetterMatch(string Letter)
        {
            if (mot_choisi.Contains(Letter))
            {
                char[] CharArray = mot_cache.ToCharArray();

                for (int i = 0; i < mot_choisi.Length; i++)
                {
                    if (mot_choisi[i] == Letter[0])
                    {
                        CharArray[i] = Letter[0];
                    }
                }

                mot_cache = new string(CharArray);
                TB_Display.Text = mot_cache;
                UpdateScore(10);
                LettersLeft -= mot_choisi.Count(c => c == Letter[0]);
                return true;
            }
            Vies -= 1;
            UpdateScore(-5);
            UpdateVies();
            return false;
        }
        private void DecideEndGame()
        {
            if (LettersLeft == 0 | Vies == 0)
            {
                ChangeAllButtonState(false);

                string Message = "You Lost, Keep going!";
                int ScoreToAdd = -25;
                bool IsWin = false;

                if (Vies > 0)
                {
                    IsWin = true;
                    Wins += 1;
                    ScoreToAdd = 100;
                    Message = "You Won! Congratulations!";
                }
                else
                {
                    Losses += 1;
                }

                UpdateScore(ScoreToAdd);
                RevealMot();
                Endgame_popup(Message, IsWin);
            }
        }
        //---------------------------------- Counter Related Functions -----------------------------------
        public void UpdateVies()
        {
            Uri resourceUri = new Uri(@"Resources/Hangman/" + (StartVies - Vies).ToString() + ".gif", UriKind.Relative);
            Img_Pendu.Source = new BitmapImage(resourceUri);
            TB_Lifes.Text = "Lifes: " + Vies.ToString();
        }

        public void UpdateScore(int num = 0)
        {
            Score += num;
            TB_Score.Text = "Score: " + Score.ToString();
        }
        //---------------------------------- Button Related Functions -----------------------------------
        private void ValidateButton(string content)
        {
            Button button = new Button();
            button.Content = "Not Changed";

            foreach (var btn in ButtonGrid.Children.OfType<Button>())
            {
                if (btn.Content.ToString() == content)
                {
                    button = btn;
                    break;
                }
            }

            if (button.IsEnabled == false | button.Content == "Not Changed")
            {
                return;
            }

            button.IsEnabled = false;

            SolidColorBrush NewColorBrush = this.TryFindResource("Wrong_Btn") as SolidColorBrush;

            if (LetterMatch(content))
            {
                NewColorBrush = this.TryFindResource("Correct_Btn") as SolidColorBrush;
            }

            button.Background = NewColorBrush;

            DecideEndGame();
        }
        public void ChangeAllButtonState(bool Value)
        {
            Style ChosenStyle = this.TryFindResource("Hang_Btn") as Style;
            bool IsEnabled = false;

            if (Value)
            {
                ChosenStyle = this.TryFindResource("Hang_Btn") as Style;
                IsEnabled = true;
            }
           
            foreach (var button in ButtonGrid.Children.OfType<Button>())
            {
                
                button.IsEnabled = IsEnabled;

                if (IsEnabled)
                {
                    button.ClearValue(Button.BackgroundProperty);
                    button.Style = ChosenStyle;
                    continue;
                }

                SolidColorBrush ChosenBrush = this.TryFindResource("Wrong_Btn") as SolidColorBrush;

                if (mot_choisi.Contains(button.Content.ToString()))
                {
                    ChosenBrush = this.TryFindResource("Correct_Btn") as SolidColorBrush;
                }

                button.Background = ChosenBrush;
            }
        }
        //---------------------------------- Pop-up Related Functions -----------------------------------

        private void Endgame_popup(string Message, bool Value)
        {
            Endgame endgame = new Endgame();
            endgame.Show();
            endgame.Message_TB.Text = Message;
            endgame.IsWin(Value);
        }
        //---------------------------------- Client-Application Interaction Related Functions -----------------------------------
        private void LettreBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string lettre = button.Content.ToString();
            ValidateButton(lettre);
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            NewGame(false);
        }
    }
}
