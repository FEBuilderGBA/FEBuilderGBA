using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace FEBuilderGBA
{
    //see
    //http://whoopsidaisies.hatenablog.com/entry/2013/12/20/065945
    class SystemIconPictureBox : PictureBox
    {
        private InterpolationMode interpolation = InterpolationMode.Bicubic;
        private PixelOffsetMode pixelOffset = PixelOffsetMode.None;

        public void EnableDoubleBuffering()
        {
        }

        public enum IconTypeEnum
        {
             Weapon
            ,Attribute
            ,Cursol
            ,Yubi
            ,Exit
            ,Allow
            ,Music
            ,Null
        }
        IconTypeEnum iconType = IconTypeEnum.Weapon;
        [DefaultValue(typeof(IconTypeEnum), "Weapon"),
        Description("Icon Type.")]
        public IconTypeEnum IconType
        {
            get { return iconType; }
            set
            {
                iconType = value;
                DrawIcon();
                Invalidate(); // Image should be redrawn when a different interpolation is selected
            }
        }

        uint iconNumber = 0;
        [DefaultValue(typeof(uint), "0"),
        Description("Icon Type.")]
        public uint IconNumber
        {
            get { return iconNumber; }
            set
            {
                iconNumber = value;
                DrawIcon();
                Invalidate(); // Image should be redrawn when a different interpolation is selected
            }
        }

        void DrawIcon()
        {
            if (Program.ROM == null)
            {
                return;
            }
            Bitmap b;
            if (IconTypeEnum.Weapon == iconType)
            {
                b = ImageSystemIconForm.WeaponIcon(iconNumber);
            }
            else if (IconTypeEnum.Attribute == iconType)
            {
                b = ImageSystemIconForm.Attribute(iconNumber);
            }
            else if (IconTypeEnum.Cursol == iconType)
            {
                b = ImageSystemIconForm.Cursol();
            }
            else if (IconTypeEnum.Yubi == iconType)
            {
                if (iconNumber == 0)
                {
                    b = ImageSystemIconForm.YubiYoko();
                }
                else
                {
                    b = ImageSystemIconForm.YubiTate();
                }
            }
            else if (IconTypeEnum.Exit == iconType)
            {
                b = ImageSystemIconForm.ExitPoint();
            }
            else if (IconTypeEnum.Allow == iconType)
            {
                b = ImageSystemIconForm.Allows((int)iconNumber);
            }
            else if (IconTypeEnum.Music == iconType)
            {
                b = ImageSystemIconForm.MusicIcon(iconNumber);
            }
            else
            {
                b = ImageSystemIconForm.Blank16();
            }
            U.MakeTransparent(b);
            base.Image = b;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (base.Image == null)
            {
                DrawIcon();
            }
            pe.Graphics.InterpolationMode = interpolation;
            pe.Graphics.PixelOffsetMode = pixelOffset;

            base.OnPaint(pe);
        }
    }
}
