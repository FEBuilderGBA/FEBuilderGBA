
.macro SET_FUNC name, value
    .global \name
    .type   \name, %function
    .set    \name, \value
.endm

SET_FUNC FixWait2, ( 0x08018244 +1 ) @ GOOD FOR FE7

SET_FUNC MoveDebuff, ( 0x0801C4D0 +1 ) @ GOOD FOR FE7
