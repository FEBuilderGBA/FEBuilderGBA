@�R�t�̖��O�ƕ�����̔�r
@
@40 0D [X] [Y] [TEXT]
@
@Author 7743
@
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@
@
.thumb
	push	{lr,r4}

	ldr  r0, [r0, #0x38]      @�C�x���g���߂ɃA�N�Z�X�炵�� [r0,#0x38] �ŃC�x���g���߂������Ă���A�h���X�̏ꏊ��
	ldrh r0, [r0, #0x2]       @����0 [TEXT]

	blh 0x0800a240   @GetStringFromIndex	{U}
	mov r4,r0

	blh 0x080314E4   @GetTacticianNameStringPtr	{U}

	mov r1,r4
	blh 0x80d5730    @strcmp
	cmp r0,#0x0
	beq ReturnTrue

ReturnFalse:
	mov r0,#0x0
	b Exit

ReturnTrue:
	mov r0,#0x1

Exit:
	ldr	r2, =0x030004B8  @MemorySlot	{U}
	str	r0, [r2, #0x30]  @memorySlotC

Term:
	pop {r4}
	pop {pc}
