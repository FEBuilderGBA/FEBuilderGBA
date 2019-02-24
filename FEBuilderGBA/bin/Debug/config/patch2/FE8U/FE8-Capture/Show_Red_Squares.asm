.thumb
.org 0x0

@jumped to from 22DAC
@this displays the red squares for the current weapon. It is being modified to only show 1 range if capturing, even if the weapon has longer range (ie, 1-2 -> 1-1)

push	{r14}
ldr		r1,Is_Capture_Set
mov		r14,r1
.short	0xF800
cmp		r0,#0x0
bne		GoBack			@returns 1 if true, which is coincidentally what we need the range to be
ldr		r0,Get_Range_Bitfield
mov		r14,r0
ldr		r0,[r4]
mov		r1,#0x0
ldsb	r1,[r5,r1]		@inventory slot
.short	0xF800
GoBack:
mov		r1,r0
ldr		r0,[r4]
pop		{r2}
bx		r2

.align
Get_Range_Bitfield:
.long 0x080171E8
Is_Capture_Set:
@
