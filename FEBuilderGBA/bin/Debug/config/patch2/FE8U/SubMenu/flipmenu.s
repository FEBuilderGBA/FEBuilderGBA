.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.macro blr reg
	mov lr, \reg
	.short 0xF800
.endm

.set prShowMenu, EALiterals+0x00
.set prIsSubMenuShow, EALiterals+0x04

@@Hook 08050110	{J}
@Hook 0804F39C	{U}
ldr  r0, [r0, #0x0] @ (KeyStatusBuffer@KeyStatusBuffer.FirstTickDelay )
ldrh r1, [r0, #0x8] @ (KeyStatusBuffer@KeyStatusBuffer.NewPresses:  1 For Press, 0 Otherwise )
push {r1}
ldr r0, =0x0010 @ right
and r0 ,r1
bne FlipMenu

ldr r0, =0x0020 @ left
and r0 ,r1
bne FlipMenu
b NormalExit


FlipMenu:
ldr r0,[r4,#0x30]   @menu define
@ldr r2,=0x085C5490 @SubMenu	{J}
ldr r2,=0x0859CFB0 @SubMenu	{U}
cmp r0,r2
beq ShowUnitMenu

@ldr r2,=0x085C56D0 @UnitMenu	{J}
ldr r2,=0x0859D1F0 @UnitMenu	{U}
cmp r0,r2
beq CheckSubMenu
b NormalExit


CheckSubMenu:
ldr r3,prIsSubMenuShow
blr r3
cmp r0,#0x1
beq ShowSubMenu
b NormalExit

ShowSubMenu:
@ldr r0,=0x085C5490 @SubMenu	{J}
ldr r0,=0x0859CFB0 @SubMenu	{U}
ldr r3,prShowMenu
blr r3
b FlipMenuExit

ShowUnitMenu:
@ldr r0,=0x085C56D0 @UnitMenu	{J}
ldr r0,=0x0859D1F0 @UnitMenu	{U}
ldr r3,prShowMenu
blr r3
@b FlipMenuExit

FlipMenuExit:
pop {r1}
mov r0, #0x7
@ldr r3 ,=0x0805016C+1	@{J}
ldr r3 ,=0x0804F3F8+1	@{U}
bx  r3

NormalExit:
pop {r1}
mov r0, #0x1
and r0 ,r1
@ldr r3 ,=0x08050118+1	@{J}
ldr r3 ,=0x0804F3A4+1	@{U}
bx  r3

.ltorg
.align

EALiterals:
	@ POIN prShowMenu
	@ POIN prGetTargetPosition
