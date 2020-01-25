.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


push {r4,lr}
@ldr r4, =0x85C5498   @補助メニューの位置	{J}
ldr r4, =0x859CFB8   @補助メニューの位置	{U}
ldr r4 , [r4]

SubMenuCheckLoop:

@効果が定義されていないメニューがあるなら終了させる
ldr r0 , [r4, #0x14]
cmp r0 , #0x00
beq HideMenu

@待機は常に表示されるので、チェックしない
@ldr r3 , =0x8022701 @待機 Wait	{J}
ldr r3 , =0x802273A @待機 Wait	{U}
cmp r0 , r3
beq NextLoop

@表示可能かどうかチェックする
ldr r0 , [r4, #0xc]
cmp r0 , #0x00
beq HideMenu

@メニューが有効かチェック
mov lr, r0
.short 0xf800
cmp r0, #0x01
beq ShowMenu

NextLoop:
add r4, #0x24
b SubMenuCheckLoop


HideMenu:
mov r0, #0x3
b   Exit

ShowMenu:
mov r0, #0x1
Exit:
pop {r4}
pop {r1}
bx r1
