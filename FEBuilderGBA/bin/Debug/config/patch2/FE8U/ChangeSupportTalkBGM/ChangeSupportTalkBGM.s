@Call 080847A4  @ FE8U
@r0   Support Talk Struct
@r4   Support Index 1=Žx‰‡C 2=Žx‰‡B 3=Žx‰‡A
.thumb
cmp r4, #0x3
beq SupportA

cmp r4, #0x2
beq SupportB

SupportC:
ldrh r0, [r0, #0xA]
b    Exit

SupportB:
ldrh r0, [r0, #0xc]
b    Exit

SupportA:
ldrh r0, [r0, #0xE]

Exit:
ldr  r1,=0x080847F0+1
bx   r1
