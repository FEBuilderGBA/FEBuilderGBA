.thumb
.org 0x0

@r0=battle struct
add		r0,#0x48
ldrh	r0,[r0]
lsl		r0,#0x18
lsr		r0,#0x18
mov		r1,#0x24
mul		r0,r1
ldr		r1,ItemTable
add		r0,r1
ldr		r0,[r0,#0x8]		@weapon abilities
mov		r2,#0x0
mov		r1,#0x2				@magic
tst		r0,r1
beq		IsNotMagic
mov		r2,#0x1
IsNotMagic:
mov 	r0,r2
bx		r14

.align
ItemTable:
@
