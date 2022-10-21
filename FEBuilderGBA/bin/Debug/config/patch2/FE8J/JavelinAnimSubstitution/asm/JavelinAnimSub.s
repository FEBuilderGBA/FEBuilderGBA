.thumb

.equ StructSize, 0x4
.equ ReturnAddress, 0x8058fde|1	@{J}

/*
{
00 ClassID
02 ReplacementAnimID
}
*/

@r2 = Current AnimID, r4 = class ID
JavelinAnimSubsititute:
    ldr  r3, JavelinAnimSubList
    Loop:
    ldrb r0, [r3]
    @Check at end of list
    cmp  r0, #0x0
    beq  End
    @Check ClassID
    cmp  r0, r4
    beq  MatchFound
    add  r3, #StructSize
    b    Loop

    MatchFound:
    ldrh r2, [r3, #0x2]

    End:
    ldr r3, =ReturnAddress
    bx  r3

.align
.ltorg

JavelinAnimSubList:
@POIN JavelinAnimSubList
