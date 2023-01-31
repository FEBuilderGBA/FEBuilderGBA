.thumb

@r4 table
@r5 unit
push {lr}
CheckUnit:
ldrb	r0, [r4]	@Table->Unit
cmp		r0, #0x00	@ANY
beq		CheckCond

ldr		r1, [r5, #0x0]
ldrb	r1, [r1, #0x4]
cmp		r0, r1
bne		ReturnFalse

CheckCond:
ldrb	r0, [r4,#0x2]	@Table->Cond
cmp		r0, #0xFF	@ANY
beq		ReturnTrue

CheckBossFlag:
@ldrb	r0, [r4,#0x2]	@Table->Cond
mov		r1, #0x1
and		r0, r1
beq		CheckEnemy

ldr		r0, [r5, #0]
ldrh	r0, [r0, #0x28]
ldr		r1, [r5, #4]
ldrh	r1, [r1, #0x28]
orr		r0, r1
lsl		r0, r0, #0x10
lsr		r0, r0, #0x1f
beq		ReturnFalse

CheckEnemy:
ldrb	r0, [r4,#0x2]	@Table->Cond
mov		r1, #0x80
and		r0, r1
beq		CheckNPC

ldrb	r0, [r5, #0xB]
mov		r1, #0x80
and		r0, r1
beq		ReturnFalse

CheckNPC:
ldrb	r0, [r4,#0x2]	@Table->Cond
mov		r1, #0x40
and		r0, r1
beq		CheckWithoutStaff

ldrb	r0, [r5, #0xB]
mov		r1, #0x40
and		r0, r1
beq		ReturnFalse

CheckWithoutStaff:
ldrb	r0, [r4,#0x2]	@Table->Cond
mov		r1, #0x02
and		r0, r1
beq		ReturnTrue

mov		r1, #0x50
ldrb	r0, [r5, r1]
cmp		r0, #0x4
beq		ReturnFalse

ReturnTrue:
mov r0, #0x1
b   Exit

ReturnFalse:
mov r0, #0x0

Exit:
pop {r1}
bx r1
