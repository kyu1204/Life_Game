namespace Life_Game
{
    partial class Speed
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_speed = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.lb_value = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // lb_speed
            // 
            this.lb_speed.AutoSize = true;
            this.lb_speed.Location = new System.Drawing.Point(12, 19);
            this.lb_speed.Name = "lb_speed";
            this.lb_speed.Size = new System.Drawing.Size(161, 15);
            this.lb_speed.TabIndex = 0;
            this.lb_speed.Text = "성장 속도(millisecond):";
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 100;
            this.trackBar1.Location = new System.Drawing.Point(15, 46);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(255, 56);
            this.trackBar1.SmallChange = 100;
            this.trackBar1.TabIndex = 100;
            this.trackBar1.TickFrequency = 100;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // lb_value
            // 
            this.lb_value.AutoSize = true;
            this.lb_value.Location = new System.Drawing.Point(179, 19);
            this.lb_value.Name = "lb_value";
            this.lb_value.Size = new System.Drawing.Size(0, 15);
            this.lb_value.TabIndex = 2;
            // 
            // Speed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 110);
            this.Controls.Add(this.lb_value);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.lb_speed);
            this.Name = "Speed";
            this.Text = "Speed";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_speed;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label lb_value;
    }
}