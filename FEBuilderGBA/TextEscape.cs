using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEBuilderGBA
{
    public class TextEscape
    {
        class AddEscape
        {
            //public string fe7edit{ get; private set; }
            public string feditorAdv { get; private set; }
            public string info { get; private set; }

            public AddEscape(string _feditorAdv, string _info)
            {
                this.feditorAdv = _feditorAdv;
                this.info = _info;
            }
        }
        Dictionary<string, AddEscape> AddEscapeMapping = new Dictionary<string, AddEscape>();

        public void Add(string fe7edit, string feditorAdv, string info)
        {
            if (AddEscapeMapping.ContainsKey(fe7edit))
            {
                return;
            }
            AddEscapeMapping[fe7edit] = new AddEscape(feditorAdv,info);
            ConvertEscapeToFEditorTable.Add(fe7edit);
            ConvertEscapeToFEditorTable.Add(feditorAdv);
        }

        static List<string> ConvertEscapeToFEditorTable = new List<string>(){
                 "@0080@0004","[LoadOverworldFaces]"    ///No Translate
                ,"@0080@0005","[G]"    ///No Translate
                ,"@0080@000A","[MoveFarLeft]"    ///No Translate
                ,"@0080@000B","[MoveMidLeft]"    ///No Translate
                ,"@0080@000C","[MoveLeft]"    ///No Translate
                ,"@0080@000D","[MoveRight]"    ///No Translate
                ,"@0080@000E","[MoveMidRight]"    ///No Translate
                ,"@0080@000F","[MoveFarRight]"    ///No Translate
                ,"@0080@0010","[MoveFarFarLeft]"    ///No Translate
                ,"@0080@0011","[MoveFarFarRight]"    ///No Translate
                ,"@0080@0016","[EnableBlinking]"    ///No Translate
                ,"@0080@0018","[DelayBlinking]"    ///No Translate
                ,"@0080@0019","[PauseBlinking]"    ///No Translate
                ,"@0080@001B","[DisableBlinking]"    ///No Translate
                ,"@0080@001C","[OpenEyes]"    ///No Translate
                ,"@0080@001D","[CloseEyes]"    ///No Translate
                ,"@0080@001E","[HalfCloseEyes]"    ///No Translate
                ,"@0080@001F","[Wink]"    ///No Translate
                ,"@0080@0020","[Tact]"    ///No Translate
                ,"@0080@0021","[ToggleRed]"    ///No Translate
                ,"@0080@0022","[Item]"    ///No Translate
                ,"@0080@0023","[SetName]"    ///No Translate
                ,"@0080@0024","[Tact2]"    ///No Translate
                ,"@0080@0025","[ToggleColorInvert]"    ///No Translate
                ,"@0001","[NL]" ///No Translate
		        ,"@0002","[Clear]"    ///No Translate
		        ,"@0002","[2NL]"    ///No Translate
		        ,"@0003","[A]"    ///No Translate
		        ,"@0004","[....]"    ///No Translate
		        ,"@0005","[.....]"    ///No Translate
		        ,"@0006","[......]"    ///No Translate
		        ,"@0007","[.......]"    ///No Translate
		        ,"@0008","[OpenFarLeft]"    ///No Translate
		        ,"@0008","[FarLeft]"    ///No Translate
		        ,"@0009","[OpenMidLeft]"    ///No Translate
		        ,"@0009","[MidLeft]"    ///No Translate
		        ,"@000A","[OpenLeft]"    ///No Translate
		        ,"@000A","[Left]"    ///No Translate
		        ,"@000B","[OpenRight]"    ///No Translate
		        ,"@000B","[Right]"    ///No Translate
		        ,"@000C","[OpenMidRight]"    ///No Translate
		        ,"@000C","[MidRight]"    ///No Translate
		        ,"@000D","[OpenFarRight]"    ///No Translate
		        ,"@000D","[FarRight]"    ///No Translate
		        ,"@000E","[OpenFarFarLeft]"    ///No Translate
		        ,"@000E","[FarFarLeft]"    ///No Translate
		        ,"@000F","[OpenFarFarRight]"    ///No Translate
		        ,"@000F","[FarFarRight]"    ///No Translate
		        ,"@0011","[ClearFace]"    ///No Translate
		        ,"@0012","[NormalPrint]"    ///No Translate
		        ,"@0013","[FastPrint]"    ///No Translate
		        ,"@0014","[CloseSpeechFast]"    ///No Translate
		        ,"@0015","[CloseSpeechSlow]"    ///No Translate
		        ,"@0016","[ToggleMouthMove]"    ///No Translate
		        ,"@0017","[ToggleSmile]"    ///No Translate
		        ,"@0018","[Yes]"    ///No Translate
		        ,"@0019","[No]"    ///No Translate
		        ,"@001A","[Buy/Sell]"    ///No Translate
		        ,"@001B","[ShopContinue]"    ///No Translate
                ,"@001C","[SendToBack]"    ///No Translate
                ,"@001D","[FastPrint2]"    ///No Translate
                ,"@001F","[.]"    ///No Translate 英語版のみ
                ,"@0010","[LoadFace]"    ///No Translate  別処理をするがハイライトの都合でリストに追加します.
                ,"@0040","[@]"    ///No Translate  @を出す
                ,"@0093","[OpenQuote]"    ///No Translate 英語版のみ
                ,"@0094","[CloseQuote]"    ///No Translate 英語版のみ
        };
        public bool Find(string parts)
        {
            for (int n = 0; n < ConvertEscapeToFEditorTable.Count; n += 2)
            {
                if (parts == ConvertEscapeToFEditorTable[n + 1])
                {
                    return true;
                }
            }
            return false;
        }
        public string table_replace(string str)
        {
            return U.table_replace(str, ConvertEscapeToFEditorTable);
        }
        public string table_replace_rev(string str)
        {
            return U.table_replace_rev(str, ConvertEscapeToFEditorTable);
        }
        public void MargeExstraEscapeList(List<TextScriptFormCategorySelectForm.TextEscape> EscapeList)
        {
            foreach (var t in AddEscapeMapping)
            {
                TextScriptFormCategorySelectForm.TextEscape te = new TextScriptFormCategorySelectForm.TextEscape();
                te.Code = t.Key;
                te.Info = t.Value.info + t.Value.feditorAdv;
                te.Category = "";
                EscapeList.Add(te);
            }
        }
        
    }
}
