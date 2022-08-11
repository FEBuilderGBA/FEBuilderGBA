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
                @r4�́Athis procs���w���O���[�o���ϐ��Ƃ��ė��p����

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

@��\��
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
ldr  r0, [r3, #0x10]	@ ArenaDicConfig->�A�j���[�V����1
b    GetRepeat_AnimationAddress_Exit

GetAnimationData_Anime2:
ldr  r3, ArenaDicConfig
ldr  r0, [r3, #0x14]	@ ArenaDicConfig->�A�j���[�V����2
b    GetRepeat_AnimationAddress_Exit

GetAnimationData_Anime3:
ldr  r3, ArenaDicConfig
ldr  r0, [r3, #0x18]	@ ArenaDicConfig->�A�j���[�V����3
@b    GetRepeat_AnimationAddress_Exit

GetRepeat_AnimationAddress_Exit:
pop {r1}
bx r1

.ltorg
DATA:
.equ	ArenaDicConfig,	DATA+0
