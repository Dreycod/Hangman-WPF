using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Suzuki_André_Pendu.Classes
{
    public class GameManager
    {
        //---------------------------------- Initializations -----------------------------------
        // Variables
        public int Score;
        public int Vies;
        public int Wins;
        public int Losses;
        public bool isWordReversed = false;

        public string mot_choisi;
        public string mot_cache;
        public int LettersLeft;

        public int StartVies = 7;
        public int StartScore = 0;

        public int time = 0;
        public bool GameEnded = false;
        public bool isMusicPlaying = false;
        // Classes 
        public MediaPlayer MusicPlayer = new MediaPlayer();
        private ButtonManager _ButtonManager;
        private WordManager _WordManager;
        private TimeManager _TimeManager;
        public MainWindow _mainWindow;
        // Constructor
        public GameManager(MainWindow mainWindow)
        {
            // Class Initialization
            _mainWindow = mainWindow; // For access to MainWindow through GameManager
            _ButtonManager = new ButtonManager(this);
            _WordManager = new WordManager(this);
            _TimeManager = new TimeManager(this);
        }
        //---------------------------------- Main Functions  -----------------------------------


        /// <summary>
        /// Toggles the music.
        /// </summary>
        public void ToggleMusic()
        {
            if (isMusicPlaying == false)
            {
                MusicPlayer.Volume = 0.2f;
                MusicPlayer.Open(new Uri(@"Resources/Music.wav", UriKind.Relative));
                MusicPlayer.Play();
                isMusicPlaying = true;
            }
            else
            {
                MusicPlayer.Stop();
                isMusicPlaying = false;
            }
        }

        // Create a function that plays 2 types of music, wins and losses
        public void PlaySoundEffect(string type = "")
        {
            MediaPlayer SFX_Player = new MediaPlayer();
            SFX_Player.Volume = 0.4f;
            Uri soundEffectPath;

            switch (type)
            {
                case "win":
                    soundEffectPath = new Uri(@"Resources/Win.wav", UriKind.Relative);
                    break;
                case "loss":
                    soundEffectPath = new Uri(@"Resources/Loss.wav", UriKind.Relative);
                    break;
                case "correct":
                    soundEffectPath = new Uri(@"Resources/Correct.wav", UriKind.Relative);
                    break;
                case "wrong":
                    soundEffectPath = new Uri(@"Resources/Wrong.wav", UriKind.Relative);
                    break;
                case "popup":
                    soundEffectPath = new Uri(@"Resources/Popup.wav", UriKind.Relative);
                    break;
                case "Tick":
                    SFX_Player.Volume = 0.2f;
                    soundEffectPath = new Uri(@"Resources/Tick.wav", UriKind.Relative);
                    break;
                default:
                    soundEffectPath = new Uri(@"Resources/Click.wav", UriKind.Relative);
                    break;
            }

            SFX_Player.Open(soundEffectPath);
            SFX_Player.Play();
        }

        /// <summary>
        /// Creates a new game by initializing new data, resets data if IsNextGame is false
        /// </summary>
        /// <param name="IsNextGame">Decides if game data should be resetted or not </param>
        public void NewGame(bool IsNextGame)
        {
            ToggleMusic();
            // Reinitalization + Setup
            GameEnded = false;

            _ButtonManager.EnableKeyboard(true);
            _ButtonManager.EnableSideBar(true);
            _TimeManager.ResetTimer();

            if (_TimeManager.isTimerRunning == false)
            {
                _TimeManager.StartTimer();
            }

            if (IsNextGame == false)
            {
                ResetGameData();
            }

            InitializeGameData();
            UpdateScore();
            UpdateVies();
            _WordManager.CacherMot();

            if (isWordReversed)
            {
                isWordReversed = false;
                _WordManager.ReverseWord();
            }
        }

        /// <summary>
        /// Valides a letter and updates the UI accordingly, also detects if the game has ended.
        /// </summary>
        /// <param name="content">The letter to validate</param>
        public void Validate_Content(string content)
        {
            Button button = _ButtonManager.GetKeyboardButtonFromContent(content);

            if (button == null | button.IsEnabled == false)
            {
                return;
            }

            button.IsEnabled = false;

            if (_WordManager.FindLetter(content))
            {
                PlaySoundEffect("correct");
                button.Background = _mainWindow.TryFindResource("Correct_Btn") as SolidColorBrush;
            }
            else
            {
                PlaySoundEffect("wrong");
                button.Background = _mainWindow.TryFindResource("Wrong_Btn") as SolidColorBrush;
            }

            DetectEndGame(false);
        }

        /// <summary>
        /// Detects if the game has ended and updates the UI accordingly.
        /// Updates the score and reveals the word if the game has ended.
        /// </summary>
        /// <param name="IsFromTimer">The letter to validate</param>
        public void DetectEndGame(bool IsFromTimer)
        {
            if (LettersLeft == 0 | Vies == 0 | IsFromTimer)
            {

                ToggleMusic();
                GameEnded = true;
                _ButtonManager.EnableKeyboard(false);
                _ButtonManager.EnableSideBar(false);

                string Message = "You Lost, Keep going!";
                int ScoreToAdd = -25;
                bool IsWin = false;

                if (Vies > 0 && time < _mainWindow.Timer_ProgressBar.Maximum)
                {
                    IsWin = true;
                    Wins += 1;
                    ScoreToAdd = 100;
                    PlaySoundEffect("win");
                    Message = "You Won! Congratulations!";
                }
                else
                {
                    PlaySoundEffect("loss");    
                    Losses += 1;
                }

                UpdateScore(ScoreToAdd);
                _WordManager.RevealMot();
                Endgame_popup(Message, IsWin);
            }
        }
        //---------------------------------- Sub Functions  -----------------------------------

        /// <summary>
        /// Initializes game data and UI
        /// </summary>
        private void InitializeGameData()
        {
            Vies = StartVies;
            _mainWindow.TB_Wins.Text = "W: " + Wins.ToString();
            _mainWindow.TB_Losses.Text = "L: " + Losses.ToString();
            mot_choisi = _WordManager.ChoisirMot();
            LettersLeft = mot_choisi.Length;
        }

        /// <summary>
        /// Resets game data
        /// </summary>
        private void ResetGameData()
        {
            Score = StartScore;
            Wins = 0;
            Losses = 0;
        }

        /// <summary>
        /// Updates Lifes UI and Pendu Image
        /// </summary>
        public void UpdateVies()
        {
            Uri resourceUri = new Uri(@"Resources/Hangman/" + (StartVies - Vies).ToString() + ".gif", UriKind.Relative);
            ImageBehavior.SetAnimatedSource(_mainWindow.Img_Pendu, new BitmapImage(resourceUri));
            _mainWindow.TB_Lifes.Text = "Lifes: " + Vies.ToString();
        }

        /// <summary>
        /// Updates Score UI and adds/removes points to score
        /// </summary>
        /// <param name="num">The amount of points to add/remove</param>
        public void UpdateScore(int num = 0)
        {
            if (isWordReversed && num > 0)
            {
                num *= 2; // 2x points if word is reversed
            }

            Score += num;
            _mainWindow.TB_Score.Text = "Score: " + Score.ToString();
        }

        /// <summary>
        /// Creates a popup window to display the endgame message
        /// </summary>
        /// <param name="Message">Endgame message to be displayed</param>
        /// <param name="Value">Win or Loss </param>
        private void Endgame_popup(string Message, bool Value)
        {
            PlaySoundEffect("popup");
            Endgame endgame = new Endgame(this);
            endgame.Show();
            endgame.Message_TB.Text = Message;
            endgame.IsWin(Value);
            endgame.Closed += Endgame_Closed;

            void Endgame_Closed(object sender, EventArgs e)
            {
                _mainWindow.NewGame_Btn.IsEnabled = true;
            }
        }
    }
}
