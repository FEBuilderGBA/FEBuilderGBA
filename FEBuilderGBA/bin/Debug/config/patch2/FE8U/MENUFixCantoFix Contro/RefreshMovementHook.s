.thumb

.macro blh to, reg
  ldr    \reg, =\to
  mov    lr, \reg
  .short 0xf800
.endm

.equ ReturnAddressRefresh, 0x801D4B0|1
.equ ReturnAddressSkipRefresh, 0x801D4BA|1
.equ ProcFind, 0x8002E9D

.global RefreshMovementHook
.type RefreshMovementHook, %function

@Hook at 801D4A8
@r6 = ProcState | Preserve
RefreshMovementHook:
    @If CantoBugProc exists, we're leaving the trade menu after not trading
    ldr  r0, =CantoBugProc
    blh  ProcFind, r1
    cmp  r0, #0x0
    beq  RefreshMovement
        ldr  r3, =ReturnAddressSkipRefresh|1
        b    End

    @Vanilla behavior
    RefreshMovement:    
    ldrb r0, [r5, #0xF]
    ldr  r1, =gMapMovement
    ldr  r1, [r1]
    lsl  r0, #0x2
    add  r0, r1
    ldr  r3, =ReturnAddressRefresh|1
    b    End

    End:
    bx   r3

.align
.ltorg
