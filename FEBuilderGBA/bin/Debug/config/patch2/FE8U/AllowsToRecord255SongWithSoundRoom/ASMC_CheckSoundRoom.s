.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@指定した曲がSound roomに追加されているか判定します
@結果はSlotCに返されます
@
@使いようによっては、セーブデータを超えたデータの転送にも使えますね。

push {lr}

@	ldr  r1, =0x030004B0      @FE8J MemorySlot 00	@{J}
@	ldr  r1, =0x030004B8      @FE8U MemorySlot 00	@{U}
@	ldr  r1, [r1, #0x1 * 0x4]	@あなたがmemory slotを愛しているならここを変えればいい。

	ldr  r1, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrh r1, [r1, #0x2]       @引数1 song id

mov r0,#0x0
@mov r1, r1 @song id
@blh 0x80A8914	@CheckSoundRoomEnableList	@{J}
blh 0x80A3ED0	@CheckSoundRoomEnableList	@{U}

@	ldr  r1, =0x030004B0      @FE8J MemorySlot 00	@{J}
	ldr  r1, =0x030004B8      @FE8U MemorySlot 00	@{U}

	str  r0, [r1, #0xC * 0x4] @store slotC

pop {r0}
bx r0
