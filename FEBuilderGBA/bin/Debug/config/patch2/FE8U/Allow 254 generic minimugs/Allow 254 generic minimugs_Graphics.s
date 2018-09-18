@Call 0x5F6C @FE8U
.thumb
PUSH {lr}
MOV r1 ,#0xff
AND r0 ,r0, r1
LSL r0 ,r0 ,#0x3    @ ID*8
LDR r1, MiniMags_Table
ADD r0 ,r0, R1
LDR r0, [r0, #0x0]  @MiniMags_Table->graphics
POP {r1}
BX r1

.ltorg
.align
MiniMags_Table:
@struct MiniMags_Table{
@	void*	graphics
@	void*	palette
@@} //sizeof() == 8
