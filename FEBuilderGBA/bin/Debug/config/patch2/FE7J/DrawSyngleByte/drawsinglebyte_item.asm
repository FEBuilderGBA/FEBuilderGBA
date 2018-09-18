@thumb
;フォント 英文 0x40 + アルファベット　としてマップする.
;080055fe から呼び出し

;r4 text
ldr r5,=$02028d68

ldrb	r3, [r4, #0x0]
cmp	r3, #0x1f		;1fより小さいコントロールコード
BLT SKIP_CODE

CMP r3,#0x81          ;念のためSJIS2バイト目チェック
BLT SINGLE_BYTE
ldrb	r2, [r4, #0x1]
CMP r2,#0x40
BLT SINGLE_BYTE
add	r4, #0x2
B FIND_MULTIBYTE

SKIP_CODE
	add	r4, #0x1
	b EXIT

SINGLE_BYTE
;   mov r3,r3
	mov	r2, #0x40
	add	r4, #0x1

FIND_MULTIBYTE
lsl	r0, r2, #0x2	;2倍したポインタ化

ldr	r2, [r5, #0]
ldr	r2, [r2, #4]
add r0, r0, r2		;font baseを加算.

ldr r2, =$0x100		;-0x100
sub	r0, r0, r2  

cmp	r0, #0
beq	NOT_FOUND
ldr	r0, [r0]

FIND_MULTIBYTE_LOOP
ldrb	r1, [r0, #0x4]	;SJIS2バイト目
cmp	r1, r3				;探しているフォントかどうか
beq	EXIT

ldr	r0, [r0, #0x0]
cmp	r0, #0
bne	FIND_MULTIBYTE_LOOP

NOT_FOUND
mov	r3, #0x81 ;//ハート
mov	r2, #0xa7
b	FIND_MULTIBYTE


EXIT
;;すべて終了
mov r1,r0

ldr r0,=$0800563a
mov pc ,r0
