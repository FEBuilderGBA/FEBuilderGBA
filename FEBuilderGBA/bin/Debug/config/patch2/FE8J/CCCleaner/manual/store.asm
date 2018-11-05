@thumb
;0002f740


	cmp r0, #0x89
	beq	metis
	ldr	r1, =$080174E4
	mov lr, r1
	@dcw $F800
	cmp r0, #0x2E
	beq manual
	ldr	r1, =$0802f760 ;その他のアイテムへ
	mov pc, r1
metis:
	b meti
	
manual:
    mov r0, r6
    ldr r1, =$08017384 ;武器威力
    mov lr, r1
    @dcw $F800
    cmp r0, #255
    bne not_eraser
    ldr r1, =%1111000000000000
    ldrh r2, [r4, #0x3A]
    and r1, r2
    
    ldr r0, =%0000111111000000
    and r0, r2
    lsr r0, r0, #6

    orr r1, r0
    strh r1, [r4, #0x3A]
    mov r1, #%00111111
    and r2, r1
    ldr r3, =$080172d4
    ldr r3, [r3]
    mov r0, #1
    lsl r1, r0, #3
    add r1, r1, r0
    lsl r1, r1, #2

    mov r5, #0 ;カウンタ
next
    add r5, #1
    add r3, r3, r1
    ldrb	r0, [r3, #30] ;アイテム効果
    cmp r0, #0x2e
    bne next
    ldrb r0, [r3, #21] ;アイテム威力
    cmp r0, r2
    beq hit
    b next
hit:
    mov r0, r4
    add r0, #0x1E
    lsl r1, r7, #1
    add r0, r0, r1
    strb r5, [r0]
    mov r1, #2
    strb r1, [r0, #1]
    mov r0, r4
    mov r1, r7
        ldr r3, =$080186a8
        mov lr, r3
        @dcw $F800
    @align 4
    ldr r0, [adr+4] ;使用後の説明
    ldr r1, =$0802f858
    mov pc, r1
    
not_eraser:
    mov r1, %111111
    and r0, r1
merge:
    ldrh r2, [r4, #0x3A]
    and r1, r2
    cmp r1, #0
    beq low
    
    ldr r3, [r4]
    ldrb r2, [r3, #0x26]
    cmp r1, r2
    beq low ;被り
    ldrb r2, [r3, #0x27]
    cmp r1, r2
    beq low ;被り
    b hi ;下位チェックは完了したので、後は確実に上位
low:
    ldrh r2, [r4, #0x3A]
    ldr r1, =%111111000000
    and r2, r1
    orr r0, r2
    strh r0, [r4, #0x3A]
    b return
hi:
    lsl r0, r0, #6
    ldrh r2, [r4, #0x3A]
    mov r1, %111111
    and r2, r1
    orr r0, r2
    strh r0, [r4, #0x3A]

return:
	mov	r0, r4
        ldr r1, =$080186a8
        mov lr, r1
    mov r1, r7
    @dcw $F800
    @align 4
    ldr r0, [adr] ;使用後の説明
    ldr r1, =$0802f858
    mov pc, r1
meti: ;メティスの書
	ldr	r0, [r4, #12]
	mov	r1, #128
	lsl	r1, r1, #6
	orr	r0, r1
	str	r0, [r4, #12]
	mov	r0, r4
ldr	r1, =$0802f750
mov pc, r1
@ltorg
adr: