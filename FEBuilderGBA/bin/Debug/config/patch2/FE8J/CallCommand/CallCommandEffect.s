.align 4
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.macro blh_EALiteral to, reg=r3
  ldr \reg, \to
  mov lr, \reg
  .short 0xf800
.endm

@for FE8U
@.equ CurrentUnitFateData, 0x203A958			@{U}
@.equ CheckEventId,0x8083da8					@{U}
@.equ CurrentUnit, 0x3004E50					@{U}
@.equ EventEngine, 0x800D07C					@{U}
@.equ gChapterData, 0x0202BCF0				@{U}
@.equ GetUnit, 0x8019430						@{U}
@.equ RefreshFogAndUnitMaps, 0x0801A1F4		@{U}
@.equ SMS_UpdateFromGameData, 0x080271a0		@{U}
@.equ UpdateGameTilesGraphics, 0x08019c3c	@{U}
@.equ gMapUnit, 0x0202E4D8					@{U}
@.equ gMapTerrain, 0x0202E4DC				@{U}
@.equ gMapSize, 0x0202E4D4					@{U}

@for FE8J
.equ CurrentUnitFateData, 0x0203A954		@{J}
.equ CheckEventId,0x080860D0				@{J}
.equ CurrentUnit, 0x03004DF0				@{J}
.equ EventEngine, 0x0800D340				@{J}
.equ gChapterData, 0x0202BCEC				@{J}
.equ GetUnit, 0x08019108					@{J}
.equ RefreshFogAndUnitMaps, 0x08019ECC		@{J}
.equ SMS_UpdateFromGameData, 0x08027144	@{J}
.equ UpdateGameTilesGraphics, 0x08019914	@{J}
.equ gMapUnit, 0x0202E4D4					@{J}
.equ gMapTerrain, 0x0202E4D8				@{J}
.equ gMapSize, 0x0202E4D0					@{J}


@ CallLimit_List, end of file +0
.equ CallCommandUsability, CallLimit_List+4

CallCommandEffect:
push	{r4-r7,lr}

blh_EALiteral CallCommandUsability 
mov r7, r1 @ Table entry we're using 
cmp r7, #0x0
beq Term


