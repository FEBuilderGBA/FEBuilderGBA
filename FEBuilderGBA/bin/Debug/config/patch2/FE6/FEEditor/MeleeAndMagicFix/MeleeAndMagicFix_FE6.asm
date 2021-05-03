.thumb

.equ origin, 0x18188
.equ GetUnitEquippedItem, . + 0x16958 - origin
.equ GetItemAttributes, . + 0x17190 - origin

push	{lr}
bl		GetUnitEquippedItem
bl		GetItemAttributes
mov		r1,#2
and		r0,r1
lsr		r0,#1
pop		{r1}
bx		r1
