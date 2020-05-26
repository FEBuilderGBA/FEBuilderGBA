.equ INVARID_ITEM, (0xFF)

.thumb
	add	r0, r0, r1
	push {r0}
	bl main
	pop {r1}
	add	r0, r0, r1
	pop	{r4}
	pop	{r1}
	bx	r1
	
main:
	push	{r5, r6, r7, lr}
	mov	r5, r2
	cmp	r5, #5 @res
	bne	rope
	sub	r4, #49		@resは+49されているので元に戻す
rope:
	mov r0, r4
		ldr r1, =0x080168d0	@装備アイテム取得
		mov lr, r1
		.short 0xF800
		ldr r1, =0x080172f0	@武器種類
		mov lr, r1
		.short 0xF800
	mov r7, r0
not_equipment:
	
	mov	r6, #28		@;カウンタセット
loop:				@;アイテム確認ループ
	add	r6, #2
	cmp	r6, #40
	beq	false
	
	ldrh	r0, [r4, r6]
	cmp	r0, #0
	beq	loop
	  ldr r1, =0x08017314
	  mov lr, r1
	  .short 0xF800
	mov	r2, r1
	mov	r1, r0
	lsl	r0, r1, #6	@;盾パッチの下
	bmi	isShield
	b	loop
false:
	mov	r0, #0
	b	end
isShield:
	ldrb	r0, [r2, #28]
	cmp r0, #0
	bne needEquipment	@;武器レベル0以外なら剣装備盾
	ldr	r3, [r2, #16]
	cmp	r3, #0
	bne	CLASSN
	b	true
	
	
CLASSN:
	ldr	r0, [r4, #4]
	ldrb	r0, [r0, #4]
CALN_loop:
	ldrb	r1, [r3]
	cmp	r1, #0
	beq	loop
	cmp	r0, r1
	beq	true
	add	r3, #1
	b	CALN_loop

needEquipment:	@装備可能装備かチェック
	cmp r7, #INVARID_ITEM
	beq true	@装備なしならtrue
	cmp r7, #0
	beq true	@剣装備ならtrue
	b loop		@不可。戻ってほかの盾を探す
	
true:
	ldr	r0, [r2, #12]
	ldrb	r0, [r0, r5]	@;盾のボーナス
end:
	pop	{r5, r6, r7, pc}
	