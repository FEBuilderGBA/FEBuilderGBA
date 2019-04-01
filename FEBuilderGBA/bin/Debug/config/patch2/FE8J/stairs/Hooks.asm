
.macro SET_FUNC name, value
    .global \name
    .type   \name, %function
    .set    \name, \value
.endm

SET_FUNC FixWait2, ( 0x080184AE +1 )

SET_FUNC MoveDebuff, ( 0x0801C7D8 +1 )
