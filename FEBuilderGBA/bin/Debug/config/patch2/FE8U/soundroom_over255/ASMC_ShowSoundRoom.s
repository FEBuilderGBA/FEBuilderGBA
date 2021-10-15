.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@指定した曲をSoundRoomに追加します。
@当然ですが、そのSongIDがSoundRoomのリストに掲載されていなければ意味はありません。

push {lr}

@	ldr  r1, =0x030004B0      @FE8J MemorySlot 00	{J}
@	ldr  r1, =0x030004B8      @FE8U MemorySlot 00	{U}
@	ldr  r1, [r1, #0x1 * 0x4]	@あなたがmemory slotを愛しているならここを変えればいい。

	ldr  r1, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrh r1, [r1, #0x2]       @引数1 song id
	
mov r0,#0x0
@mov r1, r1 @song id
@blh 0x080A894C	@SaveSoundRoomEnableList	@{J}
blh 0x080a3f08	@SaveSoundRoomEnableList	@{U}

pop {r0}
bx r0
