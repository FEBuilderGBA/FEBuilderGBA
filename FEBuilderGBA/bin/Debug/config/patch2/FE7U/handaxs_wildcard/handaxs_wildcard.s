@call 080530E4	{J}
@call 080528FC	{U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@ r8   ItemID (short表記)  アニメ定義から読み取った値
@ r4   現在ユニットが装備しているItemID

mov r0, r8         @壊すコードの再送
str r1,[sp, #0x4]  @壊すコードの再送
str r3,[sp, #0x8]  @

mov r1, #0xff      @GetItemIndex
and r0 ,r1

cmp r4, r0         @目的の武器なら関係なし
beq Exit

cmp r4, #0x28      @HandAxs 現在調べている項目は、手斧ですか? (0x28 == HandAxs)
bne Exit

push {r0}          @アニメテーブルから読み取ったItemIDを保存する

@blh 0x080178B4   @GetROMItemStructPtr	{J}
blh 0x080174AC   @GetROMItemStructPtr	{U}
mov r3, r0

pop {r0}           @復帰 アニメテーブルから読み取ったItemID

@ユニットが装備しているアイテムは手斧か?
ldrb r1,[r3, #0x07]  @ItemType
cmp  r1,#0x02        @0x2 == Axs
bne  Exit

ldrb r1,[r3, #0x19]  @Range
cmp  r1,#0x11
ble  Exit            @遠隔攻撃できない武器ならボツ

Match:
mov r0, r4           @手斧モーションとして処理する

Exit:
@ldr r3, =0x080530EE|1	@{J}
ldr r3, =0x08052906|1	@{U}
bx r3
