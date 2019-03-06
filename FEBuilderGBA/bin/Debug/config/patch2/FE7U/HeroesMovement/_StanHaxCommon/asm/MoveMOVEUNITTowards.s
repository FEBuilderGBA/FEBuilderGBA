.thumb
.include "_Definitions.h.s"

@ arguments: r0 = MOVEUNIT to move, r1 = xTarget, r2 = yTarget, r3 = speed
@ returns: 0 if nothing moved
MoveMOVEUNITTowards:
	push {r4-r6, lr}
	
	mov r4, #0 @ If this stays 0, it will mean nothing moved, this will become return value
	mov r5, r0 @ This is the MOVEUNIT we are moving
	mov r6, r3 @ This is how fast we gotta go
	
	_MakePair r0, r1, r2
	
	@ HANDLING X MOVEMENT
	@ -------------------
	
	@ r1 = target x
	_GetPairFirst r1, r0
	lsl r1, #8 @ *256 because 16 MOVEUNIT units = 1 pixel and 16 pixels = one tile (one tile = 16*16 = 256 MOVEUNIT units)
	
	mov r3, #0x4C @ MOVEUNIT field 0x4C = display xPos
	ldrh r2, [r5, r3]
		mov r12, r2 @ r12 = r2 = Current MOVEUNIT X
		
		@ r2 = (target pos) - (current pos)
		sub r2, r1, r2
		
		@ Thats a trick to make a sign value (-1, 0 or +1 depending on sign)
		neg r1, r2 @ r1 = -r2
		asr r1, r1, #31 @ r1 = -1 if r2 > 0
		asr r2, r2, #31 @ r2 = -1 if r2 < 0
		sub r2, r1      @ r2 = (r2 - r1) = sign(r2)
		
		orr r4, r2 @ Or to check result (will become non-zero if we have non-zero movement)
		lsl r2, r6 @ Multiplying by 2^speed (Maybe I should force lsl here instead to avoid loop being stuck?)
		
		add r2, r12 @ Adding
	strh r2, [r5, r3]
	
	@ HANDLING Y MOVEMENT
	@ -------------------
	
	@ (See above to know what I do, since it's pretty much the exact same thing here)
	
	_GetPairSecond r1, r0
	lsl r1, #8
	
	mov r3, #0x4E @ MOVEUNIT field 0x4E = display yPos
	ldrh r2, [r5, r3]
		mov r12, r2
		
		sub r2, r1, r2
		
		neg r1, r2
		asr r1, r1, #31
		asr r2, r2, #31
		sub r2, r1
		
		orr r4, r2
		lsl r2, r6
		
		add r2, r12
	strh r2, [r5, r3]
	
End:
	@ Returning 0 if not moved (used to break from 6C loop at the end of movement)
	mov r0, r4
	
	pop {r4-r6}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ nothing
