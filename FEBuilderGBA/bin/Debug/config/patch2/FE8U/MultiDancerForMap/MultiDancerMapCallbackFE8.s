@r0    ram pointer
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
.org 0
push {lr,r4,r5}

ldr r3, =0x0203E1F0 @(gMapAnimStruct )
mov r1 ,r3
add r1, #0x58
ldrb r2, [r1, #0x0]
lsl r1 ,r2 ,#0x2
add r1 ,r1, r2
lsl r1 ,r1 ,#0x2
add r1 ,r1, r3
ldr r1, [r1, #0x0]  @ pointer:0203E1F0 (gMapAnimStruct )

@r1‚Íƒf[ƒ^‚ª“ü‚Á‚Ä‚¢‚é‚Ì‚ÅÁ‚³‚È‚¢‚æ‚¤‚É!
ldr r2,[r1]
ldrb r4,[r2,#0x4]  @Unit->UnitID
ldr r2,[r1,#0x4]
ldrb r5,[r2,#0x4]  @Class->ClassID

ldr r3, MultiDancerClassTable
sub r3, #0x8

Loop:
add r3, #0x8
ldr r0, [r3,#0x00]
cmp  r0,#0x00
bne  CheckUnit
ldr r0, [r3,#0x04]
cmp  r0,#0x00
beq  NotMatch

CheckUnit:
ldrb r0, [r3,#0x00]
cmp  r0,#0x00
beq  CheckClass
cmp  r0,r4   @Check UnitID
bne  Loop

CheckClass:
ldrb r0, [r3,#0x01]
cmp  r0,#0x00
beq  Match
cmp  r0,r5   @Check ClassID
bne  Loop

Match:
ldrh r0, [r3,#0x04]
b    Exit

NotMatch:
ldr  r0, =0x2D5  @default SE

Exit:
@

ldrb r1, [r1, #0x10]
lsl r1 ,r1 ,#0x4
ldr r2, =0x0202BCB0 @(gMainLoopEndedFlag )
mov r3, #0xc
ldsh r2, [r2, r3]   @ pointer:0202BCBC (gCurrentRealCameraPos )
sub r1 ,r1, r2

blh 0x08014b28   @PlaySpacialSoundMaybe


pop {r4,r5}
pop {r0}
bx r0

.ltorg
MultiDancerClassTable:
@list of the Data List sizeof 8bytes  0x00==TERM
@struct
@byte unitid   00=ANY
@byte classid  00=ANY
@byte wait     00=ANY
@byte 00
@ushort song-id
@ushort 00
