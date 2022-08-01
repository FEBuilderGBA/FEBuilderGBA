.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
	.thumb

@FE8U
@	.equ MemorySlot, 0x30004B8	@{U}
@	.equ RamUnitTable, 0x859A5D0 @0th entry	@{U}
@	.equ UnitMap, 0x202E4D8		@{U}
@	.equ GetUnit, 0x8019430		@{U}
@	.equ GetUnitByEventParameter, 0x0800BC50		@{U}
@	.equ CanUnitCrossTerrain, 0x801949c		@{U}
@	.equ RefreshFogAndUnitMaps, 0x0801a1f4	@{U}
@	.equ SMS_UpdateFromGameData, 0x080271a0	@{U}
@	.equ UpdateGameTilesGraphics, 0x08019c3c	@{U}
@	.equ Rolld100, 0x8000c64	@{U}
@	.equ CharacterTable, 0x8803D30 @0th entry	@{U}
@	.equ MemorySlot3,0x30004C4    @item ID to give @[0x30004C4]!!?	@{U}
@	.equ DivisionRoutine, 0x080D18FC	@{U}
@	.equ gMapTerrainPointer, 0x0202E4DC	@{U}

@FE8J
	.equ MemorySlot, 0x030004B0	@{J}
	.equ RamUnitTable, 0x085C2A50 @0th entry	@{J}
	.equ UnitMap, 0x0202E4D4		@{J}
	.equ GetUnit, 0x08019108		@{J}
	.equ GetUnitByEventParameter, 0x0800BF3C		@{J}
	.equ CanUnitCrossTerrain, 0x08019174		@{J}
	.equ RefreshFogAndUnitMaps, 0x08019ECC	@{J}
	.equ SMS_UpdateFromGameData, 0x08027144	@{J}
	.equ UpdateGameTilesGraphics, 0x08019914	@{J}
	.equ Rolld100, 0x08000C3C	@{J}
	.equ CharacterTable, 0x08858288 @0th entry	@{J}
	.equ MemorySlot3,0x030004BC    @item ID to give @[0x30004C4]!!?	@{J}
	.equ DivisionRoutine, 0x080D65F8	@{J}
	.equ gMapTerrainPointer, 0x0202E4D8	@{J}

	.global RandomizeCoordsFEBuilderGBA
	.type   RandomizeCoordsFEBuilderGBA, function

RandomizeCoordsFEBuilderGBA: @ players, npcs, and enemies 
push {r4-r7, lr}
mov r7, r8 
push {r7}

