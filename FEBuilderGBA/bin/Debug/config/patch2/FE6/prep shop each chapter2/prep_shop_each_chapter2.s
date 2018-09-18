@編成準備店の売り物を章単位で切り替える
@Call 08095EF0	FE6

@r7 nazo struct
@

.thumb
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


ldr r0, [r7, #0x4]
cmp r0, #0x0
beq PrepShop
	ldr r0, [r7, #0x4]    @blank
	str r0, [r7, #0x14]
	b   Join

PrepShop:
	ldr  r1,PrepShop_Table

	ldr  r2, =0x0202AA48 @FE6 @ChapterData
	ldrb r2,[r2,#0xE]    @ChapterData->MAPID

	lsl  r2,#0x2         @ *4してポインタ化
	add  r1,r2
	ldr  r1,[r1]
	cmp  r1,#0x00
	bne Next1
		ldr r1, =0x08095f10 @ FE6 ディフォルト
		ldr r1, [r1]
Next1:
	str r1, [r7, #0x14]

Join:
	ldr r3, =0x08095F04+1
	bx  r3

.ltorg
PrepShop_Table:
