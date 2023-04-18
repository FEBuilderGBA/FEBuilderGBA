@ Draws a textbubble when no portraits and no named textbox is present.
@ Hooked at 0x8F668.
.thumb

@ Vanilla overwritten by hook.
lsl   r1, #0x9
and   r1, r0          @ r0 contains which flag we need to check to see if textbox is named.
cmp   r1, #0x0
bne   NamedTextBox

@ No name, draw textbubble.
@ Mostly copied over from 8F670 up to the first ObjInsert (0x8005428 call).
mov   r1, r6
sub   r1, #0x10
ldr   r5, =0x1FF
and   r1, r5
mov   r2, r7
sub   r2, #0x18
mov   r4, #0xFF
and   r2, r4
ldr   r3, =CGC_NoNameBubbleSpriteOAM
ldr   r0, =0x13C8
str   r0, [sp]
ldr   r0, =ObjInsert
mov   r12, r0
mov   r0, #0x0
bl    GOTO_R12

@ Skip over NamedTextBox draw routines.
ldr   r3, =0x808F6A7
bx    r3

NamedTextBox:
ldr   r3, =0x808F671
bx    r3

GOTO_R12:
bx    r12
