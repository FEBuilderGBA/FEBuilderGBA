using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEBuilderGBA
{
    class ToolTranslateROMWipeJPChapterName
    {
        Undo.UndoData UndoData;

        public ToolTranslateROMWipeJPChapterName(Undo.UndoData undoData)
        {
            this.UndoData = undoData;
        }
        public void Wipe(List<Address> list)
        {
            if (Program.ROM.RomInfo.version != 8)
            {
                return ;
            }

            //Chapter Name to Text パッチをインストールする
            bool r = HowDoYouLikePatchForm.CheckAndShowPopupDialog(HowDoYouLikePatchForm.TYPE.ChapterNameText);
            if (!r)
            {
                return;
            }

            //最後の一つを残して全消去
            List<U.AddrResult> alist = ImageChapterTitleForm.MakeList();
            if (alist.Count <= 1)
            {
                return ;
            }
            uint addr;
            addr = alist[alist.Count - 1].addr;
            if (! U.isSafetyOffset(addr))
            {
                return ;
            }
            uint lastChapterNameImageAddr = Program.ROM.u32(addr + 0);
            for (int i = 0; i < alist.Count; i++)
            {
                addr = alist[i].addr;
                uint a = Program.ROM.u32(addr + 0);

                if (a != lastChapterNameImageAddr)
                {
                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , addr + 0
                        , "Chapter_Save"
                        , false
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                    Program.ROM.write_u32(addr + 0, lastChapterNameImageAddr, this.UndoData);
                }
                FEBuilderGBA.Address.AddLZ77Pointer(list
                    , addr + 4
                    , "Chapter_Number"
                    , false
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                FEBuilderGBA.Address.AddLZ77Pointer(list
                    , addr + 8
                    , "Chapter_Title"
                    , false
                    , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);

                Program.ROM.write_u32(addr + 4, 0, this.UndoData);
                Program.ROM.write_u32(addr + 8, 0, this.UndoData);
            }
        }
    }
}
