using System;
using System.Collections.Generic;
using System.Text;

namespace WhoWantsToBeAMillionaire.Models
{
    public class HighScoreEntry
    {
        public string PlayerName { get; set; }
        public int Level { get; set; }
        public int PrizeAmount { get; set; }
        public DateTime PlayedAt { get; set; }
        public bool JokerFiftyFiftyUsed { get; set; }
        public bool JokerSwapUsed { get; set; }
    }
}
