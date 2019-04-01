.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@r0 item & count << 8
@r1 work
@r2 work
@r3 work
@
@
@struct GetItemRangeString_Table
@{
@	byte	range;		射程
@	byte	_00;
@	uhsort	text;		文字列	00=term
@}//sizeof(4bytes)
@

push {lr}

blh 0x08017448 @GetItemRange

ldr r3,GetItemRangeString_Table
sub r3,r3,#0x04
Loop:
add	r3,r3,#0x04

ldr r1,[r3,#0x0]
cmp r1,#0x0
beq   NOTFOUND  @term 終端

ldrb r1,[r3,#0x0]
cmp r0,r1
bne Loop

FOUND:
ldrh r0,[r3,#0x2]
b    GetString

NOTFOUND:
ldr  r0,=0x4b7

GetString:
blh 0x08009FA8 @GetStringFromIndex

pop {r1}
bx r1

.ltorg
.align
GetItemRangeString_Table:
@POIN GetItemRangeString_Table
