;
;ついている支援をクリアする
;
;@org	$08E4FE30
@thumb
	push	{r4,r5,r6,r7, lr}

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

	b		exit_return

shien_ari
	mov		r0,#0x50
	strb	r0, [r5,r4]		; 支援Cは 0x51からなので、直前の 0x50にする

	mov		r0,#0x00
	mov		r1,#0x39
	strb	r0, [r5,r1]	; 支援フラグを消す

	;
	;自分だけではなく、相手の支援も消さないといけない.
	;
	;まず自分の情報を見る
	ldr		r0, [r5,#0x00]			;キャラクター情報へ
	ldrb	r6, [r0,#0x04]          ;自分のキャラID
	ldr		r0, [r0,#0x2c]			;自分の支援情報

	mov		r1,#0x32
	sub		r2, r4 , r1				;現在消そうとしている支援のキャラIDを調べる
	ldrb	r7 ,[r0,r2]             ;支援相手のキャラID=支援情報[支援番号]

	;支援相手キャラIDからそのキャラのワークメモリを特定する
;	ldr		r3, =$0202BE48	;FE8J
	ldr		r3, =$0202BE4C	;FE8U
character_loop
	ldr		r0,[r3]

	mov		r1,#0x00
	cmp		r0,r1
	beq		exit_return

	ldrb	r1, [r0,#0x04]          ;相手のキャラID
	cmp		r7, r1					;探していた相手のキャラIDと同じか？
	bne		character_next

	;探していた相手のキャラIDっぽいので、自分のキャラIDは、支援データの何番目かを調べる
	ldr		r0, [r0,#0x2c]				;自分の支援情報
	
	mov		r1, #0x00;
find_shien_loop
	ldrb	r2,[r0,r1]
	cmp		r2,r6
	beq		found_data

	add		r1,#0x01
	cmp		r1,#0x07
	ble		find_shien_loop

	;相手っぽいキャラIDなのに支援データを持っていない場合は、
	;仕方ないので、次のキャラクターを調べる

character_next
	add		r3, #0x48	;次の人へ
	b		character_loop

found_data
	;r1 消す支援の番目
	;r3 消す支援をもっているキャラクタのワークメモリ
	
	add		r1,#0x32
	mov		r0,#0x50
	strb	r0, [r3,r1]		; 支援Cは 0x51からなので、直前の 0x50にする

	mov		r0,#0x0
	mov		r1,#0x39
	strb	r0, [r3,r1]	; 支援フラグを消す
	
	b		next_shien

exit_return
	mov	r0, #0

	pop	{pc,r7,r6, r5 , r4  }
