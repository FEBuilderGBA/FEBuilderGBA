.thumb

.equ origin, 0x188CC
.equ GetUnitEquippedItem, . + 0x16BC4 - origin
.equ GetItemAttributes, . + 0x17684 - origin

push	{lr}
bl		GetUnitEquippedItem
bl		GetItemAttributes
mov		r1,#2
and		r0,r1
lsr		r0,#1
pop		{r1}
bx		r1
