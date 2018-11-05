@thumb
	push {r4, r5, lr}
	mov r4, r0
	mov r5, r1
	lsl r0, r1, #24
	lsr	r0, r0, #24
	cmp r0, #0x89
	beq meti
	b scroll
meti:
	ldr	r0, [r4, #12]
	mov	r1, #128
	lsl	r1, r1, #6
	and	r0, r1
	cmp	r0, #0
	bne	cant_use
	b	can_use
scroll:
    
goto:
    mov r0, r5
    ldr r1, =$080174E4
    mov lr, r1
    @dcw $F800
    cmp r0, #0x2E ;メティスの書の効果
    bne cant_use
    
    mov r0, r5
    ldr r1, =$08017384 ;武器威力
    mov lr, r1
    @dcw $F800

    cmp r0, #255
    bne not_eraser
;消滅処理
    mov r2, %111111
    ldrh r1, [r4, #0x3A]
    and r1, r2
    bne can_use ;何かあるから消せる
    b cant_use
    
not_eraser:
    mov r2, %111111
    and r0, r2
    ldrh r1, [r4, #0x3A]
    and r1, r2
    cmp r0, r1
    beq cant_use ;取得済みなら終わり
    
    cmp r1, #0
    beq can_use ;空いていれば使える

    ldrh r1, [r4, #0x3A]
    ldr r0, =%111111000000
    and r0, r1
    cmp r0, #0
    beq can_use ;空いていれば使える
;first
    ldr r1, [r4]
    ldrb r3, [r1, #0x26]
    
    ldrh r1, [r4, #0x3A]
    mov r0, %111111
    and r0, r1
    cmp r0, r3
    beq can_use ;スキル被りの場合、使える
    
    ldrh r1, [r4, #0x3A]
    lsr r1, r1, #6
    mov r0, %111111
    and r0, r1
    cmp r0, r3
    beq can_use ;スキル被りの場合、使える
;second
    ldr r1, [r4]
    ldrb r3, [r1, #0x27]
    
    ldrh r1, [r4, #0x3A]
    mov r0, %111111
    and r0, r1
    cmp r0, r3
    beq can_use ;スキル被りの場合、使える
    
    ldrh r1, [r4, #0x3A]
    lsr r1, r1, #6
    mov r0, %111111
    and r0, r1
    cmp r0, r3
    beq can_use ;スキル被りの場合、使える

cant_use:
    mov r0, #1 ;使用不可
    b end
can_use:
    mov r0, #0 ;使用可能
end:
    pop {r4, r5, pc}