.thumb
.org 0 @jumpToHack at 276b4

@@ change 15690 from 12 to 20
@@202bd31 is Subtitle Help (bit 0x80), nop at 35718 to force subtitles.

push {r4-r7}

CheckOptions:
ldr r0,OptionByte
ldrb r0,[r0]
mov r1,#0x80
and r0,r1
cmp r0,#0 @0 means ON
bne NoBar

DrawBar:
mov r1, #0x10
ldsb r1,[r4,r1] @get x coord
lsl r1,#4
ldr r2,CameraPos
mov r3,#0xc
ldsh r0,[r2,r3] @camera x?
sub r3,r1,r0
mov r0,#0x11 @get y coord
ldsb r0,[r4,r0]
lsl r0,#4
mov r5,#0xe
ldsh r1,[r2,r5] @camera y?
sub r2,r0,r1
mov r1,r3
add r1,#0x10
mov r0,#0x80
lsl r0,#1
lsl r6,r7,#0x18
cmp r1,r0
bhi NoBar
mov r0,r2
add r0,#0x10
cmp r0,#0xb0
bhi NoBar
ldr r1,Xvalue
add r0,r3,r1
sub r1,#2
and r0,r1
mov r1,r2
add r1,#0xfb
mov r2,#0xff
and r1,r2
ldr r3, FramePointers

FindHP:
@if hp=max, r5 = 0
@if hp=0, r5 = E
@and everything inbetween
push {r0-r3}
mov r0,#0x12
ldrb r0,[r4,r0] @max
mov r1,#0x13
ldrb r1,[r4,r1] @curr
cmp r0,r1
bgt NotFull
mov r5,#0 @or
@b NoBar
b HPFound
NotFull:
cmp r1,#0
bne NotEmpty
mov r5,#0xE @or b NoBar?
b HPFound
NotEmpty:
sub r2,r0,r1 @r2 = damage taken
mov r1,r0
mov r0,#0xC
mul r0,r2
swi #0x6     @damage*C/maxHP
mov r5,#1
add r5,r0

HPFound:
pop {r0-r3}
lsl r2,r5,#2
add r2,r3
ldr r2,[r2]
mov r3,#0
ldr r4, WRAMDisplay
mov lr,r4
.short 0xf800

NoBar:
pop {r4-r7}


@original routine
mov r0,r4
add r0,#0x30
ldrb r0,[r0]
lsl r0,#0x1c
lsr r0,#0x1c
ldr r1,Return_to
bx r1



.align
OptionByte:
.long 0x202bd31
CameraPos:
.long 0x202bcb0
Xvalue:
.long 0x201
WRAMDisplay:
.long 0x8002bb8
Return_to:
.long 0x80276be+1
ActiveUnit:
.long 0x3004e50
SlayerCheck:
.long 0x8016c88
CheckEffectiveness:
.long 0x8016bec
CanEquip:
.long 0x8016574
GetItemData:
.long 0x80177B0
TalkCheck:
.long 0x8083f68
WS_FrameData: @should be the OAM data
.long 0x000f0001 @8x8 sprite
.long 0x085201ff @the tile is 0x52
Killer_FrameData:
.long 0x000f0001
.long 0x085301ff @tile #0x53
Talk_FrameData:
.long 0x400f0001 @16x8 sprite
.long 0x087001ee @tile #0x70
FramePointers:
@.long 0x8e903a0 @need to replace
