.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
 .short 0xf800
.endm
.org 0x0

@08005514 GetPortraitStruct	{U}
@0800541C GetPortraitStruct	{J}
@r0 contains base character portrait ID
DynamicPortraitIDSetter:
push   {r4-r5,lr}

@each dynamic portrait table entry is set up by 8 bytes laid out like this:
@Halfword Halfword Halfworld Byte Byte
@////////////////////////////////////////////
@//2 bytes: Base portrait ID.
@//2 bytes: Portrait ID to replace the base mug with if conditions are met.
@//2 bytes: Event flag needed to activate change. 0 if no flag.
@//1 bytes: Character ID to check for class. 0 if no character
@//1 bytes: Class ID to activate change. 0 if no class.
@////////////////////////////////////////////

mov    r5, r0 @portrait id
ldr    r4, DynamicPortraitTable
sub    r4, #0x08

Loop:
add     r4,#0x08

ldrh    r1,[r4,#0x0] @ DynamicPortraitTable->BasePortraitID
cmp     r1,#0x00
beq     End  @if 0, we reached our terminator. table is over and no change is done.

cmp     r5, r1 @we check if this mug's entry has a change associated to it.
bne     Loop

@Entry match, now we check for flags
CheckFlag:
ldrh	r1,[r4,#0x04]	@ DynamicPortraitTable->Flag
cmp     r1,#0x00
beq     CheckUnitID

mov     r0,r1
@blh     0x08083DA8 @CheckFlag	@{U}
blh     0x080860d0 @CheckFlag	@{J}
cmp     r0,#0x00
beq     Loop  @Flag check fail, we move on

@if we made it here, flag check was successful
@Now we check for Character ID, and then Class ID
CheckUnitID:
ldrb    r0,[r4,#0x06]	@DynamicPortraitTable->UnitID
cmp     r0,#0x00
beq     Found @No character to check, success!

@blh     0x0801829C	@GetUnitByCharId	@{U}
blh     0x08017FB0	@GetUnitByCharId	@{J}
cmp     r0,#0x00
beq     Loop @If 0, character doesnt exist in RAM (hasn't joined maybe?)

ldrb    r2,[r0,#0x0C]             @I-I <-- Erase these lines if you want dead units to not undo mug changes
mov     r3,#0x04 @dead bit        @I-I
and     r2,r3                     @I-I
cmp     r2,r3                     @I-I
beq     Loop @Unit is dead        @I-I

#remove!
#ldrb    r2,[r0,#0x0B]
#mov     r1,#0xC0
#and     r2,r1
#cmp     r2,#0x00
#bne     Loop @unit isn't player unit.

CheckClass:
ldr     r0,[r0,#0x04] @RAMUnit->Class
ldrb    r0,[r0,#0x04] @RAMUnit->Class->ClassID

ldrb    r1,[r4,#0x07] @DynamicPortraitTable->ClassID
cmp     r1,#0x00
beq     Found @If 0, we only care if character exists, not about their class

cmp     r0, r1
bne     Loop

Found:  @ All checks successful, we change portrait
ldrh    r5, [r4,#0x02] @ DynamicPortraitTable->OveraidePortraitID

End:
mov    r1, r5

@ vanilla code ahead, mostly
lsl  r0 ,r1 ,#0x3
sub  r0 ,r0, r1
lsl  r0 ,r0 ,#0x2

@ldr  r1, =0x08005524	@PortraitPointer @{U}
ldr  r1, =0x0800542C	@PortraitPointer @{J}
ldr  r1, [r1]

add  r0 ,r0, r1

pop    {r4-r5}
pop    {r1}
bx     r1

.ltorg
DynamicPortraitTable:
