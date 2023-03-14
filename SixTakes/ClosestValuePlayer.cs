using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixTakes
{
    /// <summary>
    /// Player always choosing the card which is always closest (from above) to the last card in a line that does no take the line.
    /// In case no such card exists, select the highest non taking any line.
    /// </summary>
    internal class ClosestValuePlayer : MinLineTakePlayer
    {
        public override int Play()
        {
            // unassignableOffset is set to an arbitrary large anough constant.
            const int unassignableOffset = 400;
            int? best = null;
            int? bestValue = null;

            foreach (int card in Hand) {
                int? line = Game?.GetLineToPlay(card);
                if (line.HasValue)
                {
                    // Consider only those which do not take a line.
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
                    // From the cards lower than all the lines, prefer the higher.
                    int diff = unassignableOffset - card;
                    if (best is null || best > diff)
                    {
                        best = diff;
                        bestValue = card;
                    }
                }
            }
            // If all cards take a line, choose the globally highest one.
            return bestValue ?? Hand.Max();
        }
    }
}
