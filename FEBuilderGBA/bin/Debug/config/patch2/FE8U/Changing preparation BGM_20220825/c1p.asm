@thumb
	
;	ldr  r0, =$0202BCEC	;FE8J
	ldr  r0, =$0202BCF0	;FE8U
	ldrb r0, [r0, #14]
;	ldr  r1, =$08034520	;FE8J
	ldr  r1, =$08034618	;FE8U
	mov  lr, r1
	@dcw $F800
	add  r0, #0x26 ;0x72->0x26Ç…ïœçX
	ldrh r0, [r0]
;mov	r0, #52
;   âπäyêÿÇËë÷Ç¶ñΩóﬂ
;	ldr  r1, =$08002424	;FE8J
	ldr  r1, =$080024D4	;FE8U
	mov  lr, r1
	mov  r1, #0
	@dcw $F800
;	ldr  r0, =$080b692a	;FE8J
	ldr  r0, =$080B1D0A	;FE8U
	mov  pc, r0