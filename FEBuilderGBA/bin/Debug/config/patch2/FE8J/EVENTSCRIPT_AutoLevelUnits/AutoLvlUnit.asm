.thumb 

.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@.equ MemorySlot,0x30004B8	@{U}
@.equ IncreaseUnitStatsByLevelCount, 0x8017FC5	@{U}
@.equ GetUnitByEventParameter, 0x800bc51	@{U}
@.equ EnsureNoUnitStatCapOverflow, 0x80181c9	@{U}

.equ MemorySlot,0x30004B0	@{J}
.equ IncreaseUnitStatsByLevelCount, 0x8017CE5	@{J}
.equ GetUnitByEventParameter, 0x800bf3d	@{J}
.equ EnsureNoUnitStatCapOverflow, 0x8017EDD	@{J}

@.global AutoLvlUnit_ASMC
@.type AutoLvlUnit_ASMC, %function 
@
@AutoLvlUnit_ASMC: 
push {r4-r7, lr}
@ s1 = unit id 
@ s3 = lvls 
@ s4 = visible if 1, invisible if 0

@ldr r3, =0x802bA78 @ Patch Name:Default Max Level  @FE8U
ldr r3, =0x802B9C0 @ Patch Name:Default Max Level  @FE8J

ldrb r7, [r3] @ Max Level 
ldr r4, =MemorySlot 
ldr r0, [r4, #4] @ unit 
blh GetUnitByEventParameter
cmp r0, #0 
beq Exit
mov r6, r0 @ unit 
ldr r5, [r4, #4*0x03] @ lvl gain 
cmp r5, #0xFF 
blt NoCapOnLevels 
mov r5, #0xFF @ So we don't try to gain too many levels 

NoCapOnLevels: 

ldr r0, [r4, #4*0x04] @ if not 0, raise level of unit instead of only stats 
cmp r0, #0 
beq IncreaseStatsNow @ Invisible levels 


ldr r1, [r6, #4]
mov r2,#0x2A
ldrb r1,[r1,r2] @loads Char Ability 3
mov r2,#0x8 @ trainee 
and r1,r2
cmp r1,r2 @checks if it has the byte 0x1
bne CheckCharForTraineeBitflag 
b TraineeCase

CheckCharForTraineeBitflag: 
ldr r1, [r6]
mov r2,#0x2A
ldrb r1,[r1,r2] @loads Char Ability 3
mov r2,#0x8 @ trainee 
and r1,r2
cmp r1,r2 @checks if it has the bit 0x1
bne EitherCase
b TraineeCase

TraineeCase: 
mov r7, #10
EitherCase: 
ldr r4, [r4, #4*0x03] @ lvls to gain 
ldrb r0, [r6, #8] 
add r0, r4 
cmp r0, r7 
blt StoreNewLvl 
sub r1, r0, r7 @ number of levels to ignore 
sub r5, r1 @ remove those levels 
mov r0, r7 @ Cap 
StoreNewLvl: 
strb r0, [r6, #8]

IncreaseStatsNow: 
mov r2, r5 @ Number of levels to increase by 
ldr r1, [r6, #4]
ldrb r1, [r1, #4] @ class id  
mov r0, r6 @ unit pointer 
blh IncreaseUnitStatsByLevelCount @ // str/mag split compatible
mov r0, r6
blh EnsureNoUnitStatCapOverflow
ldrb r0, [r6, #0x12] @ in case of overflow of current hp, do this 
strb r0, [r6, #0x13] @ Set to max hp 


Exit:

pop {r4-r7} 
pop {r0} 
bx r0 
