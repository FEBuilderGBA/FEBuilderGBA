.thumb
.equ IsRightToLeft, 0x805A16C

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push    {r4-r7,r14}                @ 080523EC B5F0     
mov     r5,r0                @ 080523EE 1C05     
ldr     r4,=#0x2000000                @ 080523F0 4C27     
ldr     r0,[r5,#0x60]    @0x5c for attacker's AIS           @ 080523F2 6E28     
blh      IsRightToLeft                @ 080523F4 F007FEBA 
lsl     r0,r0,#0x3                @ 080523F8 00C0     
add     r0,r0,r4                @ 080523FA 1900     
ldr     r6,[r0]                @ 080523FC 6806     
ldr     r0,[r5,#0x60]                @ 080523FE 6E28     
blh      IsRightToLeft                @ 08052400 F007FEB4 
lsl     r0,r0,#0x1                @ 08052404 0040     
add     r0,#0x1                @ 08052406 3001     
lsl     r0,r0,#0x2                @ 08052408 0080     
add     r0,r0,r4                @ 0805240A 1900     
ldr     r7,[r0]                @ 0805240C 6807     
ldr     r1,[r5,#0x58]                @ 0805240E 6DA9     
cmp     r1,#0x0                @ 08052410 2900     
bne     loc_0x805244E                @ 08052412 D11C     
  ldrh    r0,[r5,#0x2C]                @ 08052414 8DA8     
  add     r0,#0x1                @ 08052416 3001     
  strh    r0,[r5,#0x2C]                @ 08052418 85A8     
  lsl     r0,r0,#0x10                @ 0805241A 0400     
  asr     r0,r0,#0x10                @ 0805241C 1400     
  cmp     r0,#0x2                @ 0805241E 2802     
  bne     loc_0x805244E                @ 08052420 D115     
    strh    r1,[r5,#0x2C]                @ 08052422 85A9  
    mov r1, #0x48   @change HP delta to signed halfword
    ldrsh     r1,[r5,r1]                @ 08052424 6CA9     
    ldrh    r0,[r5,#0x2E]                @ 08052426 8DE8     
    add     r0,r0,r1                @ 08052428 1840     
    strh    r0,[r5,#0x2E]                @ 0805242A 85E8     
    ldr     r0,[r5,#0x60]                @ 0805242C 6E28     
    blh      IsRightToLeft                @ 0805242E F007FE9D 
    ldr     r1,=#0x203E1AC                @ 08052432 4918     @where hp is stored.
    lsl     r0,r0,#0x1                @ 08052434 0040     
    add     r0,r0,r1                @ 08052436 1840    
    mov r2, #0x48 @same here 
    ldrsh     r2,[r5,r2]                @ 08052438 6CAA     
    ldrh    r1,[r0]                @ 0805243A 8801     
    add     r1,r1,r2                @ 0805243C 1889     
    strh    r1,[r0]                @ 0805243E 8001    @@@HERE WE SAVED THE NEW HP  

      @now we check the other side's HP... I think.
      @check the current HP
      @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ make sure HP Draining is checked.
      @now update the attacker   
        ldr     r0,[r5,#0x5c]                @ 08052368 6E30     
        blh      0x805A16C                @ 0805236A F007FEFF 
        ldr     r3,=#0x203E152                @ 08052366 4C15  
        lsl     r0,r0,#0x1                @ 0805236E 0040     
        add     r0,r0,r3                @ 08052370 1900     
        mov     r1,#0x0                @ 08052372 2100     
        ldsh    r3,[r0,r1]                @ 08052374 5E45  
        @here r3 is the nth round
        @let's load up the battle buffer and check that
        ldr r0, =0x802aec4
        ldr r0, [r0] @r0 is battle buffer 203aac0
        lsl r1, r3, #3 @8 bytes per entry
        add r1, r0
            MissLoop1:
            ldr r0, [r1]
            mov r2, #2 @0x2 is miss
            and r2, r0
            cmp r2, #0
            beq NotMiss1
                @if a miss, check the next round.
                add r1, #8
                b MissLoop1
        NotMiss1:
        mov r1, #0x10
        lsl r1, #4 @0x100 is drain hp
        and r1, r0
        cmp r1, #0
        beq AttackerDone
      @ NextCheck:
      mov r2, #0x30 @curr hp
      ldsh r1, [r5,r2]
      mov r2, #0x52 @final hp
      ldrsh r0, [r5,r2]
      cmp r1, r0
      beq AttackerDone
      cmp r0, #0xff
      beq AttackerDone
        mov r1, #0x4a @attacker's hp delta
        ldrsh r1, [r5,r1]
        ldrh r0, [r5, #0x30] @attacker's hp
        add r0, r1
        strh r0, [r5, #0x30]
        ldr r0, [r5, #0x5c]
        blh IsRightToLeft
        ldr     r1,=#0x203E1AC                @ 08052432 4918     @where hp is stored.
        lsl     r0,r0,#0x1                @ 08052434 0040     
        add     r0,r0,r1                @ 08052436 1840    
        mov r2, #0x4a @same here 
        ldrsh     r2,[r5,r2]                @ 08052438 6CAA  
        ldrh r1, [r0]
        add r1, r2
        strh r1, [r0]
    AttackerDone:
    mov     r0,#0x2E                @ 08052440 202E     
    ldsh    r1,[r5,r0]                @ 08052442 5E29 
    mov r2, #0x50    
    ldrsh     r0,[r5,r2]                @ 08052444 6D28     
    cmp     r1,r0                @ 08052446 4281     @@ Current HP = Final HP?
    
    bne     loc_0x805244E                @ 08052448 D101     
      mov     r0,#0x1                @ 0805244A 2001     
      str     r0,[r5,#0x58]                @ 0805244C 65A8     
loc_0x805244E:
ldr     r1,[r5,#0x54]                @ 0805244E 6D69     
cmp     r1,#0x1E                @ 08052450 291E     
bne     loc2_0x80524F0                @ 08052452 D14D     
  ldr     r0,[r5,#0x58]                @ 08052454 6DA8     
  cmp     r0,#0x1                @ 08052456 2801     
  bne     loc2_0x80524F0                @ 08052458 D14A     
    ldr     r4,=#0x203E152                @ 0805245A 4C0F     
    ldr     r0,[r5,#0x60]                @ 0805245C 6E28     
    blh IsRightToLeft                @ 0805245E F007FE85 
    lsl     r0,r0,#0x1                @ 08052462 0040     
    add     r0,r0,r4                @ 08052464 1900     
    ldrh    r1,[r0]                @ 08052466 8801     
    add     r1,#0x1                @ 08052468 3101     
    mov     r4,#0x0                @ 0805246A 2400     
    strh    r1,[r0]                @ 0805246C 8001     
    ldr     r0,[r5,#0x60]                @ 0805246E 6E28     
    blh IsRightToLeft                @ 08052470 F007FE7C 
    ldr     r1,=#0x2017780                @ 08052474 4909     
    lsl     r0,r0,#0x1                @ 08052476 0040     
    add     r0,r0,r1                @ 08052478 1840     
    strh    r4,[r0]                @ 0805247A 8004  
    mov r0, #0x50   
    ldrh     r0,[r5,r0]                @ 0805247C 6D28     
    cmp     r0,#0x0                @ 0805247E 2800     
    bne     loc_0x80524E4                @ 08052480 D130     
        blh      0x804FD54                @ 08052482 F7FDFC67 
        cmp     r0,#0x1                @ 08052486 2801     
        bne     loc_0x80524A0                @ 08052488 D10A     
          mov     r0,#0x0                @ 0805248A 2000     
          b       loc_0x80524B4                @ 0805248C E012
          .ltorg 
        loc_0x80524A0:       
        ldr     r4,=#0x203E190                @ 080524A0 4C08     
        mov     r0,r6                @ 080524A2 1C30     
        blh IsRightToLeft                @ 080524A4 F007FE62 
        add     r0,r0,r4                @ 080524A8 1900     
        ldrb    r0,[r0]                @ 080524AA 7800     
        blh      0x80835A8                @ 080524AC F031F87C 
        lsl     r0,r0,#0x18                @ 080524B0 0600     
        asr     r0,r0,#0x18                @ 080524B2 1600  
      loc_0x80524B4:   
      cmp     r0,#0x1                @ 080524B4 2801     
      bne     loc_0x80524C8                @ 080524B6 D107     
      mov     r0,r6                @ 080524B8 1C30     
      mov     r1,r7                @ 080524BA 1C39     
      blh      0x8052DD4                @ 080524BC F000FC8A 
      b       loc_0x80524E4                @ 080524C0 E010   
      .ltorg      
  loc2_0x80524F0:
  b loc_0x80524F0
        loc_0x80524C8:
        blh      0x805B07C                @ 080524C8 F008FDD8 
        mov     r0,r6                @ 080524CC 1C30     
        mov     r1,r7                @ 080524CE 1C39     
        blh      0x8052FAC                @ 080524D0 F000FD6C 
        ldr     r0,[r5,#0x60]                @ 080524D4 6E28     
        blh IsRightToLeft                @ 080524D6 F007FE49 
        ldr     r1,=#0x203E104                @ 080524DA 4904     
        lsl     r0,r0,#0x1                @ 080524DC 0040     
        add     r0,r0,r1                @ 080524DE 1840     
        mov     r1,#0x0                @ 080524E0 2100     
        strh    r1,[r0]                @ 080524E2 8001 
    loc_0x80524E4:    
    mov     r0,r5                @ 080524E4 1C28     
    blh      0x8002E94                @ 080524E6 F7B0FCD5 
@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    ldr     r0,[r5,#0x5c]                @ 08052368 6E30     
    blh      0x805A16C                @ 0805236A F007FEFF 
    ldr     r3,=#0x203E152                @ 08052366 4C15  
    lsl     r0,r0,#0x1                @ 0805236E 0040     
    add     r0,r0,r3                @ 08052370 1900     
    mov     r1,#0x0                @ 08052372 2100     
    ldsh    r3,[r0,r1]                @ 08052374 5E45  
    @here r3 is the nth round
    @let's load up the battle buffer and check that
    ldr r0, =0x802aec4
    ldr r0, [r0] @r0 is battle buffer 203aac0
    lsl r1, r3, #3 @8 bytes per entry
    add r1, r0
            MissLoop2:
            ldr r0, [r1]
            mov r2, #2 @0x2 is miss
            and r2, r0
            cmp r2, #0
            beq NotMiss2
                @if a miss, check the next round.
                add r1, #8
                b MissLoop2
    NotMiss2:
    mov r1, #0x10
    lsl r1, #4 @0x100 is drain hp
    and r1, r0
    cmp r1, #0
    beq End

    ldr     r4,=#0x203E152                @ 0805245A 4C0F     
    ldr     r0,[r5,#0x5c]                @ 0805245C 6E28     
    blh IsRightToLeft                @ 0805245E F007FE85 
    lsl     r0,r0,#0x1                @ 08052462 0040     
    add     r0,r0,r4                @ 08052464 1900     
    ldrh    r1,[r0]                @ 08052466 8801     
    add     r1,#0x1                @ 08052468 3101     
    mov     r4,#0x0                @ 0805246A 2400     
    strh    r1,[r0]                @ 0805246C 8001     
    ldr     r0,[r5,#0x5c]                @ 0805246E 6E28     
    blh IsRightToLeft                @ 08052470 F007FE7C 
    ldr     r1,=#0x2017780                @ 08052474 4909     
    lsl     r0,r0,#0x1                @ 08052476 0040     
    add     r0,r0,r1                @ 08052478 1840     
    strh    r4,[r0]                @ 0805247A 8004     
    mov r0, #0x52
    ldrh     r0,[r5,r0]                @ 0805247C 6D28     
    cmp     r0,#0x0                @ 0805247E 2800     
    bne     loc2_0x80524E4                @ 08052480 D130     
        blh      0x804FD54                @ 08052482 F7FDFC67 
        cmp     r0,#0x1                @ 08052486 2801     
        bne     loc2_0x80524A0                @ 08052488 D10A     
          mov     r0,#0x0                @ 0805248A 2000     
          b       loc2_0x80524B4                @ 0805248C E012
          .ltorg 
        loc2_0x80524A0:       
        ldr     r4,=#0x203E190                @ 080524A0 4C08     
        mov     r0,r6                @ 080524A2 1C30     
        blh IsRightToLeft                @ 080524A4 F007FE62 
        add     r0,r0,r4                @ 080524A8 1900     
        ldrb    r0,[r0]                @ 080524AA 7800     
        blh      0x80835A8                @ 080524AC F031F87C 
        lsl     r0,r0,#0x18                @ 080524B0 0600     
        asr     r0,r0,#0x18                @ 080524B2 1600  
      loc2_0x80524B4:   
      cmp     r0,#0x1                @ 080524B4 2801     
      bne     loc2_0x80524C8                @ 080524B6 D107     
      mov     r0,r6                @ 080524B8 1C30     
      mov     r1,r7                @ 080524BA 1C39     
      blh      0x8052DD4                @ 080524BC F000FC8A 
      b       loc2_0x80524E4                @ 080524C0 E010   
      .ltorg      
        loc2_0x80524C8:
        blh      0x805B07C                @ 080524C8 F008FDD8 
        mov     r0,r6                @ 080524CC 1C30     
        mov     r1,r7                @ 080524CE 1C39     
        blh      0x8052FAC                @ 080524D0 F000FD6C 
        ldr     r0,[r5,#0x5c]                @ 080524D4 6E28     
        blh IsRightToLeft                @ 080524D6 F007FE49 
        ldr     r1,=#0x203E104                @ 080524DA 4904     
        lsl     r0,r0,#0x1                @ 080524DC 0040     
        add     r0,r0,r1                @ 080524DE 1840     
        mov     r1,#0x0                @ 080524E0 2100     
        strh    r1,[r0]                @ 080524E2 8001 
    loc2_0x80524E4:    
    mov     r0,r5                @ 080524E4 1C28     
    blh      0x8002E94                @ 080524E6 F7B0FCD5 
    b       End                @ 080524EA E007     
    .ltorg
loc_0x80524F0:    
add     r0,r1,#1                @ 080524F0 1C48     
str     r0,[r5,#0x54]                @ 080524F2 6568     
cmp     r0,#0x1D                @ 080524F4 281D     
bls     End                @ 080524F6 D901     
mov     r0,#0x1E                @ 080524F8 201E     
str     r0,[r5,#0x54]                @ 080524FA 6568 
End:    
pop     {r4-r7}                @ 080524FC BCF0     
pop     {r0}                @ 080524FE BC01     
bx      r0                @ 08052500 4700  
