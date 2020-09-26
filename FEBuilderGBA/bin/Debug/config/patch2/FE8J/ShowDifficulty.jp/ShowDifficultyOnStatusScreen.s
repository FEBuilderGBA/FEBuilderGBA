@Call 0x90898	{J}
@Call 0x8E5C4	{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


sub  sp, #0x8

bl   GetDifficulty
bl   GetTextData
cmp  r0, #0x0
beq  Exit
mov  r4, r0

ldrh r0,[r4,#0x0] @TextID
blh  0x08009fa8    @GetStringFromIndex	@{J}
@blh  0x0x0800a240    @GetStringFromIndex	@{U}

mov  r2 ,r0
mov  r0, #0x0
ldr  r1, =0x02022DCC	@{J}	DrawTile  @上に
@ldr  r1, =0x02022DD0	@{U}	DrawTile  @上に
mov  r3, #0xC
str  r3,[sp, #0x0]
str  r2,[sp, #0x4]
ldrb r2,[r4,#0x3]       @Color
mov  r3, #0x0

blh  0x08004374,r4   @DrawTextInline	{J}
@blh  0x0800443C,r4   @DrawTextInline	{U}

Exit:

add  sp, #0x8

@壊すコードの再送
mov  r4 ,r6
add  r4, #0x2b
ldrb r0, [r4, #0x0]
cmp  r0, #0x0

ldr  r3,=0x080908A0|1	@{J}
@ldr  r3,=0x0808E5CC|1	@{U}
bx   r3


GetDifficulty:
push {lr}
ldr  r0, =0x0202BCEC	@ChapterData	{J}
@ldr  r0, =0x0202BCF0	@ChapterData	{U}

mov  r1, #0x14
ldrb r1, [r0, r1]
mov  r2, #0x40
and  r1, r2
cmp  r1, #0x40
beq  GetDifficulty_HardMode

mov  r1, #0x42
ldrb r1, [r0, r1]
mov  r2, #0x20
and  r1, r2
cmp  r1, #0x20
beq  GetDifficulty_NormalMode

mov  r3, #0x0
b    GetDifficulty_AddIfCasualMode

GetDifficulty_NormalMode:
mov  r3, #0x2
b    GetDifficulty_AddIfCasualMode

GetDifficulty_HardMode:
mov  r3, #0x4
@b    GetDifficulty_AddIfCasualMode

GetDifficulty_AddIfCasualMode:
mov  r1, #0x42
ldrb r1, [r0, r1]
mov  r2, #0x40
and  r1, r2
cmp  r1, #0x40
beq  GetDifficulty_CasualMode

GetDifficulty_ClassicMode:
add  r3, #0x1
b    GetDifficulty_Exit

GetDifficulty_CasualMode:
@add r3, #0x0

GetDifficulty_Exit:
mov  r0, r3
pop {r1}
bx r1



@難易度の文字列の位置を取得する
@W0 Text
@B2 Color
@B3 Color
@
GetTextData:
push {lr}

cmp r0,#0x6
bge GetTextData_Error

ldr r1, Table
lsl r0, #0x2	@r0 * 4
add r0, r1
b   GetTextData_Exit

GetTextData_Error:
mov r0, #0x0

GetTextData_Exit:
pop {r1}
bx r1

.ltorg
Table:
