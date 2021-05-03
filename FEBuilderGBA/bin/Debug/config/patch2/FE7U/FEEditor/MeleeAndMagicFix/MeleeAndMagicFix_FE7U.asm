.thumb

.equ origin, 0x184DC
.equ GetUnitEquippedItem, . + 0x16764 - origin
.equ GetItemAttributes, . + 0x1727C - origin

push	{lr}
bl		GetUnitEquippedItem
bl		GetItemAttributes
mov		r1,#2
and		r0,r1
lsr		r0,#1
pop		{r1}
bx		r1
