@ドラクエみたいに、レアアイテムを売ろうとするとセリフを変える
@ワールドマップから売却した場合の対処
@
@HOOK 0xA2084	{J}
@HOOK 0x9FDE4	{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ RareText, RareItemTable+4

@売却モードかどうか判別する
cmp  r4, #0x1
bne  NotFound

@売ろうとしているアイテムIDの取得
GetItemID:
mov  r0, #0x30
ldrb r0, [r5, r0]    @index
ldr  r1, [r5, #0x2c] @target ram unit
lsl r0 ,r0 ,#0x1
add r1, #0x1e
add r1 ,r1, r0
ldrb r3, [r1, #0x0] @Get ItemID

@レアアイテムか判定する
CheckRareItem:
ldr r2, RareItemTable
CheckRareItemLoop:
ldrb r0,[r2]
cmp r0, #0x0
beq NotFound

cmp r0, r3
beq Match

add r2, #0x1
b   CheckRareItemLoop

@変更するセリフを返す
Match:
ldr r0, RareText
b   Exit

@通常のセリフを返す
NotFound:
ldr r0, =0x08A95260	@gShopSellTextIndexLookup	@{J}
@ldr r0, =0x08A1951C	@gShopSellTextIndexLookup	@{U}
lsl r4 ,r4 ,#0x2
add r4 ,r4, r0
ldr r0, [r4, #0x0]

Exit:
ldr r1, =0x08A95268	@gpShopSellStringBuffer	@{J}
@ldr r1, =0x08A19524	@gpShopSellStringBuffer	@{U}
ldr r3, =0x080A208C|1	@{J}
@ldr r3, =0x0809FDEC|1	@{U}
bx  r3


.align
.ltorg
RareItemTable:
@RareText
