@rescue ASMC
@expects coordinates of drop unit in Slot 0x1 (0xYYYYXXXX)
@and rescuee in Slot 0x2 (0xYYYYXXXX)

.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@
@
.thumb

push {r4-r5,lr}
ldr r2, =0x30004bc @slot 1	{U}
@ldr r2, =0x30004B4 @slot 1	{J}

ldrh r0,[r2]    @x coords
ldrh r1,[r2,#2] @y coords
bl GetUnitFromCoords
cmp r0,#0
beq End
ldr r5, =0x8019430 @turn deployment no into ram ptr	{U}
@ldr r5, =0x8019108 @turn deployment no into ram ptr	{J}
mov lr,r5
.short 0xf800
mov r4,r0 @save rescuer's ram 救出してる人
ldr r2, =0x30004c0 @slot 2	{U}
@ldr r2, =0x30004B8 @slot 2	{J}
ldrh r0,[r2]
ldrh r1,[r2,#2]
bl GetUnitFromCoords
cmp r0,#0
bne End   @降ろす場所に人がいる

mov r1,#0x1b
ldrb r0,[r4, r1]  @同行者ID
cmp r0,#0x0
beq End             @同行者がいない

sub r0,#0x1         @同行者IDは1から始まるため1を引く
mov r1,#0x48
mul r0,r1           @1つのデータは、0x48 bytes.
ldr r1,=0x0202BE4C  @RAMUnit	{U}
@ldr r1,=0x0202BE48  @RAMUnit	{J}
add r5,r0,r1        @rescuee 救出されている人

ldr  r0,[r5,#0x0]   @そのデータが存在しない??
cmp  r0,#0x00
beq  End

RunDrop:

mov r0, #0x0        @同行者を消す
mov r1,#0x1b
strb r0,[r4,r1]
strb r0,[r5,r1]

ldr r2, =0x30004c0 @slot 2 座標の上書き	{U}
@ldr r2, =0x30004B8 @slot 2 座標の上書き	{J}
ldrh r0,[r2]
ldrh r1,[r2,#2]
mov r3,#0x10
strb r0,[r5,r3]
mov r3,#0x11
strb r1,[r5,r3]

ldr r0,[r4,#0xc]   @救出しているフラグを落とす
ldr r1,=0xFFFFFFEF @~(0x10)
and r0,r1
str r0,[r4,#0xc]

ldr r0,[r5,#0xc]   @非表示と救出されているフラグを落とす
ldr r1,=0xFFFFFFDE @~(0x21)
and r0,r1
str r0,[r5,#0xc]

@	blh  0x08019ecc   @RefreshFogAndUnitMaps    {J}
@	blh  0x08027144   @SMS_UpdateFromGameData   {J}
@	blh  0x08019914   @UpdateGameTilesGraphics  {J}
	blh  0x0801a1f4   @RefreshFogAndUnitMaps    {U}
	blh  0x080271a0   @SMS_UpdateFromGameData   {U}
	blh  0x08019c3c   @UpdateGameTilesGraphics  {U}

End:
pop {r4-r5}
pop {r0}
bx r0

GetUnitFromCoords:
@gets deployment number, given r0=x and r1=y
ldr r2,=0x202e4d8 @pointer to unit map	{U}
@ldr r2,=0x202E4D4 @pointer to unit map	{J}
ldr r2,[r2]
lsl r1,#2 @y*4
add r1,r2 @row address
ldr r1,[r1]
ldrb r0,[r1,r0]
bx lr
