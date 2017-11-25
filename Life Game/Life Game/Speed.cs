using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Life_Game
{
    public partial class Speed : Form
    {
        public delegate void SendValueDelegate(int value);
        public event SendValueDelegate SendValue;
        public Speed()
        {
            InitializeComponent();
            

        }
        public Speed(Main form)
        {
            InitializeComponent();

            trackBar1.Maximum = 1000;
            trackBar1.Minimum = 1;
            trackBar1.Value = 100;
            lb_value.Text = trackBar1.Value.ToString();

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            lb_value.Text = trackBar1.Value.ToString();
            SendValue(trackBar1.Value);
        }
    }
}
