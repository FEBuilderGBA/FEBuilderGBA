@thumb
;@org	$0801d7c0
	push	{r4, r5, lr}
	ldr	r2, =$0202e4d4
	ldr	r2, [r2, #0]
	lsl	r1, r1, #2
	add	r1, r1, r2
	ldr	r1, [r1, #0]
	add	r1, r1, r0
	ldrb	r5, [r1, #0]
	lsl	r0, r5, #24
	bmi	Red
	lsl	r0, r5, #25
	bmi	Green
	ldr	r0, =$0801d7ca
	mov	pc, r0

Green:
	add	r5, #1
	mov	r4, r5	
	cmp	r5, #0x54
	bgt	g_end
g_loop:
	mov	r0, r4
	bl	subr
	lsl	r0, r0, #24
	bne	return
	add	r4, #1
	cmp	r4, #0x54
	ble	g_loop
g_end:
	mov	r4, #0x41
	ldr	r0, =$0801d7e6
	mov	pc, r0
Red:
	add	r5, #1
	mov	r4, r5	
	cmp	r5, #0xB2
	bgt	r_end
r_loop:
;索敵チェック
	mov	r0, r4
	bl	subr2
	ldr	r0, [r0, #12]
	mov	r1, #128
	lsl	r1, r1, #2
	and	r0, r1
	bne	next
	mov	r0, r4
	bl	subr
	lsl	r0, r0, #24
	bne	return
next:
	add	r4, #1
	cmp	r4, #0xB2
	ble	r_loop
r_end:
	mov	r4, #0x81
r2_loop:
	cmp	r4, r5
	bgt	return
;索敵チェック
	mov	r0, r4
	bl	subr2
	ldr	r0, [r0, #12]
	mov	r1, #128
	lsl	r1, r1, #2
	and	r0, r1
	bne	next2
	mov	r0, r4
	bl	subr
	lsl	r0, r0, #24
	bne	return
next2:
	add	r4, #1
	cmp	r4, r5
	ble	r2_loop
return:
	pop {r4, r5, pc}
	
subr:
ldr	r1, =$0801d740
mov	pc, r1
subr2:
ldr	r1, =$08019108
mov	pc, r1


