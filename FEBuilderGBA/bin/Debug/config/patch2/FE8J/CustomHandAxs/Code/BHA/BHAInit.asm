@ ProcState:
@   +0x2C, short, timer.
@   +0x2E, short, endtime.
@   +0x5C, word,  Attacker AIS.
@   +0x60, word,  Handaxe AIS.
@   +0x64, word,  Pointer to soundeffect procstate (Procs_efxTeonoSE).
@   +0x68, word,  Select Axs Data Pointer
@		0	OBJ
@		4	PAL
@		8	TEONO SOUND
@		sizeof(16)

.thumb
.include "../Definitions.asm"

push  {r4-r7, r14}
mov   r5, r0


ldr   r0, [r5, #0x5C]
ldr   r7, =getTargetAIS_void_int_a1
bl    GOTO_R7
mov   r6, r0

ldr   r0, [r5, #0x5C]
mov   r1, #0x1
neg   r1, r1
ldr   r7, =MoveBattleCameraOnto
bl    GOTO_R7
mov   r0, r5
bl    L4

  ldrh  r0, [r6, #0x10]
  mov   r1, #0x9
  orr   r0 ,r1
  strh  r0, [r6, #0x10]
  mov   r4 ,r5
  add   r4, #0x29
  ldrb  r1, [r4, #0x0]
  mov   r0 ,r6
  ldr   r7, =ThisMakesTheHPInSpellAnimGoAway
  bl    GOTO_R7
  mov   r0 ,r6
@  ldr   r7, =0x08072401		@{U}
  ldr   r7, =0x080748E5		@{J}
  bl    GOTO_R7
  lsl   r0, r0, #0x10
  asr   r0, r0, #0x10
  cmp   r0, #0x2
  beq   L2
  
    ldr   r0, [r5, #0x5C]
@    ldr   r7, =0x0805A269		@{U}
    ldr   r7, =0x0805B00D		@{J}
    bl    GOTO_R7
    cmp   r0, #0x1
    bne   L3
    
      mov   r0, r6
      ldr   r7, =StartEfxCriticalEffect
      bl    GOTO_R7
      b     L2
      
    L3:
    ldrb  r0, [r4]
    cmp   r0, #0x0
    bne   Return
    
      ldr   r0, [r5, #0x5C]
@      ldr   r7, =0x0806C87D		@{U}
      ldr   r7, =0x0806EBA1		@{J}
      bl    GOTO_R7
      
    L2:
    ldrb  r0, [r4]
    cmp   r0, #0x0
    bne   Return
    
      mov   r0 ,r6
@      ldr   r7, =0x08072451		@{U}
      ldr   r7, =0x8074935		@{J}
      bl    GOTO_R7


Return:
pop   {r4-r7}
pop   {r0}
bx    r0
GOTO_R7:
bx    r7


L4:
push  {r4-r7, r14}
sub   sp, #0x4
mov   r6, r0
ldr   r4, [r6, #0x5C]


@ New handaxe anim, create AIS
ldr   r1, =gSomeSubAnim6CCounter
ldr   r0, [r1]
add   r0, #0x1
str   r0, [r1]

ldr   r1, =BHAFrameDataMeleeLeft
ldr   r2, =BHAFrameDataMeleeRight
ldr   r3, =BHAFrameDataRangedLeft
ldr   r0, =BHAFrameDataRangedRight
str   r0, [sp]
mov   r0, r4
ldr   r7, =PrepAIS
bl    GOTO_R7
mov   r5, r0


@ Correct X for left or right
str   r5, [r6, #0x60]
mov   r0, r4
ldr   r7, =GetAISSubjectID
bl    GOTO_R7
cmp   r0, #0x0
bne L5
  ldrh  r0, [r5, #0x2]
  add   r0, #0x48
  b     L6
L5:
  ldrh  r0, [r5, #0x2]
  sub   r0, #0x48
L6:
strh  r0, [r5, #0x2]


ldr   r0, =BHATotalTime
ldrb  r0, [r0]
strh  r0, [r6, #0x2E]

@ Sound effect
ldr   r0, [r6, #0x5C]
ldr   r1, [r6, #0x60]
bl    BHASEInit
str   r0, [r6, #0x64]

@  OBJ‚Æ‰¹‚ÌŠÇ—‚ðBHAOBJ‚ÖˆÏ÷‚µ‚Ü‚·
mov   r0, r6   @BHA Procs
bl    BHAOBJInit

@ Big handaxe graphics
ldr   r0, [r6, #0x68] @this->0x68 Select Axs Data-> 0x4 PAL
ldr   r0, [r0,#0x4]   @BigHandaxePalette
mov   r1, #0x20
ldr   r7, =StoreSpellPaletteOBJ
bl    GOTO_R7
ldr   r0, [r6, #0x68] @this->0x68 Select Axs Data-> 0x0 IMG
ldr   r0, [r0,#0x0]   @BigHandaxeTiles
ldr   r1, =0x1000
ldr   r7, =StoreSpellTilesOBJ
bl    GOTO_R7

add   sp, #0x4
pop   {r4-r7}
pop   {r0}
bx    r0
