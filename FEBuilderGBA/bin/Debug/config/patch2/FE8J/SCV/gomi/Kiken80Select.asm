@thumb
;@org	0808f090 > 00 49 8F 46 20 B0 E4 08

	mov	r7, r0
	ldrb	r0, [r7 #11]
	lsr	r0, r0, #6
	beq	end	;味方はジャンプ
	ldr	r0, =$085775cc
	ldr	r0, [r0, #0]
	ldrh	r1, [r0, #8]
	mov	r0, #4
	and	r0, r1
	cmp	r0, #0
	beq	end	;セレクトチェック
;無操作時間
;	ldr	r0, =$02024CD2
;	ldrb	r0, [r0]
;	cmp	r0, #0x10
;	bls	end
;無操作時間2
;	mov	r0, #0x44
;	ldrb	r0, [r5, r0]
;	cmp	r0, #0x10
;	bls	end
	ldrb	r0, [r7 #28]
	mov	r1, #0x80
	eor	r0, r1
	strb	r0, [r7 #28]
;音を鳴らす
	ldr r0,=$0202bcec
	add r0,#0x41
	ldrb r0,[r0]
	lsl r0,r0,#30
	bmi end
	mov r0,#0x6a
	ldr r2,=$080d4ef4	;効果音を鳴らす
	mov lr, r2
	@dcw	$F800
end
	mov	r4, r5
	add	r4, #68
	ldrh	r0, [r4, #0]
	ldr	r1, =$0808f098
	mov	pc, r1