@thumb
;@org	$0801c994

	ldr	r0, =$03004DF0
	ldr	r0, [r0]
	cmp	r0, #0
	beq	unique	;�댯��Ԃ̓W�����v(��������)
	ldrb	r0, [r0, #11]	;����ID
	lsr	r0, r0, #6
	beq	normal	;���R�̓W�����v
unique:
	mov	r0, #0x80
	lsl	r0, r0, #2
	and	r0, r1
 	beq	non_push
	ldr	r0, =$0801c99c
	mov	pc, r0
	
normal:
	mov	r0, #1
	and	r0, r1
 	beq	non_push
	ldr	r0, =$0801c99c
	mov	pc, r0
non_push:
	ldr	r0, =$0801ca00
	mov	pc, r0