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
  using System.Windows.Forms;

  /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
          InitializeComponent();
          _txtSavePath.Text = MainWindow._savePath;
        }

        private void CmdPathSelectOnClick(object sender, RoutedEventArgs e)
        {
          FolderBrowserDialog fbd = new FolderBrowserDialog();
          fbd.ShowDialog();
          _txtSavePath.Text = fbd.SelectedPath;
        }

        private void CmdSaveClick(object sender, RoutedEventArgs e)
        {
          MainWindow._savePath = _txtSavePath.Text;
          Close();
        }

        private void CmdAbbortClick(object sender, RoutedEventArgs e)
        {
          Close();
        }
    }
}
