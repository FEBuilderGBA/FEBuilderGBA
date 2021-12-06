.thumb
.include "../_TargetSelectionDefinitions.s"

@thing for drawing menu commands
@arguments:
	@r0 = E_Menu 6C Struct Pointer
	@r1 = relevant 6C_085B6510 Pointer
	
push 	{r4-r6, lr}
mov 	r5, r1
mov 	r6, r2
ldr 	r0, =pActionStruct
ldrb 	r0, [r0, #0xD]
_blh GetUnit
mov 	r1, r5
add 	r1, #0x3C
ldrb 	r1, [r1]
lsl 	r1, r1, #0x1
add 	r1, #0x1E
ldrh 	r4, [r0, r1]
mov 	r1, r0
mov 	r0, r4
_blr 	r6
mov 	r2, r0
mov 	r0, r5
add 	r0, #0x34
mov 	r1, #0x2C
ldsh 	r3, [r5, r1]
lsl 	r3, r3, #0x5
mov 	r6, #0x2A
ldsh 	r1, [r5, r6]
add 	r3, r3, r1
lsl 	r3, r3, #0x1
ldr 	r1, =pBG0TileMap
add 	r3, r3, r1
ldr 	r1, =0x8016848	@{U}
@ldr 	r1, =0x80165F0	@{J}
mov 	lr, r1
mov 	r1, r4
.short 0xF800
pop 	{r4-r6}
pop 	{r1}
bx 	r1
.align
.ltorg
