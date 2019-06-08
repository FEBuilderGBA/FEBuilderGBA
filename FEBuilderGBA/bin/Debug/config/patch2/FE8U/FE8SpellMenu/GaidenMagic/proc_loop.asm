.org 0
.thumb
@ installed at 0xB55108
@replaces routine at 2b3ec - GenerateCurrentRoundData
Wrapper:
push    {r4-r7,r14}                @ 0802B3EC B5F0     
mov     r7,r10                @ 0802B3EE 4657     
mov     r6,r9                @ 0802B3F0 464E     
mov     r5,r8                @ 0802B3F2 4645     
push    {r5-r7}                @ 0802B3F4 B4E0     

mov     r8,r0             @Attacker                @ 0802B3F6 1C06     
mov     r9,r1             @Defender                @ 0802B3F8 4688     

ldr     r0,=0x802b444    @pointer to the current round
ldr     r0, [r0]          @current round pointer (usually 203a608)
ldr     r5, [r0]         @current round (originally starting at 203a5ec), increment by 4 bytes to get the next round

ldr     r0,=0x203A4D4    @Battle Stats Data?                @ 0802B3FA 4C11   
mov     r6, r0

mov     r0, #0x0                @ 0802B3FC 2000     
strh    r0, [r6,#0x4]      @Clear out damage dealt               @ 0802B3FE 80A0  

@now r5 is buffer pointer, r6 is battle stats, r7 is routine to run
@r8 is the attacker, r9 the defender
@r4 is for bl-ing

mov r4,pc
add r4, #(LoopTable - Subtract) @need to double check here
Subtract:
mov r7,r4 @r7 is the address of the next routine to run

MainLoop:
ldr r4, [r7]
cmp r4, #0 @no more routines?
beq Wrapper_End
mov r0, #1
orr r4, r0 @make sure it's thumb
mov r0, r8 @attacker
mov r1, r9 @defender
mov r2, r5 @battle buffer
mov r3, r6 @battle data
bl goto_r4 @run the routine
add r7, #4 @next pointer
b MainLoop

Wrapper_End:   
pop     {r3-r5}                @ 0802B55E BC38     
mov     r8,r3                @ 0802B560 4698     
mov     r9,r4                @ 0802B562 46A1     
mov     r10,r5                @ 0802B564 46AA     
pop     {r4-r7}                @ 0802B566 BCF0     
pop     {r0}                @ 0802B568 BC01     
bx      r0
goto_r4:
bx      r4                @ 0802B56A 4700

.ltorg
.align 2
LoopTable:
@this is a table of pointers
