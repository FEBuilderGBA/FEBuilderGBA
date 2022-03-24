@Hook 08050C44 @FE8J
@Hook 0804FED0 @FE8U
@r0 VCount

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

LDR r3, =0x02024CC0 @ FE8J FE8U (KeyStatusBuffer@KeyStatusBuffer.FirstTickDelay )
LDRH r3, [r3, #0x4] @ KeyStatusBuffer@KeyStatusBuffer.Current
MOV r2, #0x1
AND r3 ,r2
beq WaitExit

@Aボタンが押されているので早くする!

@Dummy GeneralVBlankHandler
ldr r1, =0x03007FF8 @FE8J FE8U IRQCheckFlags
mov r0, #0x1
strh r0, [r1, #0x0]   @IRQCheckFlags
blh 0x08000cf0   @FE8J IncrementGlobalClock
@blh 0x08000d40   @FE8U IncrementGlobalClock

ldr r0, =0x02026A70 @FE8J FE8U 6CPointer@MainArray[0].pointer
ldr r0, [r0, #0x0]
blh 0x08002dd4   @FE8J Exec6C
@blh 0x08002e84   @FE8U Exec6C
blh 0x080020cc   @FE8J FlushPrimaryOAM
@blh 0x0800217c   @FE8U FlushPrimaryOAM

ldr r1, =0x0202BCAC @FE8J BattleMapState@gGameState.boolMainLoopEnded
@ldr r1, =0x0202BCB0 @FE8U BattleMapState@gGameState.boolMainLoopEnded
ldrb r0, [r1, #0x0] @BattleMapState@gGameState.boolMainLoopEnded
cmp r0, #0x0
beq Exit
    mov r0, #0x0
    strb r0, [r1, #0x0]   @BattleMapState@gGameState.boolMainLoopEnded
    blh 0x08000e4c   @FE8J FlushLCDControl
    @blh 0x08000e9c   @FE8U FlushLCDControl
    blh 0x080010fc   @FE8J FlushBackgrounds
    @blh 0x0800114c   @FE8U FlushBackgrounds
    blh 0x08001fd8   @FE8J FlushTiles
    @blh 0x08002088   @FE8U FlushTiles
    blh 0x08002088   @FE8J FlushSecondaryOAM
    @blh 0x08002138   @FE8U FlushSecondaryOAM
b   Exit

WaitExit:
@Aボタンが押されていないので、VBlankIntrWaitで待機させる
blh 0x080d63d8   @FE8J VBlankIntrWait
@blh 0x080d16dc   @FE8U VBlankIntrWait

Exit:
pop {r0}
bx r0

.ltorg
