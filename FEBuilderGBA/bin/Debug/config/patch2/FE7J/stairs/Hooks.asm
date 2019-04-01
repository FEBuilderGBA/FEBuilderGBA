
.macro SET_FUNC name, value
    .global \name
    .type   \name, %function
    .set    \name, \value
.endm

SET_FUNC FixWait2, ( 0x08018634 +1 ) @ GOOD FOR FE7

SET_FUNC MoveDebuff, ( 0x0801C8D4 +1 ) @ GOOD FOR FE7
