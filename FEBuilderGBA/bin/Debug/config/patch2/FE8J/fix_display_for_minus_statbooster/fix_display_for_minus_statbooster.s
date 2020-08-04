@マイナスのstatboosterを作ったときの表示の修正
@Hook 08004A5C {J}
@Hook 08004B54 {U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

cmp r4, #0xA
bge Start
cmp r6, #0x0
bne Start

	@Draw space
    mov r2 ,#0xff  @space
    mov r0 ,r5
    sub r0 ,#0x02  @2tile back
    mov r1 ,r7
    blh 0x08004a14   @DrawSpecialUiChar	{J}
@    blh 0x08004b0c   @DrawSpecialUiChar	{U}

Start:

@壊すコードの再送
cmp r4, #0x0
bne NormalExit
    mov r0 ,r5
    mov r1 ,r7
    mov r2 ,r6	@<-0
    blh 0x08004a14   @DrawSpecialUiChar	{J}
@    blh 0x08004b0c   @DrawSpecialUiChar	{U}
    
    ldr r3, =0x8004a8a|1	@{J}
@    ldr r3, =0x8004b82|1	@{U}
    bx  r3

NormalExit:
    ldr r3, =0x8004A66|1	@{J}
@    ldr r3, =0x08004B5E|1	@{U}
    bx  r3
