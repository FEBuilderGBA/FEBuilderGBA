@FE8U Called 8089624 {U}
@FE8J Called 808B894 {J}
@Replace that routine with this one from fe7 to show text instead of gfx
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


loader:
b main


strcat_gBuffer_with_decode:
push {lr}
blh     0x800a240         @GetStringFromIndex	{J}
@blh     0x8009fa8         @GetStringFromIndex	{U}
bl      strcat_gBuffer
pop {r0}
bx r0

strcat_onechar:
push {lr}
ldr     r1,=0x202A6AC           @TextBufferOffset	{U}
@ldr     r1,=0x202A6A8           @TextBufferOffset	{J}
strb    r0,[r1]            @gBuffer[0]=0x0
mov     r0,#0x0
strb    r0,[r1,#0x01]            @gBuffer[1]=0x0

mov     r0,r1
bl      strcat_gBuffer
pop {r0}
bx  r0


strcat_gBuffer_with_atoi:
push {r4,r5,lr}

mov r5,r0

mov r4 , #0x64
cmp r0 , r4
bge strcat_gBuffer_with_atoi_Loop

mov r4 , #0xA
cmp r0 , r4
bge strcat_gBuffer_with_atoi_Loop

mov r4 , #0x01


strcat_gBuffer_with_atoi_Loop:
mov r0,r5
mov r1,r4
swi #0x6          @BIOS: Div

mov r1,#0x30      @数字の0のASCII CODE	FE8U
add r0,r1         @
bl strcat_onechar

mov r0,r5
mov r1,#0x0a
swi #0x6          @BIOS: Div
mov r5, r1

mov r0,r4
mov r1,#0x0a
swi #0x6          @BIOS: Div
mov r4, r0

cmp r4,#0x01
bge strcat_gBuffer_with_atoi_Loop

pop {r4,r5}
pop {r0}
bx  r0
.align
.ltorg

strcat_gBuffer:
push {lr}

mov     r1,r0
ldr     r0,=0x2020188      @gGenericBuffer	{U}	{J} common
bl strcat

pop {r0}
bx r0


strcat:     @r0=WriteBuffer  r1=AppendText
push {lr}

strcat_FindTermLoop:
ldrb r2,[r0]
cmp  r2,#0x00
beq  strcat_AppendLoop
add  r0,#0x01
b    strcat_FindTermLoop

strcat_AppendLoop:
ldrb r2,[r1]
strb r2,[r0]
cmp  r2,#0x00
beq  strcat_Term
add  r0,#0x01
add  r1,#0x01
b    strcat_AppendLoop

strcat_Term:
pop {r0}
bx  r0
.align
.ltorg

@章タイトルIDから、章IDを求めます
chapertileid_to_chapterid:
push    {r4,lr}
mov     r4,r0

mov     r0,#0x0
blh     0x8034618                 @GetChapterDefinition {U}
@blh     0x8034520                 @GetChapterDefinition {J}

mov     r1,#0x00
chapertileid_to_chapterid_Loop:
cmp r1,#0x7f
bge chapertileid_to_chapterid_Exit
ldrb r2,[r0,#0xe]  @ChapterTitleID
cmp r2,r4
beq chapertileid_to_chapterid_Found

add r0,#0x94
add r1,#0x1
b chapertileid_to_chapterid_Loop

chapertileid_to_chapterid_Found:
mov r4,r1

chapertileid_to_chapterid_Exit:
mov r0,r4

pop  {r4}
pop  {r1}
bx r1
.align
.ltorg

sub_8082224:
push    {r4-r6,r14}
mov     r6,r0
mov     r5,#0x0
mov     r4,#0x0
b       loc_8082288
loop_808222E:
mov     r0,r6
bl      MapCharacter
cmp     r0,#0x80
bne     loc_8082250
cmp     r4,r5
bls     loc_8082246
add     r0,r4,#3
lsl     r0,r0,#0x18
lsr     r4,r0,#0x18
mov     r5,r4
b       loc_8082286
loc_8082246:
add     r0,r5,#3
lsl     r0,r0,#0x18
lsr     r5,r0,#0x18
mov     r4,r5
b       loc_8082286
loc_8082250:
lsl     r1,r0,#0x3
ldr     r0,=ChTable
add     r2,r1,r0
ldrb    r0,[r2]
sub     r1,r4,r0
ldrb    r3,[r2,#0x1]
sub     r0,r5,r3
cmp     r1,r0
ble     loc_808226C
mov     r5,r4
b       loc_808226E
lsl     r0,r0,#0x0
mov     r7,#0x84
lsr     r4,r1,#0x3
loc_808226C:
mov     r4,r5
loc_808226E:
mov     r0,r4
add     r0,#0x0FF
ldrb    r1,[r2,#0x2]
add     r0,r1,r0
lsl     r0,r0,#0x18
lsr     r4,r0,#0x18
mov     r0,r5
add     r0,#0x0FF
ldrb    r2,[r2,#0x3]
add     r0,r2,r0
lsl     r0,r0,#0x18
lsr     r5,r0,#0x18
loc_8082286:
add     r6,#0x1
loc_8082288:
ldrb    r0,[r6]
cmp     r0,#0x0
beq     loc_8082292
cmp     r0,#0x1F
bne     loop_808222E
loc_8082292:
add     r1,r4,r5
asr     r1,r1,#0x1
mov     r0,#0x0C0
sub     r0,r0,r1
asr     r0,r0,#0x1
pop     {r4-r6}
pop     {r1}
bx      r1
.ltorg

@adapted from FE7E:820F8
MapCharacter:
push    {r14}    
add     sp,#-0x20   
mov     r3,r0    
ldrb    r2,[r3]    
mov     r0,r2    
sub     r0,#0x41     
lsl     r0,r0,#0x18     
lsr     r0,r0,#0x18    
cmp     r0,#0x19   
bhi     NotUppercase    
ldrb    r0,[r3]        @uppercase
sub     r0,#0x41     
b       GotCharacterIndex 
NotUppercase:    
mov     r0,r2    
sub     r0,#0x61     
lsl     r0,r0,#0x18     
lsr     r0,r0,#0x18    
cmp     r0,#0x19    
bhi     NotLowercase    
ldrb    r0,[r3]        @lowercase
sub     r0,#0x47    
b       GotCharacterIndex    
NotLowercase:
mov     r0,r2   
sub     r0,#0x30    
lsl     r0,r0,#0x18     
lsr     r0,r0,#0x18    
cmp     r0,#0x9    
bhi     NotNumeral    
ldrb    r0,[r3]        @numerals
add     r0,#0x4    
b       GotCharacterIndex

	NotNumeral:       @\& symbol
	mov  r1, r2
	cmp  r1, #0x26
	bne Check_Apostrophe
	
		mov  r0, #0x3E
		b    GotCharacterIndex
		
	Check_Apostrophe:
	cmp r1, #0x27
	bne Check_HyphenPeriodSlash
		
		mov  r0, #0x3F
		b    GotCharacterIndex
	
	Check_HyphenPeriodSlash:
	mov r0, r2
	sub r0, #0x2C
	lsl r0, r0, #0x18
	lsr r0, r0, #0x18
	cmp r0, #0x2
	bhi Check_Colon

		@-./ symbols
		ldrb r0, [r3]
		add r0, #0x14
		b GotCharacterIndex
		
	Check_Colon:
	cmp     r1,#0x3A     
	bne     Check_CD     
	mov     r0,#0x43     
	b       GotCharacterIndex  
	
	Check_CD:        @Í
	cmp     r1,#0xCD
	bne     Check_9C    
	mov     r0,#0x44   
	b       GotCharacterIndex 
	
	Check_9C:        @œ
	cmp     r1,#0x9C
	bne     Check_E0
	mov     r0,#0x45
	b       GotCharacterIndex 
	
	Check_E0:        @à
	cmp     r1,#0xE0
	bne     Check_E1
	mov     r0,#0x46
	b       GotCharacterIndex
	
	Check_E1:
	ldrb    r0,[r3]
	mov     r1,r0
	cmp     r1,#0xE1
	bne     Check_E2
	mov     r0,#0x47
	b       GotCharacterIndex
	
	Check_E2:
	cmp     r1,#0xE2
	bne     Check_E4
	mov     r0,#0x48
	b       GotCharacterIndex
	
	Check_E4:
	cmp     r1,#0xE4
	bne     Check_E8
	mov     r0,#0x49
	b       GotCharacterIndex
	
	Check_E8:
	cmp     r1,#0xE8
	bne     Check_E9
	mov     r0,#0x4A
	b       GotCharacterIndex
	
	Check_E9:
	cmp     r1,#0xE9
	bne     Check_EA
	mov     r0,#0x4B
	b       GotCharacterIndex
	
	Check_EA:
	cmp     r1,#0xEA
	bne     Check_ED
	mov     r0,#0x4C
	b       GotCharacterIndex
	
	Check_ED:
	cmp     r1,#0xED
	bne     Check_EE
	mov     r0,#0x4D
	b       GotCharacterIndex
	
	Check_EE:
	cmp     r1,#0xEE
	bne     Check_F1
	mov     r0,#0x4E
	b       GotCharacterIndex
	
	Check_F1:
	cmp     r1,#0xF1
	bne     Check_F2
	mov     r0,#0x56
	b       GotCharacterIndex
	
	Check_F2:
	cmp     r1,#0xF2
	bne     Check_F3
	mov     r0,#0x4F
	b       GotCharacterIndex
	
	Check_F3:
	cmp     r1,#0xF3
	bne     Check_F4
	mov     r0,#0x50
	b       GotCharacterIndex
	
	Check_F4:
	cmp     r1,#0xF4
	bne     Check_F6
	mov     r0,#0x51
	b       GotCharacterIndex
	
	Check_F6:
	cmp     r1,#0xF6
	bne     Check_FC
	mov     r0,#0x52
	b       GotCharacterIndex
	
	Check_FC:
	cmp     r1,#0xFC
	bne     Check_Parenthese_Open
	mov     r0,#0x53
	b       GotCharacterIndex
	
	Check_Parenthese_Open:
	cmp     r1,#0x28
	bne     Check_Parenthese_Close
	mov     r0,#0x54
	b       GotCharacterIndex
	
	Check_Parenthese_Close:
	cmp     r1,#0x29
	bne     MapToSpaceChar
	mov     r0,#0x55
	b       GotCharacterIndex
	
@anything unrecognized is now a space.

MapToSpaceChar:   
mov     r0,#0x80
GotCharacterIndex:  
add     sp,#0x20     
pop     {r1}    
bx      r1
.align 
.ltorg

sub_8082168:
push    {r4-r7,r14}
mov     r7,r10
mov     r6,r9
mov     r5,r8
push    {r5-r7}
add     sp,#-0x18
str     r0,[sp]
str     r1,[sp,#0x4]
mov     r4,r2
str     r3,[sp,#0x8]
mov     r0,r4
bl      sub_80820CC
mov     r1,#0x0FF
and     r1,r0
str     r1,[sp,#0x0C]
asr     r0,r0,#0x8
lsl     r0,r0,#0x4
str     r0,[sp,#0x10]
lsl     r4,r4,#0x3
ldr     r0,=ChTable
add     r6,r4,r0
ldrb    r2,[r6,#0x6]
b       loc_808220E
mov     r7,#0x84
lsr     r4,r1,#0x3
loc_808219C:
mov     r5,#0x0
add     r1,r2,#1
str     r1,[sp,#0x14]
ldrb    r0,[r6,#0x5]
cmp     r5,r0
bge     loc_808220C
ldr     r1,[sp,#0x10]
add     r0,r1,r2
asr     r1,r0,#0x3
lsl     r1,r1,#0x0A
mov     r10,r1
mov     r7,#0x7
and     r0,r7
lsl     r0,r0,#0x2
mov     r9,r0
asr     r0,r2,#0x3
lsl     r0,r0,#0x0A
mov     r8,r0
and     r2,r7
lsl     r2,r2,#0x2
mov     r12,r2
loc_80821C6:
@loops to here.
ldr     r2,[sp,#0x0C]
add     r0,r2,r5
ldr     r1,[sp,#0x8]
add     r4,r1,r5
asr     r1,r0,#0x3
lsl     r1,r1,#0x5
ldr     r2,[sp]
add     r1,r2,r1
add     r1,r10
add     r1,r9
and     r0,r7
lsl     r3,r0,#0x2
mov     r0,#0x0F
lsl     r0,r3
ldr     r2,[r1]
and     r2,r0
cmp     r2,#0x0
beq     loc_8082204
asr     r0,r4,#0x3
lsl     r0,r0,#0x5
ldr     r1,[sp,#0x4]
add     r0,r1,r0
add     r0,r8
add     r0,r12
lsr     r2,r3
and     r4,r7
lsl     r1,r4,#0x2
lsl     r2,r1
ldr     r1,[r0]
orr     r1,r2

@@@@@@@@@@@@@HERE it stores to vram.

str     r1,[r0]
loc_8082204:
add     r5,#0x1
ldrb    r2,[r6,#0x5]
cmp     r5,r2
blt     loc_80821C6
loc_808220C:
ldr     r2,[sp,#0x14]
loc_808220E:
ldrb    r0,[r6,#0x7]
cmp     r2,r0
blt     loc_808219C
add     sp,#0x18
pop     {r3-r5}
mov     r8,r3
mov     r9,r4
mov     r10,r5
pop     {r4-r7}
pop     {r0}
bx      r0
.align
.ltorg

sub_80820CC:
mov     r2,#0x0
ldr     r1,=ChTable
cmp     r0,#0x0
beq     loc_80820E0
loc_80820D4:
ldrb    r3,[r1,#0x4]
add     r2,r3,r2
add     r1,#0x8
sub     r0,#0x1
cmp     r0,#0x0
bne     loc_80820D4
loc_80820E0:
mov     r0,r2
bx      r14
.align
.ltorg


main:
@r0 is 0xb40 (position?)
@r1 is chapter number. 0x54 is no data?

push    {r4-r7,r14}
mov     r7,r8
push    {r7}
add     sp,#-0x4
mov     r4,r0
mov     r0,r1
bl      LoadChapterName
mov     r7,r0
lsl     r0,r4,#0x5
mov     r1,#0x0C0
lsl     r1,r1,#0x13
add     r1,r1,r0
mov     r8,r1
mov     r0,r7
bl      sub_8082224
lsl     r0,r0,#0x18
lsr     r5,r0,#0x18
mov     r6,r5
ldr     r1,=0x203E78C	@gChapterTitleGrahicsWorkBuffer	{U}
@ldr     r1,=0x0203E788	@gChapterTitleGrahicsWorkBuffer	{J}
ldr     r2,=0x3FF
mov     r0,r2
and     r4,r0
mov     r0,#0x0
strh    r4,[r1,#0x2]
str     r0,[sp]
ldr     r2,=0x1000200         @constant: used for cpufastset   
mov     r0,r13
mov     r1,r8

swi #0xC

ldr     r0, =ChFont
ldr     r1,=0x2020188      @gGenericBuffer	{U}	{J} common
blh      0x8012f50         @UnLZ77Decompress	{U}
@blh      0x8013008         @UnLZ77Decompress	{J}
b       loc_80823C6
.align
.ltorg
 
loc_8082368:
mov     r0,r7
bl      MapCharacter
mov     r2,r0
cmp     r2,#0x80
bne     loc_8082386
cmp     r6,r5
bls     loc_808237C
add     r0,r6,#3
b       loc_808237E
loc_808237C:
add     r0,r5,#3
loc_808237E:
lsl     r0,r0,#0x18
lsr     r5,r0,#0x18
mov     r6,r5
b       loc_80823C4

loc_8082386:
lsl     r1,r2,#0x3
ldr     r0, =ChTable
add     r4,r1,r0
ldrb    r3,[r4]
sub     r1,r6,r3
ldrb    r3,[r4,#0x1]
sub     r0,r5,r3
cmp     r1,r0
ble     loc_80823A0
mov     r5,r6
b       loc_80823A2
.ltorg
loc_80823A0:
mov     r6,r5
loc_80823A2:
ldr     r0,=0x2020188      @gGenericBuffer	{U}	{J} common
mov     r1,r8
mov     r3,r6
bl      sub_8082168
mov     r0,r6
add     r0,#0x0FF
ldrb    r1,[r4,#0x2]
add     r0,r1,r0
lsl     r0,r0,#0x18
lsr     r6,r0,#0x18
mov     r0,r5
add     r0,#0x0FF
ldrb    r4,[r4,#0x3]
add     r0,r4,r0
lsl     r0,r0,#0x18
lsr     r5,r0,#0x18
loc_80823C4:
add     r7,#0x1
loc_80823C6:
ldrb    r0,[r7]
cmp     r0,#0x0
beq     loc_80823D0
cmp     r0,#0x1F
bne     loc_8082368
loc_80823D0:
add     sp,#0x4
pop     {r3}
mov     r8,r3
pop     {r4-r7}
pop     {r0}
bx      r0
.align
.ltorg  

LoadChapterName:
push    {r4,r5,r14}
mov     r4,r0
cmp     r4,#0x0                 @if negative number -> nodata_text
blt     nodata_text
cmp     r4,#0x55                @Epilogue
beq     epilogue_text
cmp     r4,#0x57                @Creature Campaign
beq     postgame_text
cmp     r4,#0x46                @world map skirmishes
bge     worldmap_node_text
b       chapter_text

nodata_text:
mov     r0,#0xCC      @NO DATA	{U}
@mov     r0,#0x61      @NO DATA	{J}
blh     0x800a240         @GetStringFromIndex	{J}
@blh     0x8009fa8         @GetStringFromIndex	{U}
b       end_80822a4

epilogue_text:
ldr     r0,=0x7cf     @Epilogue (song name) {U}
@ldr     r0,=0x746     @Epilogue (song name) {J}
blh     0x800a240         @GetStringFromIndex	{J}
@blh     0x8009fa8         @GetStringFromIndex	{U}
b       end_80822a4

worldmap_node_text:
ldr     r0,=0x03005280  @gSomeWMEventRelatedStruct	{U}
@ldr     r0,=0x03005270  @gSomeWMEventRelatedStruct	{J}
ldrb    r0,[r0,#0x11]
blh     0x080BBA28      @GetWorldMapNodeName	{U}
@blh     0x080c086c      @GetWorldMapNodeName	{J}
b       end_80822a4

postgame_text:
ldr     r0,=0x7D0     @ blank	{U}
@ldr     r0,=0x0       @ blank	{J}	候補がない
blh     0x800a240         @GetStringFromIndex	{J}
@blh     0x8009fa8         @GetStringFromIndex	{U}
b       end_80822a4
.ltorg

chapter_text:
mov     r0,r4
bl chapertileid_to_chapterid
mov     r4,r0
blh     0x8034618                 @GetChapterDefinition {U}
@blh     0x8034520                 @GetChapterDefinition {J}
mov     r5, r0

ldr     r1,=0x2020188      @gGenericBuffer	{U}	{J} common
mov     r0,#0x0
strb    r0,[r1]            @gBuffer[0]=0x0

CheckSpecialChapter:              @終章や序章等の特殊な章のチェック
ldr     r3, =SpecialChapter
sub     r3, #0x4
SpecialChapterLoop:
add     r3, #0x4
ldr     r0, [r3]
cmp     r0, #0x0
beq     CheckTowerOrRuins @Break
ldrb    r0, [r3]
cmp     r0, r4
bne     SpecialChapterLoop
ldrh    r0, [r3,#0x2]
cmp     r0,#0x00
beq     Chapter_Main_Text
bl      strcat_gBuffer_with_decode
b       Space_Text


CheckTowerOrRuins:
blh     0x080BD068        @GetChapterThing {U}
@blh     0x080C1E74        @GetChapterThing {J}
cmp     r0,#0x0
bne     Chapter_Main_Text   @塔やタワーならば本文を表示しない

NormalChapter:
ldr     r0, =0x157        @第 Ch	{U}
@ldr     r0, =0xdf        @第 Ch	{J}
bl      strcat_gBuffer_with_decode

AppendChaperNumber:
mov     r0, #0x80
ldrb    r0, [r5 , r0]             @MapSetting->ChapterID
lsr     r0,#0x1      @ChapterNumber
bl      strcat_gBuffer_with_atoi

@@@@mov r0, #0xe0    @章         FE8Jのみ {J}
@@@@@bl      strcat_gBuffer_with_decode   @{J}

mov     r0, #0x80
ldrb    r1, [r5 , r0]   @MapSetting->ChapterID
mov     r2,#0x01
and     r1,r2
cmp     r1,#0x00
beq     Space_Text

AppendGaiden:     @外伝の追加
ldr r0 ,=0x158    @外伝	{U}
@ldr r0 ,=0xe1     @外伝	{J}
bl      strcat_gBuffer_with_decode
b       Space_Text


Space_Text: @Append Space 余白
mov     r0,#0x3a		@: ASCI CODE
bl      strcat_onechar

mov     r0,#0x20		@SPACE ASCI CODE
bl      strcat_onechar

Chapter_Main_Text:
@Chapter Name Main text
mov     r0,#0x70
ldrh    r0,[r5,r0]
bl      strcat_gBuffer_with_decode

Reverse_strcpy_TextBuffer:
ldr     r0,=0x202A6AC           @TextBufferOffset	{U}
@ldr     r0,=0x202A6A8           @TextBufferOffset	{J}
ldr     r1,=0x2020188           @gGenericBuffer	{U}	{J} common
blh     0x080D1D3C	@strcpy   return r0=TextBufferOffset	{U}
@blh     0x080d69bc	@strcpy   return r0=TextBufferOffset	{J}

end_80822a4:
pop     {r4,r5}
pop     {r1}
bx      r1
.align
.ltorg

