.thumb

push	{lr}

ldr  r0, [r0, #0x38]      @�C�x���g���߂ɃA�N�Z�X�炵�� [r0,#0x38] �ŃC�x���g���߂������Ă���A�h���X�̏ꏊ��
ldrh r2, [r0, #0x2]       @����1 songid

ldr r3, =0x02024E5C	@BGMSTRUCT	BGM	{J}
@ldr r3, =0x02024E5C	@BGMSTRUCT	BGM	{U}
strh r2, [r3, #0x4]		@�Đ����Ă���BGM

pop {r0}
bx r0
