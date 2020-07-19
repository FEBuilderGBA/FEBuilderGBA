@Call FE8J 2AC1C {J}
@Call FE8U 2ACAC {U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


ldr r1,[r6]
ldrb r1,[r1, #0x04] @RAMUnit->Unit->ID
ldr r0,[r6,#0x4]
ldrb r0,[r0, #0x04] @RAMUnit->Class->ID

ldr r3, TABLE
sub r3, #0x2
Loop:

add r3, #0x2
ldrh r2,[r3]
cmp  r2,#0xff
beq  NotFound

CheckUnit:
ldrb r2,[r3,#0x00]
cmp  r2,#0x0
beq  CheckClass
cmp  r2,r1   @ChcekUnit
bne  Loop

CheckClass:
ldrb r2,[r3,#0x01]
cmp  r2,#0x0
beq  Found
cmp  r2,r0   @CheckClass
bne  Loop

Found:

@必殺率を0に設定する
mov r0,#0x0
strh r0, [r5, #0x0]

@一応、続きの処理を実行する
b  Exit

NotFound:
Exit:
@壊すコードの再送
mov r0 ,r2
add r0, #0x48
ldrh r0, [r0, #0x0]
@blh 0x08017294   @GetItemIndex	{J}
blh 0x080174ec   @GetItemIndex	{U}

@ldr r3, =0x0802AC26|1	@{J}
ldr r3, =0x0802ACB6|1	@{U}
bx  r3

.ltorg
TABLE:
