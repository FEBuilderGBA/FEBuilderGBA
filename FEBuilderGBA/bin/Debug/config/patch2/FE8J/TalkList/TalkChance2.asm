@thumb


	mov	r4, #0
	str	r4, [sp, #60]
	ldr	r1, =$0200d6e0
	lsl	r0, r7, #2
	add	r5, r0, r1
	mov	r0, r6
	add	r0, #16
	add	r0, r8
ldr	r1, =$08003cf8
mov	lr, r1
@dcw $F800

	ldr	r0, =$0202bcec
	ldrb	r0, [r0, #14]	;マップID
;	lsl	r0, r0, #24
;	asr	r0, r0, #24
ldr	r1, =$080345b8	;イベントロード
mov	lr, r1
@dcw $F800
	ldr	r6, [r0, #4]	;r6に会話イベント先頭
	
	mov	r7, r5
	mov	r1, r8
	str	r1, [sp, #68]
	ldr	r2, [sp, #52]
	str	r2, [sp, #72]
	mov	r3, r9
	neg	r3, r3
	mov	r0, r9
	orr	r3, r0
	mov	r8, r3
	ldrh	r0, [r6]
	cmp	r0, #0
	beq	end
loopstart
	ldrh	r0, [r6, #2]
ldr	r1, =$080860d0	;flag
mov	lr,r1
@dcw	$F800
	lsl	r0, r0, #24
	bne	loopcheck
;自分かどうかチェック
	ldr	r0, [r7]
	ldr	r0, [r0]
	ldr	r0, [r0]
	ldrb	r0, [r0, #4]
	ldrb	r1, [r6, #8]
	cmp	r0,	r1
	bne	loopcheck
;04チェック
	ldrh	r0, [r6]
	cmp	r0, #4
	beq	talk04
;3チェック
	ldrh	r0, [r6, #12]
	cmp	r0, #3
	bne	got
	ldrh	r0, [r6, #14]	;条件フラグロード
ldr	r1, =$080860d0	;flag
mov	lr,r1
@dcw	$F800
	lsl	r0, r0, #24
	beq	loopcheck
	b	got
	
talk04
	ldr	r1, [r6, #12]
ldr	r0, =$080d65c0	;bx r1にジャンプ
mov	lr, r0
@dcw $F800
	lsl	r0, r0, #24
	beq	loopcheck

got
	ldrb	r0, [r6, #9]
ldr	r1, =$08017fb0	;ユニットIDからステータス取得
mov	lr, r1
@dcw	$F800
	cmp	r0, #0
	beq	loopcheck	;居ないならジャンプ
	ldr	r2, [r0, #12]
	lsl r1, r2, #29
	bmi	loopcheck	;死亡チェック
	lsl r1, r2, #15
	bmi	loopcheck	;離脱チェック
	mov	r1, #8
	and	r2, r1
	lsl	r2, r2, #16
	lsr	r5, r2, #16
;	cmp	r5, #0
;	bne	loopcheck	;相手の出撃状態？
;0なら出撃(のはず)
	ldr	r0, [r0]
	ldrh	r0, [r0]	;フォルデの名前ロード
ldr	r1, =$08009fa8	;文字変換？
mov	lr, r1
@dcw	$F800
	mov	r2, r0
	lsl	r0, r4, #3
	ldr	r1, =$0200E098
	add	r0, r0, r1
	ldr	r3, [sp, #68]	;
	add	r0, r3, r0
	lsl	r1, r4, #1
	add	r1, r1, r4
	lsl	r1, r1, #2
	add	r1, #18
	ldr	r3, [sp, #72]	;
	add	r1, r3, r1
	mov	r3, #0
	str	r3, [sp, #0]
	str	r2, [sp, #4]	;;;変換文字
	mov	r2, r8
	lsr	r2, r2, #31
	cmp	r5, #0
	beq	akarui	;相手の出撃状態？
	mov	r2, #1
akarui
ldr	r5, =$08004374
mov	lr, r5
@dcw	$F800
	add	r0, r4, #1
	lsl	r0, r0, #24
	lsr	r4, r0, #24
	cmp	r4, #3
	beq	return	;もう一画面に入る最大人数(3)なら終了?
loopcheck
	add	r6, #0x10
	ldr	r0, [r6]	;次の会話条件読み
	cmp	r0, #0
	bne	loopstart
end
	ldr	r0, =$08094e5e
	mov	pc, r0
;	cmp	r4, #2
;	bhi	return
;	ldr	r6, [sp, #80]
;	ldr	r1, [sp, #44]
;	add	r0, r6, r1
;	lsl	r5, r0, #3
;	ldr	r7, [sp, #84]
;	add	r7, r10	;(sl)
;	mov	r2, r9
;	neg	r6, r2
;	orr	r6, r2
;	ldr	r0, =$08004374
;	mov	r9, r0
;loop255
;	ldr	r0, =$000004C6
;ldr	r1, =$08009fa8	;文字変換？
;mov	lr, r1
;@dcw	$F800
;	mov	r3, r0
;	lsl	r0, r4, #3
;	ldr	r1, =$0200e098
;	add	r0, r0, r1
;	add	r0, r5, r0
;	lsl	r1, r4, #1
;	add	r1, r1, r4
;	lsl	r1, r1, #2
;	add	r1, #18
;	add	r1, r7, r1
;	mov	r2, #0
;	str	r2, [sp, #0]
;	str	r3, [sp, #4]
;	lsr	r2, r6, #31
;	mov	r3, #0
;mov	lr, r9
;@dcw	$F800
;	add	r0, r4, #1
;	lsl	r0, r0, #24
;	lsr	r4, r0, #24
;	cmp	r4, #2
;	bls	loop255
;	
return
	ldr	r0, =$08094ea6
	mov	pc, r0
