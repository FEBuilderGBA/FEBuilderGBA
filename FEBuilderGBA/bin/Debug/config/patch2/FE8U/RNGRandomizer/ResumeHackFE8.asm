.thumb

pop {r3}

@overwritten code
ldrh r1,[r0]
strh r1,[r2]
add r0,#0x2
ldrh r1,[r0]
strh r1,[r2,#0x2]
ldrh r1,[r0,#0x2]
strh r1,[r2,#0x4]

push {r0}
push {r1}

ldr r1,AttackerData
ldr r0,=#0x00000000
str r0,[r1]

ldr r1,ActiveUnitDeploymentNumber
ldrb r0,=#0x00
strb r0,[r1]

pop {r1}
pop {r0}

@ldr r1,ReturnAddress
bx r14

.align 4
AttackerData:
.long 0x0203A4EC
ActiveUnitDeploymentNumber:
.long 0x03004E6A
