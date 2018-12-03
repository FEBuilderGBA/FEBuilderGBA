.thumb
@Call 9e414 @FE8J

mov r1 ,r0
lsl r0 ,r1 ,#0x1
add r0 ,r0, r1
lsl r0 ,r0 ,#0x3
mov r1 ,r5
ldr r1, [r1, #0x4]
ldrb r1, [r1, #0x4]
ldr r2, LuckTable

add r1 ,r1, R2
ldrb r1, [r1, #0x0]  @最大の幸運値をr1に返す

ldr r3, =0x0809E41E
mov pc, r3

.ltorg
.align
LuckTable:
