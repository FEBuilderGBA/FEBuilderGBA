.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb
.macro blh2 to, reg=r3
  ldr \reg, \to
  mov lr, \reg
  .short 0xf800
.endm
@.equ ClearBG0BG1, 0x804E884 
.equ CheckForStatCaps, 0x080181C8
.equ EventEngine, 0x800D07C
.equ GetUnitStruct, 0x8019430 
.equ UnitAddItem, 0x8017948 
.equ gActionData, 0x203A958 
.equ MemorySlot, 0x30004B8 
@.equ GetUnitByEventParameter, 0x0800BC51	
.equ Defender, 0x203A56C
@.equ CurrentUnit, 0x3004E50
.equ HandleNewItemGetFromDrop, 0x801E098 
.equ Attacker, 0x203A4EC

.equ NewItemGot, 0x8011554 @ proc, unit*, itemShort 
.equ ProcStart, 0x8002c7c
.equ ProcStartBlocking, 0x8002ce0 
.equ UpdateActiveUnitFromBattle, 0x802CC1C 
.equ AreWeStealing, 0x203E1F0+0x62 

.type StealInvFull, %function 
.global StealInvFull
StealInvFull: 
push {r4, lr} 
ldr r3, =AreWeStealing 
ldrb r3, [r3] 
cmp r3, #1 
bne Exit 
mov r4, r0 @ proc 

ldr r0, =Attacker 
ldr r1, =Defender 
add r1, #0x48 
ldrh r1, [r1] 
ldrh r3, [r0, #0x26] 
cmp r3, #0 
bne Handle 
mov r2, r4 
blh UnitAddItem 
b Exit 
Handle: 
@blh UpdateActiveUnitFromBattle @ copy any levelups / exp etc first 


@ in 801E294 it does some cleanup after stealing 
@ UpdateUnitFromBattleUnit at 802C1EC overwrites things 
ldr r0, =Attacker 
ldrb r0, [r0, #0x0B] @ deployment byte 
blh GetUnitStruct 
cmp r0, #0 
beq Exit 
push {r0} 
@r0 unit struct 
ldr r1, =Attacker 
bl SavePartOfUnitFromBattle 
pop {r0} 

@r0 unit 
ldr r1, =Defender 
add r1, #0x48 
ldrh r1, [r1] 
mov r2, r4 
blh HandleNewItemGetFromDrop 

ldr r0, StealFixProc
mov r1, #3 @ root proc 3 
blh ProcStart 

Exit: 
pop {r4} 
pop {r0} 
bx r0 
.ltorg 

SavePartOfUnitFromBattle: @ copied part of 0802C1EC //SaveUnitFromBattle
push {r4-r5, lr} 
mov r4, r0 @ unit 
mov r5, r1 @ attacker 

@ copy stats over 
LDRB r0, [r5, #0x8]
STRB r0, [r4, #0x8]
LDRB r0, [r5, #0x9]
STRB r0, [r4, #0x9]

MOV r0 ,r5
ADD r0, #0x73
LDRB r0, [r0, #0x0]
LDRB r1, [r4, #0x12]
ADD r0 ,r0, R1
STRB r0, [r4, #0x12]
MOV r0 ,r5
ADD r0, #0x74
LDRB r0, [r0, #0x0]
LDRB r1, [r4, #0x14]
ADD r0 ,r0, R1
STRB r0, [r4, #0x14]
MOV r0 ,r5
ADD r0, #0x75
LDRB r0, [r0, #0x0]
LDRB r1, [r4, #0x15]
ADD r0 ,r0, R1
STRB r0, [r4, #0x15]
MOV r0 ,r5
ADD r0, #0x76
LDRB r0, [r0, #0x0]
LDRB r1, [r4, #0x16]
ADD r0 ,r0, R1
STRB r0, [r4, #0x16]
MOV r0 ,r5
ADD r0, #0x77
LDRB r0, [r0, #0x0]
LDRB r1, [r4, #0x17]
ADD r0 ,r0, R1
STRB r0, [r4, #0x17]
MOV r0 ,r5
ADD r0, #0x78
LDRB r0, [r0, #0x0]
LDRB r1, [r4, #0x18]
ADD r0 ,r0, R1
STRB r0, [r4, #0x18]

MOV r0 ,r5
ADD r0, #0x79
LDRB r0, [r0, #0x0]
LDRB r1, [r4, #0x19]
ADD r0 ,r0, R1
STRB r0, [r4, #0x19]
MOV r0 ,r5
ADD r0, #0x7A
LDRB r0, [r0, #0x0]
MOV r3, #0x3A
LDRB r1, [r4, r3]
ADD r0 ,r0, R1
STRB r0, [r4, r3]

MOV r0 ,r4
blh CheckForStatCaps

pop {r4-r5}
pop {r0} 
bx r0 
.ltorg 

StealFixProc: 

