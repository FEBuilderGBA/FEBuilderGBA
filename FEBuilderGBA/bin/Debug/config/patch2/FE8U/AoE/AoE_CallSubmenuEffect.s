.thumb
.align


.global AoE_AreAnyUsable
.type AoE_AreAnyUsable, %function

AoE_AreAnyUsable:
push {r4,r14}

@loop through all menu command usabilities looking for one that returns true

ldr r4,=AoEMenuCommandsList
add r4,#0xC @usability of first menu option

LoopStart:
ldr r3,[r4]
cmp r3,#0
beq RetFalse

mov r0, r4
sub r0, #0xC @r0=this menu struct
mov r14,r3
.short 0xF800

cmp r0,#1
beq GoBack

add r4,#36
b LoopStart

RetFalse:
mov r0,#3

GoBack:
pop {r4}
pop {r1}
bx r1

.ltorg
.align



.equ StartMenuAdjusted,0x804EB98	@{U}
@.equ StartMenuAdjusted,0x804F924	@{J}
.global AoE_Effect
.type AoE_Effect, %function

AoE_Effect:
push {r14}

@StartMenuAdjusted takes menu definition offset in r0

ldr r0,=StartMenuAdjusted
mov r14,r0
ldr r0,=AoESubmenuDef
mov r1,#0
mov r2,#0
mov r3,#0
.short 0xF800

mov r0,#0x94		@play beep sound & end menu on next frame & clear menu graphics
pop {r1}
bx r1

.ltorg
.align


