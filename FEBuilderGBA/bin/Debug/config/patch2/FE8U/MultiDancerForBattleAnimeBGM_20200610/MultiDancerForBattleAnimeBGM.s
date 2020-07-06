@Call 080727FC
@r0    ram pointer

@r0 work (unit id)
@r1 work (class id)
@r3 work (table)
@r4	bool (result: is_match)
@r7	ram unit

.thumb
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.org 0x0
ldr r3, MultiDancerClassTable
sub r3, #0x4

Loop:
add r3, #0x4
ldr r0, [r3,#0x00]
cmp  r0,#0x00
beq  NotMatch

CheckUnit:
ldrb r0, [r3,#0x00]
cmp  r0,#0x00
beq  CheckClass

ldr r1,[r7]
ldrb r1,[r1,#0x4]  @Unit->UnitID
cmp  r0,r1   @Check UnitID
bne  Loop

CheckClass:
ldrb r0, [r3,#0x01]
cmp  r0,#0x00
beq  CheckDanceOrPlay

ldr r1,[r7,#0x4]
ldrb r1,[r1,#0x4]  @Class->UnitID
cmp  r0,r1   @Check ClassID
bne  Loop

CheckDanceOrPlay: @Check Dance or Play

    ldr r0, =0x0203A4D4  @(AttackerBattleStruct@AttackerBattleStruct.Seems to be a bitfield (0x2 seems to be 'battle hasn't started yet') (0x20 is arena))	{U}
    ldrh r1, [r0, #0x0]
    mov r0, #0x40
    and r0 ,r1
    cmp r0, #0x0
    beq NotMatch

Match:
    ldrh r0, [r3,#0x02]  @Table->BGM

    @BGMがSEかBGMか判定する
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
    blh 0x08071a54   @BGMを切り替える(別命令 その2) r0=BGM番号:MUSIC

    mov  r4, #0x01   @不要だと思うが多少はね。
    ldr r3,=0x08072892+1 @For FE8U
    bx  r3

    Play_SE:
    blh 0x080d01fc   @効果音を鳴らす関数 m4aSongNumStart {U}
    @b NotMatch

NotMatch:
mov  r4, #0x00

Exit:
ldr	r3,=0x08072838+1 @For FE8U
bx	r3

.ltorg
MultiDancerClassTable:
@list of the Data List sizeof 4bytes  0x00==TERM
@struct
@byte unitid   00=ANY
@byte classid  00=ANY
@ushort song-id
