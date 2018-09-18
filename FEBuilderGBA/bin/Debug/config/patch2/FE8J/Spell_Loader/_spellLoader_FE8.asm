.thumb
.org 0
@//branch away at 8058028 (can't use jumpToHack, we need to bx via r2)	FE8U
@//at 8058028
@//SHORT $4a00 $4710
@//POIN HackLocation+1

@branch away at 8058E74 (can't use jumpToHack, we need to bx via r2)	FE8J
@at 8058E74
@
@SHORT $4a00 $4710
@POIN HackLocation+1

nop             @padding
mov  r3,r0      @FE8Jではなぜか、r3ではなくr0が利用されている
                @For FE 8J, because r0 is used instead of r3
ldrh r2,[r3,#4] @get melee anim
ldrh r0,[r3,#6] @get ranged anim
cmp r0,#0
beq DefaultAnim
@now check if ranged
ldr r1, RangeVal
ldrb r1,[r1]
cmp r1,#1 @if entry was 1, return
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
@//.long 0x203A4D6	@//FE8U
.long    0x203A4D2	@//FE8J
ReturnJav:
@//.long 0x8058035	@//FE8U
.long    0x8058E7F	@//FE8J
Return:
@//.long 0x8058197	@//FE8U
.long    0x8058FDF	@//FE8J
