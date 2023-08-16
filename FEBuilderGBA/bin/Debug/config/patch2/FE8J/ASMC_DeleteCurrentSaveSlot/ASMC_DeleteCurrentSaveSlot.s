.thumb
.macro blh to, reg=r3
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

DeleteCurrentSaveSlot:
push {lr}
@ldr r0, =0x0202BCF0	@gChapterInfo	@{U}
ldr r0, =0x0202BCEC	@gChapterInfo	@{J}
ldrb r0, [r0, #0xC]

@blh 0x080A4DC8	@DeleteSaveSlot	@r0=SlotID	{U}
blh 0x080A980C	@DeleteSaveSlot	@r0=SlotID	{J}
pop {r0}
bx r0
