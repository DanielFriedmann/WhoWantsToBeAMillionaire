using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WhoWantsToBeAMillionaire.Services;
using WhoWantsToBeAMillionaire.Views;

namespace WhoWantsToBeAMillionaire
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PlayerNameInput.Text))
            {
                MessageBox.Show("Bitte gib einen Namen ein!", "Kein Name",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            GameWindow gameWindow = new GameWindow(PlayerNameInput.Text);
            gameWindow.Show();
            this.Close();
        }

        private void HighscoreButton_Click(object sender, RoutedEventArgs e)
        {
            HighscoreWindow highscoreWindow = new HighscoreWindow();
            highscoreWindow.Show();
        }
    }
}