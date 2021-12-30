.thumb
.org 0x0

@r0 = char data, r1 = item id/uses

push	{r4,r5,r14}
mov		r4,r0
mov		r0,#0xFF
and		r0,r1
ldr		r3,=0x080177B0	@GetItemData	{U}
@ldr		r3,=0x08017558	@GetItemData	{J}
mov		r14,r3
.short	0xF800
mov		r3,r4
ldr		r5,[r0,#0x8]	@item abilities
ldr		r2,LockWord
and		r5,r2
cmp		r5,#0x0
beq		RetTrue
ldr		r4,[r3]
ldr		r4,[r4,#0x28]	@character abilities
ldr		r2,[r3,#0x4]
ldr		r2,[r2,#0x28]	@class abilities
orr		r4,r2			@total class abilities
mov		r2,#0x80
lsl		r2,r2,#0x4		@item ability 2, byte 8 (Weapon Lock 1)
and		r2,r5
cmp		r2,#0x0
beq		NextLock1
mov		r2,#0x80
lsl		r2,r2,#0x9		@class ability 3, byte 1 (Weapon Lock 1)
and		r2,r4
cmp		r2,#0x0
beq		RetFalse
NextLock1:
mov		r2,#0x80
lsl		r2,r2,#0xB		@item ability 3, byte 4 (Eirika Lock)
and		r2,r5
cmp		r2,#0x0
beq		NextLock2
mov		r2,#0x80
lsl		r2,r2,#0x15		@class ability 4, byte 10 (Rapier/Siegliede)
and		r2,r4
cmp		r2,#0x0
beq		RetFalse
NextLock2:
mov		r2,#0x80
lsl		r2,r2,#0xC		@item ability 3, byte 8 (Ephraim Lock)
and		r2,r5
cmp		r2,#0x0
beq		NextLock3
mov		r2,#0x80
lsl		r2,r2,#0x16		@class ability 4, byte 20 (Reginleig/Siegmund)
and		r2,r4
cmp		r2,#0x0
beq		RetFalse
NextLock3:
mov		r2,#0x80
lsl		r2,r2,#0xD		@item ability 3, byte 10 (Unused Lyn lock)
and		r2,r5
cmp		r2,#0x0
beq		NextLock4
mov		r2,#0x80
lsl		r2,r2,#0x17		@class ability 4, byte 40 (Mani Katti)
and		r2,r4
cmp		r2,#0x0
beq		RetFalse
NextLock4:
mov		r2,#0x80
lsl		r2,r2,#0xE		@item ability 3, byte 20 (Weapon Lock 2)
and		r2,r5
cmp		r2,#0x0
beq		NextLock5
mov		r2,#0x80
lsl		r2,r2,#0x18		@class ability 3, byte 80 (Forblaze, but I didn't see any other weapon lock 2, and in fact there's no corresponding class ability in the code. Go figure.)
and		r2,r4
cmp		r2,#0x0
beq		RetFalse
NextLock5:
mov		r2,#0x80
lsl		r2,r2,#0x5		@item ability 2, byte 10 (Myrmidon/Swordmaster lock)
and		r2,r5
cmp		r2,#0x0
beq		NextLock6
mov		r2,#0x80
lsl		r2,r2,#0x0A		@class ability 3, byte 2 (Shamshir)
and		r2,r4
cmp		r2,#0x0
beq		RetFalse
NextLock6:
mov		r2,#0x80
lsl		r2,r2,#0x3		@item ability 2, byte 4 (Monster weapon)
and		r2,r5
cmp		r2,#0x0
beq		RetTrue
mov		r2,#0x80
lsl		r2,r2,#0xB		@class ability 3, byte 4 (Monster weapon)
and		r2,r4
cmp		r2,#0x0
bne		RetTrue
RetFalse:
mov		r0,#0x0
b		GoBack
RetTrue:
mov		r0,#1
GoBack:
pop		{r4-r5}
pop		{r1}
bx		r1

.align
LockWord:
.long 0x003D3C00
