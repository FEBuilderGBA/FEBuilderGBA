
.thumb 
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ gChapterData, 0x202BCF0
.equ CheckEventId, 0x8083da8
.equ MemorySlot, 0x30004B8

push {r4-r6, lr}


@ in vanilla, r4 is unit struct 
@ r2 is a character's unit table 
@ r9 is unit ID to defend from chapter table + 0x8E 
ldrb r2, [r2, #4] @ unit ID 
cmp r9, r2 @ vanilla 
beq ReturnTrue @ maintain vanilla behaviour 

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
beq ReturnFalse @ finished looping when it finds WORD 0xFFFFFFFF 0xFFFFFFFF 
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

ReturnTrue: 

@ save unit struct to be used in IconDisplay 
mov r9, r4 

mov r1, #0x10 @ XX coord ? 

pop {r4-r6}
pop {r3} 
ldr r3, =0x80279B9 
bx r3 



ReturnFalse:
pop {r4-r6}
pop {r3} 
ldr r3, =0x80279FD 
bx r3 

.ltorg 

IconDisplayList: 

