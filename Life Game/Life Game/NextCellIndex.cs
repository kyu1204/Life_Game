using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life_Game
{
    [Serializable]
    class NextCellIndex //변경예정 및 체크에 사용되는 특정 셀의 정보
    {
        private int array_1; //해당 위치값
        private int array_2;
        private bool life; //해당 셀의 상태

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
