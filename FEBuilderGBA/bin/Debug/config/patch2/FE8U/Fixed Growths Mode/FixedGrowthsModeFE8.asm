.thumb
@ Start Conditions: r0 = growth rate, r4 = 0

Reduce1:
cmp r0,#0x64
ble Verify
sub r0,#0x64
add r4,#0x1
b Reduce1

Verify:
push {r1}
push {r2}
ldr r1,=#0x0203A4EC @ pointer to attacker data
cmp r1,r7
beq Compute
ldr r1,=#0x0203A56C @ pointer to defender data
cmp r1,r7
bne Return

Compute:
ldr r2,[r1,#0x4]
add r2,#0x29
ldr r2,[r2] @ class skill 2
ldr r3,=#0x1
and r2,r3

add r1,#0x8
ldrb r1,[r1] @ grabbing level after level-up

cmp r2,#0x1 @ checking if unit is promoted
bne cont
add r1,#0x13

cont:
sub r1,#0x2
mul r1,r0
add r1,#0x32 @ adding a constant 50 to make the first level not always empty when all growths < 100

Reduce2:
cmp r1,#0x64
blt Compute2
sub r1,#0x64
b Reduce2

Compute2:
add r1,r0
cmp r1,#0x64
bge procstat

ldr r0,=#0x0
b Return

procstat:
ldr r0,=#0x64


@ Return Conditions: r4 = (growth rate) / 100 (rounded down), r0 = 100 or 0 (representing whether the undetermined part of the growth will proc)
Return:
pop {r2}
pop {r1}
ldr r3,ReturnTo
bx r3


.align 4
ReturnTo:
.long 0x0802B9B0|1
