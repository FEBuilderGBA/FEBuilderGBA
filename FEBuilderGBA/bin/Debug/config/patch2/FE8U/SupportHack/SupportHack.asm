.thumb
.org 0x00000000

Support_Hack_start: @paste this part to 0x8e90080

push {lr}
bl Check_AB_Supports
cmp r0, #0x1
beq Return_Noshow
mov r0, r5
mov r1, r6
ldr r3, =#0x080281f5
bl GOTO_R3
bl Check_AB_Supports
cmp r0, #0x1
beq Return_Noshow

Return:
pop {r3}
ldr r3, =0x8028353
GOTO_R3:
bx r3

Return_Noshow:
pop {r3}
ldr r3, =#0x0802836f
bx r3

Check_AB_Supports:

@NEW IDEA
@Loop through every support partner ONCE.
@For every existing support, add 1 to counter.
@For every A support, add $10 to counter.
@For the target character, save the support level to another counter.
@If 6 supports and target is <C, return True
@If A support and target is B, return True
@Else return False.

push {r4-r7,r14}

@@if r0 = r5 it's the first time around
@@if r0 = r4 it's checking the other character
cmp    r0,r5          @First time around
beq    LoopStart
eor    r4,r5
eor    r5,r4          @swap the registers around, then proceed as usual
eor    r4,r5

LoopStart:
mov r7,r5             @save active char data in r7
ldr r6,[r4]
ldrb r6,[r6,#0x4]      @target char's ID stored in r6
ldr r4, CheckSupportNum
bl Goto_r4
mov r5, r0            @get total number of supports in r5
mov r4, #0            @loop counter in r4
mov r3, #0            @support counter in r3
ldr r2,[r7]
ldr r2,[r2,#0x2C]     @r2 is the pointer to the support data

Loop:
mov r0,r7
mov r1,r4
push {r4}
ldr r4,CheckSupportLev
bl Goto_r4
pop {r4}
ldrb r1,[r2,r4]        @nth character's id
cmp r1,r6             @is it a match?
bne No_match
mov r6,r0             @save the support level in r6
lsl r6,#0x10          @make sure it doesn't match a character ID
No_match:
cmp r0,#0x3           @is it A support?
bne Not_A
add r3,#0x10          @A support flag
Not_A:
cmp r0,#0x0           @is there any support?
beq No_Support
add r3,#0x1           @increment support count
No_Support:
add r4,#0x1           @increment loop count
cmp r4,r5
blt Loop

Check:
mov r0,#0x1           @default is True
mov r4,#0xF
and r4,r3             @get bottom bit of counter (number of supports)
cmp r4,#0xB           @change this to #0x5 if you want 5 max supports
blt Room_For_Support
cmp r6,#0             @was target support lower than C?
beq True
Room_For_Support:
cmp r3,#0x10
blt False             @no A-supports (01 DB, change to 01 E0 for multiple A supports)
lsr r6,#0x10
cmp r6,#0x2           @A-support AND target is B
beq True
False:
mov r0,#0
True:
pop {r4-r7}
pop {r1}
bx r1

Goto_r4:
bx r4

.align
CheckSupportLev:        @Routine at 0x802823C, change this in the debugger
.long 0x802823c+1

CheckSupportNum:        @Routine at 0x80281C8, change in debugger
.long 0x80281c8+1
