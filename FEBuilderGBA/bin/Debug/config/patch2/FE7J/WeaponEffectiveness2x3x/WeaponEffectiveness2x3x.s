.thumb
@Hook 0x28FC4	@{J}
@Hook 0x28B48	@{U}
ldr r1, Table

Loop:
ldrb r2,[r1]
cmp  r2,r0
beq  TrueExit
add  r1,#0x02
cmp  r2,#0x00
bne  Loop

FalseExit:
@規定がないので3倍特攻
mov r1 ,r5
add r1, #0x5a
ldrh r2, [r1, #0x0]
lsl r0 ,r2 ,#0x1    @ダメージ3倍
add r0 ,r2          @	{J}
                    @どういうわけかFE7Uはディフォルトで2倍らしい?

strh r0, [r1, #0x0]
b    Exit

TrueExit:
ldrb r0,[r1,#0x01]
mov r1 ,r5
add r1, #0x5a
ldrh r2, [r1, #0x0]  @ダメージ
mul  r0,r2           @N倍
strh r0, [r1, #0x0]  @修正後のダメージを格納
@b    Exit

Exit:
ldr  r3, =0x08028FF4|1	@{J}
@ldr  r3, =0x08028B52|1	@{U}
bx   r3

.ltorg
.align 4
Table:
