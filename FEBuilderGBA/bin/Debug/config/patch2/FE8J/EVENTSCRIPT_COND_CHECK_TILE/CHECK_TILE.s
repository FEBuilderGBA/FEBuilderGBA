@Check Tile
@
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
                    @MemorySlot1
                    @MemorySlot2 (TILE1,TILE2,TILE3,TILE4)
                    @MemorySlot3 (TILE5,TILE6,TILE7,TILE8)

ldr  r4,=0x030004B0 @MemorySlot FE8J
@ldr  r4,=0x030004B8 @MemorySlot FE8U

ldr r0, [r4,#0x1 * 4] @Slot1
ldr r1, =0xFFFF
beq UseCurrentUnit
ldr r1, =0xFFFE
beq UseSlotB

UseUnit:
blh  0x0800bf3c           @GetUnitStructFromEventParameter	{J}
@blh  0x0800bc50           @GetUnitStructFromEventParameter	{U}
	                          @RAM UNIT POINTERの取得
b LoadUnitCorrd

UseCurrentUnit:
ldr  r0,=#0x03004DF0      @操作中のユニット {J}
@ldr  r0,=#0x03004E50      @操作中のユニット {U}
ldr  r0,[r0]
@b LoadUnitCorrd

LoadUnitCorrd:
cmp  r0,#0x00
beq  NotFound             @取得できなかったら終了

ldrb r5, [r0,#0x10]        @X
ldrb r6, [r0,#0x11]        @Y
b   GetTile

UseSlotB:
ldrh r5, [r4,#0xB * 4 + 0]        @MemorySlot 0B -> X
ldrh r6, [r4,#0xB * 4 + 2]        @MemorySlot 0B -> Y
@b   GetTile


GetTile:
ldr r1, =0x0202E4D8 @gMapTerrainPointer	{J}
@ldr r1, =0x0202E4DC @gMapTerrainPointer	{U}
ldr r1, [r1]		@gMapTerrain

lsl r0 ,r6 ,#0x2
add r0 ,r0, r1
ldr r0, [r0, #0x0]

add r0 ,r5, r0
ldrb r0, [r0, #0x0]   @TileID

add r0, #0x01         @TileID+1

mov  r1, #0x2 * 4 + 0
mov  r3, #0x4 * 4 + 0
Loop:
ldrb r2, [r4,r1]
cmp  r2, r0
beq  Found

add  r1,#0x01
cmp  r1, r3
ble  Loop

NotFound:
mov r0, #0x0
b   Exit

Found:
mov r0, #0x1

Exit:
str  r0,[r4,#0x0C * 4]    @MemorySlotC (Result Value)

pop {r4,r5,r6}
pop {r1}
bx r1

