.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {lr}   @SaveSoundRoomEnableList_Overwrite
mov r3 ,r0

ldr r1, =0x0E000000 @SaveDataArea
ldr r2, =0x080A88D8	@SaveDataに追加数数字 {J}
@ldr r2, =0x080A3E94	@SaveDataに追加数数字 {U}
ldr r2, [r2]
add r1 ,r1, r2      @せーぶデータのSoundRoom部

mov r0 ,r3
mov r2, #0x80
blh 0x080d6420   @SRAMTransfer	@{J}
@blh 0x080D1724   @SRAMTransfer	@{U}
pop {r0}
bx r0
