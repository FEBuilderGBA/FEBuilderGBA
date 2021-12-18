.thumb
.include "_Definitions.h.s"

@最強に早いルーチン!!

Replace_FillReachRangeForUnit_Fast:
	mov r10, r1
	mov r11, r2

	@regist table
	@r10  range      1
	@r11  mask range 0

	ldr  r0, =gMapMovement
	ldr  r0, [r0]
	mov  r7, r0

	ldr  r0, =gMapUnit
	ldr  r0, [r0]
	mov  r4, r0

	ldr  r0, =gMapMovement2
	ldr  r0, [r0]
	mov  r8, r0

@	mov r5, #0 @ yIt
@	mov r6, #0 @ xIt

	@レジスタ余っているからキャッシュにしておくか
    ldr  r0, =MapAddInRange
	mov  r9, r0

	ldr  r0, =gMapSize
	ldrh r5, [r0, #2] @ r5 = yIt
	
	sub r5, #1
	
StartLoop_Y:
	ldr  r0, =gMapSize
	ldrh r6, [r0] @ r6 = xIt
	
	sub r6, #1

StartLoop_X:
	lsl r2, r5, #2 @ r2 = yIt * sizeof(pointer)
	
	@mov  r0, r7 @gMapMovement
	ldr  r0, [r7, r2]
	ldrb r0, [r0, r6]
	
	cmp r0, #120
	bhi ContinueLoop_X
	
	@mov  r0, r4  @gMapUnit
	ldr  r0, [r4, r2]	@gMapUnit
	ldrb r0, [r0, r6]
	
	cmp r0, #0
	bne ContinueLoop_X
	
	mov  r0, r8  @gMapMovement2
	ldr  r0, [r0, r2]
	ldrb r0, [r0, r6]
	
	cmp r0, #0
	bne ContinueLoop_X

FillRange:
@    ldr r3, =MapAddInRange
@    mov lr, r3
    mov lr, r9
    mov r0, r6		@x
    mov r1, r5		@y
    mov r2, r10		@range
    mov r3, #0x1	@value
    .short 0xF800
    @bl MapAddInRange

@    ldr r3, =MapAddInRange
@    mov lr, r3
    mov lr, r9
    mov r0 ,r6
    mov r1 ,r5
    mov r2, r11
    mov r3, #0x1
    neg r3 ,r3
    .short 0xF800
    @bl MapAddInRange




ContinueLoop_X:
	sub r6, #1
	bge StartLoop_X

ContinueLoop_Y:
	sub r5, #1
	bge StartLoop_Y

	ldr r0, =gSubjectMap @ Subject Map?
	str r7, [r0] @gMapMovement

pop {r0,r1,r2,r3,r4,r5,r6,r7}
mov r8, r0
mov r9, r1
mov r10, r2
mov r11, r3
pop {r3}
bx r3


.ltorg
.align
EALiterals:
