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

        protected List<int> ChooseRemainingCards(Game game, int count)
        {
            List<int> result = new List<int>();
            // TODO this is too slow.
            while(result.Count != count)
            {
                int x = rng.Next(1, 105);
                if (! result.Contains(x) && !game.History.Contains(x)) result.Add(x);
            }
            return result;  
        }

        protected (int, int) MonteCarlo(List<int> hand, Game game, int depth, int iterations = 100)
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
                        scoreDifference = MonteCarlo(newHand, newGame, depth - 1).Item2;
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
