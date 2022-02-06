@ Hooked at 0x52304. Displays numbers for most attacks. Args:
@   r0: Recipient's AIS. Can be initiator if devil effect.
.thumb

push  {r4-r6, r14}
mov   r4, r0


@ Recipient's AIS.
mov   r1, #0x0
mov   r2, #0x0
mov   r3, #0x0
bl    BAN_DisplayDamage
mov   r5, r0

@ Opposing AIS. Can heal or take damage due to
@ Sol/Aether or Counter/Countermagic respectively.
mov   r0, r4
ldr   r3, =0x805A2B4|1	@GetOpponentFrontAIS	@{U}
@ldr   r3, =0x805B058|1	@GetOpponentFrontAIS	@{J}
bl    GOTO_R3
mov   r1, #0x1
mov   r2, #0x2
ldsh  r2, [r4, r2]
mov   r3, r5
bl    BAN_DisplayDamage


@ Vanilla. Overwritten by hook.
mov   r0, r4
ldr   r3, =0x805A16C|1 @GetAISSubjectId	@{U}
@ldr   r3, =0x805AF10|1 @GetAISSubjectId	@{J}
bl    GOTO_R3
ldr   r3, =0x805230C|1	@{U}
@ldr   r3, =0x8053004|1	@{J}
GOTO_R3:
bx    r3
