@thumb
	
	push	{r4, lr}
	mov	r4, r1
	mov	r0, #0
	ldsb	r0, [r4, r0]
	mov	r1, #1
	ldsb	r1, [r4, r1]
ldr	r3, =$0801f164
mov	lr, r3
@dcw	$F800
	mov	r0, #2
	ldsb	r0, [r4, r0]
ldr	r3, =$08019108
mov	lr, r3
@dcw	$F800
	bl	jump
	pop	{r4}
	pop	{r1}
	bx	r1
	
jump
	push	{r4, r5, r6, r7, lr}
	mov	r7, r10	;sl
	mov	r6, r9
	mov	r5, r8
	push	{r5, r6, r7}
@dcw $b082     	;sub	sp, #8
	mov	r5, r1
	mov	r1, #10
	ldr	r0, =$03004DF0
	ldr	r0, [r0]
	mov	r8, r0
ldr	r3, =$080348dc
mov	lr, r3
@dcw	$F800
	mov	r4, r0
ldr	r3, =$0804f610
mov	lr, r3
@dcw	$F800
	ldr	r6, =$0203a9f8
	ldr	r0, [r6]
ldr	r2, =$0803512c
mov	pc, r2