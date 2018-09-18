@thumb
;@org	$08027658 > 00 48 87 46 D8 03 E9 08
;@org	$085C6E08
;@incbin	data.bin


;@org	$085C6EB0
	push    {r4 r5 r6 r7}
;ldr     r0,=202BD31h
;ldrb    r0,[r0]
;mov     r1,#0x80
;and     r0,r1
;cmp     r0,#0
;bne     NoBar
	mov     r1,#0x10
	ldsb    r1,[r4,r1]	;get x coord
	lsl     r1,r1,#4
@align 4
	ldr     r2, [CameraPos]
	mov     r3,#0xC
	ldsh    r0,[r2,r3]	;camera x?
	sub     r3,r1,r0
	mov     r0,#0x11	;get y coord
	ldsb    r0,[r4,r0]
	lsl     r0,r0,#4
	mov     r5,#0xE
	ldsh    r1,[r2,r5]	;camera y?
	sub     r2,r0,r1
	mov     r1,r3
	add     r1,#0x10
	mov     r0,#0x80
	lsl     r0,r0,#1
	lsl     r6,r7,#0x18
	cmp     r1,r0
	bhi     NoBar
	mov     r0,r2
	add     r0,#0x10
	cmp     r0,#0xB0
	bhi     NoBar


	ldr     r1,=$0201	;Xvalue
	add     r0,r3,r1
	sub     r1,#2
	and     r0,r1
	mov     r1,r2
	add     r1,#0xFB
	mov     r2,#0xFF
	and     r1,r2
@align 4
	ldr     r3,[adr]	;[FramePointers]
FindHP:
	push    {r0 r1 r2 r3}
	mov     r0,#0x12
	ldrb    r0,[r4,r0]	;max
	mov     r1,#0x13
	ldrb    r1,[r4,r1]	;curr
	cmp     r0,r1
	bgt     NotFull
	mov     r5,#0
	pop     {r0 r1 r2 r3}
	b NoBar
NotFull:
	cmp     r1,#0
	bne     NotEmpty
;	mov     r5,#0xE
	pop     {r0 r1 r2 r3}
	b NoBar
NotEmpty:
sub r2,r0,r1 ;@r2 = damage taken
mov r1,r0
mov r0,#11  ;@11 images
mul r0,r2
@dcw	$DF06	;	swi     #6

	mov	r5, r0
HPFound:
	pop     {r0 r1 r2 r3}
	lsl     r2,r5,#2
	add     r2,r2,r3
	ldr     r2,[r2]
@align 4
ldr     r3, [WRAMDisplay]
mov     r14,r3
	mov     r3,#0
@dcw	$F800

NoBar:
EffectivenessWarning:
	b	MarkingCheck		;動作が重いのでとりまスキップ

