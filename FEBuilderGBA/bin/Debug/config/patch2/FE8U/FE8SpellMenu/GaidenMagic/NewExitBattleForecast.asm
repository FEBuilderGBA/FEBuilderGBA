.thumb

@hook this at 0859b654

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.equ AttackCommandEffect, 0x08022B30
.equ SelectedSpellPointer, MagicUMCommandEffect+4

@08022E54 B500   PUSH {lr}
@08022E56 2000   MOV r0, #0x0
@08022E58 2100   MOV r1, #0x0
@08022E5A F7FF FE69   BL 0x08022B30   //AttackCommandEffect 
@08022E5E BC01   POP {r0}
@08022E60 4700   BX r0

push {r3, lr}

ldr r3, SelectedSpellPointer
ldr r0, [r3]
mov r1, #0x0
str r1, [r3, #0x0]		@zero out the selected spell pointer on exit regardless of usecase
cmp r0, #0x0
beq NotMagic

mov r0, #0x0
ldr r3, MagicUMCommandEffect
mov lr, r3
.short 0xf800
b ExitFunc

NotMagic:
mov r0, #0x0
blh AttackCommandEffect

ExitFunc:
pop {r3}
pop {r0}
bx r0

.ltorg
.align

MagicUMCommandEffect:
@POIN MagicUMCommandEffect
@WORD SelectedSpellPointer
