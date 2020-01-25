.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.macro blr reg
	mov lr, \reg
	.short 0xF800
.endm

.set prShowMenu, EALiterals+0x00

push {lr}
@ldr r0,=0x085C5490 @SubMenu	{J}
ldr r0,=0x0859CFB0 @SubMenu	{U}
ldr r3,prShowMenu
blr r3

mov r0,#0x7  @return 0x7
pop {r1}
bx r1

.ltorg
.align

EALiterals:
	@ POIN prShowMenu
