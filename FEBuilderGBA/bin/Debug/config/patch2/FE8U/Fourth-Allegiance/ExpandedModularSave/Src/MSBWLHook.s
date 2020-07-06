.thumb

.global MS_BWLSaveHook
.type   MS_BWLSaveHook, %function

ReturnLocation = (0x080A4662+1)

MS_BWLSaveHook:
	@ nop then LynJump from FE8U:080A460E

	@ KNOWN STATE:
	@  r0-r4 free
	@  r5 = RAM BWL entry
	@  r6 = BWL offset (aka CharID*$10)
	@  r7 = gChapterData
	@  r8 = CharID

	@ STEP 1 : SUSPEND SAVE

	mov r0, #3
	ldr r3, =GetSaveTargetAddress
	bl  BXR3

	@ var r4 = Suspend Save target address
	mov r4, r0

	@ arg r0 = Chunk Id
	ldr  r0, =gMS_BWLChunkId
	ldrb r0, [r0]

	ldr r3, =MS_FindSuspendSaveChunk
	bl  BXR3

	@ r1 = save target
	ldrh r1, [r0, #0x00] @ +00 | offset
	sub  r1, #0x10
	add  r1, r6
	add  r1, r4

	mov r0, r5 @ arg r0 = src
	@          @ arg r1 = dst
	mov r2, #1 @ arg r2 = len

	ldr r3, =WriteAndVerifySramFast
	bl  BXR3

	mov r0, sp @ arg r0 = $10 byte buffer
	mov r1, #3 @ arg r1 = block id

	ldr r3, =SaveMetadata_Load
	bl  BXR3

	mov r0, sp @ arg r0 = $10 byte buffer
	mov r1, #3 @ arg r1 = block id

	ldr r3, =SaveMetadata_Save
	bl  BXR3

	@ STEP 2 : GAME SAVE

	ldrb r0, [r7, #0x0C] @ ChapterSate.saveSlot
	ldr r3, =GetSaveTargetAddress
	bl  BXR3

	@ var r4 = Game Save target address
	mov r4, r0

	@ arg r0 = Chunk Id
	ldr  r0, =gMS_BWLChunkId
	ldrb r0, [r0]

	ldr r3, =MS_FindGameSaveChunk
	bl  BXR3

	@ r1 = save target
	ldrh r1, [r0, #0x00] @ +00 | offset
	sub  r1, #0x10
	add  r1, r6
	add  r1, r4

	mov r0, r5 @ arg r0 = src
	@          @ arg r1 = dst
	mov r2, #1 @ arg r2 = len

	@ Saving only 1 byte because skill system compat
	@ (originally saved 3, but that overlaps with skills)

	ldr r3, =WriteAndVerifySramFast
	bl  BXR3

	mov  r0, sp          @ arg r0 = $10 byte buffer
	ldrb r1, [r7, #0x0C] @ arg r1 = block id

	ldr r3, =SaveMetadata_Load
	bl  BXR3

	mov  r0, sp          @ arg r0 = $10 byte buffer
	ldrb r1, [r7, #0x0C] @ arg r1 = block id

	ldr r3, =SaveMetadata_Save
	bl  BXR3

	ldr r3, =ReturnLocation
BXR3:	bx  r3
