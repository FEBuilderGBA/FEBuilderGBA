@ Hooked at 0x552D0. 
@ Kills all other DamageMoji procs and their subAnimeEmulator procs.
@ Also delays NODAMGEMIS! symbols being put in VRAM if miss or no damage.
@   r4: 1 if miss. 0 if hit/nodmg. Anything else is ignored.
@   r5: AIS.
@   r6: Free.
@   r7: AIS.
@   r8: AIS.
@   r9: AIS.
@   r10: Some arg.
.thumb


@ Vanilla, overwritten by hook.
mov   r8, r0


@ Kill existing DamageMoji procs.
Loop:
  ldr   r0, =0x85D8D5C @gProc_efxDamageMojiEffectOBJ	@{U}
  @ldr   r0, =0x086033AC	@gProc_efxDamageMojiEffectOBJ	@{J}
  ldr   r3, =0x8002E9C|1 @ProcFind	@{U}
  @ldr   r3, =0x8002DEC|1 @ProcFind	@{J}
  bl    GOTO_R3
  cmp   r0, #0x0
  beq   EndLoop
    ldr   r6, [r0, #0x60]
    ldr   r3, =0x8002D6C|1 @EndProc	@{U}
    @ldr   r3, =0x8002CBC|1 @EndProc	@{J}
    bl    GOTO_R3
    cmp   r6, #0x0
    beq   Loop              @ Skip if subAnimeEmulator proc* is NULL.
      
      @ Kill subAnimeEmulator proc.
      mov   r0, r6
      ldr   r3, =0x8002D6C|1 @EndProc	@{U}
      @ldr   r3, =0x8002CBC|1 @EndProc	@{J}
      bl    GOTO_R3
      b     Loop
EndLoop:


@ Vanilla, overwritten by hook.
ldr   r3, =0x80552E4|1	@{U}
@ldr   r3, =0x805628C|1	@{J}
cmp   r4, #0x0
beq   GOTO_R3
  ldr   r3, =0x80552DC|1	@{U}
  @ldr   r3, =0x8056284|1	@{J}
  cmp   r4, #0x1
  bne   GOTO_R3
    ldr   r3, =0x805540E|1	@{U}
    @ldr   r3, =0x80563AA|1	@{J}
GOTO_R3:
bx    r3
