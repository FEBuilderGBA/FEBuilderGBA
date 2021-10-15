.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@指定した曲をSoundRoomから消去します
@これは結構めんどいよ

push {lr}

@	ldr  r0, =0x030004B0      @FE8J MemorySlot 00	{J}
@	ldr  r0, =0x030004B8      @FE8U MemorySlot 00	{U}
@	ldr  r0, [r0, #0x1 * 0x4]	@あなたがmemory slotを愛しているならここを変えればいい。

	ldr  r0, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrh r0, [r0, #0x2]       @引数1 song id
	
	@mov r0,r0 @song id
	bl  DeleteAndSaveSoundRoomEnableList

pop {r0}
bx r0

@r0 SongID
DeleteAndSaveSoundRoomEnableList:
push {r4,r5,lr}
sub sp, #0x80
mov r5 ,r0

mov r4, sp
mov r0, sp
@blh 0x080a8890   @LoadSoundRoomEnableList	@{J}
blh 0x080A3E4C   @LoadSoundRoomEnableList	@{U}

cmp r0, #0x0   @データをロードできないなら即終了
beq Exit

@SongIDtoSoundRoomID
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

ldr r1, =1001
cmp r0 ,r1 @念のため1001を超えていないかチェックします。
bge Exit

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
beq Exit  @既にOFF

eor r1, r2  @XORを取ってoffにする
str r1, [r3, #0x0]

@セーブデータに書き込む
mov r0 ,r4
@blh 0x080a88e8   @SaveSoundRoomEnableList_Overwrite	@{J}
blh 0x080A3EA4   @SaveSoundRoomEnableList_Overwrite	@{U}

Exit:
add sp, #0x80
pop {r4,r5}
pop {r0}
bx  r0
