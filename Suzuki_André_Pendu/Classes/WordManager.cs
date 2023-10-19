using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.IO;

namespace Suzuki_André_Pendu.Classes
{
    internal class WordManager
    {
        //---------------------------------- Initializations -----------------------------------
        // Variables
        private string chemin = @"Resources/MotsPendu.txt";
        private string[] Mots_List;
        // Classes
        private GameManager _GameManager;
        private MainWindow _mainWindow;
        // Constructor
        public WordManager(GameManager gameManager)
        {
            _GameManager = gameManager;
            _mainWindow = _GameManager._mainWindow;
            ReadFile();
        }
        //---------------------------------- Functions  -----------------------------------

        /// <summary>
        /// Reads the file containing the words and puts them in a string array [Mots_List]
        /// </summary>
        public void ReadFile()
        {
            Mots_List = File.ReadAllLines(chemin);
        }

        /// <summary>
        /// Chooses a random word from the string array [Mots_List]
        /// </summary>
        public string ChoisirMot()
        {
            Random rnd = new Random();
            return Mots_List[rnd.Next(0, Mots_List.Length - 1)];
        }

        /// <summary>
        /// Reveals the word
        /// </summary>
        public void RevealMot()
        {
            _mainWindow.TB_Display.Text = _GameManager.mot_choisi;
        }

        /// <summary>
        /// Hides the word with question marks and puts it in [mot_cache] while displaying it
        /// </summary>
        public void CacherMot()
        {
            _GameManager.mot_cache = "";

            foreach (char c in _GameManager.mot_choisi)
            {
                _GameManager.mot_cache += "?";
            }
            _mainWindow.TB_Display.Text = _GameManager.mot_cache;
        }

        /// <summary>
        /// Reverts the word and displays it while updating [isWordReversed]
        /// </summary>
        public void ReverseWord()
        {
            char[] CharArray = _GameManager.mot_cache.ToCharArray();
            char[] CharArray_2 = _GameManager.mot_choisi.ToCharArray();

            Array.Reverse(CharArray);
            Array.Reverse(CharArray_2);

            _GameManager.mot_cache = new string(CharArray);
            _GameManager.mot_choisi = new string(CharArray_2);

            _mainWindow.TB_Display.Text = _GameManager.mot_cache;
            _GameManager.isWordReversed = !_GameManager.isWordReversed;
        }

        /// <summary>
        /// Replaces the question marks with the letter if it's in the word used in hint
        /// </summary>
        public Char[] ReplaceLetter(string Letter)
        {
            char[] CharArray = _GameManager.mot_cache.ToCharArray();

            for (int i = 0; i < _GameManager.mot_choisi.Length; i++)
            {
                if (_GameManager.mot_choisi[i] == Letter[0])
                {
                    CharArray[i] = Letter[0];
                }
            }
            return CharArray;
        }

        /// <summary>
        /// Finds the letter in the word and replaces the question marks with it if it's in the word
        /// </summary>
        public bool FindLetter(string Letter)
        {
            if (_GameManager.mot_choisi.Contains(Letter))
            {
                _GameManager.mot_cache = new string(ReplaceLetter(Letter));
                _mainWindow.TB_Display.Text = _GameManager.mot_cache;
                _GameManager.UpdateScore(10);
                _GameManager.LettersLeft -= _GameManager.mot_choisi.Count(c => c == Letter[0]);
                return true;
            }

            _GameManager.Vies -= 1;
            _GameManager.UpdateScore(-5);
            _GameManager.UpdateVies();
            return false;
        }
    }
}
