using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SixTakes
{
    internal class MonteCarloPlayer : MinLineTakePlayer
    {
        Random rng = new Random();

        protected List<int> ChooseRemainingCardsBySorting(Game game, int count)
        {
            List<bool> present = new List<bool>(new bool [105]);
            List<int> result = new List<int>();
            for (int card = 1; card < 105; card++)
            {
                if (!result.Contains(card) && !game.History.Contains(card)) result.Add(card);
            }
            return result.OrderBy(x => rng.Next()).Take(count).ToList();
        }

        protected List<int> ChooseRemainingCardsRandomly(Game game, int count)
        {
            List<int> result = new List<int>();
            while (result.Count != count)
            {
                int x = rng.Next(1, 105);
                if (!result.Contains(x) && !game.History.Contains(x)) result.Add(x);
            }
            return result;
        }

        protected List<int> ChooseRemainingCards(Game game, int count)
        {
            int countPlusHistory = count + game.History.Count;
            // If the expected number of operations of randomly probing values exceeds
            // estimated number of operations of sorting, choose the cards via sorting.
            if (count * countPlusHistory * (1 - ((double)countPlusHistory) / 105) > 630)
            {
                return ChooseRemainingCardsBySorting(game, count);
            }
            return ChooseRemainingCardsRandomly(game, count);           
        }

        protected (int, int) MonteCarlo(List<int> hand, Game game, int depth, int iterations = 200)
        {
            // Stores the highest scaled value of Score - average of Score of opponents.
            List<int> worstCaseScore = new List<int>(new int[hand.Count]);
            int playerCount = game.Players.Count;

            for (int i = 0; i < hand.Count; i++)
            {
                for (int j = 0; j < iterations; j++)
                {
                    var played = ChooseRemainingCards(game, playerCount - 1);
                    played.Insert(ID, hand[i]);
                    Game newGame = new Game(game);
                    newGame.PlayCards(played);

                    int scoreDifference;
                    if (depth == 1) scoreDifference = playerCount * newGame.Players[ID].Points - newGame.Players.Select(x => x.Points).Sum();
                    else
                    {
                        var newHand = new List<int>(hand);
                        newHand.RemoveAt(i);
                        // Scale down the number of iterations in the search tree.
                        int newIterations = (int)Math.Sqrt(iterations);
                        scoreDifference = MonteCarlo(newHand, newGame, depth - 1, newIterations).Item2;
                    }
                    worstCaseScore[i] = Math.Max(worstCaseScore[i], scoreDifference);
                }
            }

            int worst = worstCaseScore.Min();
            return (worstCaseScore.IndexOf(worst), worst);
        }

        protected int MonteCarlo(int depth = 2)
        {
            return MonteCarlo(Hand, Game, Math.Min(Hand.Count, depth)).Item1;
        }


        public override int Play()
        {
            return Hand[MonteCarlo()];
        }
    }
}
