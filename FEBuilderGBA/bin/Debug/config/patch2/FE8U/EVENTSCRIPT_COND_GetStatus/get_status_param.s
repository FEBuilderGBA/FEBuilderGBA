@
@ユニットのパラメータを取得
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
	push	{r4,lr}

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

	ldrb r3, [r4, #0x3]       @引数2 param
	cmp  r3, #0x00            @unit pointer
	beq  get_ldr
	cmp  r3, #0x04            @class pointer
	beq  get_ldr
	cmp  r3, #0x0C            @bad status
	beq  get_ldr
	cmp  r3, #0x01            @unit id
	beq  get_unit_id
	cmp  r3, #0x05            @class id
	beq  get_class_id
	cmp  r3, #0x47
	ble  get_ldrb

	cmp  r3, #0x50
	beq  get_1A_lower
	cmp  r3, #0x51
	beq  get_1A_upper

	cmp  r3, #0x52
	beq  get_1D_lower
	cmp  r3, #0x53
	beq  get_1D_upper

	cmp  r3, #0x54
	beq  get_30_lower
	cmp  r3, #0x55
	beq  get_30_upper

	cmp  r3, #0x56
	beq  get_31_lower
	cmp  r3, #0x57
	beq  get_31_upper
	b    Term

get_ldr:
	ldr  r0, [r0,r3]
	b    MakeResult
get_ldrb:
	ldrb r0, [r0,r3]
	b    MakeResult
get_1A_lower:
	mov  r3, #0x1A
	b    get_lower
get_1A_upper:
	mov  r3, #0x1A
	b    get_lower
get_1D_lower:
	mov  r3, #0x1D
	b    get_lower
get_1D_upper:
	mov  r3, #0x1D
	b    get_lower
get_30_lower:
	mov  r3, #0x30
	b    get_lower
get_30_upper:
	mov  r3, #0x30
	b    get_lower
get_31_lower:
	mov  r3, #0x31
	b    get_lower
get_31_upper:
	mov  r3, #0x31
	b    get_lower
get_unit_id:
	ldr  r0, [r0,#0x00]
	cmp  r0, #0x00
	beq  Term
	ldrb r0, [r0,#0x04]
	b    MakeResult

get_class_id:
	ldr  r0, [r0,#0x04]
	cmp  r0, #0x00
	beq  Term
	ldrb r0, [r0,#0x04]
	b    MakeResult

get_upper:
	ldrb r0, [r0,r3]
	lsr  r0, #0x4      @ >>4
	b    MakeResult

get_lower:
	ldrb r0, [r0,r3]

	mov  r2,#0xf
	and  r0, r2
@	b    MakeResult

MakeResult:
	@イベント命令の条件式でぜひが取れるようにする
@	ldr	r2, =0x030004B0  @メモリスロット0
	ldr	r2, =0x030004B8  @メモリスロット0
	str	r0, [r2, #0x30]  @MemorySlotC
Term:
	pop	{pc,r4}
