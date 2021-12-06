.thumb
.align 4 

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

	.set pr6C_New,                        0x08002C7C @ arguments: r0 = pointer to ROM 6C code, r1 = parent; returns: r0 = new 6C pointer (0 if no space available)	@{U}
@	.set pr6C_New,                        0x08002BCC @ arguments: r0 = pointer to ROM 6C code, r1 = parent; returns: r0 = new 6C pointer (0 if no space available)	{J}
	.equ CurrentUnit, 0x3004E50	@{U}
@	.equ CurrentUnit, 0x3004DF0	@{J}
.equ ClearBG0BG1, 0x0804E884	@{U}
@.equ ClearBG0BG1, 0x0804F610	@{J}
.equ SetFont, 0x8003D38	@{U}
@.equ SetFont, 0x08003C68	@{J}
.equ Font_ResetAllocation, 0x8003D20  	@{U}
@.equ Font_ResetAllocation, 0x08003C50  	@{J}
.equ EndAllMenus, 0x804EF20 		@{U}
@.equ EndAllMenus, 0x0804FCAC 		@{J}
.equ EnsureCameraOntoPosition,0x08015e0d @ r0 = 0, r1 x, r2 y	@{U}
@.equ EnsureCameraOntoPosition,0x08015E19 @ r0 = 0, r1 x, r2 y	@{J}
.equ CenterCameraOntoPosition,0x8015D85 @ r0 = parent proc, r1 x, r2 y	@{U}
@.equ CenterCameraOntoPosition,0x08015D91 @ r0 = parent proc, r1 x, r2 y	@{J}


push {r4, r14}
mov r4, r0




ldr r3, =CurrentUnit
ldr r3, [r3]
ldrb r0, [r3, #0x10]
ldrb r1, [r3, #0x11]
blh 0x8015bbc @SetCursorMapPosition	@{U}
@blh 0x08015BD8 @SetCursorMapPosition	@{J}



ldr r3, =CurrentUnit
ldr r3, [r3]
mov r0, #0 
ldrb r1, [r3, #0x10]
ldrb r2, [r3, #0x11]

blh EnsureCameraOntoPosition

bl AoE_ClearGraphics


bl AoE_Effect 



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

mov r0, #0x0A @ Ends selection & Plays boop sound

mov r0, #0xB7
@mov r0, #4

pop {r4}

pop {r1}
bx r1

.ltorg
.align
