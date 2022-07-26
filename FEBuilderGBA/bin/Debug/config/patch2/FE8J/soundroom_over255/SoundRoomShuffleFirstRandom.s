.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@Hook 0xB3B9C	@{J}
@Hook 0xAEF7C	@{U}
ldr r3, =0x02021388	@Soundroom random list @{J}	@{U}
ldr r3, [r3]
cmp r3, #0xff
bge Exit

blh 0x08000cd8 @GetGameClock	{J}
@blh 0x08000D28 @GetGameClock	{U}
mov r5, #0x3F
and r5, r0

Loop:
cmp r5, #0x0
ble Exit
sub r5, #0x1
blh 0x08000B60 @NextRN	{J}
@blh 0x08000B88 @NextRN	{U}
b  Loop

Exit:
ldr r3,=0x080B3BA8|1	@{J}
@ldr r3,=0x080AEF88|1	@{U}
bx  r3
