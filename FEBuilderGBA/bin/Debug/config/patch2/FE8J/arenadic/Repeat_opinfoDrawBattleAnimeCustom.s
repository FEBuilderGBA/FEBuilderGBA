.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.macro blh_ to, reg=r3
  push {\reg}
  ldr \reg, =\to
  mov lr, \reg
  pop {\reg}
  .short 0xf800
.endm

push {r4,lr}
mov r4, r0		@this procs
                @r4は、this procsを指すグローバル変数として利用する

bl GetRepeat_AnimationAddress
cmp r0, #0x0
beq Exit

str r0, [r4, #0x38]	@AnimationData

mov r0, #0x0
strh r0, [r4, #0x2a]


mov r0 ,r4
mov r1, #0x9
blh 0x08002e74   @Goto6CLabel

Exit:
pop {r4}
pop {r0}
bx r0

GetRepeat_AnimationAddress:
@r4 this
push {lr}
ldr r0, [r4, #0x30] @ ParentProcs
mov  r1, #0x29
ldrb r0, [r0, r1]	@thisProcs->isShowData
cmp r0, #0x0
bne GetRepeat_AnimationAddress_Repeat

@非表示
mov  r0, #0x0
b    GetRepeat_AnimationAddress_Exit

GetRepeat_AnimationAddress_Repeat:
ldr r3, [r4, #0x34] @ ArenaDicStruct
ldrb r0, [r3, #0x13] @ ArenaDicStruct->Anime
cmp r0, #0x01
beq GetAnimationData_Anime2
cmp r0, #0x02
beq GetAnimationData_Anime3

GetAnimationData_Anime1:
ldr  r3, ArenaDicConfig
ldr  r0, [r3, #0x10]	@ ArenaDicConfig->アニメーション1
b    GetRepeat_AnimationAddress_Exit

GetAnimationData_Anime2:
ldr  r3, ArenaDicConfig
ldr  r0, [r3, #0x14]	@ ArenaDicConfig->アニメーション2
b    GetRepeat_AnimationAddress_Exit

GetAnimationData_Anime3:
ldr  r3, ArenaDicConfig
ldr  r0, [r3, #0x18]	@ ArenaDicConfig->アニメーション3
@b    GetRepeat_AnimationAddress_Exit

GetRepeat_AnimationAddress_Exit:
pop {r1}
bx r1

.ltorg
DATA:
.equ	ArenaDicConfig,	DATA+0
