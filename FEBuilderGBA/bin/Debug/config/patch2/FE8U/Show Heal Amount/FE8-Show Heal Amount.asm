.thumb
.org 0x0

@bl'd to at 24670
@since I cannot find a way to distinguish when a character is healing vs talk/support/dance/play, we're going to copy the entire function, essentially
@r0 = char data of target
bl		Copied_34FB0_Func		@most copied, at any rate
pop		{r4}
pop		{r1}
bx		r1

Copied_34FB0_Func:
push	{r4-r6,r14}
add		sp,#-0x8
mov		r6,r0
mov		r1,#0xA
ldr		r2,Func_349D4
mov		r14,r2
.short	0xF800
mov		r4,r0
mov		r0,#0xA
str		r0,[sp]
mov		r0,#0x1
str		r0,[sp,#0x4]
mov		r0,#0x0
mov		r1,r6
mov		r2,r4
ldr		r3,Func_3483C
mov		r14,r3
mov		r3,#0x0
.short	0xF800
mov		r5,r0
add		r5,#0x38
mov		r0,r5
mov		r1,r6
bl		New_HP_Box_Display
add		r4,#0x61
lsl		r4,#0x1
ldr		r0,Bg0Buffer
add		r1,r4,r0
mov		r0,r5
ldr		r2,Func_3E70
mov		r14,r2
.short	0xF800
add		sp,#0x8
pop		{r4-r6}
pop		{r0}
bx		r0

.align
Func_349D4:
.long 0x080349D4
Func_3483C:
.long 0x0803483C
Bg0Buffer:
.long 0x02022CA8
Func_3E70:
.long 0x08003E70

New_HP_Box_Display:
push	{r4-r7,r14}
mov		r4,r0
mov		r5,r1
ldr		r2,Func_3DC8
mov		r14,r2
.short	0xF800
ldr		r0,Process_Text_Func		@A240
mov		r14,r0
ldr		r0,HpTextID					@4E9
.short	0xF800
mov		r3,r0
mov		r0,r4
mov		r1,#0x0						@position shift (in pixels)
ldr		r2,Display_Text_Func		@4480
mov		r14,r2
mov		r2,#0x3						@palette index
.short	0xF800
ldr		r0,Process_Text_Func
mov		r14,r0
ldr		r0,ArrowTextID				@make a new text, don't use 53A
.short	0xF800
mov		r3,r0
mov		r0,r4
mov		r1,#0x24
ldr		r2,Display_Text_Func
mov		r14,r2
mov		r2,#0x3
.short	0xF800
ldr		r0,Current_Hp_Getter
mov		r14,r0
mov		r0,r5
.short	0xF800					@gets the target's current HP
mov		r7,r0
mov		r3,r7
ldr		r0,Display_Num_Func
mov		r14,r0
mov		r0,r4
mov		r1,#0x1C
mov		r2,#0x2
.short	0xF800
ldr		r0,Max_Hp_Getter
mov		r14,r0
mov		r0,r5
.short	0xF800
mov		r6,r0
ldr		r0,Get_Heal_Amount
mov		r14,r0
ldr		r0,CurrentCharPtr
ldr		r0,[r0]
ldrh	r1,[r0,#0x1E]			@assume the first item is the one being used; so far, this seems to be the case
.short	0xF800
add		r3,r0,r7				@current hp + healed amount
ldr		r0,Display_Num_Func
mov		r14,r0
mov		r0,r4
mov		r1,#0x38
mov		r2,#0x2
cmp		r3,r6
blt		NotMaxHP
mov		r2,#0x4
mov		r3,r6
NotMaxHP:
.short	0xF800
pop		{r4-r7}
pop		{r0}
bx		r0

.align
Func_3DC8:
.long 0x08003DC8
Process_Text_Func:
.long 0x0800A240
Display_Text_Func:
.long 0x08004480
Display_Num_Func:
.long 0x080044A4
Current_Hp_Getter:
.long 0x08019150
Max_Hp_Getter:
.long 0x08019190
Get_Heal_Amount:
.long 0x08016FB8
CurrentCharPtr:
.long 0x03004E50
HpTextID:
.long 0x000004E9
ArrowTextID:
@
