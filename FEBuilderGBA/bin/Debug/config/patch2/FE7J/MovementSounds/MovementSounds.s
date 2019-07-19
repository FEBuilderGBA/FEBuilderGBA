@ORG 0806D2FE  @FE7J
@
@by 7743
@
.thumb
.org 0x06D2FE
ldrb r1,[r0,#0x04]  @r0->ClassID
ldr  r3,=MovementSoundsTable
lsl  r1,r1,#0x2
ldr  r0,[r3,r1]     @Table[ClassID]
mov  pc,r0

.ltorg
MovementSoundsTable:
