.equ MagConGetter, 0x8019284 @defined in the modularstatgetter
.equ MovGetter, 0x8019224 @defined in the modularstatgetter
.equ DebuffTable, 0x203f100
.equ BufferText, 0x800A240
.equ DrawText, 0x800443C
.equ DrawBar, 0x80870BC
.equ DrawStat, 0x8004B94
.equ DrawStatBonus, 0x08004BF0
.equ WritePalette, 0x08000DB8
.equ TileBufferBase, 0x2003c2c
.equ MagCheck, 0x8018A58
.equ AidCheck, 0x80189B8
.equ StrGetter, 0x80191b0
.equ SklGetter, 0x80191d0
.equ SpdGetter, 0x8019210
.equ LuckGetter, 0x8019298
.equ DefGetter, 0x8019250
.equ ResGetter, 0x8019270
.equ DrawIcon, 0x80036BC
.equ WriteTrvText, 0x80193E8
.equ WriteStatusText, 0x8019414
.equ AppendText, 0x8004480
.equ AffinityGetter, 0x80286BC
.equ EquippedWeaponGetter, 0x8016B28
.equ DrawBWLNumbers, 0x8086FAC
.equ DrawWeaponRank, 0x8087788
.equ DrawSupports, 0x8087698
.equ StatScreenStruct, 0x2003BFC
.equ BgBitfield, 0x300000D
.equ CopyToBG, 0x80D74B8
.equ Const_2003D2C, 0x2003D2C
.equ Const_2022D40, 0x2022D40
.equ Const_200472C, 0x200472C
.equ Const_2023D40, 0x2023D40
.equ tile_origin, 0x2003c94
.equ Green, 4
.equ Yellow, 3
.equ Blue, 2
.equ Grey, 1
.equ White, 0

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.macro blm to, from=origin
  .equ func_\to, . + \to - \from
  bl func_\to
.endm

