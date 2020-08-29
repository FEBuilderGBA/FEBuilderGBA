.thumb

.global WRankTBuffer
.type WRankTBuffer, %function

.macro SET_FUNC name, value
    .global \name
    .type   \name, %function
    .set    \name, \value
.endm

SET_FUNC GetStringFromIndex, (0x0800A240+1)
SET_FUNC ItemD, (0x08809B10)
SET_FUNC BufferInfo, (0x003D3C00)

.macro blh to, reg
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

push {r4,r5,lr}   //GetWRankTextBuffer
sub sp, #0x58
mov r2, sp
ldr r1,=WRItemText @Gets the rank text for the item definition
ldmia r1,{r3,r4,r5} 
stmia r2,{r3,r4,r5}
ldmia r1,{r3,r4,r5} 
stmia r2,{r3,r4,r5}
ldmia r1,{r3,r4,r5} 
stmia r2,{r3,r4,r5}
ldmia r1,{r3,r4,r5}
stmia r2,{r3,r4,r5}
ldmia r1,{r3,r4,r5}
stmia r2,{r3,r4,r5}
ldmia r1,{r3}
stmia r2,{r3}
mov r1, #0xff
and r0 ,r1
lsl r1 ,r0 ,#0x3
add r1 ,r1, r0
lsl r1 ,r1 ,#0x2
ldr r0,=ItemD @Gets the item information
add r1 ,r1, r0
ldrb r4, [r1, #0x1c]
ldr r0, [r1, #0x8]
ldr r1,=BufferInfo
and r0 ,r1
cmp r0, #0x0
beq WRTB1 	
    mov r0 ,r4
    bl WeaponLevel 	
    cmp r0, #0x0
    bne WRTB1 	
        mov r4, #0xf
        b WRTB2 	

WRTB1:
mov r0 ,r4
bl WeaponLevel 
mov r4 ,r0

WRTB2:
lsl r0 ,r4 ,#0x2
add r0, sp
ldr r0, [r0, #0x0]
blh GetStringFromIndex, r1   //GetStringFromIndex
add sp, #0x58
pop {r4,r5}
pop {r1}
bx r1