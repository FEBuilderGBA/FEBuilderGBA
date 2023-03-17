.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@Hook 080574E0

cmp r0, #0x0
beq GoBack

mov r6 ,r1
ldr r1, =0x06010000 @OBJ_VRAM0_TEXT
blh 0x080d6390   @LZ77UnCompVram	@{J}

GoBack:
ldr r3, =0x080574E8|1
bx  r3
