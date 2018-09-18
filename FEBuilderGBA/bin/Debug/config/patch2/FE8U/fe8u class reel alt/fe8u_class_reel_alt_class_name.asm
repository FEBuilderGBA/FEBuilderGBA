.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@Call 080B369C
@クラス紹介で、指定した文字列を参照させます
@r0 class id -> result class name str
@r1 buffer

push {r4,lr}
mov r4 ,r1

mov  r1,r0
lsl  r1,#0x1
ldr  r2, fe8u_class_reel_alt_class_name_table   @Alt Class Name (Text ID)
add  r2,r1
ldrh r1,[r2]
cmp  r1, #0x00
beq  Orignal

mov  r0, r1
b    GetText

Orignal:              @ テキストが設定されていない場合、オリジナルのルーチンを再送
blh  0x08019444       @GetROMClassStruct
ldrh r0, [r0, #0x0]

GetText:
cmp r4, #0x0
bne Step2
blh 0x0800a240        @GetStringFromIndex 
mov r4 ,r0
b Step3

Step2:
mov r1 ,r4
blh 0x0800a280        @GetStringFromIndexInBuffer 

Step3:
ldr r1, =0x080B36D8
ldr r1, [r1]          @ポインタ参照してリポイント回避する
mov r0 ,r4
blh 0x080d1dcc
cmp r0, #0x0
beq step4

ldr r1, =0x080B36DC
ldr r1, [r1]         @ポインタ参照してリポイント回避する
blh 0x080d1cfc       @sprintf

step4:
mov r0 ,r4
pop {r4}
pop {r1}
bx r1

.ltorg
.align
fe8u_class_reel_alt_class_name_table:
@POIN fe8u_class_reel_alt_class_name_table
