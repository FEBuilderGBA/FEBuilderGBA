.thumb

.global GetWeaponRankLevelRedone
.type GetWeaponRankLevelRedone, %function

.macro blh to, reg
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

		@Code to check for what level of weapon rank the unit has.
GetWeaponRankLevelRedone:
push {r1,lr}   @//GetWeaponRankLevel
blh SetBases, r1
pop {r1}
cmp r0, #0x0
bgt Rank1 

mov r0, #0x0
b WLvReturn 

Rank1:
cmp r0, #0x1   @//Weapon Exp Required for Rank 1@ADDRESS
bgt Rank2 

mov r0, #0x1
b WLvReturn 

Rank2:
cmp r0, #0x2   @//Weapon Exp Required for Rank 2@ADDRESS
bgt Rank3 

mov r0, #0x2
b WLvReturn 

Rank3:
cmp r0, #0x3   @//Weapon Exp Required for Rank 3@ADDRESS
bgt Rank4 

mov r0, #0x3
b WLvReturn 

Rank4:
cmp r0, #0x4   @//Weapon Exp Required for Rank 4@ADDRESS
bgt Rank5 

mov r0, #0x4
b WLvReturn 


Rank5:
cmp r0, #0x5   @//Weapon Exp Required for Rank 5@ADDRESS
bgt Rank6 

mov r0, #0x5
b WLvReturn 


Rank6:
cmp r0, #0x6   @//Weapon Exp Required for Rank 6@ADDRESS
bgt Rank7 

mov r0, #0x6
b WLvReturn 

Rank7:
cmp r0, #0x7   @//Weapon Exp Required for Rank 7@ADDRESS
bgt Rank8 

mov r0, #0x7
b WLvReturn 

Rank8:
cmp r0, #0x8   @//Weapon Exp Required for Rank 8@ADDRESS
bgt Rank9 

mov r0, #0x8
b WLvReturn 

Rank9:
cmp r0, #0x9   @//Weapon Exp Required for Rank 9@ADDRESS
bgt RankA 

mov r0, #0x9
b WLvReturn 

RankA:
cmp r0, #0xA   @//Weapon Exp Required for Rank 10@ADDRESS
bgt RankB 

mov r0, #0xA
b WLvReturn 

RankB:
cmp r0, #0xB   @//Weapon Exp Required for Rank 11@ADDRESS
bgt RankC 

mov r0, #0xB
b WLvReturn 

RankC:
cmp r0, #0xC   @//Weapon Exp Required for Rank 12@ADDRESS
bgt RankE 

mov r0, #0xC
b WLvReturn 

RankE:
cmp r0, #0xE   @//Weapon Exp Required for Rank 14@ADDRESS
blt RankD 

mov r0, #0xE
b WLvReturn 

RankD:
mov r0, #0xD	@//Weapon Exp Required for Rank 13@ADDRESS
WLvReturn:
pop {r1}
bx r1