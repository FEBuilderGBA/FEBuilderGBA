.thumb
@@@call 0x161DA
@�󂷃R�[�h���đ�
ldr r1, =0x08017314
mov lr, r1
.short 0xF800
ldr r0, [r1, #0xc]
mov r1, #0x0
cmp r0, #0x0
beq Skip1
ldrb r0, [r0, #0x1]  @Item->StatBooster->Str

Skip1:

@TANTE_hose �� pop{r4,pc} ���s���̂ŁA�߂�l���␳����
mov r2, #0x1
mov r3, pc
add r3, #0x2 * 4 + 1 @skip 4 code
push {r3}
push {r4}
ldr r3, TANTE_hose
mov pc,r3

@���̃R�[�h�֖߂�
ldr r3,=0x080161F2|1
bx r3

.ltorg
.align
TANTE_hose:
@POIN TANTE_hose
