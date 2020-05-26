.thumb
@@@call 0x161DA
@壊すコードを再送
ldr r1, =0x08017314
mov lr, r1
.short 0xF800
ldr r0, [r1, #0xc]
mov r1, #0x0
cmp r0, #0x0
beq Skip1
ldrb r0, [r0, #0x1]  @Item->StatBooster->Str

Skip1:

@TANTE_hose が pop{r4,pc} を行うので、戻り値先を補正する
mov r2, #0x1
mov r3, pc
add r3, #0x2 * 4 + 1 @skip 4 code
push {r3}
push {r4}
ldr r3, TANTE_hose
mov pc,r3

@元のコードへ戻す
ldr r3,=0x080161F2|1
bx r3

.ltorg
.align
TANTE_hose:
@POIN TANTE_hose