.macro page_start
  push    {r4-r7,r14}       @08087184 B570     
  mov r7,r8
  push {r7}
  add     sp,#-0x50       @08087186 B094     
  ldr r7, =TileBufferBase  @r7 contains the latest buffer. starts at 2003c2c.
  ldr     r5,=StatScreenStruct
  ldr     r0,[r5,#0xC]
  mov r8, r0             @r8 contains the current unit's data
  clear_buffers
.endm

.macro page_end
  add     sp,#0x50   
  pop {r7}
  mov r8,r7
  pop {r4-r7}
  pop {r0}
  bx r0 
.endm

.macro draw_textID_at tile_x, tile_y, textID=0, width=3, colour=3, growth_func=-1		@growth func is # of growth getter in growth_getters_table; 0=hp, 1=str, 2=skl, etc
  mov r3, r7
  mov r1, #\width
  @r3 is current buffer location, r1 is width.
  ldrh r2,[r3] @current number
  add r2,r1 @for the next one.
  strb r1, [r3, #4] @store width
  strb r2, [r3, #8] @assign the next one.
  .if \textID
    ldr r0, =#\textID @otherwise assume it's in r0
  .endif
  blh BufferText
  mov r2, #0x0
  str r2, [sp]
  str r0, [sp, #4]
  mov r2, #\colour @colour
  .ifge \growth_func
  ldr r1,[sp,#0xC]			@growth getters table
  mov r0,#\growth_func-1
  lsl r0,#2
  ldr r1,[r1,r0]			@relevant growth getter function
  mov r0,r8
  mov r14,r1
  .short 0xF800				@returns growth
  mov r1,sp
  add r1,#0x18
  ldr r2,[sp,#0x14]			@growth options word and'd with 0x10, so non-zero if stat name color should reflect growth
  .set pal_index, (Get_Palette_Index - . - 6)
  ldr r3,=pal_index
  add r3,pc
  ldr r3,[r3]
  mov r14,r3
  .short 0xF800	@given growth, returns palette index, and does some shenanigans
  mov r2,r0
  .endif
  mov r0, r7
  ldr r1, =(tile_origin+(0x20*2*\tile_y)+(2*\tile_x))
  mov r3, #0
  blh DrawText, r4
  .ifge \growth_func
  ldr r1,[sp,#0x14]
  ldr r0,[sp,#0x18]
  bl  Restore_Palette		@see that func for an explanation (mss_page1_skills)
  .endif
  add r7, #8
.endm

.macro draw_textID_alt tile_x, tile_y, width, colour, textID=0, growth_func=-1		@growth func is # of growth getter in growth_getters_table; 0=hp, 1=str, 2=skl, etc
  mov r3, r7
  mov r1, #\width
  @r3 is current buffer location, r1 is width.
  ldrh r2,[r3] @current number
  add r2,r1 @for the next one.
  strb r1, [r3, #4] @store width
  strb r2, [r3, #8] @assign the next one.
  .if \textID
    ldr r0, =#\textID @otherwise assume it's in r0
  .endif
  blh BufferText
  mov r2, #0x0
  str r2, [sp]
  str r0, [sp, #4]
  mov r2, #\colour @colour
  .ifge \growth_func
  ldr r1,[sp,#0xC]			@growth getters table
  mov r0,#\growth_func-1
  lsl r0,#2
  ldr r1,[r1,r0]			@relevant growth getter function
  mov r0,r8
  mov r14,r1
  .short 0xF800				@returns growth
  mov r1,sp
  add r1,#0x18
  ldr r2,[sp,#0x14]			@growth options word and'd with 0x10, so non-zero if stat name color should reflect growth
  .set pal_index, (Get_Palette_Index - . - 6)
  ldr r3,=pal_index
  add r3,pc
  ldr r3,[r3]
  mov r14,r3
  .short 0xF800	@given growth, returns palette index, and does some shenanigans
  mov r2,r0
  .endif
  mov r0, r7
  ldr r1, =(tile_origin+(0x20*2*\tile_y)+(2*\tile_x))
  mov r3, #0
  blh DrawText, r4
  .ifge \growth_func
  ldr r1,[sp,#0x14]
  ldr r0,[sp,#0x18]
  bl  Restore_Palette		@see that func for an explanation (mss_page1_skills)
  .endif
  add r7, #8
.endm

.macro draw_HolyBlood_at tile_x, tile_y, GetterRoutineThing, width=8, colour=0, growth_func=-1		@growth func is # of growth getter in growth_getters_table; 0=hp, 1=str, 2=skl, etc
  mov r3, r7
  mov r1, #\width
  @r3 is current buffer location, r1 is width.
  ldrh r2,[r3] @current number
  add r2,r1 @for the next one.
  strb r1, [r3, #4] @store width
  strb r2, [r3, #8] @assign the next one.
  blh \GetterRoutineThing
  blh BufferText
  mov r2, #0x0
  str r2, [sp]
  str r0, [sp, #4]
  mov r2, #\colour @colour
  .ifge \growth_func
  ldr r1,[sp,#0xC]			@growth getters table
  mov r0,#\growth_func-1
  lsl r0,#2
  ldr r1,[r1,r0]			@relevant growth getter function
  mov r0,r8
  mov r14,r1
  .short 0xF800				@returns growth
  mov r1,sp
  add r1,#0x18
  ldr r2,[sp,#0x14]			@growth options word and'd with 0x10, so non-zero if stat name color should reflect growth
  .set pal_index, (Get_Palette_Index - . - 6)
  ldr r3,=pal_index
  add r3,pc
  ldr r3,[r3]
  mov r14,r3
  .short 0xF800	@given growth, returns palette index, and does some shenanigans
  mov r2,r0
  .endif
  mov r0, r7
  ldr r1, =(tile_origin+(0x20*2*\tile_y)+(2*\tile_x))
  mov r3, #0
  blh DrawText, r4
  .ifge \growth_func
  ldr r1,[sp,#0x14]
  ldr r0,[sp,#0x18]
  bl  Restore_Palette		@see that func for an explanation (mss_page1_skills)
  .endif
  add r7, #8
.endm

.macro draw_bar_at bar_x, bar_y, getter, offset, bar_id
  mov r0, r8
  blh      \getter
  mov r1, r8  
  mov     r3, #\offset
  ldsb    r3,[r1,r3]     
  str     r0,[sp]     
  ldr     r0,[r1,#0x4]  @class
  ldrb    r0,[r0,#\offset]  @stat cap
  lsl     r0,r0,#0x18    
  asr     r0,r0,#0x18    
  str     r0,[sp,#0x4]    
  mov     r0,#(\bar_id)     
  mov     r1,#(\bar_x-11)
  mov     r2,#(\bar_y-2)
  blh      DrawBar, r4
.endm

.macro draw_halved_bar_at bar_x, bar_y, getter, offset, bar_id
  mov r0, r8
  blh      \getter
  mov r1, r8  
  mov     r3, #\offset
  ldsb    r3,[r1,r3]   @base stat
  asr     r3,#1
  str     r0,[sp]     
  ldr     r0,[r1,#0x4]  @class
  ldrb    r0,[r0,#\offset]  @stat cap
  lsl     r0,r0,#0x18    
  asr     r0,r0,#0x19   @divided by 2    
  str     r0,[sp,#0x4]    
  mov     r0,#(\bar_id)     
  mov     r1,#(\bar_x-11)
  mov     r2,#(\bar_y-2)
  blh      DrawBar, r4
.endm

.macro draw_str_bar_at, bar_x, bar_y
  draw_bar_at \bar_x, \bar_y, StrGetter, 0x14, 0
.endm

.macro draw_skl_bar_at, bar_x, bar_y
  draw_bar_at \bar_x, \bar_y, SklGetter, 0x15, 1
.endm

.macro draw_skl_reduced_bar_at, bar_x, bar_y @for rescuing
  draw_halved_bar_at \bar_x, \bar_y, SklGetter, 0x15, 1
.endm

.macro draw_spd_bar_at, bar_x, bar_y
  draw_bar_at \bar_x, \bar_y, SpdGetter, 0x16, 2
.endm

.macro draw_spd_reduced_bar_at, bar_x, bar_y @for rescuing
  draw_halved_bar_at \bar_x, \bar_y, SpdGetter, 0x16, 2
.endm

.macro draw_luck_bar_at, bar_x, bar_y
  mov r0, r8
  blh      LuckGetter
  mov r1, r8  
  mov     r3, #0x19
  ldsb    r3,[r1,r3]     
  str     r0,[sp]     
  mov r0, #0x1e  @cap is always 30
  str     r0,[sp,#0x4]    
  mov     r0,#0x5     
  mov     r1,#(\bar_x-11)
  mov     r2,#(\bar_y-2)
  blh      DrawBar, r4
.endm

.macro draw_def_bar_at, bar_x, bar_y
  draw_bar_at \bar_x, \bar_y, DefGetter, 0x17, 3
.endm

.macro draw_res_bar_at, bar_x, bar_y
  draw_bar_at \bar_x, \bar_y, ResGetter, 0x18, 4
.endm

.macro draw_growth_at, bar_x, bar_y
  mov		r14,r0		@r0 = growth getter to bl to
  mov		r0,r8
  .short	0xF800		@returns total growth in r0, base growth in r1
  sub		r0,r0,r1	@difference between total and base
  str		r0,[sp,#0x10]
  mov		r2,r1		@base in r2
  mov		r1,#0x2		@palette index
  ldr		r0,=(tile_origin+(0x20*2*\bar_y)+(2*\bar_x))
  blh		DrawStat
  ldr		r0,[sp,#0x10]	@difference from earlier
  ldr		r1,=(tile_origin+(0x20*2*\bar_y)+(2*(\bar_x+1)))
  blh		DrawStatBonus
.endm

.macro draw_move_bar_at, bar_x, bar_y
  mov r1, r8
  @check AI
  mov r3, #0x41
  ldrb r3, [r1,r3] @AI byte 4
  cmp r3, #0x20
  beq NoMove
  mov r3, #0x30
  ldrb r3, [r1,r3] @status
  mov r0, #0xF
  and r3, r0
  cmp r3, #0x9 @freeze status
  beq NoMove
  ldr     r0,[r1,#0x4] @class
  mov     r3,#0x12     @move
  ldsb    r3,[r0,r3]  
  mov     r0,#0x1D     @bonus
  ldsb    r0,[r1,r0]   
  b NormalMove
  NoMove:
  mov r0, #0
  mov r3, #1
  neg r3, r3
  b DrawMove
  NormalMove:
  DrawMove:
  add     r0,r0,r3     
  str     r0,[sp]     @r0 is total, r3 is base
  mov     r6,#0xF     
  str     r6,[sp,#0x4]
  mov     r0,#0x6      @why 6?
  mov     r1,#(\bar_x-11)
  mov     r2,#(\bar_y-2)   
  blh DrawBar, r4
.endm

.macro draw_move_bar_with_getter_at, bar_x, bar_y
@base in r3, final in sp, cap in sp+4, call getter
  mov r1, r8
  ldr     r0,[r1,#0x4] @class
  mov     r3,#0x12     @move
  ldsb    r3,[r0,r3]  
  @ mov     r0,#0x1D     @bonus
  @ ldsb    r0,[r1,r0]   
  @ add     r0,r0,r3    

  push {r1-r3}
  mov r0, r8
  blh MovGetter
  pop {r1-r3}
  cmp r0, #0
  bne MoveNotNegated
    mvn r0, r0
    mov r3, r0
  MoveNotNegated:
  str r0, [sp] @final
  mov r6, #0xF
  str r6, [sp, #4]
  mov     r0,#0x6      
  mov     r1,#(\bar_x-11)
  mov     r2,#(\bar_y-2)      
  blh DrawBar, r4
.endm

.macro draw_move_number_at, tile_x, tile_y
  mov r1, r8
  ldr     r0,[r1,#0x4] @class
  mov     r3,#0x12     @move
  ldsb    r3,[r0,r3]  
  mov     r0,#0x1D     @bonus
  ldsb    r0,[r1,r0]   
  cmp r0, #0
  beq MoveNotBoosted
  mov r1, #Green
  b FromMoveBoosted
  MoveNotBoosted:
  mov r1, #Blue
  FromMoveBoosted:
  add     r0,r0,r3
  draw_number_at \tile_x, \tile_y
.endm

.macro draw_con_bar_at, bar_x, bar_y
  mov r1, r8
  ldr     r0,[r1,#0x4] @class
  mov     r3,#0x11     @con
  ldsb    r3,[r0,r3]   
  ldr     r0,[r1]      
  ldrb    r0,[r0,#0x13]@bonus
  lsl     r0,r0,#0x18  
  asr     r0,r0,#0x18  
  add     r3,r3,r0     
  mov     r0,#0x1A     
  ldsb    r0,[r1,r0]   
  add     r0,r3,r0     
  str     r0,[sp]      
  ldr     r0,[r1,#0x4] 
  ldrb    r0,[r0,#0x19]
  lsl     r0,r0,#0x18  
  asr     r0,r0,#0x18  
  str     r0,[sp,#0x4] 
  mov     r0,#0x7      
  mov     r1,#(\bar_x-11)
  mov     r2,#(\bar_y-2)      
  blh DrawBar, r4
.endm

.macro draw_con_number_at, tile_x, tile_y
  mov r1, r8
  ldr     r0,[r1,#0x4] @class
  mov     r3,#0x11     @con
  ldsb    r3,[r0,r3]   
  ldr     r0,[r1]      
  ldrb    r0,[r0,#0x13]@bonus
  lsl     r0,r0,#0x18  
  asr     r0,r0,#0x18  
  add     r3,r3,r0     
  mov     r0,#0x1A     
  ldsb    r0,[r1,r0]   
  cmp r0, #0
  beq ConNotBoosted
  mov r1, #Green
  b FromConBoosted
  ConNotBoosted:
  mov r1, #Blue
  FromConBoosted:
  add     r0,r0,r3
  draw_number_at \tile_x, \tile_y
.endm

.macro draw_con_bar_with_getter_at, bar_x, bar_y
@base in r3, final in sp, cap in sp+4, call getter
  mov r1, r8
  ldr     r0,[r1,#0x4] @class
  mov     r3,#0x11     @con
  ldsb    r3,[r0,r3]   
  ldr     r0,[r1]      
  ldrb    r0,[r0,#0x13]@bonus
  lsl     r0,r0,#0x18  
  asr     r0,r0,#0x18  
  add     r3,r3,r0     

  push {r1-r3}
  mov r0, r8
  blh MagConGetter
  pop {r1-r3}
  str r0, [sp] @final
  ldr     r0,[r1,#0x4] 
  ldrb    r0,[r0,#0x19]
  lsl     r0,r0,#0x18  
  asr     r0,r0,#0x18  
  str     r0,[sp,#0x4]  @store cap
  mov     r0,#0x7      
  mov     r1,#(\bar_x-11)
  mov     r2,#(\bar_y-2)      
  blh DrawBar, r4
.endm

.macro draw_number_at, num_x, num_y, routine=0, colour=2 @r0 is number and r1 is colour
  .if \routine
  mov r0, r8
  blh \routine
  .endif
  mov r1, #\colour @defaults to blue
  mov r2, r0
  ldr r0, =(tile_origin+(0x20*2*\num_y)+(2*\num_x))
  blh 0x8004b94
.endm

.macro draw_charge_at, num_x, num_y, colour=2 @r0 is number and r1 is colour
  mov r0, r8
  mov 	r1,#0x47
  ldrb	r0,[r0,r1]
  sub r0,#0x10
  cmp r0,#0x0
  beq Greeny
  mov r1, #0x2
  b Naxty
  Greeny:
  mov r1, #0x4
  Naxty:
  mov r2, r0
  ldr r0, =(tile_origin+(0x20*2*\num_y)+(2*\num_x))
  blh 0x8004b94
.endm

.macro draw_aid_icon_at tile_x, tile_y
  mov r0, r8
  ldr r1, [r0]
  ldr r2, [r0,#4]
  ldr r0, [r1,#0x28]
  ldr r1, [r2,#0x28]
  orr r0, r1
  blh 0x8018AF0 @takes ability bits and returns icon or -1
  mov r1, r0
  mov r2, #0xA0
  lsl r2, #7
  ldr r0, =(tile_origin+(0x20*2*\tile_y)+(2*\tile_x))
  blh DrawIcon
.endm

.macro draw_trv_text_at, tile_x, tile_y, colour=Blue
  draw_textID_at \tile_x, \tile_y, 0x4f9, width=9 @trv
  mov r4, r7
  sub r4, #8 @un-advance the buffer
  mov r0, r8
  blh WriteTrvText
  mov r3, r0
  mov r0, r4
  mov r1, #0x18 @what is this?
  mov r2, #\colour
  blh AppendText, r4
.endm

.macro draw_talk_text_at, tile_x, tile_y, colour=Blue
  draw_textID_at \tile_x, \tile_y, width=9 @ideally you want a diff id.
  mov r4, r7
  sub r4, #8
  mov r0, r8
  ldr   r0,[r0]
  ldrb  r0,[r0,#0x4]    @char byte
  bl GetTalkee
  cmp   r0,#0x0
  bne   FoundAPerson
  ldr   r1,=0x7f7f7f @---[X]
  ldr   r0,=0x202a6ac @text buffer
  str   r1,[r0]
  b   TextBuffered
  FoundAPerson:
  mov   r1,#0x34
  mul   r0,r1
  ldr   r1,=0x8017d64 @pointer to character table (in case repointed)
  ldr r1, [r1]  @actual character table
  add   r0,r1
  ldrh  r0,[r0]
  ldr   r1,=BufferText
  mov   r14,r1
  .short  0xF800
  TextBuffered:
  mov   r3,r0
  ldr   r0,=AppendText
  mov   r14,r0
  mov   r0,r4
  @ add   r0,#0x90
  mov   r1,#0x18
  mov   r2,#\colour
  .short  0xF800
.endm

.macro rescue_check
  mov r1, r8
  ldr r0, [r1, #0xc] @status
  mov r1, #0x10 @rescuing?
  and r0, r1
.endm

.macro draw_status_text_at, tile_x, tile_y, colour=Blue  
  draw_textID_at \tile_x, \tile_y, 0x4fa, width=9 @cond
  mov r4, r7
  sub r4, #8
  mov     r1, r8  
  mov     r0,r1   
  blh     WriteStatusText
  mov     r3,r0     
  mov     r0,r4 
  mov     r1,#0x16        @16 if status, otherwise 18???
  mov     r2,#\colour   
  blh     AppendText, r4
  mov r1, r8
  add r1, #0x30
  ldrb r2,[r1]
  cmp r2, #0
  beq NoStatusCount
  ldr r0, =(0x2003ca2+(0x20*2*\tile_y)+(2*\tile_x))
  lsr r2, #4
  mov r1, #0
  blh 0x8004be4
  NoStatusCount:
.endm

.macro draw_affinity_icon_at, tile_x, tile_y
  ldr r4, =(tile_origin+(0x20*2*\tile_y)+(2*\tile_x))
  mov r0, r8
  blh AffinityGetter
  mov     r1,r0      
  mov     r2,#0xA0       
  lsl     r2,r2,#0x7      
  mov     r0,r4    
  blh     DrawIcon 
.endm

.macro draw_icon_at, tile_x, tile_y, number=0
  @assumes icon number in r0 or else in number
  .if \number
    mov r0, #\number
  .endif
  ldr r4, =(tile_origin+(0x20*2*\tile_y)+(2*\tile_x))
  mov     r1,r0      
  mov     r2,#0xA0       
  lsl     r2,r2,#0x7      
  mov     r0,r4    
  blh     DrawIcon 
.endm

.macro draw_stats_box
  ldr     r0,=#0x8A02204        @0808748A @buffer TSA
  ldr     r4,=#0x2020188        @0808748C
  mov     r1,r4       @0808748E
  blh      #0x8012F50       @08087490
  ldr     r0,=#0x20049EE        @08087494
  mov     r2,#0xC1        @08087496
  lsl     r2,r2,#0x6        @08087498
  mov     r1,r4       @0808749A
  blh      #0x80D74A0       @0808749C
  ldr     r0,=#0x8205A24        @080874A0
  blh      #0x8086E00        @080874A2
  @numbers
  ldr     r6,=StatScreenStruct        @08087532
  ldr     r0,[r6,#0xC]        @08087534
  blh      #0x8016B58       @08087536
  mov     r4,r0       @0808753A
  mov     r5,#0x0       @0808753C
  ldr     r0,[r6,#0xC]        @0808753E
  ldr     r0,[r0,#0x4]        @08087540
  ldrb    r0,[r0,#0x4]        @08087542
  cmp     r0,#0x62        @08087544
  beq     loc_0x80875F8        @08087546
  cmp     r0,#0x34        @08087548
  beq     loc_0x808757C        @0808754A
  cmp     r4,#0x0       @0808754C
  blt     loc_0x808757C        @0808754E
  lsl     r4,r4,#0x1        @08087550
  add     r0,r4,#1       @08087552
  lsl     r0,r0,#0x6        @08087554
  ldr     r1,=#0x2003D4C        @08087556
  add     r0,r0,r1        @08087558
  mov     r1,#0x0       @0808755A
  mov     r2,#0x35        @0808755C
  blh      0x8004B0C        @0808755E
  add     r0,r4,#2       @08087562
  lsl     r0,r0,#0x6        @08087564
  ldr     r1,=#0x200472E        @08087566
  add     r0,r0,r1        @08087568
  ldr     r1,=#0x8A02250        @0808756A
  mov     r2,#0xC1        @0808756C
  lsl     r2,r2,#0x6        @0808756E
  blh      0x80D74A0        @08087570
  ldr     r0,[r6,#0xC]        @08087574
  add     r0,#0x1E        @08087576
  add     r0,r0,r4        @08087578
  ldrh    r5,[r0]       @0808757A
  loc_0x808757C:
  ldr     r0,=StatScreenStruct        @0808757C
  ldr     r0,[r0,#0xC]        @0808757E
  ldr     r0,[r0,#0x4]        @08087580
  ldrb    r0,[r0,#0x4]        @08087582
  cmp     r0,#0x62        @08087584
  beq     loc_0x80875F8        @08087586
  cmp     r0,#0x34        @08087588
  beq     loc_0x80875F8        @0808758A
  ldr     r4,=#0x200407C        @0808758C
  ldr     r6,=#0x203A4EC        @0808758E
  mov     r0,r6       @08087590
  add     r0,#0x5A        @08087592
  mov     r1,#0x0       @08087594
  ldsh    r2,[r0,r1]        @08087596
  mov     r0,r4       @08087598
  mov     r1,#0x2       @0808759A
  blh      #0x8004B94        @0808759C
  mov     r0,r4       @080875A0
  add     r0,#0x80        @080875A2
  mov     r1,r6       @080875A4
  add     r1,#0x60        @080875A6
  mov     r3,#0x0       @080875A8
  ldsh    r2,[r1,r3]        @080875AA
  mov     r1,#0x2       @080875AC
  blh      #0x8004B94        @080875AE
  mov     r0,r4       @080875B2
  add     r0,#0xE       @080875B4
  mov     r1,r6       @080875B6
  add     r1,#0x66        @080875B8
  mov     r3,#0x0       @080875BA
  ldsh    r2,[r1,r3]        @080875BC
  mov     r1,#0x2       @080875BE
  blh      #0x8004B94       @080875C0
  add     r4,#0x8E        @080875C4
  mov     r0,r6       @080875C6
  add     r0,#0x62        @080875C8
  mov     r6,#0x0       @080875CA
  ldsh    r2,[r0,r6]        @080875CC
  mov     r0,r4       @080875CE
  mov     r1,#0x2       @080875D0
  blh      #0x8004B94        @080875D2
  b       loc_0x8087630        @080875D6
  loc_0x80875F8:
  ldr     r4,=#0x200407C        @080875F8
  mov     r0,r4       @080875FA
  mov     r1,#0x2       @080875FC
  mov     r2,#0xFF        @080875FE
  blh      #0x8004B94        @08087600
  mov     r0,r4       @08087604
  add     r0,#0x80        @08087606
  mov     r1,#0x2       @08087608
  mov     r2,#0xFF        @0808760A
  blh      #0x8004B94        @0808760C
  mov     r0,r4       @08087610
  add     r0,#0xE       @08087612
  mov     r1,#0x2       @08087614
  mov     r2,#0xFF        @08087616
  blh      #0x8004B94        @08087618
  add     r4,#0x8E        @0808761C
  ldr     r0,=#0x203A4EC        @0808761E
  add     r0,#0x62        @08087620
  mov     r1,#0x0       @08087622
  ldsh    r2,[r0,r1]        @08087624
  mov     r0,r4       @08087626
  mov     r1,#0x2       @08087628
  blh      #0x8004B94        @0808762A
  mov     r5,#0x0       @0808762E
  loc_0x8087630:
  mov     r0,r5       @08087630
  blh      #0x8016CC0        @08087632
  mov     r5,r0       @08087636
  ldr     r4,=#0x2003CB4        @08087638
  blh      #0x8003EDC        @0808763A
  mov     r1,#0x37        @0808763E
  sub     r1,r1,r0        @08087640
  mov     r0,r4       @08087642
  mov     r2,#0x2       @08087644
  mov     r3,r5       @08087646
  blh      #0x8004480, r4        @08087648
  mov     r4,#0x0       @0808764C
  ldr     r0,=#0x2003D2C        @0808764E
  ldr     r3,=#0x7060       @08087650
  mov     r5,r3       @08087652
  ldr     r6,=#0x2C2        @08087654
  add     r2,r0,r6        @08087656
  ldr     r1,=#0x7068       @08087658
  mov     r3,r1       @0808765A
  add     r6,#0x40        @0808765C
  add     r1,r0,r6        @0808765E
  loc_0x8087660:
  add     r0,r4,r5        @08087660
  strh    r0,[r2]       @08087662
  add     r0,r4,r3        @08087664
  strh    r0,[r1]       @08087666
  add     r2,#0x2       @08087668
  add     r1,#0x2       @0808766A
  add     r4,#0x1       @0808766C
  cmp     r4,#0x7       @0808766E
  ble     loc_0x8087660        @08087670
.endm

.macro draw_items_text
  push {r7}
  mov r7, r8
  push {r7}
  ldr     r2,=StatScreenStruct        @080874A6
  ldr     r1,[r2,#0xC]        @080874A8
  ldr     r0,[r1,#0x4]        @080874AA
  ldrb    r0,[r0,#0x4]        @080874AC
  cmp     r0,#0x62        @080874AE
  beq     loc_0x8087532       @080874B0
  cmp     r0,#0x34        @080874B2
  beq     loc_0x8087532       @080874B4
  mov     r4,#0x0       @080874B6
  ldrh    r5,[r1,#0x1E]       @080874B8
  cmp     r5,#0x0       @080874BA
  beq     loc_0x8087532       @080874BC
  mov     r7,r2       @080874BE
  mov     r8,r4       @080874C0
  mov     r6,#0x40        @080874C2
  loc_0x80874C4:
  ldr     r2,[r7,#0xC]        @080874C4
  ldr     r0,[r2,#0xC]        @080874C6
  mov     r1,#0x80        @080874C8
  lsl     r1,r1,#0x5        @080874CA
  and     r0,r1       @080874CC
  cmp     r0,#0x0       @080874CE
  beq     loc_0x80874F8       @080874D0
  mov     r0,r2       @080874D2
  blh      #0x80179D8       @080874D4
  sub     r0,#0x1       @080874D8
  cmp     r4,r0       @080874DA
  bne     loc_0x80874F8       @080874DC
  mov     r2,#0x4       @080874DE
  b       loc_0x808750A       @080874E0
  .ltorg
  loc_0x80874F8:
  ldr     r0,[r7,#0xC]        @080874F8
  mov     r1,r5       @080874FA
  blh      #0x8016EE4       @080874FC
  mov     r2,#0x0       @08087500
  lsl     r0,r0,#0x18       @08087502
  cmp     r0,#0x0       @08087504
  bne     loc_0x808750A       @08087506
  mov     r2,#0x1       @08087508
  loc_0x808750A:
  lsl     r0,r4,#0x3        @0808750A
  ldr     r1,=#0x2003C8C        @0808750C
  add     r0,r0,r1        @0808750E
  ldr     r3,=#0x2003D2E        @08087510
  add     r3,r6,r3        @08087512
  mov     r1,r5       @08087514
  blh      #0x8016A2C, r5       @08087516
  mov     r0,#0x2       @0808751A
  add     r8,r0       @0808751C
  add     r6,#0x80        @0808751E
  add     r4,#0x1       @08087520
  cmp     r4,#0x4       @08087522
  bgt     loc_0x8087532       @08087524
  ldr     r0,[r7,#0xC]        @08087526
  add     r0,#0x1E        @08087528
  add     r0,r8       @0808752A
  ldrh    r5,[r0]       @0808752C
  cmp     r5,#0x0       @0808752E
  bne     loc_0x80874C4       @08087530
  loc_0x8087532:
  pop {r7}
  mov r8, r7
  pop {r7}
.endm

.macro clear_buffers
  blh 0x8003c94
  blh 0x8003578
  blh 0x8086df0 @clear 2003c00 region
  @blh 0x80790a4
  @ ldr r4, =StatScreenStruct
  @ ldr r0, [r4, #0xc]
  @ mov r1, #0x50
  @ mov r2, #0x8A
  @blh 0x80784f4
  @ str r0, [r4, #0x10]
  blh 0x8086e44
  mov r0, #0
  str r0, [sp]
  mov r0, sp
  ldr r1, =0x6001380
  ldr r2, =0x1000a68
  swi 0xC @clear vram
.endm


.equ Sword, 0
.equ Lance, 1
.equ Axe, 2
.equ Bow, 3
.equ Staff, 4
.equ Anima, 5
.equ Light, 6
.equ Dark, 7
.macro draw_weapon_rank_at, tile_x, tile_y, weapon, id
  mov     r0,#\id
  mov     r1,#\tile_x
  mov     r2,#\tile_y
  mov     r3,#\weapon      @08087862
  blh     DrawWeaponRank, r4        @08087864
.endm

.macro get_attack_speed
  mov r0, r8
  blh SpdGetter
  mov r4, r0 @speed in r4
  mov r0, r8
  blh EquippedWeaponGetter
  blh 0x801760c
  mov r5, r0 @weight in r5
  mov r1, r8
  ldr     r0,[r1,#0x4] @class
  mov     r3,#0x11     @con
  ldsb    r3,[r0,r3]   
  ldr     r0,[r1]      
  ldrb    r0,[r0,#0x13]@char bonus
  lsl     r0,r0,#0x18  
  asr     r0,r0,#0x18  
  add     r3,r3,r0     
  mov     r0,#0x1A     
  ldsb    r0,[r1,r0]   @body ring bonus
  add     r0,r3,r0 @total con in r0
  cmp r0, r5
  blt WeighedDown
  mov r0, r4 @put speed directly
  b AS_End
  WeighedDown:
  sub r5, r0 @weight - con in r5
  sub r0, r4, r5
  AS_End:
.endm

.macro get_attack
  ldr     r6,=#0x203A4EC        @0808758E
  mov     r0,r6       @08087590
  add     r0,#0x5A        @08087592
  mov     r1,#0x0       @08087594
  ldsh    r0,[r0,r1]        @08087596
.endm

.macro get_hit
  mov     r1,r6       @080875A4
  add     r1,#0x60        @080875A6
  mov     r3,#0x0       @080875A8
  ldsh    r0,[r1,r3]        @080875AA
.endm

.macro get_avoid
  ldr     r0,=#0x203A4EC        @0808761E
  add     r0,#0x62        @08087620
  mov     r1,#0x0       @08087622
  ldsh    r0,[r0,r1]        @08087624
.endm

@requires alternateicondraw
.macro draw_skill_icon_at, tile_x, tile_y, number=0
  @assumes icon number in r0 or else in number
  .if \number
    mov r0, #\number
  .endif
  ldr r4, =(tile_origin+(0x20*2*\tile_y)+(2*\tile_x))
  mov     r1,r0      
  mov     r2,#0x80
  lsl     r2,r2,#0x7      
  mov     r0,r4    
  bl      DrawSkillIcon 
.endm

.macro draw_icon_alt, tile_x, tile_y, number
  mov r0, #\number
  ldr r4, =(tile_origin+(0x20*2*\tile_y)+(2*\tile_x))
  mov     r1,r0      
  mov     r2,#0x80
  lsl     r2,r2,#0x7      
  mov     r0,r4    
  bl      DrawSkillIcon 
.endm

