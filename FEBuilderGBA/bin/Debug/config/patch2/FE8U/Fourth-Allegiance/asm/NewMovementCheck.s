.thumb
.org 0x0
@Hooked from 3003D28
@This function sets the Z flag if the moving unit can cross the other unit's tile, either because they're either both allied/npcs or enemies, or because the mover has Pass
push  {r0-r6,r14}   @actually necessary to push the scratch registers in this case
ldr r0, CheckAllegiance
mov r14, r0
ldrb  r0,[r3,#0xA]  @allegiance byte of current unit
mov r1, r7
.short 0xF800
cmp r0, #1
beq GoBackTrue

GoBackFalse:
pop   {r0-r6}
pop   {r4}
mov   r14,r4
ldr   r4,GoBackAddressFalse
bx    r4


GoBackTrue:
pop   {r0-r6}
pop   {r4}
mov   r14,r4
ldr   r4,GoBackAddressTrue
bx    r4

.align
GoBackAddressTrue:
.long 0x03003D38    @note that we need to switch back to arm
GoBackAddressFalse:
.long 0x03003D70

CheckAllegiance:
@POIN AreAllegiancesAllied
