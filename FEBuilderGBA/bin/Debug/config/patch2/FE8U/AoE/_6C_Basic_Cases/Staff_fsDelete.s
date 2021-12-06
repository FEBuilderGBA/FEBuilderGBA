.thumb
.include "../_TargetSelectionDefinitions.s"

.set PurgeVRAM, 0x8003D20	@{U}
@.set PurgeVRAM, 0x08003C50	@{J}

push {r4,r14}
mov 	r4, r0
ldr r0, =ppRangeMapRows
ldr r0, [r0]
mov r1, #0x0

_blh Map_Fill
_blh MoveRange_HideGfx

@bl AoE_ClearGraphics

_blh BottomHelpDisplay_EndAll
_blh PurgeVRAM

pop 	{r4}
pop 	{r3}
bx	r3
.ltorg
.align
