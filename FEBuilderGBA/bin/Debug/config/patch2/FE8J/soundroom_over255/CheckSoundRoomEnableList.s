.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {r4,r5,lr}
sub sp, #0x80
mov r4 ,r0
mov r5 ,r1

cmp r4, #0x0
bne LoadBuffer
mov r4, sp

LoadBuffer:
mov r0, r4
blh 0x080a8890   @LoadSoundRoomEnableList	@{J}
@blh 0x080A3E4C   @LoadSoundRoomEnableList	@{U}
cmp r0, #0x0
beq ReturnTrue

@r4 savedata bool[]
@r5 song id
ldr r3, =0x80B5044	@SoundRoom pointer @{J}
@ldr r3, =0x801BC14	@SoundRoom pointer @{U}
ldr r3, [r3]

Loop:
ldr r0, [r3]
cmp r0, #0x0
blt ReturnFalse

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
cmp r0 ,r1 @念のため1001を超えていないかチェックします。
bge ReturnFalse

mov r5, r0    @r5 is sound room id

@壊すコードの再送
asr r0 ,r5 ,#0x5
lsl r0 ,r0 ,#0x2
add r0 ,r4, r0
mov r1, #0x1f

and r1 ,r5
ldr r0, [r0, #0x0]
lsr r0 ,r1
mov r1, #0x1
and r0 ,r1
cmp r0, #0x0
bne ReturnTrue

ReturnFalse:
mov r0, #0x0
b   Exit

ReturnTrue:
mov r0, #0x1

Exit:
add sp, #0x80
pop {r4,r5}
pop {r1}
bx r1
