@
@預り所。イベントから輸送隊を呼び出す
@Call Supply
@
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.thumb

push {lr,r4}

mov r3, r0     @this procs

@actionData 構造体への設定はいらないかもしれない.
@ldr r1, =0x0203A954  @ActionData@gActionData._u00	{J}
ldr r1, =0x0203a958  @ActionData@gActionData._u00	{U}
mov r0, #0x1d
strb r0, [r1, #0x11] @ActionData@gActionData.unitActionType


@ldr r0, =0x03004DF0  @操作キャラのワークメモリへのポインタ	{J}
ldr r0, =0x03004E50  @操作キャラのワークメモリへのポインタ	{U}

@同行したときに、RAMUnit->unitがnullになってしまうことがあるのでつい検証
ldr r4, [r0, #0x0]   @RAMUnit
cmp r4, #0x0
beq Exit
ldr r0, [r4]  @RAMUnit->unit
cmp r0, #0x0
beq Exit

@ldr r0, =0x08A95030   @ Supply Procs	{J}
ldr r0, =0x08A192EC  @ Supply Procs	{U}
mov r1, r3            @ this procs
@blh 0x08002C30        @ NewBlocking6C	{J}
blh 0x08002CE0       @ NewBlocking6C	{U}
str r4, [r0, #0x2c]
add r0, #0x30
mov r1, #0x1
strb r1, [r0, #0x0]

Exit:
pop {r4}
pop {r0}
bx r0
