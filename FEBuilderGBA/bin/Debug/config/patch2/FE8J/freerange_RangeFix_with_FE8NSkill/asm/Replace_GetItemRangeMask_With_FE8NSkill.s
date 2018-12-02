.thumb

@ .org 0x016e8e
Replace_GetItemRangeMask:
	mov r3, r0
	mov r1, #0x0f
	and r0, r1     @GetMaxRange
	add r0, #1
	
	mov r1, #1
	lsl r1, r0
	sub r2, r1, #1
	
	mov r0, r3
	mov r3, #0xf0
	and r0, r3     @GetMinRange
	lsr r0, #0x4

	mov r1, #1
	lsl r1, r0
	sub r0, r1, #1
	
	@ r0 = ((1 << max) - 1) ^ ((1 << min-1) - 1)
	eor r0, r2

	@ Example result for 1-2 range:
	@ ((1 << max+1) - 1) = ((1 << 3) - 1) = (0b1000 - 1) = 0b111
	@ ((1 << min)   - 1) = ((1 << 1) - 1) = (0b10   - 1) = 0b001
	@ result = 0b111 ^ 0b001 = 0b110 (bits 1-2 set)
	
	@ Example result for 3-10 range:
	@ ((1 << max+1) - 1) = ((1 << 11) - 1) = (0b100000000000 - 1) = 0b11111111111
	@ ((1 << min)   - 1) = ((1 << 3)  - 1) = ((1 << 3) - 1) = (0b1000 - 1) = 0b111
	@ result = 0b11111111111 ^ 0b111 = 0b11111111000 (bits 3-10 set)

	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ notin
