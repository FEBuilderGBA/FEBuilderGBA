.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@Hook 0x74D38	@{J}	FE8J
@Hook 0x7285C	@{U}	FE8U

push {r0,r4,r5}

mov r5, #0xff
and r5, r0      @copy item id to r5

ldr r4, Table
sub r4, #0x8

Loop:
add r4, #0x8
ldr r0, [r4]
cmp r0, #0x0
beq NotFound

CheckItem:
ldrb r0, [r4,#0x2]
cmp r0, r5
bne Loop

CheckUnit:
ldrb r0, [r4,#0x3]
cmp r0, #0x0
beq CheckClass

ldr  r1, [r7, #0x00]
ldrb r1, [r1, #0x04]
cmp  r0, r1
bne  Loop

CheckClass:
ldrb r0, [r4,#0x4]
cmp r0, #0x0
beq CheckAllegiance

ldr  r1, [r7, #0x04]
ldrb r1, [r1, #0x04]
cmp  r0, r1
bne  Loop

CheckAllegiance:
ldrb r0, [r4,#0x5]
cmp r0, #0x1
beq CheckAllegiance_Player
cmp r0, #0x2
beq CheckAllegiance_NPC
cmp r0, #0x3
beq CheckAllegiance_Enemy
b   CheckFlag

CheckAllegiance_Player:
ldrb r1, [r7,#0xB]
cmp  r1, #0x40
bge  Loop
b    CheckFlag

CheckAllegiance_NPC:
ldrb r1, [r7,#0xB]
cmp  r1, #0x40
blt  Loop
cmp  r1, #0x80
bge  Loop
b    CheckFlag

CheckAllegiance_Enemy:
ldrb r1, [r7,#0xB]
cmp  r1, #0x80
blt  Loop
@b    CheckFlag


CheckFlag:
ldrh r0, [r4,#0x6]
cmp  r0, #0x00
beq  Found

blh  0x080860d0        @CheckFlag	@{J}
@blh  0x08083da8        @CheckFlag	@{U}
cmp  r0,#0x00
beq  Loop


Found:
ldrh r6, [r4,#0x0]
pop {r0,r4,r5}
ldr r3, =0x8074D54|1	@{J}
@ldr r3, =0x8072878|1	@{U}
bx r3


NotFound:
pop {r0,r4,r5}
blh 0x08074a4c   @IsHealStaffBGM	@{J}
@blh 0x08072570   @IsHealStaffBGM	@{U}
ldr r3, =0x8074d46|1	@{J}
@ldr r3, =0x807286a|1	@{U}
bx r3

.ltorg
.align 4
Table:
@sizeof()==8
@W0	BGM
@B2	ITEMID
@B3	UNIT
@B4	CLASS
@B5	Allegiance
@W6	FLAG
