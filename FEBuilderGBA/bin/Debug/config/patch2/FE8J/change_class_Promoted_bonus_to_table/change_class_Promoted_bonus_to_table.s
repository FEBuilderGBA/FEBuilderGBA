.thumb

Class_Bonus_Start = (adr)

@org	0x0802C294

ldr	r2, [r0, #4]
push	{r7}
mov	r7, r0
ldr	r0, [r1, #0x28]	@ユニット特性2
ldr	r1, [r2, #0x28]	@クラス特性2
orr	r0, r1
mov	r1, #0x80
lsl	r1, r1, #1
and	r0 ,r1		@上級クラス判定

push	{r0}
mov	r0, #0

Loop:
lsl	r1, r0, #2	@*4
ldr	r2, Class_Bonus_Start
ldrb	r2, [r2, r1]	@補正値変更クラスID
cmp	r2, #0
beq	Normal		@補正値変更クラスID:00なら通常処理に進み終了
ldr	r1, [r7, #4]
ldrb	r1, [r1, #4]	@クラスID
cmp	r1, r2
beq	Unit
add	r0, #1
b	Loop

Unit:
lsl	r1, r0, #2	@*4
add	r1, #1
ldr	r2, Class_Bonus_Start
ldrb	r2, [r2, r1]	@補正値変更ユニットID
cmp	r2, #0
beq	GetValue
ldr	r1, [r7, #0]
ldrb	r1, [r1, #4]	@ユニットID
cmp	r1, r2
beq	GetValue
add	r0, #1
b	Loop

GetValue:
lsl	r1, r0, #2	@*4
add	r1, #2
ldr 	r2, Class_Bonus_Start
ldrb	r1, [r2, r1]	@補正値
add	r3, r1
pop	{r0}
b	end

Normal:
pop	{r0}
cmp	r0, #0x0	@下級なら分岐
beq	end
add	r3, #20		@上級なら＋20

end:
pop	{r7}
ldr	r1, =0x0802c2a8
mov	pc, r1

.ltorg
.align
adr: