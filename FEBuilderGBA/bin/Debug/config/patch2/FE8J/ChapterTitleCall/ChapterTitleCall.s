@Call 0801553C
@r0    ram pointer
@r4    parent procs

@r0 (map id)

.thumb
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.org 0x0
push {r5, r6}
mov r6, r0  @MapID

ldr r5, ChapterTitleCallTable
sub r5, #0x8

Loop:
add r5, #0x8
ldr r0, [r5,#0x00]
cmp r0,#0x00
bne CheckUnit

ldr r0, [r5,#0x04]
cmp r0,#0x00
beq NotMatch

CheckUnit:
ldrb r0, [r5,#0x00]
cmp  r0,#0xff
beq  CheckFlag

cmp  r0,r6
bne  Loop

CheckFlag:
ldrh r0, [r5,#0x02]
cmp  r0,#0x0
beq  Match

blh 0x080860d0  @CheckFlag
cmp r0, #0x0
beq Loop

Match:
ldr r0, [r5,#0x04]
cmp r0, #0x0
beq Exit
b   TitleCall

NotMatch:
ldr r0, =0x085C3630  @ (Procs ChapterIntroFx )
TitleCall:
mov r1 ,r4
blh 0x08002c30   @ NewBlocking6C

Exit:
pop {r5,r6}
ldr r3, =0x08015558
mov pc, r3

.ltorg
ChapterTitleCallTable:
