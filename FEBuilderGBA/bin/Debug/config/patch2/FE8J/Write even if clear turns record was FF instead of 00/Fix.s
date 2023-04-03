.thumb
@クリアターン数記録領域が00ではなくFFだったとしても書き込む
@hook 080A8D24	r1	@{J}
@hook 080A42E0	r1	@{U}
cmp r0, #0x0
beq TrueExit

ldr r1, =0x0000FF80
cmp r0, r1
beq TrueExit

FalseExit:
@ldr r1, =0x080a8d18|1	@{J}
ldr r1, =0x080a42d4|1	@{U}
bx  r1

TrueExit:
mov r0 ,r3
pop {r4}
pop {r1}
bx r1
