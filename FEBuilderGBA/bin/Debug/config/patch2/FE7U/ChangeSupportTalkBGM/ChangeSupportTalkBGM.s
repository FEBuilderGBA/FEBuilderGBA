@Call 08078B84  @ FE7U
@r2   Support Talk Struct
@r5   Support Index 1=Žx‰‡C 2=Žx‰‡B 3=Žx‰‡A
.thumb
cmp r5, #0x3
beq SupportA

cmp r5, #0x2
beq SupportB

SupportC:
ldrb r0, [r2, #0x10]
b    Exit

SupportB:
ldrb r0, [r2, #0x11]
b    Exit

SupportA:
ldrb r0, [r2, #0x12]

Exit:
ldr  r1,=0x08078BCA+1
bx   r1
