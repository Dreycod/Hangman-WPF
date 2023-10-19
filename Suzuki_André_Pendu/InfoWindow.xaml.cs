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
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        /// <summary>
        /// Constructor for the InfoWindow
        /// Simply Updates the information in the window everytime it's opened
        /// </summary>
        public InfoWindow()
        {
            InitializeComponent();
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            Info_TB.Text = "1. Le but du jeu est de trouver le mot caché en devinant les lettres qui le composent.\n" +
                "2. Le joueur a droit à 7 essais.\n" +
                "3. Le bouton hint permet d'avoir un indice sur le mot. ATTENTION: Cela coute 2 vies\n" +
                "4. Le mode REVERSE permet de jouer à l'envers. Cela multiplie tout les points gagnés par 2.\n" +
                "5. Le joueur peut choisir de recommencer ou continuer lors la fin du jeu\n" +
                "6. Le joueur a droit au clavier \n";
        }
    }
}
