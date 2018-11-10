.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@Call 080B5B3E (FE8U)
@
@r4 変更禁止
@r5 変更禁止
@


.thumb

	push {r4}           @r4を利用したいので、pushして退避

	@難易度取得
	
	@ハード判定
	ldr r1,=0x0202BCF0	@FE8U  ChapterData
	ldrb r0, [r1, #0x14]
	mov r2, #0x40
	and r0 ,r2
	cmp r0 ,#0x00
	bne Is_Hard_Mode

	@ノーマル判定
	mov r2, #0x42
	ldrb r0, [r1, r2]
	mov r2, #0x20
	and r0 ,r2
	cmp r0 ,#0x00
	bne Is_Normal_Mode

Is_Easy_Mode:
	ldr r4, BetTable
	ldr r4, [r4 , #0x0]
	b Join
Is_Normal_Mode:
	ldr r4, BetTable
	ldr r4, [r4 , #0x4]
	b Join
Is_Hard_Mode:
	ldr r4, BetTable
	ldr r4, [r4 , #0x8]

Join:
	blh 0x08031ecc          @FE8U 闘技場の掛け金と払戻金を取得

	@掛け金を変動させる     @払い戻し金=掛け金*掛け率/100
	mul  r0,r4
	mov  r1,#100
	blh  0x080D167C         @FE8U div
	mov  r4,r0              @計算結果をr4に保存 掛け金
	
	blh 0x08008a18          @FE8U 会話@0080@0005で参照できるところに数字を書く r0=表示したい数字

	ldr  r0,=0x080B5B58     @FE8U ちっ勝ちやがったかのセリフ
	ldr  r0,[r0]
	mov  r1 ,r5             @謎の値
	blh 0x080b5c04          @FE8U 闘技場 セリフを表示

	mov r0,r4               @掛け金をr0に返す.
	pop {r4}

	ldr r3,=0x80b5b78|1     @FE8U 元に戻す
	bx  r3

.ltorg
.align
BetTable:
