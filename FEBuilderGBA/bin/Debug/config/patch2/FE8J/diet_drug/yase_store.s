.thumb
@0x2F7B0 からか、ここに飛んで来るコードをかかないとダメです。
@
	ldrb r1, [r4, #0x1a]
	add  r0 ,r0, r1
	strb r0, [r4, #0x1a]

	ldrb r0, [r4, #0x1a]
	cmp     r0,#0x80		@マイナスに突っ込んだ場合
	blt     Skip
	mov     r0,#0x0     @マイナスにはできないので0に補正する
	strb r0, [r4, #0x1a]

Skip:

	mov r0 ,r4

Exit:
ldr r1,=0x0802F7B8
mov pc,r1
