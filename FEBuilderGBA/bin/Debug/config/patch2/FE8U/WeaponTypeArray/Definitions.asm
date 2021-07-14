
.macro SET_FUNC name, value
    .global \name
    .type   \name, %function
    .set    \name, \value
.endm

SET_FUNC WeaponTypeArray, (0x0802C7C0+1)
