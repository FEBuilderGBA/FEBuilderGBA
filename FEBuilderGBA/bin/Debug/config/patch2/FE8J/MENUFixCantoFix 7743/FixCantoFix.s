.thumb
@Hook 0x1D114	@{J}
@Hook 0x1D4B0	@{U}

ldrb r1, [r5, #0xe] @ActionData@gActionData.xMove 
ldr r0, [r0]
add r0 ,r0, r1
ldrb r0, [r0]

cmp r0, #0x80
blt ExitStore

@FF‚È‚Ì‚ÅgActionData.MoveCount‚É‚ÍŠi”[‚µ‚È‚¢

ldr r3, =0x0801D11E|1	@{J}
@ldr r3, =0x0801D4BA|1	@{U}
bx  r3

ExitStore:
ldr r3, =0x0801D11C|1	@{J}
@ldr r3, =0x0801D4B8|1	@{U}
bx  r3
