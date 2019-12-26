@send to storage:
@break at 4f3d2, jump to end push and pop
@then jump to 4f286 right after popping, nothing happens

.thumb
.org 0 @paste to e1964-9b8 and put hook at 1e19e (c3f0e1fb)

@item data in r0
push {lr}

@check uses
mov r1, #0x80
lsl r1, #8    @0x8000, aka top bit set
cmp r0,r1
bge Skip

@original routine
ldr r1, =0x8031594
mov lr,r1
.short 0xf800 @bl 801e188
pop {r1}
bx r1

Skip:
ldr r0, MuteCheck
ldrb r0,[r0]
lsl r0,r0,#0x1e
cmp r0,#0
blt End
mov r0, #0x6c       @sound ID (bzzt)
ldr r3, PlaySound    @play sound routine
mov lr,r3
.short 0xf800

End:
pop {r1}
pop {r4,r5}
pop {r1}
pop {r4-r7}
pop {r1}
ldr r1, ReturnSkip
bx r1

.align
PlaySound:
.long 0x080d01fc
MuteCheck:
.long 0x0202bc31
ReturnSkip:
.long 0x0804f287
