.thumb
LDRB r0, [r0, #0x4]   @bisha.dmp@EA
LSL r0 ,r0 ,#0x4
LDR r1, =0x0203E880 (gBWLDataArray )
ADD r0 ,r0, r1
LDRB r0, [r0, #0xB]   @ pointer:0203E88B
LSR r0 ,r0 ,#0x2
CMP r0, #0x19
BLE _label1
   MOV r0, #0x19
label1:
ADD r2 ,r2, R0
LDR r1, [r4, #0x4]
LDRB r1, [r1, #0x4]
LDR r0, Table
LDRB r1, [r0, r1]
ADD r2 ,r2, r1
STRH r2, [r3, #0x0]
LDR r0, =0x0802ABBE
MOV PC, r0

.align
.ltorg
Table:
