.thumb

.equ origin, 0x0801876C	@UnitHasMagicRank
.equ GetUnitEquippedWeapon, . + 0x080168D0 - origin
.equ GetItemAttributes, . + 0x08017314 - origin

PUSH {r4,lr}
mov r4, r0
BL GetUnitEquippedWeapon
cmp r0, #0x0
bne DependItem
DependClass:
ldr r0, [r4, #0x2c] @Unit->Staff
cmp r0, #0x0        @–‚–@‚ªŽg‚¦‚È‚¢
beq Exit
mov r0, #0x1
b   Exit

DependItem:
BL GetItemAttributes
MOV r1, #0x2
AND r0 ,r1
LSR r0 ,r0 ,#0x1
Exit:
POP {r4,pc}
