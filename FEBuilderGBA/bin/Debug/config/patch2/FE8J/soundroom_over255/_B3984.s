.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {r4,r5,r6,lr}
mov r6 ,r0		@Save This Proc

@まずこのふざけたSoundRoom Procs構造体のデータ部を初期化する
mov r0 ,r6
add r0 ,#0x2c
mov r1 ,#0x0
mov r2 ,#0x6c - 0x2c
blh 0x080D6968	@memset	{J}
@blh 0x080d1c6c		@memset	{U}

@AIの行動を定義する領域はSoundRoomでは同時に使われないので再利用します
ldr r0, =0x0203A90C	@{J}
@ldr r0, =0x0203A910	@{U}
mov r1 ,#0x0
mov r2 ,#0xff   @ビットフラグなので、0xff *8 = 2040個格納できます
blh 0x080D6968	@memset	{J}
@blh 0x080d1c6c		@memset	{U}

@サウンドルームの総件数を数える
blh 0x080b38b4   @SoundRoom_GetAllSongCount	{J}
@blh 0x080AEC94   @SoundRoom_GetAllSongCount	{U}

@総数をprocsの所定の領域に格納する
mov r2, #0x6a
strh r0, [r6, r2]

@聞いたことがある曲リスト
ldr r0, =0x0203A90C	@{J}
@ldr r0, =0x0203A910	@{U}
blh 0x080a8890   @LoadSoundRoomEnableList	{J}
@blh 0x080A3E4C   @LoadSoundRoomEnableList	{U}

@ループを回そう
ldr r0,=0x080B5044 @SoundRoom Pointer	{J}
@ldr r0,=0x0801BC14 @SoundRoom Pointer	{U}
ldr r4, [r0]
mov r5, #0x0	@Song Number

EnableLoop:

ldr r0, [r4,#0x0] @SoundRoom->SongID
cmp r0, #0x0
blt EnableLoop_Exit

@サウンドルームUnlockが有効な場合すべて公開する
ldr r0,=0x080B3A00	@{J}
@ldr r0,=0x080AEDE0		@{U}
ldrb r0,[r0]
cmp r0,#0xB
bne EnableSong

@表示条件のチェック
bl  CheckASMCond
cmp r0,#0x1
beq EnableSong
b   EnableLoopNext


EnableSong:
@そのSongIDに対応するビットマスクをONにします
asr r2 ,r5 ,#0x5
lsl r2 ,r2 ,#0x2

mov r0 ,r5

mov r1, #0x1f
and r0 ,r1

mov r1 ,#0x01
lsl r1 ,r0

ldr r3, =0x0203A90C	@{J}
@ldr r3, =0x0203A910	@{U}
ldr r0, [r3, r2]
orr r0 ,r1
str r0, [r3, r2]

EnableLoopNext:
add r4, #16     @次のデータへ
add r5, #1      @Song Number ++
b EnableLoop

EnableLoop_Exit:

@達成率の計算
bl CalcComplateRate

pop {r4,r5,r6}
pop {r0}
bx r0

@達成率の計算
@r5   total_count   @keep
@r6   this procs    @keep
@
@達成率計算式
@ this_proc->0x34 = enable_count * 100 / total_count
@
CalcComplateRate:
push {lr}

bl  CountEnableSongs  @聞ける曲数を数える
mov r1, #0x64
mul r0 ,r1
mov r1, r5
blh 0x080d65f8		@__divsi3	@{J}
@blh 0x080D18FC		@__divsi3	@{U}
mov r1, #0x34
strb r0, [r6, r1]
pop {r1}
bx r1


CheckASMCond:
@r4 sound room struct	@keep
@r5 sound room number	@keep
@r6 this procs			@keep
push {lr}

ldr r0, [r4, #0x8] @SoundRoom->ASM
cmp r0,#0x0
beq CheckASMCond_False

cmp r0,#0xFF
ble CheckASMCond_SomeSong

ldr r1, =0x08000000
cmp r0,r1
blt CheckASMCond_False

ldr r1, =0x0A000000
cmp r0,r1
bge CheckASMCond_False

CheckASMCond_CallASM:
mov r2 ,r0
ldr r1, [r4, #0x0] @SoundRoom->SongID
mov r0 ,r6
blh 0x080d65c4   @_call_via_r2	{J}
@blh 0x080D18C8   @_call_via_r2	{U}
cmp r0,#0x0
beq CheckASMCond_False
b   CheckASMCond_True


CheckASMCond_SomeSong:
@同一SongIDならチェック不可能なので聞けることにする
cmp  r5, r0
beq  CheckASMCond_True

@その曲が聞けるならOKというゆるいチェック
sub  r0 ,#0x1  @曲が聞けるか判定は00から始まるので、1引きます。
asr r2 ,r0 ,#0x5
lsl r2 ,r2 ,#0x2

@mov r0 ,r0

mov r1, #0x1f
and r0 ,r1

mov r1 ,#0x01
lsl r1 ,r0

ldr r3, =0x0203A90C	@{J}
@ldr r3, =0x0203A910	@{U}
ldr r0, [r3, r2]
and r0 ,r1
beq CheckASMCond_False
b   CheckASMCond_True

CheckASMCond_False:
mov r0, #0x0
b   CheckASMCond_Exit

CheckASMCond_True:
mov r0, #0x1

CheckASMCond_Exit:
pop {r1}
bx r1

@聞ける曲数を数える
@r5   total_count   @keep
CountEnableSongs:
push {r4,r6,lr}
mov r4, #0x0 @counter
mov r6, #0x0 @soung counter

CountEnableSongs_Loop:
cmp r6 ,r5
bge CountEnableSongs_Exit

asr r2 ,r6 ,#0x5
lsl r2 ,r2 ,#0x2
mov r0 ,r6

mov r1, #0x1f
and r0 ,r1

mov r1 ,#0x01
lsl r1 ,r0

ldr r3, =0x0203A90C	@{J}
@ldr r3, =0x0203A910	@{U}
ldr r0, [r3, r2]
and r0, r1
beq CountEnableSongs_Next

add r4, #0x01  @聞ける曲が増えた

CountEnableSongs_Next:
add r6, #0x01  @次の曲へ

b CountEnableSongs_Loop

CountEnableSongs_Exit:
mov  r0,r4
pop  {r4,r6}
pop  {r1}
bx r1
