using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixTakes
{
    /// <summary>
    /// Class responsible for generating new games.
    /// </summary>
    internal class Dealer
    {
        int Players { get; set; }
        
        public Dealer(int players)
        { 
            Players = players; 
        }

        /// <summary>
        /// Generate a new game.
        /// 
        /// Create four rows with one random card each
        /// and give each of [Players] players 10 random cards.
        /// All cards are from 1 to 104 (including).
        /// </summary>
        /// <param name="old">Previous game. If provided,
        /// initialize players' score to the score from the previous game.</param>
        /// <returns>The generated game</returns>
        public Game Deal(Game? old)
        {
            var values = new List<int>();
            for (int i = 1; i < 105; i++) values.Add(i);           
            var rand = new Random();
            values = values.OrderBy(x => rand.Next()).ToList();
            
            var lines = new List<Line>();
            for (int i = 0; i < 4; i++) lines.Add(new Line(values[i]));
            var players = new List<PlayerInfo>();

            for (int i = 0; i < Players; i++)
            {
                var hand = new List<int>();
                for (int j = 0; j < 10; ++j) hand.Add(values[4 + j + 10 * i]);
                players.Add(new PlayerInfo(hand));
                if (old is not null)
                {
                    players.Last().Points = old.Players[i].Points;
                }
            }

            return new Game(lines, players);
        }
    }
}
