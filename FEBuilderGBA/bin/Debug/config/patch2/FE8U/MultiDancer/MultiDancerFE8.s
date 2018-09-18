@Call 08058608 (FE8J)
@Call 080576BC (FE8U)
@r0    temporary
@r1    temporary
@r3    temporary
@r4    keep
@r5    keep
@r6    (FE8J) current unit struct [0x00: unit pointer][0x04: class pointer]
@r7    keep
@r8    keep
@r9    keep
@r10   (FE8U) current unit struct [0x00: unit pointer][0x04: class pointer]

.thumb
.org 0

mov r3, r10              @for FE8U
@mov r3,r6  @for FE8J

ldr r0, [r3, #0x4]
ldrb r0, [r0, #0x4] @ Get ROM Class Struct
ldr r3, MultiDancerClassTable

Loop:
ldrb r1, [r3]
cmp r1, #0x00
beq NotDancer
cmp r0, r1
beq Dancer

add r3, #0x1
b   Loop

Dancer:
ldr	r1,=0x080576C6+1 @For FE8U
@ldr	r1,=0x08058610+1 @For FE8J
b Exit

NotDancer:
ldr	r1,=0x080576cc+1 @For FE8U
@ldr	r1,=0x08058616+1 @For FE8J
b Exit

Exit:
bx	r1

.ltorg
MultiDancerClassTable:
@list of the Data List sizeof 1bytes  0x00==TERM
@struct
@byte classid
