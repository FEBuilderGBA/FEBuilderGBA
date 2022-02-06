.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb
	
	WriteAndVerifySramFast = 0x080D184D
	ReadSramFastAddr       = 0x030067A0   @ pointer to the actual ReadSramFast function

	
	
	@ arguments:
@ a saving and loading function (they take r0 = chunk save address; r1 = size; and are responsible for writing to/reading from SRAM)
@ arguments:
@ - r0 = target address (SRAM)
@ - r1 = target size
	
	.global SaveConvoyItems
	.type   SaveConvoyItems, function
	.global StoreConvoyItemsToSaveFile
	.type   StoreConvoyItemsToSaveFile, function
	

SaveConvoyItems:
StoreConvoyItemsToSaveFile:
push {r4, r14}
mov   r2, r1
mov   r1, r0
ldr   r0, =ConvoyExpansionRamLink
ldr r0, [r0] 
ldr   r4, =WriteAndVerifySramFast
bl    GOTO_R4
pop   {r4}
pop   {r0}
bx    r0
GOTO_R4:
bx    r4

.pool
.align 4



	.global LoadConvoyItems
	.type   LoadConvoyItems, function
	.global LoadConvoyItemsFromSaveFile
	.type   LoadConvoyItemsFromSaveFile, function

LoadConvoyItems:
LoadConvoyItemsFromSaveFile:
push {r4, r14}
mov   r2, r1
ldr   r1, =ConvoyExpansionRamLink
ldr r1, [r1] 
ldr   r4, =ReadSramFastAddr
ldr   r4, [r4]
bl    GOTO_R4B
pop   {r4}
pop   {r0}
bx    r0
GOTO_R4B:
bx    r4

.pool
.align 4

