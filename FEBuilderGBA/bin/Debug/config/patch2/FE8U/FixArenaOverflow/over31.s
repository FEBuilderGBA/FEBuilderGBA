@Hook 080319F0	@FE8J
@Hook 08031AA4	@FE8U
@
@LOADできなくなるので、レベルが31を超えないようにします。
@そして、超過分は後で補正します。
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@r4  ArenaData
@r5  store data (LordUnit Data)
@r6  gArenaOpponent

ldrb r1, [r4, #0x12] @ArenaData@闘技場の一時データ.oppenentLevel
cmp r1, #0x1F  @stop level 31
ble Next
	mov r1, #0x1F
Next:

lsl r1 ,r1 ,#0x3
mov r0, #0x7
and r0 ,r2
orr r0 ,r1

strb r0, [r5, #0x3]
mov r2, r13  @assert r13 == r5
mov r1, #0x1
orr r0 ,r1
strb r0, [r2, #0x3]
mov r0, r13  @assert r13 == r5

mov r3, #0x0
strb r3, [r0, #0xc]
strb r3, [r0, #0xd]
strb r3, [r0, #0xe]
strb r3, [r0, #0xf]
strb r3, [r0, #0x10]
strb r3, [r0, #0x10]
strb r3, [r0, #0x11]
strb r3, [r0, #0x12]
strb r3, [r0, #0x13]
mov r0 ,r6
@blh 0x0801759c   @ClearUnitStruct	{J}
blh 0x080177f4   @ClearUnitStruct	{U}

mov r0, #0x80
strb r0, [r6, #0xb]
mov r0 ,r6
mov r1, r13  @assert r13 == r5
@blh 0x08017a5c   @StoreNewUnitFromCode	{J}
blh 0x08017D3C   @StoreNewUnitFromCode	{U}

@レベルを書き直す
ldrb r1, [r4, #0x12] @ArenaData@闘技場の一時データ.oppenentLevel
strb r1, [r6, #0x8]  @gArenaOpponent->LV

ldr r1, [r6, #0x0] @gArenaOpponent->Unit
mov r0 ,r6
@blh 0x08017b54   @LoadUnitStats	{J}
blh 0x08017e34   @LoadUnitStats	{U}

@このあと難易度によってはAutoLevelを経由するので、
@そこでレベルが変にならないように、書き戻しすためのレベルを保存しておきます
ldrb r4, [r6, #0x8] @gArenaOpponent->LV

@return
@ldr r3, =0x08031A36|1	@{J}
ldr r3, =0x08031AEA|1	@{U}
bx r3
