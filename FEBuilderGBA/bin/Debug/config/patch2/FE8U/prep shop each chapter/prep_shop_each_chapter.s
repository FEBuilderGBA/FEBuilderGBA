@編成準備店の売り物を章単位で切り替える
@Call 08099E48	FE8U

.thumb
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


push {r4,lr}
	mov r4 ,r0
	add r0, #0x2b
	ldrb r0, [r0, #0x0]
	blh 0x08095354    @FE8U
	                  @r0はこの関数の戻り値で固定されます
	                  @r1はワークメモリとして利用される

	@r1に、利用する編成準備店のデータを入れます
	@r2,r3はワークメモリに利用可能 r0は不可
	ldr  r1,PrepShop_Table

	ldr  r2, =0x0202BCF0 @FE8U @ChapterData
	ldrb r2,[r2,#0xE]    @ChapterData->MAPID

	lsl  r2,#0x2         @ *4してポインタ化
	add  r1,r2
	ldr  r1,[r1]
	cmp  r1,#0x00
	bne Next1
		ldr r1, =0x08A188E4  @FE8U ディフォルト値
Next1:
	mov r2 ,r4
	blh 0x080b41e0       @FE8U MakeShopArmory 
pop {pc,r4}

.ltorg
PrepShop_Table:
