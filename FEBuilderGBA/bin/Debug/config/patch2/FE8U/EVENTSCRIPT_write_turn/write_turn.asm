;���݂̃^�[�����������I�ɏ������݂܂��B
@org	$08E4FAC0
@thumb

push	{lr}

;���� r0
ldr		r0,=$0202BCF0
;�^�[���������֐��Ăяo��
ldr		r1,=$080A4350      ;RegisterChapterTimeAndTurnCount
mov		lr, r1
@dcw	$F800

;��d�ɏ������܂Ȃ��悤�ɁA03�t���O�𗎂Ƃ�
mov	r0, #0x3
ldr	r1, =$08083DA8         ;�t���O��Ԋm�F�֐� RET=����BOOL r0=�m�F����t���O:FLAG
mov	r14, r1
@dcw	$F800

mov	r0, #0
pop	{pc }
