.thumb
@Call 9e5e4 @FE8J

MOV r6 ,r1
MOV r1 ,r4
LDR r1, [r1, #0x4]
LDRB r1, [r1, #0x4]

LDR r2, LuckTable
LDRB r1, [r2, r1]

CMP r0 ,r1
BNE Else
	MOV r1 ,r6
	MOV r6, #0x4
	B Exit

Else:
	MOV r1 ,r6
	MOV r6, #0x2

Exit:
MOV r0 ,r4
ldr r3, =0x0809E5EC
mov pc, r3

.ltorg
.align
LuckTable:
