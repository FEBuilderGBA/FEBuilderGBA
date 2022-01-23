.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@r0  this procs
push {r4,r5,r6,r7,lr}
mov  r7, r0 @procs

@サウンドルームの総件数を数える
blh 0x080b38b4   @SoundRoom_GetAllSongCount	{J}
@blh 0x080AEC94   @SoundRoom_GetAllSongCount	{U}
mov r4, r0

mov r6, #0x0
RandomSongLoop:
add r6, #1
cmp r6, #11     @11回選出しても取れない場合は、あきらめる
bge Compromise
cmp r6, #10     @10回選出しても取れない場合は、先頭からスキャンする
bge ScanBeginning

mov r0, r4   @サウンドルームの最大数
blh 0x08000c58	@NextRN_N	{J}
@blh 0x08000C80	@NextRN_N	{U}
mov r5, r0

NearLoop:

@SoundRoomBuffer 6c procsには1000個入らないので、AI部のメモリスペースを拝借します。
@サウンドルームではAIは動かないので気にしない。
@この領域は、サウンドルームに入った時に割り当てられる。
ldr r0, =0x0203A90C		@{J}
@ldr r0, =0x0203A910	@{U}
mov r1, r5

bl CheckSoundRoomID
cmp r0, #0x0
beq GetNearSong

mov r0, r7 @this->procs
mov r1, r5
bl  IsPlayedSong
cmp r0, #0x1
beq RandomPlay

GetNearSong:
add r5, #0x1
cmp r5, r4     @サウンドルームの最大数を越えていなければ、リストを増やす
blt NearLoop
b   RandomSongLoop

ScanBeginning:
mov r5, #0x0
b   NearLoop

Compromise:
ldrh r5, [r7, #0x32] @play sound room id
cmp  r5, r4          @サウンドルームの最大数を越えいたらどうしようもないので、先頭の曲を再生する
blt  RandomPlay
mov  r5, #0x0

RandomPlay:
mov r0 ,r7       @procs
mov r1 ,r5       @play sound room id
mov r2, #0x20
blh 0x080b4414   @SoundRoom 指定した曲を再生する	@{J}
@blh 0x080af7f4   @SoundRoom 指定した曲を再生する	@{U}

mov r0 ,r5
blh 0x080b4c30   @SoundRoom 曲名を表示2	@{J}
@blh 0x080b0018   @SoundRoom 曲名を表示2	@{U}

@記録
mov r0 ,r7       @procs
mov r1 ,r5       @play sound room id
bl RecordPlayedSong

@内部モードの切り替え。random modeを有効に
mov r0, r7
add r0, #0x30
mov r1, #0x1
strb r1, [r0]

mov r0, r7
add r0, #0x31
mov r1, #0x0
strb r1, [r0]

mov r0, r7
add r0, #0x3f
mov r1, #0x0
strb r1, [r0]

@再生位置のスライダーの初期化
mov r0, r7
add r0, #0x2c
mov r1, #0x1
strh r1, [r0]

mov r0, #0x01
pop {r4,r5,r6,r7}
pop {r1}
bx r1

IsPlayedSong:
push {r4,r5,lr}
mov r4, r0 @this->procs
mov r5, r1 @song id

ldrh r0, [r4, #0x32] @play sound room id
cmp  r5, r0
beq  IsPlayedSong_False

ldr  r3, =0x02021388	@{U}	{J}	@サウンドルームランダム再生時のプレイリスト
ldrh r2, [r3]
mov r0, #0x7e
and r2, r0    @念のため、偶数化する

add r3, #0x2
IsPlayedSong_Loop:
cmp r2, #0x0
beq IsPlayedSong_True    @再生済みリストの中には存在しない

add r0,r3 ,r2
ldrh r0, [r0]
cmp  r5, r0
beq  IsPlayedSong_False  @再生済み

sub r2, r2, #0x2
b   IsPlayedSong_Loop


IsPlayedSong_True:
mov r0, #0x1
b   IsPlayedSong_Exit

IsPlayedSong_False:
mov r0, #0x0

IsPlayedSong_Exit:
pop {r4,r5}
pop {r1}
bx r1


RecordPlayedSong:
push {r4,r5,lr}
mov r4, r0 @this->procs
mov r5, r1 @song id

ldr  r3, =0x02021388	@{U}	{J}	@サウンドルームランダム再生時のプレイリスト
ldrh r2, [r3]
mov r0, #0x7e
and r2, r0    @念のため、偶数化する

cmp r2, #0x0  @00の位置は数の記録に使うのでどける
beq RecordPlayedSong_Reset

cmp r2, #0x80 @終端を越えていればリセット
bge RecordPlayedSong_Reset

RecordPlayedSong_Write:
strh r5, [r3, r2]

add  r1, r2, #0x2
strh r1, [r3]
b    RecordPlayedSong_Exit

RecordPlayedSong_Reset:
mov  r2, #0x0
strh r5, [r3, r2]

mov  r1, #0x2
strh r1, [r3]

RecordPlayedSong_Exit:

pop {r4,r5}
pop {r0}
bx r0


CheckSoundRoomID:
push {r4,r5,lr}
mov r4, r0
mov r5, r1

@convert to bitflag
asr r0 ,r5 ,#0x5
lsl r0 ,r0 ,#0x2
add r3 ,r4, r0
mov r0, #0x1f
and r0 ,r5
mov r2, #0x1
lsl r2 ,r0
ldr r1, [r3, #0x0]
mov r0 ,r1
and r0 ,r2
cmp r0, #0x0
bne TrueExit

FalseExit:
mov r0, #0x0
b   Exit

TrueExit:
mov r0, #0x1

Exit:

pop {r4,r5}
pop {r1}
bx r1
