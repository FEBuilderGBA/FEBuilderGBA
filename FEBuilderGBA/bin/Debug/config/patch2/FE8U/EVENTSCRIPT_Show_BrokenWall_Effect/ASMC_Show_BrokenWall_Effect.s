.thumb

push {lr}
SUB SP, #0x4

ldr	r3,=0x30004B8	@FE8U MemorySlot0
@ldr r3, =0x030004B0	@FE8J MemorySlot0

ldrb r1,[r3,#0x4 * 4]
STR r1,[SP, #0x0]      @arg4 Count

ldrb r1,[r3,#0x4 * 1]  @arg1 x
ldrb r2,[r3,#0x4 * 2]  @arg2 y
ldrb r3,[r3,#0x4 * 3]  @arg3 Direction

push {r3}
ldr  r3,=0x0801F780		@FE8U Show_BrokenWall_Effect
@ldr  r3,=0x0801F3D8		@FE8J Show_BrokenWall_Effect
mov  r14,r3
pop {r3}
.short 0xF800

ADD SP, #0x4
pop {r0}
bx r0
