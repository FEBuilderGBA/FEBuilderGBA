.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@HOOK 08025AF4	//{J}
@HOOK 08025B50	//{U}
@r4 Unit

push {r5}
ldr r5, TABLE
sub r5, #0x4

Loop:
add r5, #0x4
ldr r0, [r5]
cmp r0, #0xFF
beq TrueReturn

CheckUnit:
ldrb r0, [r5,#0x0]
cmp r0, #0x00
beq CheckClass

ldr r1, [r4, #0x0]
ldrb r1, [r1, #0x04]
cmp  r0, r1
bne  Loop

CheckClass:
ldrb r0, [r5,#0x1]
cmp r0, #0x00
beq CheckFlag

ldr r1, [r4, #0x4]
ldrb r1, [r1, #0x04]
cmp  r0, r1
bne  Loop

CheckFlag:
ldrh r0, [r5,#0x2]
cmp r0,#0x0
beq Found

blh  0x080860D0     @FE8J CheckFlag  Flag=r0  Result=r0:bool
@blh  0x08083DA8     @FE8U CheckFlag  Flag=r0  Result=r0:bool
cmp  r0, #0x1
bne  Loop


Found:
ldr  r3, =0x08025B06|1	@{J}
@ldr  r3, =0x08025B62|1	@{U}
b Exit

TrueReturn:
mov r0, #0x10     @壊すコードの再送
ldsb r0, [r4, r0]
mov r1, #0x11
ldsb r1, [r4, r1]
ldr  r3, =0x08025AFC|1	@{J}
@ldr  r3, =0x08025B58|1	@{U}
b Exit


Exit:
pop {r5}
bx r3

.align
.ltorg
TABLE:
