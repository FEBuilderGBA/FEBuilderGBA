@thumb


push	{r4-r6,r14}
ldr		r0,=$03004df0	;CurrentCharPtr
ldr		r0,[r0]
ldr		r1,[r0,#0xC]		;status word
mov		r2,#0x50			;is rescuing + has moved
tst		r1,r2
bne		RetFalse
mov		r2,#0x80
lsl		r2,r2,#0x4				;is in ballista
tst		r1,r2
bne		RetFalse
;New
	mov		r5,r0
	mov		r6,#0x0
ItemLoop:
	lsl		r4,r6,#0x1
	add		r4,#0x1E
	add		r4,r4,r5
	ldrh	r4,[r4]
	cmp		r4,#0x0
	beq		RetFalse	;アイテム末尾で終了
	mov		r0,#0xFF
	and		r0,r4
	mov		r1,#0x24
	mul		r0,r1
	ldr		r1,=$080172d4	;ItemTable
	ldr		r1, [r1]
	add		r0,r0,r1
	ldr		r1,[r0,#0x8]		;weapon abilities
	mov		r2,#0x1
	tst		r1,r2
	beq		NextWeapon
	ldrb	r1,[r0,#0x19]		;weapon range
	lsr		r1, r1, #4				;min
	cmp		r1,#0x1
	bne		NextWeapon			;can only capture at 1 range
	ldr		r0,=$080164f8	;Weapon_Wield_Check
	mov		r14,r0
	mov		r0,r5
	mov		r1,r4
	@dcw	0xF800
	cmp		r0,#0x0
	beq		NextWeapon
	
	ldr	r2, =$03004df0
	ldr	r2, [r2]
	ldr	r0, [r2, #12]
	mov	r1, #64
	and	r0, r1
	cmp	r0, #0
	bne	RetFalse	;謎の分岐　救出中とかのところ？

	push	{r4, r5}
	mov	r4, #16
	ldsb	r4, [r2, r4]
	mov	r5, #17
	ldsb	r5, [r2, r5]
	ldr	r1, =$02033f38
	str	r0, [r1]
	ldr	r0, =$0202e4e0
	ldr	r0, [r0]
	mov	r1, #0
ldr	r2, =$080194bc
mov	lr, r2
@dcw	$F800
	
	ldr	r2, =$08024f20
	mov	lr, r2
	@align 4
	ldr	r2, [adr]
	mov	r0, r4
	mov	r1, r5
	@dcw	$F800
	pop	{r4, r5}


ldr	r2, =$08050a9c
mov	lr, r2
@dcw	$F800
	cmp	r0, #0
	beq	RetFalse

;	@align 4
;	ldr		r0,[adr]		;@Fill_Capture_Range_Map
;	mov		r14,r0
;	mov		r0,r5
;	@dcw	0xF800
;	ldr		r0, =$0203E0E8	;隣接味方数など。条件の一次書き込み場所TargetQueueCounter(0203E0EC)	;@I think that's what this is
;	ldr		r0,[r0]
;	cmp		r0,#0x0
;	beq		NextWeapon
	mov		r0,#0x1
	b		GoBack
NextWeapon:
	add		r6,#0x1
	cmp		r6,#0x4
	ble		ItemLoop
RetFalse:
	mov		r0, #0x3
GoBack:
	pop		{r4-r6}
	pop		{r1}
	bx		r1

;普通の攻撃出現判定
;ldr	r0, =$0802495e
;mov	pc, r0

@ltorg
adr:
