.thumb
.include "_Text_Engine_Defs.asm"

@inserted inline at 6F00

@r0=text color group id (0-4)

push	{r4-r6,r14}
mov		r4,r0
ldr		r6,=gpDialogueState
ldr		r6,[r6]
mov		r5,#0
Loop1:
ldrb	r1,[r6,#0xA]		@number of lines in the box
cmp		r5,r1
bge		EndLoop
ldrb	r0,[r6,#0xB]		@number of lines already gone by
add		r0,r5
bl		_modsi3
lsl		r0,#3
ldr		r1,=gDialogueText
add		r0,r1
mov		r1,r4
bl		Text_SetColorId
add		r5,#1
b		Loop1
EndLoop:
strb	r4,[r6,#0x8]
pop		{r4-r6}
pop		{r0}
bx		r0

.ltorg
