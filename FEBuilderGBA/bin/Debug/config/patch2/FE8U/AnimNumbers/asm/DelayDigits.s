@ Called by BAN_Proc_DelayDigits.
@ Puts digits in VRAM.
@   +0x29, byte. Number of digits.
@   +0x2A, short. Damage/heal value.
@   +0x2C, byte. AISSubjectId. 0 if left, 1 if right.
.thumb

push  {r4-r7, r14}
mov   r4, r8
mov   r5, r9
push  {r4-r5}
mov   r4, #0x2C
ldrb  r4, [r0, r4]
mov   r5, #0x2A
ldsh  r5, [r0, r5]
mov   r7, #0x29
ldrb  r7, [r0, r7]
lsl   r7, #0x1


ldr   r0, =BAN_DigitsPalette
mov   r6, #0x0
cmp   r5, #0x0
bgt   Plus
  add   r0, #0x20
  mov   r6, #0x1
  neg   r5, r5
Plus:

@ Load palette.
ldr   r1, =0x20228A8+0x2A0	@gPaletteBuffer+0x2A0	@{U}	@{J}
sub   r2, r4, #0x1
neg   r2, r2
lsl   r2, #0x5
add   r1, r2
mov   r2, #0x8
swi   #0xC                @ CpuFastSet
ldr   r3, =0x8001F94|1 @EnablePaletteSync	@{U}
@ldr   r3, =0x8001EE4|1 @EnablePaletteSync	@{J}
bl    GOTO_R3

@ Put minus or plus in OBJ VRAM.
ldr   r0, =0x85C8278      @ Bigger stat-ups digits.	@{U}
@ldr   r0, =0x85F24A8      @ Bigger stat-ups digits.	@{J}
mov   r8, r0
mov   r0, #0x1C
add   r0, r6
lsl   r0, #0x6
add   r0, r8
ldr   r1, =0x6012400
lsl   r2, r4, #0x9
add   r1, r2
mov   r2, #0x8
swi   #0xC                @ CpuFastSet.

ldr   r0, =0x6012040
lsl   r4, #0x9
add   r4, r0
Loop:
  @ Grab highest power of 10 denominator.
  ldr   r0, =Denom
  ldrh  r1, [r0, r7]
  cmp   r1, #0x0
  beq   Return            @ Zero indicates terminate.
    mov   r6, r1          @ Denominator.

    @ Remove digit from value.
    mov   r0, r5
    swi   #0x6
    mov   r1, r0
    mul   r1, r6
    sub   r5, r1          @ Removes most significant dec digit from value.

    @ Put digit in OBJ VRAM.
    cmp   r0, #0x0
    bne   L1
      mov   r0, #0xF      @ Zero is a special case.
    L1:
    sub   r0, #0x1
    lsl   r0, #0x6
    add   r0, r8
    mov   r9, r0
    mov   r1, r4
    mov   r2, #0x10
    swi   #0xC            @ CpuFastSet.
    mov   r0, r9
    mov   r1, r4
    mov   r2, #0x40
    lsl   r2, #0x4
    add   r0, r2
    add   r1, r2
    mov   r2, #0x10
    swi   #0xC            @ CpuFastSet.

    @ Prepare next iteration.
    add   r4, #0x40
    sub   r7, #0x2
    b     Loop


Return:
pop   {r4-r5}
mov   r8, r4
mov   r9, r5
pop   {r4-r7}
pop   {r0}
bx    r0
GOTO_R3:
bx    r3

Denom:
.short 0
.short 1
.short 10
.short 100
.short 1000
.short 10000
