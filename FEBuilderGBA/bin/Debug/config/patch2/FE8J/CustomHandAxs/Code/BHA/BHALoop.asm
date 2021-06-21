@ Mimics Procs_efxTeono_CallASM.
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
ldr   r1, =BHATotalTime
ldrb  r1, [r1]
cmp   r0, r1
blt   Return

  ldr   r4, =SetSomethingSpellFxToFalse
  bl    GOTO_R4
  mov   r0, r5
  ldr   r4, =Break6CLoop
  bl    GOTO_R4
  @b     Return

Return:
pop   {r4-r5}
pop   {r0}
bx    r0
GOTO_R4:
bx    r4
