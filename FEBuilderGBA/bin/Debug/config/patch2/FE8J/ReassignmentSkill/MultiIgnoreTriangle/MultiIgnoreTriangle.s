.thumb

@複数の相性無効
@Hook 0802C6FC	@{J}
@Hook 0802C7C4	@{U}

mov  r5 ,r1   @壊すコードの再送

ldr  r3, Table
ldr  r1, [r4, #0x4] @LeftUnit->Class
ldrb r1, [r1, #0x4]	@ClassID

Loop:
ldrb r2, [r3]
cmp  r2, #0x0
beq  FalseExit

cmp  r2, r1
beq  TrueExit

add  r3, #0x01
b    Loop

FalseExit:
ldr r2, =0x0802C704	@Pointer ItemWeaponTriangle	@{J}
@ldr r2, =0x0802C7CC	@Pointer ItemWeaponTriangle	@{U}

ldr r2, [r2]
ldr r3, =0x802c74a|1	@{J}
@ldr r3, =0x802C812|1	@{U}
bx  r3

TrueExit:
pop {r4,r5,r6}
pop {r0}
bx  r0

.align
.ltorg
Table:
