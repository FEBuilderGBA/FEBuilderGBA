.thumb
.include "_Text_Engine_Defs.asm"

@jumped to from 6C28, the end of DialogueMain_OnInit

ldr		r3,=StartProc
mov		r14,r3
.short	0xF800
ldr		r0,=gpDialoguePositionTable
ldr		r0,[r0]
adr		r1,NormalFaceXPosTable
mov		r2,#0
Loop1:
ldrb	r3,[r1,r2]
strb	r3,[r0,r2]
add		r2,#1
cmp		r2,#8
blt		Loop1
@just to make sure there's no issues, put default values at CurrentSpeakerAttributesList
ldr		r3,=GetCurrentSpeakerAttributesList
mov		r14,r3
.short	0xF800
mov		r1,#0
strb	r1,[r0,#FontIdOffset]
strb	r1,[r0,#TextBoxBgPaletteIdOffset]
strb	r1,[r0,#TextBoxTypeIdOffset]
mov		r1,#1
strb	r1,[r0,#TextColorGroupIdOffset]
mov		r1,#13
strb	r1,[r0,#TextBoopPitchIdOffset]
pop		{r0}
bx		r0

.align
NormalFaceXPosTable:
.byte 0x03, 0x06, 0x09, 0x15, 0x18, 0x1B, 0xF8, 0x26
.ltorg
