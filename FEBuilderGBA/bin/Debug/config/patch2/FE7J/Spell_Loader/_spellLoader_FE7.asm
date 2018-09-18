.thumb
.org 0
@//branch away at 8052b38 (can't use jumpToHack, we need to bx via r2) //FE7U
@//at 8052b38            //FE7U
@//SHORT $4a00 $4710     //FE7U
@//POIN HackLocation+1   //FE7U
@//branch away at 8053320 (can't use jumpToHack, we need to bx via r2)  //FE7J
@//at 8053320            //FE7J
@//SHORT $4a00 $4710     //FE7J
@//POIN HackLocation+1   //FE7J

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
@//.long 0x203A3DA  @//FE7U
.long    0x203A3D6  @//FE7J
ReturnJav:
@//.long 0x8052B43  @//FE7U
.long    0x805332B  @//FE7J
Return:
@//.long 0x8052C47  @//FE7U
.long    0x805342F  @//FE7J
