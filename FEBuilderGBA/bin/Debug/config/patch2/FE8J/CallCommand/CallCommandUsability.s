.align 4
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.macro blh_EALiteral to, reg=r3
  ldr \reg, \to
  mov lr, \reg
  .short 0xf800
.endm

@for FE8U
@.equ CheckEventId,0x8083da8		@{U}
@.equ CurrentUnit, 0x3004E50		@{U}
@.equ gChapterData, 0x0202BCF0		@{U}
@.equ GetUnit, 0x8019430			@{U}

@for FE8J
.equ CheckEventId,0x080860D0	@{J}
.equ CurrentUnit, 0x03004DF0	@{J}
.equ gChapterData, 0x0202BCEC	@{J}
.equ GetUnit, 0x08019108		@{J}


@ CallUsability_List, end of file +0
 
.equ SkillTester, CallUsability_List+4

CallCommandMain:
push {r4, lr}

ldr r4, CallUsability_List
sub r4, #0xC

UsabilityLoop:
add r4, #0xC

ldr r0, [r4]
cmp r0, #0xff
beq Usability_False

ldrb r0, [r4, #0x2]
ldr r3, =gChapterData
ldrb r1, [r3, #0xE] @ chapter ID 
cmp r0, #0xFF 
beq CheckUnitID 
cmp r0, r1 
bne UsabilityLoop 

CheckUnitID: 
@ unit ID
ldr r3, =CurrentUnit
ldr r3, [r3] 
ldrb r0, [r4, #0x0] 
cmp r0, #0 
beq CheckClassID 
ldr r1, [r3] @ unit pointer 
ldrb r1, [r1, #4] @ Unit ID 
cmp r0, r1 
bne UsabilityLoop 

CheckClassID: 
ldrb r0, [r4, #0x1] 
cmp r0, #0 
beq CheckSkillID 
ldr r1, [r3, #4] @ class pointer 
ldrb r1, [r1, #4] @ class ID 
cmp r0, r1 
bne UsabilityLoop 

CheckSkillID: 
ldr r2, SkillTester 
cmp r2, #0 
beq CheckHP

mov r0, r3 @ unit struct 
ldrb r1, [r4, #3] @ skill ID 
cmp r1, #0 
beq CheckHP
blh_EALiteral SkillTester 
cmp r0, #1 
bne UsabilityLoop

CheckHP:
ldrb r1, [r4, #0x4] @Required HP
ldrb r0, [r3, #0x13] @Unit->HP
cmp  r0, r1
ble  UsabilityLoop

CheckFlagID:
ldrh r0, [r4, #6] @ Required Flag 
cmp r0, #0 
beq SkipFlagCheck 
blh CheckEventId
cmp r0, #1 
bne UsabilityLoop 
SkipFlagCheck: 

mov r0, r4
bl Get2ndFreeUnit
cmp r0, #0 
beq Usability_False

mov r0, #1 
mov r1, r4 @ table entry we're using 
b Exit 

Usability_False:
mov r0, #3 @ False usability 
mov r1, #0 

Exit: 
pop {r4}
pop {r2} 
bx r2 


@Check if there is a unit that you can call other than yourself.
Get2ndFreeUnit:
push {lr,r4,r5,r6}

Get2ndFreeUnit_FilterStatus:
ldrb r3, [r0,#0x5]
mov r2, #0x1
and r3, r2
beq Get2ndFreeUnit_FilterStatus_NotMoved
mov r6, #0x4F @ moved|dead|undeployed|cantoing|hide
b   Get2ndFreeUnit_CheckStart

Get2ndFreeUnit_FilterStatus_NotMoved:
mov r6, #0xD @ dead|undeployed|hide

Get2ndFreeUnit_CheckStart:
ldr r0, =CurrentUnit
ldr r5, [r0]
mov r4,#0

Get2ndFreeUnit_LoopThroughUnits:
add r4, #1  @ deployment byte 
cmp r4, #0x3F
bgt Get2ndFreeUnit_False

mov r0,r4
blh GetUnit
cmp r0,#0
beq Get2ndFreeUnit_LoopThroughUnits

cmp r5, r0		@Check Current Unit
beq Get2ndFreeUnit_LoopThroughUnits

ldr r3,[r0]
cmp r3,#0
beq Get2ndFreeUnit_LoopThroughUnits
ldr r3,[r0,#0xC] @ condition word

tst r3,r6  @check status
bne Get2ndFreeUnit_LoopThroughUnits
@ if you got here, unit exists and is not dead or undeployed, so go ham
@r0 is Ram Unit Struct 
b   Get2ndFreeUnit_Exit

Get2ndFreeUnit_False:
mov r0, #0x0

Get2ndFreeUnit_Exit:

pop {r4,r5,r6}
pop {r1}
bx r1

.ltorg
.align 4

CallUsability_List:

