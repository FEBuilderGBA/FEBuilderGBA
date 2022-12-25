.thumb

@Hook 0802C090	@{J}
@Hook 0802C148	@{U}

ldr r3, LimitWeaponLevel
cmp r6, r3
blt AddWeaponLevel

@ldr r3, =0x0202BCEC	@ChapterInfo @{J}
ldr r3, =0x0202BCF0	@ChapterInfo @{U}
ldrb r3, [r3, #0xF] @phase
cmp r3, #0x00
bne Exit

AddWeaponLevel:
add r6 ,r6, r0

Exit:
@ldr r3, =0x0802C098|1	@{J}
ldr r3, =0x0802C150|1	@{U}
mov lr, r3

mov r1, #0x0
ldrb r3, [r5, #0x0]
ldr r2, [r7, #0x4]

bx  lr

.ltorg
LimitWeaponLevel:
