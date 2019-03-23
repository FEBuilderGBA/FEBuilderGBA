.thumb

push {r4,r5,lr}
                    @MemorySlot1 (UnitID)  00=ANY
                    @MemorySlot2 (ClassID) 00=ANY
                    @MemorySlot3 (ItemID)  00=ANY
                    @MemorySlot4 (affiliation)  FF=ANY 00=Player 01=Enemy 02=NPC

mov  r5, #0x0       @Counter

ldr  r4,=0x030004B8 @MemorySlot FE8U
@ldr  r4,=0x030004B0 @MemorySlot FE8J

ldr r0, =0x0202BE4C @UnitRAM FE8U
@ldr r0, =0x0202BE48 @UnitRAM FE8J

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
found:
add r5,#0x1           @match_count++;
b   next_loop

break_loop:
str  r5,[r4,#0x0C * 0x4]  @store memoryslotC = match_count

pop {r4,r5}
pop {r1}
bx r1
