@Call FE8J 2AB6C {J}
@Call FE8U 2A6BC {U}
.thumb
@r1には命中率が入っているので保護する
push {r1}

@壊すコードの再送
ldr r0, [r6, #0x4]
ldrb r0, [r0, #0x4] @classid

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
pop {r1}

@ldr r3, =0x0802ab78|1	@{J}
ldr r3, =0x0802a6c8|1	@{U}
bx  r3

NotFound:
pop {r1}

@ldr r3, =0x0802ab7c|1	@{J}
ldr r3, =0x0802a6cc|1	@{U}
bx  r3

.ltorg
TABLE:
