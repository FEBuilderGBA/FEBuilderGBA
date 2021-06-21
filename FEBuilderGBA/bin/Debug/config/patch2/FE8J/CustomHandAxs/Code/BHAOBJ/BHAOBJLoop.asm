@ Mimics 0x5C080, Procs_efxTeonoOBJ_CallASM.
@ We also end the procloop like 0x5BF40, Procs_efxTeono_CallASM.
@
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

push  {r4-r5, r14}
mov   r5, r0

ldrh  r0, [r5, #0x2C]
add   r0, #0x1
strh  r0, [r5, #0x2C]

L0:
ldrh  r1, [r5, #0x2E]
cmp   r0, r1
bne   Return

  @ Handaxe anim finished, free AIS.
  ldr   r1, =gSomeSubAnim6CCounter
  ldr   r0, [r1]
  sub   r0, #0x1
  str   r0, [r1]
  ldr   r0, [r5, #0x60]
  ldr   r4, =AIS_Free
  bl    GOTO_R4
  
  @ No clue what this does, tbh
@  ldr   r0, =0x2017758	@{U}
  ldr   r0, =0x2017758	@{J}
  mov   r1, #0x1
  str   r1, [r0]
  
  @ Kill soundeffect proc
  ldr   r0, [r5, #0x64]
  ldr   r4, =Delete6C
  bl    GOTO_R4

  mov   r0, r5
  ldr   r4, =Break6CLoop
  bl    GOTO_R4

Return:
pop   {r4-r5}
pop   {r0}
bx    r0
GOTO_R4:
bx    r4
