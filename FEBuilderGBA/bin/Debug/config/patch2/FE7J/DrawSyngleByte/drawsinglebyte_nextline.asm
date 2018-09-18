@thumb
;080055c8 から呼ばれる
;r0 text

LOOP
;1文字読み込む
ldrb	r1, [r0, #0x0]
cmp	r1, #0x1
ble	EXIT

cmp	r1, #0x4
bne	NEXT_CHAR
add	r0, r0,#0x1
b	LOOP

NEXT_CHAR
;;cmp	r1, #0x1f		;1fより小さいコントロールコード
;;BLT MULTIBYTE ;互換性維持のため、マルチバイト判定をしてみるか・？

CMP r1,#0x81          ;SJIS2バイト目チェック.
BLT SINGLE_BYTE
ldrb	r1, [r0, #0x1]
CMP r1,#0x40          ;SJIS1バイト目チェック.
BLT SINGLE_BYTE

;;マルチバイト.
;MULTIBYTE
add	r0,r0, #0x2
b LOOP

SINGLE_BYTE
add	r0,r0, #0x1
b LOOP


EXIT
;r0 == next text
ldr r1,=$080055da
mov pc,r1
