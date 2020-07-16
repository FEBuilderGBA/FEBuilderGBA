@Call FE8J 2B578 {J}
@Call FE8U 2B624 {U}
.thumb

@壊すコードの再送
ldr r0, [r4, #0x4]
ldrb r0, [r0, #0x4] @classid
mov r7 ,r5
add r7, #0x48

ldr r1, [r4, #0x0]
ldrb r1, [r1, #0x4] @unitid

ldr r3, TABLE
sub r3, #0x2
Loop:

add r3, #0x2
ldrh r2,[r3]
cmp  r2,#0xff
beq  NotFound

CheckUnit:
ldrb r2,[r3,#0x00]
cmp  r2,#0x0
beq  CheckClass
cmp  r2,r1   @ChcekUnit
bne  Loop

CheckClass:
ldrb r2,[r3,#0x01]
cmp  r2,#0x0
beq  Found
cmp  r2,r0   @CheckClass
bne  Loop

Found:
@ldr r3, =0x0802b5ea|1	@{J}
ldr r3, =0x0802b696|1	@{U}
bx  r3

NotFound:
@ldr r3, =0x0802B584|1	@{J}
ldr r3, =0x0802B630|1	@{U}
bx  r3

.ltorg
TABLE:
