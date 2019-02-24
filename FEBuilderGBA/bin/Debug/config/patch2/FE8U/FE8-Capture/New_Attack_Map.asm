.thumb
.org 0x0

@modifies the check to figure out which weapons can be selected after pressing attack. Function checks if capture bit is set and then branches appropriately. Bxed to from 22CD0
@r0=current char data ptr, r5=current char pointer (3004e50), r4=item id/uses. Already checked if weapon can be wielded.
ldr		r1,Is_Capture_Set
mov		r14,r1
.short	0xF800
cmp		r0,#0x0
bne		CaptureWeapons
ldr		r0,Fill_Attack_Range_Map
mov		r14,r0
ldr		r0,[r5]
mov		r1,r4
.short	0xF800
b		CheckResults

CaptureWeapons:
mov		r0,#0xFF
and		r0,r4
mov		r1,#0x24
mul		r0,r1
ldr		r1,Is_Capture_Set+4		@ItemTable
add		r0,r1
ldrb	r0,[r0,#0x19]
lsr		r0,#0x4
mov		r1,#0x3
cmp		r0,#0x1
bne		GoBack
ldr		r0,Is_Capture_Set+8		@Fill_Capture_Range_Map
mov		r14,r0
ldr		r0,[r5]
.short	0xF800

CheckResults:
ldr		r0,Is_Target_Queue_Empty
mov		r14,r0
.short	0xF800
mov		r1,#0x3
cmp		r0,#0x0
beq		GoBack
mov		r1,#0x1
GoBack:
mov		r0,r1
pop		{r4-r5}
pop		{r1}
bx		r1

.align
Fill_Attack_Range_Map:
.long 0x080251B4
Is_Target_Queue_Empty:
.long 0x0804FD28
Is_Capture_Set:
@ItemTable
@Fill_Capture_Range_Map
