	.thumb

	.global NuggetRankingRefId
	.type   NuggetRankingRefId, function

	gChapterData = 0x0202BBF4

NuggetRankingRefId:

	@ replace FE7J:08031B54

	ldr  r3, =gChapterData

	mov  r0, #0x1B @ ChapterData.mode (should be 1 (lyn), 2 (eli) or 3 (hector))
	ldrb r0, [r3, r0]

	cmp  r0, #1
	bne  not_lyn_mode

	mov  r0, #2 @ lyn mode is eliwood mode

not_lyn_mode:
	sub  r0, #2 @ r0 = mode-2
	lsl  r0, #1 @ r0 = (mode-2)*2

	mov  r1, #0x14 @ ChapterData.config
	ldrb r1, [r3, r1]

	lsl  r1, #25 @?bit 6  -> bit 31
	lsr  r1, #31 @ bit 31 -> bit 0

	add r0, r1 @ return (mode-2)*2 + difficulty
	bx  lr
