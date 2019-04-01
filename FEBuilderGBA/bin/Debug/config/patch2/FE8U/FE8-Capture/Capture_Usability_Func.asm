.thumb
.org 0x0

push	{r4-r6,r14}
ldr		r0,CurrentCharPtr
ldr		r0,[r0]
ldr		r1,[r0,#0xC]		@status word
mov		r2,#0x50			@is rescuing + has moved
tst		r1,r2
bne		RetFalse
mov		r2,#0x80
lsl		r2,#0x4				@is in ballista
tst		r1,r2
bne		RetFalse
mov		r5,r0
mov		r6,#0x0
ItemLoop:
lsl		r4,r6,#0x1
add		r4,#0x1E
add		r4,r5
ldrh	r4,[r4]
cmp		r4,#0x0
beq		RetFalse
mov		r0,#0xFF
and		r0,r1
mov		r1,#0x24
mul		r0,r1
ldr		r1,ItemTable
add		r0,r1
ldr		r1,[r0,#0x8]		@weapon abilities
mov		r2,#0x1
tst		r1,r2
beq		RetFalse
ldrb	r1,[r0,#0x19]		@weapon range
lsr		r1,#0x4				@min
cmp		r1,#0x1
bne		RetFalse			@can only capture at 1 range
ldr		r0,Weapon_Wield_Check
mov		r14,r0
mov		r0,r5
mov		r1,r4
.short	0xF800
cmp		r0,#0x0
beq		RetFalse
ldr		r0,ItemTable+4		@Fill_Capture_Range_Map
mov		r14,r0
mov		r0,r5
.short	0xF800
ldr		r0,TargetQueueCounter	@I think that's what this is
ldr		r0,[r0]
cmp		r0,#0x0
beq		NextWeapon
mov		r0,#0x1
b		GoBack
NextWeapon:
add		r6,#0x1
cmp		r6,#0x4
ble		ItemLoop
RetFalse:
mov		r0,#0x3
GoBack:
pop		{r4-r6}
pop		{r1}
bx		r1

.align
CurrentCharPtr:
.long 0x03004E50
Weapon_Wield_Check:
.long 0x08016750
TargetQueueCounter:
.long 0x0203E0EC
ItemTable:
@
