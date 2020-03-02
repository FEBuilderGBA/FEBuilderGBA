
;@org	$0801ca04 > 00 4A 97 46 XX XX XX XX
@thumb
	mov	r2, r0
	ldr	r0, =$03004DF0
	ldr	r0, [r0]
	cmp	r0, #0
	beq	danger	;「危険」範囲モードへ
;
;特定ユニットの攻撃範囲表示モード
;
	ldrb	r1, [r0, #11]	;所属ID
	lsr	r1, r1, #6
	beq	normal	;自軍はジャンプ
	
	ldrh	r1, [r2, #8]
	lsl	r1, r1, #30
	bmi	cancel
	lsl	r1, r1, #1
	bpl	next	;
;Aで矢印
	add	r0, #0x3B
	ldrb	r1, [r0]
	mov	r2, #0x80
	orr	r1, r2
	strb	r1, [r0]
;音
	ldr r0,=$0202bcec
	add r0,#0x41
	ldrb r0,[r0]
	lsl r0,r0,#30
	bmi revert
	mov r0,#0x6A
		ldr r2,=$080d4ef4	;効果音を鳴らす
		mov lr, r2
		@dcw	$F800
revert:
	ldr	r0, =$03004DF0
	ldr	r0, [r0]
	b	delete
cancel:
	ldr	r1, [r0, #56]
	lsl	r1, r1, #1
	lsr	r1, r1, #1
	str	r1, [r0, #56]
	b	delete
next
;チェンジ判定
	ldrh	r1, [r2, #8]
	lsl	r1, r1, #22	;L押しはチェンジ
	bpl	maru
change
	ldr	r0, =$0801c99c
	mov	pc, r0
normal:
	ldrh	r1, [r2, #8]
	lsl	r2, r1, #30
	bpl	maru	;B押してないならジャンプ
	b	delete
;
;危険範囲表示モード
;
danger:
	ldr	r1, =$0202bcac
	add	r1, #62
	ldrb	r1, [r1]
	lsr	r1, r1, #7
	bne	AllUnit
;UnitType:
	ldrh	r1, [r2, #4]
	lsl	r1, r1, #30
	bmi	maru	;押しっぱなしは消さない
	b	delete

;
;カウンタが0x80時の処理
;
AllUnit:
	ldrh	r1, [r2, #14]
	lsl	r2, r1, #30
	bpl	maru	;B離してなければジャンプ
delete
	mov	r4, #2
	ldr	r0, [r0, #12]
	mov	r1, #64
	and	r0, r1
	beq	jump
	mov	r4, #0
jump
	ldr	r2, =$0801ca4a
	mov	pc, r2
maru
	ldr	r2, =$0801CA30
	mov	pc, r2