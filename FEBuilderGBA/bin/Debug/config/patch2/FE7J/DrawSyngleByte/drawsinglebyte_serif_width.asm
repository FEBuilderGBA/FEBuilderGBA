@thumb

;08005530 から呼び出される. FE7J
;r5 work
;r4 store pointer?
;r3 work
;r2 text
;r1 work
;r0 nazo pointer
PUSH {r5} ;レジスタが足りないので追加確保
LDR r0, [r0, #0x0]
LDR r1, [r0, #0x4] ;font base

LDRB r5, [r2, #0x0] ;*text (マルチバイトの1バイト目)
CMP r5,#0x81          ;念のためSJIS2バイト目チェック
BLT SINGLE_BYTE
LDRB r3, [r2, #0x1]     ;SJIS2バイト目?
CMP r3,#0x40
BLT SINGLE_BYTE
ADD r2, #0x2          ;text+=2 
B FIND_MULTIBYTE

SINGLE_BYTE
;LDRB r5, [r2, #0x0] ;*text 
mov	r3, #0x40
ADD r2, #0x1			;text++

FIND_MULTIBYTE
;MULTIBYTE マルチバイト
LSL r0  ,r3 #0x2
ADD r0  ,r0, r1
ldr r3, =$0x100		;-0x100
sub	r0, r0, r3
LDR r0, [r0, #0x0]


MULTIBYTE_FINDER_LOOP
CMP r0, #0x0
BEQ EXIT    ;ない

LDRB r3,[r0,#0x4]
CMP r5,r3
BEQ EXIT    ;発見!
LDR r0,[r0,#0x0]
b MULTIBYTE_FINDER_LOOP


EXIT
POP {R5}
CMP r0,0
BEQ EXIT2

;マルチバイト版は、cmp r0,0チェックが前方にあるため、
;storeも含めて実装する.
ldrb r1, [r0, #5]	;width
str	r1, [r4, #0]

EXIT2

;r2 = 次のtext
ldr r1,=$08005554
mov pc,r1
