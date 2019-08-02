@thumb
@org	$08025344
	;;救出条件
	push	{r4, r5, r6, lr}
	mov	r4, r0	
	ldr	r5, [$080253B0]	;FE8U
	ldr	r0, [r5, #0]
	mov	r1, #11
	ldsb	r0, [r0, r1]
	ldsb	r1, [r4, r1]	;;所属IDロード
	bl	$08024d8c ;FE8U			;;なぞ

	ldrb	r2, [r4, #11]
;	mov	r1, #0x40
;	and	r1, r2
;	bne	$0802542C	;;友軍をかつぐの禁止

	ldr	r2, [r5, #0]
	cmp	r0, #0
	bne	normal		;;多分、自軍の判定？

enemy
;;担ぐ条件
;;スリープならばHP関係なく救出できる
	ldrh	r1, [r4, #48]
	mov	r0, #0xF
	and	r1, r0
	cmp	r1, #2
	beq	normal
;;HP1ケタ
	mov	r0, #9            ; hp1桁
	ldrb	r1, [r4, #19]
	cmp	r0, r1
	blt	$0802542C	;;HP一桁ではない
normal
	ldr	r0, [r2, #4]
	ldrb	r0, [r0, #4]
	cmp	r0, #81
	beq	$0802542C	;;亡霊戦士救出不可
	ldr	r3, [r4, #4]
	ldrb	r0, [r3, #4]
	cmp	r0, #81
	beq	$0802542C	;;亡霊戦士救出不可
;;騎馬判定
	ldr	r1, [r3, #40]
	lsl	r0, r1, #31
	lsr	r0, r0, #31
	bne	$0802542C
ok
;;輸送隊判定
	ldr	r1, [r4]
	ldr	r0, [r4, #4]
	ldr	r1, [r1, #40]
	ldr	r0, [r0, #40]
	orr	r1, r0
	lsl	r0, r1, #22
	lsr	r0, r0, #31
	bne	$0802542C
	ldr	r0, [r4, #12]
	mov	r1, #48
	and	r0, r1
	bne	$0802542C
	mov	r0, r2
	mov	r1, r4
	bl	$0801831C	;FE8U
	cmp	r0, #0
	beq	$0802542C
	ldrb	r0, [r4, #16]
	ldrb	r1, [r4, #17]
	ldrb	r2, [r4, #11]
	b	$08025426
