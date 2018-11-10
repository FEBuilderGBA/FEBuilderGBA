@トラップの追加	座標	[TrapID]	Args
@
@40 0D [TrapID] [is_effect] [ASM+1]
@
@Author 7743
@
@
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@
@
.thumb
	push	{r4,r5,r6,lr}
	sub sp, #0x4
	mov  r6, r0               @Procsの保存

@	ldr  r5, =0x030004DC      @FE8J MemorySlot 0B
	ldr  r5, =0x030004E4      @FE8U MemorySlot 0B
	ldrh r0, [r5,#0x0]        @MemorySlot 0B -> X
	ldrh r1, [r5,#0x2]        @MemorySlot 0B -> Y
@	ldr  r5, =0x030004B4      @FE8J MemorySlot 01
	ldr  r5, =0x030004BC      @FE8U MemorySlot 01

	ldr  r4, [r6, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrb r4, [r4, #0x2]       @引数1 trap id
	cmp  r4, #0x01
	beq  AddBallista
	cmp  r4, #0x02
	beq  AddWallOrSrag
	cmp  r4, #0x04
	beq  AddFireTrap
	cmp  r4, #0x05
	beq  AddGasTrap
	cmp  r4, #0x07
	beq  AddArrowTrap
	cmp  r4, #0x08
	beq  AddTrap8
	cmp  r4, #0x09
	beq  AddTrap9
	cmp  r4, #0x0A
	beq  AddLight
	cmp  r4, #0x0B
	beq  AddMine
	cmp  r4, #0x0C
	beq  AddGorgonEgg
	cmp  r4, #0x0D
	beq  AddLightRune
	b    Term

AddBallista:
	ldrb r2, [r5, #0x00]
	cmp  r3,#0x00
	bne  AddBallistaRun
	mov  r3,#0x35              @ディフォルトはロングアーチ
AddBallistaRun:
@	blh 0x08037a9c,r4          @FE8J AddBallista
	blh 0x08037a04,r4          @FE8U AddBallista
	b    Join


AddWallOrSrag:
	mov r2, #0x2
	ldrb r3, [r5, #0x00]       @HP
	cmp  r3,#0x00
	bne  AddWallOrSragRun
	mov  r3,#0x14              @ディフォルトのHPは、0x14
AddWallOrSragRun:
@	blh 0x0802e1f0,r4          @FE8J AddTrap
	blh 0x0802e2b8,r4          @FE8U AddTrap
	b    Join

AddFireTrap:
	ldrb r2, [r5, #0x00]
	ldrb r3, [r5, #0x01]
@	blh 0x0802e24c,r4          @FE8J AddFireTrap
	blh 0x0802e314,r4          @FE8U AddFireTrap
	b    Join

AddGasTrap:
	ldrb r2, [r5, #0x00]
	ldrb r3, [r5, #0x01]
	ldrb r4, [r5, #0x02]
	str r4,[sp, #0x0]
@	blh 0x0802e268,r4          @FE8J AddGasTrap
	blh 0x0802e330,r4          @FE8U AddGasTrap
	b    Join

AddTrap8:
@	blh 0x0802E2C0,r4          @FE8J AddTrap8
	blh 0x0802e388,r4          @FE8U AddTrap8
	b    Join

AddTrap9:
	ldrb r2, [r5, #0x00]
@	blh 0x0802E2D0,r4          @FE8J AddTrap9
	blh 0x0802e398,r4          @FE8U AddTrap9
	b    Join

AddArrowTrap:
	ldrb r1, [r5, #0x00]
	ldrb r2, [r5, #0x01]
@	blh 0x0802e288             @FE8J AddArrowTrap
	blh 0x0802e350             @FE8U AddArrowTrap
	b    Join

AddLight:
	mov r2, #0xa
	ldrb r3, [r5, #0x00]
	cmp  r3,#0x00
	bne  AddLightRun
	mov  r3,#0x8               @ディフォルトの半径は8
AddLightRun:
@	blh 0x0802e1f0,r4          @FE8J AddTrap
	blh 0x0802e2b8,r4          @FE8U AddTrap
	b    Join

AddMine:
	mov r2, #0xb
	mov r3, #0x0
@	blh 0x0802e1f0,r4          @FE8J AddTrap
	blh 0x0802e2b8,r4          @FE8U AddTrap

	ldr  r2, [r6, #0x38]       @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrb r2, [r2, #0x3]        @引数2 with_effect
	cmp  r2, #0x01
	bne  Join                  @エフェクトなし

@	ldr  r5, =0x030004DC       @FE8J MemorySlot 0B
	ldr  r5, =0x030004E4       @FE8U MemorySlot 0B
	ldrh r1, [r5,#0x0]         @MemorySlot 0B -> X
	ldrh r2, [r5,#0x2]         @MemorySlot 0B -> Y
	mov r0,r6
@	blh 0x080222cc             @FE8J BeginMineMapAnim
	blh 0x08022300             @FE8U BeginMineMapAnim
	b    Join

AddGorgonEgg:
	ldrb r2, [r5, #0x00]
	ldrb r3, [r5, #0x01]
	ldrb r4, [r5, #0x02]
	str r4,[sp, #0x0]
@	blh 0x080379C0,r4          @FE8J AddGorgonEggTrap
	blh 0x08037928,r4          @FE8U AddGorgonEggTrap
	b    Join

AddLightRune:
@	blh 0x0802e990             @FE8J AddLightRune
	blh 0x0802ea58             @FE8U AddLightRune

	ldr  r2, [r6, #0x38]       @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
	ldrb r2, [r2, #0x3]        @引数2 with_effect
	cmp  r2, #0x01
	bne  Join                  @エフェクトなし

@	ldr  r5, =0x030004DC       @FE8J MemorySlot 0B
	ldr  r5, =0x030004E4       @FE8U MemorySlot 0B
	ldrh r1, [r5,#0x0]         @MemorySlot 0B -> X
	ldrh r2, [r5,#0x2]         @MemorySlot 0B -> Y
	mov r0,r6
@	blh 0x0802137c             @FE8J BeginLightRuneMapAnim
	blh	0x08021684             @FE8U BeginLightRuneMapAnim
@	b    Join

Join:
Term:
	mov r0 ,#0x0
	add sp, #0x4
	pop {r4,r5,r6}
	pop	{r1}
	bx	r1

