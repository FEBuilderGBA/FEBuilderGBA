using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class HowDoYouLikePatchForm : Form
    {
        public HowDoYouLikePatchForm()
        {
            InitializeComponent();
            FormIcon.Image = SystemIcons.Question.ToBitmap();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HowDoYouLikePatchForm_Load(object sender, EventArgs e)
        {
            //メッセージ（問い合わせ）を鳴らす
            System.Media.SystemSounds.Question.Play();
        }
        static bool AlwaysFalse()
        {
            return false;
        }

        public enum TYPE
        {
              Anti_Huffman_By_Translate
            , Anti_Huffman_By_English
            , C01Hack_By_ImageBattleAnimation
            , C48Hack_By_ImageBattleAnimation
            , NIMAP_By_SongTrack
            , Track12_Over_By_SongTrack
            , MeleeAndMagicFix_By_Unit
            , MagicPatch_By_Menu
            , Skill
            , SkipWorldmapFix
        };
        public static bool CheckAndShowPopupDialog(TYPE type)
        {
            Func<bool> checkFunc = null;
            string reason = "";
            string patchName1 = "";
            string patchName2 = "";
            string patchShowName = null;
            string patchCombo = "";
            if (type == TYPE.Anti_Huffman_By_Translate)
            {
                checkFunc = InputFormRef.SearchAntiHuffmanPatch;
                reason = R._("翻訳された文章がハフマン符号化テーブルを超えている可能性があります。\r\nAntiHuffmanPatchを適応しておくことをお勧めします。");
                patchName1 = "Anti-Huffman";///No Translate
            }
            if (type == TYPE.Anti_Huffman_By_English)
            {
                checkFunc = InputFormRef.SearchAntiHuffmanPatch;
                reason = R._("英語版で、テキストを変更するためには、Anti-Huffman Patchが必要です。");
                patchName1 = "Anti-Huffman";///No Translate
            }
            else if (type == TYPE.C01Hack_By_ImageBattleAnimation)
            {
                checkFunc = InputFormRef.SearchC01HackPatch;
                reason = R._("このアニメーションには、C01 命令が使われています。\r\n現在、C01ハックパッチは、まだインストールされていません。\r\nアニメーションを再生するためには、\r\nC01ハックを有効にした方がいいと思われますが、どうしますか？\r\n");
                patchName1 = "01command_hack";///No Translate
                patchName2 = "01コマンドハック";///No Translate
            }
            else if (type == TYPE.C48Hack_By_ImageBattleAnimation)
            {
                checkFunc = InputFormRef.SearchC48HackPatch;
                reason = R._("このアニメーションには、C48 命令(or Sxx命令)が使われています。\r\n現在、C48ハックパッチは、まだインストールされていません。\r\nアニメーションを再生するためには、\r\nC48ハックを有効にした方がいいと思われますが、どうしますか？\r\n");
                patchName1 = "48command_hack";///No Translate
                patchName2 = "48コマンドハック";///No Translate
            }
            else if (type == TYPE.NIMAP_By_SongTrack)
            {
                checkFunc = InputFormRef.SearchNIMAP;
                reason = R._("Midi楽器はFE楽器と並び順が違うので、\r\nNIMAPを利用することをお勧めします。");
                if (Program.ROM.RomInfo.version() == 8)
                {//FE8用には更新されたバージョンを利用する.
                    patchName1 = "SOUND_NIMAP2(Native Instrument Map)";///No Translate
                    patchName2 = "SOUND_NIMAP2";///No Translate
                }
                else
                {
                    patchName1 = "SOUND_NIMAP(Native Instrument Map)";///No Translate
                    patchName2 = "SOUND_NIMAP";///No Translate
                }
            }
            else if (type == TYPE.Track12_Over_By_SongTrack)
            {
                checkFunc = InputFormRef.Search16tracks12soundsPatch;
                reason = R._("この楽曲の楽譜は、12以上のトラックが存在しています。\r\nこの曲をゲームで再生するには、16_tracks_12_soundsが必要です。\r\nこのパッチを適応しますか？");
                patchName1 = "16_tracks_12_sounds";///No Translate
            }
            else if (type == TYPE.MeleeAndMagicFix_By_Unit)
            {
                checkFunc = InputFormRef.SearchMeleeAndMagicFixPatch;
                reason = R._("武器と魔法を同時に利用するにはMeleeAndMagicPatchが必要です。\r\n有効にしますか？");
                patchName1 = "MeleeAndMagicFix";///No Translate
            }
            else if (type == TYPE.MagicPatch_By_Menu)
            {
                return HowDoYouLikePatch2Form.CheckAndShowPopupDialog(HowDoYouLikePatch2Form.TYPE.MagicPatch_By_Menu);
            }
            else if (type == TYPE.Skill)
            {
                if (Program.ROM.RomInfo.version() != 8)
                {
                    return false;
                }
                if (Program.ROM.RomInfo.is_multibyte())
                {
                    checkFunc = () =>
                    {
                        InputFormRef.skill_system_enum a = InputFormRef.SearchSkillSystem();
                        return (a == InputFormRef.skill_system_enum.FE8N
                            || a == InputFormRef.skill_system_enum.FE8N_ver2
                            || a == InputFormRef.skill_system_enum.yugudora
                            );
                    };
                    reason = R._("スキル拡張 FE8N が必要です。\r\n有効にしますか？");
                    patchName1 = "Skill Extension FE8N 2018_06_09";///No Translate
                    patchName2 = "skill_2018_06_09";///No Translate
                }
                else
                {
                    checkFunc = () =>
                    {
                        return InputFormRef.SearchSkillSystem() == InputFormRef.skill_system_enum.SkillSystem;
                    };
                    reason = R._("スキル拡張 SkillSystems が必要です。\r\n有効にしますか？");
                    patchName1 = "Skill20181028";///No Translate
                    patchName2 = "Skill20181028";///No Translate
                }
            }
            else if (type == TYPE.SkipWorldmapFix)
            {
                if (Program.ROM.RomInfo.version() != 8)
                {
                    return false;
                }

                checkFunc = InputFormRef.SearchSkipWorldMapPatch;
                reason = R._("FE8のマップをワールドマップを経由しないで移動させるには、パッチが必要です。\r\n有効にしますか？");
                patchName1 = "Eliminate the constraint of freezing unless it enters from the world map";///No Translate
                patchName2 = "Eliminate the constraint of freezing unless it enters from the world map.";///No Translate
                patchShowName = "Skip Worldmap";
                patchCombo = "fix";
            }

            Debug.Assert(checkFunc != null);
            Debug.Assert(patchName1 != "");
            Debug.Assert(reason != "");
            if (checkFunc())
            {//すでに適応されている.
                return true;
            }
            if (patchShowName == null)
            {
                patchShowName = patchName1;
            }

            HowDoYouLikePatchForm f = (HowDoYouLikePatchForm)InputFormRef.JumpFormLow<HowDoYouLikePatchForm>();
            string tile = R._("{0}パッチを有効にしますか？", patchShowName);
            f.Text = tile;
            f.ReasonLabel.Text = R._("{0}\r\n\r\n{1}", tile, reason);
            f.EnableButton.Text = R._("{0}パッチを有効にする", patchShowName);
            f.EnableButton.Click += (sender, e) => {

                PatchForm patchF = (PatchForm)InputFormRef.JumpForm<PatchForm>();
                bool r = patchF.ApplyPatch(patchName1, patchName2, patchCombo); ///No Translate
                if (!r)
                {
                    R.ShowStopError("パッチの自動適応に失敗しました。\r\nパッチ画面にエラーが表示されていないか確認してください。\r\n必須パッチの適応に失敗したということは、FEBuilderGBAの解凍に失敗している可能性があります。\r\nFEBuilderGBAを再インストールしてください。\r\nそれでも問題が解決しない場合は、バグとして報告してください。\r\n");
                }
                f.Close();
            };
            f.ShowDialog();

            return checkFunc();
        }
    }
}
