.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb 
.equ MemorySlot0, 0x30004B8 
.equ GetUnitByCharId, 0x0801829C
.equ GetUnitByEventParameter, 0x800bc50
.equ CopyUnitToBattleStruct, 0x0802A584
.equ CheckForLevelUp, 0x0802BA28
.equ SaveUnitFromBattle, 0x0802C1EC
.equ EnsureNoUnitStatCapOverflow, 0x80181c9


PUSH {r4-r7,lr}
SUB SP, #0x80
LDR    r0, =MemorySlot0    @FE8U MemorySlot0
LDR    r5,[r0,#0x20]    @FE8U MemorySlot8 - # of levels 
LDR    r0,[r0,#0x1C]    @FE8U MemorySlot7 - Unit ID
BLH GetUnitByEventParameter @0x800bc50
cmp r0, #0 
beq Exit

ldrb r1, [r0,#0xB] @Do not use for enemies
cmp r1, #0x40
bge Exit

ldrb r1, [r0, #0x9] @Get unit exp
cmp r1, #0x64      @check EXP:--
bge Exit           @level caps 

cmp  r5, #0x0      @ what's LV+0?
beq Exit

MOV r6 ,r0        @Copy unit address
 @Get unit level
LDRB r4, [r6, #0x8] @ current lvl
add r5, r4 @ # of lvls + current lvl = final lvls

LoopStart:
MOV r7, SP
MOV r0, SP
MOV r1 ,r6
BLH CopyUnitToBattleStruct
LDRB r0, [r7, #0x9]
ADD r0, #0x64
STRB r0, [r7, #0x9]
MOV r0, SP
BLH CheckForLevelUp

LDRB r0, [r7, #0x9] @Get unit exp
CMP r0, #0x64
BLT NotMaxLevel @If the unit already reached its level cap, do not give more levels
MOV r0 ,r6
MOV r1, SP
BLH SaveUnitFromBattle
B MaxLevel

NotMaxLevel:
MOV r0 ,r6
MOV r1, SP
BLH SaveUnitFromBattle
ADD r0 ,r4, #0x1
LSL r0 ,r0 ,#0x18
LSR r4 ,r0 ,#0x18
CMP r4 ,r5
BCC LoopStart


MOV r0, #0x0 @Set EXP to 0
STRB r0, [r6, #0x9]


MaxLevel:
mov r0, r6 @ unit 
blh EnsureNoUnitStatCapOverflow


Exit:

ADD SP, #0x80
POP {r4,r5,r6,r7}
POP {r0}
BX r0

.ltorg
.align 4
