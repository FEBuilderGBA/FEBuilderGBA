.thumb

DrawSkillIcon:
push    {r4,r5,r14}				@080036BC
mov     r4,r0				@080036BE
mov     r0,r1				@080036C0
mov     r5,r2				@080036C2
cmp     r0,#0x0				@080036C4
bge     loc_0x80036D8				@080036C6
mov     r0,#0x0				@080036C8
strh    r0,[r4]				@080036CA
strh    r0,[r4,#0x2]				@080036CC
mov     r1,r4				@080036CE
add     r1,#0x40				@080036D0
strh    r0,[r1]				@080036D2
add     r1,#0x2				@080036D4
b       loc_0x8003704				@080036D6
loc_0x80036D8:
bl      DrawAlternateIcon				@080036D8
add     r0,r0,r5				@080036DC
lsl     r0,r0,#0x10				@080036DE
lsr     r1,r0,#0x10				@080036E0
mov     r2,#0x80				@080036E2
lsl     r2,r2,#0x9				@080036E4
add     r0,r0,r2				@080036E6
strh    r1,[r4]				@080036E8
lsr     r1,r0,#0x10				@080036EA
add     r0,r0,r2				@080036EC
strh    r1,[r4,#0x2]				@080036EE
mov     r2,r4				@080036F0
add     r2,#0x40				@080036F2
lsr     r1,r0,#0x10				@080036F4
mov     r3,#0x80				@080036F6
lsl     r3,r3,#0x9				@080036F8
add     r0,r0,r3				@080036FA
lsr     r0,r0,#0x10				@080036FC
strh    r1,[r2]				@080036FE
mov     r1,r4				@08003700
add     r1,#0x42				@08003702
loc_0x8003704:
strh    r0,[r1]				@08003704
pop     {r4,r5}				@08003706
pop     {r0}				@08003708
bx      r0				@0800370A

DrawAlternateIcon:
push {r4,r5,lr}
mov r4,r0
ldr r0, =0x2026a90
lsl r1,r4,#2
add r5,r1,r0
ldrb r0, [r5,#1]
cmp r0, #0
b loc_3670
@ beq loc_3670
ldrb r0, [r5]
cmp r0, #0xfe
bhi loc_36a4
add r0, #1
strb r0, [r5]
b loc_36a4
.ltorg
loc_3670:
add r0, #1
strb r0, [r5]
mov r0, r4
blh 0x8003624
add r0, #1
strb r0, [r5,#1]
lsl r4, #7
ldr r0, IconGraphic
add r4, r0
ldrb r0, [r5,#1]
blh 0x8003610
mov r1,r0
lsl r1, #0x10
lsr r1, #0xb
mov r2, #0xc0
lsl r2, #0x13
ldr r0, =0x1ffe0
and r1, r0
add r1, r2
mov r0, r4
mov r2, #0x80
blh 0x8002014
loc_36a4:
ldrb r0, [r5, #1]
blh 0x8003610
pop {r4-r5}
pop {r1}
bx r1

.ltorg
IconGraphic:
@POIN IconGraphic
