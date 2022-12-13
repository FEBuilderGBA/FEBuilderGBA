.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb 
	.equ CheckEventId,0x8083da8
@ function based on $154F4 
.global AutosaveFunc
.type AutosaveFunc, %function 
AutosaveFunc: 
push {lr} 
ldr r0, =0x202BCF0 
ldrb r0, [r0, #0xF] 
cmp r0, #0 
bne Exit 

bl CountDeployedPlayerUnits 
ldr r2, CurrentPartySize_Link 
ldrb r1, [r2] 
strb r0, [r2] 
cmp r0, r1 
blt Exit @ we don't autosave if we have fewer party members than last time 

ldr r0, QuicksaveToggleFlag 
cmp r0, #0 
beq Skip 
blh CheckEventId 
cmp r0, #0 
bne Exit 
Skip: 


ldr r1, =0x203A958 
mov r0, #9 
strb r0, [r1, #0x16] 
mov r0, #3 
blh 0x80A5A48 @SaveSuspendedGame
Exit: 
pop {r0} 
bx r0 
.ltorg 

	.equ GetUnit, 0x8019430
CountDeployedPlayerUnits: 
	push {r4-r6, lr}	

mov r4,#1 @ deployment id
mov r5,#0 @ counter

mov r6, #50 @50th deployed valid unit - we don't count past this 

LoopThroughUnits:
mov r0,r4
blh GetUnit @ 19430
cmp r0,#0
beq NextUnit
ldr r3,[r0]
cmp r3,#0
beq NextUnit
ldr r1,[r0,#0xC] @ condition word
ldr r2, =#0x401000C @ escaped, benched/dead
tst r1,r2
bne NextUnit
@ if you got here, unit exists and is not dead or undeployed, so go ham

add r5,#1
cmp r5,r6
bge End_LoopThroughUnits
NextUnit:
add r4,#1
cmp r4,#0x3F
ble LoopThroughUnits
End_LoopThroughUnits:
mov r0, r5 
pop {r4-r6}
pop {r1}
bx r1

.ltorg
.equ QuicksaveToggleFlag, CurrentPartySize_Link+4 
.align 
CurrentPartySize_Link: 















