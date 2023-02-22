using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixTakes
{
    internal class UserPlayer : Player
    {
        public override int Play()
        {
            return InputHandler.GetPlayedCard(Hand);
        }

        public override int SelectRow(List<int> played)
        {
            return InputHandler.GetLine(Lines, played);
        }
    }
}
