@thumb
;	@org	$80a0b38
	push	{r4, r5, r6}
	mov	r4, r0
	ldr	r6, =$0202BCAC
	mov	r0, #0
	strh	r0, [r6, #46]	;????
	mov	r3, #0
	
	ldr r0, =$0803144c
	mov lr, r0
	@dcw $F800 ;輸送体ベースアドレスロード
	mov r5, r0
	
loopstart
	ldrh	r0, [r5]
	cmp	r0, #0
	bne	item
okee
	strh	r4, [r5]
;	mov	r0, r3
	b	end
item

	ldr	r2, =$085775cc ;ボタン
	ldr	r2, [r2]
	ldrh	r2, [r2, #4]
	lsl	r2, r2, #22
	bpl	loop
	lsl	r1, r0, #24
	lsr	r1, r1, #24	;id
	lsl	r2, r4, #24
	lsr	r2, r2, #24	;id
	cmp	r1, r2
	bne	loop			;別物なら次へ
ldr	r1, =$08017358
mov	lr, r1
@dcw	$F800
	cmp	r0, #0xFF
	beq	loop		;無限なら次へ
	ldrh	r1, [r5]
	lsr	r1, r1, #8		;輸送隊内のアイテム個数
	cmp	r0, r1
	beq	loop		;満タンなら次へ
	sub	r1, r0, r1	;足りない分の値
	lsr	r2, r4, #8	;r2に預けようとするアイテムの数
	cmp	r2, r1
	bgt	zoushoku
	ldrh	r1, [r5]
	lsr	r0, r1, #8
	add	r0, r0, r2
	lsl	r0, r0, #8
	lsl	r1, r1, #24
	lsr	r1, r1, #24
	orr	r1, r0
	strh	r1, [r5]
;	mov	r0, r3
	b	end
;修理預け
zoushoku
	sub	r2, r2, r1
	lsl	r2, r2, #8
	lsl	r4, r4, #24
	lsr	r4, r4, #24
	orr	r4, r2
;アイテム統合
	lsl	r0, r0, #8
	ldrh	r1, [r5]
	lsl	r1, r1, #24
	lsr	r1, r1, #24	;IDのみ
	orr	r1, r0
	strh	r1, [r5]	;アイテム統合完了
;azukeloop
;	ldrh	r0, [r5]
;	cmp	r0, #0
;	beq	okee
;	add	r5, #2
;	add	r3, #1
;	cmp	r3, #99
;	ble	azukeloop
;	b	non
loop
    add r5, #2
    add r3, #1
    @align 4
    ldr r0, [adr]
    ldrb r0, [r0]
    cmp r3, r0
    ble loopstart
non
    strh r4, [r6, #46]	;????
;	mov	r0, #1
;	neg	r0, r0	
end
	pop	{r4, r5, r6}
	ldr	r0, [r5, #44]
	ldrb	r1, [r6]
	ldr	r2, =$80a0b40
	mov	pc, r2
@ltorg
adr: