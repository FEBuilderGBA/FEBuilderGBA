.thumb 
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.macro blh_free to, reg=r3
  push {\reg}
  ldr \reg, =\to
  mov lr, \reg
  pop {\reg}
  .short 0xf800
.endm


	

.global ASMC_Draw
.type ASMC_Draw, %function 

ASMC_Draw:
push {r4-r5, lr} 
mov r4, r0 
mov r1, r4 @ Parent proc 
ldr r0, =DrawSpriteProc
@ arguments: r0 = pointer to ROM 6C code, r1 = parent; returns: r0 = new 6C pointer (0 if no space available)
blh pr6C_NewBlocking
@mov r1, #3 
@blh pr6C_New


pop {r4-r5}
pop {r0} 
bx r0 

.align 4
.global Draw_SetupMemorySlots
.type Draw_SetupMemorySlots, %function 

@ This function is only used for battles. The ASMC / AoE version does not use this. 
Draw_SetupMemorySlots:
push {lr}


@ldr r3, =0x203a608 @gpCurrentRound	{U}
@ldr r0, [r3] 
@ldr r1, [r0] 
@mov r11, r11 

@bl Draw_GetActiveCoords
@mov r11, r11 


bl Draw_GetAnimationIDByWeapon @ Takes no params, returns animation id to use 
ldr r3, =MemorySlot 
str r0, [r3, #4] @ slot 1 - animation ID 



@ copied from function 080815C0 //MapAnim_MoveCameraOnTarget MapAnim_MoveCameraOnTarget
ldr r3, =0x203E1F0 @(gMapAnimStruct )	{U}
@ldr r3, =0x0203E1EC @(gMapAnimStruct )	{J}

mov r1, r3 
add r1, #0x59 
ldrb r2, [r1]
lsl r1, r2, #2 
add r1, r2 
lsl r1, #2 
add r1, r3 
ldr r2, [r1] 
mov r1, #0x10 
ldsb r0, [r2, r1] @ XX 
ldrb r1, [r2, #0x11] @ YY 
lsl r1, #24 
asr r1, #24 
@ bl EnsureCameraOntoPosition - this is what the function usually does 


ldr r3, =MemorySlot 
add r3, #4*0x0B 

@ldr r2, =CurrentUnit 
@ldrb r0, [r2, #0x10] 
@ldrb r1, [r2, #0x11] 

strh r0, [r3] 
strh r1, [r3, #2] 

pop {r1}
bx r1 

	.equ BreakProcLoop, 0x08002E94	@{U}
@	.equ BreakProcLoop, 0x08002DE4	@{J}
.align 
.ltorg

.equ RegisterTileGraphics, 0x08002014	@{U}
@.equ RegisterTileGraphics, 0x08001F64	@{J}
.equ RegisterObjectTileGraphics, 0x08012FF4	@{U}
@.equ RegisterObjectTileGraphics, 0x080130AC	@{J}

.global Draw_StoreToBuffer
.type Draw_StoreToBuffer, %function 
Draw_StoreToBuffer:
push {r4-r7, lr}



mov r1, #0 
str r1, [r0, #0x30] @ store 0 to Proc + 0x68
@ initial game time 





ldr r0, =SaveScreenNumbers 
ldr r1, =0x6013800 @ tile where numbers are usually 
mov r2, #6
mov r3, #2
@ Arguments: r0 = Source gfx (uncompressed), r1 = Target pointer, r2 = Tile Width, r3 = Tile Height
blh RegisterObjectTileGraphics, r4

@ default 8x8 sprite uses 24th palette? 
@ we should set the palette to something, at least 
@ 24th palette used by transformed myrrh 
@ldr r0, =0x80A8EE4 @ poin to save menu palette (for the numbers to draw)	{U}
ldr r0, =SaveScreenNumbersPal
mov r1, #27 @ usual palette # 
lsl r1, #5 @ multiply by #0x20
mov	r2,#0x20
blh CopyToPaletteBuffer @Arguments: r0 = source pointer, r1 = destination offset, r2 = size (0x20 per full palette)



@ AoE test 
@ldr r0, =0x202E4E0 @ Movement map	{U}
@ldr r0, [r0] 
@mov r1, #0xFF
@blh FillMap
@
@ldr r3, =MemorySlot 
@add r3, #4*0x0B 
@ldrh r0, [r3] @ XX 
@ldrh r1, [r3, #2] @ YY 
@ldr r2, =RangeTemplateTable_Smile1
@bl CreateMoveMapFromTemplate



pop {r4-r7}
pop {r1}
bx r1 



.align 4
.ltorg

.equ CopyToPaletteBuffer, 0x8000DB8	@{U}
@.equ CopyToPaletteBuffer, 0x8000D68	@{J}
.equ PushToSecondaryOAM, 0x08002BB8	@{U}
@.equ PushToSecondaryOAM, 0x08002B08	@{J}
.equ GetGameClock, 0x08000D28	@{U}
@.equ GetGameClock, 0x08000CD8	@{J}




.global Draw_Camera
.type Draw_Camera, %function
Draw_Camera:
push {lr}
ldr r3, =MemorySlot
add r3, #4*0x0B
ldrb r1, [r3] @ XX 
ldrb r2, [r3, #2] @ YY 

@r0 as parent 
@blh 0x8015D84 @CenterCameraOntoPosition	{U}
mov r0, #0 
blh EnsureCameraOntoPosition

pop {r0} 
bx r0 

.ltorg 
.align 

.equ FillMap,                    0x080197E4	@{U}
@.equ FillMap,                    0x080194BC	@{J}
.equ pr6C_NewBlocking,           0x08002CE0	@{U}
@.equ pr6C_NewBlocking,           0x08002C30	@{J}
.equ pr6C_New,                   0x08002C7C	@{U}
@.equ pr6C_New,                   0x08002BCC	@{J}
.equ Proc_CreateBlockingChild,   0x080031c4	@{U}
@.equ Proc_CreateBlockingChild,   0x08003110	@{J}
.equ BreakProcLoop,              0x08002E94	@{U}
@.equ BreakProcLoop,              0x08002DE4	@{J}
.equ ProcFind,                   0x08002E9C	@{U}
@.equ ProcFind,                   0x08002DEC	@{J}
.equ EnsureCameraOntoPosition,   0x08015e0c	@{U}
@.equ EnsureCameraOntoPosition,   0x08015E18	@{J}
.equ CheckEventId,               0x08083da8	@{U}
@.equ CheckEventId,               0x080860D0	@{J}
.equ MemorySlot,                 0x030004B8	@{U}
@.equ MemorySlot,                 0x030004B0	@{J}
.equ CurrentUnit,                0x03004E50	@{U}
@.equ CurrentUnit,                0x03004DF0	@{J}
.equ EventEngine,                0x0800D07C	@{U}
@.equ EventEngine,                0x0800D340	@{J}


.global Draw_WaitXFrames
.type Draw_WaitXFrames, %function 
Draw_WaitXFrames:
push {r4, lr}

mov r4, r0 @ Parent? 


ldr r3, =MemorySlot
add r3, #0x0B*4 
ldrh r0, [r3] 
ldrh r1, [r3, #2] 

@ given coordinates to move the camera to, decide whether the camera needs to be moved or not 
@ r0 = XX, r1 = YY 
blh 0x8015e9c @ ShouldCameraMovePos?	@{U}
@blh 0x8015EA8 @ ShouldCameraMovePos?	@{J}
cmp r0, #0 
beq ContinueDraw_Wait
mov r0, #2 @ Waiting for camera 
b End_DrawPause 

ContinueDraw_Wait: 

ldr r0, =DrawSpriteProc
blh ProcFind 
cmp r0, #0 
beq BreakProcLoopNow
mov r5, r0 @ Proc with initial game clock time 

blh GetGameClock @ Current frame 
ldr r2, [r5, #0x30] @ initial game clock time 
cmp r2, #0 
bne NoSetTime
str r0, [r5, #0x30] 
mov r2, r0 
NoSetTime: 
sub r0, r2 @ Number of frames since then 

ldr r2, =MinimumFramesLink
ldr r2, [r2] 
cmp r0, r2 
bgt Continue_DrawPause 
mov r0, #0 
b End_DrawPause @ regardless of animation or not, always pause at least X frames 
Continue_DrawPause:

@ Get total frames now 
ldr r3, =MemorySlot 
ldr r3, [r3, #4] @ Slot 1 as AnimID 
lsl r3, #2 @ x4 
add r2, r3, r3 @ x8 
ldr r3, =AnimTable2
ldr r3, [r3, r2] @ Specific animation table 
cmp r3, #0 
beq NoAnimation

sub r3, #12
mov r2, #0 @ Number of frames to wait counter 

NumberOfFramesLoop:
add r3, #12 
ldrh r1, [r3] 
add r2, r1 @ total frames to wait 
cmp r1, #0 
bne NumberOfFramesLoop


cmp r0, r2
bge BreakProcLoopNow
mov r0, #0 
b End_DrawPause

NoAnimation:
VanillaHP_BarRoutine:
mov r0, r4 @ parent proc 
blh 0x8081914 @ default routine of "wait for hp to finish going down"	@{U}
@blh 0x8083C54 @ default routine of "wait for hp to finish going down"	@{J}

b End_DrawPause

BreakProcLoopNow:
mov r0, r4 @  @ parent to break from 
blh 0x8081914 @ default routine wait for hp to finish going down	@{U}
@blh 0x8083C54 @ default routine wait for hp to finish going down	@{J}

@blh BreakProcLoop
mov r0, #1
End_DrawPause:

pop {r4}
pop {r1}
bx r1 


.type Draw_GetActiveCoords, %function  
Draw_GetActiveCoords:
push {lr}

ldr r3, =0x203E1F0 @(gMapAnimStruct )	@{U}
@ldr r3, =0x0203E1EC @(gMapAnimStruct )	@{J}
mov r1, r3 
add r1, #0x59 
ldrb r2, [r1]
lsl r1, r2, #2 
add r1, r2 
lsl r1, #2 
add r1, r3 
ldr r2, [r1] 
mov r1, #0x10 
ldsb r0, [r2, r1] @ XX 
ldrb r1, [r2, #0x11] @ YY 
lsl r1, #24 
asr r1, #24 


pop {r2}
bx r2 
.align 4 

.global Draw_GetActiveAttackerOrDefender
.type Draw_GetActiveAttackerOrDefender, %function 
Draw_GetActiveAttackerOrDefender:
push {r4, lr} 

bl Draw_GetActiveCoords @ I guess this returns the target's coords? 


ldr r3, =0x203A4EC @ Atkr	@{U}
@ldr r3, =0x0203A4E8 @ Atkr	@{J}
ldr r4, =0x203A56C @ Dfdr	@{U}
@ldr r4, =0x0203A568 @ Dfdr	@{J}

ldrb r2, [r3, #0x10]
cmp r2, r0
bne TryDfdr
ldrb r2, [r3, #0x11] 
cmp r2, r1 
bne TryDfdr 
b ExitDraw_GetActiveAttackerOrDefender


TryDfdr:
ldr r3, =0x203A56C @ Dfdr	@{U}
@ldr r3, =0x0203A568 @ Dfdr	@{J}
ldr r4, =0x203A4EC @ Atkr	@{U}
@ldr r4, =0x0203A4E8 @ Atkr	@{J}

ldrb r2, [r3, #0x10]
cmp r2, r0  
bne RetFalse
ldrb r2, [r3, #0x11] 
cmp r2, r1 
bne RetFalse 
b ExitDraw_GetActiveAttackerOrDefender


RetFalse:
mov r3, #0
mov r4, #0  
ExitDraw_GetActiveAttackerOrDefender:
mov r1, r3 @ Target @ we found the target's coords, so let's instead use the active unit 
mov r0, r4 @ Active 

@ r0 has the atkr or dfdr struct 

pop {r4} 
pop {r2}
bx r2


.global Draw_GetAnimationIDByWeapon
.type Draw_GetAnimationIDByWeapon, %function 
Draw_GetAnimationIDByWeapon:
push {r4-r5, lr}


bl Draw_GetActiveAttackerOrDefender 

cmp r0, #0 
beq Error 
mov r4, r0 

@ Current unit's battle struct is in r4 
mov r0, r4
add r0, #0x4A @ Active unit's weapon 
ldrb r0, [r0] @ Weapon ID 

mov r2, #0 @ Counter 
ldr r5, =0xFFFF @ Terminator 
ldr r3, =SpecificWeaponAnimations
sub r3, #2 @ 2 bytes per 
AnimationBySpecificWeapon_Loop:
add r3, #2 
ldrh r1, [r3] 
cmp r1, r5 
beq BreakAnimationBySpecificWeapon_Loop
ldrb r1, [r3] 
cmp r0, r1 
bne AnimationBySpecificWeapon_Loop
ldrb r0, [r3, #1] @ Animation ID 
b ExitDraw_GetAnimationIDByWeapon @ We found an animation for that specific weapon 

BreakAnimationBySpecificWeapon_Loop:
blh 0x8017548 @GetItemWType	{U}
@blh 0x80172F0 @GetItemWType	{J}

mov r2, #0 @ Counter 
ldr r5, =0xFFFF @ Terminator 
ldr r3, =WeaponTypeAnimations
sub r3, #2 

AnimationByWeaponType_Loop:
add r3, #2 
ldrh r1, [r3] 
cmp r1, r5
beq Error @ No animation found for this weapon type, so error 
ldrb r1, [r3] 
cmp r0, r1 
bne AnimationByWeaponType_Loop
ldrb r0, [r3, #1] @ Animation ID 
b ExitDraw_GetAnimationIDByWeapon

Error:
mov r0, #0 @ 0th animation is none 


ExitDraw_GetAnimationIDByWeapon:

pop {r4-r5}
pop {r1}
bx r1 
.align 


.global Draw_PushToOam
.type Draw_PushToOam, %function 
Draw_PushToOam:
push {r4-r7, lr}

mov r4, r0 

bl Draw_WaitXFrames
cmp r0, #2 
beq Skip 



@ if we miss, do not show an animation 
ldr r3, =0x203E24A @ current round - from function 8161C - address 81676	@{U}
@ldr r3, =0x203E246 @ current round - from function 8161C - address 81676	@{J}

ldrh r1, [r3] 
mov r2, #2 
and r1, r2 
cmp r1, #0 
bne Skip

@ get coordinates 
ldr r3, =MemorySlot 
add r3, #4*0x0B 
ldrh r5, [r3]
ldrh r6, [r3, #2]



mov r0, r5 @ X coord
mov r1, r6 @ Y coord
mov r2, r4 @ parent proc 
bl Draw_NumberDuringBattle

blh GetGameClock 

ldr r2, [r4, #0x30] @ initial game clock time 
sub r0, r2 @ frame we're on



 
ldr r3, =MemorySlot 
ldr r3, [r3, #4] @ Slot 1 
lsl r3, #2 @ x4 
add r2, r3, r3 @ x8 @ fixed
ldr r3, =AnimTable2
ldr r3, [r3, r2] @ Specific animation table 
cmp r3, #0 @ No animation, so exit 
beq ExitAnimation
sub r3, #12 
mov r1, #0 @ frames offset 
b TryNextFrameLoop 
ExitAnimation: 
@7b878 sets to 0 
@ 81698, 7bd3a 
@ldr r3, =0x203E24F @(gMapAnimaionWait )	{U}
@mov r0, #1
@strb r0, [r3] 


b Skip

TryNextFrameLoop:
add r3, #12 
ldrh r2, [r3] 
add r1, r2 
cmp r2, #0 
beq ExitAnimation
cmp r0, r1 @ once current frame is less than the frame offset, we have our frame offset 
bgt TryNextFrameLoop @ nov 14 - swapped from bge to bgt i guess 
mov r7, r3 @ Table offset 
cmp r0, r1 
bne NoSound 

ldrh r0, [r7, #2] @ sfx/bgm ID 
cmp r0, #0 
beq NoSound

@ Only play sound on the exact frame 
blh 0x080D01FC   @m4aSongNumStart r0=music id:SOUND // Seems to work fine for SFX 	{U}
@blh 0x080D4EF4   @m4aSongNumStart r0=music id:SOUND // Seems to work fine for SFX	{J}

@blh 0x08014B28   @ //PlaySpacialSoundMaybe, r0=BGM index, r1 = Unknown //	{U}
@blh 0x080024D4  @ //Switch BGM void r0=BGM Number:MUSIC r1=Unknown //	{U}
NoSound: 




ldr r0, [r7, #8] @ Palette to use 

@UpdatePalette
mov r1, #26 @ palette # 
lsl r1, #5 @ multiply by #0x20
mov	r2,#0x20 @ size 
blh CopyToPaletteBuffer @Arguments: r0 = source pointer, r1 = destination offset, r2 = size (0x20 per full palette)

@ palette must be updated 
ldr	r0,=0x300000E @ this is a byte (bool) that tells the game whether the palette RAM needs to be updated	@{U}
@ldr	r0,=0x300000D @ this is a byte (bool) that tells the game whether the palette RAM needs to be updated	@{J}

mov	r1,#1
strb r1,[r0]






ldr r0, [r7, #4] @ image address 



bl Draw_UpdateVRAM @ push to a buffer

ldr r0, =gGenericBuffer 
ldr r1, =VRAM_Address_Link
ldr r1, [r1] 
ldr r2, =4096 @ number of bytes 
mov r2, #8
mov r3, #8
@ Arguments: r0 = Source gfx (uncompressed), r1 = Target pointer, r2 = Tile Width, r3 = Tile Height
blh RegisterObjectTileGraphics, r4


@ldr r0, =0x859dabc @gProc_Battle	{U}
@blh ProcFind 
@cmp r0, #0 
@beq DrawByMask

ldr r3, =MemorySlot 
ldr r3, [r3, #8] @ Slot 2 
ldr r2, =0xFFFFFFFF 
@mov r11, r11 
cmp r2, r3 
beq DrawByMask @ Only draw by mask if Memory slot 2 is (-1) 

mov r0, r5 @ XX 
mov r1, r6 @ YY 
ldr r2, =VRAM_Address_Link
ldr r2, [r2] 
lsl r2, #16 @ Cut of |0x601---- 
lsr r2, #16 
lsr r2, #5 @ eg. tile #0x198 - offset where we put the tiles 
bl Draw_DisplaySprites

b Skip 

DrawByMask:
mov r0, r5 @ X coord to center on 
mov r1, r6 @ Y 

ldr r2, =VRAM_Address_Link
ldr r2, [r2] 
lsl r2, #16 @ Cut of |0x601---- 
lsr r2, #16 
lsr r2, #5 @ eg. tile #0x198 - offset where we put the tiles 
bl Draw_ForTileInMapDisplaySprites

Skip:


pop {r4-r7}
pop {r1}
bx r1 


.align 

.type Draw_ForTileInMapDisplaySprites, %function 
Draw_ForTileInMapDisplaySprites:
push {r4-r7, lr}

mov r7, r8 
push {r7}
mov r6, r9 
push {r6}

mov r5, r10 
push {r5} 
mov r5, #0 @ counter ? 
mov r10, r5 



mov r8, r2 @ VRAM address 
ldr r4, =0x202E4E0 @ Movement Map	@{U}
@ldr r4, =0x202E4DC @ Movement Map	@{J}
ldr r4, [r4] @ movement map [0,0] 
mov r9, r4 @ movement map 

ldr r3, =0x202E4D4 @ Map Size	@{U}
@ldr r3, =0x202E4D0 @ Map Size	@{J}
ldrh r6, [r3] @ XX Boundary size 
ldrh r7, [r3, #2] @ YY Boundary size 



mov r5, #0 @ Y coord 
sub r5, #1 

YLoop:
add r5, #1 
cmp r5, r7 
bge BreakYLoop

mov r4, #0 
sub r4, #1 
XLoop:
lsl r0, r5, #2 @ 4 times Y coord 
mov r3, r9 @ movement map 
ldr r1, [r3, r0] @ beginning of Y row 

XLoop_2:
add r4, #1 
cmp r4, r6 
bge YLoop @ Finished the row, so +1 to Y coord 
ldrb r0, [r1, r4] @ Xcoord to check 
cmp r0, #0xFF 
beq XLoop_2

mov r1, r10 
add r1, #1 
mov r10, r1 
cmp r1, #30 
blt NoBreak 
b BreakYLoop @ 30+ as too many to display? 
NoBreak:
@ ValidCoord:
@ We found a valid tile 
mov r0, r4 @ XX 
mov r1, r5 @ YY 



@mov r11, r11 

mov r2, r8 @ vram 
bl Draw_DisplaySprites 
b XLoop 


BreakYLoop:

pop {r5} 
mov r10, r5 
pop {r6} 
mov r9, r6 
pop {r7}
mov r8, r7

pop {r4-r7}
pop {r1}
bx r1 

.align 








.type Draw_DisplaySprites, %function 
Draw_DisplaySprites:
push {r4, lr}

mov r3, r2 @ VRAM Tile 

lsl r0, #4 @ 16 pixels per coord 
lsl r1, #4 

sub r0, #24 @ Center X coord 
sub r1, #24  @ Center Y coord 


ldr r2, =0x202BCBC @(gCurrentRealCameraPos )	@{U}
@ldr r2, =0x202BCB8 @(gCurrentRealCameraPos )	@{J}
ldrh r2, [r2]
sub r0, r2
ldr r2, =0x202BCBC @(gCurrentRealCameraPos )	@{U}
@ldr r2, =0x202BCB8 @(gCurrentRealCameraPos )	@{J}
ldrh r2, [r2, #2] 
sub r1, r2

lsl r0, #23 @ only 9 bits used for coords 
lsr r0, #23 
lsl r1, #24 
lsr r1, #24 

@sub sp, #8 
@@ Prepare OAM data
@mov   r2, #0x1 @ 
@mov   r3, sp
@str   r2, [r3]
@mov   r2, #0x0
@str   r2, [r3, #0x4]



mov r2, #3 @ 0, 1, 2, or 3 as valid here?
		@ probably 8, 16, 32, and 64 pixel squares 
lsl r2, #0xe @ bits E-F determine size 
orr r0, r2                    @ Sprite size, 64x64

mov r2, #0x4 @ blend bit
lsl r2, #8 
orr r1, r2 
@ r1 also has sprite shape (default is square, but can also be horizontal or vertical rectangle) 



@mov r3, r3 @ Vram tile 
mov r2, #26 @ palette # 26 - or 27 is the light rune palette i think 
lsl r2, #12 @ bits 12-15 
orr r3, r2 @ palette | flips | tile 

ldr r2, =0x8590f54 @gOAM_32x32Obj - seems to work fine for 64x64 objects, too	@{U}
@ldr r2, =0x85B8CEC @gOAM_32x32Obj - seems to work fine for 64x64 objects, too	@{J}
@mov r2, sp 

blh PushToSecondaryOAM, r4 

@add sp, #8 


pop {r4}
pop {r0}
bx r0





.equ    UnLZ77Decompress, 0x08012F50	@{U}
@.equ    UnLZ77Decompress, 0x08013008	@{J}
.equ    CpuFastSet, 0x080D1674	@{U}
@.equ    CpuFastSet, 0x080D636C	@{J}
.equ    gGenericBuffer, 0x02020188 @ #10016 bytes, I think 	@{U}	@{J}

.type Draw_UpdateVRAM, %function 
Draw_UpdateVRAM:
@ Arguments:
@ r0: lz77 compressed image 
.thumb

push  {r4, r14}

  
@ Decompress image into buffer
ldr   r1, =gGenericBuffer
blh UnLZ77Decompress 

ldr r0, =gGenericBuffer 
ldr r1, =VRAM_Address_Link
ldr r1, [r1] 
mov   r3, #0x80 @ size ? 
mov   r2, #0x20  

blh CpuFastSet, r4 

pop   {r4}
pop   {r1}
bx    r1












.align 4

.global Draw_NumberDuringBattle
.type Draw_NumberDuringBattle, %function 


Draw_NumberDuringBattle:
push {r4-r7, lr}

mov r6, r2 @ parent proc 

lsl r0, #4 
lsl r1, #4 

ldr r3, =0x202BCBC @(gCurrentRealCameraPos )	@{U}
@ldr r3, =0x202BCB8 @(gCurrentRealCameraPos )	@{J}
ldrh r2, [r3]
ldrh r3, [r3, #2] 

sub r0, r2 
sub r1, r3 

lsl r0, #24 @ only 9 bits used for coords 
lsr r0, #24 
lsl r1, #24 
lsr r1, #24 


mov r4, r0 @ XX 
mov r5, r1 @ YY 

@If the flag 0xEE is enabled, the numbers will not be drawn.
ldr r0, =BATTLE_MAPANIMATION_NUMBERS_FLAGLink
ldr r0, [r0]
blh CheckEventId
cmp r0, #0x0
bne ExitDraw_NumberDuringBattle


ldr r0, =0x859dabc @gProc_Battle	@{U}
@ldr r0, =0x85C5F9C @gProc_Battle	@{J}
blh ProcFind 
cmp r0, #0 
beq ExitDraw_NumberDuringBattle


blh GetGameClock 
ldr r2, [r6, #0x30] 
sub r0, r2 @ Number of frames since animation started 
mov r6, r0 
lsr r6, #1 @ every 2 frames move upwards 
cmp r6, #12 
blt Continue_DrawNumber
mov r6, #12 @ max height is +12 above 
Continue_DrawNumber:
sub r5, r6 

lsr r0, r6, #1 

add r0, #4 
DivisionLoop:
sub r0, #4 
cmp r0, #4 
bgt DivisionLoop 

add r4, #4 
sub r4, r0 @ subtract or add based on the remainder so that it will wiggle ? 



ldr r3, =0x203E24A @ current round - from function 8161C - address 81676	@{U}
@ldr r3, =0x203E246 @ current round - from function 8161C - address 81676	@{J}
ldrh r1, [r3] 
mov r2, #2 
and r1, r2 
cmp r1, #0 
bne ExitDraw_NumberDuringBattle

mov r0, r3
bl GetDamage

@If the damage is zero, it will not be drawn.
cmp r0, #0x0
beq ExitDraw_NumberDuringBattle

cmp r0, #99 
ble NoCap 
mov r0, #99 @ Max damage to display, i guess 
NoCap:
mov r7, r0 @ Damage to deal 

mov r1, r7 
cmp r7, #10 
blt SkipTensDigit

add r1, #10 
mov r2, #0 @ counter
sub r2, #1 

 
GetRemainderLoop:
sub r1, #10 
add r2, #1 
cmp r1, #10 
bge GetRemainderLoop 
@r2 as Top digit only 
mov r7, r1 @ remainder only 



mov r0, r4 
mov r1, r5 


bl Draw_NumberOAM

SkipTensDigit:
mov r0, r4 
mov r1, r5 
add r0, #8 @ 8 pixels to the right for ones column 
mov r2, r7 
bl Draw_NumberOAM

ExitDraw_NumberDuringBattle:

pop {r4-r7}
pop {r1}
bx r1 

.ltorg
.align 4

@Returns the exact damage r0.
GetDamage:
  push {r4,lr}
  mov r4, r0 @BattleRound

  @if heal or nodamage
  mov   r0, #0x3
  ldsb  r0, [r4, r0]
  cmp   r0, #0x0
  ble   GetDamage_Exit

  @If you kill the enemy, we will get real damage.
  ldrb  r1, [r4,#0x2]
  mov   r2, #0x02 @maybe 0x020000 is defeat flag
  and   r1, r2    @check Defeat flag
  cmp   r1, #0x0
  beq   GetDamage_Exit

  mov r0, r4  @BattleRound
  bl  GetDisplayDamage

  ldrb r1, [r4, #3] @ BattleRound->Damage
  cmp r1 , r0
  blt GetDamage_Exit
  
  mov r0, r1
  @b   GetDamage_Exit

  GetDamage_Exit:
  pop {r4}
  pop {r1}
  bx r1

.ltorg
.align 4

@Get the value of DMG written in the center of the battle screen.
GetDisplayDamage:
  push {lr}
  mov r3, r0  @Current Round

  ldr r2, =0x203E1F0 @(gMapAnimStruct )	@{U}
  @ldr r2, =0x203E1EC @(gMapAnimStruct )	@{J}
  add r2, #0x58
  ldrb r2, [r2]  @gMapAnimStruct.IsDefender
  cmp  r2, #0x0
  beq  GetDamage_Attacker

    GetDamage_Defender:
    ldr r2, =0x0203E108	@gBattleActorTargetOrder	@{U}
    @ldr r2, =0x0203E104	@gBattleActorTargetOrder	@{J}
                        @(0: actor on the left, 1: actor on the right)
    ldrb r2, [r2]
    cmp  r2, #0x1
    beq  GetDamage_RightSide
    b    GetDamage_LeftSide
  
    GetDamage_Attacker:
    ldr r2, =0x0203E108	@gBattleActorTargetOrder	@{U}
    @ldr r2, =0x0203E104	@gBattleActorTargetOrder	@{J}
                        @(0: actor on the left, 1: actor on the right)
    ldrb r2, [r2]
    cmp  r2, #0x1
    bne  GetDamage_RightSide
    b    GetDamage_LeftSide

  GetDamage_RightSide:
  ldr r2, =0x0203E1BC     @ DisplayValue.Damage Left	@{U}
  @ldr r2, =0x0203E1B8     @ DisplayValue.Damage Left	@{J}
  b  GetDamage_LoadDamage
  
  GetDamage_LeftSide:
  ldr r2, =0x0203E1BC     @ DisplayValue.Damage Left	@{U}
  @ldr r2, =0x0203E1B8     @ DisplayValue.Damage Left	@{J}
  add r2, #0x2            @ Right

  GetDamage_LoadDamage:
  ldrh r0, [r2]    @BattleUnit->Damage

  cmp  r0, #0x7f
  bge  GetDisplayDamage_0


  ldrb r1, [r3,#0x0]  @BattleRound
  mov  r2, #0x1       @Is Critical?
  and  r1, r2
  cmp  r1, #0x0
  beq  GetDisplayDamage_CapCheck
     mov  r2, #0x3
     mul  r0, r2         @Critical Damage 3x

     GetDisplayDamage_CapCheck:
     cmp  r0, #0x7f
     blt  GetDisplayDamage_Exit
        mov  r0, #0x7f
        b    GetDisplayDamage_Exit

  GetDisplayDamage_0:
  mov r0, #0x0

  GetDisplayDamage_Exit:
  pop {r1}
  bx r1


.ltorg
.align 4

.equ SpriteData8x8,			0x08590F44	@{U}
@.equ SpriteData8x8,			0x085B8CDC	@{J}

Draw_NumberOAM:
@ referenced Zane's MMB function for this 
.type	Draw_NumberOAM, %function

@ Inputs:
@ r0: X coordinate
@ r1: Y coordinate
@ r2: Number

push	{lr}

@ We'll need all scratch registers,
@ so we set lr first

ldr		r3, =PushToSecondaryOAM 
mov		lr, r3

@ add number to base
@ 0-9 in r2 is the number
ldr		r3, =0x81C0 @ Number base tile
cmp r2, #5 
ble GotOffset
ldr		r3, =0x81E0 @ Number base tile
sub r2, #6  
GotOffset:
add		r3, r2, r3


mov r2, #27 @ palette # 26 - or 27 is the light rune palette i think 
lsl r2, #12 @ bits 12-15 
orr r3, r2 

@ palette | flips | tile 

ldr		r2, =SpriteData8x8 @ OAM data for a single 8x8 sprite

.short 0xf800 


pop		{r0}
bx		r0




.global Draw_Cleanup
.type Draw_Cleanup, %function 

Draw_Cleanup:
push {r4-r7, lr}
blh 0x8021668 @ Redraw map at end of event effect	@{U}
@blh 0x8021360 @ Redraw map at end of event effect	@{J}


@ palette must be updated 
ldr	r0,=0x300000E @ this is a byte (bool) that tells the game whether the palette RAM needs to be updated	@{U}
@ldr	r0,=0x300000D @ this is a byte (bool) that tells the game whether the palette RAM needs to be updated	@{J}
mov	r1,#0
strb r1,[r0]


mov r0, #0 
ldr r1, =VRAM_Address_Link
ldr r1, [r1] 
ldr r2, =4096 @ 64x64 image bytes size 
@=4096 @64x64 image bytes 
@Arguments: r0 = *word* to fill with, r1 = Destination pointer, r2 = size (bytes)
blh 0x08002054 @ RegisterFillTile	@{U}
@blh 0x08001FA4 @ RegisterFillTile	@{J}

blh 0x8001FE0 @| ClearTileRigistry	@{U}
@blh 0x8001F30 @| ClearTileRigistry	@{J}

ldr r3, =MemorySlot
mov r1, #0  
str r1, [r3, #8] @ Slot 2 to have the value of "0" 



pop {r4-r7}
pop {r1}
bx r1 


.align 4 
.ltorg 

.global Draw_RoundCleanup
.type Draw_RoundCleanup, %function 
Draw_RoundCleanup:
push {lr}
@Restore the destroyed palette to draw the numbers.
ldr r0, =0x0859EEC0	@Right Lune Palette	@{U}
@ldr r0, =0x085C73E0	@Right Lune Palette	@{J}
mov r1, #27 @ usual palette # 
lsl r1, #5 @ multiply by #0x20
mov	r2,#0x20
blh CopyToPaletteBuffer @Arguments: r0 = source pointer, r1 = destination offset, r2 = size (0x20 per full palette)

@Restore WeatherLava's firestorm palette
ldr r0, =0x0202BCF0	@gChapterData	{U}
@ldr r0, =0x0202BCEC	@gChapterData	{J}
ldrb r0, [r0, #0x15]
cmp r0, #0x5	@check Weather Lava(0x5)
bne SkipWeatherLavaPalette

ldr r0, =0x085A3AC0 @WeatherLava's firestorm palette @{U}
@ldr r0, =0x085CBFE4 @WeatherLava's firestorm palette @{J}
mov r1, #0xd0
lsl r1 ,r1 ,#0x2
mov r2, #0x20
blh CopyToPaletteBuffer @Arguments: r0 = source pointer, r1 = destination offset, r2 = size (0x20 per full palette)

SkipWeatherLavaPalette:

pop {r0}
bx r0

.align 4 
.ltorg 
