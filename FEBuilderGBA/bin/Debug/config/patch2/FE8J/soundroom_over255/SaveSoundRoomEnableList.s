.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {r4,r5,lr}   @SaveSoundRoomEnableList r1=SongID
sub sp, #0x80
cmp r1 ,#0x0
beq Exit

mov r4 ,r0  @ buffer
mov r5 ,r1  @song id
cmp r4, #0x0
bne SkipLoad

mov r4, sp
mov r0, r4
blh 0x080a8890   @LoadSoundRoomEnableList	{J}
@blh 0x080a3e4c   @LoadSoundRoomEnableList	{U}
cmp r0, #0x0
beq Exit

SkipLoad:

@r4 savedata bool[]
@r5 song id
ldr r3, =0x80B5044	@SoundRoom pointer @{J}
@ldr r3, =0x801BC14	@SoundRoom pointer @{U}
ldr r3, [r3]

Loop:
ldr r0, [r3]
cmp r0, #0x0
blt Exit

cmp r0, r5
beq Found
add r3, #0x10 @ add sizeof() 16
b Loop

Found:
ldr r0, =0x80B5044	@SoundRoom pointer @{J}
@ldr r0, =0x801BC14	@SoundRoom pointer @{U}
ldr r0, [r0]

sub r0, r3, r0
lsr r0, #0x4  @/16

ldr r1 ,=1001
cmp r0 ,r1 @念のため1000を超えていないかチェックします。
bge Exit

mov r5, r0    @r5 is sound room number

@convert to bitflag
asr r2 ,r5 ,#0x5
lsl r2 ,r2 ,#0x2

mov r0 ,r5

mov r1, #0x1f
and r0 ,r1

mov r1 ,#0x01
lsl r1 ,r0

ldr r0, [r4, r2]
and r0 ,r1
cmp r0, #0x0
bne Exit  @既にON

@フラグを立てる
ldr r0, [r4, r2]
orr r0 ,r1
str r0, [r4, r2]

ExitWithSave:
mov r0 ,r4
blh 0x080a88e8   @SaveSoundRoomEnableList_Overwrite	{J}
@blh 0x080a3ea4   @SaveSoundRoomEnableList_Overwrite	{U}

Exit:
add sp, #0x80
pop {r4,r5}
pop {r0}
bx r0
