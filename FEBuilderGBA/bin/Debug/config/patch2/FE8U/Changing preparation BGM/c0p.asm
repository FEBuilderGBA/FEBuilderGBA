@thumb
	push {lr, r4, r5, r6}
	mov  r4, r1
	mov  r5, r2
	mov  r6, r3
	
	                    ;マップ設定から取得
;	ldr  r0, =$0202BCEC	;FE8J
	ldr  r0, =$0202BCF0	;FE8U
	ldrb r0, [r0, #14]
;	ldr  r1, =$08034520	;FE8J
	ldr  r1, =$08034618	;FE8U
	mov  lr, r1
	@dcw $F800
	add  r0, #0x26 ;0x72->0x26に変更
	ldrh r0, [r0]
;mov	r0, #52
;	ldr  r1, =$08002938	;FE8J
	ldr  r1, =$080029E8	;FE8U
	mov  lr, r1
	mov  r1, r4
	mov  r2, r5
	mov  r3, r6
	@dcw $F800
	pop {pc, r4, r5, r6}