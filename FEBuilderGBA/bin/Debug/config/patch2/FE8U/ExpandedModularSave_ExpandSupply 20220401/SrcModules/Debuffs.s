
	.thumb

	@ This defines two functions to be used with EMS block declarations:
	@ - SUD_SaveDebuffs, the saving function
	@ - SUB_LoadDebuffs, the loading function

	@ This EMS module is probably simple enough that you can use this as example
	@ If you ever need to write your own.

	@ NOTE: this requires DebuffTablePointer to refer to a pointer *in rom* to the debuff table

	WriteAndVerifySramFast = 0x080D184C+1
	ReadSramFastAddr       = 0x030067A0   @ pointer to the actual ReadSramFast function

	.global SUD_SaveDebuffs
	.type   SUD_SaveDebuffs, function

SUD_SaveDebuffs:
	@ arguments:
	@ - r0 = target address (SRAM)
	@ - r1 = target size

	@ we don't need to do anything after calling WriteAndVerifySramFast
	@ so we just let it return to what called this.
	@ (no push/pop, just pass lr)

	ldr r3, =WriteAndVerifySramFast

	mov r2, r1   @ WriteAndVerifySramFast arg r2 = size
	mov r1, r0   @ WriteAndVerifySramFast arg r1 = target address

	ldr r0, =DebuffTablePointer
	ldr r0, [r0] @ WriteAndVerifySramFast arg r0 = source address

	bx  r3

	.align

	.global SUD_LoadDebuffs
	.type   SUD_LoadDebuffs, function

SUD_LoadDebuffs:
	@ arguments:
	@ - r0 = source address (SRAM)
	@ - r1 = source size

	@ same logic as for SUD_SaveDebuffs, we don't need to do anything after the function call
	@ so we just don't bother making it return here at all

	ldr r3, =ReadSramFastAddr
	ldr r3, [r3] @ r3 = ReadSramFast

	mov r2, r1   @ ReadSramFast arg r2 = size
	@ implied    @ ReadSramFast arg r0 = source address

	ldr r1, =DebuffTablePointer
	ldr r1, [r1] @ ReadSramFast arg r1 = target address

	bx  r3

	.pool

	.align
