@Hook 0xD1BEC @{J}
@Hook 0xCCEE4 @{U}
@r10	TargetUnitPointer
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {r4, r5, r6}
@r0 = class id
mov r5, r0

mov r0, r10
ldr r0, [r0, #0x0] @unit
ldrb r6, [r0, #0x4] @Unit->UnidID

ldr r4, Table
sub r4, #0x4
Loop:
add r4, #0x4

ldr r0, [r4, #0x0]
cmp r0, #0x0
beq NotFound

CheckClass:
ldrb r0, [r4, #0x0]	@ClassID
cmp  r0, r5
bne  Loop

CheckUnit:
ldrb r0, [r4, #0x1]	@UnitID
cmp  r0, #0x0
beq  Found
cmp  r0, r6
bne  Loop

Found:
ldrh r0, [r4, #0x2]	@TextID
b    Exit

NotFound:
mov r0, r5	@ClassID
blh 0x0801911c   @GetROMClassStruct	@{J}
@blh 0x08019444   @GetROMClassStruct	@{U}

ldrh r0, [r0, #0x2]	@Class->à–¾Text

Exit:
pop {r4,r5,r6}

strh r0, [r5, #0x3c]	@ƒNƒ‰ƒX‚Ìà–¾


ldr r3, =0x080D1BF4|1	@{J}
@ldr r3, =0x080CCEEC|1	@{U}
bx r3

.align
.ltorg
Table:
