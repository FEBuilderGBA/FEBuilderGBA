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
			;ldr	r2, =$0802CC38 ;FE8U ;;+10 経験値取得関数へ
			;mov	r14, r2
			;@dcw	$F800
			bl		nin_i_exp
			
			ldr	r2, =$080790a4	;ClearMOVEUNITs
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

	ldr r0, =$089A2C48	;(Procs MoveUnit )
	ldr r3, =$08002e9c	;Find6C
	mov	r14, r3
	@dcw	$F800

	ldr r3, =$08002d6c	;Delete6C
	mov	r14, r3
	@dcw	$F800

	pop	{r0}
	bx	r0

;任意の経験値
nin_i_exp
	push	{r4,r5,lr}
	mov		r5, r0

	ldr	r4, =$0203A4EC ;FE8U		;謎の処理1 
	mov	r0, #0xB
	ldsb	r0, [r4, r0]
	mov	r1, #0xC0
	and	r0, r1
	cmp	r0, #0
	bne	noexp

	mov	r0, r4				;謎の処理2 
	ldr	r2,=$0802b9f4 ;FE8U
	mov	r14, r2
	@dcw	$F800

	lsl	r0, r0, #0x18
	cmp	r0, #0
	beq	noexp

	ldr	r0, =$0202BCF0 ;FE8U			;謎の処理3 
	ldrb	r1, [r0, #0x14]
	mov	r0, #0x80
	and	r0, r1
	cmp	r0, #0
	bne	noexp

	ldr	r2, =$0203A4EC ;FE8U
	mov r1, r2
	add	r1, #0x6E
	mov	r0, #0x28				;経験値+0xA 怪盗だけexp:40ぐらいやる
	strb	r0, [r1, #0x0]
	ldrb	r0, [r2, #0x9]
	add	r0, #0x28				;やはりここも置き換えないとダメかなあ 
	strb	r0, [r2, #0x9]
	mov	r0, r2

	ldr	r2,=$0802ba28 ;FE8U
	mov	r14, r2
	@dcw	$F800
								;ここまでが経験値加算計算
	noexp

	ldr r0,=$0859BAC4 ;FE8U
	mov	r1, r5

	ldr	r2,=$08002ce0 ;FE8U
	mov	r14, r2
	@dcw	$F800

	pop	{r4}
	pop	{r5}
	pop	{r0}
	bx	r0



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
@dcd $00000000
@dcd $00000000
