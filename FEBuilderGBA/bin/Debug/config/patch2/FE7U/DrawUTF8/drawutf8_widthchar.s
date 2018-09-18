.thumb
.macro bll to, reg=r3
  ldr \reg, \to
  mov lr, \reg
  .short 0xf800
.endm

@08005BD4 から呼び出される. FE7U
@r3 
@r2 text
@r1 store pointer?

PUSH {r1}

mov r1,r2
bll UTF8_COMMON
mov r2,r1


cmp r0, #0x0
bne EXIT
    ldr r0, [r3,#0x4]
    add r0, #0xfc       @NOT FOUND
    ldr r0, [r0, #0x0]
EXIT:

POP {r1}


@r1 = store pointer (FE8Uではr3だが、FE7Uではr1です。)
@r2 = 次のtext
ldr r3,=0x08005BEE      @FE7U  r0-r2まで予約されているのでr3で戻します.
mov pc,r3

.ltorg
.align
UTF8_COMMON:
