.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.equ GetItemWeaponExp, 0x8017798

@ r0-r3 are scratch
@ r4 holds attacker data
@ r5 holds defender
@ r6 holds battle buffer
@ r7 holds battle data

MagicCostsHP:
	push	{r4-r7, lr}			@
	mov 	r4, r0 				@ attacker
	mov 	r5, r1 				@ defender
	mov 	r6, r2 				@ battle buffer
	mov 	r7, r3 				@ battle data
	ldr		r2, [r4, #0x4c]		@ load Weapon Ability 1
	mov		r0, #0x2			@ load "Magic"
	and		r2, r0				@
	cmp		r2, #0x0			@ if it does not hit res
	beq		NOT_MAGIC			@ then it is not magic
	mov		r0, r4				@
	add		r0, #0x48			@
	ldrh	r0, [r0]			@ 
	blh		GetItemWeaponExp	@ check if it has a cost
	cmp		r0, #0x0			@ if cost is 0
	beq		NOT_MAGIC			@ then exit
	
	
	@edge cases:
	@first - handle the case where an attack will bring the attacker below casting hp.
	@second - handle doubling
	@third - properly fail an attack when below threshold.
	
START_MAGIC:
	mov		r0, r4				@
	add		r0, #0x48			@
	ldrh	r0, [r0]			@ load attacker weapon
	blh		GetItemWeaponExp	@ get cost
	mov		r2, r0				@ save in r2
	neg		r2, r2				@
	mov		r3, r2				@ save initial value in r3
	mov 	r0, #0x5			@ load current HP delta
	ldsb	r0, [r6, r0]		@
	add		r2, r0				@ add this change to any existing ones
	neg		r2, r2				@
	mov 	r0, #0x13			@ load HP offset
	ldrsb	r0, [r4, r0]		@ load current HP
	cmp		r2, r0				@ compare total HP change to current HP
	blt		SUCCESS				@ if won't kill, then can cast
	
FAILURE:
	mov		r0, #0x0			@ zero out damage
	strh	r0, [r7, #4]		@ battle data.currDamage
	
	mov 	r0, r4				@ experimental - unequip
	add		r0, #0x48
	mov		r2, #0x0
	mov		r1, #0x0
	strh	r1, [r0, #0x0]
	mov		r1, r4
	add		r1, #0x52
	strb	r2, [r1, #0x0]
	b		NOT_MAGIC			@

SUCCESS:
	neg		r2, r2				@
	strb	r2, [r6, #0x5]		@ set HP delta in battle buffer
	add		r0, r3				@ new hp
	strb	r0, [r4, #0x13]		@
	ldr		r2, [r6]			@ copy battlebuffer to r2
	lsl		r1, r2, #0xD		@ remove the damage byte
	lsr		r1, #0xD			@ put it back
	mov		r0, #0x1			@
	lsl		r0, #0x8			@ set HP drain byte at 0x100
	orr		r1, r0				@ 
	ldr		r0, =#0xFFF80000	@
	and		r0, r2				@
	orr		r0, r1				@
	str		r0, [r6]			@ store the byte
	
NOT_MAGIC:
	pop		{r4-r7}				@
	pop		{r0}				@
	bx		r0					@
