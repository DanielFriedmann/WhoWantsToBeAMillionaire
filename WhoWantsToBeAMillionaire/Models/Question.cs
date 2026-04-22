using System;
using System.Collections.Generic;
using System.Text;

namespace WhoWantsToBeAMillionaire.Models
{
    public class Question
    {
        public int Level { get; set; }
        public string Text { get; set; }
        public List<string> Answers { get; set; }
        public int CorrectAnswerIndex { get; set; }

    }
}
