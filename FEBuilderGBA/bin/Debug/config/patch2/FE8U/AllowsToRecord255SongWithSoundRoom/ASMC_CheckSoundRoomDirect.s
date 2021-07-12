.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@指定したSound room id が有効か判定します
@結果はSlotCに返されます
@
@使いようによっては、セーブデータを超えたデータの転送にも使えますね。

push {lr}

@	ldr  r0, =0x030004B0      @FE8J MemorySlot 00	@{J}
@	ldr  r0, =0x030004B8      @FE8U MemorySlot 00	@{U}
@	ldr  r0, [r0, #0x1 * 0x4]	@あなたがmemory slotを愛しているならここを変えればいい。

	ldr  r0, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrh r0, [r0, #0x2]       @引数1 song id

	cmp  r0, #0x0
	beq  StoreValue
	cmp  r0, #0xff
	bgt  StoreValue

	sub  r0, #0x01	@sound room id
	bl   CheckSoundRoomEnableList

StoreValue:
@	ldr  r1, =0x030004B0      @FE8J MemorySlot 00	@{J}
	ldr  r1, =0x030004B8      @FE8U MemorySlot 00	@{U}

	str  r0, [r1, #0xC * 0x4] @store slotC

pop {r0}
bx r0

@r0 soundroomID
CheckSoundRoomEnableList:
push {r4,r5,lr}
sub sp, #0x24
mov r5 ,r0

mov r4, sp
mov r0, sp
@blh 0x080a8890   @LoadSoundRoomEnableList	@{J}
blh 0x080A3E4C   @LoadSoundRoomEnableList	@{U}

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
bne TrueExit

FalseExit:
mov r0, #0x0
b   Exit

TrueExit:
mov r0, #0x1

Exit:
add sp, #0x24
pop {r4,r5}
pop {r1}
bx  r1
