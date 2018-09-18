; 080160d0 からこのコードへ飛ばす.(FE8J)
; 080160C4 からこのコードへ飛ばす.(FE8U)

@thumb
	cmp  r0, #0x10		;BGM:勝利近し
	bne  exit			;違うなら終了
	
	                    ;マップ設定から取得
;	ldr  r0, =$0202BCEC		;(FE8J)
	ldr  r0, =$0202BCF0		;(FE8U)

	ldrb r0, [r0, #14]
	
	;//マップ番号から、マップ設定のアドレスを返す関数 r0:マップ設定のアドレス r0:調べたいマップID:MAPCHAPTER
;	ldr  r1, =$08034520		;(FE8J)
	ldr  r1, =$08034618		;(FE8U)

	mov  lr, r1
	@dcw $F800
	add  r0, #0x28
	ldrh r0, [r0]
exit
;	ldr r1,=$08016fdc		;(FE8J)
	ldr r1,=$08017234		;(FE8U)

	mov pc,r1
