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

push {r4,lr}
mov r4,r0

ldr r3, FindChapterObjectiveTrap
blr r3

cmp r0 ,#0x00
bne GetObject

ldr r1, =0x0202BCEC	@ChapterData	@FE8J
@ldr r1, =0x0202BCF0		@ChapterData	@FE8U

ldrb r0,[r1,#0xE]
blh 0x08034520			@FE8J	@GetROMChapterStruct
@blh 0x08034618			@FE8U	@GetROMChapterStruct

mov r1,#0x8a

cmp r4,#0x00
beq Return
mov r1,#0x88
b   Return

GetObject:
mov r1,#0x04
cmp r4,#0x0
beq Return
mov r1,#0x06

Return:
ldrh r0,[r0, r1]

Exit:
pop {r4}
pop {r1}
bx r1

.ltorg
.align

EALiterals:
	@ POIN FindChapterObjectiveTrap
