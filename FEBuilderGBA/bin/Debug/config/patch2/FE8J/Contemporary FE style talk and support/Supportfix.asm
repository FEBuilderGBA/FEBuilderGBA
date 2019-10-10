.thumb
.org 0

push {r4-r5}

ldr r4,=0x203A954
mov r5,#0x17        @(bought from armory - guaranteed safe!)
strb r5,[r4,#0x11]
ldr r4,[sp,#0x10]   @pointer to "next routine" - we change this to take us back into the unit menu
ldr r5, [r4,#4]
sub r5, #0x30
str r5, [r4,#4]
ldr r4,=0x202bce9   @mark unit has "moved"
mov r5,#80          @unknown, but likely unused
strb r5,[r4]
ldr r4,=0x8085A54
mov lr,r4
.short 0xf800
pop {r4,r5}

@Rerun the original routine that will break.
mov r2, r9
ldr r0, [r2, #0x0] @ pointer:03004E50 (Pointer to the work memory of the operation character)



ldr r1,=0x0803238C+1
bx  r1
