	.thumb

FcukTanksHook:
	@ Jump from FE8U:0803DF10

	@ We are just after a battle simulation
	@?So the battle actor and target are still fresh and up-to-date
	@ Which allows us to load battle stats from them

	ldr  r3, =gBattleActor
	mov  r0, #0x5A
	ldsh r0, [r3, r0] @ r0 = actor battle attack

	ldr  r3, =gBattleTarget
	mov  r1, #0x5C
	ldsh r1, [r3, r1] @ r1 = target battle defense

	sub r1, r0, r1 @ r1 = attack - defense = damage per hit

	mov r0, #0 @?default to 0 (can battle)

	cmp r1, #0
	bgt end

	mov r0, #1 @ if attack - defense <= 0, get 1 (cannot battle)

end:
	ldr r3, =0x0803DF18+1

	cmp r0, #0 @?compare (replaced)

	bx r3
