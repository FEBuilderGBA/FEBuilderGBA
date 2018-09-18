@Important note: there are three literals at the bottom of the code that are for reference,
@but not actually needed by the code

@csaProcessor:
.equ origin, 0x082DC078
.equ func_0804B6C4, . + 0x0804B6C4 - origin
.equ func_080474B0, . + 0x080474B0 - origin
.equ func_080474C8, . + 0x080474C8 - origin
.equ func_08003A04, . + 0x08003A04 - origin
.equ func_0804B868, . + 0x0804B868 - origin
.equ func_0804B6D8, . + 0x0804B6D8 - origin
.equ func_08047300, . + 0x08047300 - origin
.equ func_0804B80C, . + 0x0804B80C - origin
.equ func_080474BC, . + 0x080474BC - origin
.equ func_08047358, . + 0x08047358 - origin
.equ func_08003E60, . + 0x08003E60 - origin
.equ func_080477AC, . + 0x080477AC - origin
.equ func_08047600, . + 0x08047600 - origin
.equ func_0805C358, . + 0x0805C358 - origin
.equ func_0805C804, . + 0x0805C804 - origin
.equ func_08045C90, . + 0x08045C90 - origin
  .equ func_FLIP_TSA_TO_BUFFER, . + 0x804799c - origin
  .equ func_UNZIP_TO_BUFFER, . + 0x8047a88 - origin
  .equ func_UNZIP_BG_TO_BUFFER, . + 0x8047acc - origin
.thumb

@-------------------------------@
@	Callback code		@
@-------------------------------@

.long				0x00000019
.long				0x00000000
.long				0x00000003
.long				origin + Processor + 1
.long				0x00000000
.long				0x00000000

@-------------------------------@
@	Constructor code	@
@-------------------------------@

