@Call 0807D1D0 {J}
@Call 0807AE94 {U}
.thumb

@r6  load unit struct(unit placer)
@r7  rand 100%
@r12 summoner ramunit

ldr r3, Table
sub r3, #0xC @先に引いておく

Loop:

add r3, #0xC
ldr r0,[r3]
cmp r0,#0x0
beq NotFound

CheckUnit:
ldrb r0,[r3,#0x00] @Table->unit
cmp r0,#0x0
beq CheckClass

ldrb r1,[r6,#0x00] @summoned unit id
cmp r0,r1
bne Loop

CheckClass:
ldrb r0,[r3,#0x01] @Table->class
cmp r0,#0x0
beq CheckRare

ldrb r1,[r6,#0x01] @summoned class id
cmp r0,r1
bne Loop

CheckRare:
ldrb r0,[r3,#0x0A] @Table->rare
cmp r7,r0
bgt CheckLevel1

ldrb r0,[r3,#0x0B]
cmp r0,#0x00
beq CheckLevel1
b Exit

CheckLevel1:
mov r2,r12
ldr r2, [r2, #0x0]  @ summoner ramunit
ldrb r2, [r2, #0x8] @ ramunit->level

ldrb r0,[r3,#0x02] @Table->level1
cmp r2,r0
bgt CheckLevel2

ldrb r0,[r3,#0x03]
cmp r0,#0x00
beq CheckLevel2
b Exit

CheckLevel2:
ldrb r0,[r3,#0x04] @Table->level2
cmp r2,r0
bgt CheckLevel3

ldrb r0,[r3,#0x05]
cmp r0,#0x00
beq CheckLevel3
b Exit

CheckLevel3:
ldrb r0,[r3,#0x06] @Table->level3
cmp r2,r0
bgt CheckLevel4

ldrb r0,[r3,#0x07]
cmp r0,#0x00
beq CheckLevel4
b Exit

CheckLevel4:
ldrb r0,[r3,#0x08] @Table->level3
cmp r2,r0
bgt Loop

ldrb r0,[r3,#0x09]
cmp r0,#0x00
beq Loop
b Exit



NotFound:
mov r0, #0x1f  @鉄の斧 (iron axs)

Exit:
@r0 = weapon id
@ldr r3,=0x0807D23A+1 @GoBack {J}
ldr r3,=0x0807AEFA+1 @GoBack {U}
bx r3

.ltorg
Table:
@unit class level1<= weapon1 level2<= weapon2 level3<= weapon3 level4<= weapon4,レア確率<=,レア武器
