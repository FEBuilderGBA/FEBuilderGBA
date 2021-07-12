@Hook A8928 @{J}
@Hook A3EE4 @{U}
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

@壊すコードの再送
asr r0 ,r5 ,#0x5
lsl r0 ,r0 ,#0x2
add r0 ,r4, r0
mov r1, #0x1f

ExitWithSave:
@ldr r3, =0x080A8930|1	@{J}
ldr r3, =0x080A3EEC|1	@{U}

bx  r3

Exit:
@ldr r3, =0x80A893E|1	@{J}
ldr r3, =0x80A3EFA|1	@{J}
bx  r3
