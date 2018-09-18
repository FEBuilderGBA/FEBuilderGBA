@Call 0x5F9C  @FE8U
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb
PUSH {lr}
MOV r2 ,#0xff
AND r0 ,r0, r2
LSL r0 ,r0 ,#0x3    @ ID*8
LDR r2, MiniMags_Table
ADD r0 ,r0, R2
LDR r0, [r0, #0x4]  @MiniMags_Table->palette
LSL r1 ,r1 ,#0x5
MOV r2, #0x20
BLH 0x08000db8   @FE8U Write_Palette r0=pointer to palette
POP {r0}
BX r0

.ltorg
.align
MiniMags_Table:
@struct MiniMags_Table{
@	void*	graphics
@	void*	palette
@} //sizeof() == 8
