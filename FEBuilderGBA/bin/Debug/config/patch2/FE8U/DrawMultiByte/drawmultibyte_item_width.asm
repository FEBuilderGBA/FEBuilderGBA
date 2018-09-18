@thumb

;08004576 から呼び出される. FE8U
;r3 work
;r2 width
;r1 text
;r0 nazo pointer
PUSH {r4,r5} ;レジスタが足りないので追加で確保する.
LDR r0, [r0, #0x0]
LDR r3, [r0, #0x4]

LOOP_START
LDRB r4, [r1, #0x0]   ;*text (マルチバイトの1バイト目)
CMP r4,#0x81          ;念のためSJIS2バイト目チェック
BLT SINGLE_BYTE
LDRB r5, [r1, #0x1]     ;SJIS2バイト目?
CMP r5,#0x40
BLT SINGLE_BYTE


;MULTIBYTE マルチバイト
LSL r0  ,r5, #0x2
ADD r0  ,r0, R3
ldr r5, =$0x100		;-0x100
sub	r0, r0, r5
LDR r0, [r0, #0x0]

ADD r1, #0x2          ;text+=2 

MULTIBYTE_FINDER_LOOP
CMP r0, #0x0
BEQ NEXT_TEXT    ;ない

LDRB r5,[r0,#0x4]
CMP r4,r5
BEQ FONT_WIDTH ;発見!
LDR r0,[r0,#0x0]

b MULTIBYTE_FINDER_LOOP



SINGLE_BYTE
;LDRB r4, [r1, #0x0]     ;
ADD r1, #0x1			;text++

LSL r0  ,r4, #0x2
ADD r0  ,r0, R3
LDR r0, [r0, #0x0]

FONT_WIDTH
LDRB r0, [r0, #0x5]	;width
ADD r2  ,r0, R2


NEXT_TEXT
LDRB r4, [r1, #0x0]     ;
CMP r4, #0x1
BGT LOOP_START


EXIT
POP {R4,R5}

;r2 = width
ldr r0,=$0800458E
mov pc,r0
