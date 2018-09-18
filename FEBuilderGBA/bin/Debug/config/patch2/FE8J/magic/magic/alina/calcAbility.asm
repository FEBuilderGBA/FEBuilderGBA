@thumb
@org $08031960
	ldr		r2, [r4]
	push	{r4, r5, lr}
	cmp		r0, r2
	bne		second
	mov		r2, #19	;自分(自軍)の物理魔法を取る
	@dcw	$E000
second:
	mov		r2, #20	;自分(敵軍)の物理魔法を取る
	ldsb	r2, [r4, r2]
	
	mov		r4, r0
	cmp		r2, #0
	bne		load_magic
	mov		r0, #20	;力
	@dcw	$E000
load_magic:
	mov		r0, #26	;魔力
	ldsb	r0, [r4, r0]	;power
	mov		r5, #18
	ldsb	r5, [r4, r5]	;最大HP
	add		r0, r0, r5
	mov		r2, #21
	ldsb	r2, [r4, r2]	;技
	add		r2, r2, r0
	mov		r0, #22
	ldsb	r0, [r4, r0]	;速さ
	add		r0, r0, r2
	lsl		r5, r0, #1
	mov		r0, #25
	ldsb	r0, [r4, r0]	;幸運
	add		r5, r5, r0
	
	cmp		r1, #0
	bne		magical_enemy
	mov		r0, r4
	bl		$08018f64	;守備
	b		jump
magical_enemy:
	mov		r0, r4
	bl		$08018f84	;魔防
jump:
	lsl		r0, r0, #1
    add		r5, r5, r0