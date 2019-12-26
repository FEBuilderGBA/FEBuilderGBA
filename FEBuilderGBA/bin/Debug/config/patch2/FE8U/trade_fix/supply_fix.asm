.thumb
.org 0 @paste to e18B4-908

@at 9e872 write 43f01ff8 (replaces bl to 80179d8)
@r0 is chardata, need to keep it
@r1 is inventory, r4 is item data
push {r4-r5,lr}
mov r5,r0 @save this, need it to call 179d8
mov r0,r4 
mov r4,r1
ldr r1, AbilityGetter
mov lr,r1
.short 0xF800
mov r1, #0x10 @unsellable
and r0,r1
cmp r0,#0
bne NoStore
b End
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
