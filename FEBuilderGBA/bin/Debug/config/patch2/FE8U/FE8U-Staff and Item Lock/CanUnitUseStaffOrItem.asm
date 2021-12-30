.thumb
.macro blh to, reg=r3
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm
.macro blh_ to, reg=r3
    ldr \reg, \to
    mov lr, \reg
    .short 0xF800
.endm


@r4=character data, r5=item id
@r0=0 if item is not a staff


push {r1}

mov r0, r4 @character data
mov r1, r5 @ItemID
blh_	Staff_Item_Lock

pop {r1}

cmp r0, #0x0
beq Exit_NotUse

cmp r1, #0x0  @杖かどうかの判定がboolで入ってる
beq Exit_ContinueItemProcessing

@杖処理を継続
Exit_ContinueStaffProcessing:
mov r0 ,r4
mov r1 ,r5
ldr r3 ,=0x08028888|1	@{U}
@ldr r3 ,=0x08028834|1	@{J}
bx  r3

@アイテム処理を継続
Exit_ContinueItemProcessing:
ldr r3, =0x08028894|1	@{U}
@ldr r3, =0x08028840|1	@{J}
bx  r3

@使えない判定
Exit_NotUse:
ldr r3, =0x08028C04|1	@{U}
@ldr r3, =0x08028BB0|1	@{J}
bx  r3

.ltorg
Staff_Item_Lock:
@
