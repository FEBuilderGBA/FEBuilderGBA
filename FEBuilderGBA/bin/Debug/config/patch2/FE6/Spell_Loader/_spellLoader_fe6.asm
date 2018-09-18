.thumb
.org 0
@branch away at 8049dc8 (can't use jumpToHack, we need to bx via r2)
@at 8049dc8
@SHORT $4a00 $4710
@POIN HackLocation+1

ldrh r2,[r3,#4] @get melee anim
ldrh r0,[r3,#6] @get ranged anim
cmp r0,#0
beq DefaultAnim
@now check if ranged
ldr r1, RangeVal
ldrb r1,[r1]
cmp r1,#0 @if entry was 0, return
beq DefaultAnim
mov r2,r0 @if range was melee, return
DefaultAnim:
mov r3,r2
cmp r3,#3
bne Return_NoJav
ldr r0, ReturnJav
bx r0
Return_NoJav:
ldr r0, Return
bx r0

.align
RangeVal:
.long 0x203cd14
ReturnJav:
.long 0x8049dd1
Return:
.long 0x8049e93
