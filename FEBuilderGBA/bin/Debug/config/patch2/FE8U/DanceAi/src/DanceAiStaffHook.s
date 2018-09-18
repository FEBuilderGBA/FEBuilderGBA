.thumb

.global DanceAiStaffHook
.type DanceAiStaffHook, %function

@ jumpToHack from $03FACA
DanceAiStaffHook:
	ldr  r0, =(gAiData + 0x90) @ gAiData.decision
	ldrb r0, [r0, #0x0A]

	@ If a decision was already taken, don't do dance decision checking
	cmp r0, #0
	bne end

	mov r0, r7
	ldr r3, =DanceAiTryDecide

	bl BXR3

	@ Replaced function end

	ldr  r0, =(gAiData + 0x90) @ gAiData.decision
	ldrb r0, [r0, #0x0A]

end:
	pop {r3}
	mov r8, r3
	pop {r4-r7}
	pop {r3}
BXR3:
	bx  r3
