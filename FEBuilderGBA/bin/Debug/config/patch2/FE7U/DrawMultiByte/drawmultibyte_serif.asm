@thumb

;08005BA4 から呼び出される. FE7U
;r5 fontbase?
;r4 text
;r0 nazo pointer
LDR r3, [r0, #0x0]
LDR r2, [r3, #0x4]

LDRB r1, [r4, #0x0]   ;*text (マルチバイトの1バイト目)
CMP r1,#0x81          ;念のためSJIS2バイト目チェック
BLT SINGLE_BYTE
LDRB r1, [r4, #0x1]     ;SJIS2バイト目?
CMP r1,#0x40
BLT SINGLE_BYTE


;MULTIBYTE マルチバイト
LDR r2, [r3, #0x4]
LSL r0  ,r1 #0x2
ADD r0  ,r0, R2
ldr r1, =$0x100		;-0x100
sub	r0, r0, r1
LDR r1, [r0, #0x0]

LDRB r2, [r4, #0x0]   ;*text (マルチバイトの1バイト目) レジスタが足りないので再度読み
ADD r4, #0x2          ;text+=2 

MULTIBYTE_FINDER_LOOP
CMP r1, #0x0
BEQ EXIT    ;ない

LDRB r0,[r1,#0x4]
CMP r2,r0
BEQ EXIT    ;発見!
LDR r1,[r1,#0x0]
b MULTIBYTE_FINDER_LOOP



SINGLE_BYTE
LDRB r1, [r4, #0x0]
ADD r4, #0x1			;text++

LSL r0  ,r1 #0x2
ADD r0  ,r0, R2
LDR r1, [r0, #0x0]



EXIT
;r1 = fontdata ない場合は #0x00
;r3
;r4 = 次のtext
;r5 = fontbase
ldr r0,=$08005BB2
mov pc,r0
