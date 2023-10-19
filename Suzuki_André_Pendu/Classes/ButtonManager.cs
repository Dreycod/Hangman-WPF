using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Suzuki_André_Pendu.Classes
{
    internal class ButtonManager
    {
        private MainWindow _mainWindow;
        private GameManager _GameManager;
        public ButtonManager(GameManager gameManager)
        {

            _GameManager = gameManager;
            _mainWindow = _GameManager._mainWindow;
        }

        public Button GetKeyboardButtonFromContent(string content)
        {
            foreach (var btn in _mainWindow.ButtonGrid.Children.OfType<Button>())
            {
                if (btn.Content.ToString() == content)
                {
                    return btn;
                }
            }
            return null;
        }

        public void EnableKeyboard(bool Value)
        {
            string mot_choisi = _GameManager.mot_choisi;

            foreach (var button in _mainWindow.ButtonGrid.Children.OfType<Button>())
            {
                button.IsEnabled = Value;

                if (Value == true)
                {
                    button.ClearValue(Control.BackgroundProperty);
                    button.Style = _mainWindow.TryFindResource("Hang_Btn") as Style; ;
                    continue;
                }

                if (mot_choisi.Contains(button.Content.ToString()))
                {
                    button.Background = _mainWindow.TryFindResource("Correct_Btn") as SolidColorBrush;
                }
                else
                {
                    button.Background = _mainWindow.TryFindResource("Wrong_Btn") as SolidColorBrush;
                }
            }
        }

        public void EnableSideBar(bool Value)
        {
            foreach (var button in _mainWindow.Functionalities_Grid.Children.OfType<Button>())
            {
                button.IsEnabled = Value;
                if (Value == true && button.Name != "Reverse_Btn")
                {
                    button.Background = _mainWindow.TryFindResource("Default_Btn") as SolidColorBrush;
                }
            }
        }

    }
}
