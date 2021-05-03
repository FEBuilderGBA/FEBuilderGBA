.thumb

.equ origin, 0x1876C
.equ GetUnitEquippedItem, . + 0x168D0 - origin
.equ GetItemAttributes, . + 0x17314 - origin

push	{lr}
bl		GetUnitEquippedItem
bl		GetItemAttributes
mov		r1,#2
and		r0,r1
lsr		r0,#1
pop		{r1}
bx		r1
