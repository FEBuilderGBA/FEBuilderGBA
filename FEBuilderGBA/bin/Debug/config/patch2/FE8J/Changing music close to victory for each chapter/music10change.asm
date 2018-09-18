; 080160d0 からこのコードへ飛ばす.

@thumb
	cmp  r0, #0x10		;BGM:勝利近し
	bne  exit			;違うなら終了
	
	                    ;マップ設定から取得

	ldr  r0, =$0202BCEC
	ldrb r0, [r0, #14]
	ldr  r1, =$08034520
	mov  lr, r1
	@dcw $F800
	add  r0, #0x28
	ldrh r0, [r0]
exit
	ldr r1,=$08016fdc
	mov pc,r1
