using System;
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
using System.Diagnostics.Eventing.Reader;
using Suzuki_André_Pendu.Classes;

namespace Suzuki_André_Pendu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //---------------------------------- Initializations -----------------------------------

        // Classes 
        private WordManager _WordManager;
        private GameManager _GameManager;

        // Constructor

        /// <summary>
        /// Initalizes the main window and the game, sets up the event handlers.
        /// Note: Most of the game logic is in the GameManager class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // Class Initialization
            _GameManager = new GameManager(this);
            _WordManager = new WordManager(_GameManager);
            // Game Initialization
            _GameManager.NewGame(false);
            KeyDown += new KeyEventHandler(MainWindow_KeyDown);
        }

       
        //---------------------------------- Button Event Functions -----------------------------------

        /// <summary>
        /// Handles all KeyboardGrid button clicks. 
        /// Each click will be verified by the GameManager.
        /// </summary>
        private void LettreBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string lettre = button.Content.ToString();
            _GameManager.Validate_Content(lettre);
        }

        /// <summary>
        /// Handles all Newgame button clicks. 
        /// Creates a new game while resetting scores etc.
        /// </summary>
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            _GameManager.NewGame(false);
        }

        /// <summary>
        /// Handles Hint button clicks. 
        /// Sacrifices 2 lifes for 1 letter.
        /// </summary>
        private void Hint_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.IsEnabled = false;
            btn.Background = this.TryFindResource("Disabled_Btn") as SolidColorBrush;


            int index = _GameManager.mot_cache.IndexOf('?');

            if (index == -1)
            {
                return;
            }

            string letter = _GameManager.mot_choisi[index].ToString();
            _GameManager.Vies -= 2;
            _GameManager.UpdateVies();
            _GameManager.Validate_Content(letter);
        }

        /// <summary>
        /// Handles Reverse button clicks. 
        /// Reverse the word for 2x points.
        /// </summary>
        private void Reverse_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            _WordManager.ReverseWord();

            if (_GameManager.isWordReversed)
            {
                btn.Background = this.TryFindResource("Correct_Btn") as SolidColorBrush;
            }
            else
            {
                btn.Background = this.TryFindResource("Default_Btn") as SolidColorBrush;
            }
        }

        /// <summary>
        /// Handles Reverse button clicks. 
        /// Creates an instance of the InfoWindow class. + displays info to the player.
        /// </summary>
        private void Info_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.IsEnabled = false;
            btn.Background = this.TryFindResource("Disabled_Btn") as SolidColorBrush;

            InfoWindow infoWindow = new InfoWindow();   
            infoWindow.Show();
            infoWindow.Closed += InfoWindow_Closed; 

            void InfoWindow_Closed(object sender, EventArgs e)
            {
                btn.IsEnabled = true;
                btn.Background = this.TryFindResource("Default_Btn") as SolidColorBrush;
            }
        }

        //---------------------------------- Misc Functions -----------------------------------

        /// <summary>
        /// Handles all keyboard input, only accepts letters.
        /// each letter is verified by the GameManager.
        /// </summary>
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            string letter = e.Key.ToString().ToUpper();

            if (Char.IsLetter(letter[0]) && letter.Length == 1)
            {
                _GameManager.Validate_Content(letter);
            }
        }
    }
}
