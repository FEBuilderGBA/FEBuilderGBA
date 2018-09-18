.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ titlebackgroundpalette, titlebackgroundimage+4
.equ titlebackgroundtsa, titlebackgroundpalette+4

.thumb

@set 256 colors mode for bg
ldr	r0,=#0x0300308C
ldrb	r1,[r0]
mov	r2,#0x80
orr	r1,r2
strb	r1,[r0]

@load graphics
ldr	r0,titlebackgroundimage
ldr	r1,=#0x06002E00
blh	0x08012F50
ldr	r0,titlebackgroundimage
ldr	r1,=#0x06000000
blh	0x08012F50

@load palette
ldr	r0,titlebackgroundpalette
mov	r1,#0
ldr	r2,=#0x200
blh	0x8000DB8

@load tsa
mov	r0,#0
blh	0x08001C4C
push	{r0}
mov	r0,#1
blh	0x08001C4C
mov	r1,r0
pop	{r0}
ldr	r2,titlebackgroundtsa
Loop:
ldr	r3,[r2]
str	r3,[r0]
add	r0,#4
add	r2,#4
cmp	r0,r1
beq	End
b	Loop

End:
ldr	r0,=#0x080C583D
bx	r0

.align
.ltorg

titlebackgroundimage:
@POIN titlebackgroundimage
@POIN titlebackgroundpalette
@POIN titlebackgroundtsa

