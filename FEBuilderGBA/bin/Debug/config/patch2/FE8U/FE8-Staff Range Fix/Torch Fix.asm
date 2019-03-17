.thumb
.org 0x0

ldr 	r1,Fog
ldrb 	r2,[r1,#0x0D]	@vision byte. If non-zero, fog is present.
mov		r0,#0x0
cmp		r2,#0x0
beq		GoBack
mov		r0,r4
mov		r1,r5
ldr		r3,RangeWrite
bl		goto_r3
mov		r0,#0x1
GoBack:
ldr		r1,GoBackPtr
bx		r1

goto_r3:
bx		r3

.align
Fog:
.long 0x0202BCF0
GoBackPtr:
.long 0x08028C06+1
RangeWrite:

