.thumb
ldr	r0,=0x203A4EC	@attacker FE8U
ldr	r1,=0x203A56C	@defender FE8U
@ldr	r1,=0x203a4e8	@attacker FE8J
@ldr	r1,=0x203a568	@defender FE8J

mov	r2,#0
sub	r2,#1		@default result

@check if attacker has 0 hp, if so get defender id unless the defender also has 0 hp
ldrb	r3,[r0,#0x13]	@current hp of attacker
cmp	r3,#0
bne	skip		@if the attacker has hp, he did not die
ldrb	r3,[r1,#0x13]	@current hp of defender
cmp	r3,#0
beq	end		@if the defender has no hp he can not be the killer
ldr	r2,[r1,#0x00]
ldrb	r2,[r2,#0x04]	@get id of defender, the killer
b	end

@check if the defender has 0 hp, if so get attacker id unless the defender also has 0 hp
skip:
ldrb	r3,[r1,#0x13]	@current hp of defender
cmp	r3,#0
bne	end		@if the defender has hp, he did not die
ldrb	r3,[r0,#0x13]	@current hp of attacker
cmp	r3,#0
beq	end		@if the attacker has no hp he can not be the killer
ldr	r2,[r0,#0x00]
ldrb	r2,[r2,#0x04]	@get id of attacker, the killer
b	end

end:
ldr	r0,=0x30004B8	@FE8U MemorySlot0
@ldr r0, =0x030004B0	@FE8J MemorySlot0
mov	r1,#0x0C
lsl	r1,#2
str	r2,[r0,r1]
bx	lr
