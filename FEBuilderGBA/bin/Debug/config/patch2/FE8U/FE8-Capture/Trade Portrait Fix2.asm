.thumb
.org 0x0

@r0=char data
push	{r14}
ldrb	r1,[r0,#0xB]
mov		r2,#0xC0
tst		r1,r2
beq		GetPortrait
mov		r1,#0x1		@blank mug
b		GoBack
GetPortrait:
ldr		r1,PortraitGetter
mov		r14,r1
.short	0xF800
mov		r1,r0
GoBack:
mov		r0,#0x2
pop		{r2}
str		r0,[sp]
mov		r0,#0x1
bx		r2

.align
PortraitGetter:
.long 0x080192B8
