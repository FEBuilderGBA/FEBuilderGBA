@SingleBGMの場合、戦闘後のBGMは常に最初から再生する
@Call 080025E8	{J}
@Call 08002698	{U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

ldrh r0, [r4, #0x2]

bl IsSingleBGM
cmp r0, #0x01
bne FalseExit

StartFromBeginning:
mov  r0, #0x0
b    Exit

FalseExit:
mov  r0, #0x1

Exit:
strb r0, [r4, #0x6]   @BGMSTRUCT@BGM.音楽関係のフラグ2
strb r5, [r4, #0x7]   @BGMSTRUCT@BGM.音楽関係のフラグ3
ldrh r0, [r4, #0x2]   @pointer:02024E5E

ldr r3,=0x080025F0|1	@{J}
bx r3


IsSingleBGM:
push {lr}

ldr r3 ,=0x0800280C @SongTablePointer	@{J}
@ldr r3 ,=0x080028BC @SongTablePointer	@{U}
ldr r3 ,[r3]
lsl r0,r0, #0x3  @ SongID * 8
add r3 ,r0

ldr r3,[r3]   @SongHeader
ldrb r0, [r3] @SongHeader.TrackID
cmp r0, #0x1
bgt IsSingleBGM_False

ldr r3,[r3,#0x8]   @SongHeader.Track1

ldrb r0,[r3,#0xA]  @全休符探索
cmp  r0,#0xB0
bne  IsSingleBGM_False

ldrb r0,[r3,#0xB]
cmp  r0,#0xB0
bne  IsSingleBGM_False

ldrb r0,[r3,#0xC]
cmp  r0,#0xB0
bne  IsSingleBGM_False

mov r0,#0x01
b   IsSingleBGM_Exit

IsSingleBGM_False:
mov r0,#0x00

IsSingleBGM_Exit:

pop {r1}
bx r1
