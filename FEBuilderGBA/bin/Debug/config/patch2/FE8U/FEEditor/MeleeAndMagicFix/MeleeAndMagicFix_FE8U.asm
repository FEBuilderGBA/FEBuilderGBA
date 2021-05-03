.thumb

.equ origin, 0x18A58
.equ GetUnitEquippedItem, . + 0x16B28 - origin
.equ GetItemAttributes, . + 0x1756C - origin

push	{lr}
bl		GetUnitEquippedItem
bl		GetItemAttributes
mov		r1,#2
and		r0,r1
lsr		r0,#1
pop		{r1}
bx		r1
