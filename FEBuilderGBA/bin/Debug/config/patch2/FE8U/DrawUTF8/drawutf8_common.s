.thumb

@@Arg
@r0 = Font Extra Pointer 
@r1 = String
@@Ret
@r1 = String Text
@r3 = Font Pointer

PUSH {r4,lr}

LDRB r3, [r1, #0x0]   @*text (マルチバイトの1バイト目)
CMP r3,#0x00          @TERM
BEQ NULL_TERM
CMP r3,#0x80          @UTF-8 broken skip
@BLT SINGLE_BYTE      @これでは、既存ROMにある0x80が救済できない...
BLE SINGLE_BYTE       @UTF-8の規格を逸脱するが、GBAFEで多用される0x80の救済. 

CMP r3,#0xFC          @UTF-8 6byte word
BGE UTF_8_6
CMP r3,#0xF8          @UTF-8 5byte word
BGE UTF_8_5
CMP r3,#0xF0          @UTF-8 4byte word
BGE UTF_8_4
CMP r3,#0xE0          @UTF-8 3byte word
BGE UTF_8_3
CMP r3,#0xC0          @UTF-8 2byte word
BGE UTF_8_2

@UTF_8_BROKEN:        @壊れたUTF8
ADD r1, #0x01         @SKIP
B NotFound

NULL_TERM:
B   NotFound

SINGLE_BYTE:
LDRB r3, [r1, #0x0]
ADD r1, #0x1			@text++
B   FIND_FONT

UTF_8_2:
MOV  r4,#0x1F
AND  r3,r4
LSL  r3,#0x6

LDRB r2, [r1, #0x1]
MOV  r4,#0x3F
AND  r2,r4

ORR  r3,r2

ADD r1, #0x2
B   FIND_FONT


UTF_8_3:
MOV  r4,#0x0F
AND  r3,r4
LSL  r3,#0x6

LDRB r2, [r1, #0x1]
MOV  r4,#0x3F
AND  r2,r4

ORR  r3,r2
LSL  r3,#0x6

LDRB r2, [r1, #0x2]
AND  r2,r4

ORR  r3,r2

ADD r1, #0x3
B   FIND_FONT


UTF_8_4:
MOV  r4,#0x7
AND  r3,r4
LSL  r3,#0x6

LDRB r2, [r1, #0x1]
MOV  r4,#0x3F
AND  r2,r4

ORR  r3,r2
LSL  r3,#0x6

LDRB r2, [r1, #0x2]
AND  r2,r4

ORR  r3,r2
LSL  r3,#0x6

LDRB r2, [r1, #0x3]
AND  r2,r4

ORR  r3,r2

ADD r1, #0x4
B   FIND_FONT


UTF_8_5:
MOV  r4,#0x3
AND  r3,r4
LSL  r3,#0x6

LDRB r2, [r1, #0x1]
MOV  r4,#0x3F
AND  r2,r4

ORR  r3,r2
LSL  r3,#0x6

LDRB r2, [r1, #0x2]
AND  r2,r4

ORR  r3,r2
LSL  r3,#0x6

LDRB r2, [r1, #0x3]
AND  r2,r4

ORR  r3,r2
LSL  r3,#0x6

LDRB r2, [r1, #0x4]
AND  r2,r4

ORR  r3,r2

ADD r1, #0x5
B   FIND_FONT


UTF_8_6:
MOV  r4,#0x1
AND  r3,r4
LSL  r3,#0x6

LDRB r2, [r1, #0x1]
MOV  r4,#0x3F
AND  r2,r4

ORR  r3,r2
LSL  r3,#0x6

LDRB r2, [r1, #0x2]
AND  r2,r4

ORR  r3,r2
LSL  r3,#0x6

LDRB r2, [r1, #0x3]
AND  r2,r4

ORR  r3,r2
LSL  r3,#0x6

LDRB r2, [r1, #0x4]
AND  r2,r4

ORR  r3,r2
LSL  r3,#0x6

LDRB r2, [r1, #0x5]
AND  r2,r4

ORR  r3,r2

ADD r1, #0x6
B   FIND_FONT


FIND_FONT:
LSL r2, r3, #32 - 8     @一番下の8バイトを取得したい。
LSR r2, r2, #32 - 8     @
LSL r2, r2, #2          @同時に、 *2 したい。  (moji & 0xff) * 2

LSR r3,r3,#8            @moji = moji >> 8

ldr r0,=0x02028E70   @FE8U  TextParams
ldr r0,[r0]          @ TextParams->1st
LDR r0, [r0, #0x4]   @ TextParams->1st->font
LDR r0, [r0, r2]

Loop:
CMP r0, #0x00
BEQ NotFound


@struct font
@{
@	void*	next_pointer;		//+0x0 Japanese ROM only. Otherwise it is null.
@	byte	utf2byte;			//+0x4 Japanese ROM only. Otherwise it is 0.
@	byte	width;				//+0x5 The width of the font.
@	byte	utf3byte;			//+0x6 Fixed 00.
@	byte	utf4byte;			//+0x7 Fixed 00.
@	byte	bitmap4color[64];	//+0x8 Bitmap 4 colors.
@}; sizeof() == 72(0x48)

LSL r2, r3, #32 - 8       @ (moji) & 0xff
LSR r2, r2, #32 - 8       @
LDRB r4 , [r0 , #0x4]
cmp r2,r4
BNE NextFont

LSR r2, r3, #8            @ (moji >> 8) & 0xff
LSL r2, r2, #32 - 8       @
LSR r2, r2, #32 - 8
LDRB r4 , [r0 , #0x6]
cmp r2,r4
BNE NextFont

LSR r2, r3, #16            @ (moji >> 16) & 0xff
LSL r2, r2, #32 - 8        @
LSR r2, r2, #32 - 8
LDRB r4 , [r0 , #0x7]
cmp r2,r4
BEQ Found


NextFont:
ldr r0,[r0,#0x00]
B   Loop

NotFound:
mov r0,#0x00

Found:

ldr r3,=0x02028E70   @FE8U  TextParams
ldr r3,[r3]
pop {pc,r4}
