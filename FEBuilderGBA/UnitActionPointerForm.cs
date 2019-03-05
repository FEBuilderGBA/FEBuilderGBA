using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    public partial class UnitActionPointerForm : Form
    {
        public UnitActionPointerForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            InputFormRef.ReInitPointer(SearchActionPointer());
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            if (InputFormRef.SearchUnitActionReworkPatch())
            {
                ApplyedUnitActionPatch.Show();
            }

        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            InputFormRef ifr = null;
            ifr = new InputFormRef(self
                , ""
                , 0
                , 4
                , (int i, uint addr) =>
                {
                    uint a = Program.ROM.u32(addr);
                    if (a == 0x0)
                    {
                        return true;
                    }
                    if (!U.isSafetyPointer(a))
                    {
                        if ((a & 0x10000000) > 0)
                        {
                            if (U.isSafetyPointer(a & 0xDFFFFFFF ))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    int id = i;
                    return U.ToHexString(id) + " " + U.ToHexString(id);
                }
                );
            return ifr;
        }

        private void EventFunctionPointerForm_Load(object sender, EventArgs e)
        {
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "UnitActionFunctionPointer";
            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInitPointer(SearchActionPointer());
            FEBuilderGBA.Address.AddAddress(list
                , InputFormRef
                , name
                , new uint[] { 0 }
                , FEBuilderGBA.Address.DataTypeEnum.InputFormRef_ASM);

            List<U.AddrResult> arlist = InputFormRef.MakeList();
            FEBuilderGBA.Address.AddFunctions(list, arlist, 0, name);
        }

        public static uint SearchActionPointer()
        {
            if (! InputFormRef.SearchUnitActionReworkPatch())
            {//rework されていない
                return Program.ROM.RomInfo.unitaction_function_pointer();
            }

            string filename = Path.Combine(Program.BaseDirectory, "config", "patch2", Program.ROM.RomInfo.VersionToFilename(), "UnitActionRework", "UnitActionRework", "asm", "ApplyAction.bin");
            if (!File.Exists(filename))
            {
                return 0;
            }

            uint hintAddr = Program.ROM.RomInfo.unitaction_function_pointer() - 0x100;

            byte[] bin = File.ReadAllBytes(filename);
            uint addrApplyAction = U.GrepEnd(Program.ROM.Data, bin, hintAddr, 0, 4);
            if (addrApplyAction == U.NOT_FOUND)
            {
                return 0;
            }

            return addrApplyAction;
        }

        public static void SupportActionRework(StringBuilder sb)
        {
            if (! InputFormRef.SearchUnitActionReworkPatch())
            {
                return;
            }

            uint addrApplyAction = SearchActionPointer();

            if (addrApplyAction == 0 || addrApplyAction == U.NOT_FOUND)
            {
                return;
            }

            uint tablePointer = Program.ROM.u32(addrApplyAction);
            if (!U.isSafetyPointer(tablePointer))
            {
                return;
            }

            uint tableAddr = U.toOffset(tablePointer);
            sb.AppendLine("#define HAX_ACTION_APPLICATION_REWORK_EVENT");
            sb.AppendLine("#define pActionRoutineTable " + U.To0xHexString(tableAddr)); ///No Translate
            sb.AppendLine("#define NoActionRoutine \"WORD 0\"");    ///No Translate
            sb.AppendLine("#define ActionRoutine(apRoutine) \"POIN apRoutine\"");   ///No Translate
            sb.AppendLine("#define ActionRoutine(apRoutine, abForcedYeild) \"WORD (0x08000000 | apRoutine | (abForcedYeild << 28))\""); ///No Translate
            sb.AppendLine("#define SetUnitAction(aActionId, adActionRoutine) \"PUSH; ORG (pActionRoutineTable + 4*(aActionId-1)); adActionRoutine; POP\""); ///No Translate
        }
    }
}
