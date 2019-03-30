.thumb

push {lr}
ldr r0, =0x0203A614		@FE8U	TrapData
@ldr r0, =0x0203A610	@FE8J	TrapData

ldr r3,=#0x40*8
add r3,r0				@Term

Loop:
ldrb r1,[r0,#0x2] @Trap->Type
cmp  r1,#0xEF
beq  Found
add  r0, #0x8

cmp  r0,r3
blt  Loop

NotFound:
mov  r0,#0x00

Found:
pop {r1}
bx r1
