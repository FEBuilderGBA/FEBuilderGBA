@thumb
;@org $0009fdb4 > 00 48 87 46 XX XX XX XX
	mov	r0, pc
	add	r0, #0x5C	;LA.dmp
	ldr	r1, =$06011800
ldr	r2, =$08013008
mov	lr, r2
@dcw	$F800
	mov	r0, #195
	mov	r1, #147
	mov	r2, #9
	mov	r3, r7
	bl	addon
	ldr	r0, =$0809fdc0
	mov	pc, r0
addon
	push	{r4, r5, r6, r7, lr}
	mov	r7, r9
	mov	r6, r8
	push	{r6, r7}
	mov	r8, r0
	mov	r9, r1
	mov	r5, r2
	mov	r6, r3
	mov	r7, pc
	add	r7, #0x4	;data.bin
	ldr	r0, =$0808b762
	mov	pc, r0
@incbin data.bin
@ltorg
@incbin LA.dmp