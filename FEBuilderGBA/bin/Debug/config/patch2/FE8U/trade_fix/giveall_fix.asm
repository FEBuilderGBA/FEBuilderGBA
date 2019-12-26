.thumb
.org 0 @paste to e1908 -

@originally at 9a554, write 47f0d8f9
@also need to change 1948a to d219
mov r7, #0x1e @item slot (r7 had the number of items total)
Loop:
ldr r0,[r5,r7]
bl Give_func
mov r0,r5
mov r1,#0
ldr r2, StoreFromInv
mov lr,r2
.short 0xF800
ReturnSkipped:
add r4,#1
cmp r4,#5 @(cmp 5 instead of r5, may be a couple extra loops but who cares)
bge EndLoop
add r0,r4,r6
cmp r0,#0x63
ble Loop
EndLoop:
ldr r0,ReturnTo
bx r0

Give_func:
push {r4,lr}
mov r4, r0
ldr r1, AbilityGetter
mov lr,r1
.short 0xF800
mov r1, #0x10 @unsellable
and r0,r1
cmp r0,#0
bne NoStore
b End
NoStore:
add r7, #2
pop {r4}
pop {r0}
b ReturnSkipped
End:
mov r0,r4
ldr r1,StoreFunc
mov lr,r1
.short 0xF800
pop {r4}
pop {r1}
bx r1
.align
AbilityGetter:
.long 0x0801756c
ReturnTo:
.long 0x0809a56e+1
StoreFunc:
.long 0x8031594
StoreFromInv:
.long 0x8019484
