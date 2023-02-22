using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixTakes
{
    internal class GameController
    {
        Game Game { get; }
        List<Player> Players { get; }

        public GameController (Game game, List<Player> players)
        {
            Game = game;
            Players = players;
        }

        public void Play()
        {
            InputHandler.PrintState(Game, Players);
            for (int round = 0; round < 10; round++)
            {
                List<int> played = new();
                for (int i = 0; i < Players.Count; i++)
                {
                    int card = Players[i].Play();
                    Game.Players[i].Hand.Remove(card);
                    played.Add(card);
                }

                List<Func<List<int>, int>> selectors = new();
                foreach (var player in Players)
                {
                    selectors.Add(player.SelectRow);
                }

                Game.PlayCards(played, selectors);
                
                InputHandler.PrintState(Game, Players);
            }
        }
    }
}
