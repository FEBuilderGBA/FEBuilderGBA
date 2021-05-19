@FE8J	08031900	@{J}
@FE8U	080319B4	@{U}
@r4	    選出テーブル
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


push {r4, r5, r6}
ldrb r5, [r4, #0x0]	@Get ClassID

ldr  r6, =0x0203A8EC	@ArenaData	@{J}
@ldr  r6, =0x0203A8F0	@ArenaData	@{U}

ldr  r4, Table
sub  r4, #0x8
Loop:
add  r4, #0x8

ldr  r0, [r4]
cmp  r0, #0xff	@Term
beq  NotFound

ldrb r0, [r4, #0x0] @Table->FromClass
cmp  r0, r5
bne  Loop

CHECK_LV:
ldrb r2, [r4, #0x2] @Table->YourLV
cmp  r2, #0x0
beq  CHECK_OPTION

ldr  r1, [r6]		@ArenaData->playerUnit
ldrb r1, [r1, #0x8]	@ArenaData->playerUnit->Level

cmp  r1, r2
blt  Loop

CHECK_OPTION:
ldrb r2, [r4, #0x3] @Table->Option
cmp  r2, #0x0
beq  CHECK_FLAG

ldr r1, [r6]		@ArenaData->playerUnit
ldr r0, [r1, #0xc]	@RAMUnit->Status
lsr r0 ,r0 ,#0x11
mov r1, #0x7
and r0 ,r1
cmp r0, #0x4
blt Loop

CHECK_FLAG:
ldrh r0,  [r4, #0x4]	@Table->Flag
cmp  r0, #0x0
beq  Found

blh  0x080860d0	@CheckFlag	@{J}
@blh  0x08083DA8	@CheckFlag	@{U}
cmp  r0, #0x0
beq  Loop

Found:
@見つかったのでクラスを上書きする
ldrb r0, [r4, #0x1] @Table->ToClass
b Exit

NotFound:
@見つからない場合は、ディフォルト値を利用する.
mov r0, r5

Exit:
pop {r4, r5, r6}

@壊すコードの再送
blh 0x0801911c   @GetROMClassStruct	{J}
@blh 0x08019444   @GetROMClassStruct	{U}
ldr r2, [r0, #0x28] @Class->Attribute
ldrb r0, [r0, #0x4] @Class->ID

mov r1, #0x80
lsl r1 ,r1 ,#0x1
and r2 ,r1
cmp r2 ,r5

ldr r3,=0x08031910|1	@FE8J	@{J}
@ldr r3,=0x080319C4|1	@FE8U	@{U}
bx r3

.align 4
.ltorg
Table:

@FromClass		0
@ToClass			1
@Your LV			2
@CondStatus			3
@	0=特になし
@	1=SuperArena
@Flag				4
@00					6
