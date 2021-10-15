.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr}		@InitializeSoundRoomEnableList
sub sp, #0x80

mov r0 ,sp
mov r1 ,#0x0
mov r2 ,#0x80   @ビットフラグで1000個入る領域を初期化します
@blh 0x080D6968	@memset	{J}
blh 0x080d1c6c		@memset	{U}

mov r0, sp
@blh 0x080a88e8   @SaveSoundRoomEnableList_Overwrite	@{J}
blh 0x080A3EA4   @SaveSoundRoomEnableList_Overwrite	@{U}
add sp, #0x80
pop {r0}
bx r0
