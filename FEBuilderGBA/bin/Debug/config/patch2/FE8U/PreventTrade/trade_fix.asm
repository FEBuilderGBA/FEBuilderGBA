.thumb
.org 0 @paste to e17e8
.align 4
	.equ MemorySlot3,0x30004C4    @item ID to give @[0x30004C4]!!?
@at 2d760 write b4 f0 42 f8 00 00 00 00

push {r4-r5,lr}
ldr r0, =MemorySlot3 
str r0, [r0] @[0x30004C4]!!?


mov r4,r1    @save inventory ptrs
mov r5,r2

ldrb r0,[r4] @get item data

mov r2, #0 @Counter 
ldr r3, =PreventTradingList 
PreventTradingLoopA:
ldrb r1, [r3, r2] 
cmp r1, #0x0 
beq ContinueA
add r2, #1 
cmp r0, r1 @
beq NoTrade
b PreventTradingLoopA
ContinueA:
ldrb r0,[r5]

mov r2, #0 @Counter 
ldr r3, =PreventTradingList 
PreventTradingLoopB:
ldrb r1, [r3, r2] 
cmp r1, #0x0 
beq ContinueB
add r2, #1 
cmp r0, r1 @
beq NoTrade
b PreventTradingLoopB
ContinueB: 

ldrh r1,[r5]
ldrh r0,[r4]
strh r1,[r4]
strh r0,[r5]
b End
NoTrade:
ldr r0, MuteCheck
ldrb r0,[r0]
lsl r0,r0,#0x1e
cmp r0,#0
blt Mute
mov r0,#0x6c
ldr r1, PlaySound
mov lr,r1
.short 0xF800
Mute:
pop {r4-r5}
pop {r0}
pop {r4}
pop {r0}
ldr r0, ReturnSkip
bx r0
End:
pop {r4-r5}
pop {r1}
bx r1
.align
AbilityGetter:
.long 0x0801756c
PlaySound:
.long 0x080d01fc
ReturnSkip:
.long 0x0802da83
MuteCheck:
.long 0x0202bc31
