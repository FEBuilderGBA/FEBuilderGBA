.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@全部クリアする
ASMC_ClearAll:
push {lr, r4, r5}

mov r5, #0x0	@件数を求める

ldr r4, ArenaDicStruct
Loop:
ldr r0, [r4]
cmp r0, #0x0
beq Break

add r4, #0x14	@sizeof(ArenaDicStruct)
add r5, #0x1	@件数++
b   Loop

Break:

@占有バイトを求める
lsr r0, r5, #0x3

mov r1, #0x7
and r1, r5
cmp r1, #0x0
beq RunMemset

add r0, #0x1

RunMemset:
mov r2, r0   @クリアするバイトの総数
mov r1, #0x0 @0クリアなので0固定

ldr r0, ArenaDicConfig
ldr r0, [r0, #0x1C]	@ArenaDicConfig->RAM

blh 0x080d6968 ,r4   @memset

pop {r4, r5}
pop {r0}
bx r0


.ltorg
DATA:
.equ	ArenaDicStruct,	DATA+0
.equ	ArenaDicConfig,	DATA+4
