.thumb

Class_Bonus_Start = (adr)

@org	0x0802C294

ldr	r2, [r0, #4]
push	{r7}
mov	r7, r0
ldr	r0, [r1, #0x28]	@���j�b�g����2
ldr	r1, [r2, #0x28]	@�N���X����2
orr	r0, r1
mov	r1, #0x80
lsl	r1, r1, #1
and	r0 ,r1		@�㋉�N���X����

push	{r0}
mov	r0, #0

Loop:
lsl	r1, r0, #2	@*4
ldr	r2, Class_Bonus_Start
ldrb	r2, [r2, r1]	@�␳�l�ύX�N���XID
cmp	r2, #0
beq	Normal		@�␳�l�ύX�N���XID:00�Ȃ�ʏ폈���ɐi�ݏI��
ldr	r1, [r7, #4]
ldrb	r1, [r1, #4]	@�N���XID
cmp	r1, r2
beq	Unit
add	r0, #1
b	Loop

Unit:
lsl	r1, r0, #2	@*4
add	r1, #1
ldr	r2, Class_Bonus_Start
ldrb	r2, [r2, r1]	@�␳�l�ύX���j�b�gID
cmp	r2, #0
beq	GetValue
ldr	r1, [r7, #0]
ldrb	r1, [r1, #4]	@���j�b�gID
cmp	r1, r2
beq	GetValue
add	r0, #1
b	Loop

GetValue:
lsl	r1, r0, #2	@*4
add	r1, #2
ldr 	r2, Class_Bonus_Start
ldrb	r1, [r2, r1]	@�␳�l
add	r3, r1
pop	{r0}
b	end

Normal:
pop	{r0}
cmp	r0, #0x0	@�����Ȃ番��
beq	end
add	r3, #20		@�㋉�Ȃ�{20

end:
pop	{r7}
ldr	r1, =0x0802c2a8
mov	pc, r1

.ltorg
.align
adr: