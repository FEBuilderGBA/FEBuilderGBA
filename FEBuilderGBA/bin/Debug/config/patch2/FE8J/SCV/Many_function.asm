@thumb
@org	$0801B52A
	
	ldrb	r0, [r4, #0xC]
	lsl	r0, r0, #26
	lsr	r0, r0, #31
	bne	$0801B600	;被救出中ならジャンプ
	mov	r0, #66
	ldrb	r0, [r4, r0]
	cmp	r0, #6
	beq	$0801B600	;AIが0x6ならジャンプ
	nop
;杖持ち以外でもok効果つき

