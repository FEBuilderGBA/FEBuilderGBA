@���̏����Ƀ}�b�`����G�R��F�R��
@4[0] 0D [UNIT] [CLASS] [ASM+1]
@
@���̏����Ƀ}�b�`���邷�ׂĂ̗F�R��G�R��
@4[1] 0D [UNIT] [CLASS] [ASM+1]
@
@UNIT=0x00 ANY
@CLASS=0x00 ANY
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
	push	{r4,r5,r6,r7,lr}

	ldr  r4, [r0, #0x38]      @�C�x���g���߂ɃA�N�Z�X�炵�� [r0,#0x38] �ŃC�x���g���߂������Ă���A�h���X�̏ꏊ��
	ldrb r0, [r4, #0x0]       @����0 4[����0]

	mov  r1,#0xf              @check type
	and  r0,r1                
	cmp  r0,#0x00
	bne  AllyToEnemy
	                          @EnemyToAlly
@	ldr  r5,=0x0202CFB8       @Enemy Units	{J}
@	ldr  r6,=0x0202CFB8+(0x32*0x48)       @End Address	{J}
	ldr  r5,=0x0202CFBC       @Enemy Units	{U}
	ldr  r6,=0x0202CFBC+(0x32*0x48)       @End Address	{U}
	mov  r7,#0x40
	b Each
	
AllyToEnemy:
@	ldr  r5,=0x0202DDC8       @Ally Units	{J}
@	ldr  r6,=0x0202DDC8+(0x14*0x48)       @End Address	{J}
	ldr  r5,=0x0202DDCC       @Ally Units	{U}
	ldr  r6,=0x0202DDCC+(0x14*0x48)       @End Address	{U}
	mov  r7,#0x80

Each:
	ldr  r0,[r5]              @get rom unit pointer
	cmp  r0,#0x00
	beq  NextLoop

	ldrb r2, [r4, #0x2]       @����1 40 0D [����1] 00 [�v���O�����ꏊ XX XX XX 08]
	cmp  r2,#0x00
	beq  CheckClass
                              @CheckUnitID
	ldrb r1,[r0,#0x4]         @get unit id
	cmp  r1,r2
	bne  NextLoop

CheckClass:
	ldr  r0,[r5,#0x4]         @get rom class pointer
	cmp  r0,#0x00
	beq  NextLoop

	ldrb r2, [r4, #0x3]       @����2 40 0D 00 [����2] [�v���O�����ꏊ XX XX XX 08]
	cmp  r2,#0x00
	beq  Change
                              @CheckClassID
	ldrb r1,[r0,#0x4]         @get class id
	cmp  r1,r2
	bne  NextLoop

Change:
	mov  r0,r5
	mov  r1,r7
@	blh  0x08018144   @HandleAllegianceChange	{J}
	blh  0x08018430   @HandleAllegianceChange	{U}

NextLoop:
	add  r5,#0x48
	cmp  r5,r6
	blt  Each

Term:
@	blh  0x08019ecc   @RefreshFogAndUnitMaps    {J}
@	blh  0x08027144   @SMS_UpdateFromGameData   {J}
@	blh  0x08019914   @UpdateGameTilesGraphics  {J}
	blh  0x0801a1f4   @RefreshFogAndUnitMaps    {U}
	blh  0x080271a0   @SMS_UpdateFromGameData   {U}
	blh  0x08019c3c   @UpdateGameTilesGraphics  {U}

	mov	r0, #0
	pop	{pc,r7,r6,r5,r4}
