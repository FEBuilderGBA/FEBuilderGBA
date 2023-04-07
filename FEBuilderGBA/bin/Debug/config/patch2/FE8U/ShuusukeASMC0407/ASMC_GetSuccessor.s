@Original author: 7743
@Sucessor check added by Shuusuke
.thumb
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb

push {r4,r5,r6,r7,lr}
@ldr  r4,=0x030004B0 @MemorySlot FE8J
ldr  r4,=0x030004B8 @MemorySlot FE8U

mov r5,#0x00 @r5 = commander ID
mov r6,#0xFF @r6 = commander affiliation 0=P 1=N 2=E 3=F


ldrb r0,[r4,#0x01 * 4] @MemorySlot1 (UnitID) @r0 = commander ID
cmp r0,#0x00
beq exit_commander	@no commander?
mov r5,r0	
bl get_commander_affiliation
cmp r0,#0x03
ble valid_commander	@valid commander
mov r0,#0x00	@invalid commander
b exit_commander

valid_commander:
mov r6,r0
mov r1,r5
mov r2,r6
bl get_sucessor

exit_commander:
mov r7,#0x30
str  r0,[r4,r7]    @MemorySlotC (Result Value)



pop {r4,r5,r6,r7}
pop {r1}
bx r1


get_commander_affiliation:
	
push {r4,r5,r6,r7,lr}
	@r0 = commander ID
	@ldr r4, =0x0202BE48 @UnitRAM FE8J
	ldr r4, =0x0202BE4C @UnitRAM FE8U

@ldr r5,=0x85 * 0x48 @Player+Enemy+NPC
ldr r5,=0x99 * 0x48 @Player+Enemy+NPC+Purple
add r5,r4
mov r3,r0
sub r4,#0x48        @Because it is troublesome, first subtract

next_loop_commander:
@cmp r0,#0xFF
@bne found_affiliation

cmp r4,r5
bgt break_loop_commander

add r4,#0x48

ldr r2, [r4]          @unitram->unit
ldrb r2, [r2, #0x4]   @unitram->unit->id
cmp r2, r3
bne next_loop_commander
ldrb r0, [r4, #0xb]    @r0 = commander->affiliation
cmp r0,#0x40
blt player_commander
cmp r0,#0x80
blt npc_commander
cmp r0,#0xC0
blt enemy_commander
b purple_commander

player_commander:
mov r0,#0x00
b break_loop_commander
npc_commander:
mov r0,#0x01
b break_loop_commander
enemy_commander:
mov r0,#0x02
b break_loop_commander
purple_commander:
mov r0,#0x03
@b break_loop_commander
@r0=commander affiliation 0=P 1=N 2=E 3=F
break_loop_commander:
pop {r4,r5,r6,r7}
pop {r1}
bx r1


get_sucessor:
	
push {r4,r5,r6,r7,lr}
@r0 -> sucessor ID
@r1 = commander ID
@r2 = commander affiliation 0=P 1=N 2=E 3=F

@ldr r4, =0x0202BE48 @UnitRAM FE8J
ldr r4, =0x0202BE4C @UnitRAM FE8U

ldr r3,=0x85 * 0x48 @Player+Enemy+NPC
mov r7,r3
add r7,r4

mov r5, r1 	@r5 = commander ID
mov r6,	r2 	@r6 = commander affiliation 0=P 1=N 2=E 3=F

sub r4,#0x48        @Because it is troublesome, first subtract

next_loop_unit:
cmp r4,r7
ble continue_loop_sucessor
mov r0,#0x00
b get_sucessor_break_loop
continue_loop_sucessor:
add r4,#0x48

mov r3,#0x38
ldrb r2, [r4,r3]    @r2 = unitram->commander
cmp  r2,r5          @commander mismatch
bne  next_loop_unit


ldrb r1, [r4, #0xb]    @r2 = unitram->affiliation
cmp r1,#0x40
blt player_unit
cmp r1,#0x80
blt npc_unit
cmp r1,#0xC0
blt enemy_unit
b purple_unit

player_unit:
mov r1,#0x00
b unit_allegiance_found
npc_unit:
mov r1,#0x01
b unit_allegiance_found
enemy_unit:
mov r1,#0x02
b unit_allegiance_found
purple_unit:
mov r1,#0x03
b unit_allegiance_found

unit_allegiance_found:

cmp r1,r6         
bne next_loop_unit		@affiliation mismatch?
ldr r0, [r4]          @unitram->unit
ldrb r0, [r0, #0x4]    @r0 = unitram->ID


bl validate_restriction
cmp r0,#0x00
beq next_loop_unit	@candidate was found but invalidated due to restrictions
get_sucessor_break_loop:

pop {r4,r5,r6,r7}
pop {r1}
bx r1

validate_restriction:
	
push {r4,r5,r6,lr}
@r0 = unit ID
@MemorySlot 2 = EnableNonBoss
@MemorySlot 3 = EnableFemale
@ldr r4, =0x030004B0	@Slot0	{J}
ldr r4, =0x030004B8	@Slot0	{U}

ldr r1, [r4 , #0x2 * 4]	@Slot2
cmp r1,#0x00
bne test_female
mov r6, r0	@r6 = unit ID
@blh  0x0800bf3c           @UNITIDの解決 GetUnitStructFromEventParameter	{J}
blh  0x0800bc50           @UNITIDの解決 GetUnitStructFromEventParameter	{U}
@cmp  r0,#0x00
@beq  end_test         
mov  r5, r0
mov r0,r6

ldr  r1, [r5, #0x0] @RAMUnit->Unit
ldr  r3, [r5, #0x4] @RAMUnit->Class
ldr  r2, [r1, #0x28]
ldr  r3, [r3, #0x28]
orr  r3 ,r2

mov r2,#0x80
lsl r2,#0x08 @boss ability
and r2,r3
cmp r2,#0x00
bne test_female
mov r0,#0x00 @unit disqualified for not being a boss
b end_test

test_female:
ldr r1, [r4 , #0x3 * 4]	@Slot3
cmp r1,#0x00
bne end_test
@b test_lord
mov r6, r0	@r6 = unit ID
@blh  0x0800bf3c           @UNITIDの解決 GetUnitStructFromEventParameter	{J}
blh  0x0800bc50           @UNITIDの解決 GetUnitStructFromEventParameter	{U}
@cmp  r0,#0x00
@beq  end_test         
mov  r5, r0
mov r0,r6

ldr  r1, [r5, #0x0] @RAMUnit->Unit
ldr  r3, [r5, #0x4] @RAMUnit->Class
ldr  r2, [r1, #0x28]
ldr  r3, [r3, #0x28]
orr  r3 ,r2

mov r2,#0x40
lsl r2,#0x08 @female ability
and r2,r3
cmp r2,#0x00
beq end_test
mov r0,#0x00 @unit disqualified for being female
b end_test




end_test:

pop {r4,r5,r6}
pop {r1}
bx r1
