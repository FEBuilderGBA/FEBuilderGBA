.thumb

@FE8U
@.equ origin, 0x0802A7DE	@{U}

@FE8J
@.equ origin, 0x0802A76A	@{J}

@FE7U
.equ origin, 0x08028842	@{U}

@FE7J
@.equ origin, 0x08028CBE	@{J}


strh r1, [r2, #0x0]
strh r1, [r5, #0x1E]	@add

mov r1, r9
strb r0, [r1, #0x0]
@b 0x802a7b2 + . - origin	@FE8J @{J}
@b 0x802A846 + . - origin	@FE8U @{U}
@b 0x8028d26 + . - origin	@FE7J @{J}
b 0x80288aa + . - origin	@FE7U @{U}
