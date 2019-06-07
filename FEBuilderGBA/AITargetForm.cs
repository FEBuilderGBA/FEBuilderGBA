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
    public partial class AITargetForm : Form
    {
        public AITargetForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            InputFormRef.markupJumpLabel(this.ExplainLink);
            MakeExplainFunctions();
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.ai3_pointer()
                , 20
                , (int i, uint addr) =>
                {
                    return i < 8;
                }
                , (int i, uint addr) =>
                {
                    string name = EventUnitForm.GetAIName3((uint)i);
                    if (name != "")
                    {
                        return name;
                    }
                    switch (i)
                    {
                        case 0:
                        default:
                            return R._("標的AI00");
                        case 1:
                            return R._("標的AI08");
                        case 2:
                            return R._("標的AI10");
                        case 3:
                            return R._("標的AI18");
                        case 4:
                            return R._("標的AI20");
                        case 5:
                            return R._("標的AI28");
                        case 6:
                            return R._("標的AI30");
                        case 7:
                            return R._("標的AI38");
                    }
                }
                );
        }

        private void AITargetForm_Load(object sender, EventArgs e)
        {

        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "AI3", new uint[] { });
        }

        private void ExplainLink_Click(object sender, EventArgs e)
        {
            U.OpenURLOrFile( MainFormUtil.GetAboutTragetAI3() );
        }
        void MakeExplainFunctions()
        {
            J_0.AccessibleDescription = R._("もし致死ダメージを与えられる敵なら、この項目のTPは50固定です（優先度無視）\r\n\r\n致死ダメージを与えられない場合のTPは「命中率xダメージ/100」の値を計算します。なお、この計算に追撃は考慮されません。\r\n加算されるTP上限は40です。（上限は優先度を乗算した値にかかります）");
            J_1.AccessibleDescription = R._("「20-敵の残りHP」で計算されます。全部の攻撃が当たると想定して計算します。この計算が0以下になれば0です。\r\n例を挙げると、HP30の敵に25ダメージを与えられるとすると、敵の残りHPは5なので、20-5=15で「15」が計算結果です。");
            J_2.AccessibleDescription = R._("動く前の位置関係もTPに加算されます。\r\n 加算されるTP上限は10です。\r\n")
                + "\r\n" + "      1       \r\n      2 1     \r\n  1 2 3 2 1   \r\n1 2 3   3 2 1 \r\n  1 2 3 2 1  \r\n     1 2 1\r\n        1 \r\n";///No Translate
            J_3.AccessibleDescription = R._("優先して狙うクラスの設定ができます。項目00～09に設定してある値と優先度を乗算したものがTPです。\r\n優先クラス設定のポインタ以降にあり、敵がどのクラスでもない場合は、09（ゼロ）を乗算します。\r\n\r\n指定されたクラスはこの値と「01の値」を乗算したものがTPです。\r\nただし、烈火・聖魔ではポインタのクラス指定がすべてゼロなので、意味がありません。封印だけに設定されています。\r\n\r\n加算されるTP上限は20です。");
            J_4.AccessibleDescription = R._("現在のターン数がTPに加算されます");
            J_5.AccessibleDescription = R._("反撃されない場合は、10の優先ポイントとなります。（警戒度無視）\r\n反撃を受ける場合のTPは「命中率xダメージ/100」の値を計算します。なお、この計算に追撃は考慮されません。\r\n\r\n減算されるTP下限は40です。");
            J_6.AccessibleDescription = R._("標的AIには攻撃するときの位置取りにも影響します。\r\n攻撃すると敵の攻撃範囲に入る場合、\r\n敵ごとに「力+一番強い武器の威力/2」の値を計算し、それを合算した値を8で除算した結果をTPから減算します。\r\n\r\n減算されるTP下限は20です。");
            J_7.AccessibleDescription = R._("「20-自分の残りHP」で計算されます。全部の攻撃が当たると想定して計算します。\r\nこの計算が0以下になれば0です。");
            ExplainLink.AccessibleDescription = R._("AIは、ターゲットポイント（TP）を計算し、それがもっとも高くなった攻撃対象を攻撃します。\r\nTPの計算は、手持ちの装備を上から順に計算します。\r\nそして、装備ごとに範囲内の敵を出撃表の上から順に計算します。\r\nTPの量が被った場合は、武器、出撃表、共に下にある物を優先します。\r\n\r\nそれぞれの値を計算し、合計したものを40倍したものが合計TPです。\r\n\r\n出典 FEUniverse Crazycolorz5 / 翻訳 ngmansion aera\r\n詳細はリンクをクリックしてください。");

        }
    }
}
