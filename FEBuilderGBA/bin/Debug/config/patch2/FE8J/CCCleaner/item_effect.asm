@thumb
;0002fbc0

    mov r0, r8
    ldr r1, =$080174E4
    mov lr, r1
    @dcw $F800
    cmp r0, #0x2E ;メティス
    beq manual
    
    mov r0, r8
    sub r0, #75	;;ライブのID
    cmp r0, #118
    bls jump
    ldr r0, =$0802fec6
    mov pc, r0
jump:
    ldr r1, =$0802fbca
    mov pc, r1
    
    
manual: ;メティス
    ldr r0, =$0802fe90
    mov pc, r0