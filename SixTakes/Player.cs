using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixTakes
{
    /// <summary>
    /// Class for storing information about the type of player
    /// and provides interface for interaction with the game.
    /// </summary>
    internal abstract class Player
    {
        public int ID { get; init; }

        public Game? Game { get; set; }

        protected List<int> Hand { get => Game?.Players?[ID]?.Hand; }

        protected List<Line> Lines { get => Game?.Lines ?? new List<Line>(); }

        /// <summary>
        /// Get the card to be played.
        /// </summary>
        /// <returns>The card to be played.</returns>
        public abstract int Play();

        /// <summary>
        /// Choose the line to be taken in case the played card is lower than all the last cards in the lines.
        /// </summary>
        /// <param name="played">The cards played by all players this turn.</param>
        /// <returns>The index of the row to be taken. Should be from 0 to 3.</returns>
        public abstract int SelectRow(List<int> played); 
    }

    /// <summary>
    /// Template player who always takes the cheapest row.
    /// </summary>
    internal abstract class MinLineTakePlayer : Player
    {
        public override int SelectRow(List<int> played)
        {
            return Game.CheapestRow();
        }
    }
}
