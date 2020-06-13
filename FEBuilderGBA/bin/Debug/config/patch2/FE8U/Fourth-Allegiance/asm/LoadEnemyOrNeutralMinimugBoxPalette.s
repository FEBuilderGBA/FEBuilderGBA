.thumb
.org 0x0
cmp r0, #0x80
bne LoadNeutralPalette
ldr r4, =0x8A173AC
b GoBack

LoadNeutralPalette:
ldr r4, NeutralMinimugBoxPalette

GoBack:
ldr r3, =0x808C305
bx r3

.align
.pool
NeutralMinimugBoxPalette:
