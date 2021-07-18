@ドラクエみたいに、レアアイテムを売ろうとするとセリフを変える
@ディフォルト選択子は「いいえ」を自動選択
@
@HOOK 0xB9478	{J}
@HOOK 0xB48F0	{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ RareText, RareItemTable+4

@売ろうとしているアイテムIDの取得
GetItemID:
ldr r1, [r5, #0x2c]
ldrb r0, [r4, #0x0]
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
@ldr r0, =0x00000855	@{J}
ldr r0, =0x000008B5	@{U}

Exit:
mov r1 ,r5
@blh 0x080b8cf0   @店 会話を表示	{J}
blh 0x080b4168   @店 会話を表示	{U}

@ldr r3, =0x080B9480|1	@{J}
ldr r3, =0x080B48F8|1	@{U}
bx  r3

.align
.ltorg
RareItemTable:
@RareText
