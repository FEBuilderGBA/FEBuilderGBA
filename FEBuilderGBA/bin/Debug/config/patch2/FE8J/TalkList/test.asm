@thumb
;@org	$08092ee8 > 00 48 87 46 XX XX XX XX

	ldrb	r1, [r3]
	lsl	r0, r1, #1
	add	r0, r0, r1
	lsl	r0, r0, #2
	add	r0, r0, r4
	strb	r6, [r0, #10]
	mov	r6, r0
	bl	kaiwa
	strb	r0, [r6, #11]
	ldr	r0, =$08092ef4
	mov	pc, r0
	
kaiwa
	push	{lr, r3, r4}
	ldr	r0, =$0202bcec
	ldrb	r0, [r0, #14]
	lsl	r0, r0, #24
	asr	r0, r0, #24
ldr	r1, =$080345b8	;イベントロード
mov	lr, r1
@dcw $F800
	ldr	r5, [r0, #4]	;r5に会話イベント先頭
	mov	r4, #0
loopstart
	ldrh	r0, [r5, #2]
ldr	r1, =$080860d0	;flag
mov	lr,r1
@dcw	$F800
	lsl	r0, r0, #24
	cmp	r0, #0
	bne	loopcheck
;自分かどうかチェック
	ldr	r0, [r7]
	ldrb	r0, [r0, #4]	;自分
	ldrb	r1, [r5, #8]	;会話条件攻め
	cmp	r0,	r1
	bne	loopcheck
;04チェック
	ldrh	r0, [r5]
	cmp	r0, #4
	beq	talk04
;3チェック
	ldrh	r0, [r5, #12]
	cmp	r0, #3
	bne	got
	ldrh	r0, [r5, #14]	;条件フラグロード
ldr	r1, =$080860d0	;flag
mov	lr,r1
@dcw	$F800
	b	onaji
talk04
	ldr	r1, [r5, #12]
ldr	r0, =$080d65c0	;bx r1にジャンプ
mov	lr, r0
@dcw $F800
onaji
	lsl	r0, r0, #24
	cmp	r0, #0
	beq	loopcheck

got
	ldrb	r0, [r5, #9]
ldr	r1, =$08017fb0	;ユニットIDからステータス取得
mov	lr, r1
@dcw	$F800
	cmp	r0, #0
	beq	loopcheck	;居ないならジャンプ
	mov	r2, r0
	ldr	r0, [r0, #12]
	lsl r1, r0, #29
	bmi	loopcheck	;死亡チェック
	lsl r1, r0, #15
	bmi	loopcheck	;離脱チェック

	add	r0, r4, #1
	lsl	r0, r0, #24
	lsr	r4, r0, #24
	cmp	r4, #3
	beq	end	;もう一画面に入る最大人数(3)なら終了?
loopcheck
	add	r5, #0x10
	ldr	r0, [r5]	;次の会話条件読み
	cmp	r0, #0
	bne	loopstart
end
	mov	r0, r4
	pop	{pc, r3, r4}