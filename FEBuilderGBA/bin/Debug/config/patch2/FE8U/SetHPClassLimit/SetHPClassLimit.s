@Hook 17EE0	{J}
@Hook 181CC {U}
.thumb

ldr  r7, [r4, #0x4] @class
mov  r5 ,r7         @class  後でやらないといけないので、今ここでやっておく

mov  r0, #0xb
ldrb r0, [r4, r0]  @ 所属
cmp  r0,#0x80
blt  PlayerOrNPC
cmp  r0,#0xc0
bge  PlayerOrNPC

Enenmy:
@LDR  r2,=0x08017EF0  @Enemy HP Caps Pointer	{J}
LDR  r2,=0x080181DC  @Enemy HP Caps Pointer	{U}
b    Join

PlayerOrNPC:
mov   r2,#0x13
add   r2,r2,r7       @Class HP Caps

Join:
ldrb  r2,[r2]        @HPCaps

mov  r1, #0x12
ldrb r1, [r4, r1]    @MAX HP

cmp   r1,#0xF0       @MAXHPをたくさん減らすしてマイナス地になったときの対処
bge   SetHP1

cmp  r1, r2          @Current MAX HPが上限を超えていたら補正する
bgt  Fix

CheckUnder:
cmp  r1,#0x0
bne  GoBack

SetHP1:
mov  r2,#0x1         @HPが0なので1に補正する

Fix:
mov  r1,r2           @ HPCapsを超えているので、補正する
mov  r2, #0x12
strb r1, [r4, r2]    @MAX HP
mov  r2, #0x13
strb r1, [r4, r2]    @Current MAX HP

GoBack:
@ldr r1, =0x08017F0C+1	@{J}
ldr r1, =0x080181F8+1	@{U}
bx  r1
