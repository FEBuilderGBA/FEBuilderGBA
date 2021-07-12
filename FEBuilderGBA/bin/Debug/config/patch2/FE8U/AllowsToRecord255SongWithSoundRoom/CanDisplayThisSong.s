.thumb
@Hook 080B39E8	{J}
@Hook 080AEDC8	{U}

@r0     結果を返すbool
@r1     壊していい
@r2     keep
@r3     must set 0x1f
@r4     keep
@r5     keep
@r6     sound room id keep
@r7     this procs keep
@sp     スタック変数にアクセスするのでpush/popがないのか望ましい

@行儀が悪いのだが、sound room id == 0 だったときに、領域の初期化を行います
@これは、procsの割り当て時に後半ま領域にはデータが入ってることがあるためです。
@Sound roomでは、 this procs + 0x20 より 0x20 バイトを曲が聞けるかどうかの判定に利用します。
@この領域で後半の領域にゴミデータがあると、誤判定が起きてしまうので初期化しておきます。
@本当は、このループ内ではなく、事前にやるべきですが、フックポイントを増やしたくないので、ここでやります。
cmp   r6, #0x0
bne   CanDisplaySong
InitEnableSongList:
str   r6, [r7 , #0x40]
str   r6, [r7 , #0x44]
str   r6, [r7 , #0x48]
str   r6, [r7 , #0x50]
str   r6, [r7 , #0x54]
str   r6, [r7 , #0x58]
str   r6, [r7 , #0x60]

CanDisplaySong:
                    @sp[r/8] & (1 << (r%8))
lsr   r0, r6 , #0x3 @ r0 = r6 / 8
mov   r1, sp
ldrb  r3, [r1, r0]

mov   r1, #0x7      @ r0 = r6 % 8
and   r1, r6        
mov   r0, #0x1
lsl   r0, r1        @ r0 = 1 << r1
and   r0, r3        @check bit

@戻しに使えるレジスタが足りないので、なんとかする
cmp   r0, #0x0
beq   NotFound

Found:
mov   r0, #0x1
b     Exit

NotFound:
@mov  r0, #0x0      @既に0x0なのでやる必要はない

Exit:
mov   r3, #0x1f     @0x1fでなければならない

@ldr r1, =0x80B39FA|1	@{J}
ldr r1, =0x80AEDDA|1	@{U}
bx  r1
