.thumb
.macro bll to, reg=r3
  ldr \reg, \to
  mov lr, \reg
  .short 0xf800
.endm

@08004540 から呼び出される. FE8U
@r3 store pointer?
@r2 text
@r1 work

PUSH {r3}

mov r1,r2
bll UTF8_COMMON
mov r2,r1

cmp r0, #0x0
bne EXIT
    ldr r0, [r3,#0x4]
    add r0, #0xfc       @NOT FOUND
    ldr r0, [r0, #0x0]
EXIT:

POP {r3}

@r0 = fontdata
@r3 = store pointer?
@r2 = 次のtext
ldr r1,=0x08004558
mov pc,r1

.ltorg
.align
UTF8_COMMON:
