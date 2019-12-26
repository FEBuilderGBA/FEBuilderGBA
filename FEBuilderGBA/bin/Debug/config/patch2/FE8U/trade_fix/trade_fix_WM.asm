.thumb
.org 0 @paste to e184c-8b4

@at 9b550 write 46f07cf9
@need to save r0 actually

@change r1,r2 to r0,r1
@change r4,r5 to r2,r3

push {r4-r6,lr}
mov r6,r0    @save r0 for later
mov r5,r1    @save inventory ptrs
ldrh r0,[r4] @get item data
ldr r1, AbilityGetter
mov lr,r1
.short 0xF800
mov r1, #0x10 @unsellable
and r0,r1
cmp r0,#0
bne NoTrade
ldrh r0,[r5]
ldr r1, AbilityGetter
mov lr,r1
.short 0xF800
mov r1,#0x10
and r0,r1
cmp r0,#0
bne NoTrade
ldrh r1,[r5]
ldrh r0,[r4]
strh r1,[r4]
strh r0,[r5]
mov r0,r6
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
pop {r4-r6}
pop {r0}
pop {r4-r6}
pop {r0}
ldr r0, ReturnSkip
bx r0
End:
pop {r4-r6}
pop {r1}
bx r1
.align
AbilityGetter:
.long 0x0801756c
PlaySound:
.long 0x080d01fc
ReturnSkip:
@.long 0x0809be1b
.long 0x0809bc89
MuteCheck:
.long 0x0202bc31
