.thumb
	push {r4-r6, lr}
	mov  r4, r1
	mov  r5, r2
	mov  r6, r3
    sub  sp, #0x4
    mov  r0, #0x0
    str  r0, [sp]
	
	                    @マップ設定から取得
@	ldr  r0, =0x0202BCEC	@FE8J
	ldr  r0, =0x0202BCF0	@FE8U
	ldrb r0, [r0, #14]
@	ldr  r1, =#08034520	@FE8J
	ldr  r1, =0x08034618	@FE8U
	mov  lr, r1
	.short 0xF800
	add  r0, #0x26 @0x72->0x26に変更
	ldrh r0, [r0]
@mov	r0, #52
@	ldr  r1, =0x08002938	@FE8J
	ldr  r1, =0x080029E8	@FE8U
	mov  lr, r1
	mov  r1, r4
	mov  r2, r5
	mov  r3, r6
	.short 0xF800
    add sp, #0x4
    pop {r4-r6}
	pop {pc}
