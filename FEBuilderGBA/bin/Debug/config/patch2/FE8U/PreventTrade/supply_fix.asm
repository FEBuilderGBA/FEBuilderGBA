.thumb
.org 0 @paste to e18B4-908
.align 4
	.equ MemorySlot3,0x30004C4    @item ID to give @[0x30004C4]!!?
@at 9e872 write 43f01ff8 (replaces bl to 80179d8)
@r0 is chardata, need to keep it
@r1 is inventory, r4 is item data
push {r4-r5,lr}
mov r5,r0 @save this, need it to call 179d8
mov r0,r4 
mov r4,r1

ldr r2, =MemorySlot3 
str r0, [r2] @[0x30004C4]!!?

mov r2, #0 @Counter 
PreventTradingLoop:
ldr r3, =PreventTradingList 
ldrb r1, [r3, r2] 
cmp r1, #0x0 
beq End
add r2, #1 
lsl r3, r0, #24 
lsr r3, #24 
cmp r3, r1 @
beq NoStore
b PreventTradingLoop


NoStore:
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
pop {r4,r5}
pop {r0}
ldr r0, ReturnSkip
bx r0
End:
mov r0,r5
ldr r1,StoreFunc
mov lr,r1
.short 0xF800
pop {r4,r5}
pop {r1}
bx r1
.align
AbilityGetter:
.long 0x0801756c
PlaySound:
.long 0x080d01fc
ReturnSkip:
.long 0x0809e94f
MuteCheck:
.long 0x0202bc31
StoreFunc:
.long 0x80179d8
