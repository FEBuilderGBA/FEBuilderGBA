.thumb
@0x2F7B0 ���炩�A�����ɔ��ŗ���R�[�h�������Ȃ��ƃ_���ł��B
@
	ldrb r1, [r4, #0x1a]
	add  r0 ,r0, r1
	strb r0, [r4, #0x1a]

	ldrb r0, [r4, #0x1a]
	cmp     r0,#0x80		@�}�C�i�X�ɓ˂����񂾏ꍇ
	blt     Skip
	mov     r0,#0x0     @�}�C�i�X�ɂ͂ł��Ȃ��̂�0�ɕ␳����
	strb r0, [r4, #0x1a]

Skip:

	mov r0 ,r4

Exit:
ldr r1,=0x0802F7B8
mov pc,r1
