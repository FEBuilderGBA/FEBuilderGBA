@�嗤�����񂳂�������������Ƃ��ɓ����쐬����Ȃ��o�O��������邽�߁A
@���Ƀ��X�g�ɓ����Ă��铹�́A���X�g�ɒǉ����Ȃ��悤�ɂ���
@
.thumb
@@@.org 0x080BC8BC    @FE8U
@r0  gSomeWMEventRelatedStruct !����!:r0�͖߂�l������܂ŏ��������֎~!
@r1  path array byte[0x20] and byte[0x21]=length
@r2  new path id

push {r6,r5,r4,lr}
mov  r6,#0x20   @const����
ldrb r5,[r1,r6] @���̌����擾 r6=0x20

mov  r3,#0x00

Loop:           @ �󂫂�T��
ldrb r4,[r1, r3]
cmp  r4,r2
beq  TrueExit   @���ɂ���

add  r3,#0x01
cmp  r3,r5
blt  Loop

cmp  r5,r6      @0x20�Ȗ���?
blt  NotFound

mov  r0, #0x1   @���X�g�͖��t �߂�l1��Ԃ�
b    Exit


NotFound:
strb r2, [r1, r5]     @����ǉ�
add  r5, #0x01
strb r5, [r1, r6]     @���̍X�V r6=0x20

bl 0x080bca0c         @FE8U nazo function ���Ԃ񓹂���ʂɏo���֐�?
                      @����r0���K�v�B ������A������ĂԂ܂�r0�͕ێ����Ȃ��Ƃ����Ȃ�

TrueExit:
mov  r0, #0x0   @����I����0��Ԃ�.

Exit:
pop {r6,r5,r4}
pop {r1}
bx  r1
