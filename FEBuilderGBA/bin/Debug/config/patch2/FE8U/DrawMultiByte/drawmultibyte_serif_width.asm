@thumb

;08004540 から呼び出される. FE8U
;r5 work
;r4 work
;r3 store pointer?
;r2 text
;r1 work
;r0 nazo pointer
PUSH {r4,r5} ;レジスタが足りないので追加確保
LDR r0, [r0, #0x0]
LDR r1, [r0, #0x4] ;font base

LDRB r5, [r2, #0x0] ;*text (マルチバイトの1バイト目)
CMP r5,#0x81          ;念のためSJIS2バイト目チェック
BLT SINGLE_BYTE
LDRB r4, [r2, #0x1]     ;SJIS2バイト目?
CMP r4,#0x40
BLT SINGLE_BYTE

;MULTIBYTE マルチバイト
LSL r0  ,r4 #0x2
ADD r0  ,r0, r1
ldr r4, =$0x100		;-0x100
sub	r0, r0, r4
LDR r0, [r0, #0x0]

ADD r2, #0x2          ;text+=2 

MULTIBYTE_FINDER_LOOP
CMP r0, #0x0
BEQ EXIT    ;ない

LDRB r4,[r0,#0x4]
CMP r5,r4
BEQ EXIT    ;発見!
LDR r0,[r0,#0x0]
b MULTIBYTE_FINDER_LOOP

SINGLE_BYTE
;LDRB r5, [r2, #0x0] ;*text 
ADD r2, #0x1			;text++

LSL r0  ,r5 #0x2
ADD r0  ,r0, r1
LDR r0, [r0, #0x0]

EXIT
POP {R4,R5}
;r0 = fontdata ない場合は #0x00
;r3 store pointer?
;r2 = 次のtext
ldr r1,=$0800454E
mov pc,r1
