@ Hooked at 0x6C70C.
@ Nulls [DamageMoji+0x60] pointer to subAnimeEmulator
@ procstate when subAnimeEmulator proc gets killed.
@ BAN_KillDigits uses this NULLed pointer so it won't
@ kill [DamageMoji+0x60] which can otherwise be a
@ re-allocated procstate (an unrelated procstate).
.thumb

@ Vanilla, overwritten by hook.
@ Deletes the subAnimeEmulator proc.
ldr   r3, =0x8002D6C|1 @EndProc	@{U}
@ldr   r3, =0x8002CBC|1 @EndProc	@{J}
bl    GOTO_R3

mov   r0, #0x0
str   r0, [r4, #0x60]         @ NULL [DamageMoji+0x60], pointer to subAnimeEmulator procstate.

@ Vanilla, overwritten by hook.
mov   r0, r4
ldr   r3, =0x08002e94|1	@BreakProcLoop	@{U}
@ldr   r3, =0x08002de4|1	@BreakProcLoop	@{J}
bl    GOTO_R3

ldr   r3, =0x806C716|1	@{U}
@ldr   r3, =0x806ea3a|1	@{J}
GOTO_R3:
bx    r3
