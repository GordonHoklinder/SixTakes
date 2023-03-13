using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixTakes
{
    internal class Dealer
    {
        int Players { get; set; }
        
        public Dealer(int players)
        { 
            Players = players; 
        }

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
                for (int j = 0; j < 10; ++j) hand.Add(values[4 + j + 10 *i]);
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
