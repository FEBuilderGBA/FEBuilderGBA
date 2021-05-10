.thumb
.include "_Text_Engine_Defs.asm"

push	{r14}
ldr		r5,[r4]
ldr		r3,=GetCurrentSpeakerAttributesList
mov		r14,r3
.short	0xF800
mov		r6,r0
ldrb	r0,[r5,#0x11]
ldr		r3,=GetFaceProcByPosition
mov		r14,r3
.short	0xF800
cmp		r0,#0
beq		Label1				@if there's no face at the new place, skip Font Setting
add		r0,#TextAttributesOffsetInFaceProc
ldrb	r0,[r0,#FontIdOffset]
ldrb	r1,[r6,#FontIdOffset]
cmp		r0,r1
beq		Label1
strb	r0,[r6,#FontIdOffset]
ldr		r3,=UpdateFontGlyphSet
mov		r14,r3
.short	0xF800
Label1:
ldr		r0,[r5,#4]			@alternate text buffer
cmp		r0,#0
bne		Label2
ldr		r0,[r5]
Label2:
mov		r1,#0
ldr		r3,=GetStringTextWidthWithDialogueCodes
mov		r14,r3
.short	0xF800
pop		{r1}
bx		r1

.ltorg
