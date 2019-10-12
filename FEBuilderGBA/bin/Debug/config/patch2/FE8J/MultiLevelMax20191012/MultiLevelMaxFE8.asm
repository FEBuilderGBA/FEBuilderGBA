@Call 0802B9A8 (FE8J)
@Call 0802BA60 (FE8U)
@r0    flag temporary
@r1    temporary
@r2    level
@r3    keep
@r4    temporary
@r5    temporary
@r6    temporary
@r7    current unit struct [0x00: unit pointer][0x04: class pointer]

.thumb
.org 0

@minarai check
mov  r1, #0x80
lsl  r1  ,r1 ,#0xc
and  r0 ,r1
cmp  r0, #0x0
bne  exit_minarai

lsl  r6  ,r2 ,#0x18 @level cleanup
asr  r6  ,r6 ,#0x18 @

ldr  r0, [r7, #0x0] @get unit struct
ldrb r4, [r0, #0x4] @load unit id -> r4

ldr  r0, [r7, #0x4] @get class struct
ldrb r5, [r0, #0x4] @load class id -> r5

ldr  r1,DataList
data_find_loop:

ldr   r0,[r1, #0x0]
cmp   r0,#0x0
beq   exit_normal   @term data 0x00 0x00 0x00 0x00

ldrb  r0,[r1, #0x0]
cmp   r0,#0xFF       @if (unitid == 0xFF) { ANY }
beq   class_check

cmp   r0,r4
bne   next_data

class_check:
ldrb  r0,[r1, #0x1]

cmp   r0,#0xFF      @if (classid == 0xFF) { ANY }
beq   level_check

cmp   r0,r5
bne   next_data

level_check:

ldrb r0,[r1,#0x2]       @load max_level
cmp  r0,r6              @check level
ble  exit_level_max
b    exit_not_level_up

next_data:
add  r1,#0x4      @sizeof() == 4
b    data_find_loop

exit_level_max:
ldr	r0, LevelMaxBranch
mov	pc,r0

exit_not_level_up:
ldr	r0, NotLevelUpBranch
mov	pc,r0

exit_minarai:
ldr	r0, MinaraiBranch
mov	pc,r0

exit_normal:
ldr	r0, NotFoundBranch
mov	pc,r0


.align
LevelMaxBranch:
@.long 0x0802BA7C+1          @FE8U 
.long  0x0802B9C4+1          @FE8J 

NotLevelUpBranch:
@.long 0x0802BA8A+1          @FE8U 
.long  0x0802B9D2+1          @FE8J 

MinaraiBranch:
@.long 0x0802BA6A+1          @FE8U 
.long  0x0802B9B2+1          @FE8J 

NotFoundBranch:
@.long 0x0802BA74+1          @FE8U 
.long  0x0802B9BC+1          @FE8J 

.ltorg
DataList:
@list of the Data List sizeof 4bytes
@struct
@byte unit
@byte class
@byte max_level
