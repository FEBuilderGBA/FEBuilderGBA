.thumb

push {r4,r5,lr}
ldr r4, NewFunc
bx r4

.ltorg
.align

NewFunc:
@POIN NewFunc
