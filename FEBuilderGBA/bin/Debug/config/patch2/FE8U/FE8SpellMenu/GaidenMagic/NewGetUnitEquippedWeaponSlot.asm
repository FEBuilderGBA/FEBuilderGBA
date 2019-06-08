.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.equ CanUnitUseWeapon, 0x08016750

@hooks at 16B5C

mov r4, #0x0

CheckSelectedSpell:
ldr r1, SelectedSpellPointer
ldrh r1, [r1, #0x0]
mov r0, r5
blh CanUnitUseWeapon
lsl r0, r0, #0x18
asr r0, r0, #0x18
cmp r0, #0x1
bne LoopStart
ldr r1, SelectedSpellPointer
mov r0, #0x9
b Exit

LoopStart:
	lsl r1, r4, #0x1
	mov r0, r5
	add r0, #0x1E
	add r0, r0, r1
	ldrh r1, [r0, #0x0]
	mov r0, r5
	blh CanUnitUseWeapon
	lsl r0, r0, #0x18
	asr r0, r0, #0x18
	cmp r0, #0x1
	bne KeepLooping

	mov r0, r4
	b Exit

KeepLooping:
	add r4, #0x1
	cmp r4, #0x4
	ble LoopStart
	mov r0, #0x1
	neg r0, r0

Exit:
	ldr r1, =0x8016B85
	bx r1

.ltorg
.align

SelectedSpellPointer:
@POIN SelectedSpellPointer
