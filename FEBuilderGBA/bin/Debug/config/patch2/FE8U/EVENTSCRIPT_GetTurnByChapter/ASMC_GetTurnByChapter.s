@GetTurnByChapter
@
@
@Author 7743
@
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb

push {r4,r5,r6,lr}

@blh 0x080a8d00   @GetNextChapterWinDataEntryIndex	@{J}
blh 0x080A42BC   @GetNextChapterWinDataEntryIndex	@{U}

mov r5 ,r0
mov r4, #0x0

@ldr  r3,=0x030004B0 @MemorySlot FE8J
ldr  r3,=0x030004B8 @MemorySlot FE8U
ldrb  r6, [r3, #0x1 * 0x4]	@Slot1 ChapterID

Loop:
	mov r0 ,r4
@	blh 0x080a8ce0	@{J}
	blh 0x080A429C	@{U}
			@ClearTurn@00	short	章IDとターン数	ターン数 << 7 + 章ID
			@ClearTurn@02	short	クリアタイム秒数
	ldrh r1, [r0,#0x0]
	mov  r2,#0x7f
	and  r1, r2         @chapterid 
	cmp  r1, r6
	beq  Found


Next:
	add r4, #0x1
	cmp r4 ,r5
	blt Loop
Break:
	mov r0,#0x0
	b Exit

Found:
	ldr r0, [r0, #0x0]
	lsl r0 ,r0 ,#0x10
	lsr r0 ,r0 ,#0x17
Exit:
	@ldr  r3,=0x030004B0 @MemorySlot FE8J
	ldr  r3,=0x030004B8 @MemorySlot FE8U
	str  r0,[r3,#0x0C * 4]    @MemorySlotC (Result Value)

pop {r4,r5,r6}
pop {r1}
bx r1
