@thumb
;0802C2F0

ldr	r0, [r0, #0]
push	{r7}
mov	r7, r0
ldr	r0, [r0, #0x28]
ldr	r1, [r3, #0x28]
orr	r0, r1
mov	r1, #0x80
lsl	r1, r1, #1
and	r0 ,r1

push	{r0}
mov	r0, #0

aaa:
lsl	r1, r0, #1	;*2
add	r1, r1, r0	;*3
ldr	r2, [adr+8]
ldrb	r2, [r2, r1]
cmp	r2, #0
beq	ccc
ldrb	r1, [r3, #4]	;class id
cmp	r1, r2
beq	bbb
add	r0, #1
b	aaa

bbb:
lsl	r1, r0, #1	;*2
add	r1, r1, r0	;*3
add	r1, #1
ldr	r2, [adr+8]
ldrb	r2, [r2, r1]
cmp	r2, 0
beq	GetValue
ldrb	r1, [r7, #5]	;yunitto id
and	r1, r2
beq	GetValue
add	r0, #1
b	aaa

GetValue:
lsl	r1, r0, #1	;*2
add	r1, r1, r0	;*3
add	r1, #2
ldr 	r2, [adr+8]
ldrb	r2, [r2, r1]
add	r4, r2
pop	{r0}
b	end

ccc:
pop	{r0}
cmp	r0, #0x0	;‰º‹‰‚È‚ç•ªŠò
beq	end
add	r4, #60

end:
pop	{r7}
ldr	r1, =0x0802c318
mov	pc, r1

.ltorg
.align
adr: