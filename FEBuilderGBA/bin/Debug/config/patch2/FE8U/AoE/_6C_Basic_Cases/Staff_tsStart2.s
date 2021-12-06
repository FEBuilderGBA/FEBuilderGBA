.thumb
.include "../_TargetSelectionDefinitions.s"
@target select but with box that lists all the items the target has
push {r4-r6,lr}
mov 	r4, r0
mov 	r5, r1
mov 	r6, r2
@hide and redisplay range squares just in case some squares in range aren't selectable
_blh HideRangeSquares
_blh Font_ResetAllocation @clear space just in case
mov r0, r6
_blh ShowRangeSquares

mov 	r0, r4
_blh 	0x8034C18	@related to drawing item list box?	@{U}
@_blh 	0x8034B20	@related to drawing item list box?	@{J}

@setup targeting here
HelpText:
@Target Help Text Display
mov 	r0, r5
@ldr 	r0, TargetTextID
cmp 	r0, #0x0
beq 	NoHelpText
ldr 	r3, =0x800A240 @GetInTextBuffer	@{U}
@ldr 	r3, =0x8009FA8 @GetInTextBuffer	@{J}
mov 	r14, r3
.short 0xF800
mov 	r1, r0
mov 	r0, r4
ldr 	r3, =0x8035708 @display help text at bottom of screen	@{U}
@ldr 	r3, =0x8035610 @display help text at bottom of screen	@{J}
mov 	r14, r3
.short 0xF800
NoHelpText:

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

End:
pop 	{r4-r6}
pop 	{r3}
bx	r3


.align
.ltorg
OffsetList:
