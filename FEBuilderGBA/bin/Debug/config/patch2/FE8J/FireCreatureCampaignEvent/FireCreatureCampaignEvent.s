.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@Hook 080BF1D4	{J}
@r5 = this procs

ldrb r1, [r2, #0x0] @WMEventRelatedStruct@gSomeWMEventRelatedStruct.byte
mov r0, #0x2
and r0 ,r1
push {r0}
cmp r0, #0x0
beq Return

ldr r0, =0x0202BCEC@	@ChapterInfo	@{J}
ldrb r1, [r0, #0x14]	@ChapterInfo->ChapterStuff
mov r2, #0x4	@大陸の魔物退治

and r1, r2
cmp r1, r2
bne Return


CheckTriggerFlag:
ldr r4, EVENTOBJ
ldrh r0, [r4,#0x2]	@判定フラグ
cmp r0, #0x0
beq CheckAchievementFlag

blh 0x080860d0	@CheckFlag	@{J}
cmp r0, #0x0
beq Return


CheckAchievementFlag:
ldrh r0, [r4,#0x0]	@達成フラグ
cmp r0, #0x0
beq FireEvent

blh 0x080860d0	@CheckFlag	@{J}
cmp r0, #0x0
bne Return

ldrh r0, [r4,#0x0]	@達成フラグ
blh 0x080860a8	@SetFlag	@{J}

FireEvent:
pop {r0}
ldr r0, [r4,#0x4]	@イベント
ldr r3, =0x080BF24A|1	@{J}
bx  r3


@戻す
Return:
pop {r0}
cmp r0, #0x0
ldr r3, =0x080BF1DC|1	@{J}
bx  r3

.ltorg
EVENTOBJ:
