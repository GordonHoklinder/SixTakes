using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixTakes
{
    /// <summary>
    /// Player that plays a random card and if needed selects a random row.
    /// </summary>
    internal class RandomPlayer : Player
    {
        readonly Random rand = new();
        public override int Play()
        {
            return Hand[rand.Next() % Hand.Count];
        }

        public override int SelectRow(List<int> played)
        {
            return rand.Next() % 4;
        }
    }

    /// <summary>
    /// Player that plays a random card and if needed selects the cheapest row.
    /// </summary>
    internal class RandomReasonablePlayer : MinLineTakePlayer
    {
        readonly Random rand = new();
        public override int Play()
        {
            return Hand[rand.Next() % Hand.Count];
        }
    }
}
