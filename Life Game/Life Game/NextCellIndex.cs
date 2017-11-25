using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life_Game
{
    class NextCellIndex
    {
        private int array_1;
        private int array_2;
        private bool life;

        public int Array_1 { get { return array_1; } }
        public int Array_2 { get { return array_2; } }
        public bool Life { get { return life; } }

        public NextCellIndex(int i,int j, bool life)
        {
            this.array_1 = i;
            this.array_2 = j;
            this.life = life;
        }
    }
}
