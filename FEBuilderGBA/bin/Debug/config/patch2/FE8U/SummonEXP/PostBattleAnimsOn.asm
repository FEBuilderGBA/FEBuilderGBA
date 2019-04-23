
.global SetUpSummonProcAnimsOn
.type SetUpSummonProcAnimsOn, %function
SetUpSummonProcAnimsOn: @ Autohook to 0x0807357C
@ Now to check if this is a phantom. r5 has phantom's character struct (and also r0)
ldr r1, [ r5, #0x04 ]
ldrb r1, [ r1, #0x04 ] @ Class ID
ldr r0, =PhantomIDSummonASM
ldrb r0, [ r0 ]
cmp r0, r1
bne EndProcAnimsOn @ If this isn't a phantom, end.
@ So we have a phantom. Now to insert the character struct of the summoner.
mov r0, r5
bl FindSummoner @ r0 = Summoner's character struct.
mov r5, r0
@ Now to do all that vanilla stat-getting crap.
EndProcAnimsOn:
ldr r1, =#0x02020110
mov r3, r4
add r3, #0x70
mov r0, #0x00
ldsb r0, [ r3, r0 ]
strh r0, [ r1 ]
ldr r2, =#0x02020114
mov r0, #0x12
ldsb r0, [ r5, r0 ]
strh r0, [ r2 ]
ldr r1, =#0x08073591
bx r1

.global FixSummonAnimsOnLevelUpPortrait
.type FixSummonAnimsOnLevelUpPortrait, %function
FixSummonAnimsOnLevelUpPortrait: @ Autohook to 0x08073DBC
@ r1 has the battle struct
ldr r2, [ r1, #0x04 ]
ldrb r2, [ r2, #0x04 ] @ Class ID
ldr r0, =PhantomIDSummonASM
ldrb r0, [ r0 ]
cmp r0, r2
bne EndPortraitAnimsOn

@ So if I'm here, I have a phantom. Use the summoner's character struct to get the portrait ID.
mov r0, r1
bl FindSummoner @ r0 = Summoner's character struct.
mov r1, r0

EndPortraitAnimsOn:
ldr r0, [ r1 ]
ldrh r4, [ r0, #0x06 ]
ldr r0, =#0x087592CC
blh #0x08005544, r2
ldr r0, =#0x1042
str r0, [ sp ]
mov r0, #0x00
mov r1, r4
mov r2, #0xBC
ldr r3, =#0x0800563C
mov lr, r3
mov r3, #0x50
.short 0xF800 @ Ew can't use the blh macro because all scratch registers are being used. Grody.
ldr r0, =#0x08073DCF
bx r0

.global FixSummonAnimsOnClassText
.type FixSummonAnimsOnClassText, %function
FixSummonAnimsOnClassText: @ Autohook to 0x08073808
ldr r0, [ r0 ] @ Character struct in r0
ldr r1, [ r0, #0x04 ]
ldrb r1, [ r1, #0x04 ] @ Class ID
ldr r2, =PhantomIDSummonASM
ldrb r2, [ r2 ]
cmp r1, r2
bne EndTextClassOn
@ If I'm here, I have a phantom yadda yadda
bl FindSummoner @ r0 = Summoner's character struct.

EndTextClassOn:
ldr r0, [ r0, #0x04 ]
ldrh r0, [ r0 ]
blh #0x0800A240, r1
mov r1, r0
mov r0, r4
blh #0x08004004, r2
mov r0, #0xE0
ldr r1, =#0x0807381D
bx r1
