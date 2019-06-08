.thumb

@hooks at 080171ec

mov r7, #0x0
cmp r1, #0x0
blt NotSelectingAnOption

SelectNthSpell:
push {r2,r7}
ldr r7, =0x0203F082
ldrh r7, [r7]
cmp r7, #0x1
bne LoadNthItemFromInventory
mov r7, r1
ldr r3, SpellsGetter
mov lr, r3
.short 0xf800
mov r1, r7
ldrb r2, [r0, r1] @get nth spell in list
cmp r2, #0x0
beq LoadNthItemFromInventory
mov r0, #0xFF
lsl r0, r0, #0x8
orr r0, r2
b SuccessfullySelectedSpellOrWeapon

LoadNthItemFromInventory:
lsl r1, r1, #0x1
add r0, #0x1E
add r0, r0, r1
ldrh r0, [r0]

SuccessfullySelectedSpellOrWeapon:
pop {r2,r7}
ldr r3, =0x80171fb
bx r3

NotSelectingAnOption:
ldr r5, =0x0203F082
ldrh r5, [r5]
cmp r5, #0x0
beq LoadEquippedWeapon
mov r5, #0x0

ldr r3, SpellsGetter
mov lr, r3
.short 0xf800
ldrb r4, [r0]
cmp r4, #0x0
bne FoundUsableSpell

LoadEquippedWeapon:
ldr r3, =0x8017201
bx r3

FoundUsableSpell:
mov r0, #0xFF
lsl r0, r0, #0x8
orr r4, r0
ldr r3, =0x8017209
bx r3

.ltorg
.align

SpellsGetter:
@POIN SpellsGetter