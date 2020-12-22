@Not sure if this is killing exp yet.
0802C450 B5F0     push    {r4-r7,r14}
0802C452 1C07     mov     r7,r0         @attacker
0802C454 1C0D     mov     r5,r1         @defender
0802C456 2013     mov     r0,#0x13      @Current HP
0802C458 5628     ldsb    r0,[r5,r0]    @Defender HP
0802C45A 2800     cmp     r0,#0x0       @if defender HP is equal
0802C45C D001     beq     #0x802C462
0802C45E 2000     mov     r0,#0x0       @No bonus
0802C460 E043     b       #0x802C4EA    @b end
0802C462 2614     mov     r6,#0x14      @r6 = 20d (assuming is promoted level bonus?)
0802C464 4809     ldr     r0,=#0x202BCB0@???
0802C466 7901     ldrb    r1,[r0,#0x4]  @Flags?
0802C468 2040     mov     r0,#0x40
0802C46A 4008     and     r0,r1
0802C46C 2800     cmp     r0,#0x0
0802C46E D103     bne     #0x802C478
0802C470 4807     ldr     r0,=#0x202BCF0@???
0802C472 7EC0     ldrb    r0,[r0,#0x1B] @"Mode Coefficient" on serenes?
0802C474 2801     cmp     r0,#0x1
0802C476 D00D     beq     #0x802C494    @It's set in my game.
0802C478 1C28     mov     r0,r5         @Suppose the flag isn't set. r0 = defender
0802C47A F7FFFF8D bl      #0x802C398    @Calcs [(enemy’s Level x enemy’s Class power) + enemy’s Class bonus B]
0802C47E 1C06     mov     r6,r0
0802C480 3614     add     r6,#0x14
0802C482 1C38     mov     r0,r7
0802C484 F7FFFF88 bl      #0x802C398    @Same for ally
0802C488 1A36     sub     r6,r6,r0
0802C48A E020     b       #0x802C4CE
0802C48C BCB0     pop     {r4,r5,r7}
0802C48E 0202     lsl     r2,r0,#0x8
0802C490 BCF0     pop     {r4-r7}
0802C492 0202     lsl     r2,r0,#0x8
0802C494 1C28     mov     r0,r5
0802C496 F7FFFF7F bl      #0x802C398
0802C49A 1C04     mov     r4,r0
0802C49C 1C38     mov     r0,r7
0802C49E F7FFFF7B bl      #0x802C398
0802C4A2 4284     cmp     r4,r0
0802C4A4 DC0A     bgt     #0x802C4BC
0802C4A6 1C28     mov     r0,r5
0802C4A8 F7FFFF76 bl      #0x802C398
0802C4AC 1C04     mov     r4,r0
0802C4AE 1C38     mov     r0,r7
0802C4B0 F7FFFF72 bl      #0x802C398
0802C4B4 0FC1     lsr     r1,r0,#0x1F
0802C4B6 1840     add     r0,r0,r1
0802C4B8 1040     asr     r0,r0,#0x1
0802C4BA E006     b       #0x802C4CA
0802C4BC 1C28     mov     r0,r5
0802C4BE F7FFFF6B bl      #0x802C398
0802C4C2 1C04     mov     r4,r0
0802C4C4 1C38     mov     r0,r7
0802C4C6 F7FFFF67 bl      #0x802C398
0802C4CA 1A24     sub     r4,r4,r0
0802C4CC 1936     add     r6,r6,r4
0802C4CE 1C38     mov     r0,r7         @Land here under mode coefficient 1
0802C4D0 1C29     mov     r1,r5
0802C4D2 F7FFFF81 bl      #0x802C3D8    @Thief and Entombed bonus
0802C4D6 1836     add     r6,r6,r0
0802C4D8 1C38     mov     r0,r7
0802C4DA 1C29     mov     r1,r5
0802C4DC F7FFFF96 bl      #0x802C40C
0802C4E0 4346     mul     r6,r0
0802C4E2 2E00     cmp     r6,#0x0
0802C4E4 DA00     bge     #0x802C4E8
0802C4E6 2600     mov     r6,#0x0
0802C4E8 1C30     mov     r0,r6
0802C4EA BCF0     pop     {r4-r7}
0802C4EC BC02     pop     {r1}
0802C4EE 4708     bx      r1