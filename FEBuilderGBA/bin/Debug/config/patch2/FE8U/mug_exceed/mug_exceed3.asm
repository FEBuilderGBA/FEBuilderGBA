; cut slides
; by laqieer
; 2019/2/10

@thumb
	mov r0, r9
	mov r1, 5
loop
	ldrb r2,[r0,0]
	cmp r2, $E7
	bne no_cut_1
	mov r2, 0
	strh r2,[r0,0]
no_cut_1
	ldrb r2,[r0,$12]
	cmp r2, $E7
	bne no_cut_2
	mov r2, 0
	strh r2,[r0,$12]
no_cut_2
	add r0, $40
	sub r1, 1
	cmp r1, 0
	bge loop
	ldr r2,=$08005D2E+1
	bx r2
