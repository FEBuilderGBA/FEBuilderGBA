.thumb
.org 0x0

@new skl getter; bxed to from 191D0
push	{r4-r5,r14}
mov		r4,r0
ldr		r1,Get_Equipped_Weapon
mov		r14,r1
.short	0xF800
ldr		r1,Get_Weapon_Skl_Bonus
mov		r14,r1
.short	0xF800
mov		r5,r0
ldrb	r0,[r4,#0x15]
add		r5,r0
ldr		r0,[r4,#0xC]
mov		r1,#0x10
tst		r0,r1
beq		CheckCapture
lsr		r5,#0x1
b		GoBack					@can't capture if you're rescuing, so no sense in checking
CheckCapture:
ldr		r0,Is_Capture_Set
mov		r14,r0
mov		r0,r4
.short	0xF800
cmp		r0,#0x0
beq		GoBack
lsr		r5,#0x1
GoBack:
mov		r0,r5
pop		{r4-r5}
pop		{r1}
bx		r1

.align
Get_Equipped_Weapon:
.long 0x08016B28
Get_Weapon_Skl_Bonus:
.long 0x08016450
Is_Capture_Set:
@
