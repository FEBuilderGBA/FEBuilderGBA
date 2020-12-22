0802C368 B570     push    {r4-r6,r14}
0802C36A 1C06     mov     r6,r0         @Attacker
0802C36C 1C0C     mov     r4,r1         @Defender
0802C36E F7FFFFE9 bl      #0x802C344    @Get effective level (+20 if promoted)
0802C372 1C05     mov     r5,r0         @r5 = level of attacker
0802C374 1C20     mov     r0,r4         @Defender
0802C376 F7FFFFE5 bl      #0x802C344    
0802C37A 1A2D     sub     r5,r5,r0      @(Attacker - Defender) level
0802C37C 201F     mov     r0,#0x1F      @31
0802C37E 1B45     sub     r5,r0,r5      @31 - attackerlvl + defenderlvl
0802C380 2D00     cmp     r5,#0x0       @Bottom out EXP at 0.
0802C382 DA00     bge     #0x802C386
0802C384 2500     mov     r5,#0x0
0802C386 6870     ldr     r0,[r6,#0x4]  @Attacker class
0802C388 211A     mov     r1,#0x1A
0802C38A 5641     ldsb    r1,[r0,r1]    @Attacker class relative power
0802C38C 1C28     mov     r0,r5         @EXP to gain so far.
0802C38E F0A5FAB5 bl      #0x80D18FC    @r0/r1
0802C392 BC70     pop     {r4-r6}
0802C394 BC02     pop     {r1}
0802C396 4708     bx      r1
