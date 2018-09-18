@4[0] 0D [UNIT] [MARK] [ASM+1]
@
@UNIT=0xFF 
@MARK

.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
	push	{r4,r5,r6,r7 , lr}

	ldr  r4, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrb r0, [r4, #0x0]       @引数0 [FLAG]

	mov  r1,#0xf
	and  r0,r1

	cmp  r0,#0x1
	beq  GetUnitInfoBySVAL1
@	cmp  r0,#0xF
@	beq  GetUnitInfoByCurrent
	cmp  r0,#0xB
	beq  GetCoord

	ldrb r0, [r4, #0x2]       @引数1 [UNIT]
	b   GetUnitInfo

@GetUnitInfoByCurrent:
@	ldr  r0,=#0x03004E50      @FE8U
@	ldr  r0,[r0]
@	b   GetUnitInfo

GetUnitInfoBySVAL1:
	ldr  r0,=#0xFFFFFFFF
	@b   GetUnitInfo

GetUnitInfo:
	blh  0x0800bc50           @FE8U UNITIDの解決 GetUnitStructFromEventParameter
	                          @RAM UNIT POINTERの取得
	cmp  r0,#0x00
	beq  Term                 @取得できなかったら終了

	ldrb r5, [r0,#0x10]       @X
	ldrb r6, [r0,#0x11]       @Y
	b    Show

GetCoord:
	ldr r0, =0x030004E4       @FE8U (MemorySlot0B 主に座標が読みこまれるのに利用されます )
	ldrh r5, [r0,#0x0]        @X
	ldrh r6, [r0,#0x2]        @Y

Show:
	lsl  r5, #0x04            @map.x * 16
	lsl  r6, #0x04            @map.y * 16

	ldr r0, =0x0202bcbc       @FE8U (gCurrentRealCameraPos )
	ldrh r1, [r0,#0x0]        @Camera X
	sub  r5, r1               @X - CameraX

	ldrh r1, [r0,#0x2]        @Camera Y
	sub  r6, r1               @Y - CameraY

	ldrb r0, [r4, #0x3]       @引数2
	lsl  r0,r0, #0x4          @構造体のサイズを掛け算して、位置を特定.

	ldr  r4, Table
	add  r4, r0

	ldr r0, [r4, #0x00]
	ldr r1, =0x06013000
	blh 0x08012f50                @FE8U UnLZ77Decompress 

	ldr r0, [r4, #0x04]
	mov r1, #0x2
	blh 0x0800927c                @FE8U TCS_New Creates a new TCS TCS Pointer r0=Source ROMTCS, r1=OAM Index? idk

	mov r7 ,r0
	mov r0, #0x8c
	lsl r0 ,r0 ,#0x5
	strh r0, [r7, #0x22]
	mov r0 ,r7
	mov r1, #0x0
	blh 0x08009518                @FE8U TCS_SetAnim Sets Anim of TCS void r0=TCS r1=Anim Index

	ldr r0, [r4, #0x08]
	mov r1, #0x3
	blh 0x08002c7c                @FE8U New6C 

	str r7, [r0, #0x50]
	add r5, #0x8
	str r5, [r0, #0x2c]
	sub r6, #0x4
	str r6, [r0, #0x30]

	@効果音を鳴らす
	ldr  r0,=0x0202BCF0                 @FE8U gChapterData
	ADD  r0, #0x41                      @効果音の設定の取得
	LDRB r0, [r0, #0x0]
	LSL  r0 ,r0 ,#0x1E
	CMP  r0, #0x0
	BLT  Term                           @効果音offの場合何もしない
	
	ldrh r0, [r4, #0x0C]                @効果音(Procsの中で鳴らすと変更しずらいのでここで鳴らす)
	blh  0x080d01fc                     @FE8U 効果音を鳴らす関数 m4aSongNumStart r0=鳴らしたい音番号:SOUND

Term:

	mov	r0, #0
pop {r4,r5,r6,r7}
pop {pc}

.ltorg
.align
Table:

@struct {
@	void*	image  32*8,  2*8
@	void*   ROMTS
@	void*	Procs
@   ushort	Sound
@	ushort	padding
@} //sizeof()==16
