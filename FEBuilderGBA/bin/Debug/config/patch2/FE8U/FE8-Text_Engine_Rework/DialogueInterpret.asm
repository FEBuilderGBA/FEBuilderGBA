.thumb
.include "_Text_Engine_Defs.asm"

@inserted inline at 6FD0

@r0 = dialogue proc

.global TerminateString
.global NewLine
.global TwoNewLines
.global WaitForAPress
.global PauseText
.global SetPositionActive
.global LoadFaceNormal
.global ClearFace
.global MakeNewTextBox
.global ClearTextBox
.global ToggleMouthMove
.global ToggleSmile
.global ConvoChoiceYes
.global ConvoChoiceNo
.global ConvoChoiceBuy
.global ConvoChoiceSell
.global SendFaceToBack
.global Case_0x1D

.global VanillaChangeColor
.global PauseDialogueExecution
.global PrintMonetaryAmount
.global SwitchToMiniTextBuffer
.global RetThree
.global RetZero
.global MoveFaceToPosition
.global DefaultBlinking
.global MediumPauseBetweenBlinks
.global LongPauseBetweenBlinks
.global ShortPauseBetweenBlinks
.global BlinkNow
.global StopBlinking
.global OpenEyes
.global CloseEyes
.global HalfCloseEyes
.global WinkEyes
.global TacticianName
.global ToggleRed
.global ExecuteSomeRoutine
.global ToggleColorInvert

.global ChangeFont
.global ChangeTextPalette
.global ChangeTextColorGroup
.global ChangeTextBoxBgPalette
.global ChangeTextBoxType
.global ChangeTextBoxHeight
.global ChangeTextBoopPitch
.global PlaySound
.global ChangePortraitPosition
.global LoadFaceFancy
.global MoveFaceToPositionVariableSpeed
.global ChangeTextSpeed

.global GetCurrentSpeakerAttributesList
.global UpdateFontGlyphSet
.global GetFaceProcByPosition
.global UpdateTextBoxBgPalette 

push	{r4-r7,r14}
add		sp,#-8
mov		r7,r0
ldr		r5,=gpDialogueState
ldr		r5,[r5]			@r5=gDialogueState
MainLoop:
ldr		r4,[r5]			@pointer to place in current text buffer
ldrb	r6,[r4]
cmp		r6,#0
beq		Label2389238
cmp		r6,#0x80		@extra text codes
beq		ExtraTextCodes
cmp		r6,#0x1D		@0x1E-0x7F all return 1
bls		Label1
b		RetOne
Label1:
add		r4,#1
str		r4,[r5]
Label2389238:
ldr		r1,=HandleNormalTextCodesTable	@defined by EA
lsl		r0,r6,#2
add		r0,r1
ldr		r0,[r0]
mov		r15,r0

