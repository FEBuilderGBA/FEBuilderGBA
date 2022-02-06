@ Hooked at 0x527A4. Displays numbers for HP healed by Nosferatu. Args:
@   r0: Attacker's AIS.
@ r5 is still unused at this point.
.thumb

mov   r5, r0


@ Attacker's AIS.
mov   r1, #0x1
mov   r2, #0x0
mov   r3, #0x0
bl    BAN_DisplayDamage


@ Vanilla. Overwritten by hook.
mov   r0, r5
ldr   r3, =0x805A16C|1 @GetAISSubjectId	@{U}
@ldr   r3, =0x805AF10|1 @GetAISSubjectId	@{J}
bl    GOTO_R3
lsl   r0, #0x1
add   r0, r4
ldr   r3, =0x80527AC|1	@{U}
@ldr   r3, =0x0805349C|1	@{J}
GOTO_R3:
bx    r3
