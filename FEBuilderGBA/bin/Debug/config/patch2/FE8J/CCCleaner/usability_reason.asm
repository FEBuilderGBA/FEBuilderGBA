@thumb
    ;28bc2
    push {r0}
        ldr r1, =$080174E4
        mov lr, r1
        @dcw $F800
    mov r1, r0
    pop {r0}
    
    cmp r1, #0x2E ;メティス効果
    beq metis
;通常処理
    sub r0, #86
    cmp r0, #107
    bls jump
    ldr r1, =$08028e00
    mov pc, r1
jump:
    lsl r0, r0, #2
    ldr r1, =$08028bcc
    mov pc, r1
    
;メティス
metis:
    ldr r1, =$08028e00
    mov pc, r1
    
    