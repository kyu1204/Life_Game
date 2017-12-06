using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life_Game
{
    class DistinctItemComparer : IEqualityComparer<NextCellIndex>
    {
        public bool Equals(NextCellIndex x, NextCellIndex y)
        {
            return (x.Array_1 == y.Array_1) && (x.Array_2 == y.Array_2) && (x.Life == y.Life);
        }

        public int GetHashCode(NextCellIndex obj)
        {
            return obj.Array_1.GetHashCode() ^ obj.Array_2.GetHashCode() ^ obj.Life.GetHashCode();
        }
    }
    class Cell
    {

        public List<List<Boolean>> cell;
        public List<NextCellIndex> nextcell;
        public int ori_width;
        public int ori_height;

        public Cell(int w, int h)
        {
            cell = new List<List<bool>>();
            nextcell = new List<NextCellIndex>();
            ori_width = w;
            ori_height = h;

            for (int i = 0; i < ori_height; ++i)
            {
                cell.Add(new List<Boolean>());
                for (int j = 0; j < ori_width; ++j)
                {
                    cell[i].Add(false);
                }
            }
        }
        public void CreateChecklist(List<NextCellIndex> checklist,int x,int y) //체크리스트 생성 --> 전체 배열을 검사하지 않고 체크리스트에 등록된 셀과 셀주변만 검사하기위해
        {
            checklist.Add(new NextCellIndex(x, y, true)); //선택된 셀 + 선텍된 셀 주변 셀 총 9개  
            checklist.Add(new NextCellIndex(x, y, true)); 

            checklist.Add(new NextCellIndex(x-1, y, true));
            checklist.Add(new NextCellIndex(x-1, y+1, true));
            checklist.Add(new NextCellIndex(x, y+1, true));
            checklist.Add(new NextCellIndex(x+1, y+1, true));
            checklist.Add(new NextCellIndex(x+1, y, true));
            checklist.Add(new NextCellIndex(x+1, y-1, true));
            checklist.Add(new NextCellIndex(x, y-1, true));
            checklist.Add(new NextCellIndex(x-1, y-1, true));

            IEnumerable<NextCellIndex> check = checklist.Distinct(new DistinctItemComparer()); //중복 검사 방지 Distinct로 중복 셀 제거 -> DistinctItemComparer 생성해서 Equals 지정
            checklist = check.ToList(); //열거자 check를 List화 시켜서 checklist에 덮어씌우기

        }

        public int NeighborCell(int x, int y, int offset_x, int offset_y)
        {
            int proposeX = x + offset_x;
            int proposeY = y + offset_y;

            bool outOfbounds = proposeX < 0 || proposeX >= ori_width || proposeY < 0 || proposeY >= ori_height;
            if (!outOfbounds)
                return cell[proposeY][proposeX] ? 1 : 0; ;
            return 0;
        }
        public void AliveCell(List<NextCellIndex> check)
        {
            int totalAlive = 0;

            for (int i = 0; i < ori_height; ++i)
            {
                for (int j = 0; j < ori_width; ++j)
                {
                    totalAlive = NeighborCell(j, i, -1, 0)
                        + NeighborCell(j, i, -1, 1)
                        + NeighborCell(j, i, 0, 1)
                        + NeighborCell(j, i, 1, 1)
                        + NeighborCell(j, i, 1, 0)
                        + NeighborCell(j, i, 1, -1)
                        + NeighborCell(j, i, 0, -1)
                        + NeighborCell(j, i, -1, -1);

                    if (cell[i][j] && (totalAlive == 2 || totalAlive == 3))
                    {
                        nextcell.Add(new NextCellIndex(i, j, true));
                    }
                    else if (!cell[i][j] && totalAlive == 3)
                    {
                        nextcell.Add(new NextCellIndex(i, j, true));
                    }
                    else
                    {
                        nextcell.Add(new NextCellIndex(i, j, false));
                    }
                }
            }

            foreach (NextCellIndex item in nextcell)
            {
                cell[item.Array_1][item.Array_2] = item.Life;
            }


            nextcell.Clear();
        }
    }
}
