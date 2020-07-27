.thumb

.equ origin, 0x080BCBAC
.equ CheckForSkirmishesAt, . + 0x080BCA90 - origin

@original size: 336 bytes
@new size: 336 bytes

PathfindingFunc2:
	push {r4-r7,lr}
	mov r7, r10
	mov r6, r9
	mov r5, r8
	push {r5-r7}
	sub sp, #0x1c
	mov r6, r0
	mov r8, r1
	ldr r0, [sp, #0x3c]
	ldr r7, [sp, #0x40]
	ldr r1, [sp, #0x44]
	mov r9, r1
	mov r5, #0xff
	and r2, r2, r5
	str r2, [sp, #0xc]
	and r3, r3, r5
	and r0, r0, r5
	str r0, [sp, #0x10]
	mov r0, #0x40
	ldr r0, [r6, r0]
	cmp r7, r0
	blt Continue
		b BreakReturnFalse
Continue:
	lsl r0, r3, #0x18
	asr r1, r0, #0x15
	add r1, r8
	str r1, [sp, #0x18]
	mov r2, #0x0
	str r2, [sp, #0x14]
	ldrb r1, [r1, #0x0]
	lsl r1, r1, #0x18
	asr r1, r1, #0x18
	mov r10, r0
	cmp r2, r1
	blt Continue2
		b BreakReturnFalse
Continue2:
	ldr r5, [sp, #0x18]
	add r5, #0x1
Loop1Start:
	ldrb r3, [r5, #0x0]
	mov r2, #0x0
	ldsb r1, [r5, r2]
	ldr r4, [sp, #0xc]
	lsl r0, r4, #0x18
	asr r0, r0, #0x18
	cmp r1, r0
	beq CheckNextNode
		ldr r4, [sp, #0x10]
		lsl r0, r4, #0x18
		asr r4, r0, #0x18
		cmp r1, r4
		bne NotMatch
			mov r2, #0x1
	NotMatch:
		mov r1, r6
		add r1, #0x20
		add r0, r1, r7
		strb r3, [r0, #0x0]
		mov r3, r1
		mov r0, r9
		cmp r0, #0x0
		blt label7
        cmp r2, #0x0
        beq label8
			mov r2, #0x44
            ldr r0, [r6, r2]
            cmp r9, r0
            bge label6
                mov r1, r9
                str r1, [r6, r2]
                mov r2, #0x1
                b label5
		label6:
			mov r2, #0x0
            cmp r9, r0
            bne NotMatch2
				mov r0, #0x40
                ldr r0, [r6, r0]
                cmp r7, r0
                bge NotMatch2
                    mov r2, #0x1
		NotMatch2:
            cmp r2, #0x0
            beq BreakReturnTrue
			label5:	
				mov r2, #0x1
				cmp r2, r7
				bgt StoreByte
				Loop3Start:
                    add r0, r6, r2
                    add r1, r3, r2
                    ldrb r1, [r1, #0x0]
                    strb r1, [r0, #0x0]
                    add r2, #0x1
                    cmp r2, r7
                    ble Loop3Start
                    b StoreByte
			label7:
                cmp r2, #0x0
                bne label4
                    ldsb r1, [r5, r2]		@since r2 must be 0x0 to be here
                    ldr r0, =0x0201b100
                    bl CheckForSkirmishesAt
                    cmp r0, #0x0
                    beq label3
				label8:
                    mov r3, #0x0
                    ldsb r3, [r5, r3]
                    str r4, [sp, #0x0]
                    add r0, r7, #0x1
                    str r0, [sp, #0x4]
                    mov r0, r9
                    add r0, #0x1
                    str r0, [sp, #0x8]
                    mov r0, r6
                    mov r1, r8
                    mov r4, r10
                    asr r2, r4, #0x18
                    bl PathfindingFunc2
                    b CheckNextNode
			.ltorg		
			label4:
				mov r0, #0x40
                ldr r0, [r6, r0]
                cmp r7, r0
                bge BreakReturnTrue
                    mov r2, #0x1
                    cmp r2, r7
                    bgt StoreByte
                        mov r3, r1
					LoopStart2:
                        add r0, r6, r2
                        add r1, r3, r2
                        ldrb r1, [r1, #0x0]
                        strb r1, [r0, #0x0]
                        add r2, #0x1
                        cmp r2, r7
                        ble LoopStart2
			StoreByte:
				mov r0, #0x40
                str r7, [r6, r0]
        BreakReturnTrue:
		mov r0, #0x1
        b ExitFunc

	label3:
		mov r3, #0x0
		ldsb r3, [r5, r3]
		str r4, [sp, #0x0]
		add r0, r7, #0x1
		str r0, [sp, #0x4]
		mov r0, r9
		str r0, [sp, #0x8]
		mov r0, r6
		mov r1, r8
		mov r4, r10
		asr r2, r4, #0x18
		bl PathfindingFunc2
CheckNextNode:
	add r5, #0x1
	ldr r0, [sp, #0x14]
	add r0, #0x1
	str r0, [sp, #0x14]
	ldr r1, [sp, #0x18]
	mov r0, #0x0
	ldsb r0, [r1, r0]
	ldr r2, [sp, #0x14]
	cmp r2, r0
	blt Loop1Start
BreakReturnFalse:
	mov r0, #0x0
ExitFunc:
	add sp, #0x1c
	pop {r3-r5}
	mov r8, r3
	mov r9, r4
	mov r10, r5
	pop {r4-r7}
	pop {r1}
	bx r1

.align
.ltorg
