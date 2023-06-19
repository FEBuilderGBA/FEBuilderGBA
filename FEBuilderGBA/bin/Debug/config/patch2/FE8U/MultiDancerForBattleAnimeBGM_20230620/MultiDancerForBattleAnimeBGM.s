@Call 08074CD8	@{J}
@Call 080727FC	@{U}
@r0    ram pointer

@r0 work (unit id)
@r1 work (class id)
@r3 work (table)
@r4	bool (result: is_match)
@r7	ram unit

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

ldr r4, MultiDancerClassTable
sub r4, #0x8

Loop:
add r4, #0x8
ldr r0, [r4,#0x00]
cmp  r0,#0x00
beq  NotMatch

CheckUnit:
ldrb r0, [r4,#0x00]
cmp  r0,#0x00
beq  CheckClass

ldr r1,[r7]
ldrb r1,[r1,#0x4]  @Unit->UnitID
cmp  r0,r1   @Check UnitID
bne  Loop

CheckClass:
ldrb r0, [r4,#0x01]
cmp  r0,#0x00
beq  CheckLV

ldr r1,[r7,#0x4]
ldrb r1,[r1,#0x4]  @Class->UnitID
cmp  r0,r1   @Check ClassID
bne  Loop

CheckLV:
ldrb r0, [r4,#0x02]
cmp  r0,#0x00
beq  CheckFlag

ldrb r1,[r7,#0x8]
cmp r1, r0
blt Loop

CheckFlag:
ldrh r0, [r4,#0x04]
cmp  r0,#0x00
beq  CheckRand

@blh  0x080860d0	@CheckFlag	@{J}
blh  0x08083DA8	@CheckFlag	@{U}
cmp  r0, #0x0
beq  Loop

CheckRand:
ldrb r0, [r4,#0x03]
cmp  r0,#100
bge  CheckDanceOrPlay

@blh 0x08000C78 @Roll1RN	@{J}
blh 0x08000CA0 @Roll1RN	@{U}

cmp  r0, #0x0
beq  Loop


CheckDanceOrPlay: @Check Dance or Play

    @ldr r0, =0x0203A4D0  @(AttackerBattleStruct@AttackerBattleStruct.Seems to be a bitfield (0x2 seems to be 'battle hasn't started yet') (0x20 is arena))	{J}
    ldr r0, =0x0203A4D4  @(AttackerBattleStruct@AttackerBattleStruct.Seems to be a bitfield (0x2 seems to be 'battle hasn't started yet') (0x20 is arena))	{U}

    ldrh r1, [r0, #0x0]  @(AttackerBattleStruct@AttackerBattleStruct.Seems to be a bitfield (0x2 seems to be 'battle hasn't started yet') (0x20 is arena))
    mov r0, #0x40
    and r0 ,r1
    cmp r0, #0x0
    beq NotMatch

Match:
    ldrh r0, [r4,#0x06]  @Table->BGM
    
    @BGMがSEかBGMか判定する
    @ldr r2,=0x080D5024  @Song Table Pointer {J}
    ldr r2,=0x080D032C  @Song Table Pointer {U}
    ldr r2,[r2]
    lsl r1 ,r0 ,#0x3    @SongID*8
    add r2,r1
    ldrh r1,[r2,#0x4]
    cmp r1,#0x6         @曲の優先度の数字が5未満ならBGM, そうじゃければSE
    bge Play_SE

    Play_BGM:
    mov r1, #0x80
    lsl r1 ,r1 ,#0x1
    @blh 0x08073f38   @BGMを切り替える(別命令 その2) r0=BGM番号:MUSIC {J}
    blh 0x08071a54   @BGMを切り替える(別命令 その2) r0=BGM番号:MUSIC {U}

    mov  r4, #0x01   @
    @ldr r3,=0x08074d6e+1 @For FE8J
    ldr r3,=0x08072892+1 @For FE8U
    bx  r3

    Play_SE:
    @blh 0x080d4ef4   @効果音を鳴らす関数 m4aSongNumStart {J}
    blh 0x080d01fc   @効果音を鳴らす関数 m4aSongNumStart {U}
    @b NotMatch

NotMatch:
mov  r4, #0x00

@ldr	r3,=0x08074D14+1 @For FE8J
ldr	r3,=0x08072838+1 @For FE8U
bx	r3

.align
.ltorg
MultiDancerClassTable:
@list of the Data List sizeof 8bytes  0x00==TERM
@struct
@byte unitid   00=ANY
@byte classid  00=ANY
@byte lvl      00=ANY
@byte rand     100=ANY
@short flag    00=ANY
@ushort song-id
