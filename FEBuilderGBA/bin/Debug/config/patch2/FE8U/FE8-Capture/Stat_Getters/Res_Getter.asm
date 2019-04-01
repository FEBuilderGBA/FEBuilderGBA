.thumb
.org 0x0

@new res getter; bxed to from 19270
push	{r4-r5,r14}
mov		r4,r0
ldr		r1,Get_Equipped_Weapon
mov		r14,r1
.short	0xF800
ldr		r1,Get_Weapon_Res_Bonus
mov		r14,r1
.short	0xF800
mov		r5,r0
ldrb	r0,[r4,#0x18]
add		r5,r0
mov		r0,#0x31
add		r0,r4
ldrb	r0,[r0]
lsr		r0,#0x1				@barrier nibble
add		r5,r0
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
Get_Weapon_Res_Bonus:
.long 0x080164E0
Is_Capture_Set:
@
