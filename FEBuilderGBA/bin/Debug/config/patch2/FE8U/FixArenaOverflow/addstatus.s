@Hook 080319F0	@FE8J
@Hook 08031B48	@FE8U
@
@対戦相手のステータスの補正を行います
@
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@r4  ArenaData
@r5  store data (LordUnit Data)
@r6  gArenaOpponent

push {r5}

@Table //sizeof == 0xC
@BYTE lv
@BYTE use_status_cap
@BYTE add_hp
@BYTE add_pow

@BYTE add_skill
@BYTE add_spd
@BYTE add_def
@BYTE add_res

@BYTE add_luck
@BYTE add_mag_reserve
@BYTE 00
@BYTE 00

@利用するデータの探索
bl FindTable
cmp r0,#0x0
beq RunStatusCap    @見つからない場合は終了

mov r5, r0          @よく参照するのでTableをr5にコピーします.
@ldr r4, =0x0203A8EC @ArenaData	@{J}
ldr r4, =0x0203A8F0 @ArenaData	@{U}

AddStatusHP:
mov r0,#0x12
mov r1,#0x2
mov r2,#120
bl AddParam

AddStatusStr:
mov r0,#0x14
mov r1,#0x3
mov r2,#63
bl AddParam

AddStatusSkill:
mov r0,#0x15
mov r1,#0x4
mov r2,#63
bl AddParam

AddStatusSpd:
mov r0,#0x16
mov r1,#0x5
mov r2,#63
bl AddParam

AddStatusDef:
mov r0,#0x17
mov r1,#0x6
mov r2,#63
bl AddParam

AddStatusRes:
mov r0,#0x18
mov r1,#0x7
mov r2,#63
bl AddParam

AddStatusLuck:
mov r0,#0x19
mov r1,#0x8
mov r2,#63
bl AddParam

AddStatusMag:
bl CheckMagSplit
cmp r0,#0x0
beq CheckStatusCap

@FE8Jは、magをCONに格納します
mov r0,#0x1A
mov r1,#0x9
mov r2,#63
bl AddParam

@@FE8Uは、magを空き領域 0x3Aに格納します
@mov r0,#0x3A
@mov r1,#0x9
@mov r2,#63
@bl AddParam


CheckStatusCap:
ldrb r0,[r5,#0x1]
cmp r0,#0x1
bne FixLevel

RunStatusCap:
mov r0 ,r6
@blh 0x08017edc   @CheckForStatCaps	@FE8J
blh 0x080181c8   @CheckForStatCaps	@FE8U

@見た目が悪いので、Lv99以降は99とします。
FixLevel:
ldrb r0, [r6, #0x8]  @gArenaOpponent->Lv
cmp r0, #99
ble GoBack
	mov	r0, #99
	strb r0, [r6, #0x8]

GoBack:
pop {r5}
@ldr r3, =0x08031AA4|1	@FE8J
ldr r3, =0x08031B58|1	@FE8U
bx r3
.align 4
.ltorg

@ステータスパラメータの追加
@r0,#0x15 r6->status_pos
@r1,#0x4  r3->struct_pos
@r2,#63   limit
AddParam:
push {lr}

ldrb  r1, [r5, r1] @struct->etc
ldrb  r3, [r6, r0] @gArenaOpponent->etc
add   r1, r3

cmp   r1 ,r2
blt   AddParam_store
	mov r1, r2
AddParam_store:
strb  r1, [r6, r0] @gArenaOpponent->etc

pop {r0}
bx r0
.align 4
.ltorg


CheckMagSplit:
push {lr}

mov r0 , #0x0 @false

@@FE8J
@ldr r3,=0x802a542
@ldrh r1,[r3]
@
@ldr r2,=0x1c30
@cmp  r1,r2
@bne  CheckMagSplit_exit

@FE8U
ldr r3,=0x802BB44
ldr r1,[r3]

ldr r2,=0xF0A54B01
cmp  r1,r2
bne  CheckMagSplit_exit

mov r0, #0x1 @true
CheckMagSplit_exit:
pop {r0}
bx r0
.align 4
.ltorg


FindTable:
push {lr}

mov  r0, #0x0
ldrb r3, [r6, #0x8]  @gArenaOpponent->Lv

ldr r1, Table

FindTable_loop:

ldr r2, [r1]
cmp r2, #0xFF  @TERM ff 00 00 00
beq FindTable_Exit

ldrb r2, [r1]  @Table->Lv
cmp  r3, r2
blt  FindTable_Exit

@条件を満たしているので値を記録します。
@次のデータがダメだったらこれを利用します
mov  r0, r1    @LastMatchTable
add  r1, #0xC
b    FindTable_loop

FindTable_Exit:
pop {r1}
bx r1

.align 4
.ltorg
Table:
