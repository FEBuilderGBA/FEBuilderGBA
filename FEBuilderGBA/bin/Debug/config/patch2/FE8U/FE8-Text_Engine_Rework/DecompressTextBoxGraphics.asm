.thumb
.include "_Text_Engine_Defs.asm"

@inserted inline at 83F8

@r0=proc

push	{r4-r6,r14}
mov		r6,r0
mov		r5,r6
add		r5,#0x64
ldrh	r4,[r5]
add		r0,r4,#1
strh	r0,[r5]
mov		r0,#1
tst		r0,r4
bne		GoBack
bl		GetCurrentSpeakerAttributesList
ldrb	r0,[r0,#TextBoxTypeIdOffset]
lsl		r0,#2
ldr		r1,=TextBoxTypePointerTable		@defined in EA
add		r0,r1
ldr		r0,[r0]
lsr		r4,#1
lsl		r4,#2
add		r4,r0 
ldr		r5,[r4,#4]		@next graphics (if 0, terminate loop at the end)
ldr		r4,[r4]			@current graphics
mov		r0,#1
bl		GetBgTileDataOffset
ldr		r1,=#0x6000200
add		r1,r0
mov		r0,r4
bl		Decompress
cmp		r5,#0
bne		GoBack
mov		r0,r6
bl		BreakProcLoop
GoBack:
pop		{r4-r6}
pop		{r0}
bx		r0

.ltorg
