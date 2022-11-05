.thumb 
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@.equ RefreshFogAndUnitMaps, 0x801A1F4 @{U}
.equ RefreshFogAndUnitMaps, 0x08019ECC @{J}
@.equ AddArrowTrapTargetsToTargetList, 0x802E710 @r0 = x, r1 = y, r2 = trap type (7 for arrow)  @{U}
.equ AddArrowTrapTargetsToTargetList, 0x0802E648 @r0 = x, r1 = y, r2 = trap type (7 for arrow)  @{J}
@.equ BWL_AddLossFromTrap, 0x802EA1C  @{U}
.equ BWL_AddLossFromTrap, 0x0802E954  @{J}
@.equ GenerateDisplayedTrapDamageTargets, 0x802E8A8  @{U}
.equ GenerateDisplayedTrapDamageTargets, 0x0802E7E0  @{J}
@.equ AddTarget, 0x804F8BC  @{U}
.equ AddTarget, 0x08050630  @{J}
@.equ InitTargets, 0x804F8A4  @{U}
.equ InitTargets, 0x08050618  @{J}
@.equ gProc_ShowArrowAnimation, 0x859E490  @{U}
.equ gProc_ShowArrowAnimation, 0x085C69B8  @{J}
@.equ RefreshEntityMaps, 0x801A1F4  @{U}
.equ RefreshEntityMaps, 0x08019ECC  @{J}
@.equ CheckForDeadUnitsAndGameOver, 0x802EA28  @{U}
.equ CheckForDeadUnitsAndGameOver, 0x0802E960  @{J}
@.equ MemorySlot, 0x30004B8  @{U}
.equ MemorySlot, 0x030004B0  @{J}
@.equ ProcStart, 0x8002C7C  @{U}
.equ ProcStart, 0x08002BCC  @{J}
@.equ ProcStartBlocking, 0x8002CE0  @{U}
.equ ProcStartBlocking, 0x08002C30  @{J}

push {r4-r7, lr} 
@mov r4, r0 @ Parent proc 

blh RefreshFogAndUnitMaps

mov r0, #0 
mov r1, #0 
blh InitTargets 

ldr r3, =MemorySlot 
ldrh r5, [r3, #4*1] @ damage 
add r3, #0x0B*4 

ldrh r6, [r3] @ XX 
ldrh r7, [r3, #2] @ YY
mov r0, r6 
mov r1, r7  
mov r2, r5 @ damage 
blh AddArrowTrapTargetsToTargetList 
blh BWL_AddLossFromTrap 
@blh GenerateDisplayedTrapDamageTargets 

mov r0, #0 
mov r1, #0 
blh InitTargets 

mov r0, r6 @ XX 
mov r1, r7 @ YY 
mov r2, #0 
mov r3, #7 @ arrow trap ID @ $36384 has a whole bunch of if statements related to target's r3 value 
blh AddTarget, r4  
mov r0, r6 @ XX 
mov r1, r7 @ YY 
mov r2, r5 @ damage 
blh AddArrowTrapTargetsToTargetList 

@ldr r0, =gProc_ShowArrowAnimation 
@mov r1, r4 @ parent 
@blh ProcStartBlocking 





pop {r4-r7} 
pop {r0} 
bx r0 
.ltorg 





