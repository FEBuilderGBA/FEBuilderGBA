.include "./_TargetSelectionDefinitions.s"

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
@A button case



.equ ActionID, 0x01

push 	{r4, r14}
mov 	 r4, r0

@check to make sure tile is selectable
ldr 	r0, =ppRangeMapRows
lsl 	r3, r2, #0x2
ldr 	r0, [r0]
ldr 	r0, [r0, r3]
ldrb	r0, [r0, r1]

cmp 	r0, #0
beq BadTile

push {r0-r2}

mov r0, r1 @ r0 - xCoord
mov r1, r2 @ r1 - yCoord
    
@blh CheckFunc
ldr r3, CheckFunc
mov lr, r3
.short 0xF800

mov r3, r0
pop {r0-r2}
cmp r3, #0x0
beq BadTile

@store cooridinates in action struct
ldr 	r0, =pActionStruct
mov 	r3, #0x0
strb 	r3, [r0, #0xD]
mov 	r3, #ActionID
strb 	r3, [r0,#0x11]
strb 	r1, [r0, #0x13]
strb 	r2, [r0, #0x14]

ldr r3, =MemorySlot 
add r3, #0x04*0x0B 
strh r1, [r3] @ XX
add r3, #2 
strh r2, [r3] @ YY 

bl AoE_Animation


ldr r3, =MemorySlot
mov r2, #0x68 @ byte 0x68 of Proc  
ldrb r2, [r4, r2] 
str r2, [r3, #4*0x03] @ Store to Memory Slot 3 as rotation 

ldr r0, =AoE_MainEvent
mov r1, #1 
blh EventEngine 
@bl AoE_GenericEffect

bl AoE_ClearGraphics


@Effect/Idle Routine Return Value (r0 Bitfield):
@        & 0x01 | Does things? idunno - pause the hand selector ? 
@        & 0x02 | Ends the menu
@        & 0x04 | Plays the beep sound
@        & 0x08 | Plays the boop sound
@        & 0x10 | Clears menu graphics
@        & 0x20 | Deletes E_FACE #0
@        & 0x40 | Nothing (Not handled)
@        & 0x80 | Orrs 0x80 to E_Menu+0x63 bitfield (Ends the menu on next loop call (next frame))
@

mov r0, #0xb7 
@mov 	r0, #0x6
b End

BadTile:
@bl AoE_ClearGraphics
@blh AoE_FS6C_ButtonPress_Cancel
@mov r0, #0xb7 

ldr  	r0, =pChapterDataStruct
add  	r0, #0x41
ldrb 	r0, [r0]

@ Options set to "no sound effect"
lsl 	r0, #0x1E
cmp 	r0, #0x0
blt 	NoSound

mov r0, #0x6c  @error sound
blh PlaySoundEffect

NoSound:

@I don't want the parent routine to do anything, so it returns 0.
mov  r0, #0x0

End:
pop 	{r4}
pop 	{r3}
bx	r3

.ltorg
.align

CheckFunc:
@POIN CheckFunc
