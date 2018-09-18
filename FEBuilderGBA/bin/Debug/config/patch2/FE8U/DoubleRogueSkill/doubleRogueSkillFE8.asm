@Call 23E8C (FE8U)
@Call 23E48 (FE8J)
@r0    temporary    class id
@r1    temporary	Loop ClassList
@r2    data (Keep)
@r3    temporary
@

.thumb
.org 0

ldr r0, [r2, #0x4]
ldrb r0, [r0, #0x4]

ldr r1, ClassList @ for(int i=0;i<max;i++) {} //This is not the case
                  @ for(p = ClassList ; *p ; p++){} //When writing like this, there are few registers and processing is fast.
Loop:
ldrb r3, [r1]
cmp r3, #0x00
beq NoPickSkill
cmp r3, r0
beq PickSkill
add r1,#0x01	@loop next
b Loop

NoPickSkill:
@@mov r0, #0x3               @This time it is unnecessary, but when hooking from an address which can not be divided by 4,.
                             @Because we use up to 5 bytes for hook, we should consider it.
ldr r3, NoPickSkillBranch
bx r3
PickSkill:
ldr r3, PickSkillBranch
bx r3

.align
PickSkillBranch:
.long  0x8023E9C+1          @FE8U 08023E9C
@.long 0x8023E58+1          @FE8J 08023E58
NoPickSkillBranch:
.long  0x8023E94+1          @FE8U 08023E94
@.long 0x8023E50+1          @FE8J 08023E50
.ltorg
ClassList:
@list of the classes you give access to PickSkill
