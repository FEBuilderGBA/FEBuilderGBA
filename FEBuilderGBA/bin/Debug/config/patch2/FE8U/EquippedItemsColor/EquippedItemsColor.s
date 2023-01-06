.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@Hook 0x8A758	@{J}
@Hook 0x884CC	@{U}

ldr r0, =0x02003BFC @Stat Screens StatScreenStruct	@{J}	@{U}
ldr r1, [r0, #0xc]  @gpStatScreenUnit

mov r0, #0xb
ldsb r0, [r1, r0]

mov r1, #0xc0
and r0 ,r1
mov r1, #0x3
@blh 0x0808E5CC	@MMBPalSelect Routine	@{J}
blh 0x0808C2CC	@MMBPalSelect Routine	@{U}
mov r0, #0x80

@ldr r3,=0x0808A760|1	@{J}
ldr r3,=0x080884D4|1	@{U}
bx  r3

