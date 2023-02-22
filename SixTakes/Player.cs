using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixTakes
{
    internal abstract class Player
    {
        public int ID { get; init; }

        public Game? Game { get; set; }

        protected List<int> Hand { get => Game?.Players?[ID]?.Hand; }

        protected List<Line> Lines { get => Game?.Lines ?? new List<Line>(); }

        public abstract int Play();

        public abstract int SelectRow(List<int> played); 
    }

    internal abstract class MinLineTakePlayer : Player
    {
        public override int SelectRow(List<int> played)
        {
            return Game.CheapestRow();
        }
    }
}
