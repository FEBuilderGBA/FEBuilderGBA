.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

PUSH {lr}

mov r0, #0x26
blh 0x08083d80       @フラグを立てる関数

LDR r0, shinan_table
LDR r1, =0x0202BCF0
LDRB r1, [r1, #0xE]  @ ChapterData->MapID
LSL r1 ,r1 ,#0x2
LDR r0, [r0, r1]
MOV r1, #0x1

blh 0x0800d07c  @ イベント命令を動作させる関数

MOV r0, #0x17
POP {r1}
bx r1

.ltorg
shinan_table:
