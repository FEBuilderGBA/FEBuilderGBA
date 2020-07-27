.thumb

.equ origin, 0x080BCAB8
.equ CheckForSkirmishesAt, . + 0x080BCA90 - origin

@original size: 244 bytes
@new size: 244 bytes

PathfindingFunc1:
push {r4-r7,lr}
mov r7, r10
mov r6, r9
mov r5, r8
push {r5-r7}
sub sp, #0x1c
mov r7, r0
str r1, [sp, #0x8]
ldr r0, [sp, #0x3c]
ldr r1, [sp, #0x40]
mov r8, r1
mov r5, #0xFF
and r2, r2, r5
str r2, [sp, #0xc]
and r3, r3, r5
and r0, r0, r5
str r0, [sp, #0x10]
mov r0, #0x40
ldr r0, [r7, r0]
cmp r8, r0
bge EndReturnFalse
    lsl r0, r3, #0x18
    asr r1, r0, #0x15
    ldr r2, [sp, #0x8]
    add r2, r2, r1
    mov r10, r2
    mov r3, #0x0
    mov r9, r3
    mov r1, #0x0
    ldsb r1, [r2, r1]
    str r0, [sp, #0x18]
    cmp r9, r1
    bge EndReturnFalse
        mov r0, r7
        add r0, #0x20
        add r0, r8
        str r0, [sp, #0x14]
        mov r4, r10
        add r4, #0x1
	StartLoop:
        mov r1, #0x0
        ldsb r1, [r4, r1]
        ldr r5, [sp, #0xc]
        lsl r0, r5, #0x18
        asr r0, r0, #0x18
        cmp r1, r0
        beq CheckNextNode
            mov r2, #0x0
            ldr r3, [sp, #0x10]
            lsl r0, r3, #0x18
            asr r6, r0, #0x18
            cmp r1, r6
            bne NotMatch
                mov r2, #0x1
		NotMatch:
            mov r5, r2
            cmp r5, #0x0
            bne DontCheckMonsters
                mov r1, #0x0
                ldsb r1, [r4, r1]
                ldr r0, =0x0201B100
                bl CheckForSkirmishesAt
                cmp r0, #0x0
                bne CheckNextNode
			DontCheckMonsters:
                ldrb r0, [r4, #0x0]
                ldr r1, [sp, #0x14]
                strb r0, [r1, #0x0]
                mov r1, r7
                add r1, #0x20
                cmp r5, #0x0
                beq label3
					mov r0, #0x40
                    ldr r0, [r7, r0]
                    cmp r8, r0
                    bge BreakReturnTrue
                        mov r2, #0x1
                        cmp r2, r8
                        bgt StoreNewCount
                            mov r3, r1
						LoopPoint:
                            add r0, r7, r2
                            add r1, r3, r2
                            ldrb r1, [r1, #0x0]
                            strb r1, [r0, #0x0]
                            add r2, #0x1
                            cmp r2, r8
                            ble LoopPoint
					StoreNewCount:
						mov r0, #0x40
                        mov r2, r8
                        str r2, [r7, r0]
				BreakReturnTrue:
                    mov r0, #0x1
                    b ExitFunc
					
			label3:
                mov r3, #0x0
                ldsb r3, [r4, r3]
                str r6,[sp, #0x0]
                mov r0, r8
                add r0, #0x1
                str r0,[sp, #0x4]
                mov r0 ,r7
                ldr r1,[sp, #0x8]
                ldr r5,[sp, #0x18]
                asr r2 ,r5 ,#0x18
                bl PathfindingFunc1 
	CheckNextNode:
        add r4, #0x1
        mov r0, #0x1
        add r9, r0
        mov r1, r10
        mov r0, #0x0
        ldsb r0, [r1, r0]
        cmp r9, r0
        blt StartLoop
EndReturnFalse:
	mov r0, #0x0
ExitFunc:
	add sp, #0x1c
	pop {r3,r4,r5}
	mov r8, r3
	mov r9, r4
	mov r10, r5
	pop {r4,r5,r6,r7}
	pop {r1}
	bx r1
.align
.ltorg
