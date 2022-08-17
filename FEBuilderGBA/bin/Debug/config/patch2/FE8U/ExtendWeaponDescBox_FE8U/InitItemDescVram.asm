.thumb 
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ InitVramRow, 0x80045FC		@{U}
@.equ InitVramRow, 0x8004504		@{J}
.equ CpuFastSet, 0x80D1674		@{U}
@.equ CpuFastSet, 0x80D636C		@{J}
.equ HelpTextHandles, 0x203E794	@{U}
@.equ HelpTextHandles, 0x203E790	@{J}
.equ ProcFind, 0x8002e9c		@{U}
@.equ ProcFind, 0x8002DEC		@{J}

.equ gProc_TradeMenu, 0x859BB1C		@{U}
@.equ gProc_TradeMenu, 0x85C3FFC		@{J}
.equ gProc_TradeMenu_HelpBoxControlSub,	0x859BBD4	@{U}
@.equ gProc_TradeMenu_HelpBoxControlSub,	0x85C40B4	@{J}
.equ gProc_HelpBoxControl, 0x8A00A98	@{U}
@.equ gProc_HelpBoxControl, 0x8A72B50	@{J}
.equ DeleteFaceByIndex, 0x8005758	@{U}
@.equ DeleteFaceByIndex, 0x8005660	@{J}
.equ gpFaceProcs, 0x3004980		@{U}
@.equ gpFaceProcs, 0x3004920		@{J}
.equ TradeMenu_InitGraphics_Jump, 0x802D7EA|1	@{U}
@.equ TradeMenu_InitGraphics_Jump, 0x802D722|1	@{J}

push {r4-r7, lr} 

ldr r5, =0x10000D8 @ Vanilla uses this 
ldr r6, =0x44444444

@ mov r0, r4 
@ add r0, #0x18 
blh InitVramRow 

@ [203E7C4]!!
ldr r0, =HelpTextHandles+0x30 @ vram tile to use 
ldrh r0, [r0] 
ldr r1, =0x3FF 
and r0, r1 
lsl r0, #5 @ << 5 
ldr r1, =0x6010000
add r7, r0, r1 

mov r0, r4 
add r0, #0x20 
blh InitVramRow 
mov r0, r4 
add r0, #0x28 
blh InitVramRow 

ldr r4, ProcExceptionsList @ if any of these procs are running, then don't allocate extra vram 
sub r4, #4 
Loop:
add r4, #4 
ldr r0, [r4] 
cmp r0, #0 
beq BreakLoop 
blh ProcFind 
cmp r0, #0 
beq Loop 
b Exit2 

BreakLoop: 

sub sp, #8 

@Init4Menu
str r6, [sp] 
mov r0, sp 
mov r1, r7 
ldr r2, =0x1800 
add r1, r2 
mov r2, r5 
blh CpuFastSet 

str r6, [sp] 
mov r0, sp 
mov r1, r7 
ldr r2, =0x1C00 
add r1, r2 
mov r2, r5 
blh CpuFastSet 

@Resolves various conflicts with the location of the TradeMenu exchange partner's face image during the initialization of line 5.
ldr r0, =gProc_TradeMenu_HelpBoxControlSub
blh ProcFind
cmp r0, #0x0
beq InitFiveMenu	@if not TradeMenu, so initialize 5 rows regardless.

ldr r1,[r0,#0x4]	@gProc_TradeMenu_HelpBoxControlSub->CurrentLine
ldr r2, =gProc_TradeMenu_HelpBoxControlSub + 16
cmp r1, r2
ble OnOpenTradeMenuHelpBox

OnCloseTradeMenuHelpBox:
bl  Restore_TrandeMenuFace1
b   Exit

OnOpenTradeMenuHelpBox:
ldr r0, =gProc_HelpBoxControl
blh ProcFind
cmp r0, #0x0
beq TradeMenuButNot5Lines  @Unable to discern. I'll assume it's not 5 lines for now.

mov  r1, #0x46       @gProc_HelpBoxControl->Height
ldrh r1, [r0, r1]
cmp  r1, #0x50
bge  TradeMenuAnd5Lines

TradeMenuButNot5Lines:
bl  Restore_TrandeMenuFace1
b   Exit

TradeMenuAnd5Lines:
@This is a 5-line display on the TradeMenu, 
@so it erases the face image of the exchange partner
@After that, initialize the 5th line of VRAM
mov r0, #0x1 @Delete trade partner portrait
blh DeleteFaceByIndex

@b  InitFiveMenu

InitFiveMenu:
str r6, [sp] 
mov r0, sp 
mov r1, r7 
ldr r2, =0x2000 
add r1, r2 
mov r2, r5 
blh CpuFastSet 

str r6, [sp] 
mov r0, sp 
mov r1, r7 
ldr r2, =0x2400 
add r1, r2 
mov r2, r5 
blh CpuFastSet 

Exit: 
add sp, #8 
Exit2: 

pop {r4-r7}
pop {r0} 
bx r0 



@Redrawing a Trade Partner's Portrait
Restore_TrandeMenuFace1:
push {lr}
ldr r0, =gpFaceProcs
ldr r0, [r0, #0x4]	@gpFaceProcs[1]
cmp r0, #0x0
bne Restore_TrandeMenuFace1_Exit	@If FACE is displayed, there is no need to restore.

ldr r0, =gProc_TradeMenu
blh ProcFind
cmp r0, #0x0
beq Restore_TrandeMenuFace1_Exit	@TradeMenu Procs can never be missing, but I'll give you an error handle just in case.

bl Restore_TrandeMenuFace1_Main

Restore_TrandeMenuFace1_Exit:
pop {r0}
bx r0


@This is black magic.
@Execute only the last part of TradeMenu_InitGraphics(0802D794 {U})
@The reason for all this weirdness is for compatibility with the capture and blink hacks.
Restore_TrandeMenuFace1_Main:
push {r4,r5}
sub sp, #0x4

mov r5, r0   @trade proc
mov r4, #0x0
ldr r3, =TradeMenu_InitGraphics_Jump
bx r3



.ltorg 
ProcExceptionsList: 

