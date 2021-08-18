@Hook 0805896C	@{J}
@Hook 08057A24	@{U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

blh 0x0802c9d0   @GetBattleAnimType	@{J}
@blh 0x0802ca98   @GetBattleAnimType	@{U}
cmp r0, #0x3
beq BattleBGON

CheckBoss:
mov	r2, r10		@{J}
@mov	r2, r9		@{U}

ldr	r0, [r2, #0]
ldrh	r0, [r0, #0x28]
lsl	r0, r0, #0x10
lsr	r0, r0, #0x1f
bne	BattleBGON

BattleBGOff:
ldr r3, =0x080589ae|1	@{J}
@ldr r3, =0x08057a66|1	@{U}
bx  r3

BattleBGON:
ldr r3, =0x08058974|1	@{J}
@ldr r3, =0x08057A2C|1	@{U}
bx  r3
