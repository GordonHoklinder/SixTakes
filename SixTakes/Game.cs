using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("SixTakesUnitTest")]
namespace SixTakes
{


    internal class Line
    {
        internal static int CardValue(int card)
        {
            if (card == 55) return 7;
            else if (card % 11 == 0) return 5;
            else if (card % 10 == 0) return 3;
            else if (card % 5 == 0) return 2;
            else return 1;
        }

        public List<int> Cards { get; set; }

        public int Value { get => Cards.Select(x => CardValue(x)).Sum(); }

        public Line(int first)
        {
            Cards = new List<int> { first };
        }

        public Line (Line line)
        {
            this.Cards = new List<int>(line.Cards);
        }

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

    internal class PlayerInfo
    {
        public List<int> Hand { get; set; }

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

    internal class Game
    {
        public List<Line> Lines { get; set; }
        public List<PlayerInfo> Players { get; set;}

        public List<int> History { get; set; }

        public Game (List<Line> lines, List<PlayerInfo> players)
        { 
            Lines = lines;
            Players = players; 
            History = new List<int>();
            foreach (Line line in Lines)
            {
                History.AddRange(line.Cards);
            }
        }

        public Game(Game game)
        {
            Lines = game.Lines.ConvertAll(line => new Line(line));
            Players = game.Players.ConvertAll(player => new PlayerInfo(player));
            History = new List<int>(game.History);
        }


        public int InsertCard(int line, int card)
        {
            History.Add(card);
            return Lines[line].InsertCard(card);
        }

        public int? GetLineToPlay(int card)
        {
            var validLines = Lines.Select(line => ((int, int))( line.Cards.Last(), Lines.IndexOf(line))).Where(x => x.Item1 < card);
            if (!validLines.Any()) return null;
            else return validLines.OrderBy(x => x.Item1).Select(x => x.Item2).Last();
        }

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

        public void PlayCards(List<int> cards, List<Func<List<int>, int>>? selectors = null)
        {
            if (cards.Count != Players.Count) throw new ArgumentException($"cards have size {cards.Count} but should have {Players.Count}.");
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
