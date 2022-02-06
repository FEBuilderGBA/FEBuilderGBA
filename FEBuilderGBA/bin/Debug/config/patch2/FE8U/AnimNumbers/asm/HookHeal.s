@ Hooked at 0x52A0C. Displays numbers for heals. Args:
@   r0: Recipient's AIS.
.thumb

push  {r4-r7, r14}
mov   r7, r0


@ Recipient's AIS.
mov   r1, #0x0
mov   r2, #0x0
mov   r3, #0x0
bl    BAN_DisplayDamage
mov   r4, r0

@ Opposing AIS. Can heal due to LiveToServe.
mov   r0, r7
ldr   r3, =0x805A2B4|1	@GetOpponentFrontAIS	@{U}
@ldr   r3, =0x805B058|1	@GetOpponentFrontAIS	@{J}
bl    GOTO_R3
mov   r1, #0x1
mov   r2, #0x2
ldsh  r2, [r7, r2]
mov   r3, r4
bl    BAN_DisplayDamage


@ Vanilla. Overwritten by hook.
ldr   r1, =0x2017728      @ gBattleAnimeCounter	@{U}	@{J}
ldr   r0, [r1]
ldr   r3, =0x8052A14|1	@{U}
@ldr   r3, =0x8053704|1	@{J}
GOTO_R3:
bx    r3
