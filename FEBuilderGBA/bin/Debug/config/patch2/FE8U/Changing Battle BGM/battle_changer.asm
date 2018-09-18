@thumb
;@org	$08074ba6		;FE8J
@org	$080726CA		;FE8U

;	ldr	r6, [$08074bdc+4]  ;FE8J
	ldr	r6, [$08072700+4]  ;FE8U
	ldrb	r0, [r6, #0xE]

	;//マップ番号から、マップ設定のアドレスを返す関数 r0:マップ設定のアドレス r0:調べたいマップID:MAPCHAPTER
;	bl	$08034520          ;FE8J
	bl	$08034618          ;FE8U
	ldrb	r1, [r6, #0xF]
	add	r0, #28
	lsl	r2, r1, #25
	bmi	$08074be0          ;FE8J
	bmi	$08072704          ;FE8U
	ldrh	r6, [r0, #2]
	lsl	r1, r1, #24
	bmi	$08074bbe          ;FE8J
	bmi	$080726E2          ;FE8U
	ldrh	r6, [r0, #0]
	
	
;@org	$08074bdc     ;FE8J
@org	$08072700     ;FE8U
;@dcd	$0202bcec     ;FE8J  ;現在のマップ
@dcd	$0202BCF0     ;FE8U

	ldrh	r6, [r0, #4]
	nop
