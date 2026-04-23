using System;
using System.Collections.Generic;
using System.Text;

namespace WhoWantsToBeAMillionaire.Models
{
    public class GameState
    {

        public int CurrentLevel { get; set; } = 0;
        public bool JokerFiftyFiftyUsed { get; set; } = false;
        public bool JokerSwapUsed { get; set; } = false;
        public bool IsGameOver { get; set; } = false;
        public string PlayerName { get; set; }
        public GameState(string playerName)
        {
            PlayerName = playerName;
        }

        

    }
}
