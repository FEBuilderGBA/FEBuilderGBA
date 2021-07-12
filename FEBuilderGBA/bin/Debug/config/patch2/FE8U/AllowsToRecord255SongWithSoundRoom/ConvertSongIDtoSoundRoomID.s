@Hook 80A8968 {J}
@Hook 80A3F24 {U}
.thumb

@r4 savedata bool[]
@r5 song id
@ldr r3, =0x80B5044	@SoundRoom pointer @{J}
ldr r3, =0x801BC14	@SoundRoom pointer @{U}
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
@ldr r0, =0x80B5044	@SoundRoom pointer @{J}
ldr r0, =0x801BC14	@SoundRoom pointer @{U}
ldr r0, [r0]

sub r0, r3, r0
lsr r0, #0x4  @/16

cmp r0 ,#0xff @念のため0xff(255)を超えていないかチェックします。
bgt Exit

mov r5, r0    @r5 is sound room id

@convert to bitflag
asr r0 ,r5 ,#0x5
lsl r0 ,r0 ,#0x2
add r3 ,r4, r0
mov r0, #0x1f
and r0 ,r5
mov r2, #0x1
lsl r2 ,r0
ldr r1, [r3, #0x0]
mov r0 ,r1
and r0 ,r2
cmp r0, #0x0
bne Exit  @既にON

orr r1 ,r2
str r1, [r3, #0x0]

ExitWithSave:
@ldr r3, =0x80A89BA|1	@{J}
ldr r3, =0x80A3f76|1	@{U}
bx  r3

Exit:
@ldr r3, =0x80a89c0|1	@{J}
ldr r3, =0x80A3F7C|1	@{U}
bx  r3
