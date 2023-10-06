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
            NewGame();
        }
        private string[] Mots_List = { "CIEL", "UNIVERS", "AMOUR", "FENETRE", "AIMER" };
        string mot_choisi;
        string mot_cache;
        int LettersLeft; 
        int Vies;
        public void NewGame()
        {
            string ChoisirMot()
            {
                Random rnd = new Random();
                return Mots_List[rnd.Next(0, Mots_List.Length - 1)];
            }

            void CacherMot()
            {
                foreach(char c in mot_choisi)
                {
                    mot_cache += "?";
                }
                TB_Display.Text = mot_cache;
            }

            Vies = 5;
            mot_choisi = ChoisirMot();
            LettersLeft = mot_choisi.Length;
            UpdateVies();
            CacherMot();


        }
        
        public void UpdateVies()
        {
            TB_Feedback2.Text = "Vous avez "+Vies.ToString()+" Vies";    
        }

        public bool LetterMatch(string Letter)
        {
            if (mot_choisi.Contains(Letter))
            {
                char[] CharArray = mot_cache.ToCharArray();

                List<int> indexes = new List<int>();    

                for (int i = 0; i < mot_choisi.Length-1; i++)
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
                if (LettersLeft > 0) {
                    TB_Feedback.Text = lettre + " is the right one";
                    button.Background = new SolidColorBrush(Colors.Green);
                    return;
                }
                TB_Feedback.Text = "Vous avez gagné!";

            }
            else
            {
                TB_Feedback.Text = lettre + " is not the right one";
                button.Background = new SolidColorBrush(Colors.Red);
                Vies -= 1;
                UpdateVies();
            }
           
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            NewGame();
        }
    }
}
