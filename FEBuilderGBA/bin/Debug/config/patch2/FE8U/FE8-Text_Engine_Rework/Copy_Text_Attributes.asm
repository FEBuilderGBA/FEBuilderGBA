.thumb
.include "_Text_Engine_Defs.asm"

push	{r4-r6,r14}
ldr		r5,=gpDialogueState
ldr		r5,[r5]

@Update CurrenSpeakerAttributeList with the new ActiveFace's attributes if they're different (and call relevant functions as necessary)
ldr		r3,=GetCurrentSpeakerAttributesList
mov		r14,r3
.short	0xF800
mov		r6,r0
ldrb	r0,[r5,#0x11]
ldr		r3,=GetFaceProcByPosition
mov		r14,r3
.short	0xF800
cmp		r0,#0
beq		Label17
mov		r4,r0
add		r4,#TextAttributesOffsetInFaceProc
@r6=old, r4=new, update r6 with r4 if different
	@ldrb	r0,[r4,#FontIdOffset]
	@ldrb	r1,[r6,#FontIdOffset]
	@cmp		r0,r1
	@beq		Label20
	@strb	r0,[r6,#FontIdOffset]
	@ldr		r3,=UpdateFontGlyphSet
	@mov		r14,r3
	@.short	0xF800
	@Label20:
ldrb	r0,[r4,#TextColorGroupIdOffset]
ldrb	r1,[r6,#TextColorGroupIdOffset]
cmp		r0,r1
beq		Label21
strb	r0,[r6,#TextColorGroupIdOffset]
ldr		r3,=ChangeTextColorID
mov		r14,r3
.short	0xF800
Label21:
ldrb	r0,[r4,#TextBoxBgPaletteIdOffset]
ldrb	r1,[r6,#TextBoxBgPaletteIdOffset]
cmp		r0,r1
beq		Label22
strb	r0,[r6,#TextBoxBgPaletteIdOffset]
ldr		r3,=UpdateTextBoxBgPalette
mov		r14,r3
.short	0xF800
Label22:
@text box and text pitch don't directly change anything here, so we just update the values
ldrb	r0,[r4,#TextBoxTypeIdOffset]
strb	r0,[r6,#TextBoxTypeIdOffset]
ldrb	r0,[r4,#TextBoopPitchIdOffset]
strb	r0,[r6,#TextBoopPitchIdOffset]
Label17:

pop		{r4-r6}
pop		{r0}
bx		r0

.ltorg
