@ Hooked at 0x525E8. Displays numbers for damage dealt by Nosferatu. Args:
@   r0: Recipient's AIS.
.thumb

push  {r4-r6, r14}
mov   r4, r0


@ Recipient's AIS.
mov   r1, #0x0
mov   r2, #0x0
mov   r3, #0x0
bl    BAN_DisplayDamage


@ Vanilla. Overwritten by hook.
ldr   r1, =0x2017728      @ gBattleAnimeCounter	@{U}	{J}
ldr   r0, [r1]
@ldr   r3, =0x80525F0|1	@{U}
ldr   r3, =0x80532E4|1	@{J}
GOTO_R3:
bx    r3
