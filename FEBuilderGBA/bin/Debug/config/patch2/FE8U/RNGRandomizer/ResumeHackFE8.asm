.thumb

@overwritten code
ldrh r1,[r0]
strh r1,[r2,#0x2]
ldrh r0,[r2,#0x2]
strh r0,[r2,#0x4]

push {r0}

ldr r3,AttackerData
ldr r0,=#0x00000000
str r0,[r3]

ldr r3,ActiveUnitDeploymentNumber
ldrb r0,=#0x00
strb r0,[r3]

pop {r0}

ldr r3,ReturnAddress
bx r3

.align 4
ReturnAddress:
.long 0x08000C44|1
AttackerData:
.long 0x0203A4EC
ActiveUnitDeploymentNumber:
.long 0x03004E6A
