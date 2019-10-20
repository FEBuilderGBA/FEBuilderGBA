@Call 080BB204 FE8J

@r0 textidを返す責任がある
@r1 Pointer
@r2 Check UnitID
@r3 temp

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

ldrb r0, [r1, #0x0] @ED2->Unit
cmp r0 ,r2
bne SkipData

ldrh r0, [r1, #0x2] @ED2->FLag
cmp r0,#0x00
beq Found

push {r1,r2} @blhで壊れてしまうレジスタの退避
blh  0x080860D0     @FE8J CheckFlag  Flag=r0  Result=r0:bool
pop {r1,r2}

cmp	r0,#0x00
beq SkipData

Found:
ldr  r0, [r1, #0x4]  @ED2->Text
ldr  r3, =0x080bb218+1
bx   r3

SkipData:
ldr  r3, =0x080BB20E+1
bx   r3
