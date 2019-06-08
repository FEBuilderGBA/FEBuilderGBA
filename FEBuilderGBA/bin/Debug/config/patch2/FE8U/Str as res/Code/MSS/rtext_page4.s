@somewhere in bl range
.thumb
ldr r0, StructPointer
str r0, [r1, #0x14]
ldr r0, =0x80889e9
bx r0

.ltorg
StructPointer:
@POIN HelpTextPg4
