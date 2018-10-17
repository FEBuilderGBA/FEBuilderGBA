@
@ユニットのパラメータの設定
@
@40 0D [UNIT] [PARAM] [ASM+1]
@41 0D 00 [PARAM] [ASM+1]  (Load SVAL1 ID)
@4B 0D 00 [PARAM] [ASM+1]  (Load SVALB coord)
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
	ldr  r0,=#0xFFFFFFFF
	b   GetUnitInfo

GetUnitInfoByCurrent:
@	ldr  r0,=#0x03004DF0      @操作中のユニット {J}
	ldr  r0,=#0x03004E50      @操作中のユニット {U}
	ldr  r0,[r0]
	b   ProcessMain

GetUnitInfoByCoord:
	ldr  r0,=#0xFFFFFFFE
	@b   GetUnitInfo

GetUnitInfo:
@	blh  0x0800bf3c           @UNITIDの解決 GetUnitStructFromEventParameter	{J}
	blh  0x0800bc50           @UNITIDの解決 GetUnitStructFromEventParameter	{U}

ProcessMain:
	cmp  r0,#0x00
	beq  Term                 @取得できなかったら終了


                              @設定する値.
@	ldr	r2, =0x030004B0       @メモリスロット0
	ldr	r2, =0x030004B8       @メモリスロット0
	ldr	r2, [r2, #0x8]        @MemorySlot2

	ldrb r3, [r4, #0x3]       @引数2 param
	cmp  r3, #0x00            @unit pointer
	beq  set_str
	cmp  r3, #0x04            @class pointer
	beq  set_str
	cmp  r3, #0x0C            @bad status
	beq  set_str
	cmp  r3, #0x01            @unit id
	beq  set_unit_id
	cmp  r3, #0x05            @class id
	beq  set_class_id
	cmp  r3, #0x47
	ble  set_strb

	cmp  r3, #0x50
	beq  set_1A_lower
	cmp  r3, #0x51
	beq  set_1A_upper

	cmp  r3, #0x52
	beq  set_1D_lower
	cmp  r3, #0x53
	beq  set_1D_upper

	cmp  r3, #0x54
	beq  set_30_lower
	cmp  r3, #0x55
	beq  set_30_upper

	cmp  r3, #0x56
	beq  set_31_lower
	cmp  r3, #0x57
	beq  set_31_upper
	b    Term

set_str:
	str  r2, [r0,r3]
	b    MakeResult
set_strb:
	strb r2, [r0,r3]
	b    MakeResult
set_1A_lower:
	mov  r3, #0x1A
	b    set_lower
set_1A_upper:
	mov  r3, #0x1A
	b    set_lower
set_1D_lower:
	mov  r3, #0x1D
	b    set_lower
set_1D_upper:
	mov  r3, #0x1D
	b    set_lower
set_30_lower:
	mov  r3, #0x30
	b    set_lower
set_30_upper:
	mov  r3, #0x30
	b    set_lower
set_31_lower:
	mov  r3, #0x31
	b    set_lower
set_31_upper:
	mov  r3, #0x31
	b    set_lower
set_unit_id:
	mov  r5, r0        @unit ram構造体のポインタを保存
	mov  r0, r2        @unit id
@	blh  0x08019108    @GetUnitStruct
	blh  0x08019430    @GetUnitStruct
	cmp  r0, #0x00
	beq  Term

	str  r0, [r5, #0x00]
	b    MakeResult

set_class_id:
	mov  r5, r0        @unit ram構造体のポインタを保存
	mov  r0, r2        @class id
@	blh  0x0801911C    @GetROMClassStruct
	blh  0x08019444    @GetROMClassStruct
	cmp  r0, #0x00
	beq  Term

	str  r0, [r5, #0x04]
	b    MakeResult

set_upper:
	ldrb r5, [r0,r3]    @  ([r0,r3] & 0xf) | r2 << 4
	mov  r1,#0xf
	and  r5,r1
	lsl  r2,#0x4
	orr  r5,r2
	strb r5, [r0,r3]
	b    MakeResult

set_lower:
	ldrb r5, [r0,r3]    @  ([r0,r3] & 0xf0) | r2
	mov  r1,#0xf0
	and  r5,r1
	orr  r5,r2
	strb r5, [r0,r3]
@	b    MakeResult

MakeResult:
@	blh  0x08019ecc   @RefreshFogAndUnitMaps 
@	blh  0x08027144   @SMS_UpdateFromGameData 
@	blh  0x08019914   @UpdateGameTilesGraphics 
	blh  0x0801a1f4   @RefreshFogAndUnitMaps
	blh  0x080271a0   @SMS_UpdateFromGameData
	blh  0x08019c3c   @UpdateGameTilesGraphics
Term:
	mov r0, #0x00
	pop	{pc,r4,r5}
