.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

PUSH {r4,r5,r6,r7,lr}
SUB SP, #0x4
MOV r6 ,r0
MOV r1 ,r6
ADD r1, #0x5E
MOV r0, #0x4
LDRH r1, [r1, #0x0]	@謎の儀式
AND r0 ,r1
CMP r0, #0x0
BNE Exit

	ldr r7, =0x030004B0	@MemorySlot00	@{J}
	@ldr r7, =0x030004B8	@MemorySlot00	@{U}

    LDR r3, =0x0202BCAC @gGameState	{J}
    @LDR r3, =0x0202BCB0 @gGameState	{U}
    MOV r0, #0xC
    LDSH r0, [r3, r0]	@gGameState->cameraRealPos	X

    mov  r2, #0xB * 4 + 0
    LDRB r2, [r7, r2] @SlotB->X
    lsl  r2, r2, #4	@X*16
    sub  r4, r2 , r0            @SlotB->X - gGameState->cameraRealPos->X


    MOV r0, #0xE
    LDSH r0, [r3, r0]	@gGameState->cameraRealPos	Y

    mov  r2, #0xB * 4 + 2
    LDRB r2, [r7, r2] @SlotB->Y
    lsl  r2, r2, #4	@X*16
    sub  r5, r2 , r0            @SlotB->Y - gGameState->cameraRealPos->Y

	SetUnk1:	@arg3=width? わからん
    LDRB r3, [r7, #0x1 * 4 + 0] @Slot1->Unk
    cmp r3, #0x0
    bne SetUnk2
    	mov r3, #8
	    b   SetUnk2

	SetUnk2:   @arg4=height
    LDRB r0, [r7, #0x1 * 4 + 2] @Slot1->Unk
    cmp r0, #0x0
    bne CallFunction
	    mov r0, #10
	    b CallFunction
	SetUnk2_:

	CallFunction:
    STR r0,[SP, #0x0]
    
    MOV r0 ,r6	@this procs
    mov r1 ,r4	@X
    mov r2 ,r5	@Y

	blh 0x080220B0, r4		@main dish	ピカピカ関数	{J}
	@blh 0x08022250, r4		@main dish	ピカピカ関数	{U}
Exit:
ADD SP, #0x4
POP {r4,r5,r6,r7}
POP {r0}
BX r0
