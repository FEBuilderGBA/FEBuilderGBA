;
;輸送隊とキャラクタの持ち物をスキャンして、アイテムを所持しているか調べます
;
; ITEM ID リトルエンディアン u16
;40 0D [ITEM] [ID] E1 F6 E4 08
;

;@org	$08E4FB90
@thumb
	push	{r4,r5,r6,r7, lr}

	ldr  r0, [r0, #0x38]      ;イベント命令にアクセスらしい [r3,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrh r7, [r0, #0x2]       ;引数1 40 0D [引数1] [引数2] [プログラム場所 XX XX XX 08]

	;輸送隊アドレスの取得
	ldr		r0, =$08031524
	ldr		r2, [r0]       ;輸送隊アドレス
	ldrb	r3, [r0,#0x4]  ;最大個数

	;ループ上限 = 開始アドレス+(個数*2バイト)
	lsl		r3,r3,#0x1	;r3 * 2
	add		r6,r3,r2	;終端

	;輸送隊アドレス
	mov		r5,r2

;#IFDEF SELL
;	;輸送隊書き込みアドレス
;	;アイテムを没収する場合、余白ができるので詰めていく
;	mov		r4,r2
;#ENDIF SELL

yusotai_loop
	ldrb	r0,[r5,#0x0] ;アイテムID
	ldrb	r1,[r5,#0x1] ;個数
	add		r5,#0x2		;アイテムID 個数の2バイト配列のため

	cmp		r0,r7
	beq		found

	cmp		r0,#0x00     ;アイテム終端
	beq		yusotai_item_zeropadding

;yusotai_next
	cmp		r5,r6
	blt		yusotai_loop		;上限アドレスを超えていれなければ続く

yusotai_item_zeropadding
;#IFDEF Confiscation
;;	;輸送隊は、ゼロ終端ではない。
;	;特効薬を売って余白ができていた場合、
;	;輸送隊欄に余白ができないように0で埋めないとダメ。
;	cmp		r6,r4
;	ble		yusotai_end
;
;	mov		r0,0x00
;	strb	r0,[r4,#0x0]
;	strb	r0,[r4,#0x1]
;	add		r4,r4,#0x2
;	b		yusotai_item_zeropadding
;#ENDIF Confiscation

yusotai_end


;個別キャラの持ち物スタート
	ldr		r5, =$0202BE4C

character_loop
	ldrh	r0,[r5]

	mov		r1,#0x00
	cmp		r0,r1
	beq		character_end

	add		r5,#0x1E	;アイテム1 アイテムIDへ
;#IFDEF Confiscation
;	mov		r4,r5		;書込
;#ENDIF Confiscation
	
	mov		r6,r5		;個数5つまでループする
	add		r6,#0xA		;5個*2バイト = 0xA
item_loop
	ldrb	r0,[r5,#0x00]	;アイテムID
	ldrb	r1,[r5,#0x01]	;アイテム個数
	add		r5,#0x2
	
	cmp		r0,r7
	beq		found

	cmp		r0,#0x00     ;アイテム終端
	beq		character_item_zeropadding

;#IFDEF Confiscation
;	;特定のアイテム以外はコピーする
;	strb	r0,[r4,#0x0]
;	strb	r1,[r4,#0x1]
;	add		r4,#0x2			;アイテムID 個数の2バイト配列のため
;#ENDIF Confiscation

character_item_next
	cmp		r5,r6
	blt		item_loop		;アイテム5を処理するまでループ

character_item_zeropadding
;#IFDEF Confiscation
;	;アイテム欄も、ゼロ終端ではない。
;	;特効薬を売って余白ができていた場合、
;	;5つのアイテム欄に余白ができないように0で埋めないとダメ。
;	cmp		r6,r4
;	ble		character_next
;
;	mov		r0,0x00
;	strb	r0,[r4,#0x0]
;	strb	r0,[r4,#0x1]
;	add		r4,r4,#0x2
;	b		character_item_zeropadding
;#ENDIF Confiscation

character_next

	mov		r5, #0x20	;次の人へ #0x48 - (#0xA + #0x1E) = 0x20
	add		r5,r5, r6
	b		character_loop

found
	mov	r0, #0x1		;アイテムを所持している
	b		make_result

character_end
	mov	r0, #0x0		;アイテムを所持していない

make_result
	;イベント命令の条件式でぜひが取れるようにする
	ldr	r2, =$030004B8  ; + #0x30 FE8U
	str	r0, [r2, #0x30]	;条件式で取れる領域に書き込む
						;400CXX00 0C000000	未達成ならジャンプ  / ;410CXX00 0C000000	達成ならジャンプ

exit_return
;	mov	r0, #0

	pop	{pc,r7, r6 , r5 ,r4 }
