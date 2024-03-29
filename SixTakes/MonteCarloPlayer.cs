﻿namespace SixTakes
{
    /// <summary>
    /// Player which for each action considers the worst-case scenario
    /// generated by Monte Carlo method and selects the one with the least bad.
    /// </summary>
    internal class MonteCarloPlayer : MinLineTakePlayer
    {
        readonly protected Random rng = new();

        /// <summary>
        /// Aggregate a new simulation score result to the aggregated value.
        /// </summary>
        /// <param name="global">The previous aggregated value.</param>
        /// <param name="next">The score of the new simulation.</param>
        /// <returns>The current aggregated value.</returns>
        protected virtual int Aggregate(int global, int next)
        {
            return Math.Max(global, next);
        }

        /// <summary>
        /// Decide which card should be played based on their aggregated scores.
        /// </summary>
        /// <param name="scores">Aggregated scores corresponding to the cards.</param>
        /// <returns>The index of the card to be played.</returns>
        protected virtual int Decide (List<int> scores)
        {
            return scores.IndexOf(scores.Min());
        }

        /// <summary>
        /// The depth of the Monte Carlo search.
        /// </summary>
        protected virtual int SearchDepth() => 2;
        
        /// <summary>
        /// The number of instances of opponent plays in the first round of Monte Carlo search.
        /// </summary>
        protected virtual int InitialIterations() => 100;

        /// <summary>
        /// Return distinct list of size [count] of cards which haven't been played.
        /// 
        /// The algorithmic approach to this is via sorting the remaining cards.
        /// Thus the time complexity is O(n log n) where n is the number of not yet played cards.
        /// </summary>
        /// <param name="game">The current game.</param>
        /// <param name="count">Number of cards to be retrieved.</param>
        /// <returns>Distinct not yet played cards.</returns>
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

        /// <summary>
        /// Return distinct list of size [count] of cards which haven't been played.
        /// 
        /// The algorithmic approach to this is via probing the cards until the card hasn't been yet played.
        /// The average time complexity is O(count^2 * ratio of unplayed cards)
        /// </summary>
        /// <param name="game">The current game.</param>
        /// <param name="count">Number of cards to be retrieved.</param>
        /// <returns>Distinct not yet played cards.</returns>
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

        /// <summary>
        /// Return distinct list of size [count] of cards which haven't been played.
        /// 
        /// This selects the better option from sorting the values and probing randomly.
        /// </summary>
        /// <param name="game">The current game.</param>
        /// <param name="count">Number of cards to be retrieved.</param>
        /// <returns>Distinct not yet played cards.</returns>
        protected List<int> ChooseRemainingCards(Game game, int count)
        {
            int countPlusHistory = count + game.History.Count;
            int remainingCards = 105 - game.History.Count;
            // If the expected number of operations of randomly probing values exceeds
            // estimated number of operations of sorting, choose the cards via sorting.
            if (count * count * (1 - ((double)countPlusHistory) / 105) > remainingCards * Math.Log2(remainingCards)) 
            {
                return ChooseRemainingCardsBySorting(game, count);
            }
            return ChooseRemainingCardsRandomly(game, count);
        }

        /// <summary>
        /// Retrieve the best cards to be played by running the Monte Carlo algoritm.
        /// </summary>
        /// <param name="hand">Reamining cards in the hand.</param>
        /// <param name="game">The current game.</param>
        /// <param name="depth">Depth of the search.</param>
        /// <param name="iterations">The number of iterations in the first level of search.</param>
        /// <returns>Pair, where the first element is index of the best response
        /// and the second is the accumulated score for that card.</returns>
        protected (int, int) MonteCarlo(List<int> hand, Game game, int depth, int iterations)
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
                    worstCaseScore[i] = Aggregate(worstCaseScore[i], scoreDifference);
                }
            }

            int choice = Decide(worstCaseScore);
            return (choice, worstCaseScore[choice]);
        }

        protected int MonteCarlo()
        {
            return MonteCarlo(Hand, Game, Math.Min(Hand.Count, SearchDepth()), InitialIterations()).Item1;
        }


        public override int Play()
        {
            return Hand[MonteCarlo()];
        }
    }

    /// <summary>
    /// Player that tries to minimize the expected score via Monte Carlo algorithm, provided other player play randomly.
    /// </summary>
    internal class ExpectedValuePlayer : MonteCarloPlayer
    {
        protected sealed override int Aggregate(int global, int next)
        {
            return global + next;
        }
    }
}
