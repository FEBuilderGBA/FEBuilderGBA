@build target list
@clear movement cost map
@create target selection 6c

.thumb
.include "../_TargetSelectionDefinitions.s"

@.equ RangeBuilder, OffsetList
@.equ TargetTextID, OffsetList + 0x4
@parameters
	@r0 = Unit Pointer
	@r1 = tsPointer List
	@r2 = item id
	@r3 = range builder

push {r4,lr}
mov 	r4, r1
@ldr 	r3, RangeBuilder
_blr 	r3
ldr 	r0, =ppMoveMapRows
ldr 	r0, [r0]
mov 	r1, #0x1
neg 	r1, r1
_blh 	Map_Fill
mov 	r0, r4
_blh 	TargetSelection_New
mov 	r4, r0

@HelpText:
@Target Help Text Display
@ldr 	r0, TargetTextID
@cmp 	r0, #0x0
@beq 	NoHelpText
@ldr 	r3, =0x800A240 @GetInTextBuffer
@mov 	r14, r3
@.short 0xF800
@mov 	r1, r0
@mov 	r0, r4
@ldr 	r3, =BottomHelpDisplay_New @display help text at bottom of screen
@mov 	r14, r3
@.short 0xF800
@NoHelpText:

SoundCheck:
@check if sounds are turned off?
ldr 	r0, =0x202BCF0 	@chapter data in ram	@{U}
@ldr 	r0, =0x202BCEC 	@chapter data in ram	@{J}
add 	r0, #0x41
ldrb	r0, [r0]
lsl 	r0, r0, #0x1E
cmp 	r0, #0x0
blt 	Muted
@play sound effect?
mov 	r0, #0x6A
ldr 	r3, =PlaySoundEffect
mov 	r14, r3
.short 0xF800
Muted:
pop 	{r4}
pop 	{r3}
bx	r3

.align
.ltorg
OffsetList:
