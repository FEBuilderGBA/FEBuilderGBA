.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.equ proc_truehit, 0x802A558
.equ d100Result, 0x802a52c
@ r0 is attacker, r1 is defender, r2 is current buffer, r3 is battle data
push {r4-r7,lr}
mov r4, r0 @attacker
mov r5, r1 @defender
mov r6, r2 @battle buffer
mov r7, r3 @battle data

@removed sure shot check, just unset the miss flag if needed.
ldrh    r0,[r7,#0xA]      @final hit rate                @ 0802B41A 8960     
mov     r1,#0x1           @Default depending on where battle is called, leave it alone             @ 0802B41C 2101     
blh     proc_truehit        @Proc hit rate                @ 0802B41E F7FFF89B     
cmp     r0,#0x0                @ 0802B424 2800     
bne     SuccessfulHit        @If we hit, branch                @ 0802B426 D111   
@if we missed, set the miss flag  
ldr     r2,[r6]    
lsl     r1,r2,#0xD                @ 0802B42C 0351     
lsr     r1,r1,#0xD                @ 0802B42E 0B49     
mov     r0,#0x2           @miss flag     @ 0802B430 2002  
orr     r1,r0                @ 0802B432 4301     
ldr     r0,=#0xFFF80000                @ 0802B434 4804     
and     r0,r2                @ 0802B436 4010     
orr     r0,r1                @ 0802B438 4308     
str     r0,[r6]    @store the new battle buffer   
b End

SuccessfulHit:
@now calculate normal damage
ldrh r0, [r7, #6] @final mt
lsl r0, #0x10
asr r0, #0x10
ldrh r1, [r7, #8] @final def
lsl r1, #0x10
asr r1, #0x10
sub r0, r1
strh r0, [r7, #4] @final damage

@now to check for a crit
ldrh r0, [r7, #0xc] @crit rate
mov r1, #0
blh d100Result
cmp r0, #1
bne End

@if crit:
mov r0, #4
ldrsh r0, [r7, r0]
lsl r1, r0, #1
add r0, r1 @damagex3
strh r0, [r7, #4] @final damage

@set crit flag
ldr     r2,[r6]    
lsl     r1,r2,#0xD                @ 0802B42C 0351     
lsr     r1,r1,#0xD                @ 0802B42E 0B49     
mov r0, #1
orr r1, r0
ldr     r0,=#0x7FFFF                @ 0802B516 4815     
and     r1,r0                @ 0802B518 4001
ldr     r0,=#0xFFF80000                @ 0802B434 4804     
and     r0,r2                @ 0802B436 4010     
orr     r0,r1                @ 0802B438 4308     
str     r0,[r6]                @ 0802B43A 6018   

End:
pop {r4-r7}
pop {r15}
