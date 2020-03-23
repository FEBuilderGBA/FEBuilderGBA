@Hook 080B0868 FE8J

@r6 slot

.thumb
ldr r3, Table
sub r3, #0x4

Loop:
add r3, #0x4

ldrb r0, [r3,#0x0]
cmp r0, #0xff
beq NotFound

ldr r1, =0x02000940
add r1, r6
ldrb r0, [r1]

ldrb r2,[r3,#0x0]
cmp r0, r2
bne Loop

lsl r1,r6,#0x3  @*8
add r1, #0x2
ldr r0, =0x02000948
add r1, r0
ldrb r0, [r1]

ldrb r2,[r3,#0x1]
cmp r0, r2
bne Loop

Found:
ldrb r0,[r3,#0x3]
b Exit

NotFound:
mov r0, #0x20  @ディフォルトカラー設定
@b Exit

Exit:
ldr r3,=0x080B08AC+1
bx r3

.ltorg
Table:
