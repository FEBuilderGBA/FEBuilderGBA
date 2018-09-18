.thumb
.include "_Definitions.h.s"

@ .org 0x0170D4
Replace_GetItemRangeMask:
	push {r4-r5, lr}
	
	mov r4, r0
	_blh prItem_GetMaxRange
	
	add r0, #1
	
	mov r1, #1
	lsl r1, r0
	sub r5, r1, #1
	
	mov r0, r4
	_blh prItem_GetMinRange
	
	mov r1, #1
	lsl r1, r0
	sub r0, r1, #1
	
	@ r0 = ((1 << max) - 1) ^ ((1 << min-1) - 1)
	eor r0, r5
	
	@ Example result for 1-2 range:
	@ ((1 << max+1) - 1) = ((1 << 3) - 1) = (0b1000 - 1) = 0b111
	@ ((1 << min)   - 1) = ((1 << 1) - 1) = (0b10   - 1) = 0b001
	@ result = 0b111 ^ 0b001 = 0b110 (bits 1-2 set)
	
	@ Example result for 3-10 range:
	@ ((1 << max+1) - 1) = ((1 << 11) - 1) = (0b100000000000 - 1) = 0b11111111111
	@ ((1 << min)   - 1) = ((1 << 3)  - 1) = ((1 << 3) - 1) = (0b1000 - 1) = 0b111
	@ result = 0b11111111111 ^ 0b111 = 0b11111111000 (bits 3-10 set)
	
	pop {r4-r5}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ notin
