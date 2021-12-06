.align 4
.thumb

.include "Definitions.s"

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@ 0802a95c FillPreBattleStats ?	@{U}
@@ 0802A8C8 FillPreBattleStats ?	@{J}



@ given r0 = effect index
@ return damage (between the ranges)

.global AoE_FixedDamage
.type AoE_FixedDamage, %function
AoE_FixedDamage:
push {r4, lr} 
@r0 = table effect address 
mov r4, r0 
ldrb r0, [r4, #PowerLowerBoundByte] @ lower bound dmg 
ldrb r1, [r4, #PowerUpperBoundByte] @ upper bound dmg 
bl GetRandBetweenXAndY
@ returns the dmg dealt 

pop {r4} 
pop {r1} 
bx r1

.global GetRandBetweenXAndY
.type GetRandBetweenXAndY, %function
GetRandBetweenXAndY:

push {r4-r5, lr}

mov r4, r0 @ lower bound
mov r5, r1 @ upper bound
blh NextRN_100
@r0 as 0-100 i guess 

sub r3, r5, r4 @ difference 
mul r0, r3 @ 

add r0, #50 @ for rounding 


mov r1, #100 
swi 6 @ divide by 100 

add r0, r4 @ rand # between the diff + lower boundary 

pop {r4-r5} 
pop {r1} 
bx r1


@ given r0 = effect index
@ return damage (between the ranges)
.global AoE_RegularDamage
.type AoE_RegularDamage, %function
AoE_RegularDamage:
push {r4-r7, lr} 

mov r4, r0 
@r0 = table effect address 
@r1 = attacker / current unit ram 
@r2 = current target unit ram
mov r6, r1 
mov r7, r2 


ldrb r0, [r4, #PowerLowerBoundByte] @ lower bound mt 
ldrb r1, [r4, #PowerUpperBoundByte] @ upper bound mt
cmp r0, r1 
bgt FoundMt @ if lower bound is higher than upper bound because of user error, then we just use the lower bound 
bl GetRandBetweenXAndY
FoundMt:
mov r5, r0 @ mt 




@ terrain bonuses function ? 0802a6dc BattleSetupTerrainData

ldrb r2, [r4, #ConfigByte] 
mov r0, #MagBasedBool 
mov r1, #0x14 @ str/pow default 
tst r0, r2 
beq LoadAtt
mov r1, #0x3A @ mag byte. do not select "use mag" if no strmag split lol
LoadAtt:
ldrb r0, [r6, r1] @ str or mag 

add r5, r0 @ dmg to deal 
@ get unit def/res 
@ add to terrain bonus

ldrb r2, [r4, #ConfigByte] 
mov r3, #HitResBool 
mov r1, #0x17  @ Def as default 
tst r3, r2 
beq LoadDefOrRes
mov r1, #0x18 @ Res 
LoadDefOrRes: 
ldrb r1, [r7, r1] @ Def or Res 

mov r0, r5 @ dmg 
sub r0, r1 @ Dmg to deal 
cmp r0, #0 
bgt NoCap 
mov r0, #1 @ Always deal at least 1 damage 
NoCap:

pop {r4-r7} 
pop {r1} 
bx r1

