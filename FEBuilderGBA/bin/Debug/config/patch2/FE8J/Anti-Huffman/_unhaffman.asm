;// Original source and concept by Hextator
;// Rewritten by Nintenlord
;// for FE8J Import by 7743

@thumb

pointerTester
@org $08002af4 ;//20 bytes max ;//r0 = data, r1 = dest
	push {lr}
	lsr r2, r0, #31
	beq pointerTester_compressed

pointerTester_uncompressed
	bl uncompHelper
	b pointerTester_end

pointerTester_compressed
	bl compressedHelper 

pointerTester_end:	
	pop {pc}
;//1 fits

@org $08009fb2
	beq loaded
	ldr r1, [pc, #16]
	add r0, r5, #0
	bl $08009fe4
	str r5, [r6, #0]
	b end

loaded
	ldr r0, [pc, #4]

end
	pop {r4-r6}
	pop {pc}

@dcw $00
@dcd $0202a6a8

compressedHelper ;//0x8002af4
	push {lr}
	ldr r2, [pc, #8]
	ldr r2, [r2, #0]
	bl $080d65c4		;//jmp bx2
	pop {pc}

@dcd $03004150

uncompHelper ;//0x800a274(FE8U)
	;//push {lr}
	;//bl main
	;//pop {pc}
	ldr r2, [pc, #0]
	mov pc, r2
@dcd main

;//The following was originally at 0x800a2c4(FE8U)

@org $084540f0

main
	mov r3, #0x80
	lsl r3, r3, #24
	sub r0, r0, r3
	
main_loop
	ldrb r2, [r0, #0]
	strb r2, [r1, #0]
	add r1, #1
	add r0, #1
	cmp r2, #0
	bne main_loop
	bx lr
