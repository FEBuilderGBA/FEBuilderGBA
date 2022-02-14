@Call FE7J 2986C {J}
@Call FE7U 293BC {U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


@壊すコードの再送
ldr r0, [r7, #0x4]
ldrb r0, [r0, #0x4] @classid

ldr r1, [r7, #0x0]
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
add r4, #0x48

ldr r3, =0x0802989e|1	@{J}
@ldr r3, =0x0802948e|1	@{U}
bx  r3

NotFound:
add r4, #0x48
ldrh r0, [r4, #0x0]
blh 0x0801782c   @GetItemWeaponEffect	@{J}
@blh 0x08017424   @GetItemWeaponEffect	@{U}

ldr r3, =0x08029874|1	@{J}
@ldr r3, =0x080293C4|1	@{U}
bx  r3

.ltorg
TABLE:
