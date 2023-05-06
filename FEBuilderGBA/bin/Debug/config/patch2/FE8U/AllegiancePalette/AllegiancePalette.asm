.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.equ CheckEventId,0x8083da8
.equ BattleMapState, 0x202BCB0 
.equ CopyToPaletteBuffer, 0x8000DB8 
.equ gChapterData, 0x202BCF0 

push {r4-r5, lr} 

mov r2, #0x80 
blh CopyToPaletteBuffer 
ldr r4, =0xFFFFFFFF @ terminator 
ldr r5, AllegiancePaletteList 
sub r5, #8 
Loop: 
add r5, #8
ldr r0, [r5] 
cmp r0, r4 
beq Break  
ldrh r0, [r5] 
cmp r0, #0 
beq AnyFlag 
blh CheckEventId 
cmp r0, #1 
bne Loop 
AnyFlag: 
ldrb r0, [r5, #2] @ allegiance to effect 
cmp r0, #3 
bge Loop 
ldr r3, =gChapterData 
ldrb r1, [r3, #0xE] @ ch ID 
ldrb r0, [r5, #3] @ chapter ID 
cmp r0, #0xFF 
beq AnyChapter
cmp r0, r1 
bne Loop 
AnyChapter: 

ldr r0, [r5, #4] @ palette to use 
ldrb r1, [r5, #2] @ allegiance 
cmp r1, #0 
beq Skip
cmp r1, #1 
beq SetTo2 @ allegiance palettes are not in the same order as deployment byte 
mov r1, #1 
b Skip 
SetTo2: 
mov r1, #2 
Skip: 

lsl r1, #3 @ *8, as each palette is 16 colours, which each use a short, and it will be *4 shortly 
add r1, #0xE0 @ standing sprite palettes start at 0xE0<<2 
lsl r1, #2
mov r2, #0x20 
blh CopyToPaletteBuffer 
b Loop @ repeat for all valid cases until reaching the terminator 

Break: 

ldr r0, =BattleMapState
ldrb r1, [r0, #4] 
mov r0, #0x40 

pop {r4-r5} 
pop {r3} 
bx r3 
.ltorg 
AllegiancePaletteList: 
@ POIN AllegiancePaletteList 












