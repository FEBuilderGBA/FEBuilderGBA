@Call 080B5268 FE8U
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
push {r6,r5,r4,lr}        @売り値を求める関数 RET=売値 r0=個数 << 8 + アイテムID

mov  r5,r0
mov  r1,#0xff
and  r5,r1

blh  0x0801763c   @FE8U GetItemCost 
mov  r6,r0

ldr  r4,SellPriceExpansion_Table
sub  r4,#0x4

Loop:
add  r4,#0x4
ldrb r0,[r4]     @Item
cmp  r0,#0x00
beq  NotFound

cmp  r0,r5
bne  Loop

ldrh r0,[r4,#0x02]
cmp  r0,#0x00
beq  GetPrice

blh  0x08083DA8   @FE8U CheckFlag
cmp  r0,#0x00
beq  Loop

GetPrice:
mov  r0,r6
ldrb r1,[r4,#0x01]
mul  r0,r1
mov  r1,#100
blh  0x080D167C         @FE8U div

@Check65535             @65535を超えると売れなくなるので補正する
ldr  r1,=#0xFFFF
cmp  r0,r1
blt  Term
mov  r0,r1              @65535に固定

b    Term

NotFound:
mov  r0 ,r6
lsr  r1 ,r0 ,#0x1f
add  r0 ,r0, r1
lsl  r0 ,r0 ,#0xf
lsr  r0 ,r0 ,#0x10

Term:
pop {r6,r5,r4}
pop {r1}
bx r1

.ltorg
.align
SellPriceExpansion_Table:
@POIN SellPriceExpansion_Table
