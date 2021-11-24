.thumb 

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


.equ RemoveUnitBlankItems,0x8017984
.equ GetItemAfterUse, 0x08016AEC
.equ EventEngine, 0x800D07C
.equ CurrentUnitFateData, 0x203A958

.equ gActionData, 0x203A958
.equ CurrentUnit, 0x3004E50
.equ CheckEventId,0x8083da8

.equ ClearBG0BG1, 0x0804E884
.equ SetFont, 0x8003D38
.equ Font_ResetAllocation, 0x8003D20  
.equ EndAllMenus, 0x804EF20 

.equ ItemSpecialEffectUsability, ItemSpecialEffectTable+4


.global ItemSpecialEffect
.type ItemSpecialEffect, %function
ItemSpecialEffect:
push {r4-r7, lr} 


ldr r5, =CurrentUnit 
ldr r5, [r5]
ldr r3, =gActionData 
ldrb r4, [r3, #0x12] @ inventory slot # 
lsl r4, #1 @ 2 bytes per inv slot 
add r4, #0x1E 
add r4, r5 @ unit ram address of actual item to load 
ldrb r0, [r4] 
mov r1, r5 

ldr r3, ItemSpecialEffectUsability 
mov lr, r3 
.short 0xF800 
mov r6, r1 @ Table 


blh ClearBG0BG1
@ copied from vanilla 
mov r0, #0 
blh SetFont 
blh Font_ResetAllocation 

RunEvent: 
add r6, #8
ldr r0, [r6] @ event address 
mov r1, #1 
blh EventEngine 


@ reduce durability and remove item if 0 durability 


ldrh r0, [r4]
blh GetItemAfterUse

strh r0, [r4] @ version after use 


mov r0, r5 
blh RemoveUnitBlankItems

ExitItemSpecialEffect:
ldr r1, =CurrentUnitFateData	@these four lines copied from wait routine
mov r0, #0x10 @ Visit @ warning: using 0x1A "Use Item" will cause the item to also be used if it can (eg. vuln will also heal)  
strb r0, [r1,#0x11]

@Effect/Idle Routine Return Value (r0 Bitfield):
@        & 0x01 | Does things? idunno - pause the hand selector ? 
@        & 0x02 | Ends the menu
@        & 0x04 | Plays the beep sound
@        & 0x08 | Plays the boop sound
@        & 0x10 | Clears menu graphics
@        & 0x20 | Deletes E_FACE #0
@        & 0x40 | Nothing (Not handled)
@        & 0x80 | Orrs 0x80 to E_Menu+0x63 bitfield (Ends the menu on next loop call (next frame))
@




blh EndAllMenus

mov r0, #0xb7 


pop {r4-r7} 
pop {r1} 
bx r1

.align
.ltorg

ItemSpecialEffectTable:

