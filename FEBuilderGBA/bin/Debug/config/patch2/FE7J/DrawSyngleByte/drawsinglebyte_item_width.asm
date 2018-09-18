@thumb

;フォント 英文 0x40 + アルファベット　としてマップする.
;080054ec から呼び出し

;r2 text
;r4 total width
;r5 font-base

ldrb	r3, [r2, #0x0]
cmp	r3, #0x1f		;1fより小さいコントロールコード
BLT SKIP_CODE

CMP r3,#0x81          ;念のためSJIS2バイト目チェック
BLT SINGLE_BYTE
ldrb	r1, [r2, #0x1]
CMP r1,#0x40
BLT SINGLE_BYTE
add	r2, #0x2
B FIND_MULTIBYTE

SKIP_CODE
	b EXIT

SINGLE_BYTE
;   mov r3,r3
	mov	r1, #0x40
	add	r2, #0x1

FIND_MULTIBYTE
lsl	r0, r1, #0x2	;2倍したポインタ化

ldr	r1, [r5, #0]
ldr	r1, [r1, #4]
add r0, r0, r1		;font baseを加算.

ldr r1, =$0x100		;-0x100
sub	r0, r0, r1  

cmp	r0, #0
beq	NOT_FOUND
ldr	r0, [r0]

FIND_MULTIBYTE_LOOP
ldrb	r1, [r0, #0x4]	;SJIS2バイト目
cmp	r1, r3				;探しているフォントかどうか
beq	STORE_EXIT

ldr	r0, [r0, #0x0]
cmp	r0, #0
bne	FIND_MULTIBYTE_LOOP

NOT_FOUND
mov	r3, #0x81 ;//ハート
mov	r1, #0xa7
b	FIND_MULTIBYTE

STORE_EXIT
ldrb	r1, [r0, #0x5]
add	r4, r4, r1	;width += size


EXIT
;;すべて終了
;r5 font-base
;r4 = total width
;r2 = text

ldr r0,=$08005514
mov pc,r0
