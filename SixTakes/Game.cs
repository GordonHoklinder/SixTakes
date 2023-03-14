using System.Runtime.CompilerServices;


[assembly: InternalsVisibleTo("SixTakesUnitTest")]
namespace SixTakes
{
    /// <summary>
    /// Model for one of four card lines visible to all players.
    /// </summary>
    internal class Line
    {
        /// <summary>
        /// Return the number of cows on a card.
        /// </summary>
        /// <param name="card">The value of the card.</param>
        /// <returns>Number of cows.</returns>
        internal static int CardValue(int card)
        {
            if (card == 55) return 7;
            else if (card % 11 == 0) return 5;
            else if (card % 10 == 0) return 3;
            else if (card % 5 == 0) return 2;
            else return 1;
        }

        public List<int> Cards { get; set; }
        
        /// <summary>
        /// The total amount of cows on the cards in the line.
        /// </summary>
        public int Value { get => Cards.Select(x => CardValue(x)).Sum(); }

        /// <summary>
        /// Construct a new line with the provided card as the first one.
        /// </summary>
        /// <param name="first">The only card in the row.</param>
        public Line(int first)
        {
            Cards = new List<int> { first };
        }

        public Line (Line line)
        {
            this.Cards = new List<int>(line.Cards);
        }

        /// <summary>
        /// Insert the card at the end of the line.
        /// If this would result into adding sixth card
        /// or a card with lower value than the last, replace the line with the card.
        /// </summary>
        /// <param name="card">The card to be added.</param>
        /// <returns>The number of point the operation generated.</returns>
        public int InsertCard(int card)
        {
            if (Cards.Count < 5 && card > Cards.Last())
            {
                Cards.Add(card);
                return 0;
            }       
            else
            {
                int ret = Value;
                Cards = new List<int> { card };
                return ret;
            }
        }
    }

    /// <summary>
    /// Stores game-related info about the given player.
    /// </summary>
    internal class PlayerInfo
    {
        /// <summary>
        /// A list of card values in the player's hand.
        /// </summary>
        public List<int> Hand { get; set; }

        /// <summary>
        /// Number of point the player has received so far.
        /// </summary>
        public int Points { get; set; } = 0;

        public PlayerInfo(List<int> hand)
        {
            Hand = hand;
        }

        public PlayerInfo(PlayerInfo info)
        {
            Points = info.Points;
            Hand = new List<int>(info.Hand);
        }
    }

    /// <summary>
    /// Model for the state of the game.
    /// </summary>
    internal class Game
    {
        /// <summary>
        /// The four lines visible to all players.
        /// </summary>
        public List<Line> Lines { get; set; }

        /// <summary>
        /// A list of players' states.
        /// </summary>
        public List<PlayerInfo> Players { get; set;}

        /// <summary>
        /// A hash map storing all played cards.
        /// </summary>
        public HashSet<int> History { get; set; }

        public Game (List<Line> lines, List<PlayerInfo> players)
        { 
            Lines = lines;
            Players = players; 
            History = new HashSet<int>();
            foreach (Line line in Lines)
            {
                foreach(var card in line.Cards)
                {
                    History.Add(card);
                }
            }
        }

        public Game(Game game)
        {
            Lines = game.Lines.ConvertAll(line => new Line(line));
            Players = game.Players.ConvertAll(player => new PlayerInfo(player));
            History = new HashSet<int>(game.History);
        }

        /// <summary>
        /// Insert the given card to the given row.
        /// </summary>
        /// <param name="line">The index of the row.</param>
        /// <param name="card">The card value.</param>
        /// <returns>The amount of points the play generates.</returns>
        public int InsertCard(int line, int card)
        {
            History.Add(card);
            return Lines[line].InsertCard(card);
        }

        /// <summary>
        /// Get the index of the line where the card is supposed to be played.
        /// 
        /// The card is supposed to be played in the line where the last card satifies:
        /// - The last card is lower than the card played.
        /// - It is the highest of those allowed by the previous rule.
        /// </summary>
        /// <param name="card">Value of the card to be played.</param>
        /// <returns>The index of the correct line. If there is none, return null.</returns>
        public int? GetLineToPlay(int card)
        {
            var validLines = Lines.Select(line => ((int, int))( line.Cards.Last(), Lines.IndexOf(line))).Where(x => x.Item1 < card);
            if (!validLines.Any()) return null;
            else return validLines.OrderBy(x => x.Item1).Select(x => x.Item2).Last();
        }

        /// <summary>
        /// Get the index of the row with the lowest sum of points.
        /// </summary>
        /// <returns>The index of the cheapest row.</returns>
        public int CheapestRow()
        {
            int best = 100;
            int bestline = 0;
            for (int i = 0; i < Lines.Count; i++)
            {
                int value = Lines[i].Value;
                if (value < best)
                {
                    best = value;
                    bestline = i;
                }
            }
            return bestline;
        }

        /// <summary>
        /// Simulate one turn of the game.
        /// 
        /// Start from the lowest to the highest card.
        /// Put the card to the row with the highest lower card.
        /// If there is no such row, select the row via the provided selector.
        /// </summary>
        /// <param name="cards">The list of cards played, index corresponds to a player.</param>
        /// <param name="selectors">The list of functions for selecting a row in case of no valid row. If null, always take the cheapest row.</param>
        /// <exception cref="ArgumentException">Exception is thorwn if any of the arguments passed does not have size equal to the size of [Players].</exception>
        public void PlayCards(List<int> cards, List<Func<List<int>, int>>? selectors = null)
        {
            if (cards.Count != Players.Count) throw new ArgumentException($"cards have size {cards.Count} but should have {Players.Count}.");
            if (selectors is not null && selectors.Count != Players.Count) throw new ArgumentException($"selectors have size {selectors.Count} but should have {Players.Count}.");
            List<(int, int)> indexedCards = new();
            for (int i = 0; i < cards.Count; i++)
            {
                indexedCards.Add((cards[i], i));
            }
            indexedCards.Sort();
            foreach(var card in indexedCards)
            {
                int index = card.Item2;
                int value = card.Item1;
                int? line = GetLineToPlay(value);
                if (line == null)
                {
                    if (selectors is null)
                    {
                        line = CheapestRow();
                    }
                    else
                    {
                        line = selectors[index](cards);
                    }
                }
                Players[index].Points += InsertCard((int)line, value);
            }
        }
    }
}
