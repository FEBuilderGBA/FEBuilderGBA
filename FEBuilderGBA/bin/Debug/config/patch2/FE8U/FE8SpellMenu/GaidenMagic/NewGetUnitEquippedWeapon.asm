.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.equ CanUnitUseAsWeapon, 0x08016574

@ hooks at 16b2c with jumpToHack

mov r5, #0x0

CheckSelectedSpell:
ldr r1, SelectedSpellPointer
ldrh r1, [r1, #0x0]
mov r0, r6
blh CanUnitUseAsWeapon
lsl r0, r0, #0x18
asr r0, r0, #0x18
cmp r0, #0x1
bne StartInventorySearchLoop
ldr r1, SelectedSpellPointer
ldrh r0, [r1, #0x0]
b Exit

StartInventorySearchLoop:
	lsl r1, r5, #0x1
	mov r0, r6
	add r0, #0x1E
	add r4, r0, r1
	ldrh r1, [r4, #0x0] @load weapon 
	mov r0, r6
	blh CanUnitUseAsWeapon
	lsl r0, r0, #0x18
	asr r0, r0, #0x18
	cmp r0, #0x1
	bne KeepLooping
	ldrh r0, [r4, #0x0]
	b Exit

KeepLooping:
	add r5, #0x1
	cmp r5, #0x4
	ble StartInventorySearchLoop
	mov r0, #0x0

Exit:
	ldr r1, =0x8016B53
	bx r1
	
.ltorg
.align

SelectedSpellPointer:
@POIN SelectedSpellPointer
