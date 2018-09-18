@r0
@r1
@r2
@r3
@r4    Haiku Table
@r5    gChapterData
@r6    UnitID
@r7    (相手のデータを見つける)

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@Call 080846E4
.thumb

push  {r4,r5,r6,r7,lr}   @ 関数ごと乗っ取る
mov   r6,r0              @ Unit ID

ldr   r5,=0x0202BCF0     @ FE8U (gChapterData )
                         @ よく利用するのでキャッシュしましょう.

@相手を探す.
ldr   r0,=0x0203E190     @ FE8U
ldrb  r7,[r0,#0x00]
cmp   r7,r6
bne   SearchHaikuTable

ldrb  r7,[r0,#0x01]      @ 自分でなかったら、相手のデータを参照する.


SearchHaikuTable:
ldr  r4, =0x0808472C     @ FE8U haiku pointer
ldr  r4, [r4]
sub  r4, #0xC            @ ループが面倒なので-12バイトを引いておく.


Loop:
add  r4, #0xC            @ 次のデータへ

ldrh r0, [r4, #0x0]      @ 終端のチェック 0xFFFF
ldr  r1, =0xFFFF
cmp  r0, r1
beq  NotFound

ldrb r0, [r4, #0x0]      @ Target   UnitID
cmp  r0, r6
bne  Loop

ldrb r1, [r4, #0x1]      @ Attacker UnitID
cmp  r1, #0x00           @ If Any
beq  CheckEdition

cmp  r1, r7              @ Check Attacker Unit
bne  Loop

CheckEdition:
ldrb r0, [r4, #0x2]
cmp  r0, #0xff           @Check Any
beq  CheckMap


ldrb r1, [r5, #0x1b]     @gChapterData.Edition
cmp  r0,r1
bne  Loop

CheckMap:
ldrb r0, [r4, #0x3]
cmp  r0, #0xff           @Check Any
beq  CheckFlag


ldrb r1, [r5, #0xE]      @gChapterData.MapID
cmp  r0,r1
bne  Loop

CheckFlag:
ldrh r0, [r4, #0x4]
blh  0x0800d4b8          @FE8U  CheckEventId_ 
@@lsl r0 ,r0 ,#0x18      @おそらく不要だとは思う bool型
cmp r0, #0x0
bne  Loop

Found:
mov  r0, r4
b    Exit

NotFound:
mov  r0, #0x00

Exit:
pop {r4,r5,r6,r7}
pop {r1}
bx r1