@-------------------------------@Constructor@ tested and working!
Dim_Constructor:
push	{r4-r6, lr}		@
mov	r5,	r0		@
mov	r0,	#0x1		@
push	{r0}			@
From_No_Dim_Constructor:
ldr	r0,	[r7,	#0x38]	@
bl				func_0804B6C4
@-------------------------------@r0 is now the boolean "isLeftToRight"
mov	r6,	r0		@now r6 is
mov	r0,	r5		@
bl				func_080474B0
bl				func_080474C8
ldr	r0,			ProcessStreamLoc
mov	r1,	#0x3		@
bl				func_08003A04
mov	r4,	r0		@
str	r5,	[r4,	#0x5C]	@
mov	r0,	#0x0		@
strh	r0,	[r4,	#0x2C]	@
mov	r0,	r5		@
bl				func_0804B868
lsl	r0,	r0,	#0x10	@
asr	r0,	r0,	#0x10	@
bl				func_0804B6D8
add	r4,	#0x29		@
strb	r0,	[r4]		@
eor	r0,	r6		@r0 is now the corrected boolean "isLeftToRight"
ldr	r1,			SPELL_ID_PTR
cmp	r0,	#0x0		@
beq				NO_SUB
sub	r1,	#0x2		@
NO_SUB:
ldrb	r1,	[r1]		@
lsl	r5,	r0,	#0x2	@r5 is now variable animation data offset
lsl	r1,	r1,	#0x2	@
lsl	r6,	r1,	#0x2	@
add	r1,	r1,	r6	@r1 now equals AnimationDataBase offset
ldr	r6,			AnimationDataBase
add	r6,	r1,	r6	@r6 now points to animation data struct
cmp	r0,	#0x0		@
beq				R_T_L
mov	r0,	#0x44		@
b				L_T_R
R_T_L:
mov	r0,	#0xAC		@
L_T_R:
ldr	r4,			LOW_DUMMY_FRAME_DATA
add	r4,	#0x2C		@
strb	r0,	[r4,	#0x02]	@Low dummy AIS's X offset is set
add	r4,	#0x48		@
strb	r0,	[r4,	#0x02]	@High dummy AIS's X offset is set
sub	r4,	#0x74		@
mov	r0,	#0x80		@
lsl	r0,	r0,	#0x18	@
str	r0,	[r4,	#0x0C]	@
str	r0,	[r4,	#0x1C]	@Both instances of dummy frame data are now terminated
ldr	r4,			FRAME_DATA_STREAM_PTR
ldr	r0,	[r6]		@
str	r0,	[r4]		@
add	r5,	#0x4		@
ldr	r0,	[r6,	r5]	@
sub	r4,	#0x54		@
str	r0,	[r4,	#0x30]	@High dummy AIS's base OAM pointer set
mov	r0,	#0x38		@
strb	r0,	[r4,	#0x0A]	@High dummy AIS's priority value set
add	r5,	#0x8		@
ldr	r0,	[r6,	r5]	@
sub	r4,	#0x48		@
str	r0,	[r4,	#0x30]	@Low dummy AIS's base OAM pointer set
mov	r0,	#0xFF		@
strb	r0,	[r4,	#0x0A]	@Low dummy AIS's priority value set

@@@@ commenting out this whole section, not needed

@ ldr	r4,			CON_MAP_1_TSA_HEAP_1
@ sub	r5,	#0xC		@
@ lsl	r5,	#0x8		@r5 = H flip bit for TSA@ r4 = MapBase
@ mov	r0,	#0x11		@
@ lsl	r0,	r0,	#0x8	@r0 is selecting the top left tile of the sheet
@ orr	r0,	r5		@r0 is correctly flipping the tiles
@ mov	r6,	#0x1		@
@ lsl	r6,	#0x9		@Loop size
@ add	r3,	r4,	r6	@r3 = End of TSA
@ cmp	r5,	#0x0		@
@ bne				L_T_R_TSA
@ @-------------------------------@TSA_LOOP_1
@ TSA_LOOP_1:
@ strh	r0,	[r4]		@
@ add	r0,	#0x1		@
@ add	r4,	#0x2		@
@ cmp	r4,	r3		@
@ bne				TSA_LOOP_1
@ @-------------------------------@TSA set
@ b				TSA_COMPLETE
@ L_T_R_TSA:
@ sub	r0,	#0x20		@
@ L_T_R_TSA_LOOP:
@ mov	r5,	#0x20		@
@ add	r0,	r0,	r5	@
@ @-------------------------------@TSA_LOOP_2
@ TSA_LOOP_2:
@ sub	r5,	#0x1		@
@ add	r2,	r0,	r5	@
@ sub	r2,	#0x2		@
@ strh	r2,	[r4]		@
@ add	r4,	#0x2		@
@ cmp	r5,	#0x0		@
@ bne				TSA_LOOP_2
@ cmp	r4,	r3		@
@ bne				L_T_R_TSA_LOOP
@ TSA_COMPLETE:
@ ldr	r4,			LCD_IO_SOURCE
@ mov	r0,	#0x18		@
@ strb	r0,	[r4,	#0x04]	@H-Blank set (part 1)
@ mov	r4,	#0x4		@
@ lsl	r4,	r4,	#0x10	@
@ add	r4,	#0x2		@
@ lsl	r4,	r4,	#0x8	@
@ ldrh	r0,	[r4]		@
@ mov	r1,	#0x2		@
@ orr	r0,	r1		@
@ strh	r0,	[r4]		@H-Blank set (part 2)
@ ldr	r0,			H_BLANK_BS
@ ldr	r1,			SCREEN_STRETCH_PC
@ str	r1,	[r0]		@Screen stretching IRQ set

pop	{r0}			@
cmp	r0,	#0x0		@
bne				super_with_dim
b				super_without_dim
No_Dim_Constructor:
push	{r4-r6, lr}		@
mov	r5,	r0		@
mov	r0,	#0x0		@
push	{r0}			@
b				From_No_Dim_Constructor
.align				2
LOW_DUMMY_FRAME_DATA:
.long				0x0203FF34
@-------------------------------@This constant is shared with a later routine
@FRAME_DATA_STREAM_PTR:
@.long				0x0203FFFC
ProcessStreamLoc:
.long				origin
SPELL_ID_PTR:
.long				0x0203CD0E

@@@@@@@@@@@@@@@@@@@@@@ MOVED TO THE BOTTOM

@AnimationDataBase:
@.long				0x08000000

@CON_MAP_1_TSA_HEAP_1:
@.long				0x0203FC00
@@@@@ Not needed tbh


@-------------------------------@This constant is shared with a later routine
@LCD_IO_SOURCE:
@.long				0x030026B0
@H_BLANK_BS:
@.long				0x03002724
@SCREEN_STRETCH_PC:
@.long				origin + H_BLANK_HANDLER + 1

super_with_dim:
bl				func_08047300
super_without_dim:
pop	{r4-r6, pc}		@
.align				2

@-------------------------------@
@	H Blank code		@UNCOMMENTED FOR COMMAND_53
@-------------------------------@

H_BLANK_HANDLER:
mov	r0,	#0x4		@
lsl	r0,	r0,	#0x18	@r0 = IO base address
ldrh	r1,	[r0,	#0x04]	@
mov	r2,	#0x1		@
and	r2,	r1		@
cmp	r2,	#0x0		@
bne				NO_UPDATE_SCANLINE
ldrh	r1,	[r0,	#0x06]	@
lsr	r2,	r1,	#0x1	@
neg	r2,	r2		@
strh	r2,	[r0,	#0x16]	@
NO_UPDATE_SCANLINE:
bx	lr

@-------------------------------@
@	Processor code		@
@-------------------------------@

Processor:

@-------------------------------@Processor@ tested and working!
@processAnimation:
.thumb
push	{r0-r7, lr}		@
mov	r4,	r0		@r4 points to "this spell"
ldr	r0,	[r4,	#0x5C]	@Loads pointer to AIS for unit casting this spell
mov	r6,	r0		@Attacker's AIS ref'd by r6
bl				func_0804B80C
add	r5,	r0,	#0x0	@Puts returned pointer to target unit's AIS into r5
ldr	r7,			FRAME_DATA_STREAM_PTR
@-------------------------------@Begin coding here

ldr	r3,	[r7]		@r3 points to frame data

NEXT_FRAME_LOOP_POINT: @for stretching commands

ldr	r0,	[r3]		@r0 is frame data
lsr	r1,	r0,	#0x18	@
cmp	r1,	#0x86		@
beq				FRAME_TYPE_86_ladder
cmp	r1,	#0x80		@
beq				TERMINATOR_PASS_ladder

	FRAME_TYPE_85:
	lsl	r1,	r0,	#0x18	@
	lsr	r1,	r1,	#0x18	@
	cmp	r1,	#0x1F		@
	beq				COMMAND_1F
	cmp	r1,	#0x29		@
	beq				COMMAND_29
	cmp	r1,	#0x2A		@
	beq				COMMAND_2A
	cmp	r1,	#0x40		@
	beq				COMMAND_40
	cmp	r1,	#0x48		@
	beq				COMMAND_48
	cmp	r1,	#0x1A		@
	beq				COMMAND_1A
	cmp r1, #0x53   @
	beq 			COMMAND_53 @@@@@@@@@@@@@@@Toggle Screen stretching
	cmp	r1,	#0x08		@
	bne				COMMAND_DEFAULT

		COMMAND_08:
		COMMAND_1A:
		mov	r2,	#0x9		@
		ldrh	r0,	[r5,	#0x10]	@Load target's status flags
		orr	r2,	r0		@Set "hit" flags that were immediately loaded
		strh	r2,	[r5,	#0x10]	@Update status flags
		mov	r3,	r1		@
		add	r4,	#0x29		@
		ldrb	r1,	[r4]		@
		mov	r0,	r5		@
		cmp	r3,	#0x1A		@
		bne				Steal_HP
		bl				func_08047600
		b				No_Steal_HP
		Steal_HP:
		bl				func_080477AC
		No_Steal_HP:
    @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    @here we need to check for a miss, and advance the FRAME_DATA_STREAM_PTR to the terminator if so
    @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    ldrb r1, [r4] @ Seems to be 0 for hit, 1 for miss?
    cmp r1, #1
    beq MISSED_ATTACK
    b       COMMAND_COMPLETE @continue as normal if not a miss

    MISSED_ATTACK: @skip ahead to terminator
    ldr r0, FRAME_DATA_STREAM_PTR
    ldr r1, [r0] @current frame
    ldr r3, TERMINATOR_CODE
    MISS_LOOP_START:
    ldr r2, [r1] @data at current frame
    add r1, #4
    cmp r2,r3
    beq SET_MISS_FRAME
    b MISS_LOOP_START

    SET_MISS_FRAME:
    str r1, [r0]
    b COMMAND_COMPLETE

		COMMAND_1F:
		add	r4,	#0x29		@
		ldrb	r1,	[r4]		@
		cmp	r1,	#0x0		@Only play hit sound if hitting
		bne				COMMAND_COMPLETE
		mov	r0,	r5		@
		bl				func_0805C358
		b				COMMAND_COMPLETE

		COMMAND_29:
		ldr	r6,			LCD_IO_SOURCE
		mov	r2,	#0x3F		@
		mov	r1,	#0x34		@
		strb	r2,	[r6,	r1]	@Enable special color effects
		mov	r1,	#0x3C		@
		lsl	r2,	r1,	#0x8	@
		add	r2,	#0x42		@
		strh	r2,	[r6,	r1]	@LOL optimization@ enable alpha blending
		add	r1,	#0x9		@
		lsr	r2,	r0,	#0x8	@
		strb	r2,	[r6,	r1]	@Set transparency
		sub	r1,	#0x1		@
		lsr	r2,	r2,	#0x8	@
		strb	r2,	[r6,	r1]	@Set brightness
		b				COMMAND_COMPLETE

		COMMAND_2A:
		ldr	r6,			LCD_IO_SOURCE
		lsl	r2,	r0,	#0x10	@
		lsr	r2,	r2,	#0x18	@
		mov	r1,	#0x1F		@Show map2/map3
		cmp	r2,	#0x0		@
		bne				DRAW_MAPS_2_AND_3	
		mov	r1,	#0x13		@Or you know, don't
		DRAW_MAPS_2_AND_3:
		strb	r1,	[r6,	#0x1]	@
		b				COMMAND_COMPLETE

		COMMAND_40:
		mov	r0,	r6		@
		mov	r1,	#0x1		@
		neg	r1,	r1		@
		bl				func_08045C90
		b				COMMAND_COMPLETE

		COMMAND_48:
		lsl	r0,	r0,	#0x8	@
		lsr	r0,	r0,	#0x10	@r0 = music ID to play
		mov	r1,	#0x80		@
		ldrh	r2,	[r5,	#0x2]	@
		mov	r3,	#0x2		@
		bl				func_0805C804
		b				COMMAND_COMPLETE


		FRAME_TYPE_86_ladder:
		b FRAME_TYPE_86

		TERMINATOR_PASS_ladder:
		b TERMINATOR_PASS

		COMMAND_53:
		lsl	r0,	r0,	#0x8	@
		lsr	r0,	r0,	#0x10	@ get the param hword
		cmp r0, #0
		bne STRETCHING_ON @C53 for off, C153 for on

		STRETCHING_OFF:
		ldr	r0,			LCD_IO_SOURCE
		mov	r1,	#0x8		@
		strb	r1,	[r0,	#0x4]	@Terminate H blank IRQ stuff (screen stretch)
		@let's advance to the next frame immediately
		ldr	r3,	[r7]		@
		add	r3,	#0x4		@
		str	r3,	[r7]		@
		b NEXT_FRAME_LOOP_POINT

		STRETCHING_ON:
		ldr	r4,			LCD_IO_SOURCE
		mov	r0,	#0x18		@
		strb	r0,	[r4,	#0x04]	@H-Blank set (part 1)
		mov	r4,	#0x4		@
		lsl	r4,	r4,	#0x10	@
		add	r4,	#0x2		@
		lsl	r4,	r4,	#0x8	@
		ldrh	r0,	[r4]		@
		mov	r1,	#0x2		@
		orr	r0,	r1		@
		strh	r0,	[r4]		@H-Blank set (part 2)
		ldr	r0,			H_BLANK_BS
		ldr	r1,			SCREEN_STRETCH_PC
		str	r1,	[r0]		@Screen stretching IRQ set
		@let's advance to the next frame immediately
		ldr	r3,	[r7]		@
		add	r3,	#0x4		@
		str	r3,	[r7]		@
		b NEXT_FRAME_LOOP_POINT
		.align
		@ LCD_IO_SOURCE:
		@ .long				0x030026B0
		H_BLANK_BS:
		.long				0x03002724
		SCREEN_STRETCH_PC:
		.long				origin + H_BLANK_HANDLER + 1

		COMMAND_DEFAULT:
		cmp	r1,	#0x13		@
		ble				COMMAND_COMPLETE
		strb	r1,	[r6,	#0x15]	@Send command value
		ldrh	r0,	[r6,	#0x06]	@
		add	r0,	#0x1		@
		strh	r0,	[r6,	#0x06]	@Increment delay countdown to ensure it is over 0
		mov	r1,	#0x1		@
		neg	r1,	r1		@
		lsr	r1,	r1,	#0x14	@r1 = 0x00000FFF
		ldrh	r0,	[r6,	#0x0C]	@
		and	r0,	r1		@Clear top bits of the +0x0C flags
		add	r1,	#0x1		@r1 = 0x00001000
		add	r0,	r0,	r1	@And set the bit for using commands
		strh	r0,	[r6,	#0x0C]	@Update +0x0C flags
		mov	r1,	#0x1		@
		strb	r1,	[r6,	#0x14]	@Fire command processing request

	COMMAND_COMPLETE:
	ldr	r3,	[r7]		@
	add	r3,	#0x4		@
	str	r3,	[r7]		@
	COMMAND_ESCAPE:
	b				Exit
	TERMINATOR_PASS:
	b				FRAME_TYPE_80

	FRAME_TYPE_86:
	ldrh	r2,	[r4,	#0x2C]	@FrameID--
	sub	r2,	#0x1		@FrameID--
	strh	r2,	[r4,	#0x2C]	@FrameID--
	mov	r3,	#0x0		@
	cmp	r2,	#0x0		@For checking if frame delay is == 0
	bgt				NO_ADVANCE
	beq				ADVANCE
	strh	r0,	[r4,	#0x2C]	@Update frame ID
	b				NO_ADVANCE
	ADVANCE:
	mov	r3,	#0x1		@
	NO_ADVANCE:
	push	{r3}			@r3 is boolean of whether to advance in frame data stream
	mov	r4,	r7		@
	sub	r4,	#0xC8		@r4 = LOW_DUMMY_FRAME_DATA
	str	r0,	[r4]		@
	str	r0,	[r4,	#0x10]	@
	
	@@ REMOVED
	@Load top TSA from heap
	@ldr	r0,			MAP_1_TSA_HEAP_1
	@ldr	r1,			MAP_1_TSA_BASE_1
	@mov	r2,	#0x80		@128 words in the TSA
	@lsl	r2,	r2,	#0x1	@256 halfwords, though@ writing to VRAM
	@swi	#0x0B			@CpuSet

	@@ REPLACEMENT
  ldr r3, [r7]    @ r3 is now the current frame data pointer
  ldr r2, [r3, #0x1C] @ this is the tsa pointer.
  @ hang on, we need to flip the tsa using 8055670 (r0 and r1 are separate tsas. just put the same in both)
  @ think they write to 2019790. TODO: find out if this is automatically updated.
  mov r1, r2
  mov r0, r6 @ oh god i sure hope this works

  bl     func_FLIP_TSA_TO_BUFFER @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@FIND THIS ROUTINE

	ldr	r3,	[r7]		@
	ldr	r0,	[r3,	#0x04]	@Get OAM sheet pointer
	@ldr	r1,			OAM_SHEET_PTR
	@str	r0,	[r4,	#0x04]	@
	@str	r0,	[r4,	#0x14]	@
	cmp	r0,	#0x0		@Check if it's null
	bne				VALID_OAM_SHEET
	strh	r0,	[r1]		@Set fill value
	mov	r0,	r1		@Set source address
	mov	r2,	#0x1		@Set fill bit
	lsl	r2,	r2,	#0x13	@
	add	r2,	#0x40		@Set word count
	lsl	r2,	r2,	#0x5	@Finish shifting
	swi	#0x0B			@CpuSet
	b				INVALID_OAM_SHEET
	VALID_OAM_SHEET:
  @r0 contains graphics data
  mov r1, #0x80
  lsl r1, r1, #5 @r1 = 0x00001000
  bl func_UNZIP_TO_BUFFER @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@FIND THIS ROUTINE TOO
  @swi #0x12     @LZ77UnCompVram
	INVALID_OAM_SHEET:
	@Fill dummy AIS structs and set parent AIS pointer
	@Also complete filling of dummy frame data
	ldr	r3,	[r7]		@
	ldr	r2,	[r3,	#0x08]	@r2 = OAM data offset
	ldr	r1,	[r3,	#0x0C]	@r1 = BG OAM data offset
	str	r1,	[r4,	#0x08]	@
	str	r2,	[r4,	#0x18]	@
	str	r4,	[r4,	#0x4C]	@Mind blowing optimization
	add	r4,	#0x10		@
	mov	r3,	r4		@
	add	r4,	#0x84		@
	str	r3,	[r4]		@
	mov	r4,	r3		@
	add	r4,	#0x10		@r4 = DEFAULT_OAM_PTR
	mov	r0,	#0x1		@
	strb	r0,	[r4]		@
	strb	r0,	[r4,	#0x0C]	@
	add	r4,	#0x54		@r4 = HI_DUMMY_AIS_PTR
	strb	r0,	[r4]		@+0x00 of dummy AISs set
	strb	r0,	[r4,	#0x06]	@
	sub	r4,	#0x48		@r4 = LOW_DUMMY_AIS_PTR
	strb	r0,	[r4,	#0x06]	@+0x06 of dummy AISs set
	add	r4,	#0x48		@r4 = HI_DUMMY_AIS_PTR
	ldrh	r0,	[r6,	#0x04]	@
	strh	r0,	[r4,	#0x04]	@
	sub	r4,	#0x48		@r4 = LOW_DUMMY_AIS_PTR
	strh	r0,	[r4,	#0x04]	@+0x04 of dummy AISs set
	mov	r0,	#0x28		@Sets palette/priority to use for sprites
	lsl	r0,	r0,	#0x8	@
	add	r0,	#0x40		@Sets tile base to use for sprites
	strh	r0,	[r4,	#0x08]	@
	add	r4,	#0x48		@r4 = HI_DUMMY_AIS_PTR
	mov	r0,	#0x24		@
	lsl	r0,	r0,	#0x8	@Higher priority because of nasty bugs involving
	@-------------------------------@increasing priority the "correct" way
	add	r0,	#0x40		@
	strh	r0,	[r4,	#0x08]	@+0x08 of dummy AISs set
	sub	r4,	#0x48		@r4 = LOW_DUMMY_AIS_PTR
	mov	r0,	#0x0		@
	str	r5,	[r4,	#0x34]	@+0x34 of low dummy AIS set
	ldr	r0,	[r5,	#0x38]	@
	cmp	r0,	r4		@
	beq				SKIP_CHAIN_1
	str	r0,	[r4,	#0x38]	@+0x38 of low dummy AIS set@ low AIS complete
	str	r4,	[r0,	#0x34]	@Low dummy AIS injected completely
	SKIP_CHAIN_1:
	str	r4,	[r5,	#0x38]	@Low dummy AIS is now child of defender
	add	r4,	#0x48		@r4 = HI_DUMMY_AIS_PTR
	ldr	r1,			PARENT_AIS_PTR
	ldr	r6,	[r1]		@
	ldr	r3,	[r6,	#0x38]	@
	str	r4,	[r6,	#0x38]	@Dummy is child of parent
	cmp	r3,	r4		@
	beq				SKIP_CHAIN_2
	str	r3,	[r4,	#0x38]	@Child is child of dummy
	SKIP_CHAIN_2:
	str	r6,	[r4,	#0x34]	@Parent is parent of dummy


@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@WRITE BG TO BUFFER
	ldr	r3,	[r7]		@
	ldr	r1,			BG_SHEET_PTR_1
	ldr	r0,	[r3,	#0x10]	@Get BG sheet pointer
	cmp	r0,	#0x0		@Check if it's null
	bne				VALID_BG_SHEET_1
	strh	r0,	[r1]		@Set fill value
	mov	r0,	r1		@Set source address
	mov	r2,	#0x1		@Set fill bit
	lsl	r2,	r2,	#0x13	@
	add	r2,	#0x80		@Set word count
	lsl	r2,	r2,	#0x5	@Finish shifting
	swi	#0x0B			@CpuSet
	b				INVALID_BG_SHEET_1
	VALID_BG_SHEET_1:
  @swi #0x12     @LZ77UnCompVram
  @r0 is bg sheet
  push {r3}
  mov r1, #0x20
  lsl r1, #8 @r1 = 0x2000
  bl func_UNZIP_BG_TO_BUFFER @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@FIND THIS ROUTINE
  pop {r3}

	INVALID_BG_SHEET_1:
	ldr	r3,	[r7]		@
	ldr	r0,	[r3,	#0x18]	@r0 = BG palette pointer
	cmp	r0,	#0x0		@Check if it's null
	beq				NO_BG_PALETTE
	ldr	r1,			BG_PALETTE_ADDR
	mov	r2,	#0x8		@8 words in a palette
	swi	#0x0C			@CpuFastSet
	ldr	r3,	[r7]		@
	NO_BG_PALETTE:
	ldr	r0,	[r3,	#0x14]	@r0 = OAM palette pointer
	cmp	r0,	#0x0		@Check if it's null
	beq				NO_OAM_PALETTE
	ldr	r1,			OAM_PALETTE_ADDR
	mov	r2,	#0x8		@8 words in a palette
	swi	#0x0C			@CpuFastSet
	NO_OAM_PALETTE:
	pop	{r0}			@
	cmp	r0,	#0x0		@r0 is boolean of whether to advance to next frame
	beq				Exit
	ldr	r3,	[r7]		@
  @ add r3, #0x1C   @r3 = pointer to next frame
  add r3, #0x20   @ 1c is now the TSA pointer. ain't i a cool one.
  str r3, [r7]    @Update frame data stream pointer
  b       Exit
		

	FRAME_TYPE_80:
	add	r3,	#0x4		@
	str	r3,	[r7]		@
	ldrh	r2,	[r4,	#0x2C]	@FrameID--
	sub	r2,	#0x1		@FrameID--
	bmi				NEGATIVE_DELAY
	strh	r2,	[r4,	#0x2C]	@FrameID--
	NEGATIVE_DELAY:
	cmp	r2,	#0x0		@For checking if frame delay is <= 0
	bgt				Exit
	lsl	r1,	r0,	#0x10	@
	lsr	r1,	r1,	#0x18	@
	cmp	r1,	#0x0		@
	beq				TERMINATE_ANIMATION
	add	r4,	#0x29		@
	ldrb	r1,	[r4]		@
	sub	r4,	#0x29		@
	cmp	r1,	#0x0		@
	beq				Exit
	TERMINATE_ANIMATION:
	mov	r2,	#0x0		@
	ldr	r1,			FRAME_DATA_STREAM_PTR
	sub	r1,	#0x9C		@
	strb	r2,	[r1]		@
	add	r1,	#0x48		@
	strb	r2,	[r1]		@Dummy AISs are disabled

  @clear both the tsa and the buffer. this is important for missed animations
  ldr r0,     MAP_1_TSA_HEAP_1
  mov r1, #0x0    @
  strh  r1, [r0]    @
	mov	r1,	r0		@
	mov	r2,	#0x1		@
	lsl	r2,	r2,	#0x15	@
	add	r2,	#0x52		@
	lsl	r2,	r2,	#0x3	@
  swi #0x0B     @CpuSet; clears TSA buffer

	ldr	r0,			MAP_1_TSA_BASE_1
	mov	r1,	#0x0		@
	strh	r1,	[r0]		@
	mov	r1,	r0		@
	mov	r2,	#0x1		@
	lsl	r2,	r2,	#0x17	@
	add	r2,	#0x80		@
	lsl	r2,	r2,	#0x1	@
	swi	#0x0B			@CpuSet@ clears TSA
	@ldr	r0,			LCD_IO_SOURCE
	@mov	r1,	#0x8		@
	@strb	r1,	[r0,	#0x4]	@Terminate H blank IRQ stuff (screen stretch)
	ldr	r0,			FRAME_DATA_STREAM_PTR
	sub	r0,	#0xA8		@r0 points to dummy OAM data
	mov	r1,	#0x1		@
	strb	r1,	[r0]		@
	str	r0,	[r0,	#0x48]	@OAM of low AIS dummy is terminated
	mov	r1,	#0x84		@
	str	r0,	[r0,	r1]	@OAM of high AIS dummy is terminated
	bl				func_080474BC
	bl				func_08047358
	add	r0,	r4,	#0x0	@
	bl				func_08003E60

Exit:
pop	{r0-r7, pc}		@Hurf

.align				2
FRAME_DATA_STREAM_PTR:
.long				0x0203FFFC
MAP_1_TSA_HEAP_1:
@.long				0x0203FC00
.long				0x2022308 @0x0201977C
MAP_1_TSA_BASE_1:
.long				0x06006800
LCD_IO_SOURCE:
.long				0x030026B0
PARENT_AIS_PTR:
.long				0x02028AE0
OAM_SHEET_PTR:
.long				0x06010800
BG_SHEET_PTR_1:
.long				0x06002000
BG_PALETTE_ADDR:
.long				0x02021728
OAM_PALETTE_ADDR:
.long				0x02021948
    TERMINATOR_CODE:
    .long 0x80000100

.long				origin + Dim_Constructor + 1
.long				origin + No_Dim_Constructor + 1
@.long				origin + AnimationDataBase
.ltorg @for safety
AnimationDataBase:
@add the offset after the #incbin
