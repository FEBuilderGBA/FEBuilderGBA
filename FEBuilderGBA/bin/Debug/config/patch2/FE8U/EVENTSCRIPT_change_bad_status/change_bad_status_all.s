@健康状態の変更
@
@40 0D [UNIT]  [SICK] [ASM+1]
@41 0D [CLASS] [SICK] [ASM+1]
@
@Author 7743
@
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb

push {r4,r5,r6,lr}

ldr  r4, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ
ldrb r5, [r4, #0x2]       @引数1 [ID]
ldrb r6, [r4, #0x3]       @引数2 [Sick]

ldrb r4, [r4, #0x0]       @引数  [FLAG]
mov  r1,#0xf
and  r4,r1                @isClass

@ldr r0, =0x0202BE48 @UnitRAM FE8J
ldr r0, =0x0202BE4C @UnitRAM FE8U

ldr r1,=#0x85 * 0x48 @Player+Enemy+NPC
add r1,r0

sub r0,#0x48        @Because it is troublesome, first subtract

next_loop:
cmp r0,r1
bgt break_loop

add r0,#0x48

ldr r2, [r0]          @unitram->unit
cmp r2, #0x00
beq next_loop         @Check Empty

ldrb r2, [r0,#0xC]    @unitram->status
mov  r3,#0xC          @dead or not deploy
and  r2,r3
cmp  r2,#0x0          @maybe he is dead
bne  next_loop

check_unit_id:
cmp r4,#0x1
beq check_class_id

ldr r2, [r0]          @unitram->unit
ldrb r2, [r2, #0x4]   @unitram->unit->id
cmp  r2, r5
beq  found
b    next_loop

check_class_id:
ldr r2, [r0, #0x4]    @unitram->class
cmp r2, #0x00
beq  next_loop

ldrb r2, [r2, #0x4]   @unitram->class->id
cmp  r2, r5
bne  next_loop

found:

	mov  r3,r6
	mov  r2,#0x0f             @check bad status
	and  r3,r2                
	cmp  r3,#0x00
	beq  Change               @状態なしにするらしい

	mov  r3,r6
	mov  r2,#0xf0             @check turn
	and  r2,r3                
	cmp  r2,#0x00
	bne  Change               @ターン指定されているのでそのまま採用.

	mov  r2,#0x30             @ターンが入っていないので、3ターンに自動設定
	orr  r3,r2
	@b    Change

Change:
	@r0  ram unit pointer
	@r3  status

	mov  r2,#0x30
	strb r3,[r0,r2]

b   next_loop

break_loop:
@	blh  0x08019ecc   @RefreshFogAndUnitMaps	@FE8J
@	blh  0x08027144   @SMS_UpdateFromGameData	@FE8J
@	blh  0x08019914   @UpdateGameTilesGraphics	@FE8J
	blh  0x0801a1f4   @RefreshFogAndUnitMaps    @FE8U
	blh  0x080271a0   @SMS_UpdateFromGameData   @FE8U
	blh  0x08019c3c   @UpdateGameTilesGraphics  @FE8U

pop {r4,r5,r6}
pop {r1}
bx r1
