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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Go
{
    using System.Security.Principal;

    using Go.Controlers;
    using Go.Models;
    using Go.Views;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      static internal string _savePath = @"C:";

        public MainWindow()
        {
            InitializeComponent();
            _cboPlayerOneColor.Items.Add(TokenState.BLACK);
            _cboPlayerOneColor.Items.Add(TokenState.WHITE);
            _cboPlayerTwoColor.Items.Add(TokenState.BLACK);
            _cboPlayerTwoColor.Items.Add(TokenState.WHITE);
            _cboPlayerOneColor.SelectedIndex = 0;
            _cboPlayerTwoColor.SelectedIndex = 1;
        }

        private void CmdStartOnClick(object sender, RoutedEventArgs e)
        {
            if (_cboPlayerOneColor.SelectedIndex != _cboPlayerTwoColor.SelectedIndex)
            {
                MainController mainControler = new MainController(_txtSpieler1.Text, _txtSpieler2.Text, (TokenState)_cboPlayerOneColor.SelectedItem, (TokenState)_cboPlayerTwoColor.SelectedItem);
                mainControler.StartGame();
                Close();
            }
            else
            {
                MessageBox.Show("Es können nicht beide Spieler die selbe Farbe haben.\nWählen Sie eine andere Farbe.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void CmdExitOnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CboPlayerOneColorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cboPlayerOneColor.SelectedIndex == 0)
            {
                _rectOne.Fill = Brushes.Black;
            }
            else
            {
                _rectOne.Fill = Brushes.White;
            }
        }

        private void CboPlayerTwoColorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cboPlayerTwoColor.SelectedIndex == 0)
            {
                _rectTwo.Fill = Brushes.Black;
            }
            else
            {
                _rectTwo.Fill = Brushes.White;
            }
        }

        private void CmdLoadClick(object sender, RoutedEventArgs e)
        {
            MainController mainController = new MainController();
            Close();
        }

        private void CmdSettignsClick(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        private void CmdCreditsClick(object sender, RoutedEventArgs e)
        {
          new AboutWindow().Show();
        }
    }
}
