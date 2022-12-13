
;Source code for AffinityTableCode.bin by EgalLau37

.org 0x8CC2258+0x18
.dw AffinityCode

.org FreespaceArea
AffinityCode:
ldr r0,=0x200310C
ldr r0,[r0,0xC]
ldr r0,[r0];loads current unit ROM pointer from RAM
ldrb r0,[r0,0x9];loads affinity byte from unit data
lsl r0,r0,0x1
ldr r1,=TextIDTable
ldrh r0,[r1,r0]
mov r1,r4
add r1,0x4C
strh r0,[r1]
bx r14
.pool

TextIDTable:
.dh 0x26F;affinity 0/null/no affinity
.dh 0x270
.dh 0x271
.dh 0x272
.dh 0x273
.dh 0x274
.dh 0x275
.dh 0x276

.close