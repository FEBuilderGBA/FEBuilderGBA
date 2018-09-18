.thumb
.macro bll to, reg=r3
  ldr \reg, \to
  mov lr, \reg
  .short 0xf800
.endm

@08004574 から呼び出される. FE8U
@r3 work
@r2 width
@r1 text

PUSH {r4}
mov  r4, #0x0         @string width
Loop:

bll UTF8_COMMON
cmp r0,#0x00
beq Exit

ldrb r0,[r0, #0x05]   @Font->Width
add r4,r0             @string width
B Loop

Exit:

mov r2, r4            @string width
POP {r4}

@r2 = width
ldr r0,=0x0800458E
mov pc,r0

.ltorg
.align
UTF8_COMMON:
