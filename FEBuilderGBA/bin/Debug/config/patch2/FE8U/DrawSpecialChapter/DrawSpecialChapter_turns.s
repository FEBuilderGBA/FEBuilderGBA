@.org 0x080B78DC		@FE8U
.thumb

@r0 chapter.no
@r10 mapid

mov  r2, r0
lsr  r2 ,r2 ,#0x1
mov  r9, r2      @chapter.no >> 1
mov  r2, r0      @chapter.no
ldr  r3, Table

Loop:
ldr  r1, [r3]
cmp  r1,#0x0
beq  NotFound

ldrb r1, [r3]
cmp  r1,r10
beq  Found

add  r3, #0x4
b    Loop

Found:
ldrh r0, [r3,#0x02]
ldr  r3,=0x0159           @終章
cmp  r0,r3
beq  last_chapter

ldr	r3,=0x080B7990+1	@r0で指定された文字列を表示
bx  r3

NotFound:
cmp r2,#0x00           @序章
beq first_chapter

mov r3,#0x01
and r3,r2
bne	gaiden_chapter
lsr r2 ,r2 ,#0x1
ldr	r3,=0x080B7A40+1	@章
bx  r3

first_chapter:
ldr	r3,=0x080B7988+1	@序章
bx  r3

gaiden_chapter:
lsr r2 ,r2 ,#0x1
ldr	r3,=0x080B79CC+1	@外伝
bx  r3

last_chapter:
ldr	r3,=0x080B798E+1	@終章
bx  r3

.align 4
.ltorg
Table:
