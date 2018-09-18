.thumb
;//@hook at 8058290		;//FE8U
@hook at 80590DC		;//FE8J

bl LoopParent
cmp r0, #1 @replace this with the check from the table
bne NotRS
cmp r6, #0x0
bne NotRS
  mov r5, #1
  str r5, [sp, #0x14]

NotRS:
ldr r4, [sp, #0x8]
add r4, #0x4a
ldrh r0, [r4]
;//ldr r1, =0x80174ec	;//FE8U
ldr r1, =0x08017294		;//FE8J
mov lr, r1
.short 0xf800
mov r5, r4

bl LoopParent
cmp r0, #1 @same here
bne StillNotRS
ldr r0, [sp, #0x18]
cmp r0, #0
bne StillNotRS
  mov r1, #1
  str r1, [sp, #0x18]

StillNotRS:
ldr r4, [sp, #4]
Return:
;//ldr r1, =0x8058332|1	;//FE8U
ldr r1, =0x0805917E|1	;//FE8J
bx r1

LoopParent:
ldr r1, list
@ ldr r1, [r1]
LoopStart:
ldrb r2, [r1]
cmp r2, #0
beq False
cmp r2, r0
beq True
add r1, #1
b LoopStart

True:
mov r0, #1
b End
False:
mov r0, #0
End:
bx lr

.ltorg
list:
@POIN list
