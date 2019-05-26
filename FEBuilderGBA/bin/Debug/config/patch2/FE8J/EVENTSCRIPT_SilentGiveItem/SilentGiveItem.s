.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
	push	{r4,r5,lr}

	ldr  r4, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrb r0, [r4, #0x0]       @引数0 [FLAG]

	mov  r1,#0xf
	and  r0,r1

	cmp  r0,#0x1
	beq  GetUnitInfoBySVAL1
	cmp  r0,#0xB
	beq  GetUnitInfoByCoord
	cmp  r0,#0xF
	beq  GetUnitInfoByCurrent
	cmp  r0,#0x2
	beq  SetSupply

	ldrb r0, [r4, #0x2]       @引数1 [UNIT]
	b   GetUnitInfo

GetUnitInfoByCurrent:
	ldr  r0,=#0x03004DF0      @操作中のユニット {J}
@	ldr  r0,=#0x03004E50      @操作中のユニット {U}
	ldr  r0,[r0]
	b   CheckUnitInfo

GetUnitInfoBySVAL1:
	ldr  r0,=#0xFFFFFFFF
	b   GetUnitInfo

GetUnitInfoByCoord:
	ldr  r0,=#0xFFFFFFFE
	@b   GetUnitInfo

GetUnitInfo:
	blh  0x0800bf3c           @UNITIDの解決 GetUnitStructFromEventParameter	{J}
@	blh  0x0800bc50           @UNITIDの解決 GetUnitStructFromEventParameter	{U}
	                          @RAM UNIT POINTERの取得
CheckUnitInfo:
	cmp  r0,#0x00
	beq  Term                 @取得できなかったら終了

	mov  r5,r0                @現在のユニットのポインタを保存
	                          @アイテムが満杯の時は、自軍であれば輸送隊に送るため

	mov  r3,#0x28
	add  r3,r3,r0
	add  r0,#0x1E
ItemLoop:
	cmp  r0,r3
	bge  ItemFull             @アイテムが満杯
	ldrb r1,[r0]
	cmp  r1,#0x00
	beq  StoreItem
	add  r0,#0x02
	b    ItemLoop

ItemFull:
	ldrb  r0,[r5,#0xB]
	cmp   r0,#0x40
	bge   Error               @自軍ではないので、輸送隊は使えない。 エラー終了とする。
	@ 自軍の場合は、輸送隊にアイテムを送る
	@b     SetSupply

SetSupply:                    @輸送隊にアイテムを送る
                              @輸送体のアドレスは sendToConvoyASMC (Author:circleseverywhere) の成果を基にする
	ldr  r3, =0x8031508 @size of convoy	{J}
@	ldr  r3, =0x80315bc @size of convoy	{U}
	ldrb r3, [r3] @normally 0x63

	ldr  r0, =0x8031500 @pointer to convoy	{J}
@	ldr  r0, =0x80315b4 @pointer to convoy	{U}
	ldr  r0, [r0]

	lsl  r3, #0x01            @end = size*2 + convoy
	add  r3, r0

SupplyItemLoop:
	cmp  r0,r3
	bgt  Error                @アイテムが満杯なので無理
	ldrb r1,[r0]
	cmp  r1,#0x00
	beq  StoreItem
	add  r0,#0x02
	b    SupplyItemLoop

Error:                        @アイテムが満杯なので、渡せませんでした。 エラー終了
	mov  r0, #0x00
	b    Term

StoreItem:                    @アイテムを書き込む
	mov  r5,r0                @Store Address

	ldrb r0, [r4, #0x3]       @引数2 [ITEM]
	blh  0x080162e8           @アイテムIDから耐久度を取得する MakeItemShort RET=ITEMPACK 耐久回数<<8|アイテムID 例:鉄の剣 耐久:46 == 2e01 r0=アイテムID	{J}
@	blh  0x08016540           @アイテムIDから耐久度を取得する MakeItemShort RET=ITEMPACK 耐久回数<<8|アイテムID 例:鉄の剣 耐久:46 == 2e01 r0=アイテムID	{U}

	strh r0,[r5]
	mov  r0, #0x01

Term:
	pop {r4,r5}
	pop	{r1}
	bx	r1
