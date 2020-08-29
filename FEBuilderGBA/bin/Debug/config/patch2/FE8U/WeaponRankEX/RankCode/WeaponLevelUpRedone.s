.thumb

.global WeaponLevelUpRedone
.type WeaponLevelUpRedone, %function

.macro SET_FUNC name, value
    .global \name
    .type   \name, %function
    .set    \name, \value
.endm

SET_FUNC CDClock, (0x0202BCF0)
SET_FUNC BMState, (0x0202BCB0)
SET_FUNC BattleStats, (0x0203A4D4)
SET_FUNC ItemData, (0x08809B10)

.macro blh to, reg
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

WeaponLevelUpRedone:
push {r4,r5,r6,r7,lr}   //GetBattleNewWExp
blh SetBases, r1
mov r7 ,r0
mov r0, #0xb
ldsb r0, [r7, r0]
mov r1, #0xc0
and r0 ,r1
cmp r0, #0x0
bne GetBattleNewWExpP1    

mov r0, #0x13
ldsb r0, [r7, r0]
cmp r0, #0x0
beq  GetBattleNewWExpP1    

ldr r0,=CDClock 
ldrb r1, [r0, #0x14] 
mov r0, #0x80
and r0 ,r1
cmp r0, #0x0
bne GetBattleNewWExpP1    

ldr r0,=BMState @[pc, #0x40] 
ldrb r1, [r0, #0x4] 
mov r0, #0x40
and r0 ,r1
cmp r0, #0x0
bne GetBattleNewWExpP1    

ldr r0,=BattleStats
ldrh r1, [r0, #0x0] 
mov r0, #0x20
and r0 ,r1
cmp r0, #0x0
bne GetBattleNewWExpP2 

mov r0 ,r7   
add r0, #0x52
ldrb r0, [r0, #0x0]
lsl r0 ,r0 ,#0x18
asr r0 ,r0 ,#0x18
cmp r0, #0x0
beq GetBattleNewWExpP1    

ldr r1, [r7, #0x4c]
mov r0, #0x5
and r0 ,r1
cmp r0, #0x0
beq GetBattleNewWExpP1     

mov r0, #0x88
lsl r0 ,r0 ,#0x3
and r1 ,r0
cmp r1, #0x0
beq GetBattleNewWExpP2     

GetBattleNewWExpP1:
mov r0, #0x1
neg r0 ,r0
b WXPReturn2 

GetBattleNewWExpP2:
mov r5 ,r7
add r5, #0x50
ldrb r0, [r5, #0x0]
mov r4 ,r7
add r4, #0x28
add r0 ,r4, r0
ldrb r6, [r0, #0x0]    
mov r0 ,r7
add r0, #0x48   //Weapon LV up when destroyed@SWITCH
ldrh r0, [r0, #0x0]
bl GetItemWExp //GetItemWExp
mov r1 ,r7  
add r1, #0x7b
ldrb r1, [r1, #0x0]
lsl r1 ,r1 ,#0x18
asr r1 ,r1 ,#0x18
mul r0 ,r1
bl NewWXPRoutine @This is where the new weapon xp is checked.
add r6 ,r6, r0
mov r1, #0x0
ldrb r3, [r5, #0x0]

GetBattleNewWEXP5:
ldr r2, [r7, #0x4]
cmp r1, r3
beq GetBattleNewWEXP3  //Modify S-Rank Limits@SWITCH (2bytes)

mov r0 ,r2
add r0, #0x2c
add r0 ,r0, r1
cmp r0, #0xfb
beq GetBattleNewWEXP3 

add r0 ,r4, r1
ldrb r0, [r0, #0x0]

cmp r0, #0xfa
bls GetBattleNewWEXP3 

cmp r6, #0xfa
ble GetBattleNewWEXP4  

mov r6, #0xfa
b GetBattleNewWEXP4  
    
GetBattleNewWEXP3:
add r1, #0x1
cmp r1, #0x7
ble GetBattleNewWEXP5 

GetBattleNewWEXP4:
ldr r0, [r7, #0x0]
ldr r4, [r0, #0x28]
ldr r0, [r2, #0x28]
orr r4 ,r0
mov r0, #0x80
lsl r0 ,r0 ,#0x1
and r0 ,r4
cmp r0, #0x0
beq GetBattleNewWExp6 

cmp r6, #0xfb
ble WXPReturn 

mov r6, #0xfb
b WXPReturn 

GetBattleNewWExp6:
mov r0, #0x80
lsl r0 ,r0 ,#0xc
and r4 ,r0
cmp r4, #0x0
beq GetBattleNewWEXP7 

cmp r6, #0x47
ble WXPReturn  

mov r6, #0x47
b WXPReturn  

@____________________________________________________________________________________________
GetBattleNewWEXP7:
ldr r0,=lMaxRank
add r0, #0x1
ldrb r0, [r0]
cmp r6, r0   //Max Weapon Level for Unpromoted Classes@ADDRESS
blt WXPReturn @#0x802c1a8
push {r0,r1,r2,r3}
b XPNull
XPNullR:
pop {r0,r1,r2,r3}

mov r6, r0   //Max Weapon Level for Unpromoted Classes@ADDRESS
@____________________________________________________________________________________________

WXPReturn:
mov r0 ,r6
WXPReturn2:
pop {r4,r5,r6,r7}
pop {r1}
bx r1


		@Code for getting held item's weapon XP
GetItemWExp: 
mov r1, #0xff   //GetItemWExp
and r0 ,r1
lsl r1 ,r0 ,#0x3
add r1 ,r1, r0
lsl r1 ,r1 ,#0x2
ldr r0,=ItemData 
add r1 ,r1, r0
add r1, #0x20
ldrb r0, [r1, #0x0]
bx lr


XPNull:
ldr r2, =lWEXPRFix
ldr r2, [r2, #0x0]
ldrb r2, [r2]
cmp r2, #0x1
	beq XPNullR
ldr r3,=lWEXPRFix
ldr r3, [r3]
add r3, #0x1
ldrb r3, [r3]
ldr r1, [r7, #0x0] @Loads that unit's location
ldrb r1, [r1, #0x4] @Loads that unit's character ID byte
lsl r1, r1, #0x4
ldr r2,=StartWEXP
ldr r2, [r2, #0x0]
add r1, r1, r2  @Loads the current units new weapon xp xword
add r1, r1, r3 @Loads the current units current weapon
mov r2, #0x0
strb r2, [r1]
b XPNullR

