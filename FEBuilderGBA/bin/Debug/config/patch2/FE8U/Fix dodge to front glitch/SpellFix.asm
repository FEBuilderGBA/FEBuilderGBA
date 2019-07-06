
.thumb
.global SpellEngineFix
.type SpellEngineFix, %function

.macro blh to, reg
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

@ Hook to 0x08004E9A

SpellEngineFix:
mov r5, #0x01
cmp r0, #0x00
bne Skip
mov r0, r4
blh #0x08005208, r2
Skip:
ldr r0, [ r4, #0x38 ] @ Vanilla
	@ Okay. Now I need to check whether the AIS is in a 2-part infinite loop like when spell frames are rendering.
	@ lol next AIS frame is already in r0.
	ldr r2, [ r0, #0x38 ]
	cmp r2, r4
	bne End
	
		@ Well it is a 2-part infinite loop. Let's see if it's done rendering yet.
		
		mov r2, sp
		sub r2, #0x38
		ldr r2, [ r2 ]
		lsr r2, #0x10 @ Shift 0x00190000 2 bytes to get 0x19.
		cmp r2, #0x19
		bne Fix @ Wow! This is a trapped spell if this doesn't equal 0x19!
		
		

		
			@ Looks like it's not done rendering yet... FINE BE THAT WAY.
			False:
			ldr r2, =#0x08004E55
			bx r2 @ Used to be bne #0x08004E54, but WAY out of range reee

End:
cmp r0, #0x0
bne False
Fix:
cmp r5, #0x01 @ Also vanilla
bne Skipbl
blh #0x08004FAC, r0
Skipbl:
pop { r4, r5 }
pop { r0 }
bx r0
