using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixTakes
{
    /// <summary>
    /// Class for handling all the interaction with user via CLI.
    /// </summary>
    internal static class InputHandler
    {
        /// <summary>
        /// Print the cards played this turn.
        /// </summary>
        /// <param name="playedCards">The values of cards played this turn.</param>
        public static void PrintPlayedCards(List<int> playedCards)
        {
            Console.WriteLine("Cards played this turn: " + String.Join(" ", playedCards));
        }

        /// <summary>
        /// Print the current content of lines visible to all players.
        /// </summary>
        /// <param name="lines">List of 4 lines.</param>
        public static void PrintLines(List<Line> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Console.WriteLine($"Line {i}: " + String.Join(" ", lines[i].Cards));
            }
        }

        /// <summary>
        /// Retrieve the line to be taken in case the played card is lower than all the lines.
        /// </summary>
        /// <returns>The line entered by user. It is an integer from 0 to 3.</returns>
        public static int GetLine(List<Line> lines, List<int> playedCards)
        {
            Console.WriteLine("Enter a line to be taken.");
            PrintPlayedCards(playedCards);
            PrintLines(lines);
            do
            {
                if (ushort.TryParse(Console.ReadLine(), out ushort line))
                {
                    if (line is < 4 and >= 0) return line;
                }
                else
                {
                    Console.WriteLine("Not a valid integer between 0 and 3.");
                }
            }
            while (true);
        }

        /// <summary>
        /// Print the current score for all players
        /// </summary>
        public static void PrintPlayers(Game game)
        {
            for(int i = 0; i < game.Players.Count;i++)
            {
                Console.WriteLine($"Player {i}: {game.Players[i].Points} cows");
            }
        }

        /// <summary>
        /// Print the cards in hand of the current player.
        /// </summary>
        public static void PrintHand(List<int> hand)
        {
            Console.WriteLine("Hand: " + String.Join(" ", hand));
        }

        /// <summary>
        /// Print the current scores, lines and for all user players their hands.
        /// </summary>
        public static void PrintState(Game game, List<Player> players)
        {
            PrintPlayers(game);
            PrintLines(game.Lines);

            for (int i = 0; i < players.Count;i++)
            {
                if (players[i] is UserPlayer)
                {
                    PrintHand(game.Players[i].Hand);
                }
            }
        }

        /// <summary>
        /// Retrieves a card to be played by the user player.
        /// </summary>
        /// <returns>An integer from [hand].</returns>
        public static int GetPlayedCard(List<int> hand)
        {
            Console.WriteLine("Choose a card to play.");
            PrintHand(hand);
            do
            {
                if (ushort.TryParse(Console.ReadLine(), out ushort card))
                {
                    if (hand.Contains(card)) return card;
                }
                Console.WriteLine("Not a valid integer from the hand.");              
            }
            while (true);
        }

        /// <summary>
        /// Get the player type from its one character code.
        /// </summary>
        /// <param name="type">The one character coding.</param>
        /// <param name="id">The index of the player in the player list.</param>
        /// <returns>The handler class for a given type. null if the type is not recognized.</returns>
        static Player? GetPlayer(char type, int id)
        {
            return type switch
            {
                'R' => new RandomPlayer { ID = id },
                'U' => new UserPlayer { ID = id },
                'r' => new RandomReasonablePlayer { ID= id },
                'C' => new ClosestValuePlayer { ID= id },
                'M' => new MonteCarloPlayer { ID = id },
                'E' => new ExpectedValuePlayer { ID = id },
                _ => (Player?)null,
            } ;
        }

        public static List<Player> GetPlayers()
        {
            Console.WriteLine("Enter the players for the game.");
            Console.WriteLine("Players are expected to be a string of 2 to 10 characters.");
            Console.WriteLine("Each player is of a given type and should be represented by one character.");
            Console.WriteLine("Supported player types are:");
            Console.WriteLine("  U - user player");
            Console.WriteLine("  R - random player");
            Console.WriteLine("  r - random player selecting the cheapest row");
            Console.WriteLine("  C - player playing the closest card");
            Console.WriteLine("  M - Monte Carlo player");
            Console.WriteLine("  E - Player minimizing expected value of loss");

            do
            {
                string? entry = Console.ReadLine();
                if (entry is string s && s.Length is <= 10 and >= 2)
                {
                    var ret = new List<Player>();
                    for  (int i = 0; i < s.Length; i++)
                    {
                        var player = GetPlayer(s[i], i);
                        if (player is not null) ret.Add(player);
                        else Console.WriteLine($"Unsupported player type {s[i]}");
                    }
                    if (ret.Count == entry.Length)
                    {
                        return ret;
                    }
                }
                else
                {
                    Console.WriteLine("The number of players must be between 2 and 10.");
                }
            }
            while(true);
        }

        public static int GetRounds()
        {
            Console.WriteLine("Enter the number of rounds.");
            do
            {
                if (ushort.TryParse(Console.ReadLine(), out ushort rounds))
                {
                    return rounds;
                }
                else
                {
                    Console.WriteLine("Not a valid positive integer. Enter again.");
                }
            }
            while(true);
        }
    }
}
