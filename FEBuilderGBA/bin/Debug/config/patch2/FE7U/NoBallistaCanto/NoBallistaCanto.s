.thumb

@called from 1CBA0

@vanilla code
push    {r14}
ldr     r5,=0x3004690
ldr     r2, [r5] @Getting unit data
ldr     r0, [r2]
ldr     r3, [r2,#0x4]
ldr	    r0, [r0,#0x28]  @Unit abilities
ldr	    r1, [r3,#0x28]	@Class abilities
orr r0, r1
mov r1, #0x02
and r0, r1
cmp r0, #0x0
beq End @if unit does not have canto, end

@checking if unit is in ballista
ldrb    r3, [r2,#0xD] @unit status bitfield 2

mov r0, #0x08 @is riding ballista
tst r0,r3
beq End @if not set, branch to end, so unit will canto
mov r0, #0x0 @if set, make r0 zero, so unit won't canto 

End:
pop {r3}
bx  r3

.ltorg
.align
