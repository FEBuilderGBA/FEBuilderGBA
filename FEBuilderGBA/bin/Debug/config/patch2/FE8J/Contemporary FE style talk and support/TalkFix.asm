.thumb
.org 0

ldr r5,[sp]       @pointer to "next routine" - change this to take us back to unit menu
mov r2,#0x17
strb r2,[r4,#0x11] @change action to "bought from armory" - does not actually affect the Armory command so this is safe
ldr r4, [r5,#4]
sub r4, #0x30
str r4, [r5,#4]
ldr r4,=0x202bce9 @mark unit has "moved"
mov r5,#80      @unknown, but likely unused
strb r5,[r4]
ldr r4,=0x8086288
mov lr,r4
.short 0xf800

@Rerun the original routine that will break.
mov r0, #0x0
pop {r4,r5}
pop {r1}
bx r1
