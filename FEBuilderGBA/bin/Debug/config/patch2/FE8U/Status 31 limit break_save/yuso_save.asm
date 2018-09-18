@thumb
;000a7c28 FE8J
;A31E4    FE8U
    push {r4, r5, r6, r7, lr}
    mov r7, r8
    push {r7}
    @dcw $b0ac ;sub sp, #176
    @dcw $b0ac ;sub sp, #176
    
    mov r0, lr
;    ldr r1, =$080a9abd ;FE8J
    ldr r1,  =$080A50A5  ;FE8U
    cmp r0, r1
    beq normal
;    ldr r1, =$080a99c9 ;FE8J
    ldr r1,  =$080A4F8D  ;FE8U
    cmp r0, r1
    beq start
    
    mov r0, #0
    b merge
start:
    mov r0, r9
    add r0, #1
    b merge
normal:
    mov r0, r10
    add r0, #1
merge:
    ldr r1, =0x160
    mul r0, r1
    ldr r1, =$0E007400
    add r0, r0, r1
    
    mov r8, r0
;        ldr r0, =$0803144c ;=輸送隊のベースアドレスロード FE8J
        ldr r0, =$08031500 ;=輸送隊のベースアドレスロード FE8U
        mov lr, r0
    @dcw $F800
    mov r6, r0
    @dcw $AD19
    add r5, #100
;    ldr r0, =$080a7c3a ;FE8J
    ldr r0, =$080A31F6  ;FE8U
    mov pc, r0