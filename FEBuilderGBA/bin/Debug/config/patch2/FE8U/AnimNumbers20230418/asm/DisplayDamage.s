@ Start Damage/Heal numbers animations. Args:
@   r0: AIS.
@   r1: 0 if OverDamage or OverHeal (recipient). 1 otherwise.
@   r2: X of previous damage display. 0 if there is none.
@   r3: Digitcount of previous damage display. 0 if there is none.
@ Return:
@   Digitcount of new damage display.
.thumb

push  {r4-r7, r14}
sub   sp, #0x8
mov   r4, r0
mov   r5, r1
str   r2, [sp]
str   r3, [sp, #0x4]


ldr   r0, =BATTLE_ANIMATION_NUMBERS_FLAG
lsl   r0, #0x5
lsr   r0, #0x5
ldr   r3, =0x8083DA8|1 @CheckEventId	@{U}
@ldr   r3, =0x80860D0|1 @CheckEventId	@{J}
bl    GOTO_R3
cmp   r0, #0x0
bne   End

  @ Flag unset, display damage numbers.
  @ Recipient's AIS might still be finishing up their round,
  @ so we grab the highest round.
  mov   r0, r4
  ldr   r3, =0x805A2B4|1	@GetOpponentFrontAIS	@{U}
  @ldr   r3, =0x805B058|1	@GetOpponentFrontAIS	@{J}
  bl    GOTO_R3
  ldrh  r0, [r0, #0xE]
  ldrh  r1, [r4, #0xE]
  cmp   r0, r1
  bge   Max
    mov   r0, r1
  Max:
  
  sub   r0, #0x1
  ldr   r1, =0x802CC5C      @ &BattleBufferWidth.	@{U}
  @ldr   r1, =0x802CB94      @ &BattleBufferWidth.	@{J}
  ldrb  r1, [r1]
  mul   r0, r1
  ldr   r1, =0x802aec4      @ &Battle buffer.	@{U}
  @ldr   r1, =0x802AE34      @ &Battle buffer.	@{J}
  ldr   r1, [r1]
  add   r6, r0, r1          @ Current round in battle buffer.

  bl    GetDamage
  mov   r7, r0
  cmp   r7, #0x0
  beq   End
  
    @ Start proc which will put digits in VRAM.
    ldr   r0, =BAN_Proc_DelayDigits
    mov   r1, #0x3
    ldr   r3, =0x8002C7C|1 @ProcStart	@{U}
    @ldr   r3, =0x08002BCC|1 @ProcStart	@{J}
    bl    GOTO_R3
    strh  r7, [r0, #0x2A]   @ Damage/heal.
    mov   r7, r0
    mov   r0, #0x2A
    ldsh  r0, [r7, r0]
    cmp   r0, #0x0
    bge   Abs
      neg   r0, r0          @ Take absolute value of damage/heal.
    Abs:
    mov   r6, #0x1
    FindDigitCountLoop:
      mov   r1, #0xA
      swi   #0x6
      cmp   r0, #0x0
      beq   EndLoop
        add   r6, #0x1
        b     FindDigitCountLoop
    EndLoop:
    mov   r0, #0x29
    strb  r6, [r7, r0]      @ Number of digits.
    mov   r0, r4
    ldr   r3, =0x805A16C|1 @GetAISSubjectId	@{U}
    @ldr   r3, =0x805AF10|1 @GetAISSubjectId	@{J}
    bl    GOTO_R3
    mov   r1, #0x2C
    strb  r0, [r7, r1]      @ AISSubjectId. 0 if left, 1 if right.

    @ Start AIS.
    mov   r3, r6
    mov   r2, r0
    sub   r1, r0, #0x1
    neg   r1, r1
    add   r1, r1, #0x5
    mov   r0, r4
    bl    StartAIS
    mov   r0, r5


End:
add   sp, #0x8
pop   {r4-r7}
pop   {r1}
bx    r1
GOTO_R3:
bx    r3
GOTO_R12:
bx    r12

.if 0  @For SkillSystems
GetDamage:
  push {lr}
  cmp   r5, #0x0
  bne   CapDmgHeal
    mov   r0, #0x6
    ldsh  r0, [r6, r0]      @ OverDamage/OverHeal.
    b     IfThenElse
  CapDmgHeal:
    mov   r0, #0x5
    ldsb  r0, [r6, r0]      @ Capped damage/heal.
  IfThenElse:

  pop {r1}
  bx r1
.endif

.if 1  @For Standalone
@Returns the exact damage r0.
@If you are doing damage, r0 will be a negative value.
@If it has recovered, r0 will be a positive value.
@
@Since it is troublesome, r4, r5, r6 are referenced like global variables.
@r4 AIS
@r5 
@r6 BattleRound
GetDamage:
  push {lr}

  @if heal
  mov   r0, #0x3
  ldsb  r0, [r6, r0]
  cmp   r0, #0x0
  bgt   GetDamage_Attack

    @In the case of heal, it is displayed only on the other side.
    GetDamage_Heal:
    cmp   r5, #0x0
    bne   GetDamage_0   @Omitted because the healer itself does not heal.
    neg   r0, r0
    b     GetDamage_Exit

  GetDamage_Attack:
  bl GetRealDamage

  ldrb r1, [r6,#0x1]
  mov  r2, #0x1       @Is StealHP?
  and  r1, r2
  cmp  r1, #0x0
  bne  GetDamage_StealHP

  GetDamage_NormalAttack:
  cmp   r5, #0x0
  bne   GetDamage_0
  b     GetDamage_Exit

  @Steals HP and recovers.
  GetDamage_StealHP:
  cmp   r5, #0x0
  beq   GetDamage_Exit
  neg   r0, r0
  b     GetDamage_Exit

  GetDamage_0:
  mov   r0, #0x0
  @b     GetDamage_Exit

  GetDamage_Exit:
  pop {r1}
  bx r1


@With BattleRound.Damage and DMG on the battle screen, the one that seems to be correct is adopted and returned to r0.
GetRealDamage:
  push {lr}

  @If you kill the enemy, we will get real damage.
  ldrb  r0, [r6,#0x2]
  mov   r1, #0x30
  and   r0, r1    @Defeat flag
  cmp   r0, #0x0
  beq   GetRealDamage_NotDefeat

  GetRealDamage_CalcRealDamage:
  bl  GetDisplayDamage

  ldrb r1, [r6,#0x3]  @BattleRound.Damage
  cmp  r1, r0
  bgt  GetRealDamage_NotDefeat

  neg  r0, r0        @display damage is correct, so overwrite it.
  b    GetRealDamage_Exit

  GetRealDamage_NotDefeat:
  mov   r0, #0x3
  ldsb  r0, [r6, r0]
  neg   r0, r0

  GetRealDamage_Exit:
  pop {r1}
  bx r1

@Get the value of DMG written in the center of the battle screen.
GetDisplayDamage:
  push {lr}

  mov   r0, r4
  ldr   r3, =0x805A16C|1 @GetAISSubjectId	@{U}
  @ldr   r3, =0x805AF10|1 @GetAISSubjectId	@{J}
  bl    GOTO_R3

  ldr r1, =0x0203E1BC     @ DisplayValue.Damage Left	@{U}
  @ldr r1, =0x0203E1B8     @ DisplayValue.Damage Left	@{J}
  cmp r5, #0x0
  bne GetDisplayDamage_Rev
    cmp r0, #0x0    @AIS Left
    bne GetDisplayDamage_LoadDisplayDamage
       add r1, #0x02   @Damage Right
       b   GetDisplayDamage_LoadDisplayDamage

  GetDisplayDamage_Rev:
    cmp r0, #0x0    @AIS Left
    beq GetDisplayDamage_LoadDisplayDamage
       add r1, #0x02   @Damage Right
       @b   GetDisplayDamage_LoadDisplayDamage

  GetDisplayDamage_LoadDisplayDamage:
  ldrh r0, [r1]       @This is true damage.
  cmp  r0, #0x7f
  bge  GetDisplayDamage_0

  ldrb r1, [r6,#0x0]
  mov  r2, #0x1       @Is Critical?
  and  r1, r2
  cmp  r1, #0x0
  beq  GetDisplayDamage_CapCheck
     mov  r2, #0x3
     mul  r0, r2         @Critical Damage 3x

     GetDisplayDamage_CapCheck:
     cmp  r0, #0x7f
     blt  GetDisplayDamage_Exit
        mov  r0, #0x7f
        b    GetDisplayDamage_Exit

  GetDisplayDamage_0:
  mov r0, #0x0

  GetDisplayDamage_Exit:
  pop {r1}
  bx r1
.endif


@ Starts an EkrsubAnimeEmulator which mimics an AIS.
@ Also starts an gProc_efxDamageMojiEffectOBJ to align
@ the EkrsubAnimeEmulator X value and end it when it finishes.
@ Args:
@   r0:     AIS. Used for its X and Y values.
@   r1:     palette index
@   r2:     AISSubjectId. 0 if left, 1 if right.
@   r3:     Number of digits. Determines which frameData to use.
@   [sp]:   X of previous damage display. 0 if there is none.
@   [sp+4]: Digitcount of previous damage display. 0 if there is none.
@ Returns:
@   The EkrsubAnimeEmulator proc.
StartAIS:
push  {r4-r7, r14}
mov   r4, r0
mov   r5, r1
mov   r6, r3
sub   sp, #0xC


@ Prep stack args.
mov   r0, #0x3
strb  r0, [sp, #0x8]      @ Parent proc (tree 3).
mov   r0, #0x0
strb  r0, [sp, #0x4]      @ OAM0 cat OAM1.
lsl   r0, r5, #0x4
add   r0, #0x1
lsl   r0, #0x8
lsl   r2, #0x4
add   r0, r2
strb  r0, [sp]            @ OAM2.


@ Check if digits overlap.
@ If they do, raise current AIS' digits.
mov   r7, #0x28           @ Y if no overlap.
ldr   r2, [sp, #0x24]     @ Digitcount0.
cmp   r2, #0x0
beq   NoOverlap
  mov   r1, #0x2
  ldsh  r1, [r4, r1]      @ X0.
  ldr   r0, [sp, #0x20]   @ X1.
  @mov   r3, r6           @ Digitcount1.
  cmp   r0, r1
  ble   NoFlip
    mov   r7, r0          @ Ensure X0 <= X1.
    mov   r0, r1 
    mov   r1, r7
    mov   r7, r2
    mov   r2, r3
    mov   r3, r7
    mov   r7, #0x28       @ Y if no overlap.
  NoFlip:
  lsl   r2, #0x3          @ Half of length of digits (16 pixels each).
  add   r2, #0x4          @ Half of length of plus or minus (8 pixels).
  add   r2, r0            @ Right-most pixel of left number.
  lsl   r3, #0x3          @ Half of length of digits (16 pixels each).
  add   r3, #0x4          @ Half of length of plus or minus (8 pixels).
  sub   r3, r1, r3        @ Left-most pixel of right number.
  sub   r0, r3, r2
  cmp   r0, #0x0
  bge   Abs2
    neg   r0, r0          @ Take absolute value of distance.
  Abs2:
  cmp   r0, #0x8
  bgt   NoOverlap
    mov   r7, #0x38       @ Heighten digits to avoid overlap.
NoOverlap:


@ Prep other args.
mov   r3, #0x2            @ Idk this arg. Copying what 0x6C6D6 does here.
ldr   r0, =frameData-4
lsl   r1, r6, #0x2
ldr   r2, [r0, r1]        @ frameData, differs depending on number of digits.
mov   r0, #0x2
ldsh  r0, [r4, r0]        @ X.
@lsl   r1, r6, #0x3       @ Can centre X like this. But I decided to bake this
@add   r1, #0x4           @ into the frameData. If someone decides to use one
@sub   r0, r1             @ frameData for each digitcount, this code could be used.
ldr   r1, =0x80716C8|1 @StartEkrsubAnimeEmulator	@{U}
@ldr   r1, =0x8073BBC|1 @StartEkrsubAnimeEmulator	@{J}
mov   r12, r1
mov   r1, #0x4
ldsh  r1, [r4, r1]
sub   r1, r7              @ Y.
bl    GOTO_R12
mov   r5, r0

@ Start gProc_efxDamageMojiEffectOBJ
ldr   r0, =0x85D8D5C	@gProc_efxDamageMojiEffectOBJ	@{U}
@ldr   r0, =0x086033AC	@gProc_efxDamageMojiEffectOBJ	@{J}
mov   r1, #0x3
ldr   r3, =0x8002C7C|1 @ProcStart	@{U}
@ldr   r3, =0x08002BCC|1 @ProcStart	@{J}
bl    GOTO_R3
mov   r6, r0
str   r4, [r6, #0x5C]     @ AIS.
mov   r0, #0x0
strh  r0, [r6, #0x2C]     @ Timer.
mov   r0, #0x32
strh  r0, [r6, #0x2E]     @ Endtime, 50 frames.
str   r5, [r6, #0x60]     @ EkrsubAnimeEmulator proc.


mov   r0, r5
add   sp, #0xC
pop   {r4-r7}
pop   {r1}
bx    r1

.align
frameData:
.word BAN_Digits1FD
.word BAN_Digits2FD
.word BAN_Digits3FD
.word BAN_Digits4FD
.word BAN_Digits5FD
