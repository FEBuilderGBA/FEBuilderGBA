.thumb

.global MS_SaveSizeHook
.type   MS_SaveSizeHook, %function

SaveMetadata_Save.end      = (0x080A301A+1)
SaveMetadata_Save.continue = (0x080A3002+1)

MS_SaveSizeHook:
	@ LynJump from FE8U:080A2FB0

	@ Known State:
	@  r0 = type
	@  r4 = metadata
	@  r5 = 0
	@  r6 = slot id
	@  r7 = 0

	@ Check for the blank block type
	cmp r0, #0xFF
	beq blank_block

	@ Not too much please
	cmp r0, #3
	bhi end

	@ Load size from external lookup

	ldr  r1, =gSaveBlockTypeSizeLookup
	lsl  r0, #1
	ldrh r0, [r1, r0]

	strh r0, [r4, #0xA] @ size

	b continue

blank_block:
	strh r5, [r4, #0x4] @ magic2
	strh r5, [r4, #0x8] @ offset
	strh r5, [r4, #0xA] @ size

continue:
	ldr r3, =SaveMetadata_Save.continue
	bx  r3

end:
	ldr r3, =SaveMetadata_Save.end
	bx  r3