ExtraTextCodes:			@80 XX
ldrb	r6,[r4,#1]
add		r4,#2
str		r4,[r5]
cmp		r6,#MaxExtraTextCode
bls		Label2
b		RetOne
Label2:
ldr		r1,=HandleExtendedTextCodesTable	@defined by EA
lsl		r0,r6,#2
add		r0,r1
ldr		r0,[r0]
mov		r15,r0

.ltorg

TerminateString:		@00
ldr		r0,[r5,#4]		@alternate text buffer
cmp		r0,#0
bne		Label10
b		RetZero
Label10:
add		r0,#2			@not sure why we add 2
str		r0,[r5]
mov		r0,#0
str		r0,[r5,#4]
b		MainLoop

NewLine:				@01
ldrb	r0,[r5,#0x15]	@is line ready to draw
cmp		r0,#1
beq		Label3
ldrb	r0,[r5,#0x9]	@line number
cmp		r0,#1
bne		Label4
Label3:
ldrb	r0,[r5,#0x9]
add		r0,#1
strb	r0,[r5,#0x9]
Label4:
mov		r0,#0
strb	r0,[r5,#0x15]
b		RetTwo

TwoNewLines:			@02
mov		r0,#0x80		@80 is set for world map stuff
bl		CheckDialogueFlag
cmp		r0,#0
beq		label5
bl 		ResetNarrationLines
add		r4,#1
str		r4,[r5]			@no idea why we skip an extra text code
b		RetThree
label5:
mov		r0,#1			@1 disables scrolling
bl		CheckDialogueFlag
cmp		r0,#0
bne		Label6
ldr		r0,=gProc_TextScrollInBoxBy2	@591470
mov		r1,r7
bl		StartBlockingProc
b		RetThree
Label6:
bl 		ResetDialogueLines
b		RetThree

WaitForAPress:			@03
bl		GetCurrentLineTextStruct
bl		Text_GetXCursor
ldrb	r1,[r5,#0xC]	@x coord of box
lsl		r1,#3
add		r1,r0
add		r1,#4
ldrb	r2,[r5,#0xD]	@y coord of box
lsl		r2,#3
ldrb	r0,[r5,#0x9]
lsl		r0,#4
add		r2,r0
add		r2,#8
mov		r0,r7
bl		NewTextBluArrowAndButtonChecker
b		RetThree

PauseText:				@04 to 07
ldrb	r0,[r5,#0x12]	@1 if fast-forwarding through text
cmp		r0,#0
beq		Label7
b		RetTwo
Label7:
ldr		r0,=gProc_DialoguePauseTimer
mov		r1,r7
bl		StartBlockingProc
mov		r1,r6
mov		r6,r0
mov		r0,r1
bl		GetTextPauseDurationFromControlCode
add		r6,#0x64
strh	r0,[r6]
b		RetThree

SetPositionActive:		@08 to 0F, aka [OpenMidLeft] and that sort
ldrb	r0,[r5,#0x11]
bl		UnsetFaceDisplayBits38
mov		r0,r6
sub		r0,#8
bl		Dialogue_SetActiveFacePosition
b		RetThree

LoadFaceNormal:			@10
@This has checks afterward for SetPosition, but I'm going to ignore those because it only saves 1 frame
mov		r0,r7
mov		r1,#0xFF		@argument indicating no extra checks to make
bl		LoadFace
@Set default attributes
ldrb	r0,[r5,#0x11]
bl		GetFaceProcByPosition
mov		r6,r0
add		r6,#TextAttributesOffsetInFaceProc
@bl		GetCurrentSpeakerAttributesList
mov		r1,#0
@strb	r1,[r0,#FontIdOffset]
strb	r1,[r6,#FontIdOffset]
@strb	r1,[r0,#TextBoxBgPaletteIdOffset]
strb	r1,[r6,#TextBoxBgPaletteIdOffset]
@strb	r1,[r0,#TextBoxTypeIdOffset]
strb	r1,[r6,#TextBoxTypeIdOffset]
mov		r1,#1
@strb	r1,[r0,#TextColorGroupIdOffset]
strb	r1,[r6,#TextColorGroupIdOffset]
mov		r1,#12
@strb	r1,[r0,#TextBoopPitchIdOffset]
strb	r1,[r6,#TextBoopPitchIdOffset]
add		r4,#2
str		r4,[r5]
b		RetThree

ClearFace:				@11
bl		ShouldKeepTextBox
cmp		r0,#0
beq		Label8
bl		ClearActiveTextBubble
Label8:
ldrb	r0,[r5,#0x11]	@active position
bl		GetFaceProcByPosition
bl		StartFaceFade
mov		r0,r5
add		r0,#0x18
ldrb	r1,[r5,#0x11]
lsl		r1,#2
add		r0,r1
mov		r1,#0
str		r1,[r0]
mov		r0,r7
mov		r1,#0x10
bl		StartBlockingTimer
b		RetThree

MakeNewTextBox:			@12 - 14
bl		ShouldKeepTextBox
mov		r1,r0
mov		r0,r4
bl		GetStringTextWidthWithDialogueCodes
add		r0,#7
mov		r1,#8
bl		Div
add		r0,#2
strb	r0,[r5,#0xE]	@text box with
b		MainLoop

ClearTextBox:			@15
bl		ClearActiveTextBubble
b		RetThree

ToggleMouthMove:		@16
ldrb	r0,[r5,#0x16]	@something about mouth moving
mov		r1,#1
sub		r0,r1,r0
strb	r0,[r5,#0x16]
b		RetThree

ToggleSmile:			@17
ldrb	r0,[r5,#0x17]	@something about mouth smiling
mov		r1,#1
sub		r0,r1,r0
strb	r0,[r5,#0x17]
b		RetThree

ConvoChoiceYes:			@18
ldr		r0,=gTextChoiceDef_YesNo
mov		r1,#1
b		DrawTextChoice
ConvoChoiceNo:			@19
ldr		r0,=gTextChoiceDef_YesNo
mov		r1,#2
b		DrawTextChoice
ConvoChoiceBuy:			@1A
ldr		r0,=gTextChoiceDef_Shop
mov		r1,#1
b		DrawTextChoice
ConvoChoiceSell:		@1B
ldr		r0,=gTextChoiceDef_Shop
mov		r1,#2
DrawTextChoice:
str		r0,[sp]
str		r1,[sp,#4]
bl		GetCurrentLineTextStruct
mov		r1,r0
ldrb	r0,[r5,#0x9]	@current line number
lsl		r0,#1
ldrb	r2,[r5,#0xD]	@y coordinate of box
add		r0,r2
lsl		r0,#5
ldrb	r2,[r5,#0xC]	@x coordinate of box
add		r0,r2
lsl		r0,#1
ldr		r2,=gBg0MapBuffer
add		r2,r0
ldrb	r3,[r5,#8]		@text color
ldr		r0,[sp]
str		r3,[sp]
ldr		r3,[sp,#4]
str		r7,[sp,#4]
bl		StartDialogueTextChoice
b		RetThree

SendFaceToBack:			@1C
mov		r0,#0x10
bl		SetDialogueFlag
b		RetThree

Case_0x1D:				@1D (not sure what this does, exactly. It's called FastPrint in FEditor's text control code doc, but it seems to undo 1C, so idk
mov		r0,#0x10
bl		UnsetDialogueFlag
b		RetThree

.ltorg

RetZero:
mov		r0,#0
b		ReturnValue
RetOne:
mov		r0,#1
b		ReturnValue
RetTwo:
mov		r0,#2
b		ReturnValue
RetThree: 
mov		r0,#3
b		ReturnValue

@###################### 80 XX CODE START HERE ###########################

VanillaChangeColor:		@80 00-03 (00 isn't technically valid)
add		r0,r6,#1		@go from 2 to 4
b		ToggleColorsVanilla	@in ToggleRed

PauseDialogueExecution:	@80 04, commonly known as [LoadOverworldFaces]
mov		r0,r7
bl		BlockProc
b		RetThree

PrintMonetaryAmount:	@80 05, aka [G]
ldr		r0,[r5,#0x3C]	@number buffer
mov		r1,r5
add		r1,#0x40		@number to string buffer
bl		String_FromNumber
sub		r4,#2			@I guess this is why we added 2 in the 00 text code when switching text buffers
str		r4,[r5,#4]
mov		r0,r5
add		r0,#0x40
str		r0,[r5]
b		MainLoop

SwitchToMiniTextBuffer:	@80 06, not used afaik, but could be used to print small things (buffer is only 0x20 bytes long)
sub		r4,#2
str		r4,[r5,#4]
mov		r0,r5
add		r0,#0x60
str		r0,[r5]
b		MainLoop

@80 07 and 08 just return 3

@80 09 returns 0

MoveFaceToPosition:		@80 0A to 11
ldrb	r0,[r5,#0x11]	@current face position
mov		r1,r6
sub		r1,#0xA
mov		r2,#0
bl		CallMoveFaceAndWriteSpeed
b		RetThree

@80 12 to 15 return 0

DefaultBlinking:			@80 16
mov		r6,#0
b		CallSetFaceBlinkControl
MediumPauseBetweenBlinks:	@80 17
mov		r6,#1
b		CallSetFaceBlinkControl
LongPauseBetweenBlinks:		@80 18
mov		r6,#3
b		CallSetFaceBlinkControl
ShortPauseBetweenBlinks:	@80 19
mov		r6,#2
b		CallSetFaceBlinkControl
BlinkNow:					@80 1A
mov		r6,#4
b		CallSetFaceBlinkControl
StopBlinking:				@80 1B
mov		r6,#5
CallSetFaceBlinkControl:
ldrb	r0,[r5,#0x11]
bl		GetFaceProcByPosition
mov		r1,r6
bl		SetFaceBlinkControl
b		RetThree

OpenEyes:					@80 1C
mov		r6,#0
b		CallSetFaceEyeStuff
CloseEyes:					@80 1D
mov		r6,#2
b		CallSetFaceEyeStuff
HalfCloseEyes:				@80 1E
mov		r6,#3
b		CallSetFaceEyeStuff
WinkEyes:					@80 1F
mov		r6,#4
CallSetFaceEyeStuff:
ldrb	r0,[r5,#0x11]
bl		GetFaceProcByPosition
mov		r1,r6
bl		SetFaceEyeControl
b		RetThree

TacticianName:				@80 20
sub		r4,#2
str		r4,[r5,#4]
bl		GetTacticianNameString
str		r0,[r5]
b		MainLoop

ToggleRed:					@80 21
mov		r0,#4
ToggleColorsVanilla:
@r0=text color index. If it matches the current value (r5+8), then set r0=1 (default); else, leave it alone
ldrb	r1,[r5,#8]			@current color
cmp		r0,r1
bne		CallChangeTextColor
mov		r0,#1
CallChangeTextColor:		@the new text index changing code branches here
bl		ChangeTextColorID	@TODO: Update this
b		MainLoop

@80 22 and 80 23 ([Item] and [SetName], respectively) return 0

ExecuteSomeRoutine:			@80 24; probably not used
ldr		r1,[r5,#0x38]
cmp		r1,#0
beq		Label9
mov		r0,r7
bl		BXR1
Label9:
b		RetThree

ToggleColorInvert:			@80 25
mov		r2,r5
add		r2,#0x83
ldrb	r0,[r2]
mov		r1,#1
and		r0,r1
mov		r1,#3
sub		r0,r1,r0
strb	r0,[r2]
b		RetThree

@###################################### NEW TEXT CODES HERE #######################################

ChangeFont:					@80 26 XX; XX goes from 1-255
ldrb	r6,[r4]				@font argument
sub		r6,#1
ldrb	r0,[r5,#0x11]
mov		r1,r6
mov		r2,#FontIdOffset
bl		SetAttrToCurrentSpeakerAndFaceProc
mov		r0,r6
bl		UpdateFontGlyphSet
add		r4,#1
str		r4,[r5]
b		MainLoop

ChangeTextPalette:			@80 27 XX YY; XX goes from 1-5 and is the "text group" to change, YY is index of a set of 3 colors
ldrb	r1,[r4]				@XX
sub		r1,#1
mov		r2,#6				@colors are 2 bytes, each group is 3 colors
mul		r1,r2
add		r1,#2				@first color in palette bank is background
ldr		r0,=gpCurrentFont
ldr		r0,[r0]
ldrh	r0,[r0,#0x14]		@palette index of the current font struct
lsl		r0,#5
add		r1,r0
ldrb	r0,[r4,#1]
sub		r0,#1
mul		r0,r2
ldr		r2,=TextPaletteTable	@defined in EA
add		r0,r2
mov		r2,#6				@length of palette to copy
bl		CopyToPaletteBuffer
add		r4,#2
str		r4,[r5]
b		RetThree			@so the palette can update

ChangeTextColorGroup:		@80 28 XX; XX goes from 1-5 (basically the same as ToggleRed, except it doesn't toggle)
ldrb	r6,[r4]
sub		r6,#1
ldrb	r0,[r5,#0x11]
mov		r1,r6
mov		r2,#TextColorGroupIdOffset
bl		SetAttrToCurrentSpeakerAndFaceProc
mov		r0,r6
bl		ChangeTextColorID
add		r4,#1
str		r4,[r5]
b		MainLoop

ChangeTextBoxBgPalette:		@80 29 XX; XX goes from 1-255
ldrb	r6,[r4]
sub		r6,#1
ldrb	r0,[r5,#0x11]
mov		r1,r6
mov		r2,#TextBoxBgPaletteIdOffset
bl		SetAttrToCurrentSpeakerAndFaceProc
mov		r0,r6
bl		UpdateTextBoxBgPalette
add		r4,#1
str		r4,[r5]
b		RetThree

ChangeTextBoxType:			@80 2A XX; XX goes from 1-127, 0x80 bit set to not have a tail
ldrb	r0,[r5,#0x11]
ldrb	r1,[r4]
sub		r1,#1
mov		r2,#TextBoxTypeIdOffset
bl		SetAttrToCurrentSpeakerAndFaceProc
add		r4,#1
str		r4,[r5]
b		MainLoop

ChangeTextBoxHeight:		@80 2B XX; XX goes from 1-3 and is the number of lines in a box
ldrb	r0,[r4]
strb	r0,[r5,#0xA]
add		r4,#1
str		r4,[r5]
b		MainLoop

ChangeTextBoopPitch:		@80 2C XX; XX goes from 1 to 25, with 13 being the default pitch
ldrb	r1,[r4]
sub		r1,#1
ldrb	r0,[r5,#0x11]
mov		r2,#TextBoopPitchIdOffset
bl		SetAttrToCurrentSpeakerAndFaceProc
add		r4,#1
str		r4,[r5]
b		MainLoop

PlaySound:					@80 2D 8D 8C 8B 8A; encodes sound id 0xABCD
mov		r2,#0xF
ldrb	r0,[r4]
and		r0,r2
ldrb	r1,[r4,#1]
and		r1,r2
lsl		r1,#4
orr		r0,r1
ldrb	r1,[r4,#2]
and		r1,r2
lsl		r1,#8
orr		r0,r1
ldrb	r1,[r4,#3]
and		r1,r2
lsl		r1,#12
orr		r0,r1
bl		m4aSongNumStart
add		r4,#4
str		r4,[r5]
b		RetThree

ChangePortraitPosition:		@80 2E XX YY; XX goes from 1-8 and is the position index, while YY is the (signed) coordinate in tiles. Default values are 03, 06, 09, 15, 18, 1B, -8, 26
ldr		r0,=gpDialoguePositionTable
ldr		r0,[r0]
ldrb	r1,[r4]
sub		r1,#1				@position
ldrb	r2,[r4,#1]
cmp		r2,#0x80
bne		Label13				@putting 00 would be interpreted as a terminator, so this is a workaround, even though I do not believe anyone actually would need this particular value
mov		r2,#0
Label13:
strb	r2,[r0,r1]
add		r4,#2
str		r4,[r5]
b 		MainLoop

LoadFaceFancy:				@80 2F XX XX AA BB CC DD EE FF; XX XX is the portrait id, in the same manner as normal LoadFace
mov		r0,r7
ldrb	r6,[r4,#2]			@AA; if bit 0x1 is set, face right (flip), otherwise face left, (both independent of position on screen)
mov		r1,#0x7F
and		r6,r1
mov		r1,#1
and		r1,r6
bl		LoadFace
mov		r0,#2
tst		r0,r6				@if 0x2 is set, close eyes (reason for this is that if you do it normally (LoadFace followed by CloseEyes) the face fades in with eyes open, and that looks weird
beq		Label14
ldrb	r0,[r5,#0x11]
bl		GetFaceProcByPosition
mov		r1,#2
bl		SetFaceEyeControl
Label14:
ldrb	r0,[r5,#0x11]
bl		GetFaceProcByPosition
mov		r6,r0
add		r6,#TextAttributesOffsetInFaceProc
@bl		GetCurrentSpeakerAttributesList
ldrb	r1,[r4,#3]			@BB; font
sub		r1,#1
@strb	r1,[r0,#FontIdOffset]
strb	r1,[r6,#FontIdOffset]
ldrb	r1,[r4,#4]			@CC; text color group
sub		r1,#1
@strb	r1,[r0,#TextColorGroupIdOffset]
strb	r1,[r6,#TextColorGroupIdOffset]
ldrb	r1,[r4,#5]			@BB; text bubble bg palette
sub		r1,#1
@strb	r1,[r0,#TextBoxBgPaletteIdOffset]
strb	r1,[r6,#TextBoxBgPaletteIdOffset]
ldrb	r1,[r4,#6]			@BB; text box type
sub		r1,#1
@strb	r1,[r0,#TextBoxTypeIdOffset]
strb	r1,[r6,#TextBoxTypeIdOffset]
ldrb	r1,[r4,#7]			@BB; text pitch boop
sub		r1,#1
@strb	r1,[r0,#TextBoopPitchIdOffset]
strb	r1,[r6,#TextBoopPitchIdOffset]
add		r4,#8
str		r4,[r5]
b		RetThree

MoveFaceToPositionVariableSpeed:	@80 30-37 XX, same as MoveMidLeft and their ilk. XX is the number of frames to get from A to B. 0x10 is default for distances <12 tiles, otherwise 0x20
ldrb	r0,[r5,#0x11]	@current face position
mov		r1,r6
sub		r1,#0x30
ldrb	r2,[r4]
bl		CallMoveFaceAndWriteSpeed
add		r4,#1
str		r4,[r5]
b		RetThree

ChangeTextSpeed:			@80 38 XX; XX = number of frames between letters. 0xFF = use default setting
ldrb	r0,[r4]
cmp		r0,#0xFF
bne		Label16
bl		GetTextSpeed
Label16:
strb	r0,[r5,#0x13]		@text speed
add		r4,#1
str		r4,[r5]
b		RetThree

ReturnValue:
add		sp,#8
pop		{r4-r7}
pop		{r1}
bx		r1
.ltorg

@#################### OTHER FUNCTIONS ############################

GetCurrentLineTextStruct:
push	{r14}
ldr		r2,=gpDialogueState
ldr		r2,[r2]
ldrb	r0,[r2,#0x9]	@current line number
ldrb	r1,[r2,#0xB]	@number of lines scrolled
add		r0,r1
ldrb	r1,[r2,#0xA]	@max number of lines in box
bl		_modsi3
lsl		r0,#3
ldr		r1,=gDialogueText
add		r0,r1
pop		{r1}
bx		r1
.ltorg

GetFaceProcByPosition:
@r0=position
ldr		r1,=gpDialogueState
ldr		r1,[r1]
add		r1,#0x18
lsl		r0,#2
add		r0,r1
ldr		r0,[r0]
bx		r14
.ltorg

GetCurrentSpeakerAttributesList:
ldr		r0,=gpDialogueState
ldr		r0,[r0]
add		r0,#0x58		@using this because the vanilla NumToString doesn't do numbers over 5 digits, so the text buffer for this (at +40) shouldn't ever get to this point
bx		r14
.ltorg
@Note to self: Table for GetDialogueFaceSlotXTile will be at +50 and is 8 bytes long

SetAttrToCurrentSpeakerAndFaceProc:
@r0=current face position (0x11), r1=value, r2=offset of attribute
push	{r4-r5,r14}
mov		r4,r1
mov		r5,r2
cmp		r0,#0xFF		@no current position
beq		Label12
bl		GetFaceProcByPosition
cmp		r0,#0
beq		Label12			@no face at current position
add		r0,#TextAttributesOffsetInFaceProc
strb	r4,[r0,r5]
Label12:
bl		GetCurrentSpeakerAttributesList
strb	r4,[r0,r5]
pop		{r4-r5}
pop		{r0}
bx		r0
.ltorg

UpdateFontGlyphSet:
@r0=index for table of font glyphs
ldr		r1,=FontGlyphsPointerTable		@defined in EA
lsl		r0,#2
add		r0,r1
ldr		r0,[r0]
ldr		r1,=gpCurrentFont
ldr		r1,[r1]
str		r0,[r1,#4]
bx		r14
.ltorg

CallMoveFaceAndWriteSpeed:
@r0=original position id, r1=new position id, r2=speed (0 if game should figure it out)
push	{r4-r6,r14}
mov		r4,r0
mov		r5,r1
mov		r6,r2
@rewriting 79E4 (MoveFaceFromr0tor1) inline here
mov		r7,#0
mov		r0,r5			@new position
bl		GetFaceProcByPosition
cmp		r0,#0
beq		Label26
mov		r7,#1
mov		r0,r5
mov		r1,r4
mov		r2,#1
bl		CreateMovingFaceProc	@editing this function to return the new proc
mov		r1,#0
str		r1,[r0,#0x58]	@start frame count
str		r6,[r0,#0x5C]	@end frame count
Label26:
mov		r0,r4
mov		r1,r5
mov		r2,r7
bl		CreateMovingFaceProc
mov		r1,#0
str		r1,[r0,#0x58]	@start frame count
str		r6,[r0,#0x5C]	@end frame count
@switch the face proc positions
ldr		r0,=gpDialogueState
ldr		r0,[r0]
add		r0,#0x18
lsl		r1,r4,#2
add		r1,r0
lsl		r2,r5,#2
add		r0,r2
ldr		r2,[r0]
ldr		r3,[r1]
str		r2,[r1]
str		r3,[r0]
mov		r0,r5
bl		Dialogue_SetActiveFacePosition
pop		{r4-r6}
pop		{r0}
bx		r0
.ltorg

UpdateTextBoxBgPalette:
@r0=index for table of palettes
push	{r14}
lsl		r0,#5
ldr		r1,=TextBoxBgPaletteTable	@defined in EA
add		r0,r1
mov		r1,#0x60			@palette bank
mov		r2,#0x20			@length
bl		CopyToPaletteBuffer
pop		{r0}
bx		r0
.ltorg
