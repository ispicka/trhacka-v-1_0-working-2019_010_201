﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace trhacka_v_1_0_working_2019_010_201
{
    using System;
    using System.Windows.Forms;

    public class VerticalProgressBar : ProgressBar
    {
        private SolidBrush brush = null;

        public VerticalProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (brush == null || brush.Color != this.ForeColor)
                brush = new SolidBrush(this.ForeColor);

            Rectangle rec = new Rectangle(0, 0, this.Width, this.Height);
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawVerticalBar(e.Graphics, rec);
            rec.Height = (int)(rec.Height * ((double)Value / Maximum)) - 4;
            rec.Width = rec.Width - 4;
            e.Graphics.FillRectangle(brush, 2, rec.Height, rec.Width, this.Height - 2);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x04;
                return cp;
            }
        }
    }
}
