.thumb

.macro blh to, reg
  ldr    \reg, =\to
  mov    lr, \reg
  .short 0xf800
.endm

.equ ReturnAddress, 0x801D1C0|1
.equ ProcStart, 0x8002C7D
.equ PlayerPhase_CancelAction, 0x801CFF1

.global CancelTradeHook
.type CancelTradeHook, %function

@Hook at 801D1B8
@r6 = ProcState | Preserve
CancelTradeHook:
    @Create proc to notify PlayerPhase_ApplyUnitMovement that movement shouldn't refresh
    ldr  r0, =CantoBugProc
    mov  r1, r6
    blh  ProcStart, r2

    @Vanilla
    End:
    mov  r0, r6
    blh  PlayerPhase_CancelAction, r1
    mov  r0, #0x1
    ldr  r3, =ReturnAddress|1
    bx   r3

.align
