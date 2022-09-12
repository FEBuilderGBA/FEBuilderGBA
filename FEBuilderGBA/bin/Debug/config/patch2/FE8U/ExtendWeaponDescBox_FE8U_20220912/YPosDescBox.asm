.thumb 
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ GetValueFromEasingFunction, 0x8012DCC	@{U}
@.equ GetValueFromEasingFunction, 0x08012E84	@{J}
push {r4, lr} 

sub sp, #4 
str r0, [sp] 
mov r0, r5 
mov r3, r12 
blh GetValueFromEasingFunction, r4 
cmp r0, #0 
bge NoCap 
mov r0, #0x0
NoCap: 

strh r0, [r6, #0x32] 

add sp, #4 

pop {r4} 
pop {r0} 
bx r0 
.ltorg 

