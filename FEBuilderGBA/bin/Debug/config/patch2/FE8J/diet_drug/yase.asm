@thumb
;0x29410 ���炩�A�����ɔ��ŗ���R�[�h�������Ȃ��ƃ_���ł��B
;
;�t���[�̈���g���܂��B
;@org	$8E4F740
	ldrb r0,[r4,#0x8]	;;�\�͕␳�̑̊i�l (jump�R�[�h�ŉ󂷏���)
	ldrb r1,[r5,#0x1A]	;;�A�C�e���̑̊i�̕⏕�l��
	cmp r0,#0xFF		;;255 �Ȃ�΂₹�悤
	beq yase
	add r0,r0,r1
	b store
yase
	cmp     r1,#0x2		;�̊i�␳�l��2�����Ȃ��0�ɂ��Ȃ��Ƃ������Ȃ��Ƃ�
	blt		zero		
	sub r0,r1,#0x2		;�_�C�G�b�g �{���͑̊i��2���������̂�
						;�Ȃ���1��������Ȃ��񂾂�ˁB�Ȃ�ł��낤�H
	b store
zero
	mov r0,#0x0			  ;�̊i�␳�l��0�ɂ���
store
	strb  r0,[r6,#0x1A]   ;;�̊i�␳�l���X�g�A

;;�����Ɍ��Ɍ��̃A�h���X�ɖ߂��V���̃R�[�h������

ldr r1,=$08029418
mov pc,r1
