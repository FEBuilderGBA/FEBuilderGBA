.thumb
.macro blh to, reg=r3
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm

.macro blr reg
	mov lr, \reg
	.short 0xF800
.endm

.set FindChapterObjectiveTrap, EALiterals+0x00

push {lr}

ldr r3, FindChapterObjectiveTrap
blr r3

cmp r0 ,#0x00
bne SetObject

mov r0 ,#0xFF
mov r1 ,#0xFF
mov r2 ,#0xEF
mov r3 ,#0x0
@blh 0x0802E1F0		@FE8J	AddTrap
blh 0x0802E2B8		@FE8U	AddTrap

cmp r0,#0x00
beq Exit

SetObject:

@ldr r3, =0x030004B0	@FE8J MemorySlot0
ldr r3, =0x030004B8	@FE8U MemorySlot0
ldr r2, [r3,#0x1 * 4]	@MemorySlot1
strh r2,[r0,#0x4]		@Trap.Data1 = [MemorySlot1]

ldr r2, [r3,#0x2 * 4]	@MemorySlot2
strh r2,[r0,#0x6]		@Trap.Data2 = [MemorySlot2]

Exit:

pop {r1}
bx r1

.ltorg
.align

EALiterals:
	@ POIN FindChapterObjectiveTrap
