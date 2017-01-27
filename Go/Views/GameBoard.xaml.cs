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

namespace Go.Views
{
    using Go.Controlers;
    using Go.Models;

    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Window
    {
        #region Fields

        public bool _isGameActive = true;

        private const int _size = 19;

        private MainController _mainControlerInstance;

        #endregion

        #region Constructors
        public GameBoard(MainController mainControlerInstances, ColumnDefinition firstCol, RowDefinition firstRow, List<ColumnDefinition> columnsList, List<RowDefinition> rowsList, List<Token> buttonsList)
        {
            #region ConstructorSection

            PlayFieldGeneratorController fieldGenerator = new PlayFieldGeneratorController();
            _mainControlerInstance = mainControlerInstances;
            InitializeComponent();
            _mainControlerInstance.Initialize(this);

            #endregion

            #region drawPlayField

            _ControlGrid.ColumnDefinitions.Add(firstCol);
            _ControlGrid.RowDefinitions.Add(firstRow);

            foreach (ColumnDefinition column in columnsList)
            {
                _ControlGrid.ColumnDefinitions.Add(column);
            }
            foreach (RowDefinition row in rowsList)
            {
                _ControlGrid.RowDefinitions.Add(row);
            }

            foreach (Token button in buttonsList)
            {
                button.Click += ButtonBaseOnClick;
                _ControlGrid.Children.Add(button);
            }

            _mainControlerInstance.UpdateGameStatusInfo();

            #endregion

        }

        #endregion

        #region Methods

        private void ButtonBaseOnClick(object sender, RoutedEventArgs e)
        {
            if (_isGameActive)
            {
                _isGameActive = _mainControlerInstance.OnPlayStoneClick((Token)sender);
                _mainControlerInstance.UpdateGameStatusInfo();
            }
        }

        private void CmdSkipOnClick(object sender, RoutedEventArgs e)
        {
           _isGameActive = _mainControlerInstance.OnPlayStoneClick(null);
           _mainControlerInstance.UpdateGameStatusInfo();
            if (!_isGameActive)
            {
                _cmdSkip.IsEnabled = false;
            }
        }

        private void CmdCloseOnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Wollen Sie das Spiel vor dem beenden Speichern?", "Beenden", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Speichern
                _mainControlerInstance.SaveGame();
                MessageBox.Show("Gespeichert");
                new MainWindow().Show();
                Close();                
            }
            if (result == MessageBoxResult.No)
            {
                new MainWindow().Show();
                Close();
            }
        }

        private void CmdSaveClick(object sender, RoutedEventArgs e)
        {
            // Speichern
            _mainControlerInstance.SaveGame();
            MessageBox.Show("Gespeichert");       
        }

        #endregion
    }
}
