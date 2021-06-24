@ORG D1440	@{J}
@ORG CC724	@{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@
@�Ȃ���sp�Ƀf�[�^��ۑ����Ă���̂ŁA������g��Ȃ��悤�ɁA�������܂�
@�e�[�u���I��

@r4  table + add address(index*16)
@r3  index

ldr  r2, Table
lsl  r4, r3, #0x4   @index*16
str  r3, [sp, #0xc] @�o�j���ł̓X�^�b�N�ϐ��ɕۑ����Ă�B
                    @���ʂ��Ƃ͎v�����ǁA�\����ς������Ȃ��̂ŗ��p���܂�.
add  r5 ,r2, r4

ldrh r0,[r5, #0x4]  @CheckFlag
cmp  r0,#0x0
beq  CheckUnit

blh  0x080860d0	@CheckFlag	@{J}
@blh  0x08083DA8	@CheckFlag	@{U}
cmp  r0, #0x0
beq  Exit       @NotFound
                @���j�b�g�����Ȃ����Ƃɂ��āA���̃e�[�u���̓��e������Ԃ��܂�.

CheckUnit:
ldrb r0, [r5, #0x0]
blh 0x08017fb0   @GetUnitByCharId	@{J}
@blh 0x0801829c   @GetUnitByCharId	@{U}

Exit:
mov r2 ,r0
ldr r3,[sp, #0xc]   @��قǕۑ�����r3�̕��A

ldr r0, =0x080D1452|1	@{J}
@ldr r0, =0x080CC736|1	@{U}
bx  r0

.align
.ltorg
Table:
@Table
