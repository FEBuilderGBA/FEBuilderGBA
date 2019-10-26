.thumb

@hook at $464476
push	{r4}
push	{lr}
ldr		r4, AutoNewline
bl		BXR4
pop		{r4} @what used to be in lr
mov		lr, r4
pop		{r4}
bx		lr

BXR4:
bx		r4

.ltorg
.align
AutoNewline: