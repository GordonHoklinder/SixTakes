using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixTakes
{
    internal class ClosestValuePlayer : MinLineTakePlayer
    {
        public override int Play()
        {
            const int unassignableOffset = 400;
            int? best = null;
            int? bestValue = null;

            foreach (int card in Hand) {
                int? line = Game?.GetLineToPlay(card);
                if (line.HasValue)
                {
                    if (Lines[(int)line].Cards.Count < 5)
                    {
                        int diff = card - Lines[(int)line].Cards.Last();
                        if (best is null || best > diff)
                        { 
                            best = diff;
                            bestValue = card;
                        }
                    }
                }
                else
                {
                    int diff = unassignableOffset - card;
                    if (best is null || best > diff)
                    {
                        best = diff;
                        bestValue = card;
                    }
                }
            }

            return bestValue ?? Hand.Max();
        }
    }
}
