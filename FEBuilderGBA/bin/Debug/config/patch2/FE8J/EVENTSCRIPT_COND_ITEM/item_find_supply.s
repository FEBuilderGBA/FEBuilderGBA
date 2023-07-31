.align 4
.macro blh to, reg=r3
ldr \reg, =\to
mov lr, \reg
.short 0xf800
.endm

@.equ GetConvoyItemArray, 0x8031500|1	@FE8U
.equ GetConvoyItemArray, 0x803144C|1	@FE8J
@.equ GetConvoyItemCount, 0x8031570|1	@FE8U
.equ GetConvoyItemCount, 0x080314BC|1	@FE8J

.thumb
    push    {r4,r5,r6,r7, lr}

@    ldr    r0,=0x30004B8    @FE8U MemorySlot0
    ldr    r0,=0x30004B0    @FE8J MemorySlot0
    ldrb    r7,[r0,#0x4 * 0x1]    @MemorySlot1 = item ID to search in convoy
    
    blh    GetConvoyItemArray
    mov    r5, r0

    blh    GetConvoyItemCount
    mov     r6, r0

    mov        r1,#0x0        @current item

convoy_loop:
    ldrb    r0,[r5,#0x0] 
    add        r1,#0x1
    add        r5,#0x2        

    cmp        r0,r7
    beq        found

    cmp        r0,#0x00     
    beq        convoy_end

    cmp        r1,r6
    blt        convoy_loop        

convoy_end:
    mov        r0, #0x0        
    b         make_result

found:
    mov        r0, #0x1        
    b        make_result

make_result:
@    ldr    r2,=0x30004B8    @FE8U MemorySlot0
    ldr    r2,=0x30004B0    @FE8J MemorySlot0
    str    r0, [r2, #0x4 * 0xC]

    pop    {pc,r7, r6 , r5 ,r4 }

.ltorg
.align 4
