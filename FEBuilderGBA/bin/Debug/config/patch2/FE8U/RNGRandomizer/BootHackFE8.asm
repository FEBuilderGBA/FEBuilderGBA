.thumb

@overwritten code
neg r4,r0
orr r4,r0
lsr r4,r4,#0x1F
mov r0,r4

push {r0}

ldr r3,AttackerData
ldr r0,=#0xFFFFFFFF
str r0,[r3]

ldr r3,ActiveUnitDeploymentNumber
ldrb r0,=#0xFF
strb r0,[r3]

pop {r0}
ldr r3,ReturnAddress
bx r3

.align 4
ReturnAddress:
.long 0x08000A54|1
AttackerData:
.long 0x0203A4EC
ActiveUnitDeploymentNumber:
.long 0x03004E6A
