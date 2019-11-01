.thumb

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


push {r4,lr}
ldr r4, =0x03004E50 @(Pointer to work memory of operation character ) {U}
ldr r4, [r4, #0x0]
ldr r0, [r4, #0xc]
mov r1, #0x40    @再移動ではないことの確認
and r0 ,r1
cmp r0, #0x0
bne FlaseReturn

CheckHero:
mov r0 ,r4
blh 0x08037B64   @Hero determination function bool	{U}
cmp r0, #0x0
beq FlaseReturn

CheckCond:
ldrb r0, [r4, #0x10]
ldrb r1, [r4, #0x11]
blh 0x08084078   @GetAvailableLocaCommandAt	{U}
ldr r1,=0x19     @MapObject 0x19
cmp r0, r1
bne FlaseReturn

TrueReturn:
	mov r1, #0x1
	b Exit

FlaseReturn:
    mov r1, #0x3

Exit:
mov r0 ,r1
pop {r4}
pop {r1}
bx r1
