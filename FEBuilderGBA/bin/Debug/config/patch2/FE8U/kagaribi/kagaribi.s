@霧の中でも特定の座標に視界を確保します。
@
@Call 08019E3C (FE8J)
@Call 0801A164 (FE8U) <-
@
@関数末尾をフックするので全部使えます.
@r0 temp
@r1 temp
@r2 temp
@r3 temp
@r4 temp (ChapterData)
@r5 temp
@r6 temp (KAGARIBI_Table)
@r7 temp
@
@Author 7743
@
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@
@
.thumb
@	ldr  r4, =0x0202bcec @ @ChapterData	@{J}
	ldr  r4, =0x0202BCF0 @ @ChapterData	@{U}
	ldr  r6,KAGARIBI_Table

	sub r6,r6,#0x08
Loop:
	add	r6,r6,#0x08

	ldr  r0,[r6,#0x0]
    cmp  r0,#0x00
	beq  Term

	ldrb r0,[r6,#0x0]
	ldrb r1,[r4,#0xE] @ChapterData->MAPID
	cmp  r0,r1
	bne  Loop

	ldrh r0,[r6,#0x4]
	cmp  r0,#0x00
	beq  AddPoint

@	blh  0x080860D0   @CheckFlag {J}
	blh  0x08083DA8   @CheckFlag {U}
	cmp  r0,#0x00
	beq  Loop

AddPoint:
	ldrb r0, [r6, #0x1]
	ldrb r1, [r6, #0x2]
	ldrb r2, [r6, #0x3]
	mov r3, #0x1
@	blh 0x0801a798   @MapAddInRange {J}
	blh 0x0801aabc   @MapAddInRange {U}
	b   Loop

Term:
	pop {r4,r5,r6,r7}
	pop {r0}
	bx r0

.ltorg
KAGARIBI_Table:
