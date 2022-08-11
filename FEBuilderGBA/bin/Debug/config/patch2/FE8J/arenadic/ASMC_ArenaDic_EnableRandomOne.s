.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


@どれか一つをランダムで有効にする
ASMC_ArenaDic_EnableRandomOne:
push {lr}

@総数を求めて、乱数で適当なところを探して、そこから最初に見つかった非公開データを有効にします
bl GetCount
blh 0x08000C58	@NextRN_N
bl EnableFirstData
cmp r0, #0x1
beq ASMC_ArenaDic_EnableRandomOne_Exit

@なんか全部有効みたいなので、最初から探索します
mov r0, #0x0
bl EnableFirstData

ASMC_ArenaDic_EnableRandomOne_Exit:
pop {r0}
bx r0


@r0件からスタートして、最初に見つかった非公開データを有効にします.
@有効にできたら1が返ります。 できなかったら0が返ります。
EnableFirstData:
push {lr, r4, r5}

mov r5, r0 @counter

ldr r4, ArenaDicStruct
mov r1, #0x14	@sizeof(ArenaDicStruct)
mul r0, r1
add r4, r0

EnableFirstData_Loop:
ldr r0, [r4]
cmp r0, #0x0
beq EnableFirstData_Break

EnableFirstData_CheckFlag:
ldrb r0, [r4,#0xC]	@ArenaDicStruct->ShowAlways
cmp r0, #0x0
bne EnableFirstData_Next

mov r0, r5
bl  CheckBit
cmp r0 ,#0x0
bne EnableFirstData_Next

@最初に見つかった非公開データなので有効にする
mov r0, r5
bl  SetBit
mov r0, #0x1
b   EnableFirstData_Exit

EnableFirstData_Next:
add r4, #0x14	@sizeof(ArenaDicStruct)
add r5, #0x1	@件数++
b   EnableFirstData_Loop

EnableFirstData_Break:
mov r0, #0x0

EnableFirstData_Exit:
pop {r4, r5}
pop {r1}
bx r1


CheckBit:
push {lr}
@convert to bitflag
asr r2 ,r0 ,#0x5
lsl r2 ,r2 ,#0x2

mov r1, #0x1f
and r0 ,r1

mov r1 ,#0x01
lsl r1 ,r0

ldr r3, ArenaDicConfig
ldr r3, [r3, #0x1C]	@ArenaDicConfig->RAM

ldr r0, [r3, r2]
and r0 ,r1
cmp r0, #0x0
beq CheckBit_Exit

mov r0, #0x1
CheckBit_Exit:
pop {r1}
bx r1

SetBit:
push {lr}
@convert to bitflag
asr r2 ,r0 ,#0x5
lsl r2 ,r2 ,#0x2

mov r1, #0x1f
and r0 ,r1

mov r1 ,#0x01
lsl r1 ,r0

ldr r3, ArenaDicConfig
ldr r3, [r3, #0x1C]	@ArenaDicConfig->RAM

ldr r0, [r3, r2]
and r0 ,r1
cmp r0, #0x0
bne SetBit_Exit  @既にON

@フラグを立てる
ldr r0, [r3, r2]
orr r0 ,r1
str r0, [r3, r2]

SetBit_Exit:
pop {r0}
bx r0


@総数を取得する
GetCount:
push {lr, r4, r5}

mov r5, #0x0	@件数を求める

ldr r4, ArenaDicStruct
GetCount_Loop:
ldr r0, [r4]
cmp r0, #0x0
beq GetCount_Break

add r4, #0x14	@sizeof(ArenaDicStruct)
add r5, #0x1	@件数++
b   GetCount_Loop

GetCount_Break:

pop {r4, r5}
pop {r1}
bx r1


.ltorg
DATA:
.equ	ArenaDicStruct,	DATA+0
.equ	ArenaDicConfig,	DATA+4
