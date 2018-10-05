.thumb
@Additional Data Format:
@0-2: Debuffs, 4 bits each (str/skl/spd/def/res/luk)
@3: Rallys (bit 7 = rally move, bit 8 = rally spectrum)
@4: Str/Skl Silver Debuff (6 bits), bit 7 = half strength, HO bit = Hexing Rod
@5: Magic
@6: LO Nibble = Dual Guard Meter?

@Original at 191D0
push {r4-r6, lr}
mov r4, r0              @Unit
mov r5, #0x15
ldsb r5, [r4, r5]       @Base skill

ldr r6, AdditionalDataTable
ldrb r1, [r4 ,#0xB]     @Deployment number
lsl r1, r1, #0x3    @*8
add r6, r1          @r0 = *debuff data
@r6 = &Additional Data

@Now get the weapon bonus.
ldr r1, GetEquippedWeapon
bl gotoR1
ldr r1, GetWeaponSklBonus
bl gotoR1

@Add it to the accumulator.
add r5, r0

@Now get the debuff
@Skill debuff is LO nibble of 0th byte
ldr r0, [r6]
mov r1, #0xF0
and r0, r1 
lsr r0, r0, #0x4
@Subtract the debuff off.
sub r5, r0

@Silver debuff NOTE TO SELF: Recovers independantly of normal debuff
ldrb r0, [r6, #0x4]
mov r1, #0x1F           @Lower 6 bits
and r0, r1

@Subtract the debuff off
sub r5, r0

@Pair Up Bonus
@TODO: Read the bit that stores whether Rescue rules are in effect or Pair Up rules are in effect, and halve accordingly


@Rally Bonus
@2 bit of the 0x3 byte
ldrb r1, [r6, #0x3]
mov r0, #0x2
and r0, r1
cmp r0, #0x0
beq noSklRally
add r5, #0x4
noSklRally:
@Rally Spectrum
mov r0, #0x80
and r0, r1
cmp r0, #0x0
beq noRallySpectrum
add r5, #0x2
noRallySpectrum:

@Rescue - halve final stat
mov r0, r4
ldr r0, [r0, #0xc]
mov r1, #0x10 @rescuing
and r0, r1
cmp r0, #0
beq noRescue
  lsr r5, #1 @halve
noRescue:

@Return the acccumulator.
mov r0, r5

cmp r0, #0x0
bge end
mov r0, #0x0
end:
pop {r4-r6}
pop {r1}
gotoR1:
bx r1

.align
GetEquippedWeapon:
    .long 0x8016B29
GetWeaponSklBonus:
    .long 0x8016451
AdditionalDataTable:
    @Handled by installer.
    @.long 0x0203E894
