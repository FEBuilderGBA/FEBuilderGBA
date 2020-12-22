0802B9F4 B500     push    {r14}
0802B9F6 1C02     mov     r2,r0
0802B9F8 4808     ldr     r0,=#0x202BCB0
0802B9FA 7901     ldrb    r1,[r0,#0x4]      @Something about the action performed by the unit?
0802B9FC 2040     mov     r0,#0x40
0802B9FE 4008     and     r0,r1
0802BA00 2800     cmp     r0,#0x0
0802BA02 D108     bne     retTrue
0802BA04 7A50     ldrb    r0,[r2,#0x9]      @EXP
0802BA06 28FF     cmp     r0,#0xFF          @0xFF EXP (enemies)
0802BA08 D00A     beq     retFalse        
0802BA0A 200B     mov     r0,#0xB           @Deployment number
0802BA0C 5610     ldsb    r0,[r2,r0]
0802BA0E 21C0     mov     r1,#0xC0
0802BA10 4008     and     r0,r1
0802BA12 2800     cmp     r0,#0x0
0802BA14 D104     bne     retFalse          @If not player unit, ret false.
retTrue:
0802BA16 2001     mov     r0,#0x1
0802BA18 E003     b       end
0802BA1A 0000     lsl     r0,r0,#0x0
0802BA1C BCB0     pop     {r4,r5,r7}
0802BA1E 0202     lsl     r2,r0,#0x8
retFalse:
0802BA20 2000     mov     r0,#0x0
end:
0802BA22 BC02     pop     {r1}
0802BA24 4708     bx      r1
0802BA26 0000     lsl     r0,r0,#0x0