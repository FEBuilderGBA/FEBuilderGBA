@
@ユニットのステータスフラグを静かに変更します
@バッチ処理で一気に数十人のユニットのステータスフラグを変更すると、変更に耐えきれずゲームがハングアップするのを抑制します
@この処理でステータスを変更しても、マップのユニットの状態は更新されません。
@終了イベント、開始イベントでの一括バッチ処理に利用してください。
@
@UNCM_slient < セット
@UNCR_slient < 解除
@REVEAL_slient
@REMU_slient
@
@40 0D [UNIT] [PARAM] [ASM+1]                 + sval2
@41 0D 00 [PARAM] [ASM+1]  (Load SVAL1 ID)    + sval2
@4B 0D 00 [PARAM] [ASM+1]  (Load SVALB coord) + sval2
@
@param 1 = set
@param 0 = unset
@

@Author 7743
@
.align 4
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@
@
	push	{r4,r5,lr}

	ldr  r4, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ

	ldrb r0, [r4, #0x0]       @引数0 [FLAG]

	mov  r1,#0xf
	and  r0,r1

	cmp  r0,#0x1
	beq  GetUnitInfoBySVAL1
	cmp  r0,#0xB
	beq  GetUnitInfoByCoord
	cmp  r0,#0xF
	beq  GetUnitInfoByCurrent

	ldrb r0, [r4, #0x2]       @引数1 [UNIT]
	b   GetUnitInfo

GetUnitInfoBySVAL1:
	ldr  r0,=0xFFFFFFFF
	b   GetUnitInfo

GetUnitInfoByCurrent:
@	ldr  r0,=0x03004DF0      @操作中のユニット {J}
	ldr  r0,=0x03004E50      @操作中のユニット {U}
	ldr  r0,[r0]
	b   ProcessMain

GetUnitInfoByCoord:
	ldr  r0,=0xFFFFFFFE
	@b   GetUnitInfo

GetUnitInfo:
@	blh  0x0800bf3c           @UNITIDの解決 GetUnitStructFromEventParameter	{J}
	blh  0x0800bc50           @UNITIDの解決 GetUnitStructFromEventParameter	{U}

ProcessMain:
	cmp  r0,#0x00
	beq  Term                 @取得できなかったら終了


                              @設定する値.
@	ldr	r2, =0x030004B0       @メモリスロット0	{J}
	ldr	r2, =0x030004B8       @メモリスロット0	{U}
	ldr	r2, [r2, #0x8]        @MemorySlot2

	ldrb r3, [r4, #0x3]       @引数2 param
	cmp  r3, #0x00            @unit pointer
	beq  label_off

label_on:
	ldr r1, [r0,#0xC]         @状態フラグ
	orr r1,r2
	str r1, [r0,#0xC]         @状態フラグ

	b Term
label_off:
	ldr r1,=0xFFFFFFFF
	eor r2,r1   @ <-事実上のNOT命令

	ldr r1, [r0,#0xC]         @状態フラグ
	and r1,r2
	str r1, [r0,#0xC]         @状態フラグ
	
@	b Term

Term:
	mov r0, #0x00
	pop	{pc,r4,r5}
