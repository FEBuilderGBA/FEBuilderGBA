;
;支援がついているか調べる
;
;@org	$08E4FDD0
@thumb
	push	{r4,r5, lr}

	;操作キャラ
;	ldr		r0, =$03004df0    ;操作キャラのワークメモリへのポインタ FE8J
	ldr		r0, =$03004E50    ;操作キャラのワークメモリへのポインタ FE8U
	ldr		r5, [r0]
	mov		r4, #0x32		;支援1一人目

shien_loop
	ldrb	r0, [r5,r4]
	mov		r1,#0x51
	cmp		r1,r0	       ;支援Cは 0x51以降から
	ble		shien_ari      ;なぜかbge系の>=使えないので、条件反転させて blt/bleで代用

next_shien
	add		r4,#0x1
	cmp		r4,#0x38
	ble		shien_loop

	mov		r0,#0x00
	b		exit_return

shien_ari
	mov		r0,#0x01

exit_return
;	ldr	r2, =$030004B0  ; + #0x30  FE8J
	ldr	r2, =$030004B8	; + #0x30  FE8U
	str	r0, [r2, #0x30]	;条件式で取れる領域に書き込む
						;400CXX00 0C000000	未達成ならジャンプ  / ;410CXX00 0C000000	達成ならジャンプ

	mov	r0, #0

	pop	{pc,r5 , r4  }
