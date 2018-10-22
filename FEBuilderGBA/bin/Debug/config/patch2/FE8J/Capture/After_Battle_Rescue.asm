;bl'd to from 328F0(JP=$0803283c)
;r4=defender, r6=attacker battle structs
;r7 should contain char data ptr of person dropping item, 0 if capturing; r5 has char data of receiver


@thumb
	ldrb	r5,[r6,#0xB]		;attacker allegiance(攻め側の所属)
	lsl	r0, r5, #24
	lsr	r0, r0, #30
	beq	CheckStart
;
;オリジナルの処理
;
	mov	r0, #19
	ldsb	r0, [r6, r0]	;現在HP？
	cmp	r0, #0
	bne	Jump
	ldr	r0, =$08032844
	mov	pc, r0
Jump:
	ldr	r0, =$08032858
	mov	pc, r0
;
;処理スタート
;
CheckStart:
	ldrb	r7,[r4,#0xB]		;defender allegiance(受け側の所属)(待ち伏せであろうと正しい)
@align 4
ldr		r0,[Get_Char_Data]
mov		r14,r0
mov		r0,r5
@dcw	0xF800
	mov		r5,r0
	ldrb	r0,[r4,#0x13]		;defender current hp
	cmp		r0,#0x0
	beq		Defchar
	ldrb	r0,[r6,#0x13]		;attacker current hp
	cmp		r0,#0x0
	beq		SwitchCharacters
Non:
	mov		r7,#0x0
	b		GoBack				;if neither party is dead, skip this business
SwitchCharacters:
	ldrb	r7,[r6,#0xB]
	ldrb	r5,[r4,#0xB]
@align 4
ldr		r0,[Get_Char_Data]
mov		r14,r0
mov		r0,r5
@dcw	0xF800
mov		r5,r0

@align 4
Defchar:
ldr		r0,[Get_Char_Data]
mov		r14,r0
mov		r0,r7
@dcw	0xF800
	mov		r7,r0
	@align 4
	ldr		r0,[Is_Capture_Set]
	mov		r14,r0
	mov		r0,r5
	@dcw	0xF800
	cmp		r0,#0x0
	beq		Return		;攻撃者が捕獲攻撃でないならばジャンプ
;騎馬判定
	ldr	r1, [r7, #4]
	ldr	r1, [r1, #40]
	lsl	r0, r1, #31
	bmi	Reset
;輸送体判定
	ldr	r1, [r7]
	ldr	r1, [r1, #40]
	ldr	r0, [r7, #4]
	ldr	r0, [r0, #40]
	orr	r1, r0
	lsl	r0, r1, #22
	bmi	Reset
	
	ldr	r0, =$08018030	;救出判定
	mov	lr, r0
	mov	r0, r5
	mov	r1, r7
@dcw	0xF800
	lsl	r0, r0, #24
	cmp	r0, #0
	beq	Reset
;近接判定
	ldr	r0,	=$0203a4d2
	ldrb	r0, [r0]	;距離
	cmp	r0, #1
	bne	Reset
	mov	r0, #1
	strb	r0,[r7,#0x13]
	
@align 4
ldr		r0,[Write_Rescue_Data]
mov		r14,r0
mov		r0,r5
mov		r1,r7
@dcw	0xF800
	mov		r7,#0x0				;captured units don't drop anything
	ldr		r0,[r5,#0xC]
	b	Half

GoBack:

@align 4
ldr		r0,[Is_Capture_Set]
mov		r14,r0
mov		r0,r5
@dcw	0xF800
cmp		r0,#0x0
beq		Return
Reset:
	ldr		r0,[r5,#0xC]
	mov		r1,#0x10
	mvn		r1,r1
	and		r0,r1
Half:
	mov		r1,#0x80
	lsl		r1,r1,#0x17
	mvn		r1,r1
	and		r0,r1
str		r0,[r5,#0xC]		;remove the 'is capturing' bit

Return:
ldr	r0, =$08032874
mov	pc, r0

@ltorg
Get_Char_Data:
@dcd 0x08019108
Write_Rescue_Data:
@dcd 0x08018060
Is_Capture_Set:

