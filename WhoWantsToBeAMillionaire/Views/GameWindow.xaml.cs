using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Services;

namespace WhoWantsToBeAMillionaire.Views
{
    public partial class GameWindow : Window
    {
        private readonly QuestionService _questionService;
        private readonly HighscoreService _highscoreService;
        private GameState _gameState;
        private Question _currentQuestion;
        private readonly Button[] _answerButtons;
        private bool _isAnswering = false; 

        private List<PrizeLevel> _prizeLevels;

        private static readonly int[] Amounts =
        {
            100, 200, 300, 500, 1_000,
            2_000, 4_000, 8_000, 16_000, 32_000,
            64_000, 125_000, 250_000, 500_000, 1_000_000
        };

        public GameWindow(string playerName)
        {
            InitializeComponent();
            _questionService = new QuestionService();
            _highscoreService = new HighscoreService();
            _gameState = new GameState(playerName);
            _answerButtons = new Button[] { AnswerA, AnswerB, AnswerC, AnswerD };
            InitializePrizeLevels();
            LoadQuestion();
        }

        private void InitializePrizeLevels()
        {
            _prizeLevels = Amounts
                .Select((amount, i) => new PrizeLevel
                {
                    Level = i + 1,
                    Amount = amount                    
                })
                .ToList();
            
            PrizePanel.ItemsSource = _prizeLevels
                .OrderByDescending(p => p.Level)
                .ToList();

            UpdatePrizeLevels();
        }

        private void UpdatePrizeLevels()
        {
            foreach (var pl in _prizeLevels)
            {
                pl.IsActive = (pl.Level == _gameState.CurrentLevel + 1);
                pl.IsReached = (pl.Level < _gameState.CurrentLevel + 1);
            }
        }



        private void LoadQuestion()
        {
            _currentQuestion = _questionService.GetRandomQuestion(_gameState.CurrentLevel + 1);

            QuestionText.Text = _currentQuestion.Text;
            string[] prefixes = { "A", "B", "C", "D" };
            for (int i = 0; i < _answerButtons.Length; i++)
            {
                _answerButtons[i].Content = $"{prefixes[i]}: {_currentQuestion.Answers[i]}";
                _answerButtons[i].IsEnabled = true;
                _answerButtons[i].Background = new SolidColorBrush(Color.FromRgb(0x1a, 0x1a, 0x4e));
                _answerButtons[i].BorderBrush = System.Windows.Media.Brushes.Gold;
                _answerButtons[i].Opacity = 1;
            }
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            if (_isAnswering) return; 
            _isAnswering = true;

            int selectedIndex = int.Parse(((Button)sender).Tag.ToString());
            bool isCorrect = selectedIndex == _currentQuestion.CorrectAnswerIndex;

            
            _answerButtons[_currentQuestion.CorrectAnswerIndex].Background =
                System.Windows.Media.Brushes.Green;
            _answerButtons[_currentQuestion.CorrectAnswerIndex].BorderBrush =
                System.Windows.Media.Brushes.LimeGreen;

            
            if (!isCorrect)
            {
                _answerButtons[selectedIndex].Background =
                    System.Windows.Media.Brushes.Red;
                _answerButtons[selectedIndex].BorderBrush =
                    System.Windows.Media.Brushes.DarkRed;
            }

            var delay = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            delay.Tick += (s, _) =>
            {
                delay.Stop();
                _isAnswering = false;

                if (isCorrect)
                {
                    _gameState.CurrentLevel++;
                    UpdatePrizeLevels();

                    if (_gameState.CurrentLevel >= 15)
                    {
                        MessageBox.Show("🎉 Herzlichen Glückwunsch! Sie sind Millionär!", "Gewonnen!", MessageBoxButton.OK);
                        EndGame();
                        return;
                    }

                    LoadQuestion();
                }
                else
                {
                    _gameState.IsGameOver = true;
                    int prizeAmount = _gameState.CurrentLevel > 0 ? Amounts[_gameState.CurrentLevel - 1] : 0;
                    MessageBox.Show(
                        $"Falsch!\n\nSie verlassen das Spiel mit {prizeAmount:N0} €.",
                        "✗ Spiel vorbei",
                        MessageBoxButton.OK);
                    EndGame();
                }
            };
            delay.Start();
        }

        private void EndGame()
        {
            var entry = new HighScoreEntry
            {
                PlayerName = _gameState.PlayerName,
                Level = _gameState.CurrentLevel,
                PrizeAmount = _gameState.CurrentLevel > 0
                                          ? Amounts[_gameState.CurrentLevel - 1]
                                          : 0,
                PlayedAt = DateTime.Now,
                JokerFiftyFiftyUsed = _gameState.JokerFiftyFiftyUsed,
                JokerSwapUsed = _gameState.JokerSwapUsed
            };

            _highscoreService.SaveHighscore(entry);

            var highscoreWindow = new HighscoreWindow();
            highscoreWindow.Show();
            this.Close();
        }

        private void JokerFiftyFifty_Click(object sender, RoutedEventArgs e)
        {
            if (_gameState.JokerFiftyFiftyUsed) return;
            _gameState.JokerFiftyFiftyUsed = true;
            JokerFiftyFifty.IsEnabled = false;

            var wrongIndexes = Enumerable.Range(0, 4)
                .Where(i => i != _currentQuestion.CorrectAnswerIndex)
                .OrderBy(_ => Guid.NewGuid())
                .Take(2)
                .ToList();

            foreach (int i in wrongIndexes)
            {
                _answerButtons[i].IsEnabled = false;
                _answerButtons[i].Opacity = 0.3;
            }
        }

        private void JokerSwap_Click(object sender, RoutedEventArgs e)
        {
            if (_gameState.JokerSwapUsed) return;
            _gameState.JokerSwapUsed = true;
            JokerSwap.IsEnabled = false;

            _currentQuestion = _questionService.GetRandomQuestion(
                _gameState.CurrentLevel + 1, _currentQuestion);

            QuestionText.Text = _currentQuestion.Text;
            string[] prefixes = { "A", "B", "C", "D" };
            for (int i = 0; i < _answerButtons.Length; i++)
            {
                _answerButtons[i].Content = $"{prefixes[i]}: {_currentQuestion.Answers[i]}";
                _answerButtons[i].IsEnabled = true;
                _answerButtons[i].Opacity = 1;
            }
        }
    }
}