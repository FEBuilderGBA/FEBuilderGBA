.thumb 
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


@.equ CurrentUnit, 0x03004E50	@{U}
.equ CurrentUnit, 0x03004DF0	@{J}
@.equ MemorySlot,0x030004B8	@{U}
.equ MemorySlot,0x030004B0	@{J}
@.equ EventEngine, 0x0800D07C	@{U}
.equ EventEngine, 0x0800D340	@{J}
@.equ CurrentUnitFateData, 0x0203A958	@{U}
.equ CurrentUnitFateData, 0x0203A954	@{J}
@.equ CheckEventId, 0x08083da8	@{U}
.equ CheckEventId, 0x080860D0	@{J}
@.equ gChapterData, 0x0202BCF0	@{U}
.equ gChapterData, 0x0202BCEC	@{J}

push {r4-r6, lr}

mov r6, #3 @ False. Default - Menu false usability is 3 

ldr r4, =CurrentUnit 
ldr r4, [r4] 
cmp r4, #0 
beq ReturnValue

ldr r0, [r4, #4] @ Class pointer 
ldrb r0, [r0, #4] @ ClassID 
lsl r0, #1 @ 2 bytes / choices per class 

@ldr r2, =0x080CC7D0 @ POIN PromotionTable	@{U}
ldr r2, =0x080D14EC @ POIN PromotionTable	@{J}
ldr r2, [r2] @ PromotionTable (Vanilla: 0x895DFA4)
ldrh r0, [r2, r0] @ Are both choices 0? 
cmp r0, #0 
beq ReturnValue @ If no possible class to promote into, then return false. 

ldr r5, PromotionMenuList
sub r5, #8 

UsabilityLoop:
ldr r3, =0xFFFFFFFF 
add r5, #8
ldr r0, [r5] 
cmp r0, r3 
bne DoNotTerminate
ldr r0, [r5, #4]
cmp r0, r3  
beq ReturnValue

DoNotTerminate:

ldrb r0, [r5, #0] @ UnitID 
cmp r0, #0 
beq SkipUnitCheck 
ldr r1, [r4] @ unit pointer 
ldrb r1, [r1, #4] 
cmp r0, r1 
bne UsabilityLoop 
SkipUnitCheck: 

ldrb r0, [r5, #1] @ ClassID 
cmp r0, #0 
beq SkipClassCheck 
ldr r1, [r4, #4] @ class pointer 
ldrb r1, [r1, #4] 
cmp r0, r1 
bne UsabilityLoop 
SkipClassCheck: 

ldrb r0, [r5, #2] @ Required Level 
cmp r0, #0 
beq SkipLevelCheck
ldrb r1, [r4, #8] @ Current Level 
cmp r0, r1 
bgt UsabilityLoop 
SkipLevelCheck:

ldr r2, =gChapterData @gChapterData 
ldrb r1, [r2, #0xE] 
ldrb r0, [r5, #3] @ Lowest chapter 
ldrb r2, [r5, #4] @ Highest chapter 
cmp r0, #0xFF 
beq SkipChCheck 
cmp r1, r0 @ Current chapter vs lowest allowed chapter 
blt UsabilityLoop
cmp r1, r2 @ current chapter vs highest allowed chapter 
bgt UsabilityLoop 
SkipChCheck: 

ldrh r0, [r5, #6] @ Required Flag 
cmp r0, #0 
beq SkipFlagCheck
blh CheckEventId
cmp r0, #1
bne UsabilityLoop
SkipFlagCheck:

mov r6, #1 @ True 

ldrb r2, [r5, #5] 
mov r1, #1 
and r2, r1 @ Boolean: Exception to usability if true. Requirement if false. 

cmp r2, #1 @ If exception to the rule, make this case false instead of true. 
bne UsabilityLoop 
mov r6, #3 @ Exception to the rule 
b UsabilityLoop

ReturnValue:
mov r0, r6  

End: 

pop {r4-r6}
pop {r1}
bx r1 

.ltorg 
.align 4

PromotionMenuList:

