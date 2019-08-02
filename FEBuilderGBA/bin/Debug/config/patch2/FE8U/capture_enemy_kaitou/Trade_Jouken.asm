@thumb
@org	$08022F3C	;FE8U
	;同行者がいる場合は敵ユニットでも交換対象
	lsl	r0, r0, #25
	bmi	$08022f80	;敵ならジャンプ
	ldrb	r0, [r2, #27]	;同行者ロード
	lsl	r1, r0, #24
	bmi	check
	lsl	r1, r0, #25
	bmi	$08022F66	;友軍ジャンプ
	beq	$08022F66
check
	bl	$08019430	;FE8U
	ldr	r2, [$08022F78]
	ldr	r2, [r2]
	ldrb	r0, [r0, #0x1E]
	ldrb	r1, [r2, #0x1E]
	orr	r0, r1
	bne	$08022F74	;道具有るならジャンプ
	b	$08022F66