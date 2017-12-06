using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life_Game
{
    class DistinctItemComparer : IEqualityComparer<NextCellIndex> //중복제거 
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
    [Serializable]
    class Cell
    {
        public List<List<Boolean>> cell;
        public List<NextCellIndex> nextcell;
        public List<NextCellIndex> checklist;

        public int ori_width;
        public int ori_height;

        public Cell(int w, int h) 
        {
            cell = new List<List<bool>>(); //창 크기를 받아와 1:1로 매칭되는 셀 테이블 생성
            nextcell = new List<NextCellIndex>(); //NextGeneration 시 변경예정목록
            checklist = new List<NextCellIndex>();
            ori_width = w; //현재 창의 가로
            ori_height = h; //현재 창의 세로

            for (int i = 0; i < ori_height; ++i)
            {
                cell.Add(new List<Boolean>()); //셀 테이블 생성
                for (int j = 0; j < ori_width; ++j)
                {
                    cell[i].Add(false);
                }
            }
        }
        public void CreateChecklist(int h,int w) //체크리스트 생성 --> 전체 배열을 검사하지 않고 체크리스트에 등록된 셀과 셀주변만 검사하기위해
        {
            OutofBoundsCheck(h, w); //선택된 셀 + 선텍된 셀 주변 셀 총 9개
            OutofBoundsCheck(h, w-1);
            OutofBoundsCheck(h + 1, w-1);
            OutofBoundsCheck(h+1, w);
            OutofBoundsCheck(h+1, w+1);
            OutofBoundsCheck(h, w+1);
            OutofBoundsCheck(h-1, w+1);
            OutofBoundsCheck(h-1, w);
            OutofBoundsCheck(h-1, w-1);

            IEnumerable<NextCellIndex> check = checklist.Distinct(new DistinctItemComparer()); //중복 방지 Distinct로 중복 셀 제거 -> DistinctItemComparer 생성해서 Equals 지정
            List<NextCellIndex> tmp = check.ToList(); //열거자가 가지고있는 Result View 요소를 List화 시켜서 tmp에 저장
            checklist.Clear(); //중복제거 하기전 기존 체크리스트 비우기
            for(int i = 0; i<tmp.Count;++i)
            {
                checklist.Add(tmp[i]); //중복제거된 요소로 재등록
            }
        }
        void OutofBoundsCheck(int y, int x)
        {
            bool outOfbounds = x < 0 || x >= ori_width || y < 0 || y >= ori_height; //창 크기를 벗어나는지 검사
            if (!outOfbounds)
                checklist.Add(new NextCellIndex(y, x, true)); //벗어나지 않으면 체크리스트에 등록
        }

        int NeighborCell(int x, int y, int offset_x, int offset_y) //이웃 셀 검사 함수
        {
            int proposeX = x + offset_x; //현재 위치값 + offset값 으로 검사할 위치값 얻기
            int proposeY = y + offset_y;

            bool outOfbounds = proposeX < 0 || proposeX >= ori_width || proposeY < 0 || proposeY >= ori_height; //창 크기를 벗어나는 위치는 검사 x
            if (!outOfbounds)
                return cell[proposeY][proposeX] ? 1 : 0; ; //검사할 위치의 셀에 상태에따라 살아있으면 1 죽어있으면 0 리턴
            return 0;
        }
        public void AliveCell() //수정된 주변 셀 검사
        {
            int totalAlive = 0;

            foreach(NextCellIndex item in checklist) //기존 전체 화면 전체크기의 셀 검사에서 체크된 셀 주변 8개(중복제외)만 검사(checklist)
            {
                int h = item.Array_1, w = item.Array_2; //체크리스트에 등록된 셀의 주변 셀 검사
                totalAlive = NeighborCell(w, h, -1, 0)  //지금 셀 기준으로 주변셀의 생존 개수 얻어오기
                        + NeighborCell(w, h, -1, 1)
                        + NeighborCell(w, h, 0, 1)
                        + NeighborCell(w, h, 1, 1)
                        + NeighborCell(w, h, 1, 0)
                        + NeighborCell(w, h, 1, -1)
                        + NeighborCell(w, h, 0, -1)
                        + NeighborCell(w, h, -1, -1);
                if (cell[h][w] && (totalAlive == 2 || totalAlive == 3)) //주변 셀 개수의 따라 다음세대에 살아남을지 죽을지 결정(Life Game 핵심 알고리즘)
                {
                    nextcell.Add(new NextCellIndex(h, w, true)); //변경 예정인 셀들의 목록인 nextcell에 등록   
                }
                else if (!cell[h][w] && totalAlive == 3)
                {
                    nextcell.Add(new NextCellIndex(h, w, true));
                }
                else
                {
                    nextcell.Add(new NextCellIndex(h, w, false));
                }
            }

            foreach (NextCellIndex item in nextcell) //nextcell에 들어있는 셀들의 상태만 골라서 전체 셀에 적용
            {
                cell[item.Array_1][item.Array_2] = item.Life;
                if (item.Life)
                    CreateChecklist(item.Array_1, item.Array_2);
            }


            nextcell.Clear(); //변경예정목록 초기화
        }
    }
}
