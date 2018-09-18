@r0 = Unit battle data
@r1 = IID/durability of item or staff being used
@Paste the resulting .dmp to $16FB8	FE8U
@Paste the resulting .dmp to $16D60	FE8J
@Makes staves heal (Magic/2)+Staff power
.thumb
push    {r4,r14}
mov     r3,r0
mov     r2,r1
mov     r4,#0x24
mov     r1,#0xFF
and     r1,r2
mul     r1,r4
mov     r4,#0x0
ldr     r0,ItemTable
add     r0,r0,r1
ldrb    r4,[r0,#0x15]	@Staff/Item 'Might'
ldr     r1,[r0,#0x8]	@Item Ability
mov     r2,#0x4			@Is item a staff? 
and     r2,r1
cmp     r2,#0x0
bne     Staff
b       HealingCap
Staff:
mov     r0,r3
add		r0,#0x2C		@Checking for S-rank staves
ldrb	r0,[r0]
cmp		r0,#0xFB
blo		ACheck
add		r4,#0x8
b		AddMagic
ACheck:
cmp		r0,#0xB5
blo		BCheck
add		r4,#0x4
b		AddMagic
BCheck:
cmp		r0,#0x79
blo		CCheck
add		r4,#0x2
b		AddMagic
CCheck:
cmp		r0,#0x47
blo		AddMagic
add		r4,#0x1
AddMagic:
mov		r0,r3
ldr		r1,GetPower		@Finds effective Magic
bl		Longcall
asr     r0,r0,#0x1
add     r4,r4,r0
HealingCap:
cmp     r4,#0x50
ble     End
mov     r4,#0x50
End:
mov     r0,r4
pop     {r4}
pop     {r1}
Longcall:				@This instruction doubles as our longcall and the end to the overall function. Handy!
bx      r1

.align 2
GetPower:
@ .long	0x080191B1	@FE8U
.long	0x08018EC5	@FE8J
ItemTable:
@ .long	0x08809B10	@FE8U
.long	0x0885E068	@FE8J
