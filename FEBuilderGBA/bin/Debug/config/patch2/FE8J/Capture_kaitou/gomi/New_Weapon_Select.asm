@thumb

;@branched to from 22D08
;@fills in the range for 1-2+ as only 1-1 when capturing after selecting a weapon
;@r4=3004e50

push	{r14}
@align 4
ldr		r0, [Func_4E884]
mov		r14,r0
@dcw	0xF800			;@I think this clears backgrounds and stuff?
@align 4
ldr		r0,[Is_Capture_Set]
mov		r14,r0
ldr		r0,[r4]
@dcw	0xF800
cmp		r0,#0x0
beq		RegularRange
@align 4
ldr		r0,[Is_Capture_Set+4]		;@actually Fill_Capture_Range_Map
mov		r14,r0
ldr		r0,[r4]
@dcw	0xF800
b		GoBack

RegularRange:
@align 4
ldr		r0, [RegularAttackMap]
mov		r14,r0
ldr		r0,[r4]
ldrh	r1,[r0,#0x1E]
@dcw	0xF800

GoBack:
pop		{r0}
bx		r0

@ltorg

Func_4E884:
@dcd	$0804f610	;.long 0x0804E884
RegularAttackMap:
@dcd $08025164	;0x080251B4
Is_Capture_Set:
;Fill_Capture_Range_Map
