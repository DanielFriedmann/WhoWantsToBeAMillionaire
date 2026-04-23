using System.Windows;
using System.Windows.Controls;
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

        public GameWindow(string playerName)
        {
            InitializeComponent();
            _questionService = new QuestionService();
            _highscoreService = new HighscoreService();
            _gameState = new GameState(playerName);
            _answerButtons = new Button[] { AnswerA, AnswerB, AnswerC, AnswerD };
            LoadQuestion();
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
                _answerButtons[i].Background = System.Windows.Media.Brushes.Navy;
            }
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = int.Parse(((Button)sender).Tag.ToString());

            if (selectedIndex == _currentQuestion.CorrectAnswerIndex)
            {
                _answerButtons[selectedIndex].Background = System.Windows.Media.Brushes.Green;
                _gameState.CurrentLevel++;
                MessageBox.Show("Richtig!", "✓", MessageBoxButton.OK);
                LoadQuestion();
            }
            else
            {
                _answerButtons[selectedIndex].Background = System.Windows.Media.Brushes.Red;
                MessageBox.Show("Falsch! Spiel vorbei.", "✗", MessageBoxButton.OK);
                _gameState.IsGameOver = true;
                this.Close();
            }
        }

        private void JokerFiftyFifty_Click(object sender, RoutedEventArgs e)
        {
            if (_gameState.JokerFiftyFiftyUsed) return;
            _gameState.JokerFiftyFiftyUsed = true;
            JokerFiftyFifty.IsEnabled = false;

            // Zwei falsche Antworten ausblenden
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