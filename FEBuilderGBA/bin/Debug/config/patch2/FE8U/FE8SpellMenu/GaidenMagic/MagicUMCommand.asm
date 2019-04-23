.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ GetItemAttributes, 0x0801756C
.equ CanUnitUseWeapon, 0x08016750
.equ MakeTargetListForWeapon, 0x080251B4
.equ GetTargetListSize, 0x0804FD28

push {r3-r7,lr}
ldr r0, =0x03004E50
ldr r4, [r0] 			@save active unit in r4
ldr r1, [r4, #0xC] 		@turn info
mov r0, #0x40			@has not moved
and r0, r1				@
cmp r0, #0x0
bne False

@check if active unit can use spells (2D 2E 2F)
CheckMagic:
mov r0, r4
mov r1, #0x2D
ldrb r1, [r0, r1] @anima
cmp r1, #0x0
bgt CheckHP
mov r1, #0x2E
ldrb r1, [r0, r1] @light
cmp r1, #0x0
bgt CheckHP
mov r1, #0x2F
ldrb r1, [r0, r1] @dark
cmp r1, #0x0
bgt CheckHP
b False

@check if unit is above 1 hp
CheckHP:
mov r0, r4
ldrb r1, [r0, #0x13] @current HP
cmp r1, #0x1
ble False

@now check if can attack
CheckAttack:

mov r6, #0x0
ldr r7, SpellsGetter
mov lr, r7
.short 0xf800
mov r7, r0		@save spells
mov r2, #0x0
ldrb r5, [r0, r2] @load spell from buffer
cmp r5, #0x0
beq False

StartLoop:
mov r2, #0xFF
lsl r2, r2, #0x8
orr r5, r2
mov r0, r5
blh GetItemAttributes
mov r1, #0x1
and r1, r0
cmp r1, #0x0
beq MoveNext
mov r0, r4
mov r1, r5
blh CanUnitUseWeapon
lsl r0, r0, #0x18
cmp r0, #0x0
beq MoveNext
mov r0, r4
mov r1, r5
blh MakeTargetListForWeapon
blh GetTargetListSize
cmp r0, #0x0
bne True

MoveNext:
add r6, #0x1
cmp r6, #0x4
bgt False
ldrb r5, [r0, r2] @load spell from buffer
cmp r5, #0x0
bne StartLoop

True:
mov r0, #0x1
b EndFunc

False:
mov r0, #0x3

EndFunc:
pop {r3-r7}
pop {r1}
bx r1

.ltorg
.align

SpellsGetter:
@POIN SpellsGetter
