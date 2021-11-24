@名前を設定する

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.org 0

PUSH {r4,r5,r6,lr}
MOV r6 ,r0 @ parent proc? 
MOV r4 ,r1

MOV r0 ,r4
ADD r0, #0x34
LDR r1, [r4, #0x30]
LDRB r1, [r1, #0x8] @メニューの色
@BLH 0x08003D90 @{J} Text_SetColorId
BLH 0x08003E60   @{U} Text_SetColorId

MOV r0 ,r4
ADD r0, #0x3D
LDRB r0, [r0, #0x0]
MOV r5 ,r4
ADD r5, #0x34
CMP r0, #0x2
BNE Skip2
    MOV r0 ,r5
    MOV r1, #0x1
@    BLH 0x08003D90 @{J} Text_SetColorId
    BLH 0x08003E60   @{U} Text_SetColorId
Skip2:

mov r0, r4 
bl get_menu_name
mov r1,r0
cmp r1, #0x00
bne DrawString

@LDR r0, [r4, #0x30] @{J}
@LDR r1, [r0, #0x0]  @{J}

LDR r0, [r4, #0x30] @{U}
LDRH r0, [r0, #0x4] @{U}
BLH 0x0800A240      @{U} GetStringFromIndex
MOV r1 ,r0          @{U}

DrawString:
MOV r0 ,r5
@BLH 0x08003F28      @{J} Text_AppendString
BLH 0x08004004      @{U} Text_AppendString


MOV r0 ,r6
ADD r0, #0x64
LDRB r0, [r0, #0x0]
LSL r0 ,r0 ,#0x1C
LSR r0 ,r0 ,#0x1E
@BLH 0x08001BC0     @{J} BG_GetMapBuffer
BLH 0x08001C4C     @{U} BG_GetMapBuffer
MOV r1 ,r0
MOV r2, #0x2C
LDSH r0, [r4, r2]
LSL r0 ,r0 ,#0x5
MOV r3, #0x2A
LDSH r2, [r4, r3]
ADD r0 ,r0, R2
LSL r0 ,r0 ,#0x1
ADD r1 ,r1, R0
MOV r0 ,r5
@BLH 0x08003DA0      @{J}	Text_Draw
BLH 0x08003E70      @{U} Text_Draw
POP {r4,r5,r6}
POP {r1}
BX r1

.align 


get_menu_name:
PUSH {r4,r5,lr}


ldr r3, ItemSpecialEffectCallUsability 
mov lr, r3 
.short 0xF800 
@mov r4, r1 @ Table 


add r1, #6 @ String to use 
ldrh r0, [r1]
cmp r0,#0x0
beq Break

@BLH 0x08009fa8       @{J} GetStringFromIndex
BLH 0x0800A240      @{U} GetStringFromIndex
b Exit

Break:
MOV r0, #0x0

Exit:

POP {r4,r5}
POP {r1}
bx r1

.ltorg
.align 
ItemSpecialEffectCallUsability:
