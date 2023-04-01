.thumb

@ _blh macro, for absolute branch with link
.macro _blh to, reg=r3
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm

	push {r4, lr}
	
	@ Saving argument from r0 to r4 (The Event Engine 6C)
	mov r4, r0
	
	_blh  0x08003c50 @Text_ResetTileAllocation

	_blh 0x0804f610 @ ClearBG0BG1

	ldr  r2, =0x03003020 @ pLCDControlBuffer
	ldrb r0, [r2, #1]
	
	mov r1, #1
	orr r0, r1 @ enable bg0 display
	
	mov r1, #2
	orr r0, r1 @ enable bg1 display
	
	mov r1, #4
	orr r0, r1 @ enable bg2 display
	
	mov r1, #8
	orr r0, r1 @ enable bg3 display
	
	mov r1, #0x10
	orr r0, r1 @ enable obj display
	
	strb r0, [r2, #1]
	
	@ Setting up relevant graphics (Setting up text font and loading UI frame graphics)
	
	mov r0, #0
	_blh 0x08003C68 @ SetFont | Arguments: r0 = Font Struct (0 for default)
	
	_blh 0x080042E0 @ SetupFontForUI
	
	_blh 0x0804F8F4 @ LoadNewUIGraphics

	@ Calling the actual lord split menu 6C, with the event engine as parent
	
	ldr r0, =0x030004B8 @MemorySlot2
	ldr r0, [r0]
	mov r1, r4
	_blh 0x0804F954  @ NewMenu_DefaultChild | Arguments: r0 = Menu Def, r1 = Parent

	pop {r4}
	
	pop {r0}
	bx r0

