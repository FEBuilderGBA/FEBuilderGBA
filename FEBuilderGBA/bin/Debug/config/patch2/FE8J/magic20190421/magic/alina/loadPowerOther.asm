@thumb
	mov	r1, #0x14
	ldsb	r1, [r4, r1]	;相手の物魔読み込み
	cmp	r1, #0
	beq	buturi
	ldr	r1, =$08018ecc
	mov	pc, r1
buturi
	ldr	r1, =$08018ec4
	mov	pc, r1