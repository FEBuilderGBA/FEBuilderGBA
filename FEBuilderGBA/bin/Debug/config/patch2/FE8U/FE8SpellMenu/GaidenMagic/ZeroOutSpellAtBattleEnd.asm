.thumb

@hooks at 0802c1f4

ldr r2, SelectedSpellPointer
mov r1, #0x0
str r1, [r2, #0x0]

ldrb r0, [r5, #0x8]
strb r0, [r4, #0x8]
ldrb r0, [r5, #0x9]
strb r0, [r4, #0x9]
ldrb r0, [r5, #0x13]
strb r0, [r4, #0x13]
ldr r0, [r5, #0xC]
str r0, [r4, #0xC]
ldr r1, =0x802C203
bx r1

.ltorg
.align

SelectedSpellPointer:
@POIN SelectedSpellPointer
