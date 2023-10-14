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
            NewGame();
        }
        //---------------------------------- Class Variables -----------------------------------
        string mot_choisi;
        string mot_cache;

        int LettersLeft;
        int StartVies = 7;
        int StartScore = 0;
        int Vies;
        int Score; 

        string chemin = @"Resources/MotsPendu.txt";
        string[] Mots_List;
        //---------------------------------- Constructor Functions -----------------------------------
        public void ReadFile()
        {
            Mots_List = File.ReadAllLines(chemin);
        }
        public void NewGame()
        {
            ChangeButtonState(true);
            Vies = StartVies;
            Score = StartScore;
            mot_choisi = ChoisirMot();
            LettersLeft = mot_choisi.Length;
            UpdateScore();
            UpdateVies();
            CacherMot();
            //TB_Feedback.Text = "Bonne Chance!";
        }
        //---------------------------------- Word Related Functions -----------------------------------
        public void RevealMot()
        {
            TB_Display.Text = mot_choisi;
        }
        string ChoisirMot()
        {
            Random rnd = new Random();
            return Mots_List[rnd.Next(0, Mots_List.Length - 1)];
        }

        void CacherMot()
        {
            mot_cache = "";

            foreach (char c in mot_choisi)
            {
                mot_cache += "?";
            }
            TB_Display.Text = mot_cache;
        }

        public bool LetterMatch(string Letter)
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
        public void ChangeButtonState(bool Value)
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
        //---------------------------------- Client-Application Interaction Related Functions -----------------------------------
        public void LettreBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string lettre = button.Content.ToString();

            button.IsEnabled = false;

            SolidColorBrush NewColorBrush = this.TryFindResource("Wrong_Btn") as SolidColorBrush;

            if (LetterMatch(lettre))
            {
                 NewColorBrush = this.TryFindResource("Correct_Btn") as SolidColorBrush;
            }

            button.Background = NewColorBrush;


            if (LettersLeft == 0 | Vies == 0)
            {
                ChangeButtonState(false);

                string Message = "Game Over, You Lost!";
                int ScoreToAdd = -50;

                if (Vies > 0)
                {
                    Message = "You Won! Congratulations!";
                    ScoreToAdd = 100;
                }
                UpdateScore(ScoreToAdd);
                RevealMot();
                MessageBox.Show(Message);
                
            }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            NewGame();
        }
    }
}
