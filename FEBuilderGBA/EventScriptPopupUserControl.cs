using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EventScriptPopupUserControl : UserControl
    {
        public EventScriptPopupUserControl()
        {
            InitializeComponent();

            //タブを消す.
            this.Tab.Location = new Point(this.Tab.Location.X,this.Tab.Location.Y - 25);
            this.MAP.MapDoubleClickEvent += ClosePopup;
//            this.MAP.HideCommandBar();
        }
        void ClosePopup(object sender, EventArgs e)
        {
            X_Tooltip.HideEvent(sender, e);
            this.Hide();
        }
        public void LoadScreen()
        {
            if (Tab.SelectedIndex != 0)
            {//地図ではない場合、非表示にはなるため、 MapPicture.OnLoadが呼ばれないらしい...
                Tab.SelectedIndex = 0;
            }

            Bitmap map = ImageUtil.Blank(320, 240);
            map = ImageUtil.DrawGrid(map, OptionForm.Color_Keyword_ForeColor(), 16);

            MAP.ClearAllPoint();
            MAP.LoadMap(map);

            MAP.SetChipSize(1);
            Bitmap icon = ImageSystemIconForm.YubiTate();
            U.MakeTransparent(icon);
            MAP.SetDefaultIcon(icon, -8, -14);

            if (!MAP.IsMapLoad())
            {
                return;
            }
            int width = MAP.GetMapBitmapWidth();
            int height = MAP.GetMapBitmapHeight() + this.MAP.CommandBar.Height * 2;
            this.Tab.Width = width;
            this.Tab.Height = height;
            this.MAP.Width = width;
            this.MAP.Height = height;
            Tab.SelectedIndex = 0;
            this.Width = width;
            this.Height = height;
        }

        public void LoadMap(uint mapid)
        {
            if (Tab.SelectedIndex != 0)
            {//地図ではない場合、非表示にはなるため、 MapPicture.OnLoadが呼ばれないらしい...
                Tab.SelectedIndex = 0;
            }

            MAP.ClearAllPoint();
            MAP.LoadMap(mapid);

            if (MAP.IsWorldmap(mapid))
            {
                MAP.SetChipSize(1);
                Bitmap icon = ImageSystemIconForm.YubiTate();
                U.MakeTransparent(icon);
                MAP.SetDefaultIcon(icon, -8, -14);
            }
            else
            {
                MAP.SetChipSize(16);
                Bitmap icon = ImageSystemIconForm.YubiTate();
                U.MakeTransparent(icon);
                MAP.SetDefaultIcon(icon, 0, 0);
            }

            if (! MAP.IsMapLoad())
            {
                return;
            }
            int width = MAP.GetMapBitmapWidth();
            int height = MAP.GetMapBitmapHeight() + this.MAP.CommandBar.Height * 2;

            this.Tab.Width = width;
            this.Tab.Height = height;
            this.MAP.Width = width;
            this.MAP.Height = height;
            Tab.SelectedIndex = 0;
            this.Width = width;
            this.Height = height;
        }
        public void LoadImage(Bitmap bitmap)
        {
            MAP.ClearAllPoint();
            PIC.Image = bitmap;

            int width = bitmap.Width;
            int height = bitmap.Height;
            this.Tab.Width = width;
            this.Tab.Height = height;
            this.MAP.Width = width;
            this.MAP.Height = height;
            Tab.SelectedIndex = 1;
            this.Width = width;
            this.Height = height;
        }
        public void LoadMusic(uint sound_id , EventScript.ArgType argtype)
        {
            MAP.ClearAllPoint();
            PlaySappyButton.Tag = sound_id;
            this.MusicName.Text = SongTableForm.GetSongName(sound_id);

            string errormessage = "";
            if (argtype == EventScript.ArgType.SOUND)
            {
                errormessage = SongTableForm.GetErrorMessage(sound_id, "SFX");
            }
            else if (argtype == EventScript.ArgType.MAPMUSIC)
            {
                errormessage = SongTableForm.GetErrorMessage(sound_id, "MAP");
            }
            this.MusicName.ErrorMessage = errormessage;

            int width =  300;
            int height = 100;
            this.Tab.Width = width;
            this.Tab.Height = height;
            this.MAP.Width = width;
            this.MAP.Height = height;
            Tab.SelectedIndex = 2;
            this.Width = width;
            this.Height = height;
        }

        private void PlaySappyButton_Click(object sender, EventArgs e)
        {
            MainFormUtil.RunAsSappy((uint)PlaySappyButton.Tag);
        }

        public bool IsFocusedControl()
        {
            if (this.Visible == false)
            {//非表示なのにフォーカスがあるわけがない
                return false;
            }
            if (Tab.SelectedIndex != 0)
            {//マップではないなら無視.
                return false;
            }
            return this.MAP.IsFocusedControl();
        }

        ToolTipEx X_Tooltip;
        private void EventScriptPopupUserControl_Load(object sender, EventArgs e)
        {
            X_Tooltip = new ToolTipEx();
            this.MusicName.SetToolTipEx(X_Tooltip);
        }
    }
}
