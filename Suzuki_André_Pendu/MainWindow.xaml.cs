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

        string mot_choisi;
        string mot_cache;

        int LettersLeft;
        int StartVies = 7;
        int Vies;

        string chemin = @"Resources/MotsPendu.txt";
        string[] Mots_List;

        public void ReadFile()
        {
            Mots_List = File.ReadAllLines(chemin);
        }

        public void NewGame()
        {

            string ChoisirMot()
            {
                Random rnd = new Random();
                return Mots_List[rnd.Next(0, Mots_List.Length - 1)];
            }

            void CacherMot()
            {
                mot_cache = "";

                foreach(char c in mot_choisi)
                {
                    mot_cache += "?";
                }
                TB_Display.Text = mot_cache;
            }
            
            ChangeButtonState(true);
            Vies = StartVies;
            mot_choisi = ChoisirMot();
            LettersLeft = mot_choisi.Length;
            UpdateVies();
            CacherMot();
            //TB_Feedback.Text = "Bonne Chance!";
        }
        public void ChangeButtonState(bool Value)
        {
            if (Value == true)
            {
                foreach (var button in ButtonGrid.Children.OfType<Button>())
                {
                    button.IsEnabled = true;
                    button.Background = this.TryFindResource("Default_Btn") as SolidColorBrush;
                }
            }
            else 
            {
                foreach (var button in ButtonGrid.Children.OfType<Button>())
                {
                    button.IsEnabled = false;
                    button.Background = this.TryFindResource("Disabled_Btn") as SolidColorBrush;
                }
            }
        }

        public void UpdateVies()
        {
            Uri resourceUri = new Uri(@"Resources/Hangman/" + (StartVies-Vies).ToString() + ".gif", UriKind.Relative);
            Img_Pendu.Source =  new BitmapImage(resourceUri);
            TB_Lifes.Text = "Lifes: "+Vies.ToString();    
        }

        public bool LetterMatch(string Letter)
        {
            if (mot_choisi.Contains(Letter))
            {
                char[] CharArray = mot_cache.ToCharArray();
                List<int> indexes = new List<int>();    

                for (int i = 0; i < mot_choisi.Length; i++)
                {
                    if (mot_choisi[i] == Letter[0])
                    {
                        indexes.Add(i); 
                    }
                }

                foreach (int index in indexes)
                {
                    CharArray[index] = Letter[0];
                }

                string updatedMotcache = new string(CharArray); 
                mot_cache = updatedMotcache;
                TB_Display.Text = mot_cache;
                

                LettersLeft -= mot_choisi.Count(c => c == Letter[0]);
                return true;
            }
            return false;
        }

        public void LettreBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string lettre = button.Content.ToString();

            button.IsEnabled = false;

            if (LetterMatch(lettre))
            {
                button.Background = this.TryFindResource("Correct_Btn") as SolidColorBrush;
            }
            else
            {
                button.Background = this.TryFindResource("Wrong_Btn") as SolidColorBrush;
                Vies -= 1;
                UpdateVies();
            }

            if (LettersLeft == 0 | Vies == 0)
            {
                ChangeButtonState(false);
                if (Vies > 0)
                {
                    // Win
                    //TB_Feedback.Text = "Vous avez gagné!";

                }
                else
                {
                    // Lose
                    //TB_Feedback.Text = "Vous avez perdu!";
                }
            }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            NewGame();
        }
    }
}
