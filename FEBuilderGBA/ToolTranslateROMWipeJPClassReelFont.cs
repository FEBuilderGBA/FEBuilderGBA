using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEBuilderGBA
{
    class ToolTranslateROMWipeJPClassReelFont
    {
        Undo.UndoData UndoData;

        public ToolTranslateROMWipeJPClassReelFont(Undo.UndoData undoData)
        {
            this.UndoData = undoData;
        }
        public void Wipe(List<Address> list)
        {
            if (Program.ROM.RomInfo.version != 8)
            {
                return ;
            }
            if (!Program.ROM.RomInfo.is_multibyte)
            {
                return ;
            }

            if (Program.ROM.u16(0xB7890) != 0x4B00 )
            {
                return;
            }

            //最初の一つを残して全消去
            List<U.AddrResult> alist = OPClassFontForm.MakeList();
            if (alist.Count <= 1)
            {
                return ;
            }

            uint addr;
            addr = alist[0].addr;
            if (! U.isSafetyOffset(addr))
            {
                return ;
            }
            uint lastJpFontImageAddr = Program.ROM.u32(addr + 0);
            for (int i = 0; i < alist.Count; i++)
            {
                addr = alist[i].addr;
                uint a = Program.ROM.u32(addr + 0);

                if (a != lastJpFontImageAddr)
                {
                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , addr + 0
                        , "OPClassFont " + U.To0xHexString(i)
                        , false
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                    Program.ROM.write_u32(addr + 0, lastJpFontImageAddr, this.UndoData);
                }
            }
        }
    }
}
