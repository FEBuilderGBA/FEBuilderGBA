@thumb

;@r0=char data
;@based on the heal staff fill-in-range function at 25E7C
push	{lr,r4,r5}
mov		r4,#0x10
ldsb	r4,[r0,r4]
mov		r5,#0x11
ldsb	r5,[r0,r5]
@align 4
ldr		r1,[Const1]
str		r0,[r1]
@align 4
ldr		r0,[Clear_Map_Func]
mov		r14,r0
@align 4
ldr		r0,[RangeMap]
ldr		r0,[r0]
mov		r1,#0x0
@dcw	0xF800
@align 4
ldr		r0,[Fill_One_Range_Map]		;@1-1 range stuff
mov		r14,r0
mov		r0,r4
mov		r1,r5
@align 4
ldr		r2,[Capture_Target_Check]
@dcw	0xF800
pop		{r4,r5}
pop		{r0}
bx		r0

@align 4
Const1:
@dcd	$02033f38	;0x02033F3C
Clear_Map_Func:
@dcd	$080194bc	;0x080197E4
RangeMap:
@dcd	$0202E4E0	;0x0202E4E4
Fill_One_Range_Map:
@dcd	$08024f20	; 0x08024F70
Capture_Target_Check:	;RegularAttackMapの途中
@dcd	$080251cc+1	;0x0802517C+1
