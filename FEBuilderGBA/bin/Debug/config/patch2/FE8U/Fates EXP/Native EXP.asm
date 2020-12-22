0802B92C B570     push    {r4-r6,r14}
0802B92E 4D19     ldr     r5,=#0x203A4EC    @Attacker
0802B930 200B     mov     r0,#0xB           
0802B932 5628     ldsb    r0,[r5,r0]        @Unit data index
0802B934 21C0     mov     r1,#0xC0          @not-player unit
0802B936 4008     and     r0,r1
0802B938 2800     cmp     r0,#0x0
0802B93A D106     bne     #0x802B94A        @Give EXP to defender
0802B93C 4816     ldr     r0,=#0x203A56C    @Defender
0802B93E 7AC0     ldrb    r0,[r0,#0xB]
0802B940 0600     lsl     r0,r0,#0x18
0802B942 1600     asr     r0,r0,#0x18
0802B944 4008     and     r0,r1
0802B946 2800     cmp     r0,#0x0
0802B948 D021     beq     end               @Both are ally; No EXP. (Berserk)

0802B94A 4814     ldr     r0,=#0x202BCF0    @Misc data
0802B94C 7D01     ldrb    r1,[r0,#0x14]     @Difficulty/Mode? 0x40 = difficult, 0x01 = Eirika
0802B94E 2080     mov     r0,#0x80          @No exp mode
0802B950 4008     and     r0,r1
0802B952 2800     cmp     r0,#0x0
0802B954 D11B     bne     end
0802B956 4C10     ldr     r4,=#0x203A56C    @Defender
0802B958 1C28     mov     r0,r5
0802B95A 1C21     mov     r1,r4
0802B95C F000FDEA bl      #0x802C534
0802B960 1C2E     mov     r6,r5
0802B962 366E     add     r6,#0x6E
0802B964 7030     strb    r0,[r6]
0802B966 1C20     mov     r0,r4
0802B968 1C29     mov     r1,r5
0802B96A F000FDE3 bl      #0x802C534
0802B96E 1C21     mov     r1,r4
0802B970 316E     add     r1,#0x6E
0802B972 7008     strb    r0,[r1]
0802B974 7831     ldrb    r1,[r6]
0802B976 7A6A     ldrb    r2,[r5,#0x9]
0802B978 1889     add     r1,r1,r2
0802B97A 7269     strb    r1,[r5,#0x9]
0802B97C 7A61     ldrb    r1,[r4,#0x9]
0802B97E 1809     add     r1,r1,r0
0802B980 7261     strb    r1,[r4,#0x9]
0802B982 1C28     mov     r0,r5
0802B984 F000F850 bl      #0x802BA28
0802B988 1C20     mov     r0,r4
0802B98A F000F84D bl      #0x802BA28
end:
0802B98E BC70     pop     {r4-r6}
0802B990 BC01     pop     {r0}
0802B992 4700     bx      r0
0802B994 A4EC     add     r4,=#0x802BD48
0802B996 0203     lsl     r3,r0,#0x8
0802B998 A56C     add     r5,=#0x802BB4C
0802B99A 0203     lsl     r3,r0,#0x8
0802B99C BCF0     pop     {r4-r7}
0802B99E 0202     lsl     r2,r0,#0x8
