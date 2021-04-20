@この条件にマッチするすべての敵軍のAIをSVAL1の内容に変更
@4[0] 0D [UNIT] [CLASS] [ASM+1]
@
@この条件にマッチするすべての友軍のAIをSVAL1の内容に変更
@4[1] 0D [UNIT] [CLASS] [ASM+1]
@
@UNIT=0x00 ANY
@CLASS=0x00 ANY
@
@Author 7743
@
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@
@
.thumb
	push	{r4,r5,r6,lr}

	ldr  r4, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrb r0, [r4, #0x0]       @引数0 4[引数0]

	mov  r1,#0xf              @check type
	and  r0,r1                
	cmp  r0,#0x00
	bne  EachAlly
	                          @EachEnemy
	ldr  r5,=0x0202CFB8       @Enemy Units
	ldr  r6,=0x0202CFB8+(0x32*0x48)       @End Address
	b Each

EachAlly:
	ldr  r5,=0x0202DDC8       @Ally Units
	ldr  r6,=0x0202DDC8+(0x14*0x48)       @End Address

Each:
	ldr  r0,[r5]              @get rom unit pointer
	cmp  r0,#0x00
	beq  NextLoop

	ldrb r2, [r4, #0x2]       @引数1 40 0D [引数1] 00 [プログラム場所 XX XX XX 08]
	cmp  r2,#0x00
	beq  CheckClass
                              @CheckUnitID
	ldrb r1,[r0,#0x4]         @get unit id
	cmp  r1,r2
	bne  NextLoop

CheckClass:
	ldr  r0,[r5,#0x4]         @get rom class pointer
	cmp  r0,#0x00
	beq  NextLoop

	ldrb r2, [r4, #0x3]       @引数2 40 0D 00 [引数2] [プログラム場所 XX XX XX 08]
	cmp  r2,#0x00
	beq  Change
                              @CheckClassID
	ldrb r1,[r0,#0x4]         @get class id
	cmp  r1,r2
	bne  NextLoop

Change:
	ldr  r0,=0x030004B0       @メモリスロット0の位置
	ldrb r1,[r0,#4]           @AI1の設定 [AI1][AI2]0000  (実際にはリトルエンディアン)
	ldrb r2,[r0,#5]           @AI2の設定
	mov r0,r5                 @r0=ram unit pointer

	blh  0x08011DB0           @ChangeAI


NextLoop:
	add  r5,#0x48
	cmp  r5,r6
	blt  Each

Term:
	mov	r0, #0
	pop {pc,r6,r5,r4}
