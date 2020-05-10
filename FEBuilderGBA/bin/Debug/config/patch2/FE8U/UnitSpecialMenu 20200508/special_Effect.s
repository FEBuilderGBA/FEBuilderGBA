.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

PUSH {r4,r5,lr}

@ldr r5,=0x03004DF0 @(操作キャラのワークメモリへのポインタ )	{J}
ldr r5,=0x3004E50	@(操作キャラのワークメモリへのポインタ )	{U}
ldr r5,[r5]

LDR r4, special_table
sub r4, #0xC
Loop:
add r4, #0xC

ldr r0, [r4]
cmp r0, #0x0
bne CheckUnit

ldr r0, [r4,#0x8]
cmp r0, #0x0
beq Break

CheckUnit:
ldrb r1, [r4,#0x0]
cmp r1,#0x0
beq CheckClass

ldr r0,[r5]          @ramunit->unit
ldrb r0,[r0,#0x4]    @ramunit->unit->ID
cmp r0,r1
bne Loop

CheckClass:
ldrb r1, [r4,#0x1]
cmp r1,#0x0
beq CheckFlags

ldr r0,[r5,#0x4]     @ramunit->class
ldrb r0,[r0,#0x4]    @ramunit->class->ID
cmp r0,r1
bne Loop

CheckFlags:
ldrh r0, [r4,#0x2]
cmp r0,#0x0
beq CheckItem

@blh	0x080860d0     @フラグ状態確認関数	{J}
blh	0x08083DA8     @フラグ状態確認関数	{U}
cmp r0,#0x01
bne Loop

CheckItem:
ldrb r1, [r4,#0x4]
cmp r1,#0x0
beq Run

mov  r2,#0x1e
ItemLoop:
cmp r2,#0x26
bgt Loop

ldrb r0,[r5,r2]     @ramunit->item1
cmp  r0,r1
beq  Run
add  r2,#0x2
b    ItemLoop

Run:
ldr r0, [r4,#0x8]
cmp r0,#0x1
ble Loop

MOV r1, #0x1
@blh 0x0800D340  @ イベント命令を動作させる関数	{J}
blh 0x0800D07C  @ イベント命令を動作させる関数	{U}

MOV r0, #0x17
b Exit

Break:
MOV r0, #0x8

Exit:
POP {r4,r5}
POP {r1}
bx r1

.ltorg
special_table:
