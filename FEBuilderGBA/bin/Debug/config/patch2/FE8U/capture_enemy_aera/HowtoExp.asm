@thumb

	ldr	r0, [r5]
	cmp	r0, #0
	bne	non
	ldr	r0, =$0203A4EC ;FE8U
	ldr	r1, =$03004E50 ;FE8U
	ldr	r1, [r1]
			ldr	r2, =$0802a584 ;FE8U
			mov	r14, r2
			@dcw	$F800
	mov	r0, r6
			ldr	r2, =$0802CC38 ;FE8U ;;+10 経験値取得関数へ
			mov	r14, r2
			@dcw	$F800

;			ldr	r2, =$080790a4 ;FE8U	;;待機状態に変える？？？
;			mov	r14, r2
;			@dcw	$F800
	bl	effect
non
	mov	r0, #0
	pop	{r4, r5, r6}
	pop	{r1}
	bx	r1


@ltorg

effect
	push	{lr}
	ldr	r0, =$0203A4EC ;FE8U
	mov	r2, r0
	add	r2, #74
	mov	r3, #0
	mov	r1, #79
	strh	r1, [r2, #0]
	ldr	r1, =$0203e1f0 ;FE8U
	mov	r12, r1
	add	r1, #95
	strb	r3, [r1, #0]
	mov	r2, r12
	add	r2, #98
	mov	r1, #2
	strb	r1, [r2, #0]
	mov	r1, r12
	add	r1, #94
	mov	r2, #1
	strb	r2, [r1, #0]
	sub	r1, #6
	strb	r3, [r1, #0]
	add	r1, #1
	strb	r2, [r1, #0]
	ldr	r1, =$0203A56C ;FE8U
	ldr	r2, =$0203a5ec ;FE8U
		
			ldr	r3, =$0807b900 ;FE8U
			mov	r14, r3
			@dcw	$F800
		
	mov	r0, pc
	add	r0, #10
	mov	r1, #3
			ldr	r3, =$08002c7c ;FE8U
			mov	r14, r3
			@dcw	$F800
	pop	{pc}
	
;@incbin	event.bin
@dcd $00000002
@dcd $08015361
@dcd $0005000E
@dcd $00000000
@dcd $0000000D
@dcd $089A35B0
