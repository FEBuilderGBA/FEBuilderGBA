.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@SoundRoomに登録されているすべての曲を非表示にします

push {r4,r5,r6,lr}
sub sp, #0x24

mov r4, sp
mov r0, sp
blh 0x080a8890   @LoadSoundRoomEnableList	@{J}
@blh 0x080A3E4C   @LoadSoundRoomEnableList	@{U}

mov r5, #0x0 @counter

ldr r6, =0x80B5044	@SoundRoom pointer @{J}
@ldr r6, =0x801BC14	@SoundRoom pointer @{U}
ldr r6, [r6]

Loop:
ldr r0, [r6]
cmp r0, #0x0
blt Exit

cmp r5, #0xff
bge Exit

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

eor r1, r2  @XORを取ってoffにする
str r1, [r3, #0x0]

add r3, #0x10 @ add sizeof() 16
add r5, #0x1  @sound room id ++
b Loop

Exit:

@セーブデータに書き込む
mov r0 ,r4
blh 0x080a88e8   @SaveSoundRoomEnableList_Overwrite	@{J}
@blh 0x080A3EA4   @SaveSoundRoomEnableList_Overwrite	@{U}

add sp, #0x24
pop {r4,r5,r6}
pop {r0}
bx  r0
