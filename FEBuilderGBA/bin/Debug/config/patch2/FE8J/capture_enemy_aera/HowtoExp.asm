@thumb
;@org	$08E4F940

	ldr	r0, [r5]
	cmp	r0, #0
	bne	non
	ldr	r0, =$0203a4e8
	ldr	r1, =$03004df0
	ldr	r1, [r1]
			ldr	r2, =$0802a4f0
			mov	r14, r2
			@dcw	$F800
	mov	r0, r6
			ldr	r2, =$0802cb70 ;;+10 経験値取得関数へ
			mov	r14, r2
			@dcw	$F800

	bl	effect
	bl	clear_double_battleanime

non
	mov	r0, #0
	pop	{r4, r5, r6}
	pop	{r1}
	bx	r1


;二重に描画されるマップアニメを消す
;このルーチンはかなりイケていない。 
;ただ、最初に見つかったアニメは二重に描画されるアニメである可能性が高いので、それをとりあえず消す.
clear_double_battleanime
	push	{lr}

	ldr r0, =$08A132D0	;(Procs MoveUnit )
	ldr r3, =$08002dec	;Find6C
	mov	r14, r3
	@dcw	$F800

	ldr r3, =$08002cbc	;Delete6C
	mov	r14, r3
	@dcw	$F800
	pop	{r0}
	bx	r0


@ltorg

effect
	push	{lr}
	ldr	r0, =$0203a4e8
	mov	r2, r0
	add	r2, #74
	mov	r3, #0
	mov	r1, #79
	strh	r1, [r2, #0]
	ldr	r1, =$0203e1ec
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
	ldr	r1, =$0203a568
	ldr	r2, =$0203a5e8
		
			ldr	r3, =$0807dc48
			mov	r14, r3
			@dcw	$F800
		
	mov	r0, pc
	add	r0, #10
	mov	r1, #3
			ldr	r3, =$08002bcc
			mov	r14, r3
			@dcw	$F800
	pop	{pc}
	
;@incbin	event.bin
@dcd $00000002	;Run
@dcd $08015385	;	AddSkipThread2
@dcd $0005000E	;Wait
@dcd $00000000	;
@dcd $0000000D	;Run
@dcd $08A13C38	;	Procs MapAnimEnd
@dcd $00000000	;End
@dcd $00000000	;
