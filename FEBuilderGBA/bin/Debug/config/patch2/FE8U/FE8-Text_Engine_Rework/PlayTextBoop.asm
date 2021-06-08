.thumb
.include "_Text_Engine_Defs.asm"

@called at 6D70

push	{r14}
ldrb	r0,[r1]
lsl		r0,#0x1E
cmp		r0,#0
blt		SoundDone
ldr		r3,=GetCurrentSpeakerAttributesList
mov		r14,r3
.short	0xF800
ldrb	r0,[r0,#TextBoopPitchIdOffset]
mov		r1,#12
mul		r1,r0
ldr		r0,=TextBoopTable			@defined in EA
add		r1,r0
ldr		r0,=(gMPlayTable+0x24)		@not entirely sure what this is
ldr		r0,[r0]
ldr		r3,=MPlayStart
mov		r14,r3
.short	0xF800
SoundDone:
pop		{r0}
bx		r0

.ltorg
