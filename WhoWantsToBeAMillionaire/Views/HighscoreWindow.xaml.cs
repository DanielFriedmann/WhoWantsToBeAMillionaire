using System.Windows;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Services;

namespace WhoWantsToBeAMillionaire.Views
{
    public partial class HighscoreWindow : Window
    {
        public HighscoreWindow()
        {
            InitializeComponent();
            LoadHighscores();
        }

        private void LoadHighscores()
        {
            var service = new HighscoreService();
            var entries = service.LoadHighScores();
            
            var ranked = entries
                .Select((e, i) => new
                {
                    Platz = i + 1,
                    e.PlayerName,
                    e.Level,
                    e.PrizeAmount,
                    e.PlayedAt
                })
                .ToList();

            HighscoreList.ItemsSource = ranked;
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}