align 4
ldr r0, [ActiveUnit]
ldr r6,[r0] ;@r6 contains active units data
ldrb r0,[r6,#0xC] ;@status byte
mov r1,#1
tst r0,r1 ;@AND r0,r1
beq NotSelected ;@the active unit isn't selected

;@make sure active unit is ally:
ldrb r0,[r6,#0xB]
mov r1, #0x80
tst r0,r1
bne NoEffectiveness

ldrb r0,[r4,#0xB] ;@deployment #
mov r1,#0x80
tst r0,r1
beq NoEffectiveness ;@if zero flag is set, not an enemy unit so skip
mov r5, r4
add r5, #0x1e                             ;@start of inventory
mov r7, #0                                ;@loop counter
LoopThroughItems:
ldrh r0,[r5]                              ;@item #
cmp r0,#0                                 ;@no more items?
beq NoEffectiveness

CheckEquippable:
  mov r0,r4 ;@current unit data
  ldrh r1,[r5] ;@current unit's weapon
@align 4
  ldr r2, [CanEquip]
  mov lr,r2
@dcw 0xf800
  cmp r0,#1
  bne KeepLooping                         ;@if can't equip, skip to next item
    ldrh r0,[r5]
    mov r1, r6                            ;@r0 is item, r1 is target
@align 4
    ldr r2, [CheckEffectiveness]
    mov lr,r2
@dcw 0xf800
    cmp r0,#1
    beq IsEffective
      mov r0,r4 ;@current unit = attacker  @if not effective, check for Slayer
      mov r1,r6 ;@active unit = defender
@align 4
      ldr r2, [SlayerCheck]
      mov lr, r2
@dcw 0xf800
      cmp r0,#1
      beq IsEffective
        ldrb r0,[r5]                      ;@if neither, check crit rate
@align 4
        ldr r1, [GetItemData]
        mov lr,r1
@dcw 0xf800
        ldrb r0,[r0,#0x18]
        cmp r0, #24
        ble KeepLooping                   ;@0-24 no warning
        cmp r0, #0xFF
        beq KeepLooping                   ;@0xFF means cannot crit
      IsDangerous:
        mov r0,#8
        bl DrawWarningSign
        b NoEffectiveness
      IsEffective:                        ;@Draw Red warning
        mov r0,#0
        bl DrawWarningSign
        b NoEffectiveness
KeepLooping:
cmp r7, #5 ;@last item?
bge NoEffectiveness
add r7,#1
b LoopThroughItems
	
	
	
	
NoEffectiveness:
NotSelected:
	
MarkingCheck:
	ldrb	r0, [r4, #11]
	lsr	r0, r0, #6
	beq	nonArrow	;敵以外なら無視
	mov	r0, #0x3B
	ldrb	r0, [r4, r0]
	lsl	r0, r0, #24
	bpl	nonArrow	;ビット立ってなければ無視
	mov     r1,#0x10
	ldsb    r1,[r4,r1]
	lsl     r1,r1,#4
@align 4
	ldr     r2, [CameraPos]
	mov     r3,#0xC
	ldsh    r0,[r2,r3]
	sub     r3,r1,r0
	mov     r0,#0x11
	ldsb    r0,[r4,r0]
	lsl     r0,r0,#4
	mov     r5,#0xE
	ldsh    r1,[r2,r5]
	sub     r2,r0,r1
	mov     r1,r3
	add     r1,#0x10
	mov     r0,#0x80
	lsl     r0,r0,#1
	lsl     r6,r7,#0x18
	cmp     r1,r0
	bhi     nonArrow
	mov     r0,r2
	add     r0,#0x10
	cmp     r0,#0xB0
	bhi     nonArrow
	
	ldr     r1,=$0201	;Xvalue
	add     r0,r3,r1
	sub     r1,#2
	and     r0,r1
	mov     r1,r2
	add     r1,#0xFB
	mov     r2,#0xFF
	and     r1,r2
@align 4
	mov r2, #(here - LoadHereData - 4)
LoadHereData:
	mov	r3, pc
	add	r2, r2, r3
	mov     r3,#0
@align 4
ldr     r4, [WRAMDisplay] 
mov     r14,r4
@dcw	$F800
	
nonArrow:
	pop     {r4 r5 r6 r7}
	
;ここからオリジナルの処理
	mov     r0,r4
	add     r0,#0x30
	ldrb    r0,[r0]
	lsl     r0,r0,#0x1C
	lsr     r0,r0,#0x1C
@align 4
	ldr     r1, [Return_to];=$08027660
	mov	pc, r1
here:
@dcd	$00030001
@dcd	$086F0004

DrawWarningSign:
push {r4-r7,r14}
mov r5,r0       ;@r5 is either 0 or 8 depending on effective/killer (or 0x10 for talk)
mov r1, #0x10
ldsb r1,[r4,r1] ;@get x coord
lsl r1,r1,#4
@align 4
ldr r2,[CameraPos]
mov r3,#0xc
ldsh r0,[r2,r3] ;@camera x?
sub r3,r1,r0
mov r0,#0x11 ;@get y coord
ldsb r0,[r4,r0]
lsl r0,r0,#4
mov r1,#0xe
ldsh r1,[r2,r1] ;@camera y?
sub r2,r0,r1
mov r1,r3
add r1,#0x10
mov r0,#0x80
lsl r0,r0,#1
lsl r6,r7,#0x18
cmp r1,r0
bhi NoSign
mov r0,r2
add r0,#0x10
cmp r0,#0xb0
bhi NoSign
ldr r1,=$0201	;Xvalue
add r0,r3,r1
add r0,#0xB ;@tweak for X coord
sub r1,#2
and r0,r1
mov r1,r2
add r1,#0xee  ;@y coord
mov r2,#0xff
and r1,r2
@align 4
ldr r2, [adr+4]
add r2, r2, r5    ;@loads up either effective or killer depending on r5
mov r3,#0
@align 4
ldr r4, [WRAMDisplay]
mov lr,r4
@dcw 0xf800

NoSign:
pop {r4-r7}
pop {r0}
bx r0

@ltorg
CameraPos:
@dcd	$0202BCAC	;(202BCB0)
WRAMDisplay:
@dcd	$08002B08	;(8002bb8)
Return_to:
@dcd	$08027660	;(80276BFh)
ActiveUnit:
@dcd	$03004df0	;.long 0x3004e50
SlayerCheck:
@dcd	$08016a30	;.long 0x8016c88
CheckEffectiveness:
@dcd	$08016994	;.long 0x8016bec
CanEquip:
@dcd	$0801631c	;.long 0x8016574
GetItemData:
@dcd	$08017558;.long 0x80177B0
adr:


