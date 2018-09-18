;3人組みを表示する関数を拡張して、ED挿絵を表示できるようにする 本体
;
;アドレスはすべてFE8Jのものです.
;
;ED3人組を表示するコード
;221A0000
;42210000 00000200
;221A0000
;↓↓Nで切り替えられるようにする↓↓
;221A0000
;42210[N]00 00000200
;221A0000
;
;画像表示後、マップがおかしくなる時がある。
;その時は、適当に背景を描画して背景消去してください。背景付き会話でも可。
;画像表示時間を延ばしたいときは、 後ろの221A0000の前にウェイト命令を。
;
;N=
;[0]->0x08ac0524 ED3人組
;[1]->0x08AC0530 王女を抱えるエイリーク
;[2]->0x08ac053c 魔石研究
;[3]->0x08AC0548 ドラゴンナイト
;[4]->0x08AC0554 ゼトとエイリーク
;[5]->0x08AC0560 魔王と聖石
;[6]->0x08AC056C ゼト訓練
;[7]->0x08AC0578 ペガサスナイト
;[8]->0x08AC0584 ラーチェル乗馬
;[9]->0x08AC0590 踊り子と町の人
;
@thumb
;@orgは使ってはいけません。 
;@org	$08E4FD50 ;FREEAREA

;
;when fe8u , the function called 0xB65FE.
;

mov r4,r5           ;r5のメモリ位置がどうしても必要

mov r5,r8           ;元の処理をする
push	{r5, r6, r7}
mov	r9, r0
mov	r7, r1
mov	r8, r2
mov	r10, r3         ;ここまで元処理

mov r0,r4           ;元r5のメモリ位置 + 0x38 = 現在のイベント命令 スクリプトコードへのポインタ
add r0,#0x38

ldr r0,[r0]         ; 現在のイベント命令 スクリプトコードへ
ldrb r6,[r0,#0x2]   ; イベント命令 スクリプトコードへ 番号をとる.

mov r0,#0x0C
mul r6,r0           ; 1つ 12バイト区切り 番号*12バイト
;ldr r0,=$0x08ac0524
ldr  r0,=$0x08A3CCEC ;for FE8U
add r6,r6,r0        ;$0x08ac0524 + (12バイト*個数)
                    ;
                    ;struct split10image_list{
                    ; split10image* table;
                    ; tsa*         tsa;
                    ; pallet*      pallet;
                    ;}; sizeof() == 4*3 = 12バイト
                    ;
                    ;struct split10image{
                    ; image*    row1; //圧縮画像10個
                    ; image*    row2;
                    ; image*    row3;
                    ; image*    row4;
                    ; image*    row5;
                    ; image*    row6;
                    ; image*    row7;
                    ; image*    row8;
                    ; image*    row9;
                    ; image*    row10;
                    ;};
                    ;

;元に戻す.
;;ldr	r0, =$080bb196  ;080bb196 2500     	mov	r5, #0        //r5=0
ldr	r0, =$080B6612  ;;for FE8U
mov	pc, r0
