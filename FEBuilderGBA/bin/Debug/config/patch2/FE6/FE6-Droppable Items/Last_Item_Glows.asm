.thumb
.org 0

@called at 6F030
@r0=char data, r5=item id/uses, r4=item slot #
@return palette index in r2

push	{r14}
ldr		r1,Can_Drop_Item
mov		r14,r1
.short	0xF800
cmp		r0,#0
beq		Label1
mov		r0,r9
ldr		r0,[r0,#0xC]		@char data ptr
ldr		r1,=#0x8017520
mov		r14,r1
.short	0xF800
sub		r0,#1
cmp		r0,r4
bne		Label1
mov		r2,#4				@glowy palette index
b		GoBack

Label1:
mov		r0,r9
ldr		r0,[r0,#0xC]
mov		r1,r5
ldr		r2,=#0x8016BD8		@checks if item can be used, I guess
mov		r14,r2
.short	0xF800
mov		r2,#0
cmp		r0,#0
bne		GoBack
mov		r2,#1
GoBack:
@remember, return's in r2
pop		{r1}
bx		r1

.ltorg
Can_Drop_Item:
@
