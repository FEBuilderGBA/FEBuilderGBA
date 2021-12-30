.thumb
.macro blh to, reg=r3
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm
.macro blh_ to, reg=r3
    ldr \reg, \to
    mov lr, \reg
    .short 0xF800
.endm

@r0=character data, r1=item id
push {lr}
cmp  r1, #0x0
beq  Exit_NotUse

push {r0, r1}           @à¯êîÇÃï€åÏ

blh_ Check_Lock
mov  r2, r0

pop  {r0, r1}
cmp  r2, #0x0
beq  Exit_NotUse

Exit_ContinueStaffProcess:
mov r3 ,r0
ldr r2, =0x080167AC|1	@{U}
@ldr r2, =0x08016554|1	@{J}
bx  r2


Exit_NotUse:
mov r3 ,r0
ldr r2, =0x080167da|1	@{U}
@ldr r2, =0x08016582|1	@{J}
bx  r2

.ltorg
Check_Lock:
@
