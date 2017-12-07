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
        Cell c;
        Speed speedwindow;
        Brush b;
        string filename;
        int generation = 0;

        public Main()
        {
            InitializeComponent();

            c = new Cell(this.ClientSize.Width, this.ClientSize.Height);

            b = Brushes.Black;

            this.Text = generation + "세대";

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        private void NextGeneration()
        {
            c.AliveCell();

            ++generation;
            this.Text = generation + "세대";

            Invalidate();
        }

        ////////////////Paint////////////////////////
        private void DrawRec(Graphics g)
        {
            g.Clear(this.BackColor);
            for (int i = 0; i < c.ori_height; ++i)
            {
                for (int j = 0; j < c.ori_width; ++j)
                {
                    if (c.cell[i][j])
                    {
                        g.FillRectangle(b, (j * 10), (i * 10), 10, 10);
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
                if (c.cell[y][x] == true)
                {
                    c.cell[y][x] = false;
                }
                else
                {
                    c.cell[y][x] = true;
                    c.CreateChecklist(y, x);
                }
            }
            if (e.Button == MouseButtons.Right)
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
            if (c == null)
                return;
            if (c.cell.Count != 0)
            {
                int h = this.ClientSize.Height - c.ori_height;
                int w = this.ClientSize.Width - c.ori_width;

                if (h > 0)
                {
                    for (int i = h; i > 0; --i)
                    {
                        c.cell.Add(new List<Boolean>());
                        for (int j = 0; j < c.ori_width; ++j)
                        {
                            c.cell[this.ClientSize.Height - i].Add(false);
                        }
                    }
                }
                if (w > 0)
                {
                    for (int i = 0; i < this.ClientSize.Height; ++i)
                    {
                        for (int j = 0; j < w; ++j)
                        {
                            c.cell[i].Add(false);
                        }
                    }
                }
            }
            c.ori_height = this.ClientSize.Height;
            c.ori_width = this.ClientSize.Width;

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
                    Serialize(filename);

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
                    Deserialize(filename);

                    Invalidate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                }
            }
        }
        private void Serialize(String sFileName)
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

                Color b_color = (b as SolidBrush).Color;
                oBinFormat.Serialize(oFS, b_color);

                //리스트를 직렬화한다.
                oBinFormat.Serialize(oFS, c);
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

        private void Deserialize(String sFileName)
        {
            FileStream oFS = null;
            BinaryFormatter oBinFormat = null;

            try
            {
                // 불러올 파일 스트림 객체를 생성한다.
                oFS = File.Open(sFileName, FileMode.Open, FileAccess.Read);
                // 바이너리 형식으로 역 직렬화를 수행한다.
                oBinFormat = new BinaryFormatter();

                // 세대를 역직렬화 한다.
                this.generation = (int)oBinFormat.Deserialize(oFS);
                this.Text = generation + "세대";

                Color b_color = (Color)oBinFormat.Deserialize(oFS);
                this.b = new SolidBrush(b_color);
                // Cell을 역직렬화 한다.   
                this.c = (Cell)oBinFormat.Deserialize(oFS);
                this.ClientSize = new Size(c.ori_width, c.ori_height);
            }
            catch
            {
                // Do Nothing!
            }
            finally
            {
                if (oFS != null)
                    oFS.Close();
                Invalidate();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format("Left Click : Create Cell\nRight Click : Start or Stop"));
        }

        private void growthSpeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form s in Application.OpenForms)
            {
                if (s.Name == "Speed")
                    speedwindow = (Speed)s;
                else
                    speedwindow = null;
            }
            if (speedwindow == null)
            {
                speedwindow = new Speed(this);
                speedwindow.SendValue += new Speed.SendValueDelegate(Timer_speed);
                speedwindow.Show();
            }
            else
                speedwindow.Focus();
        }
        private void Timer_speed(int value)
        {
            this.timer1.Interval = value;
        }

        private void cellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if(cd.ShowDialog() == DialogResult.OK)
            {
                b = new SolidBrush(cd.Color);
            }
            Invalidate();
        }
    }
}