ldr r0, =MemorySlot
ldr r5, [r0, #4*0x09] @r5 / s9 as valid coords to place into 
ldr r6, [r0, #4*0x01] @r6 / s1 as valid terrain type 

ldr r1, [r0, #4*0x05] @s5 as unit type
cmp r1, #0x0 @Player
beq RandomizeCoordsPlayer
cmp r1, #0x1 @Enemy
beq RandomizeCoordsEnemy
cmp r1, #0x2 @NPC
beq RandomizeCoordsNPC

RandomizeCoordsAll:
mov r0, #0xBF
mov r8, r0
mov r4, #0x0 @ deployment id / counter 
b LoopThroughUnits

RandomizeCoordsPlayer:
mov r0, #0x3F 
mov r8, r0
mov r4, #0x0 @ deployment id / counter 
b LoopThroughUnits

RandomizeCoordsEnemy:
mov r0, #0xBF
mov r8, r0
mov r4,#0x80 @ deployment id / counter 
b LoopThroughUnits

RandomizeCoordsNPC:
mov r0, #0x7F
mov r8, r0
mov r4, #0x40 @ deployment id / counter
@b LoopThroughUnits


LoopThroughUnits:
mov r0,r4
blh GetUnit @ 19430
cmp r0,#0
beq NextUnitLadder
ldr r3,[r0]
cmp r3,#0
beq NextUnitLadder
ldr r1,[r0,#0xC] @ condition word
mov r2,#0xC @ benched/dead
tst r1,r2
bne NextUnitLadder
b ValidUnit
NextUnitLadder:
b NextUnit

ValidUnit: 
mov r7, r0 

ldr r3, =MemorySlot
ldr r3, [r3, #4*0x08] @r3 / s8 as valid coords to check for units 

lsl r2, r3, #0 
lsr r2, r2, #24 
ldrb r1, [r0,#0x10] 
cmp r1, r2 
blt NextUnit @X coord lower bound 
lsl r2, r3, #16 
lsr r2, r2, #24 
cmp r1, r2 
bgt NextUnit @X coord upper bound 

lsl r2, r3, #8 
lsr r2, r2, #24 
ldrb r1, [r0,#0x11] 
cmp r1, r2 
blt NextUnit @Y coord lower bound 
lsl r2, r3, #24 
lsr r2, r2, #24 
cmp r1, r2 
bgt NextUnit @Y coord upper bound 



@ if you got here, unit exists and is not dead or undeployed, so go ham

mov r6, #0 @counter + x + y 

XCoordInRange:
lsr r6, #16 
add r6, #1 
mov r0, r6 
lsl r6, r6, #16 

cmp r0, #0xFF 
bge End_LoopThroughUnits @we tried 255 times and found no valid result, so give up 

blh Rolld100 


lsl r2, r5, #0
lsr r2, r2, #24 @X lower bound 

lsl r1, r5, #16 
lsr r1, r1, #24 @X upper bound 

sub r1, r1, r2 
mul r0, r1 
mov r1, #0x64 @div 
blh DivisionRoutine

lsl r2, r5, #0
lsr r2, r2, #24 @X lower bound 
add r0, r2 
@lsl r1, r5, #16 
@lsr r1, r1, #24 @X upper bound 

ldr r1,[r7,#0x10] @X 
@strb r0,[r7,#0x10]
lsl r0, #8 
add r6, r0 @--CCXX-- in r6 




YCoordInRange:

blh Rolld100 

lsl r2, r5, #8
lsr r2, r2, #24 @Y lower bound 

lsl r1, r5, #24 
lsr r1, r1, #24 @Y upper bound 

sub r1, r1, r2 
mul r0, r1 
mov r1, #0x64 @div 
blh DivisionRoutine

lsl r2, r5, #8
lsr r2, r2, #24 @Y lower bound 
add r0, r2 


add r6, r6, r0 @----xxyy coord to move unit to 
@mov r6, r0 
@ Check if unit

@r0/r6 as y coord to move unit to 
ldr		r1, =UnitMap
ldr		r1, [r1]
lsl		r0 ,r0 ,#0x2
add		r0 ,r0, r1

@r1 as x coord now 
lsl 	r1, r6, #16 
lsr 	r1, r1, #24 

ldr		r0, [r0]
add		r0 ,r0, r1
ldrb	r0, [r0, #0x0]	@ Character index.

blh 	GetUnit 
cmp		r0, #0x0
bne		XCoordInRange		@ Coord occupied, so try again 


@x coord to move to 
@r1 as x coord now 
lsl 	r1, r6, #16 
lsr 	r1, r1, #24 

@y coord to move to 
lsl 	r0, r6, #24 
lsr 	r0, r0, #24 
mov 	r2, r0 

GetTile:
ldr r3, =gMapTerrainPointer
ldr r3, [r3]		@gMapTerrain

lsl r0 ,r2 ,#0x2 		@Y coord to check 
add r0 ,r0, r3
ldr r0, [r0, #0x0]

add r0 ,r1, r0 			@X coord to check 
ldrb r0, [r0, #0x0]   @TileID

ldr r3, =MemorySlot 
ldr r3, [r3, #4*0x04] @Valid terrain 
cmp r3, #0xFF 
bge AnyTerrainException

cmp r0, r3 
bne XCoordInRange
b ForceSpecificTerrain
AnyTerrainException:
lsl 	r0, r6, #16 
lsr 	r0, r0, #24 
@y coord to move to 
lsl 	r1, r6, #24 
lsr 	r1, r1, #24 


@ Given r0 = x, r1 = y, r2 = unit struct
ldr		r3, =gMapTerrainPointer @ Terrain map	@Load the location in the table of tables of the map you want
ldr		r3,[r3]			@Offset of map's table of row pointers
lsl		r1,#0x2			@multiply y coordinate by 4
add		r3,r1			@so that we can get the correct row pointer
ldr		r3,[r3]			@Now we're at the beginning of the row data
add		r3,r0			@add x coordinate
ldrb	r1,[r3]			@load datum at those coordinates
mov r0, r7 @ unit struct 
blh CanUnitCrossTerrain @0x801949c @ r1 terrain type, r0 unit 
cmp r0, #1
bne XCoordInRange @ If we cannot cross terrain, try another coord. 

ForceSpecificTerrain: @ If terrain type is given, units can be placed there regardless of their ability to walk on it or not 
@x coord to move to 
@r1 as x coord now 
lsl 	r1, r6, #16 
lsr 	r1, r1, #24 
strb r1,[r7,#0x10]

@y coord to move to 
lsl 	r0, r6, #24 
lsr 	r0, r0, #24 
mov r1, #0x11 
strb r0,[r7,#0x11]

blh  RefreshFogAndUnitMaps
blh  SMS_UpdateFromGameData
blh  UpdateGameTilesGraphics

NextUnit:
add r4,#1
cmp r4, r8
bgt End_LoopThroughUnits
b LoopThroughUnits
End_LoopThroughUnits:
pop {r7}
mov r8, r7 
pop {r4-r7}
pop {r0}
bx r0


.ltorg
.align 
	

