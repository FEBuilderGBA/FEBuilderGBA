@経験値の取得
@
@40 0D [UNIT] [ADDEXP] [ASM+1]
@41 0D 00 [ADDEXP] [ASM+1]  (Load SVAL1 ID)
@4B 0D 00 [ADDEXP] [ASM+1]  (Load SVALB coord)
@4F 0D 00 [ADDEXP] [ASM+1]  (Current)
@
@Author 7743   Effect Logic: aera
@
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
	push	{r4,r5,r6,lr}

	mov  r5, r0               @Current Procs
	ldr  r4, [r0, #0x38]      @イベント命令にアクセスらしい [r0,#0x38] でイベント命令が書いてあるアドレスの場所へ

	ldrb r0, [r4, #0x0]       @引数0 [FLAG]
	ldrb r6, [r4, #0x3]       @引数2  give exp

	mov  r1,#0xf
	and  r0,r1

	cmp  r0,#0x1
	beq  GetUnitInfoBySVAL1
	cmp  r0,#0xB
	beq  GetUnitInfoByCoord
	cmp  r0,#0xF
	beq  GetUnitInfoByCurrent

	ldrb r0, [r4, #0x2]       @引数1 [UNIT]
	b   GetUnitInfo

GetUnitInfoByCurrent:
@	ldr  r0,=#0x03004DF0      @操作中のユニット {J}
	ldr  r0,=#0x03004E50      @操作中のユニット {U}
	ldr  r0,[r0]
	b   CheckUnitInfo

GetUnitInfoBySVAL1:
	ldr  r0,=#0xFFFFFFFF
	b   GetUnitInfo

GetUnitInfoByCoord:
	ldr  r0,=#0xFFFFFFFE
	@b   GetUnitInfo

GetUnitInfo:
@	blh  0x0800bf3c           @UNITIDの解決 GetUnitStructFromEventParameter	{J}
	blh  0x0800bc50           @UNITIDの解決 GetUnitStructFromEventParameter	{U}
	                          @RAM UNIT POINTERの取得
CheckUnitInfo:
	cmp  r0,#0x00
	beq  Term                 @取得できなかったら終了


Change:
	mov  r4, r0            @ram unit pointer

@	ldr	r0, =0x0203a4e8    @Unit@戦闘アニメで右側。 {J}
	ldr	r0, =0x0203A4EC    @Unit@戦闘アニメで右側。 {U}
	mov	r1, r4             @Unit
@	blh	0x0802a4f0         @CopyUnitToBattleStruct	{J}
	blh	0x0802a584         @CopyUnitToBattleStruct	{U}

	mov	r0, r5         @Current Procs
	mov r1, r6         @GetExp
	bl	nin_i_exp

@@	mov	r0, r5         @Current Procs これではなく、
@@	nin_i_expの戻り値 Procs	Exp Bar Graph の Procsの子としてeffectを登録する必要がある。
@@	mov	r0, r0
	bl	effect
	
	bl	clear_double_battleanime
Term:
	mov r0 ,#0x0
	pop {r4,r5,r6}
	pop	{r1}
	bx	r1

@二重に描画されるマップアニメを消す
@このルーチンはかなりイケていない。 
@ただ、最初に見つかったアニメは二重に描画されるアニメである可能性が高いので、それをとりあえず消す.
clear_double_battleanime:
	push	{lr}

@	ldr r0, =0x08A132D0	@(Procs MoveUnit )	{J}
	ldr r0, =0x089A2C48	@(Procs MoveUnit )	{U}

@	blh	0x08002dec	@Find6C	{J}
	blh	0x08002e9c	@Find6C	{U}

@	blh	0x08002cbc	@Delete6C	{J}
	blh	0x08002d6c	@Delete6C	{U}

	pop	{r0}
	bx	r0

@任意の経験値
nin_i_exp:
	push	{r4,r5,r6,lr}
	mov		r5, r0
	mov		r6, r1

@	ldr	r4, =0x0203a4e8    @Unit@戦闘アニメで右側。 {J}
	ldr	r4, =0x0203A4EC    @Unit@戦闘アニメで右側。 {U}
	mov	r0, #0xB
	ldsb	r0, [r4, r0]
	mov	r1, #0xC0
	and	r0, r1
	cmp	r0, #0
	bne	noexp

	mov	r0, r4
@	blh	0x0802B93C          @CanUnitNotLevelUp	{J}
	blh	0x0802b9f4          @CanUnitNotLevelUp	{U}

	lsl	r0, r0, #0x18
	cmp	r0, #0
	beq	noexp

@	ldr	r0, =0x0202bcec     @(gChapterData ) {J}
	ldr	r0, =0x0202BCF0     @(gChapterData ) {U}
	ldrb	r1, [r0, #0x14]
	mov	r0, #0x80
	and	r0, r1
	cmp	r0, #0
	bne	noexp

@	ldr	r2, =0x0203a4e8    @Unit@戦闘アニメで右側。 {J}
	ldr	r2, =0x0203A4EC    @Unit@戦闘アニメで右側。 {U}
	mov r1, r2
	add	r1, #0x6E
	mov	r0, r6				@経験値を追加
	strb	r0, [r1, #0x0]
	ldrb	r0, [r2, #0x9]
	add	r0, r6				@やはりここも置き換えないとダメかなあ 
	strb	r0, [r2, #0x9]
	mov	r0, r2

@	blh 0x0802B970          @CheckForLevelUp	{J}
	blh 0x0802ba28          @CheckForLevelUp	{U}
	noexp:

@	ldr r0,=0x085C3FA4       @Procs	Exp Bar Graph {J}
	ldr r0,=0x0859BAC4       @Procs	Exp Bar Graph {U}

	mov	r1, r5
@	blh	0x08002C30       @NewBlocking6C	{J}
	blh	0x08002ce0       @NewBlocking6C	{U}

	pop	{r4,r5,r6}
	pop	{r1}
	bx	r1

effect:
	push	{r4,lr}
	mov     r4,r0
	                       @フォントを初期化しないと、PAL2が使われることがあるらしい。Thanks stan
@	blh 0x08003bc4         @Font_InitForUIDefault / Font_InitDefault {J}
	blh 0x08003C94         @Font_InitForUIDefault / Font_InitDefault {U}

@	ldr	r0, =0x0203a4e8    @Unit@戦闘アニメで右側。 {J}
	ldr	r0, =0x0203A4EC    @Unit@戦闘アニメで右側。 {U}
	mov	r2, r0
	add	r2, #0x4A
	mov	r3, #0
	mov	r1, #0x4F
	strh	r1, [r2, #0]
@	ldr	r1, =0x0203e1ec      @(gMapAnimStruct ) {J}
	ldr	r1, =0x0203E1F0      @(gMapAnimStruct ) {U}
	mov	r12, r1
	add	r1, #0x5F
	strb	r3, [r1, #0]
	mov	r2, r12
	add	r2, #0x62
	mov	r1, #2
	strb	r1, [r2, #0]
	mov	r1, r12
	add	r1, #0x5E
	mov	r2, #1
	strb	r2, [r1, #0]
	sub	r1, #6
	strb	r3, [r1, #0]
	add	r1, #1
	strb	r2, [r1, #0]
@	ldr	r1, =0x0203a568    @(Unit@戦闘アニメで左側。敵軍のユニットデータのコピー.ROMユニットポインタ RET=@UnitForm ){J}
	ldr	r1, =0x0203A56C    @(Unit@戦闘アニメで左側。敵軍のユニットデータのコピー.ROMユニットポインタ RET=@UnitForm ){U}

@	ldr	r2, =0x0203a5e8    @(gRoundArray ) r0=UnitForm	{J}
	ldr	r2, =0x0203A5EC    @(gRoundArray ) r0=UnitForm	{U}
	
@	blh     0x0807dc48     @SetupMapBattleAnim	{J}
	blh     0x0807b900     @SetupMapBattleAnim	{U}

	ldr	r0, event_procs
@		mov	r1, #0x3                     @これではダメ
@		blh     0x08002bcc     @New6C    @これではダメ
	mov	r1, r4
@	blh	0x08002C30       @NewBlocking6C	{J}
	blh	0x08002ce0       @NewBlocking6C	{U}

	pop	{r4,pc}

.ltorg
.align
event_procs:
