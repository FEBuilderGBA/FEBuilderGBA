@thumb

;魔防
@dcw	$1c20
@dcw	$3027
@dcw	$7800
@dcw	$7e29
@dcw	$1840
@dcw	$7628
@dcw	$0600
@dcw	$1600
@dcw	$7e22
@dcw	$2118
@dcw	$5661
@dcw	$4288
@dcw	$dd00
@dcw	$762a
;体格
	mov	r0, r4
	add	r0, #82
	ldrb	r0, [r0, #0]	;;CCボーナス
	ldrb	r1, [r5, #26]
	add	r0, r0, r1
	strb	r0, [r5, #26]
	lsl	r0, r0, #24
	asr	r0, r0, #24
	ldrb	r2, [r4, #25]	;上限
	mov	r1, #25
	ldsb	r1, [r4, r1]	;上限
	cmp	r0, r1
	ble	jump
	strb	r2, [r5, #26]
jump
	ldr	r3, =$0802bd5e
	mov	pc, r3