@Call 080B672C FE8U

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
blh  0x08083DA8     @FE8U CheckFlag  Flag=r0  Result=r0:bool
pop {r1,r2}

cmp	r0,#0x00
beq SkipData

Found:
ldr  r0, [r1, #0x4]  @ED2->Text
ldr  r3, =0x080B6740+1
bx   r3

SkipData:
ldr  r3, =0x080B6736+1
bx   r3
