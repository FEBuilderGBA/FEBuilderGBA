.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb


@.equ ClearBG0BG1, 0x804E884 
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
@ in 801E294 it does some cleanup after stealing 
@ UpdateUnitFromBattleUnit at 802C1EC overwrites things 
ldr r0, =Attacker 
ldrb r0, [r0, #0x0B] @ deployment byte 
blh GetUnitStruct 
cmp r0, #0 
beq Exit 
@r0 unit 
ldr r1, =Defender 
add r1, #0x48 
ldrh r1, [r1] 
mov r2, r4 
blh HandleNewItemGetFromDrop @ edited to return new proc pointer 

ldr r0, StealFixProc
mov r1, #3 @ root proc 3 
blh ProcStart 

Exit: 
pop {r4} 
pop {r0} 
bx r0 
.ltorg 
StealFixProc: 

