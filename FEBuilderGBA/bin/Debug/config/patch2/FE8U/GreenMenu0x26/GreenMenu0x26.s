.thumb
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.org 0x0

PUSH {r4,r5,r6,lr}   @GuideCommandPaint
MOV r6 ,r0
MOV r4 ,r1

mov r0, #0x26
blh 0x8083da8  @CheckFlag

LSL r0 ,r0 ,#0x18
CMP r0, #0x0
BNE Skip1
    MOV r0 ,r4
    ADD r0, #0x34
    MOV r1, #0x4
    BLH 0x08003E60   @Text_SetColorId
Skip1:
MOV r0 ,r4
ADD r0, #0x3D
LDRB r0, [r0, #0x0]
MOV r5 ,r4
ADD r5, #0x34
CMP r0, #0x2
BNE Skip2
    MOV r0 ,r5
    MOV r1, #0x1
    BLH 0x08003E60   @Text_SetColorId
Skip2:
LDR r0, [r4, #0x30]
LDRH r0, [r0, #0x4]
BLH 0x0800A240   @GetStringFromIndex
MOV r1 ,r0
MOV r0 ,r5
BLH 0x08004004   @Text_AppendString
MOV r0 ,r6
ADD r0, #0x64
LDRB r0, [r0, #0x0]
LSL r0 ,r0 ,#0x1C
LSR r0 ,r0 ,#0x1E
BLH 0x08001C4C   @BG_GetMapBuffer

MOV r1 ,r0
MOV r2, #0x2C
LDSH r0, [r4, r2]
LSL r0 ,r0 ,#0x5
MOV r3, #0x2A
LDSH r2, [r4, r3]
ADD r0 ,r0, R2
LSL r0 ,r0 ,#0x1
ADD r1 ,r1, R0
MOV r0 ,r5
BLH 0x08003E70   @Text_Draw

POP {r4,r5,r6}
POP {r1}
BX r1
