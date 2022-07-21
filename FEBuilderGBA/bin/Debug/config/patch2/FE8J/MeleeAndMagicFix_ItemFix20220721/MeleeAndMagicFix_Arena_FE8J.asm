.thumb

@FE8U
@.equ origin, 0x0802A7DE	@{U}

@FE8J
.equ origin, 0x0802A76A	@{J}

strh r1, [r2, #0x0]
strh r1, [r5, #0x1E]	@add

mov r1, r9
strb r0, [r1, #0x0]
b 0x802a7b2 + . - origin	@{J}
@b 0x802A846 + . - origin	@{U}
