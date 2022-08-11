.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

ArenaDicCommandUsability:
push {r4,lr}
ldr r4, ArenaDicMenuCond
sub r4, #0x4

Loop:
add r4, #0x04
ldrb r0, [r4,#0x1]
cmp r0, #0xFF
beq NotFound

ldrb r0, [r4,#0x0]	@ArenaDicMenuCond->MapID
cmp  r0, #0xFF
beq  CheckFlag

ldr r3, =0x0202BCEC @ChapterData
ldrb r1, [r3, #0xE] @ChapterData->MapID
cmp  r0, r1
bne  Loop

CheckFlag:
ldrh r0, [r4,#0x2]	@ArenaDicMenuCond->Flag
cmp  r0, #0x0
beq  Found

blh 0x080860D0	@CheckFlag
cmp r0 ,#0x0
beq Loop

Found:
mov r0, #0x1
b   Exit

NotFound:
mov r0, #0x3
@b   Exit

Exit:
pop {r4}
pop {r1}
bx r1

.ltorg
ArenaDicMenuCond:
