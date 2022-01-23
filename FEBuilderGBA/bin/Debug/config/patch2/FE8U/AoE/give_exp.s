@経験値の取得
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
	push	{r4,r5,r6,lr}     @ Event parameter in memory slot 0x1, EXP to grant in slot 0x4. Keep the unit in r4 and the EXP to give in r6.
	mov  r5, r0               @Current Procs
	
@	ldr r0, =0x030004B0     @ gMemorySlot.	@{J}
	ldr r0, =0x030004B8     @ gMemorySlot.	@{U}
	ldr r6, [ r0, #4*0x04 ]   @ Get EXP from memory slot 0x4.
	mov r1, #0x04
    ldsh r0, [ r0, r1 ]  @ Get the event parameter from slot 0x1.
@    blh 0x0800BF3C           @ GetUnitStructFromEventParameter	@{J}
    blh 0x800BC50           @ GetUnitStructFromEventParameter	@{U}
    mov r4, r0	
	
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
	mov r1, r4 @ unit 
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
	push	{r4-r5,lr}
	mov     r4,r0
	mov r5, r1 @ unit 
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


	ldr	r0, give_exp_proc
@		mov	r1, #0x3                     @これではダメ
@		blh     0x08002bcc     @New6C    @これではダメ
	mov	r1, r4
@	blh	0x08002C30       @NewBlocking6C	{J}
	blh	0x08002ce0       @NewBlocking6C	{U}

@mov r0, r5 @ unit 
bl HideMMSFunc

	pop	{r4-r5,pc}
	
	
@.equ CurrentUnit,                0x03004DF0	@{J}
.equ CurrentUnit,                0x03004E50	@{U}
@.equ ProcFind, 0x8002dec	@{J}
.equ ProcFind, 0x8002E9C	@{U}
@.equ gProc_MoveUnit, 0x8A132D0	@{J}
.equ gProc_MoveUnit, 0x89A2C48	@{U}
.ltorg
.align 
.type HideMMSFunc, %function 
HideMMSFunc:
	@ Arguments: nothing 
	@ Returns:   nothing
push {lr} 
@ even if giving exp to a unit that is not the active unit, we only want to show sms for the active unit 
@blh 0x0807B4B8 @ ClearMOVEUNITs	@{J}
blh 0x080790a4 @ ClearMOVEUNITs	@{U}

ldr r0, =gProc_MoveUnit
blh ProcFind 
cmp r0, #0 
beq SkipHidingInProc
add r0, #0x40 @this is what MU_Hide does @MU_Hide, 0x80797D5
mov r1, #1 
strb r1, [r0] @ store back 0 to show active MMS again aka @MU_Show, 0x80797DD

SkipHidingInProc: 
ldr r3, =CurrentUnit 
ldr r3, [r3] 
cmp r3, #0 
beq Exit 
ldr r1, [r3, #0x0C] @ Unit state 
mov r2, #1 @ Hide 
bic r1, r2 @ Show SMS 
str r1, [r3, #0x0C] 

Exit:
@blh 0x08019C78   @UpdateUnitMapAndVision	@{J}
@blh 0x08019E78   @UpdateTrapHiddenStates	@{J}
@blh  0x08027144   @SMS_UpdateFromGameData	@{J}
@blh  0x08019914   @UpdateGameTilesGraphics	@{J}

blh 0x08019FA0   @UpdateUnitMapAndVision	@{U}
blh 0x0801A1A0   @UpdateTrapHiddenStates	@{U}
blh  0x080271a0   @SMS_UpdateFromGameData	@{U}
blh  0x08019c3c   @UpdateGameTilesGraphics	@{U}

pop {r0}
bx r0

.ltorg 
.align 4

give_exp_proc:
