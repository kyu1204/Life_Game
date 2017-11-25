using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Life_Game
{
    public partial class Main : Form
    {
        List<List<Boolean>> cell;
        List<NextCellIndex> nextcell;
        string filename;
        int ori_width;
        int ori_height;
        int generation = 0;

        public Main()
        {
            cell = new List<List<Boolean>>();
            nextcell = new List<NextCellIndex>();
            InitializeComponent();

            ori_width = this.ClientSize.Width;
            ori_height = this.ClientSize.Height;

            for (int i = 0; i < ori_height; ++i)
            {
                cell.Add(new List<Boolean>());
                for (int j = 0; j < ori_width; ++j)
                {
                    cell[i].Add(false);
                }
            }

            this.Text = generation + "세대";

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        private void NextGeneration()
        {
            int totalAlive = 0;
            for(int i=0;i<this.ClientSize.Height;++i)
            {
                for(int j =0;j<this.ClientSize.Width;++j)
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

            foreach(NextCellIndex item in nextcell)
            {
                cell[item.Array_1][item.Array_2] = item.Life;
            }


            nextcell.Clear();

            ++generation;
            this.Text = generation + "세대";
            
            Invalidate();
        }
        private int NeighborCell(int x,int y,int offset_x,int offset_y)
        {
            int proposeX = x + offset_x;
            int proposeY = y + offset_y;

            bool outOfbounds = proposeX < 0 || proposeX >= this.ClientSize.Width || proposeY < 0 || proposeY >= this.ClientSize.Height;
            if (!outOfbounds)
                return  cell[proposeY][proposeX] ? 1 : 0; ;
            return 0;
        }

////////////////Paint////////////////////////
        private void DrawRec(Graphics g)
        {
            g.Clear(this.BackColor);
            for (int i = 0; i < ori_height; ++i)
            {
                for (int j = 0; j < ori_width; ++j)
                {
                    if (cell[i][j])
                    {
                        g.FillRectangle(Brushes.Black, (j * 10), (i * 10), 10, 10);
                    }
                }
            }

            DrawLine(g);
        }

        private void DrawLine(Graphics g)
        {
            for (int cor = 10; cor < this.ClientSize.Width; cor += 10)
            {
                g.DrawLine(Pens.DarkGray, cor, 0, cor, this.ClientSize.Height);
            }
            for (int row = 10; row < this.ClientSize.Height; row += 10)
            {
                g.DrawLine(Pens.DarkGray, 0, row, this.ClientSize.Width, row);
            }
        }

/////////// Event//////////////////////
        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = (int)e.X / 10;
                int y = (int)e.Y / 10;
                if (cell[y][x] == true)
                    cell[y][x] = false;
                else
                    cell[y][x] = true;
            }
            if(e.Button == MouseButtons.Right)
            {
                if (timer1.Enabled == true)
                    timer1.Enabled = false;
                else
                    timer1.Enabled = true;
            }

            Invalidate();
        }

        private void Main_Paint(object sender, PaintEventArgs e)
        {
            DrawRec(e.Graphics);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void Main_ClientSizeChanged(object sender, EventArgs e)
        {
            if (cell.Count != 0)
            {
                int h = this.ClientSize.Height - ori_height;
                int w = this.ClientSize.Width - ori_width;

                if (h > 0)
                {
                    for (int i = h; i > 0; --i)
                    {
                        cell.Add(new List<Boolean>());
                        //nextcell.Add(new List<Boolean>());
                        for (int j = 0; j < ori_width; ++j)
                        {
                            cell[this.ClientSize.Height - i].Add(false);
                            //nextcell[this.ClientSize.Height - i].Add(false);
                        }
                    }
                }
                if (w > 0)
                {
                    for (int i = 0; i < this.ClientSize.Height; ++i)
                    {
                        for (int j = 0; j < w; ++j)
                        {
                            cell[i].Add(false);
                            //nextcell[i].Add(false);
                        }
                    }
                }
            }
            ori_height = this.ClientSize.Height;
            ori_width = this.ClientSize.Width;

            Invalidate();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog spd = new SaveFileDialog();

            spd.DefaultExt = ".life";             // 기본 파일타입 설정
            spd.Filter = "Life files (*.life)|*.life"; // 파일타입

            string strAppDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            spd.InitialDirectory = strAppDir;   // 파일불러오기를 했을 때 제일 처음에 열리는 디렉토리 설정

            if (spd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    filename = spd.FileName;
                    Serialize(cell, filename);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog();

            opd.DefaultExt = "All files";                               // 기본 파일타입 설정
            opd.Filter = "All files (*.*)|*.*"; // 파일타입
            opd.Multiselect = false;            // 다중선택되지 않도록.               
            string strAppDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            opd.InitialDirectory = strAppDir;   // 파일불러오기를 했을 때 제일 처음에 열리는 디렉토리 설정

            if (opd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (!opd.SafeFileName.EndsWith(".life"))
                    {
                        MessageBox.Show(".life 파일만 열 수 있습니다!");
                        return;
                    }

                    filename = opd.FileName;
                    Deserialize(cell, filename);
                    Invalidate();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
            }
        }
        private void Serialize(List<List<Boolean>> cell, String sFileName)
        {
            FileStream oFS = null;
            BinaryFormatter oBinFormat = null;

            try
            {
                // 저장할 파일 스트림 객체를 생성한다.
                oFS = File.Open(sFileName, FileMode.Create, FileAccess.Write);
                // 바이너리 형식으로 직렬화를 수행한다.
                oBinFormat = new BinaryFormatter();

                //현재 세대를 직렬화 한다
                oBinFormat.Serialize(oFS, generation);
                //리스트를 직렬화한다.
                oBinFormat.Serialize(oFS, cell);
            }
            catch
            {
                // Do Nothing!
            }
            finally
            {
                if (oFS != null)
                    oFS.Close();
            }
        }

        private void Deserialize(List<List<Boolean>> cell, String sFileName)
        {
            FileStream oFS = null;
            BinaryFormatter oBinFormat = null;
            List<List<Boolean>> tmp;

            try
            {
                // 불러올 파일 스트림 객체를 생성한다.
                oFS = File.Open(sFileName, FileMode.Open, FileAccess.Read);
                // 바이너리 형식으로 역 직렬화를 수행한다.
                oBinFormat = new BinaryFormatter();

                // 세대를 역직렬화 한다.
                this.generation = (int)oBinFormat.Deserialize(oFS);
                this.Text = generation + "세대";
                // 리스트를 역직렬화 한다.   
                tmp = (List<List<Boolean>>)oBinFormat.Deserialize(oFS);

                //Deep Copy
                for (int i = 0; i < tmp.Count; ++i)
                {
                    for(int j = 0; j<tmp[0].Count;++j)
                    {
                        cell[i][j] = tmp[i][j];
                    }
                }
            }
            catch
            {
                // Do Nothing!
            }
            finally
            {
                if (oFS != null)
                    oFS.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format("Mouse_right Click : Start or Stop"));
        }

        
    }
}
