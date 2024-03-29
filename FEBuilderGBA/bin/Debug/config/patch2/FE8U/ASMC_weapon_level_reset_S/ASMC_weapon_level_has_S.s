.thumb 

.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ MemorySlot,0x30004B8	@{U}
.equ GetUnitByEventParameter, 0x800bc51	@{U}
.equ WeaponLevelS_EXP_POINTER, 0x8016d86	@{U}
.equ WeaponLevelA_EXP_POINTER, 0x8016d7e	@{U}

@.equ MemorySlot,0x30004B0	@{J}
@.equ GetUnitByEventParameter, 0x800bf3d	@{J}
@.equ WeaponLevelS_EXP_POINTER, 0x8016B2E	@{J}
@.equ WeaponLevelA_EXP_POINTER, 0x8016B26	@{J}

ASMC_weapon_level_has_S:
push {r4-r6, lr}
@ s1 = unit id
@ s3 = ignoreStaff
ldr r6, =MemorySlot 
ldr r0, [r6, #0x1 * 0x4] @ unit 
blh GetUnitByEventParameter
cmp r0, #0 
beq Exit
mov r4, r0 @ unit 

@武器レベルSがあるか?
ldr r3, =WeaponLevelS_EXP_POINTER @ レベルSになれるweapon exp
ldrb r5, [r3]

mov r1, #0x28		@sword
Loop1:
ldrb r0, [r4, r1]
cmp r0, r5
bgt TrueExit

add r1, #0x1
cmp r1, #0x2F	@Dark
ble Loop1


FlaseExit:
mov r0, #0x0
b   Exit

TrueExit:
mov r0, #0x1

Exit:
@r6 == MemorySlot
str r0, [r6, #0xC * 0x4] @ result

pop {r4-r6}
pop {r0}
bx r0
