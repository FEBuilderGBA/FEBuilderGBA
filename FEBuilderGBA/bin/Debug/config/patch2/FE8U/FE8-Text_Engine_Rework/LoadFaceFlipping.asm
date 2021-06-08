.thumb
.include "_Text_Engine_Defs.asm"

@inserted inline at 7858

.equ nextplace, . + 0x8007894 - 0x8007858

mov		r6,r1
ldr		r4,=gpDialogueState
ldr		r5,[r4]
ldrb	r0,[r5,#0x11]
cmp		r0,#0xFF
bne		Label1
mov		r0,#1
bl		Dialogue_SetActiveFacePosition
Label1:
cmp		r6,#0xFF
bne		Label2			@if not 0xFF, it should be 1 or 0, which accounts for flipping
ldrb	r0,[r5,#0x11]
bl		GetDialogueFaceSlotXTile
mov		r6,#0
cmp		r0,#0xE
bgt		Label2
mov		r6,#1
Label2:
bl		IsBattleDeamonActive
cmp		r0,#0
bne		Label5
mov		r0,#2
orr		r6,r0
b		Label4
Label5:
bl		SetFaceGfxConfigForBattle
Label4:
b		nextplace
.ltorg
