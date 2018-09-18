@thumb
	push {lr}
;	ldr  r0, =$0202BCEC	;FE8J
	ldr  r0, =$0202BCEC	;FE8U
	ldrb r0, [r0, #14]
;	ldr  r1, =$08034520	;FE8J
	ldr  r1, =$08034618	;FE8U
	mov  lr, r1
	@dcw $F800
	add  r0, #0x26 ;0x72->0x26�ɕύX
	ldrh r0, [r0]
;mov	r0, #52
;	ldr  r1, =$08002570	;FE8J
	ldr  r1, =$08002620	;FE8U
	mov  lr, r1
	@dcw $F800
	pop {pc}