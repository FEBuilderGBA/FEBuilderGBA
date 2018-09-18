.thumb

.org 0x18D0C
	Graphx_1:

.org 0x295E0
	Get_Stat_Boost:

.org 0x29634
	Can_Level_Up:

.org 0x29660
Allocate:
push	{r4-r7,r14}
mov 	r5,r8
mov		r6,r9
push	{r5-r6}
add 	sp,#-0x1C		@7stats*4perword
mov 	r7,r0			@&Unit

@Should Level Up Check
bl  	Can_Level_Up	@I think? Should we level up? Hit max level?
lsl 	r0,r0,#0x18
cmp 	r0,#0x0			@No level up I assume
beq 	Deallocate		@End of the routine
ldrb	r0,[r7,#0x9]	@EXP
cmp 	r0,#0x63		@
ble 	Deallocate		@<100 EXP

@Update EXP
sub 	r0,#0x64		@Take away 100 EXP
strb	r0,[r7,#0x9]	@Write it back

@Update Level
ldrb	r0,[r7,#0x8]	@Increment level
add 	r0,#0x1			@
strb	r0,[r7,#0x8]	@Write it back

cmp 	r0,#0x14		@Level 20?
bne 	Afas_Drops_Check@

@Level 20 EXP Exception
mov 	r1,#0x6E		@EXP to gain
ldrb	r3,[r1, r7]		@
sub 	r0,r3,r2		@EXP until level up
strb	r0,[r1, r7]		@
mov 	r0,#0xFF		@
strb	r0,[r7,#0x9]	@EXP = 0xFF --> "--"

Afas_Drops_Check:
ldr 	r0,[r7,#0xC]	@Turn status/Hidden status
mov 	r1,#0x80		@
lsl 	r1,r1,#0x6		@Afa's drop bit
and 	r0,r1			@
mov 	r3,#0x0			@Default bonus
cmp 	r0,#0x0			@
beq 	No_Afas_Drops	@
mov 	r3,#0x5			@Give bonus
No_Afas_Drops:			@
mov 	r8,r3			@Store it in r8


@For each stat we need to
@> Load the character
@> Load the growth
@> Add afa's drops
@> Proc
@> Store pointer to battle data in stack
@> Store it into Battle Data
Mod_Check_End:
ldr 	r4,[r7]			@Do this once do I don't need to keep doing it.
add		r4,#0x1C		@Explicitly advance to growths -Venno
ldr		r1,[r7,#0x4]
add		r1,#0x1B
mov		r9,r1

mov 	r5, #0x0		@going to be my loop variable
mov 	r6, #0x0		@total stat boost

Stat_Up_Loop:
ldsb 	r0, [r4, r5]	@load the growth
add 	r0, r8			@Afa's drops bonus
mov		r1,r9
ldsb	r1,[r1,r5]
add		r0,r1
cmp		r0,#0x0
bge		Growth_Pos
mov		r0,#0x0
Growth_Pos:
bl  	Get_Stat_Boost	@r1,r2,r3 written to here
mov 	r2, r7			@r2 = &stat change
add 	r2, #0x73		@Start of stat changes
add 	r2, r5			@
lsl 	r3, r5, #0x2	@
mov		r1, sp
str 	r2, [r1, r3]	@where to store the pointer into the stack
strb	r0, [r2]		@store the boost
add 	r6, r0			@add on the stat boost

add 	r5, #0x1		@
cmp 	r5, #0x7		@7 stats to up: hp, str, skl, spd, def, res, luk
blt 	Stat_Up_Loop	@


cmp 	r6,#0x0			@If stats gained, go to end
bne 	end				@End

@Otherwise, start reprocing
Reprocs:
mov 	r5, #0x0		@going to be my loop variable

@For each stat to reproc, we need to:
@> Load Unit
@> Load Growth
@> Reproc
@> Load &Stat_Change (it was stack)
@> Store the new change there
@> See if the change was 0x0
@> If not, go to end
Reproc_Loop:
ldsb 	r0, [r4, r5]	@load the growth
mov		r1,r9
ldsb	r1,[r1,r5]
add		r0,r1
cmp		r0,#0x0
bge		Growth_Pos_2
mov		r0,#0x0
Growth_Pos_2:
bl  	Get_Stat_Boost	
lsl 	r3, r5, #0x2	@
mov		r2, sp
ldr 	r2, [r2, r3]	@where the stat change is to be stored.
strb	r0, [r2]		@store the revised boost

cmp 	r0, #0x0		@Did we get a stat?
bne 	end				@if so, then end immediately.

add 	r5, #0x1		@
cmp 	r5, #0x7		@7 stats to up: hp, str, skl, spd, def, res, luk
blt 	Reproc_Loop		@

add 	r6,#0x1			@Only reproc at most 2 times (3 procs total)
cmp 	r6,#0x1			@
ble 	Reprocs			@

end:
mov 	r0,#0xB			
ldsb	r0,[r7,r0]
bl  	Graphx_1	
mov 	r1,r7	
bl  	Graphx_2

Deallocate:
add 	sp,#0x1C		@Dealloc
pop 	{r5-r6}			@
mov 	r8,r5			@
mov		r9,r6
pop 	{r4-r7}			@

@Return
pop 	{r0}			@
bx  	r0				@

@Original routine ends at 08029818

.align 2
ItemTable:
.long 0x08BE222C

.org 0x29970
	Graphx_2:
