.thumb

.macro blh to, reg=r3
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

@Call 080AD386	{J}
@Call 080A896E	{U}

mov  r1, #0x14
ldrb r1, [r0, r1]
mov  r2, #0x40
and  r1, r2
cmp  r1, #0x40
beq  HardMode

mov  r1, #0x42
ldrb r1, [r0, r1]
mov  r2, #0x20
and  r1, r2
cmp  r1, #0x20
beq  NormalMode

mov  r3, #0x0
b    AddIfCasualMode

NormalMode:
mov  r3, #0x2
b    AddIfCasualMode

HardMode:
mov  r3, #0x4
@b    AddIfCasualMode

AddIfCasualMode:
mov  r1, #0x42
ldrb r1, [r0, r1]
mov  r2, #0x40
and  r1, r2
cmp  r1, #0x40
beq  CasualMode

ClassicMode:
add  r3, #0x1
b    Exit

CasualMode:
@add r3, #0x0

Exit:
ldr r1, =0x0203EF60 @gSaveMenuRTextData	@{J}
@ldr r1, =0x0203EF64 @gSaveMenuRTextData	@{U}

strb r3, [r1, #0x2]   @gSaveMenuRTextData.CurrentWorldmapNodeID 
                      @ここに、難易度のモードを格納する.

ldr r3, =0x080ad3f0|1	@{J}
@ldr r3, =0x080A89D8|1	@{U}
bx r3
