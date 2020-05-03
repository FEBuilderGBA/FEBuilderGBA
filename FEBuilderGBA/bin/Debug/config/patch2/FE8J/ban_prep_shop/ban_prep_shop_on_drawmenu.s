.thumb
@Call: 0809B6EC
@
@条件にマッチする章で、進撃準備の店を利用できなくします
@

ldr r2,=0x0202BCEC	@ChapterData	{J}
ldrb r2,[r2,#0xe]  @Current->MapID

ldr r3, BAN_PREP_SHOP_TABLE
sub r3, #0x1

Loop:
add r3, #0x1

ldrb r0, [r3]

cmp r0,r2
beq Match

cmp r0, #0xFF
beq Break
b   Loop

@店の使用禁止
Match:
mov r6 ,r5

ldr r0, =0x03005270 	@(gSomeWMEventRelatedStruct ) FE8U
ldrb r1, [r0, #0x0]
mov r0, #0x1
and r0 ,r1
cmp r0, #0x0
beq Armory

ldr	r3,=0x0809B716 + 1	@ For FE8J
b Exit

Armory:
ldr	r3,=0x0809B750 + 1	@ For FE8J
b Exit

@普通の店として機能させる
Break:

@壊すコードの再送
ldr r0, =0x03005270 	@(gSomeWMEventRelatedStruct ) FE8J
ldrb r1, [r0, #0x0]
mov r0, #0x1
and r0 ,r1

ldr	r3,=0x0809B6F4+1	@For FE8J
b Exit

Exit:
bx	r3

.ltorg
BAN_PREP_SHOP_TABLE:
