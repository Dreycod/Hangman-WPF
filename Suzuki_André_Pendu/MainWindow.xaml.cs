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
using System.Windows.Threading;
using System.Windows.Media.Animation;

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
        public bool isWordReversed = false;
        public string mot_choisi;

        private string mot_cache;
        private int LettersLeft;

        private int StartVies = 7;
        private int StartScore = 0;

        private int time = 0;
        private bool isTimerRunning = false;
        private bool GameEnded = false;

        private string chemin = @"Resources/MotsPendu.txt";
        private string[] Mots_List;
        //---------------------------------- Mechanics Functions -----------------------------------
        private void ReadFile()
        {
            Mots_List = File.ReadAllLines(chemin);
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            string letter = e.Key.ToString().ToUpper();

            if (Char.IsLetter(letter[0]) && letter.Length == 1)
            {
                Validate_Content(letter);
            }
        }

        //---------------------------------- Game State Functions -----------------------------------
        public void NewGame(bool IsNextGame)
        {
            GameEnded = false;
            isWordReversed = false;

            EnableKeyboard(true);
            EnableSideBar(true);
            ResetTimer();

            if (isTimerRunning == false)
            {
                StartTimer();
            }

            if (IsNextGame == false)
            {
                ResetGameData();
            }

            InitializeGameData();
            UpdateScore();
            UpdateVies();
            CacherMot();
        }
        private void InitializeGameData()
        {
            Vies = StartVies;
            TB_Wins.Text = "W: " + Wins.ToString();
            TB_Losses.Text = "L: " + Losses.ToString();
            mot_choisi = ChoisirMot();
            LettersLeft = mot_choisi.Length;
        }
        private void ResetGameData()
        {
            Score = StartScore;
            Wins = 0;
            Losses = 0;
        }
        private void DetectEndGame(bool IsFromTimer)
        {
            if (LettersLeft == 0 | Vies == 0 | IsFromTimer)
            {
                GameEnded = true;
                EnableKeyboard(false);
                EnableSideBar(false);

                string Message = "You Lost, Keep going!";
                int ScoreToAdd = -25;
                bool IsWin = false;

                if (Vies > 0 && time < Timer_ProgressBar.Maximum)
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
        //---------------------------------- Timer Functions -----------------------------------
        public void StartTimer()
        {
            isTimerRunning = true;
            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Timer_ProgressBar.Value >= Timer_ProgressBar.Maximum | GameEnded)
            {

                DispatcherTimer timer = (DispatcherTimer)sender;
                timer.Stop();
                isTimerRunning = false;

                if (GameEnded == false)
                {
                    DetectEndGame(true);
                }
                return;
            }

            time++;
            Timer_ProgressBar.Value = time;
        }

        //---------------------------------- Word Related Functions -----------------------------------
        private string ChoisirMot()
        {
            Random rnd = new Random();
            return Mots_List[rnd.Next(0, Mots_List.Length - 1)];
        }

        public void RevealMot()
        {
            TB_Display.Text = mot_choisi;
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

        private void ReverseWord()
        {
            char[] CharArray = mot_cache.ToCharArray();
            char[] CharArray_2 = mot_choisi.ToCharArray();

            Array.Reverse(CharArray);
            Array.Reverse(CharArray_2);
            mot_cache = new string(CharArray);
            mot_choisi = new string(CharArray_2);

            TB_Display.Text = mot_cache;
            isWordReversed = true;
        }

        private Char[] ReplaceLetter(string Letter)
        {
            char[] CharArray = mot_cache.ToCharArray();

            for (int i = 0; i < mot_choisi.Length; i++)
            {
                if (mot_choisi[i] == Letter[0])
                {
                    CharArray[i] = Letter[0];
                }
            }
            return CharArray;
        }
        private bool FindLetter(string Letter)
        {
            if (mot_choisi.Contains(Letter))
            {
                mot_cache = new string(ReplaceLetter(Letter));
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
            if (isWordReversed)
            {
                num *= 2; // 2x points if word is reversed
            }

            Score += num;
            TB_Score.Text = "Score: " + Score.ToString();
        }

        public void ResetTimer()
        {
            time = 0;
            Timer_ProgressBar.Value = time;
        }

        //---------------------------------- Button Related Functions -----------------------------------
        private Button GetButtonFromContent(string Content)
        {
            foreach (var btn in ButtonGrid.Children.OfType<Button>())
            {
                if (btn.Content.ToString() == Content)
                {
                    return btn;
                }
            }
            return null;
        }
        private void Validate_Content(string content)
        {
            Button button = GetButtonFromContent(content);

            if (button == null | button.IsEnabled == false)
            {           
                return;
            }

            button.IsEnabled = false;

            if (FindLetter(content))
            {
                button.Background = this.TryFindResource("Correct_Btn") as SolidColorBrush;
            }
            else
            {
                button.Background = this.TryFindResource("Wrong_Btn") as SolidColorBrush;
            }

            DetectEndGame(false);
        }
        public void EnableKeyboard(bool Value)
        {
            foreach (var button in ButtonGrid.Children.OfType<Button>())
            {
                button.IsEnabled = Value;

                if (Value == true)
                {
                    button.ClearValue(Button.BackgroundProperty);
                    button.Style = this.TryFindResource("Hang_Btn") as Style; ;
                    continue;
                }

                if (mot_choisi.Contains(button.Content.ToString()))
                {
                    button.Background = this.TryFindResource("Correct_Btn") as SolidColorBrush;
                }
                else
                {
                    button.Background = this.TryFindResource("Wrong_Btn") as SolidColorBrush;
                }
            }
        }

        public void EnableSideBar(bool Value)
        {
            foreach (var button in Functionalities_Grid.Children.OfType<Button>())
            {
                button.IsEnabled = Value;
            }
        }

        //---------------------------------- Pop-up Related Functions -----------------------------------

        private void Endgame_popup(string Message, bool Value)
        {
            Endgame endgame = new Endgame();
            endgame.Show();
            endgame.Message_TB.Text = Message;
            endgame.IsWin(Value);
            endgame.Closed += Endgame_Closed;

            void Endgame_Closed(object sender, EventArgs e)
            {
                NewGame_Btn.IsEnabled = true;
            }
        }

        //---------------------------------- Button Click Functions -----------------------------------
        private void LettreBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string lettre = button.Content.ToString();
            Validate_Content(lettre);
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            NewGame(false);
        }
        private void Hint_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.IsEnabled = false;

            int index = mot_cache.IndexOf('?');

            if (index == -1)
            {
                return;
            }

            string letter = mot_choisi[index].ToString();
            Vies -= 2;
            UpdateVies();
            Validate_Content(letter);
        }
        private void Reverse_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.IsEnabled = false;
            ReverseWord();
        }
    }
}
