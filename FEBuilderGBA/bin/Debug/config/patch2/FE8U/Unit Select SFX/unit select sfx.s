@call 0x186B0
@r0 unk
@r1 unk
@r2 RAM Unit Pointer
@r3 work memory
@r4 unk
@r5 unk
@r6 unk
@
.thumb

@Record the flag that you played.used 16 bytes.
@再生したというフラグを記録する. 16バイト利用します.
.equ PLAYED_TURNHASH_RAM, 0x203B1FE
.equ PLAYED_FLAG_RAM, 0x203B1F0
@

@Execute corrupt code.
@壊してしまうコードを実行
add		r0  ,r0, r1
strb	r5, [r0, #0x0]

@Save the unit pointer of RAM as it is in r2.
@RAMのユニットポインタがr2にあるので保存する.
mov		r5,r2				@r5=RAM Unit Pointer (@Unit)
ldr		r6,=#0x202BCF0      @r6=Chaptor Pointer  (@ChapterData)

@check if player phase, if not end
@プレイヤーフェーズでなければ終了.
ldrb	r0,[r6,#0x0F]
cmp		r0,#0x00
bne		End

CheckTurn:
ldrh	r0,[r6,#0x10] @Get Turn
ldrb	r1,[r6,#0x0E] @Get MapID
lsl		r0,r0,#0x4   @ hash = (turn << 4) | mapID
orr		r0,r0,r1

ldr		r1,=PLAYED_TURNHASH_RAM
ldrh	r2,[r1]
cmp		r0,r2
beq		FindSound

@Write the current turns hash.
@現在のターンハッシュを書く
strh	r0,[r1]

@Clear only at turn 0x01.
@ターン0x01の時のみクリアする
ldrh	r0,[r6,#0x10] @Get Turn
cmp		r0,#0x01
bne		FindSound

ClearAll:
ldr		r1,=PLAYED_FLAG_RAM
mov		r0,#0x00		@clear 14 bytes.(0xD) 
str		r0,[r1]
str		r0,[r1,#0x4]
str		r0,[r1,#0x8]
strh	r0,[r1,#0xC]


@Search the list
@リストの探索
@
@struct data{
@short sound      //play sound if 0x00 term
@byte  unit		  //if 0x00 ANY
@byte  class      //if 0x00 ANY CLASS
@byte  edition    //if 0xFF ANY edition
@byte  map        //if 0xFF ANY MAP
@short flags      //if 0x00 ANY   (Judgment only)
@} //sizeof(8)

FindSound:
ldr	r4, SelectSfxList		@r4=Current SelectSfxList Pointer
sub	r4,#0x8 @troublesome to make a loop count, so substitute it.
            @ループ用カウントを作るのが面倒なので、代用する
Loop:
add	r4,#0x8

@check sound
ldrh	r0,[r4,#0x00] @data->sound
cmp	r0,#0x00
beq	End

@check if character
ldrb	r1,[r4,#0x02] @data->unit
cmp	r1,#0x00
beq	check_class

@check if the current character has that character ID
ldr	r2, [r5] @Load RAM Unit->ROM Unit
ldrb	r2, [r2,#0x04] @ROM Unit->Unit ID
cmp	r1, r2
bne	Loop


check_class:
ldrb	r1,[r4,#0x03] @data->class
cmp	r1,#0x00
beq	check_edition

ldr	r2, [r5,#0x4] @Load RAM Unit->ROM Class
ldrb	r2, [r2,#0x04] @ROM Class->Class ID
cmp	r1, r2
bne	Loop

check_edition:
ldrb	r1,[r4,#0x04] @data->edition
cmp	r1,#0xFF
beq	check_map

ldrb	r0, [r2, #0x1B] @Get Edition
cmp	r1, r0

check_map:
ldrb	r1,[r4,#0x05] @data->map
cmp	r1,#0xFF
beq	check_flag

ldrb	r0,[r6,#0x0E] @Get MapID
cmp	r1, r0
bne	Loop


check_flag:
ldrh	r0,[r4,#0x06] @data->flag
cmp	r0,#0x00
beq	check_already_played

ldr	r2, =#0x08083DA8
mov	r14, r2
.short	0xF800

cmp	r0,#0x00
beq	Loop

check_already_played:
@check if already played the effect, if so end if not set bit and play
@The area to be recorded is an unused flag area larger than 0x28.
@Record with unit bit mask.
@すでに再生したかどうかを記録する。
@記録する領域は、0x28より大きい未使用のフラグ領域。
@ユニットのビットマスクで記録する。

ldr		r2, [r5] @Load RAM Unit->ROM Unit
ldrb	r0, [r2,#0x04] @ROM Unit->unit_id

cmp	r0,#0x00
beq	Sound
cmp	r0,#0x8 * 0x0D
bgt	Sound

ldr	r1,=PLAYED_FLAG_RAM
mov r2,r1
add r2,r2,#0x0D

AddToOffsetLoop:
cmp	r0,#0x09
blo	EndLoop
add	r1,#0x01
sub	r0,#0x08
b	AddToOffsetLoop
EndLoop:
cmp	r1,r2
bhi	Loop
mov	r2,#0x01
lsl	r2,r0
lsr	r2,#0x01
ldrb	r0,[r1]
and	r0,r2
cmp	r0,#0x00
bne	Loop
ldrb	r0,[r1]
orr	r2,r0
strb	r2,[r1]

@play sound effect r0
Sound:
ldrh	r0,[r4,#0x00] @data->sound
ldr	r1,=#0x080D01FC @m4aSongNumStart
mov	lr, r1
.short	0xF800

@some original instructions then end
End:
pop {r4,r5,r6}
pop {r0}
bx r0
.ltorg
.align
SelectSfxList:
@POIN SelectSfxList

