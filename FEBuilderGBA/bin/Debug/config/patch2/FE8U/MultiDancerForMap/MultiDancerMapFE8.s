@Call 080812D4 (FE8U)
@r0    ram pointer

.thumb
.org 0
push {r4,r5}
ldr r1,[r0]
ldrb r4,[r1,#0x4]  @Unit->UnitID
ldr r1,[r0,#0x4]
ldrb r5,[r1,#0x4]  @Class->ClassID

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
ldrb r1, [r3,#0x03]
b    Exit

NotMatch:
mov  r1, #0xc

Exit:
ldr r0, MultiDancerClassTable+4

pop {r4,r5}
ldr	r3,=0x0080812F4+1 @For FE8U
bx	r3

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
