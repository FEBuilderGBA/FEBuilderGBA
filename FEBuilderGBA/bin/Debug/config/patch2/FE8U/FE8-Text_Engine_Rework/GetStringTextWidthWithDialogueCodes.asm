.thumb
.include "_Text_Engine_Defs.asm"

@inserted inline at 8B44

@r0=pointer to text buffer, r1=bool (1 if gDialogueState+0xF = +0x11 and +0xE = +0x10)

push	{r4-r7,r14}
mov		r7,r9
mov		r6,r8
push	{r6-r7}
add		sp,#-0x28			@sp=current font, sp+4=place to write glyph width to, sp+8=temporary text buffer
mov		r4,r0
mov		r8,r1
ldr		r0,=gpDialogueState
ldr		r0,[r0]
ldrb	r5,[r0,#0x11]		@active position
mov		r1,#0xF
ldsb	r1,[r0,r1]
mov		r9,r1				@no idea why this is loaded as signed
mov		r6,#0				@current line length
mov		r7,#0x18			@maximum length so far
ldr		r0,=gpCurrentFont
ldr		r0,[r0]				@current font struct
ldr		r0,[r0,#4]			@current font glyph pointer
str		r0,[sp]

MainLoop:
ldrb	r0,[r4]
cmp		r0,#0x80
beq		ExtraTextCodes
cmp		r0,#0x1D			@0x1E-0x7F all return 1
bls		Label1
b		NormalText
Label1:
lsl		r1,r0,#1
add		r1,r15
mov		r15,r1

b		EndAndReturnWidth	@0, terminator
b		StartNewLine		@1, newline
b		StartNewLine		@2, 2 newlines
b		Add12				@3, [A]
b		Advance1			@4
b		Advance1
b		Advance1
b		Advance1
b		SetNewActivePos1	@8, [OpenFarLeft]
b		SetNewActivePos1
b		SetNewActivePos1
b		SetNewActivePos1
b		SetNewActivePos1
b		SetNewActivePos1
b		SetNewActivePos1
b		SetNewActivePos1
b		Advance3			@10, [LoadFace]
b		EndIfClearingActiveFace	@11, [ClearFace]
b		EndIfBoolIsFalse	@12
b		EndIfBoolIsFalse
b		EndIfBoolIsFalse
b		EndAndReturnWidth	@15
b		Advance1
b		Advance1
b		Add80				@18, [Yes]
b		Add80
b		Add80
b		Add80				@1B, [Sell]
b		Advance1			@1C, [SendToBack]
b		Advance1

ExtraTextCodes:
add		r4,#1
ldrb	r0,[r4]
cmp		r0,#MaxExtraTextCode
bls		Label2
b		MainLoop
Label2:
lsl		r1,r0,#1
add		r1,r15
mov		r15,r1

b		Advance1			@80 00 would terminate anyway
b		Advance1
b		Advance1
b		Advance1
b		Advance1			@80 04, [LoadOverworldFaces]
b		GoldRecurse			@80 05
b		TextRecurse			@80 06
b		Advance1
b		Advance1
b		Advance1
b		SetNewActivePos2	@80 0A, [MoveFarLeft]
b		SetNewActivePos2
b		SetNewActivePos2
b		SetNewActivePos2
b		SetNewActivePos2
b		SetNewActivePos2
b		SetNewActivePos2
b		SetNewActivePos2
b		MainLoop			@80 12, just get interpreted as normal 12?
b		MainLoop
b		MainLoop
b		MainLoop
b		Advance1			@80 16
b		Advance1
b		Advance1
b		Advance1
b		Advance1
b		Advance1
b		Advance1
b		Advance1
b		Advance1
b		Advance1
b		TactName			@80 20
b		Advance1
b		MainLoop			@80 22 [Item]
b		MainLoop			@80 23 [SetName]
b		Advance1			@80 24 [ToggleRed]
b		Advance1			@80 25 [ToggleColorInvert]
b		ChangeFont			@80 26 [SetFont] (new)
b		Advance3			@80 27 [SetTextPalette]
b		Advance2			@80 28 [SetTextGroup]
b		Advance2			@80 29 [SetTextBoxPalette]
b		Advance2			@80 2A [SetTextBoxType]
b		Advance2			@80 2B [SetTextBoxHeight]
b		Advance2			@80 2C [SetTextBoopPitch]
b		Advance5			@80 2D [PlaySound]
b		Advance3			@80 2E [SetPortraitPosition]
b		Advance9			@80 2F [LoadFaceFancy]
b		SetNewActivePos3	@80 30 [MoveFarLeftWithVariableSpeed]
b		SetNewActivePos3	
b		SetNewActivePos3	
b		SetNewActivePos3	
b		SetNewActivePos3	
b		SetNewActivePos3	
b		SetNewActivePos3	
b		SetNewActivePos3	
b		Advance2			@80 38 [SetTextSpeed]

.ltorg

NormalText:
cmp		r5,r9
beq		GetWidthOfGlyph
cmp		r5,#0xFF
beq		GetWidthOfGlyph
mov		r1,r8
cmp		r1,#0
bne		EndAndReturnWidth
mov		r2,#1
mov		r8,r2
mov		r9,r5
GetWidthOfGlyph:
mov		r0,r4
add		r1,sp,#4
bl		GetCharTextWidth
mov		r4,r0
ldr		r0,[sp,#4]
add		r6,r0
b		MainLoop

Advance1:
add		r4,#1
b		MainLoop
Advance2:
add		r4,#2
b		MainLoop
Advance3:
add		r4,#3
b		MainLoop
Advance5:
add		r4,#5
b		MainLoop
Advance9:
add		r4,#5
b		MainLoop

StartNewLine:				@[NL] and [2NL]
cmp		r6,r7
ble		Label3
mov		r7,r6
Label3:
mov		r6,#0
b		Advance1

Add12:						@[A]
add		r6,#12
b		Advance1
Add80:
add		r6,#80				@[Yes], [No], [Buy], [Sell]
b		Advance1

SetNewActivePos1:			@[OpenFarLeft], etc
ldrb	r5,[r4]
sub		r5,#8
b		Advance1
SetNewActivePos2:			@[MoveFarLeft], etc
ldrb	r5,[r4]
sub		r5,#0xA
b		Advance1
SetNewActivePos3:			@[MoveFarLeftWithVariableSpeed], etc
ldrb	r5,[r4]
sub		r5,#0x30
b		Advance2

EndIfClearingActiveFace:	@[ClearFace]
cmp		r5,r9
beq		EndAndReturnWidth
b		Advance1

EndIfBoolIsFalse:			@12 - 14
mov		r2,r8
cmp		r2,#0
beq		EndAndReturnWidth
b		Advance1

GoldRecurse:
ldr		r0,=gpDialogueState
ldr		r0,[r0]
ldr		r0,[r0,#0x3C]		@number
add		r1,sp,#8
bl		String_FromNumber
add		r0,sp,#8
mov		r1,r8
lsl		r1,#0x18
asr		r1,#0x18
bl		GetStringTextWidthWithDialogueCodes
add		r6,r0
b		Advance1

TextRecurse:
ldr		r0,=gpDialogueState
ldr		r0,[r0]
add		r0,#0x60
mov		r1,r8
lsl		r1,#0x18
asr		r1,#0x18
bl		GetStringTextWidthWithDialogueCodes
add		r6,r0
b		Advance1
	
TactName:
bl		GetTacticianNameString
bl		GetStringTextWidth
add		r6,r0
b		Advance1

ChangeFont:
ldrb	r0,[r4,#1]
sub		r0,#1					@font argument
bl		UpdateFontGlyphSet
b		Advance2

EndAndReturnWidth:
cmp		r6,r7
ble		GoBack
mov		r7,r6
GoBack:
ldr		r1,[sp]				@original font glyph pointer
ldr		r0,=gpCurrentFont
ldr		r0,[r0]				@current font struct
str		r1,[r0,#4]			@current font glyph pointer
mov		r0,r7
add		sp,#0x28
pop		{r6-r7}
mov		r8,r6
mov		r9,r7
pop		{r4-r7}
pop		{r1}
bx		r1

.ltorg
