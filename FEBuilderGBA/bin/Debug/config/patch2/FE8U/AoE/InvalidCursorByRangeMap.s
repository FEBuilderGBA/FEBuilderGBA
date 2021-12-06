.equ ppRangeMapRows, 0x0202E4E4	@{U}
@.equ ppRangeMapRows, 0x0202E4E0	@{J}

.thumb

    push {lr}  

    @check to make sure tile is selectable
    ldr     r0, =ppRangeMapRows
    lsl     r3, r2, #0x2
    ldr     r0, [r0]
    ldr     r0, [r0, r3]
    ldrb    r0, [r0, r1]

    cmp     r0, #0
    beq Invalid 
    
    mov r0, r1 @ r0 - xCoord
    mov r1, r2 @ r1 - yCoord
    
    @blh CheckFunc
    ldr r3, CheckFunc
    mov lr, r3
    .short 0xF800
    
    cmp r0, #0
    beq Invalid
    
    mov r0, #0x20 @ (0x20 = Set Valid Cursor)
    b End
    
Invalid:
    mov r0, #0x40 @ (0x40 = Set Invalid Cursor)

End:
    pop {r1}
    bx r1

.ltorg
.align

CheckFunc:
@POIN CheckFunc
