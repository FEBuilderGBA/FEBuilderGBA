.thumb
.macro blh to, reg=r3
    ldr \reg, =\to
    mov lr, \reg 
    .short 0xF800
.endm

@Hook	0x0589FE	@{J}
@Hook	0x057AB6	@{U}

@���������A�j��ON�ł���?
cmp r4, #0x0
beq Exit			@�A�j��OFF�Ȃ̂ŉ������Ȃ�

@��Ɨx��A�j���̐ݒ�̊m�F
ldr r0, =0x0202BCEC  @ChapterData	{J}
@ldr r0, =0x0202BCF0  @ChapterData	{U}
add r0, #0x41
ldrb r0, [r0]
                   @0x02 ���ʉ� OFF
                   @0x80 �����w���v OFF
mov r1, #0x82
and r0, r1
cmp r0, #0x0
beq Exit           @��Ɨx��A�j���̓I���Ȃ̂ŉ������Ȃ�


@�����̓{�X�ł���?
ldr	 r3, =0x0203A4E8	@gBattleActor	@{J}
@ldr	 r3, =0x0203A4EC	@gBattleActor	@{U}
ldr	 r0, [r3, #0]
ldrh	r0, [r0, #0x28]
lsl		r0, r0, #0x10
lsr		r0, r0, #0x1f
bne		Exit			@�����̓{�X�Ȃ̂ŃA�j����\�����܂�

@��Ɨx��Ȃ�A�j���I�t�ɂ���
mov  r2, #0x50		@WeaponType
ldrb r2, [r3, r2]	@gBattleActor->WeaponType
cmp  r2, #0xFF		@Dance
beq  AnimeOFF
cmp  r2, #0x04		@Staff
beq  AnimeOFF
b    Exit

AnimeOFF:
mov  r4, #0x0

Exit:
@�󂷃R�[�h�̍đ�
blh 0x080599a0	@{J}
@blh 0x08058b70	@{U}
ldr r0, =0x0203E11C @gSomethingRelatedToAnimAndDistance	@{J}
@ldr r0, =0x0203E120 @gSomethingRelatedToAnimAndDistance	@{U}
mov r1, #0x0
ldsh r0, [r0, r1]

ldr r3, =0x08058A08|1	@{J}
@ldr r3, =0x08057AC0|1	@{U}
bx  r3
