@thumb
;@org	$080896ac

    push {r6,lr}
    mov r6, r0
	ldr	r2, [r4, #12]
	ldr	r5, [r2, #0xC]
	lsl	r5, r5, #26
	lsr	r5, r5, #31	;黒星
	bne	black
	mov	r5, #2	;星の色
black:

    mov r0, r2
        @align 4
        ldr r1, =$0802A968
        ldr r1, [r1]
        mov lr, r1
        @dcw $F800

    mov r2, r0
	cmp	r2, #0
	beq	jump
backSTAR:
	ldr	r0	=$08009fa8
	mov	lr, r0
	
	cmp	r2, #5
	beq	five
	cmp	r2, #4
	beq	four
	cmp	r2, #3
	beq	three
	cmp	r2, #2
	beq	two
	cmp	r2, #1
	beq	one
miracleSTAR: ;6以上の★
    mov r0, r6;;ldr	r0, =$02003f06;;;;;;;;;数字位置
	ldr	r1, =$08004a9c
	mov	lr, r1
	mov	r1, r5		;★数字の色
	cmp	r2, #10
	blt	Hitoketa
	add	r0, #0x2
Hitoketa:
	@dcw	$F800
	mov	r2, #1
	b	backSTAR:
	
	
one:
    @align 4
    ldr r0, [adr]
	b	nextSTAR
two:
    @align 4
    ldr r0, [adr+4]
	b	nextSTAR
three:
    @align 4
    ldr r0, [adr+8]
	b	nextSTAR
four:
    @align 4
    ldr r0, [adr+12]
	b	nextSTAR
five:
    @align 4
    ldr r0, [adr+16]
nextSTAR:
	@dcw	$F800
	
	ldr	r3	=$080043b8
	mov	lr, r3
	
	mov	r3, r0
	ldr	r0, =$02003CEC	;指揮（元救出）
	

	mov	r1, #0x18	;横位置なのは確か
	mov	r2, r5		;星の色
	@dcw	$F800
	b	end
	
jump:
	mov	r2, #255
    mov r0, r6;;	ldr	r0, =$02003f06;;;;;;;;;数字位置
	ldr	r1, =$08004a9c
	mov	lr, r1
	mov	r1, r5		;数字の色
	@dcw	$F800
	b	end

end:
    pop {r4, lr}


	ldr	r1, =$080896d8
	mov	pc, r1
@ltorg
adr: