.thumb
ldr   r0,=0x2024CC0     @Key press bitfield pointer
ldrh  r1,[r0,#0x4]      @Key press bitfield start
mov   r2,r1
ldr   r0,=0x30004B8     @FE8U MemorySlot0
mov   r1,#0x0C          @FE8U MemorySlotC
lsl   r1,#2
str   r2,[r0,r1]
bx    lr
