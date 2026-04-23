using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;

using WhoWantsToBeAMillionaire.Models;

namespace WhoWantsToBeAMillionaire.Services
{
    public class QuestionService
    {
        private readonly string _filePath = "Data/questions.json";
        private readonly List<Question> _allQuestions;

        public QuestionService()
        {
            string jsonString = File.ReadAllText(_filePath);
            _allQuestions = JsonSerializer.Deserialize<List<Question>>(jsonString);
        }

        public Question GetRandomQuestion(int level, Question? exclude = null)
        {
            Random rnd = new Random();

            List<Question> filteredQuestions = _allQuestions
                                               .Where(q => q.Level == level && q != exclude)
                                               .ToList();

            return filteredQuestions[rnd.Next(filteredQuestions.Count)];
        }
    }
}
