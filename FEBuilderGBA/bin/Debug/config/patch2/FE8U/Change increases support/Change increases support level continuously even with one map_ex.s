.thumb

ORR r0 ,r2
LSR r0 ,r0 ,#0x1F
@r0を書き換えてはいけない!

@ldr r1, =0x0202BCEC @{J} ChapterData@ステージの領域.Clock
ldr r1, =0x0202BCF0 @{U} ChapterData@ステージの領域.Clock
ldrb r1, [r1, #0xe] @MAPID
ldr  r3,TABLE

Loop:
ldrb r2,[r3]
cmp  r2,#0xff
beq  NotFound

cmp  r2,r1
beq  Match
add  r3,r3,#0x01
b    Loop


Match:
MOV r0, #0x0

NotFound:
bx lr

.ltorg
TABLE:
