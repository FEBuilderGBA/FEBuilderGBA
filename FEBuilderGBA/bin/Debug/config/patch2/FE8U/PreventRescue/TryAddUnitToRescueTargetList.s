.equ gUnitSubject, 0x2033F3C
.equ AreUnitsAllied, 0x8024D8C
.equ CanUnitRescue, 0x801831C
.equ AddTarget, 0x804F8BC
.equ RescueeTable, RescuerTable + 4

.macro blh to, reg=r3
ldr \reg, =\to
mov lr, \reg
.short 0xF800
.endm

.thumb

// Input
// r0 = active unit

//push {r4,r5,lr}
mov r4, r0
ldr r5, =gUnitSubject

// r4 - active unit
// r5 - unit to be added to target list

// Check if units are same faction
ldr r0, [r5]
ldrb r0, [r0, #0xB]
lsl r0, #0x18
asr r0, #0x18
mov r1, #0xb
ldsb r1, [r4, r1]
blh AreUnitsAllied
lsl r0, #0x18
cmp r0, #0x0
beq DoNotAddToTargetList

// Check if Rescuer is allowed to Rescue
ldr r2, [r5]
ldr r0, [r2, #0x0]
ldrb r0, [r0, #0x4]
ldr r1, RescuerTable
ldr r1, [r1, r0]
cmp r1, #0x1
beq DoNotAddToTargetList

// Check if Rescuee is allowed to be rescued
ldr r0, [r4, #0x0]
ldrb r0, [r0, #0x4]
ldr r1, RescueeTable
ldr r1, [r1, r0]
cmp r1, #0x1
beq DoNotAddToTargetList

// Check for Berserk status
mov r0, r4
add r0, #0x30
ldrb r1, [r0]
mov r0, #0xF
and r0, r1
cmp r0, #0x4
beq DoNotAddToTargetList

// Check if unit is already rescuing or being rescued (0x30 = 0x10 Rescuing + 0x20 Being Rescued)
ldr r0, [r4, #0xC]
mov r1, #0x30
and r0, r1
cmp r0, #0x0
bne DoNotAddToTargetList

// Check rescue calculations (con vs aid)
mov r0, r2
mov r1, r4
blh CanUnitRescue
lsl r0, #0x18
cmp r0, #0x0
beq DoNotAddToTargetList

@ Add to Target List
mov r0, #0x10
ldsb r0, [r4, r0]
mov r1, #0x11
ldsb r1, [r4, r1]
mov r2, #0xB
ldsb r2, [r4, r2]
mov r3, #0x0
blh AddTarget, r4

DoNotAddToTargetList:
pop {r4,r5}
pop {r0}
bx r0

.ltorg
.align

RescuerTable:
@POIN RescuerTable
@POIN RescueeTable
