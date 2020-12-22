0802C534 B530     push    {r4,r5,r14}
0802C536 B081     add     sp,-#0x4
0802C538 1C04     mov     r4,r0         @Attacker
0802C53A 1C0D     mov     r5,r1         @Defender
0802C53C F7FFFA5A bl      #0x802B9F4    @Can the unit gain EXP?
0802C540 0600     lsl     r0,r0,#0x18
0802C542 2800     cmp     r0,#0x0
0802C544 D00D     beq     retZero       @No. End.
0802C546 2013     mov     r0,#0x13      @Current HP
0802C548 5620     ldsb    r0,[r4,r0]    
0802C54A 2800     cmp     r0,#0x0       @Am I ded?
0802C54C D009     beq     retZero       @Yes. End.
0802C54E 6828     ldr     r0,[r5]       @Defender char
0802C550 6869     ldr     r1,[r5,#0x4]  @Defender class
0802C552 6A80     ldr     r0,[r0,#0x28] @Ability
0802C554 6A89     ldr     r1,[r1,#0x28] @
0802C556 4308     orr     r0,r1
0802C558 2180     mov     r1,#0x80
0802C55A 0449     lsl     r1,r1,#0x11   @0x01000000
0802C55C 4008     and     r0,r1         @
0802C55E 2800     cmp     r0,#0x0       @
0802C560 D001     beq     CalcEXP       @Not-no exp skill
retZero:
0802C562 2000     mov     r0,#0x0       @
0802C564 E023     b       end           @
CalcEXP:
0802C566 1C20     mov     r0,r4         @Attacker
0802C568 307C     add     r0,#0x7C      @Damage dealt in combat
0802C56A 7800     ldrb    r0,[r0]       @
0802C56C 0600     lsl     r0,r0,#0x18   @
0802C56E 1600     asr     r0,r0,#0x18   @
0802C570 2800     cmp     r0,#0x0       @
0802C572 D101     bne     #0x802C578    @
0802C574 2001     mov     r0,#0x1       @1 Exp for no damage dealt
0802C576 E01A     b       end           @
0802C578 1C20     mov     r0,r4         @attacker
0802C57A 1C29     mov     r1,r5         @defender
0802C57C F7FFFEF4 bl      #0x802C368    @Seems to get exp for damaging
0802C580 9000     str     r0,[sp]       @
0802C582 1C20     mov     r0,r4         @attacker
0802C584 1C29     mov     r1,r5         @defender
0802C586 F7FFFF63 bl      #0x802C450    @
0802C58A 9900     ldr     r1,[sp]       @
0802C58C 1809     add     r1,r1,r0      @
0802C58E 9100     str     r1,[sp]       @
0802C590 2964     cmp     r1,#0x64      @100 exp
0802C592 DD01     ble     #0x802C598    @don't need to cap it
0802C594 2064     mov     r0,#0x64      @
0802C596 9000     str     r0,[sp]       @
0802C598 9800     ldr     r0,[sp]       @
0802C59A 2800     cmp     r0,#0x0       @If <0 exp
0802C59C DC01     bgt     #0x802C5A2    @
0802C59E 2001     mov     r0,#0x1       @Min exp to give is 0.
0802C5A0 9000     str     r0,[sp]
0802C5A2 1C20     mov     r0,r4
0802C5A4 1C29     mov     r1,r5
0802C5A6 466A     mov     r2,r13        @&how much exp to give
0802C5A8 F7FFFFA2 bl      #0x802C4F0
0802C5AC 9800     ldr     r0,[sp]       @Return
end:
0802C5AE B001     add     sp,#0x4
0802C5B0 BC30     pop     {r4,r5}
0802C5B2 BC02     pop     {r1}
0802C5B4 4708     bx      r1
0802C5B6 0000     lsl     r0,r0,#0x0