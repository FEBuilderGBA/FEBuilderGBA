@.org 0x080B3594		@FE8J
.thumb

@r0 chapter.no
@r7 mapid

mov  r2, r0      @chapter.no
lsr  r4, r0 ,#0x1
ldr  r3, Table

Loop:
ldr  r1, [r3]
cmp  r1,#0x0
beq  NotFound

ldrb r1, [r3]
cmp  r1,r7
beq  Found

add  r3, #0x4
b    Loop

Found:
ldrh r0, [r3,#0x02]

.short	0xAC0A          @add sp...
ldr	r3,=0x080B3650+1	@r0‚Åw’è‚³‚ê‚½•¶š—ñ‚ğ•\¦
bx  r3

NotFound:
cmp r2,#0x00            @˜Í
beq first_chapter

ldr	r3,=0x080B367C+1	@Í
bx  r3

first_chapter:
ldr	r3,=0x080B364C+1	@˜Í	
bx  r3

.align 4
.ltorg
Table:
