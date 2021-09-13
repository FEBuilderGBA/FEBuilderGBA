@Hook B4A4C @FE8J
@Hook AFE34	@FE8U

.thumb

push {r3}             @r3はthis procsが格納されているので神聖不可侵である

ldr   r0, =0x02024CC0 @KeyStatusBuffer	FE8UとFE8J同じアドレス
ldrh  r3, [r0, #0x6]
cmp   r3, #0x0
beq   Exit				@どのキーも押されていないので終了

@現在CGモードですか?
ldr   r0, =0x03003080	@IORegisters	@FE8U {U}
@ldr   r0, =0x03003020	@IORegisters	@FE8J {J}
ldrb  r1, [r0, #0x1]
cmp   r1, #0x8   @BG3しか表示しないマジック番号
beq   ShowSoundRoomUI

CheckLKey:          @Lキーが押されていますか?
ldr   r1, =0x200
tst   r3, r1
beq   Exit          @Lキーが押されていないなら即終了

@Lキーが押されている場合、モード遷移
HideSoundRoomUI:
	ldr   r0, =0x03003080	@IORegisters	@FE8U {U}
@	ldr   r0, =0x03003020	@IORegisters	@FE8J {J}
	mov   r1, #0x8
	strb  r1, [r0, #0x1]
	b Exit2

@CGモードで何かキーが押されたら元に戻す
ShowSoundRoomUI:
	ldr   r0, =0x03003080	@IORegisters	@FE8U {U}
@	ldr   r0, =0x03003020	@IORegisters	@FE8J {J}
	mov   r1, #0x3F
	strb  r1, [r0, #0x1]
@	b Exit2

@入力されているキーで余計なことをしないように、親関数の終端に戻して、キーチェックを抜けます。
Exit2:
pop {r3}
ldr   r2, =0x080AFE88|1	@FE8U {U}
@ldr   r2, =0x080B4AA0|1	@FE8J {J}
bx    r2

Exit:
@壊すコードの再送
ldr   r0, =0x02024CC0 @KeyStatusBuffer	FE8UとFE8J同じアドレス
ldrh  r1, [r0, #0x8]
MOV   r0, #0x10
pop   {r3}
ldr   r2, =0x080AFE3C|1	@FE8U {U}
@ldr   r2, =0x080B4A54|1	@FE8J {J}
bx    r2

.align
.ltorg
