@
@特定範囲の全ユニットのパラメータを一気に変更する
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

push {r4,r5,lr}
                    @MemorySlot1 (UnitID)  00=ANY
                    @MemorySlot2 (ClassID) 00=ANY
                    @MemorySlot3 (ItemID)  00=ANY
                    @MemorySlot4 (affiliation)  FF=ANY 00=Player 01=Enemy 02=NPC
                    @MemorySlot5 XXYY start range
                    @MemorySlot6 XXYY end   range

                    @MemorySlotA Type
                    @MemorySlotB Value

@ldr  r4,=0x030004B0 @MemorySlot FE8J
ldr  r4,=0x030004B8 @MemorySlot FE8U

@ldr r0, =0x0202BE48 @UnitRAM FE8J
ldr r0, =0x0202BE4C @UnitRAM FE8U

ldr r5,=0x85 * 0x48 @Player+Enemy+NPC
add r5,r0

sub r0,#0x48        @Because it is troublesome, first subtract

next_loop:
cmp r0,r5
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
ldr r3,[r4,#0x01 * 4] @MemorySlot1 (UnitID)
cmp r3,#0x00
beq check_class_id

ldr r2, [r0]          @unitram->unit
ldrb r2, [r2, #0x4]   @unitram->unit->id
cmp  r2, r3
bne  next_loop

check_class_id:
ldr r3,[r4,#0x02 * 4] @MemorySlot2 (ClassID)
cmp r3,#0x00
beq check_item_id

ldr r2, [r0, #0x4]    @unitram->class
cmp r2, #0x00
beq next_loop

ldrb r2, [r2, #0x4]   @unitram->class->id
cmp  r2, r3
bne  next_loop


check_item_id:
ldr r3,[r4,#0x03 * 4]  @MemorySlot3 (ItemID)
cmp r3,#0x00
beq check_affiliation

ldrb r2, [r0, #0x1e]    @unitram->item1
cmp  r2, r3
beq  item_match

mov  r2, #0x20
ldrb r2, [r0, r2]    @unitram->item2
cmp  r2, r3
beq  item_match

mov  r2, #0x22
ldrb r2, [r0, r2]    @unitram->item3
cmp  r2, r3
beq  item_match

mov  r2, #0x24
ldrb r2, [r0, r2]    @unitram->item4
cmp  r2, r3
beq  item_match

mov  r2, #0x26
ldrb r2, [r0, r2]    @unitram->item5
cmp  r2, r3
bne  next_loop

item_match:
check_affiliation:

ldr r3,[r4,#0x04 * 4] @MemorySlot4 (affiliation) 
cmp r3,#0xFF          @FF=ANY
beq affiliation_match

ldrb r2, [r0, #0xb]    @unitram->affiliation

cmp r3,#0x01          @01=Enemy
beq check_affiliation_enemy

cmp r3,#0x02          @02=NPC
beq check_affiliation_npc

check_affiliation_player: @00=Player
                          @Player that did misconfiguration is treated as Player.
cmp r2,#0x40          @if (unit->affiliation >= 0x40){ cotinue; }
bge next_loop
b   affiliation_match

check_affiliation_npc:
cmp r2,#0x40          @if (unit->affiliation < 0x40 || unit->affiliation >= 0x80){ cotinue; }
blt next_loop
cmp r2,#0x80
bge next_loop
b   affiliation_match

check_affiliation_enemy:
cmp r2,#0x80          @if (unit->affiliation < 0x80){ cotinue; }
blt next_loop
@b   affiliation_match


affiliation_match:
check_range:
ldr r3,[r4,#0x04 * 6] @MemorySlot6 (end range) 
cmp r3,#0x0           @00=ANY
beq check_match

ldrb r1, [r0, #0x10]    @unitram->x
ldrb r3,[r4,#0x04 * 5 + 0] @MemorySlot5 (start->x) 
cmp  r1,r3
blt  next_loop

ldrb r1, [r0, #0x11]    @unitram->y
ldrb r3,[r4,#0x04 * 5 + 2] @MemorySlot5 (start->y) 
cmp  r1,r3
blt  next_loop

ldrb r1, [r0, #0x10]    @unitram->x
ldrb r3,[r4,#0x04 * 6 + 0] @MemorySlot6 (end->x) 
cmp  r1,r3
bgt  next_loop

ldrb r1, [r0, #0x11]    @unitram->y
ldrb r3,[r4,#0x04 * 6 + 2] @MemorySlot6 (end->y) 
cmp  r1,r3
bgt  next_loop
@b check_match

check_match:
found:

	bl Update

b   next_loop

break_loop:
@	blh  0x08019ecc   @RefreshFogAndUnitMaps	@FE8J
@	blh  0x08027144   @SMS_UpdateFromGameData	@FE8J
@	blh  0x08019914   @UpdateGameTilesGraphics	@FE8J
	blh  0x0801a1f4   @RefreshFogAndUnitMaps    @FE8U
	blh  0x080271a0   @SMS_UpdateFromGameData   @FE8U
	blh  0x08019c3c   @UpdateGameTilesGraphics  @FE8U

pop {r4,r5}
pop {r1}
bx r1

@r0 current unit ram
@r1 temp
@r2 temp
@r3 temp
@r4 MemorySlot

Update:
push {r5,lr}
	ldr  r3,[r4,#0x0A * 4]    @MemorySlotA (Type)
	cmp  r3, #0x00            @unit pointer
	beq  set_str
	cmp  r3, #0x04            @class pointer
	beq  set_str
	cmp  r3, #0x0C            @bad status
	beq  set_str
	cmp  r3, #0x01            @unit id
	beq  set_unit_id
	cmp  r3, #0x05            @class id
	beq  set_class_id
	cmp  r3, #0x47
	ble  set_strb

	cmp  r3, #0x50
	beq  set_1A_lower
	cmp  r3, #0x51
	beq  set_1A_upper

	cmp  r3, #0x52
	beq  set_1D_lower
	cmp  r3, #0x53
	beq  set_1D_upper

	cmp  r3, #0x54
	beq  set_30_lower
	cmp  r3, #0x55
	beq  set_30_upper

	cmp  r3, #0x56
	beq  set_31_lower
	cmp  r3, #0x57
	beq  set_31_upper
	b    Update_Exit

set_str:
	ldr  r2,[r4,#0x0B * 4]    @MemorySlotB (Value)
	str  r2, [r0,r3]
	b   Update_Exit
set_strb:
	ldr  r2,[r4,#0x0B * 4]    @MemorySlotB (Value)
	strb r2, [r0,r3]
	b   Update_Exit
set_1A_lower:
	mov  r3, #0x1A
	b    set_lower
set_1A_upper:
	mov  r3, #0x1A
	b    set_lower
set_1D_lower:
	mov  r3, #0x1D
	b    set_lower
set_1D_upper:
	mov  r3, #0x1D
	b    set_lower
set_30_lower:
	mov  r3, #0x30
	b    set_lower
set_30_upper:
	mov  r3, #0x30
	b    set_lower
set_31_lower:
	mov  r3, #0x31
	b    set_lower
set_31_upper:
	mov  r3, #0x31
	b    set_lower
set_unit_id:
	mov  r5, r0        @unit ram構造体のポインタを保存
	ldr  r0,[r4,#0x0B * 4]    @MemorySlotB (Value)  @unit id
@	blh  0x08019108    @GetUnitStruct	@FE8J
	blh  0x08019430    @GetUnitStruct	@FE8U
	cmp  r0, #0x00
	beq  Update_Exit

	str  r0, [r5, #0x00]
	mov  r0, r5
	b   Update_Exit

set_class_id:
	mov  r5, r0        @unit ram構造体のポインタを保存
	ldr  r0,[r4,#0x0B * 4]    @MemorySlotB (Value)  @class id
@	blh  0x0801911C    @GetROMClassStruct	@FE8J
	blh  0x08019444    @GetROMClassStruct	@FE8U
	cmp  r0, #0x00
	beq  Update_Exit

	str  r0, [r5, #0x04]
	mov  r0, r5
	b   Update_Exit

set_upper:
	ldrb r2, [r0,r3]    @  ([r0,r3] & 0xf) | r2 << 4
	mov  r1,#0xf
	and  r1,r2

	ldr  r2,[r4,#0x0B * 4]    @MemorySlotB (Value)
	lsl  r2,#0x4

	orr  r2, r1
	strb r2, [r0,r3]
	b   Update_Exit

set_lower:
	ldrb r2, [r0,r3]    @  ([r0,r3] & 0xf0) | r2
	mov  r1,#0xf0
	and  r1,r2

	ldr  r2,[r4,#0x0B * 4]    @MemorySlotB (Value)
	orr  r2,r1
	strb r2, [r0,r3]
@b	Update_Exit

Update_Exit:

pop {r5}
pop {r1}
bx r1

