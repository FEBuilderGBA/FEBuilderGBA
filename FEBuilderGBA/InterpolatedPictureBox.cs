using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Text;

namespace FEBuilderGBA
{
    //see
    //http://whoopsidaisies.hatenablog.com/entry/2013/12/20/065945
    class InterpolatedPictureBox : PictureBox
    {
        private InterpolationMode interpolation = InterpolationMode.Bicubic;
        private PixelOffsetMode pixelOffset = PixelOffsetMode.None;

        public void EnableDoubleBuffering()
        {
            this.DoubleBuffered = true;
        }

        [DefaultValue(typeof(InterpolationMode), "NearestNeighbor"),
        Description("The interpolation used to render the image.")]
        public InterpolationMode Interpolation
        {
            get { return interpolation; }
            set
            {
                if (value == InterpolationMode.Invalid)
                    throw new ArgumentException("\"Invalid\" is not a valid value."); // (Duh!)

                interpolation = value;
                Invalidate(); // Image should be redrawn when a different interpolation is selected
            }
        }

        [DefaultValue(typeof(PixelOffsetMode), "None"),
        Description("The pixelOffset used to render the image.")]
        public PixelOffsetMode PixelOffset
        {
            get { return pixelOffset; }
            set
            {
                if (value == PixelOffsetMode.Invalid)
                    throw new ArgumentException("\"Invalid\" is not a valid value."); // (Duh!)

                pixelOffset = value;
                Invalidate(); // Image should be redrawn when a different interpolation is selected
            }
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.InterpolationMode = interpolation;
            pe.Graphics.PixelOffsetMode = pixelOffset;

            base.OnPaint(pe);
        }
    }
}
