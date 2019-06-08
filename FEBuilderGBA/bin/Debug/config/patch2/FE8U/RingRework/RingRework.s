@BattleComputeBuffStatus
@ORG 0802AD00  @FE8J
@ORG 0802AD90  @FE8U
@
@by 7743
@
.thumb

push {r4,lr}
mov  r1, #0x30
ldrb r1, [r0, r1]
mov  r2,#0xF
and  r1,r2
lsl  r1,r1,#3   @r1*8
ldr  r3, RingReworkTable
add  r3,r1
mov  r1,#0x66   @回避 0x66
add  r4,r0,r1
mov  r1,#0x5a   @攻撃 0x5a
add  r0,r0,r1
Loop:
ldrb r1,[r3]
ldrh r2,[r0]
add  r2,r1
strh r2,[r0]
add r3,#0x1
add r0,#0x2
cmp r0,r4
ble Loop
Exit:
pop {r4}
pop {r0}
bx r0
nop

.ltorg
RingReworkTable:
