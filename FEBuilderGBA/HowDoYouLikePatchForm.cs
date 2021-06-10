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
            if (this.NoRecommedPatchCheckBox.Checked)
            {
                IgnorePatchs.Add(this.CurrentType);
            }
            this.Close();
        }
        static List<TYPE> IgnorePatchs = new List<TYPE>();
        TYPE CurrentType;

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
            , ItemIconExpands
            , CAMERA_Event_OutOfBand_Fix
            , CAMERA_Event_NotExistsUnit_Fix
            , UnitGetStateEvent_0x33_Fix
            , UnitUpdateStateEvent_0x34_Fix
            , SetActiveEvent_0x38_Fix
            , WakuEvent_0x3B_Fix
            , StatusToLocalization
            , ExtendedMovingMapAnimationList
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
                checkFunc = PatchUtil.SearchAntiHuffmanPatch;
                reason = R._("翻訳された文章がハフマン符号化テーブルを超えている可能性があります。\r\nAntiHuffmanPatchを適応しておくことをお勧めします。");
                patchName1 = "Anti-Huffman";///No Translate
            }
            if (type == TYPE.Anti_Huffman_By_English)
            {
                checkFunc = PatchUtil.SearchAntiHuffmanPatch;
                reason = R._("英語版で、テキストを変更するためには、Anti-Huffman Patchが必要です。");
                patchName1 = "Anti-Huffman";///No Translate
            }
            else if (type == TYPE.C01Hack_By_ImageBattleAnimation)
            {
                checkFunc = PatchUtil.SearchC01HackPatch;
                reason = R._("このアニメーションには、C01 命令が使われています。\r\n現在、C01ハックパッチは、まだインストールされていません。\r\nアニメーションを再生するためには、\r\nC01ハックを有効にした方がいいと思われますが、どうしますか？\r\n");
                patchName1 = "01command_hack";///No Translate
                patchName2 = "01コマンドハック";///No Translate
            }
            else if (type == TYPE.C48Hack_By_ImageBattleAnimation)
            {
                checkFunc = PatchUtil.SearchC48HackPatch;
                reason = R._("このアニメーションには、C48 命令(or Sxx命令)が使われています。\r\n現在、C48ハックパッチは、まだインストールされていません。\r\nアニメーションを再生するためには、\r\nC48ハックを有効にした方がいいと思われますが、どうしますか？\r\n");
                patchName1 = "48command_hack";///No Translate
                patchName2 = "48コマンドハック";///No Translate
            }
            else if (type == TYPE.NIMAP_By_SongTrack)
            {
                checkFunc = PatchUtil.SearchNIMAP;
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
                checkFunc = PatchUtil.Search16tracks12soundsPatch;
                reason = R._("この楽曲の楽譜は、12以上のトラックが存在しています。\r\nこの曲をゲームで再生するには、16_tracks_12_soundsが必要です。\r\nこのパッチを適応しますか？");
                patchName1 = "16_tracks_12_sounds";///No Translate
            }
            else if (type == TYPE.MeleeAndMagicFix_By_Unit)
            {
                checkFunc = PatchUtil.SearchMeleeAndMagicFixPatch;
                reason = R._("武器と魔法を同時に利用するにはMeleeAndMagicPatchが必要です。\r\n有効にしますか？");
                patchName1 = "MeleeAndMagicFix";///No Translate
            }
            else if (type == TYPE.ItemIconExpands)
            {
                checkFunc = PatchUtil.SearchIconExpandsPatch;
                reason = R._("アイコンを拡張するには、テーブルを拡張する前にパッチを適応する必要があります。\r\n有効にしますか？");
                patchName1 = "Extended to item icon 0xFE";///No Translate
                patchName2 = "Extend Item Icon List Length";///No Translate
                patchCombo = "fix";///No Translate
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
                        PatchUtil.skill_system_enum a = PatchUtil.SearchSkillSystem();
                        return (a == PatchUtil.skill_system_enum.FE8N
                            || a == PatchUtil.skill_system_enum.FE8N_ver2
                            || a == PatchUtil.skill_system_enum.FE8N_ver3
                            || a == PatchUtil.skill_system_enum.yugudora
                            );
                    };
                    reason = R._("スキル拡張 FE8N が必要です。\r\n有効にしますか？");
                    patchName1 = "Skill Extension FE8N 2019_02_19";///No Translate
                    patchName2 = "skill_2019_02_19";///No Translate
                }
                else
                {
                    checkFunc = () =>
                    {
                        return PatchUtil.SearchSkillSystem() == PatchUtil.skill_system_enum.SkillSystem;
                    };
                    reason = R._("スキル拡張 SkillSystems が必要です。\r\n有効にしますか？");
                    patchName1 = "Skill20201128";///No Translate
                    patchName2 = "Skill20201128";///No Translate
                }
            }
            else if (type == TYPE.CAMERA_Event_OutOfBand_Fix)
            {
                if (Program.ROM.RomInfo.version() != 8)
                {
                    return false;
                }

                checkFunc = () =>
                {
                    return PatchUtil.SearchCAMERA_Event_OutOfBand_FixPatch() ;
                };
                reason = R._("カメラを移動する命令で、画面外に飛び出してしまうバグを修正するパッチをインストールしますか？");
                patchName1 = "Fix CAM1_CAMERA2 going out of bounds";///No Translate
                patchName2 = "Fix CAM1_CAMERA2 going out of bounds";///No Translate
                patchShowName = "Fix CAM1_CAMERA2 going out of bounds";///No Translate
                patchCombo = "Fix";///No Translate
            }
            else if (type == TYPE.CAMERA_Event_NotExistsUnit_Fix)
            {
                if (Program.ROM.RomInfo.version() != 8)
                {
                    return false;
                }

                checkFunc = () =>
                {
                    return PatchUtil.SearchCAMERA_Event_NotExistsUnit_FixPatch();
                };
                reason = R._("存在しないユニットを指定した時にフリーズするバグを修正するパッチをインストールしますか？");
                patchName1 = "Event26_CameraControlMaybe_Fix not to freeze even nonexistent units";///No Translate
                patchName2 = "Event26_CameraControlMaybe_Fix not to freeze even nonexistent units";///No Translate
                patchShowName = "Event26_CameraControlMaybe_Fix not to freeze even nonexistent units";///No Translate
                patchCombo = "Fix";///No Translate
            }
            else if (type == TYPE.UnitGetStateEvent_0x33_Fix)
            {
                if (Program.ROM.RomInfo.version() != 8)
                {
                    return false;
                }

                checkFunc = () =>
                {
                    return PatchUtil.SearchGetUnitStateEvent_0x33_FixPatch();
                };
                reason = R._("存在しないユニットを指定した時にフリーズするバグを修正するパッチをインストールしますか？");
                patchName1 = "Event33_CheckUnitVarious_Fix not to freeze even nonexistent units";///No Translate
                patchName2 = "Event33_CheckUnitVarious_Fix not to freeze even nonexistent units";///No Translate
                patchShowName = "Event33_CheckUnitVarious_Fix not to freeze even nonexistent units";///No Translate
                patchCombo = "Fix";///No Translate
            }
            else if (type == TYPE.UnitUpdateStateEvent_0x34_Fix)
            {
                if (Program.ROM.RomInfo.version() != 8)
                {
                    return false;
                }

                checkFunc = () =>
                {
                    return PatchUtil.SearchUpdateUnitStateEvent_0x34_FixPatch();
                };
                reason = R._("存在しないユニットを指定した時にフリーズするバグを修正するパッチをインストールしますか？");
                patchName1 = "Event34_MessWithUnitState_Fix not to freeze even nonexistent units";///No Translate
                patchName2 = "Event34_MessWithUnitState_Fix not to freeze even nonexistent units";///No Translate
                patchShowName = "Event34_MessWithUnitState_Fix not to freeze even nonexistent units";///No Translate
                patchCombo = "Fix";///No Translate
            }
            else if (type == TYPE.SetActiveEvent_0x38_Fix)
            {
                if (Program.ROM.RomInfo.version() != 8)
                {
                    return false;
                }

                checkFunc = () =>
                {
                    return PatchUtil.SearchActiveUnitEvent_0x38_FixPatch();
                };
                reason = R._("存在しないユニットを指定した時にフリーズするバグを修正するパッチをインストールしますか？");
                patchName1 = "Event38_activeunit_Fix not to freeze even nonexistent units";///No Translate
                patchName2 = "Event38_activeunit_Fix not to freeze even nonexistent units";///No Translate
                patchShowName = "Event38_activeunit_Fix not to freeze even nonexistent units";///No Translate
                patchCombo = "Fix";///No Translate
            }
            else if (type == TYPE.WakuEvent_0x3B_Fix)
            {
                if (Program.ROM.RomInfo.version() != 8)
                {
                    return false;
                }

                checkFunc = () =>
                {
                    return PatchUtil.SearchWakuEvent_0x3B_FixPatch();
                };
                reason = R._("存在しないユニットを指定した時にフリーズするバグを修正するパッチをインストールしますか？");
                patchName1 = "Event3B_frame_Fix not to freeze even nonexistent units";///No Translate
                patchName2 = "Event3B_frame_Fix not to freeze even nonexistent units";///No Translate
                patchShowName = "Event3B_frame_Fix not to freeze even nonexistent units";///No Translate
                patchCombo = "Fix";///No Translate
            }
            else if (type == TYPE.StatusToLocalization)
            {
                if (Program.ROM.RomInfo.version() != 8)
                {
                    return false;
                }
                if (Program.ROM.RomInfo.is_multibyte() == false)
                {
                    return false;
                }

                checkFunc = () =>
                {
                    return PatchUtil.SearchStatusToLocalization_FixPatch();
                };
                reason = R._("ローカリゼーションパッチをインストールしますか？");
                patchName1 = "StatusToLocalization";///No Translate
                patchName2 = "StatusToLocalization";///No Translate
                patchShowName = "StatusToLocalization";///No Translate
                patchCombo = "Fix";///No Translate
            }
            else if (type == TYPE.ExtendedMovingMapAnimationList)
            {
                if (Program.ROM.RomInfo.version() == 6)
                {//FE6のパッチはないです
                    return false;
                }

                checkFunc = () =>
                {
                    return PatchUtil.SearchExtendedMovingMapAnimationListPatch();
                };
                reason = R._("ユニット移動アイコンを拡張するには、テーブルを拡張するパッチを適応する必要があります。\r\n有効にしますか？");
                patchName1 = "Extended Moving Map Animation List";///No Translate
                patchName2 = "Extended Moving Map Animation List";///No Translate
                patchShowName = "Extended to Moving Map Animation 0xFF";///No Translate
                patchCombo = "Extend 0xFF";///No Translate
            }

            Debug.Assert(checkFunc != null);
            Debug.Assert(patchName1 != "");
            Debug.Assert(reason != "");
            if (checkFunc())
            {//すでに適応されている.
                return true;
            }
            if (IgnorePatchs.IndexOf(type) >= 0)
            {
                return true;
            }
            if (patchShowName == null)
            {
                patchShowName = patchName1;
            }

            HowDoYouLikePatchForm f = (HowDoYouLikePatchForm)InputFormRef.JumpFormLow<HowDoYouLikePatchForm>();
            string tile = R._("{0}パッチを有効にしますか？", patchShowName);
            f.CurrentType = type;
            f.Text = tile;
            f.ReasonLabel.Text = R._("{0}\r\n\r\n{1}", tile, reason);
            f.EnableButton.Text = R._("{0}パッチを有効にする", patchShowName);
            f.EnableButton.Click += (sender, e) => {

                f.Close();
                PatchForm patchF = (PatchForm)InputFormRef.JumpForm<PatchForm>();
                bool r = patchF.ApplyPatch(patchName1, patchName2, patchCombo); ///No Translate
                if (!r)
                {
                    R.ShowStopError("パッチの自動適応に失敗しました。\r\nパッチ画面にエラーが表示されていないか確認してください。\r\n必須パッチの適応に失敗したということは、FEBuilderGBAの解凍に失敗している可能性があります。\r\nFEBuilderGBAを再インストールしてください。\r\nそれでも問題が解決しない場合は、バグとして報告してください。\r\n");
                }
                f.Close();
            };

            if (Program.IsCommandLine)
            {//コマンドラインモードだったら自動適応するしかないね。
                U.FireOnClick(f.EnableButton);
                return checkFunc();
            }

            f.ShowDialog();

            return checkFunc();
        }

    }
}
