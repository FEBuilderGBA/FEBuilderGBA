@Hook 08025344
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb

@救出条件
push	{r4, r5, r6, lr}
mov	r4, r0
ldr	r5, =0x02033F3C	@FE8U
ldr	r0, [r5, #0]
mov	r1, #11
ldsb	r0, [r0, r1]
ldsb	r1, [r4, r1]	@所属IDロード
blh	0x08024d8c @AreUnitsAllied	FE8U

@ldrb	r2, [r4, #11]
@mov	r1, #0x40
@and	r1, r2
@bne	FalseExit	@友軍をかつぐの禁止

ldr	r2, [r5, #0]
cmp	r0, #0
bne	CheckNormal		@自軍?

IsEnemy:
@担ぐ条件
@スリープならばHP関係なく救出できる
ldrh	r1, [r4, #48]
mov	r0, #0xF
and	r1, r0
cmp	r1, #2
beq	CheckNormal

@HP1ケタ
mov	r0, #9            @ hp1桁
ldrb	r1, [r4, #19]
cmp	r0, r1
blt	FalseExit	@HP一桁ではない

CheckNormal:
CheckPhantom:
ldrb r0, [r4, #0xF] @ramunit->status4
lsr  r0,#0x7
cmp r0,#0x01
beq FalseExit

@亡霊戦士のクラスかどうかチェックする
@ldr r0, =0x0807D118	@{J}	亡霊戦士のクラスが書いてあるポインタ
ldr r0, =0x0807ADD0	@{U}	亡霊戦士のクラスが書いてあるポインタ
ldrb r0,[r0]
ldr r1, [r4, #0x4]   @ramunit->class
ldrb r1, [r1, #0x4]  @ramunit->class->id
cmp r0,r1
beq FalseExit

@@騎馬判定    騎馬でもいいじゃない
CheckCav:
ldr	r1, [r4, #40]
lsl	r0, r1, #31
lsr	r0, r0, #31
bne	FalseExit

@輸送隊判定
CheckSupply:
	ldr	r1, [r4]
	ldr	r0, [r4, #4]
	ldr	r1, [r1, #40]
	ldr	r0, [r0, #40]
	orr	r1, r0
	lsl	r0, r1, #22
	lsr	r0, r0, #31
	bne	FalseExit
	ldr	r0, [r4, #12]
	mov	r1, #48
	and	r0, r1
	bne	FalseExit

CheckCanUnitRescue:
	mov	r0, r2
	mov	r1, r4
	blh	0x0801831C	@CanUnitRescue	FE8U
	cmp	r0, #0
	beq	FalseExit

Match:
	ldrb	r0, [r4, #16]
	ldrb	r1, [r4, #17]
	ldrb	r2, [r4, #11]
	mov		r3, #0x0
	blh		0x0804f8bc,r6   @AddTarget

FalseExit:
pop {r4,r5,r6}
pop {r0}
bx r0
