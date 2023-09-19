;現在のターン数を強制的に書き込みます。
@org	$08E4FAC0
@thumb

push	{lr}

;引数 r0
ldr		r0,=$0202BCF0
;ターン数書込関数呼び出し
ldr		r1,=$080A4350      ;RegisterChapterTimeAndTurnCount
mov		lr, r1
@dcw	$F800

;二重に書き込まないように、03フラグを落とす
mov	r0, #0x3
ldr	r1, =$08083DA8         ;フラグ状態確認関数 RET=結果BOOL r0=確認するフラグ:FLAG
mov	r14, r1
@dcw	$F800

mov	r0, #0
pop	{pc }
