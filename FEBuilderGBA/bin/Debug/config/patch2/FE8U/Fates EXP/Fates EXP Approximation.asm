@Routine to replace at 080x2C534

.thumb
push {r4-r7, r14}
mov r4, r0              @Attacker
mov r5, r1              @Defender
ldr r1, canGainEXP
bl gotoR1               @Can the unit gain EXP?
cmp r0, #0x0
beq retZero
mov r0, #0x13           @Current HP
ldsb r0, [r4, r0]
cmp r0, #0x0
beq retZero             @If dead, give no EXP.
ldr r0, [r5]
ldr r1, [r5, #0x4]
ldr r0, [r0, #0x28]
ldr r1, [r1, #0x28]
orr r0, r1
mov r1, #0x80
lsl r1, #0x11
and r0, r1
bne retZero             @If not no EXP, return 0.
@If no damage dealt, give no EXP. (different from GBAFE)
mov r0, #0x7C
ldrb r0, [r4, r0]
mov r1, #0x1            @Has done damage this turn (I'm making 0x2 into has hit this turn)
and r0, r1
cmp r0, #0x0
bne calcEXP
retZero:
mov r0, #0x0
b end

calcEXP:
@Either way we need effective levels. I'll store them in 
@r6 = Attacker's, r7 = Defender's
@First, get effective level of the attacker
ldrb r6, [r4, #0x8]     @Level
ldr r0, [r4]            @Character
ldr r0, [r0, #0x28]     @Char abilities
ldr r3, [r4, #0x4]      @Class
ldr r3, [r3, #0x28]     @Class abilities
mov r2, r3              @Make a copy for below.
orr r3, r0
mov r1, #0x08
lsl r1, #0x10           @Max lvl 10.
and r3, r1
cmp r3, #0x0
beq notTraineeAttacker
sub r6, #0xA
notTraineeAttacker:
mov r1, #0x1
lsl r1, #0x8            @Promoted
and r0, r1
cmp r0, #0x0
bne unpromotedAttacker
and r2, r1              @Promoted
cmp r2, #0x0
beq unpromotedAttacker
add r6, #0x14           @0x14 levels for promoted.
unpromotedAttacker:

@Now the defender
ldrb r7, [r5, #0x8]     @Level
ldr r0, [r5]
ldr r0, [r0, #0x28]
ldr r3, [r5, #0x4]      @Class
ldr r3, [r3, #0x28]     @Class abilities
mov r2, r3              @Make a copy for below.
orr r3, r0
mov r1, #0x08
lsl r1, #0x10           @Max lvl 10.
and r3, r1
cmp r3, #0x0
beq notTraineeDefender
sub r7, #0xA
notTraineeDefender:
and r0, r1
cmp r0, #0x0
bne unpromotedDefender
mov r1, #0x1
lsl r1, #0x8            @Promoted
and r2, r1
cmp r2, #0x0
beq unpromotedDefender
add r7, #0x14
unpromotedDefender:

@Now determine which formula to use.
@Get mode into r3; 0x0 = Easy, 0x1 = Normal, 0x2 = Hard
ldr r2, HardModeBit
ldrb r0, [r2]
mov r1, #0x40
and r0, r1
cmp r0, #0x0
bne isHardMode
ldr r2, EasyModeBit
ldrb r0, [r2]
mov r1, #0x20
and r0, r1
cmp r0, #0x0
beq isEasyMode
mov r3, #0x1
b deathDetermination
isEasyMode:
mov r3, #0x0
b deathDetermination
isHardMode:
mov r3, #0x2
deathDetermination:
mov r0, #0x13
ldsb r0, [r5, r0]       @Current HP of defender
cmp r0, #0x0
beq killingEXP          @If defender is dead, calc EXP for a kill


damagingEXP:            @Otherwise, only damage.
cmp r3, #0x2
bne notHardDamagingEXP


hardDamagingEXP:
    cmp r6, r7
    blt hardDamagingEXPLowerLevel
    mov r0, #0x6C           @108d
    mov r1, #0x12           @18d
    sub r2, r6, r7          @Player-Enemy level
    mul  r1, r2             @18*diff
    sub r0, r1
    @Now divide by 10.
    asr r0, #0x1
    ldr r1, DivideByFiveConstant
    mul r0, r1
    asr r0, #0x10
    b boundaryCheck
    hardDamagingEXPLowerLevel: @Player is of lower level
    mov r0, #0x0A
    sub r1, r7, r6
    lsr r1, #0x2
    add r0, r1
    b boundaryCheck


notHardDamagingEXP:
cmp r0, #0x1
beq normalDamagingEXP


easyDamagingEXP:
    cmp r6, r7
    blt normalDamagingEXPLowerLevel @Same formula as normal mode.
    mov r0, #0x0A
    sub r1, r6, r7
    sub r0, r1
    b boundaryCheck


normalDamagingEXP:
    cmp r6, r7
    blt normalDamagingEXPLowerLevel
    mov r0, #0x0A
    sub r1, r6, r7
    lsl r2, r1, #0x1
    add r1, r2                  @3*lvlDiff
    lsr r1, #0x1                @3/2*lvlDiff
    sub r0, r1
    b boundaryCheck
    normalDamagingEXPLowerLevel: @Player is of lower level
    mov r0, #0x0A
    sub r1, r7, r6
    lsr r1, #0x1
    add r0, r1
    b boundaryCheck


killingEXP:
    cmp r3, #0x2
    bne notHardKillingEXP

    
hardKillingEXP:
    cmp r6, r7
    blt hardKillingEXPLowerLevel
    @30-FLOOR(5.5*x)
    mov r0, #0x1E       @30d
    mov r1, #0xB
    sub r2, r6, r7
    mul r1, r2
    asr r1, #0x1
    sub r0, r1
    b killingBoundaryCheck
    hardKillingEXPLowerLevel:
    @30+2*x 
    mov r0, #0x1E
    sub r1, r7, r6
    lsl r1, #0x1
    add r0, r1
    b killingBoundaryCheck

notHardKillingEXP:
    cmp r0, #0x1
    beq normalKillingEXP
    
easyKillingEXP:
    cmp r6, r7
    blt easyKillingEXPLowerLevel
    @30-FLOOR(3.5*x)
    mov r0, #0x1E
    mov r1, #0x7
    sub r2, r6, r7
    mul r1, r2
    asr r1, #0x1
    sub r0, r1
    b killingBoundaryCheck
    
    easyKillingEXPLowerLevel:
    @30+FLOOR((20*x+3)/6) 
    mov r0, #0x1E
    mov r1, #0x14       @20
    sub r2, r7, r6
    mul r1, r2
    add r1, #0x3
    ldr r2, DivideByThreeConstant
    mul r1, r2
    asr r1, #0x11
    add r0, r1
    b killingBoundaryCheck


normalKillingEXP:
    cmp r6, r7
    blt normalKillingEXPLowerLevel
    @30-FLOOR(4.5*x)
    mov r0, #0x1E
    mov r1, #0x9
    sub r2, r6, r7
    mul r1, r2
    asr r1, #0x1
    sub r0, r1
    b killingBoundaryCheck
    
    normalKillingEXPLowerLevel:
    @30+FLOOR((16*x+3)/6) 
    mov r0, #0x1E
    mov r1, #0x10
    sub r2, r7, r6
    mul r1, r2
    add r1, #0x3
    ldr r2, DivideByThreeConstant
    mul r1, r2
    asr r1, #0x11
    add r0, r1
    b killingBoundaryCheck



killingBoundaryCheck:
    cmp r0, #0x1
    bge positive
    mov r0, #0x1
    b positive

boundaryCheck:
cmp r0, #0x0
bge positive
mov r0, #0x0
positive:
cmp r0, #0x64
ble end
mov r0, #0x64
end:
pop {r4-r7}
pop {r1}
gotoR1:
bx r1

.align
DivideByThreeConstant:
    .long 0x00005556
DivideByFiveConstant:
    .long 0x00003334
canGainEXP:
    .long 0x802B9F5
HardModeBit:
    .long 0x202BD04 @0x40 = hard mode
EasyModeBit:
    .long 0x202BD32 @0x20 = not easy mode
