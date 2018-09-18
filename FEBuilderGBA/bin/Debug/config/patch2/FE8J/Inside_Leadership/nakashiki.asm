@thumb
    ldr	r0, =$02003f06;;;;;;;;;数字位置
        @align 4
        ldr r1, =$0802A96C
        ldr r1, [r1]
        mov lr, r1
        @dcw $F800
    ldr r0, =$080896d8
    mov pc, r0