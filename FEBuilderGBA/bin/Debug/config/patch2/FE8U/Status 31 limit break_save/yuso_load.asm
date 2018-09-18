@thumb
;;;000a7cb0 FE8J
;;;A326C ;;FE8U
    push {r4, r5, r6, r7, lr}
    @dcw $b0ac ;sub sp, #176
    @dcw $b0ac ;sub sp, #176
    
;    ldr r1, =$080a9bb5 ;FE8J
    ldr r1, =$080A519D ;FE8U
    mov r0, lr
    cmp r0, r1
    beq normal
    mov r0, #0
    b merge
    
normal:
    mov r0, r9
    add r0, #1
merge:
    ldr r1, =0x160
    mul r0, r1
    ldr r1, =$0E007400
    add r0, r0, r1
    
;    ldr r1, =$080a7d24	;FE8U
    ldr r1, =$080A32E0	;FE8J
    ldr r1, [r1]
    ldr r3, [r1]
    mov r1, sp
;        ldr r2, =$080d65c8 ;FE8J
         ldr r2, =$080D18CC ;FE8U
        mov lr, r2
    mov r2, #176
    lsl r2, r2, #1
    @dcw $F800
    
;        ldr r0, =$0803144c ;=輸送隊のベースアドレスロード FE8J
         ldr r0, =$08031500 ;=輸送隊のベースアドレスロード FE8U
        mov lr, r0
    @dcw $F800
    mov r4, r0
    @dcw $AD19
    add r5, #100
;    ldr r1, =$080a7cc8	;FE8J
	ldr r1, =$080A3284	;FE8U
    mov pc, r1