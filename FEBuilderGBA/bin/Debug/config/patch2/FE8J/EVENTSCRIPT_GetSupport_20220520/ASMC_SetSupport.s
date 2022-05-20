@
@支援のレベルの設定
@
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb

push {r4,lr}
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
ldr  r0,=0x03004DF0      @操作中のユニット {J}
@ldr  r0,=0x03004E50      @操作中のユニット {U}
ldr  r0,[r0]
b   ProcessMain

GetUnitInfoByCoord:
ldr  r0,=0xFFFFFFFE
@b   GetUnitInfo

GetUnitInfo:
blh  0x0800bf3c           @UNITIDの解決 GetUnitStructFromEventParameter	{J}
@blh  0x0800bc50           @UNITIDの解決 GetUnitStructFromEventParameter	{U}

ProcessMain:
cmp  r0,#0x00
beq  Exit                 @取得できなかったら終了

ldrb r1, [r4, #0x3]       @引数2 [UNIT] ターゲットUnit ID
mov  r4, r0               @現在のUnitIDを保存
bl SetSupport

cmp r0,#0x00
beq Exit

@相手の支援を消すため、反転して実行する
ldr r1, [r4, #0x0]       @ターゲットUnit
ldrb r1, [r1, #0x04]     @UnitID
bl SetSupport

Exit:
pop {r4}
pop {r0}
bx r0


SetSupport:
push {r4,r5,r6,r7,lr}
mov r4,r1       @Target Unit ID
mov r7,r0       @My Unit Pointer
blh 0x0802815c   @GetROMUnitSupportCount {J}
@blh 0x080281c8   @GetROMUnitSupportCount {U}
mov r6, r0
mov r5, #0x0

Loop:
cmp r5 ,#0x7
bge NotFound

mov r0,r7      @My Unit Pointer
mov r1,r5      @Index
blh 0x08028188   @GetUnitSupportingUnit {J}
@blh 0x080281f4   @GetUnitSupportingUnit {U}

cmp r0, #0x0
beq Next

ldr  r1, [r0, #0x0]  @相手のUnit
ldrb r1, [r1, #0x4]  @相手のUnitID
cmp r1,r4
bne Next

Match:
mov r6, r0
bl GetSupportValue

mov r1,#0x32
add r1,r5
add r1,r7
strb r0,[r1]

mov  r0,r6
b    Return

Next:
add r5, #0x01
b   Loop

NotFound:
mov r0,#0x00

Return:
pop {r4,r5,r6,r7}
pop {r1}
bx r1

GetSupportValue:
push {lr}
ldr r2, =0x030004B0  @MemorySlot {J}
@ldr r2, =0x030004B8 @MemorySlot {U}
ldrb r2, [r2, #0x8]  @Slot2
cmp r2,#0x00
beq Level0
cmp r2,#0x01
beq LevelC
cmp r2,#0x02
beq LevelB

LevelA:
mov r0,#0xF1
b   GetSupportValue_Return

LevelB:
mov r0,#0xB1
b   GetSupportValue_Return

LevelC:
mov r0,#0x51
b   GetSupportValue_Return

Level0:
mov r0,#0x0

GetSupportValue_Return:
pop {r1}
bx r1

