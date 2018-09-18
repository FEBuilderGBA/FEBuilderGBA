@thumb
;@org	$0808eae8
    ldr r2, [sp, #0]
    ldrb r1, [r2, #11]
    lsl r1, r1, #24
    bmi tekiyo ;所属チェック
    lsl r1, r1, #1
    bpl jigun ;所属チェック
    b not
tekiyo
	ldrh	r1, [r2, #32]
	cmp	r1, #0
	beq	not		;;アイテムの持ちチェック
	mov	r2, #0
	b	end
jigun:
    bl pulse
    cmp r1, #0
    beq not
    mov r2, #4
    b end

not
	mov	r2, #5
end	
ldr	r1, =$08003d98
mov	lr, r1
	mov	r1, r6
@dcw	$F800
	ldr	r0, =$0808eaf0
	mov	pc, r0
pulse:
    push {r4, lr}
    mov r4, r0
    mov r1, #0
    mov r0, r2
    add r2, #67
    ldrb r2, [r2]
    cmp r2, #4 ;条件
    blt end_pulse
        @align 4
        ldr r3, [adr] ;奥義の鼓動
        mov lr, r3
        @dcw $F800
    mov r1, r0
end_pulse:
    mov r0, r4
    pop {r4, pc}

@ltorg
adr:
