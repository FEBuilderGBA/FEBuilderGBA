@call 080596F0	{J}
@call 080588C0	{U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@ r0   ItemID (short�\�L)  �A�j����`����ǂݎ�����l
@ r4   ���݃��j�b�g���������Ă���ItemID

str r1,[sp, #0x4]  @�󂷃R�[�h�̍đ�
str r2,[sp, #0x8]  @

mov r1, #0xff      @GetItemIndex
and r0 ,r1

cmp r4, r0         @�ړI�̕���Ȃ�֌W�Ȃ�
beq Exit

cmp r4, #0x28      @HandAxs ���ݒ��ׂĂ��鍀�ڂ́A�蕀�ł���? (0x28 == HandAxs)
bne Exit

push {r0}          @�A�j���e�[�u������ǂݎ����ItemID��ۑ�����

@blh 0x08017558   @GetROMItemStructPtr	{J}
blh 0x080177B0   @GetROMItemStructPtr	{U}
mov r3, r0

pop {r0}           @���A �A�j���e�[�u������ǂݎ����ItemID

@���j�b�g���������Ă���A�C�e���͎蕀��?
ldrb r1,[r3, #0x07]  @ItemType
cmp  r1,#0x02        @0x2 == Axs
bne  Exit

ldrb r1,[r3, #0x19]  @Range
cmp  r1,#0x11
ble  Exit            @���u�U���ł��Ȃ�����Ȃ�{�c

Match:
mov r0, r4           @�蕀���[�V�����Ƃ��ď�������

Exit:
@ldr r3, =0x080596F8|1	@{J}
ldr r3, =0x080588C8|1	@{U}
bx r3
