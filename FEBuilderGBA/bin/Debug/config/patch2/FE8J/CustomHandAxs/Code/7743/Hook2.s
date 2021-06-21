@
@ BHA ProcState:
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
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.macro blh_label to, reg=r3
  ldr \reg, \to
  mov lr, \reg
  .short 0xf800
.endm

@Hook 0805CCA8	{J}
@Hook 0805BF0C	{U}

@r5 AIS

push {r4}

mov r0, r5
blh 0x08056108   @SetSomethingSpellFxToTrue	@{J}
@blh 0x08055160   @SetSomethingSpellFxToTrue	@{U}

blh 0x08056120   @ClearBG1Setup	@{J}
@blh 0x08055178   @ClearBG1Setup	@{U}

mov r0, r5
blh_label SelectAxsData
mov r4, r0
cmp r0, #0x1
bne StartBHA

StartVanillaHandAxs:
ldr   r0, =0x085FF2B8 @Procs efxTeono	@{J}
@ldr   r0, =0x085D5088 @Procs efxTeono	@{U}
mov   r1, #0x3
blh   0x08002bcc   @New6C	@{J}
@blh   0x08002c7c   @New6C	@{U}
b     Exit

StartBHA:
ldr   r0, =0x0805CCB0	@BHA Procs Pointer	{J}
@ldr   r0, =0x0805BF14	@BHA Procs Pointer	{U}
ldr   r0, [r0]
mov   r1, #0x03
blh   0x08002bcc   @New6C	@{J}
@blh   0x08002c7c   @New6C	@{U}
str   r4,[r0,#0x68]	@SelectAxsData

Exit:
@r0 == Procs (efxTeono or BHA)

pop {r4}

ldr  r3, =0x0805CCB8|1	@{J}
@ldr  r3, =0x0805BF1C|1	@{U}
bx   r3

.align
.ltorg
SelectAxsData:
