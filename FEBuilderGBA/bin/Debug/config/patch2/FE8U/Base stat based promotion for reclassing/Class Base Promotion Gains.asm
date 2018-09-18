.thumb
.org 0x19190
getMaxHP:

.org 0x19444
getClass:

@r0 = &RAM Data
@r1 = class to change to
.org 0x2BD50 
push    {r4-r7, lr}
mov     r4,r0           @r4 = &RAM Data
mov r0, r1
bl      getClass        @Get a pointer to class to change to.
mov r6, r0              @r6 = &New Class
ldr r5, [r4, #0x4]      @r5 = &Old Class

@Do max HP separately.
mov r3, #0xB            @Base HP
ldrb r0, [r5, r3]       @Old Class baseHP
ldrb r1, [r6, r3]       @New Class baseHP
sub r1, r0              @r1 = delta baseHP

mov r2, #0x12           @RAM Max HP
ldrb r3, [r4, r2]       @Current maxHP
add r3, r1              @new stat.

@Check if exceeds cap
mov r1, #0x13           @Max HP Cap
ldrb r1, [r6, r1]       @New class max HP
cmp r3, r1
ble belowMaxHP
mov r3, r1
belowMaxHP:
strb r3, [r4, r2]       

@Now loop through the other stats.
mov r7, #0x0            @Loop counter; start at 0.

statChangeLoop:
@First calc deltaStat
mov r3, #0xC
add r3, r7
ldrb r0, [r5, r3]       @Old Class base
ldrb r1, [r6, r3]       @New Class base
sub r1, r0              @r1 = delta base

@Now get the current stat from RAM.
mov r2, #0x14           @Stength offset
add r2, r7
ldsb r3, [r4, r2]       @Current stat

add r3, r1              @new stat.

@Now check it against the cap.
mov r1, #0x14           @Max Strength
add r1, r7
ldrb r1, [r6, r1]
cmp r3, r1
ble dontOverwrite
mov r3, r1
dontOverwrite:
strb r3, [r4, r2]

@loop
add r7, #0x1
cmp r7, #0x4            @&res = &str + 4
ble statChangeLoop


@Okay, now do weapon levels.
mov r7, #0x0
weaponLevelLoop:
mov r1, #0x28           @Sword level in ram
mov r2, #0x2C           @Sword level in class ROM
add r1, r7
add r2, r7
ldrb r3, [r4, r1]       @Current wlvl
ldrb r2, [r6, r2]       @New base
cmp r3, r2
bge continue
strb r2, [r4, r1]       @Base was higher, so store that.
continue:
add r7, #0x1            @Otherwise don't change it.
cmp r7, #0x7
ble weaponLevelLoop


@Now store the new class pointer to my unit.
str r6, [r4, #0x4]

@Last thing is to check if currentHP exceeds the effective max.
mov r0, r4
bl      getMaxHP
mov r1, #0x13           @current HP offset
ldsb r2, [r4, r1]
cmp r2, r0
ble validCurrentHP
strb r0, [r4, r1]
validCurrentHP:

@Now if it's an unpromoted class changing to a promoted class,
@Or if the old class has the lv10 cap skill,
@Reset level and EXP.
ldr r0, [r5, #0x28]     @Old class skills
mov r1, #0x08           @Max level 10
lsl r1, #0x10
and r1, r0
cmp r1, #0x0
bne resetLevel
mov r1, #0x01           @Promoted
lsl r1, #0x08
and r0, r1              @Promoted status of old class
ldr r2, [r6, #0x28]     @New class skills
and r1, r2              @Promoted status of new class
cmp r0, #0x0
bne depromoteCheck      @If old class was promoted...
cmp r1, #0x0
beq end                 @Going from unpromoted to unpromoted

@Unpromoted to promoted
    @If level < 20, then reset it.
    @Otherwise subtract 20.
    ldrb r0, [r4, #0x8]     @Level
    cmp r0, #0x14
    ble resetLevel
    sub r0, #0x14
    strb r0, [r4, #0x8]
    b end

@Lv 10 cap to anything
resetLevel:
    mov     r1,#0x0         @New EXP
    mov     r0,#0x1         @New Level
    strb    r0,[r4,#0x8]
    strb    r1,[r4,#0x9]
    b end


depromoteCheck:
    cmp r1, #0x0
    bne end                 @Going from promoted to promoted.
    ldrb r0, [r4, #0x8]     @Level
    add r0, #0x14
    strb r0, [r4, #0x8]
    
@return
end:
pop {r4-r7}
pop {r0}
bx r0

.org 0x02BEA0       @Make sur edon't exceed original's space.
