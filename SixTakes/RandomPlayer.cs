using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixTakes
{
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

    internal class RandomReasonablePlayer : MinLineTakePlayer
    {
        readonly Random rand = new();
        public override int Play()
        {
            return Hand[rand.Next() % Hand.Count];
        }
    }
}
