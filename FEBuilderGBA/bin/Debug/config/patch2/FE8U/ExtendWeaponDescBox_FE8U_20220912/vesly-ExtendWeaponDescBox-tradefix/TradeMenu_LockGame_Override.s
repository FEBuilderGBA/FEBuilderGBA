.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

PUSH {r4,lr}
LDR r0, TradeMenuGfxData
blh 0x08005544 @SetupFaceGfxData	@{U}
@blh 0x0800544C @SetupFaceGfxData	@{J}

ldr r0, =0x089A2C48 @Procs MovingUnitGfx	@{U}
@ldr r0, =0x08A132D0 @Procs MovingUnitGfx	@{J}
blh 0x08002E9C @Find6C	@{U}
@blh 0x08002DEC @Find6C	@{J}
CMP r0, #0x0
BEQ Exit
    MOV r2, #0x8F
    LDR r3, =0x00030102
    LSL r2 ,r2 ,#0x2
    STRH r2, [r3, #0x2]
    LDR r1, [r0, #0x30] @pointer:089A2C78 (gWalkSpeedLookup ) -> 00000102
    LDRH r3, [r1, #0x22]
    LSR r3 ,r3 ,#0xA
    LSL r3 ,r3 ,#0xA
    ORR r3 ,r2
    STRH r3, [r1, #0x22]
    MOV r2, #0x1
    MOV r3, #0x20
    STRB r2, [r1, r3]
Exit:
blh 0x08015360 @AddSkipThread2	@{U}
@blh 0x08015384 @AddSkipThread2	@{J}
POP {r4}
POP {r0}
BX r0

.ltorg
TradeMenuGfxData:

