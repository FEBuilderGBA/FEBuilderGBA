@thumb
    ;0x029ea8
    push {r0}
        ldr r1, =$080174E4
        mov lr, r1
        @dcw $F800
    mov r1, r0
    pop {r0}
    
    cmp r1, #0x2E ;メティス効果
    beq metis
;通常処理
    sub r0, #91
    cmp r0, #102
    bls jump
    ldr r1, =$0802a08c
    mov pc, r1
jump:
    ldr r1, =$08029eb0
    mov pc, r1
    
;メティス
metis:
    ldr r1, =$0802a070
    mov pc, r1
    
    