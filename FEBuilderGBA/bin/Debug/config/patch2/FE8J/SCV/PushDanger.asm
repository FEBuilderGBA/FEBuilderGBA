@thumb
	ldrh	r1, [r2, #8]
	mov	r0, #0x8
	and	r0, r1
	cmp	r0, #0
	beq	nonstart
	ldr r0, =$0801c74a
	mov	pc, r0
nonstart
	ldrh r1,[r2,#0x8]
	mov r0,#0x2		;@check if B
	and r0,r1
	cmp r0,#0x0
	beq Back
ldr r0, =$0808f44c	;Clear_Screen
mov	lr, r0
@dcw	$F800
	
	ldr r2, =$0202bcac
	ldr r1, [r2,#0x18]
	ldr r0, [r2,#0x14]
	
	bl	MS_Hook

ldr r0, =$080226c0	;Dangerzone:
mov	lr, r0
@dcw	$F800
Back
	ldr r0, =$0801c7a0
	mov	pc, r0
MS_Hook
	
	push	{r4, lr}
	ldr r3, =$08027a5e	;MS_Hook
	mov	pc, r3