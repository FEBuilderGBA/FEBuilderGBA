
.thumb 
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ gChapterData, 0x202BCF0
.equ CheckEventId, 0x8083da8
.equ GetChapterDefinition, 0x8034618 

push {r4-r6, lr} 

ldr r1, =0x1FF @ vanilla 
and r0, r1 
ldr r3, =0x107 
add r1, r2, r3 
mov r2, #0xFF 
and r1, r2 @ end of vanilla 
push {r0-r1} @ save vanilla values 

mov r4, r9 @ unit pointer 
cmp r4, #0xFF 
ble DefaultBehaviour

ldr r5, IconDisplayList 
mov r6, #0 
sub r6, #1 @ 0xFFFFFFFF as terminator 
sub r5, #8 
Loop: 
add r5, #8 
ldr r0, [r5] 
cmp r0, r6 
bne SkipCheckTerminator
ldr r0, [r5, #4] @ table entry +4 
cmp r0, r6 
beq DefaultBehaviour @ finished looping when it finds WORD 0xFFFFFFFF 0xFFFFFFFF 
SkipCheckTerminator: 
ldrb r0, [r5, #0] @ unit ID 
cmp r0, #0 
beq SkipCheckUnitID
ldr r1, [r4] @ unit pointer 
ldrb r1, [r1, #4] @ unit ID 
cmp r0, r1 
bne Loop  
SkipCheckUnitID: 

ldrb r0, [r5, #1] @ class ID 
cmp r0, #0 
beq SkipCheckClassID
ldr r1, [r4, #4] @ class pointer 
ldrb r1, [r1, #4] @ class ID 
cmp r0, r1 
bne Loop  
SkipCheckClassID:

ldrh r0, [r5, #2] @ flag 
cmp r0, #0 
beq SkipCheckFlag 
blh CheckEventId 
cmp r0, #0 
beq Loop 
SkipCheckFlag: 

ldrb r0, [r5, #4] @ chapter ID 
cmp r0, #0xFF @ any 
beq SkipCheckChapter 
ldr r2, =gChapterData
ldrb r1, [r2, #0xE] @ chapter ID 
cmp r0, r1 
bne Loop 

SkipCheckChapter: 
mov r3, #8 @ default value  
ldrb r2, [r5, #5] @ tile ID 
cmp r2, #0xFF 
bne NotDefaultTile 
mov r2, #0x11 @ default vanilla value 

NotDefaultTile: 
lsl r3, #8 
orr r3, r2 
b Exit

DefaultBehaviour: 
ldr r3, =0x811 
Exit: 
push {r3} 
ldr r2, =gChapterData
ldrb r0, [r2, #0xE] @ chapter ID 
blh GetChapterDefinition
add r0, #0x8E @ unit ID to defend default 
ldrb r0, [r0] 
mov r9, r0 @ restore r9 to what it was 
pop {r3} 
pop {r0-r1} 
pop {r4-r6}
pop {r2} 
bx r2 
.ltorg 

IconDisplayList: 














