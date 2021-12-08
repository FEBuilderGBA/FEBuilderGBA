.equ pActionStruct, 0x0203A958	@{U}
@.equ pActionStruct, 0x0203A954	@{J}
.equ ActiveUnit,    0x03004E50 	@{U}
@.equ ActiveUnit,    0x03004DF0	@{J}
.equ MemorySlot,    0x30004B8	@{U}
@.equ MemorySlot,    0x30004B0	@{J}
.equ EventEngine,   0x800D07C	@{U}
@.equ EventEngine,   0x800D340	@{J}


.include "_TargetSelectionDefinitions.s"
.include "Definitions.s"

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb

@s1 Unit
@s6 AoETypeID
@s7 Rotate
@sB X,Y

.equ ActionID, 0x01

.global AoE_ASMC 
.type AoE_ASMC, %function 

AoE_ASMC:
push 	{r4,r5, r14}
mov 	 r4, r0

ldr r3, =MemorySlot
ldr  r0, [r3, #4*0x01] @slot 1 as unit 

blh  0x0800BC50   @GetUnitFromEventParam	{U}
@blh  0x0800BF3C   @GetUnitFromEventParam	{J}
cmp  r0, #0x0
beq  End
@Make that unit an Active Unit.
ldr r3, =ActiveUnit
str r0, [r3]

@setting AoE ID.
ldr r3, =MemorySlot
ldrb  r0, [r3, #4*0x06] @slot 6 as AoETypeID
ldr r2, =AoE_RamAddress @ pointer 
ldr r2, [r2]
strb r0, [r2]

ldr r3, =MemorySlot
ldrb  r5, [r3, #4*0x07] @slot 7 as Rotate



ldr   r3, =MemorySlot
str r5, [r3, #4*0x03] @ Store to Memory Slot 3 as rotation 
mov   r2, #4*0x0B       @slot B as X,Y
ldrh  r0, [r3, r2] @XX
add   r2, #0x2
ldrh  r1, [r3, r2] @YY

@store cooridinates in action struct
ldr 	r3, =pActionStruct
mov 	r2, #0x0
strb 	r2, [r3, #0xD]
mov 	r2, #ActionID
strb 	r2, [r3,#0x11]
strb 	r0, [r3, #0x13] @ XX 
strb 	r1, [r3, #0x14] @ YY 
mov r2, r5 @ Rotation 
@ basically the same as AoE_DisplayDamageArea function 
bl AoE_FillMoveMapWithTemplate @ r0 = XX, r1, = YY, r2 = Rotation 
bl AoE_Animation



@mov r0, r4 @ parent 
@bl AoE_GenericEffect

ldr r0, =AoE_MainEvent
mov r1, #1 
blh EventEngine 


bl AoE_ClearGraphics

End:
pop 	{r4,r5}
pop 	{r0}
bx	r0

.ltorg
.align


.align 4
.global AoE_FillMoveMapWithTemplate
.type AoE_FillMoveMapWithTemplate, %function 

AoE_FillMoveMapWithTemplate:

push {r4-r7, lr} 

@given r0 = xx, r1 = yy, r2= rotation, fill movement map with it 
mov r4, r0 
mov r5, r1 
mov r7, r2 @ rotation byte 

bl AoE_GetTableEntryPointer
mov r6, r0 

ldr r0, =0x202E4E0 @ Movement map	@{U}
@ldr r0, =0x202E4DC @ Movement map 	@{J}

ldr r0, [r0] 
mov r1, #0xFF
blh FillMap

ldr r0, =0x202E4F0 @ Backup Movement map	@{U}
@ldr r0, =0x202E4EC @ Backup Movement map	@{J}
ldr r0, [r0] 
mov r1, #0xFF
blh FillMap



ldrb r2, [r6, #RangeMaskByte]
lsl r2, #2 @ x4 
add r2, r7 @rotation byte 
lsl r2, #2 @ 4 words per entry 
ldr r1, =RangeTemplateIndexList
ldr r2, [r1, r2] @ POIN to the RangeMask we want 



@ Arguments: r0 = center X, r1 = center Y, r2 = pointer to template
mov r0, r4 @ XX 
mov r1, r5  @ YY 
bl CreateMoveMapFromTemplate


pop {r4-r7}
pop {r0} 
bx r0 

