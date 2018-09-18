using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;

namespace FEBuilderGBA
{
    //https://stackoverflow.com/questions/15926377/change-the-backcolor-of-the-toolstripseparator-control
    class ToolStripSeparatorEx : ToolStripSeparator
    {
        public ToolStripSeparatorEx()
        {
            this.Paint += ToolStripSeparatorEx_Paint;
        }

        private void ToolStripSeparatorEx_Paint(object sender, PaintEventArgs e)
        {
            // Get the separator's width and height.
            ToolStripSeparator toolStripSeparator = (ToolStripSeparator)sender;
            int width = toolStripSeparator.Width;
            int height = toolStripSeparator.Height;

            // Choose the colors for drawing.
            // I've used Color.White as the foreColor.
            Color foreColor = this.ForeColor;
            // Color.Teal as the backColor.
            Color backColor = this.BackColor;
            
            // Fill the background.
            e.Graphics.FillRectangle(new SolidBrush(backColor), 0, 0, width, height);

            // Draw the line.
            e.Graphics.DrawLine(new Pen(foreColor), 4, height / 2, width - 4, height / 2);
        }
    }
}