ldrb r1, [r7, #0x4] @Required HP
cmp  r1, #0x0
beq  OverraideCurrentUnitCoord

ldr r3, =CurrentUnit 
ldr r3, [r3]
ldrb r0, [r3, #0x13] @Unit->HP
cmp  r0, r1          @Check it just in case.
ble  OverraideCurrentUnitCoord
sub  r0, r1
strb r0, [r3, #0x13] @Unit->HP


OverraideCurrentUnitCoord:
ldr r0, =CurrentUnit 
ldr r0, [r0] @ Current unit ram struct pointer 
mov r1, #0x1
bl  OverraideMapUnit

mov r4,#0 @ current deployment id

ldrb r3, [r7,#0x5]
mov r2, #0x1
and r3, r2
beq FilterStatus_NotMoved
mov r5, #0x4F @ moved/dead/undeployed/cantoing/hide
b   LoopThroughUnits

FilterStatus_NotMoved:
mov r5, #0xD  @ dead/undeployed/cantoing/hide

LoopThroughUnits:
add r4, #1  @ deployment byte 
cmp r4, #0x3F 
bgt End

mov r0,r4
blh GetUnit
cmp r0,#0
beq LoopThroughUnits
ldr r3,[r0]
cmp r3,#0
beq LoopThroughUnits

ldr r1, =CurrentUnit
ldr r1, [r1]
cmp r0, r1
beq LoopThroughUnits  @myself

ldr r3,[r0,#0xC] @ condition word
@ if you add +1 to include Hide (eg 0x4F), it'll ignore the active unit, which may be useful 
tst r3,r5
bne LoopThroughUnits
@ if you got here, unit exists and is not dead or undeployed, so go ham
@r0 is Ram Unit Struct 
mov r6, r0

bl IsLimitUnit
cmp r0, #0x0
beq LoopThroughUnits

@Erase the target r6 character from the map.
@This is to create a margin.
ldrb r1, [r6, #0x10]  @Unit->X
ldrb r2, [r6, #0x11]  @Unit->Y

mov r0, r6   @Unit
mov r1, #0x0
bl  OverraideMapUnit

@Overlay the coordinates of the target r6 character with the coordinates of the CurrentUnit that triggered the Call.
ldr r2, =CurrentUnit 
ldr r2, [r2] @ Current unit ram struct pointer 

ldrh r0, [r2, #0x10] 
strh r0, [r6, #0x10] @ So units have matching coords 

mov r0, r6 @ Unit to place 
bl  FindFreeTile

@SetY
lsr r1, r0, #16
mov r2, #0xff
and r2, r1

@SetX
mov r1, #0xff
and r1, r0

@ Store their new coordinates 
strb r1, [r6, #0x10] @ X
strb r2, [r6, #0x11] @ Y

mov r0, r6   @Unit
mov r1, #0x1
bl  OverraideMapUnit

CheckFunctionMoved:
ldrb r3, [r7,#0x5]
mov r2, #0x2     @Will the warp unit be moved?
and r3, r2
beq CheckFunctionHPHalf

ldr r3,[r6,#0xC] @ condition word
mov r2, #0x42    @ moved|cantoing
orr r3, r2
str r3,[r6,#0xC]

CheckFunctionHPHalf:
ldrb r3, [r7,#0x5]
mov r2, #0x4
and r3, r2
beq LoopThroughUnits

ldrb r3, [r6,#0x13] @Unit->HP
cmp r3, #0x1
ble LoopThroughUnits
lsr r3, #0x1
strb r3, [r6,#0x13] @Unit->HP


b LoopThroughUnits

	
End:

blh RefreshFogAndUnitMaps
blh SMS_UpdateFromGameData
blh UpdateGameTilesGraphics

ldr r1, =CurrentUnitFateData	@these four lines copied from wait routine  ActionData
mov r0, #0x1
strb r0, [r1,#0x11]

ldr r0, [r7,#0x8] @event
cmp r0, #0x1
ble Term
mov r1, #3
blh EventEngine

Term:
mov r0, #0x17	@makes the unit wait?? makes the menu disappear after command is selected??
pop {r4-r7}
pop {r1}
bx r1 

@r6 is CurentUnit
IsLimitUnit:
	push {lr,r4}
	ldr r4, CallLimit_List
	sub r4, #0x8
IsLimitUnit_Loop:
	add r4, #0x8
	ldr r0, [r4]
	cmp r0, #0xFF
	beq IsLimitUnit_True

	ldrb r0, [r4, #0x2]
	ldr r2, =gChapterData
	ldrb r1, [r2, #0xE] @ chapter ID 
	cmp r0, #0xFF 
	beq CheckLimitUnitID 
	cmp r0, r1 
	bne IsLimitUnit_Loop

	CheckLimitUnitID:
	ldrb r1, [r4, #0x0]
	cmp r1, #0x0
	beq CheckLimitClassID
	ldr r2, [r6] @ unit pointer 
	ldrb r0, [r2, #0x4] @ unit ID 
	cmp r0, r1
	bne IsLimitUnit_Loop

	CheckLimitClassID:
	ldrb r1, [r4, #0x1]
	cmp r1, #0x0
	beq CheckLimitFlag
	ldr r2, [r6, #0x4] @ class pointer 
	ldrb r0, [r2, #0x4] @ class ID 
	cmp r0, r1
	bne IsLimitUnit_Loop

	CheckLimitFlag:
	ldrh r0, [r4, #0x4]
	cmp r0, #0x0
	beq IsLimitUnit_False @ matches, they cannot be called.
	blh CheckEventId 
	cmp r0, #0
	beq IsLimitUnit_Loop
IsLimitUnit_False:
	mov r0, #0x0
	b   IsLimitUnit_Exit

IsLimitUnit_True:
	mov r0, #0x1

IsLimitUnit_Exit:
	pop {r4}
	pop {r1}
	bx r1


@r0 unit
FindFreeTile:
	push {r4,r5,r6,r7,lr}
	mov r7, r11
	mov r6, r10
	mov r5, r9
	mov r4, r8
	push {r4,r5,r6,r7}

	mov  r1, #0x0
	mov  r10, r1			@best XY

	ldrb r1, [r0, #0x10] @Unit->X
	mov  r8, r1		@base X
	ldrb r1, [r0, #0x11] @Unit->Y
	mov  r9, r1		@base Y

	FindFreeTile_GetMoveTable:
	ldr  r1, [r0, #0x4]	@Unit->Class
	cmp  r1, #0x0
	beq  FindFreeTile_Break
	ldr  r1, [r1, #0x38]
	cmp  r1, #0x0
	beq  FindFreeTile_Break
	mov  r11, r1			@MoveTable

	ldr  r0, =gMapUnit
	ldr  r6, [r0]

	ldr  r0, =gMapTerrain
	ldr  r7, [r0]


@Find the nearest empty position.
FindFreeTile_LoopStart:
	ldr  r0, =9999			@max distance
	mov  r12, r0

	ldr  r0, =gMapSize
	ldrh r5, [r0, #0x2]		@gMapSize.Heiht
FindFreeTile_LoopY:
	cmp  r5, #0x0
	ble  FindFreeTile_Break
	sub  r5, #0x1

	@y座標はx loopが回っている間は固定値なので計算結果をキャッシュしておきます。
		disY:
		mov  r3, r9
		cmp  r3, r5
		bge  disY2
			 sub r3, r5, r3
			 b   disYJoin
		disY2:
			 sub r3, r3, r5
		disYJoin:

		lsl  r2, r5, #0x2	@y*2

	ldr  r0, =gMapSize
	ldrh r4, [r0, #0x0]		@gMapSize.Width

	FindFreeTile_LoopX:
		cmp  r4, #0x0
		ble  FindFreeTile_LoopY
		sub  r4, #0x1

		@r0とr1だけ空いている

		CheckEmpty:
@@@		lsl  r2, r5, #0x2	@y*2	@キャッシュへ移動
		ldr  r1, [r6, r2]
		ldrb r1, [r1, r4]	@gMapUnit[Y][X]
		cmp  r1, #0x0
		bne  FindFreeTile_LoopX			@Not Empty
		
		CheckTile:
		ldr  r1, [r7, r2]
		ldrb r1, [r1, r4]	@tileID = gMapTerrain[Y][X]
		add  r1, r11
		ldrb r1, [r1]		@movCost = Class->MoveCostPtr[tileID]
		cmp  r1, #0x20
		bge  FindFreeTile_LoopX			@Not Move

		@distance=abs(basex-x)+abs(basey-y)
		disX:
		mov  r0, r8
		cmp  r0, r4
		bge  disX2
			 sub r0, r4, r0
			 b   checkDistance
		disX2:
			 sub r0, r0, r4

		checkDistance:
		add r0, r3
		cmp r0, r12
		bge FindFreeTile_LoopX

		mov r12, r0			@update best distance
		lsl r0, r5, #0x10   @bestXY
		add r0, r4
		mov r10, r0
		b   FindFreeTile_LoopX

FindFreeTile_Break:

	pop {r0,r1,r2,r3,r4,r5,r6,r7}
	mov r8, r0
	mov r9, r1
	mov r0, r10 @ret
	mov r10, r2
	mov r11, r3
	pop {r1}
	bx r1

@r0 = Unit
@r1 = Value
OverraideMapUnit:
	push {lr,r4,r5}

	mov  r4, r0
	mov  r5, r1

	ldrb r1, [r4, #0x10]  @Unit->X
	ldrb r2, [r4, #0x11]  @Unit->Y

	ldr  r0, =gMapUnit
	ldr  r0, [r0]

	lsl  r2, #2           @Y*4 
	ldr  r0, [r0, r2]
	strb r5, [r0 , r1]
	pop {r4,r5}
	pop {r0}
	bx r0


.ltorg
.align 4

CallLimit_List:

