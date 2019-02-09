.thumb

@r0 = char data of unit being stolen from
@returns worth of most expensive item in r0 (0 if nothing) and slot id in r1 (-1 if nothing)

push	{r4-r7,r14}
add		sp,#-0x4
mov		r4,r0
mov		r6,#0		@slot id
mov		r7,#0		@value of most expensive item
mvn		r0,r7
str		r0,[sp]		@slot id of current most valuable item
InventoryLoop:
lsl		r5,r6,#1
add		r5,#0x1E
ldrh	r5,[r4,r5]
cmp		r5,#0
beq		GoBack
mov		r0,r4
mov		r1,r6
ldr		r3,=#0x8017054
mov		r14,r3
.short	0xF800		@is item stealable
cmp		r0,#0
beq		NextItem
mov		r0,r5
ldr		r3,=#0x801763C	@GetItemCost
mov		r14,r3
.short	0xF800
cmp		r0,r7
bls		NextItem
mov		r7,r0
str		r6,[sp]
NextItem:
add		r6,#1
cmp		r6,#4
ble		InventoryLoop
GoBack:
mov		r0,r7
ldr		r1,[sp]
add		sp,#0x4
pop		{r4-r7}
pop		{r2}
bx		r2

.ltorg
