.thumb

.macro blh to, reg=r3
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

@Call 0808C0F8	{J}
@Call 08089DE8	{U}

push {r4,lr}
ldr  r1, =0x0203EF60 @gSaveMenuRTextData	{J}
@ldr  r1, =0x0203EF64 @gSaveMenuRTextData	{U}
ldrb r0, [r1, #0x2]   @gSaveMenuRTextData.CurrentWorldmapNodeID 
                      @難易度のモードを取得する.
bl  GetTextData
cmp  r0, #0x0
beq  ShowBar
mov  r4, r0

ldrh r0,[r4,#0x0] @TextID
blh  0x08009fa8   @GetStringFromIndex	@{J}
@blh  0x0800A240   @GetStringFromIndex	@{U}

ldrb r2,[r4,#0x2] @Color
b    Show

ShowBar:
mov r0, #0x99			@{J}	@----
lsl r0 ,r0 ,#0x3		@{J}

@mov r0, #0xa7			@{U}	@----
@lsl r0 ,r0 ,#0x3		@{U}

blh  0x08009fa8   @GetStringFromIndex	@{J}
@blh  0x0800A240   @GetStringFromIndex	@{U}
mov r2, #0x0     @white color

Show:
mov r3, r0   @TextRAWData
ldr r0 ,=0x0203E7A8		@{J}
@ldr r0 ,=0x0203E7AC		@{U}
mov r1, #0x1d @Shift
@r2=color
blh 0x080043b8,r4   @Text_InsertString	@{J}
@blh 0x08004480,r4   @Text_InsertString	@{U}

pop {r4}
pop {r0}
bx r0

@難易度の文字列の位置を取得する
@W0 Text
@B2 Color
@B3 Color
@
GetTextData:
push {lr}

cmp r0,#0x6
bge GetTextData_Error

ldr r1, Table
lsl r0, #0x2	@r0 * 4
add r0, r1
b   GetTextData_Exit

GetTextData_Error:
mov r0, #0x0

GetTextData_Exit:
pop {r1}
bx r1

.ltorg
Table:
