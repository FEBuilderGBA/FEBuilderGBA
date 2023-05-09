.align 4
.macro blh to, reg=r3
ldr \reg, =\to
mov lr, \reg
.short 0xf800
.endm

.thumb
.equ MemorySlot0, 0x30004B8 @FE8U
.equ GetUnitByEventParameter, 0x800bc50
.equ HandleAllegianceChange, 0x08018430	@FE8U

@s1 = unit

push {r4,lr}

ldr	r4,=MemorySlot0
ldr	r0,[r4,#0x4]	
blh	GetUnitByEventParameter		
mov	r1,#0xC0			
blh  	HandleAllegianceChange	

blh  0x0801a1f4   @RefreshFogAndUnitMaps    {U}
blh  0x080271a0   @SMS_UpdateFromGameData   {U}
blh  0x08019c3c   @UpdateGameTilesGraphics  {U}

POP {r4}
POP {r0}
BX r0
