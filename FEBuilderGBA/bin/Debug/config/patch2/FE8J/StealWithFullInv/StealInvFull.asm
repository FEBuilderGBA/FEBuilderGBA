.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb


@.equ GetUnitStruct, 0x8019430	@{U}
.equ GetUnitStruct, 0x08019108	@{J}

@.equ UnitAddItem, 0x8017948		@{U}
.equ UnitAddItem, 0x080176F0		@{J}

@.equ Defender, 0x203A56C		@{U}
.equ Defender, 0x0203A568		@{J}

@.equ HandleNewItemGetFromDrop, 0x801E098	@{U}
.equ HandleNewItemGetFromDrop, 0x0801DCF4	@{J}

@.equ Attacker, 0x203A4EC	@{U}
.equ Attacker, 0x0203A4E8	@{J}

@.equ ProcStart, 0x8002c7c	@{U}
.equ ProcStart, 0x08002BCC	@{J}

@.equ AreWeStealing, 0x203E1F0+0x62	@{U}
.equ AreWeStealing, 0x0203E1EC+0x62	@{J}

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

