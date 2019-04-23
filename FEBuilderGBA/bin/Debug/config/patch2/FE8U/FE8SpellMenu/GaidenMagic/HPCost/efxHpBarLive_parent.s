.thumb

@ restructure efxhpbar to store attacker and defender hp data
@ hook at 8052a50
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

str     r0,[r6,#0x60]                @ 08052364 6630     
ldr     r4,=#0x203E152                @ 08052366 4C15     
ldr     r0,[r6,#0x60]                @ 08052368 6E30     
blh      0x805A16C                @ 0805236A F007FEFF 
lsl     r0,r0,#0x1                @ 0805236E 0040     
add     r0,r0,r4                @ 08052370 1900     
mov     r1,#0x0                @ 08052372 2100     
ldsh    r5,[r0,r1]                @ 08052374 5E45     
add     r4,r5,#1                @ 08052376 1C6C     
lsl     r4,r4,#0x10                @ 08052378 0424     
lsr     r4,r4,#0x10                @ 0805237A 0C24     
ldr     r0,[r6,#0x60]                @ 0805237C 6E30     
blh      0x805A16C                @ 0805237E F007FEF5 
lsl     r5,r5,#0x1                @ 08052382 006D     
add     r5,r5,r0                @ 08052384 182D     
mov     r0,r5                @ 08052386 1C28     
blh      0x8058A60                @ 08052388 F006FB6A 
lsl     r0,r0,#0x10                @ 0805238C 0400     
asr     r0,r0,#0x10                @ 0805238E 1400     
mov r1, #0x4c
strh     r0,[r6,r1]                @ 08052390 64F0     
ldr     r0,[r6,#0x60]                @ 08052392 6E30     
blh      0x805A16C                @ 08052394 F007FEEA 
lsl     r4,r4,#0x10                @ 08052398 0424     
asr     r4,r4,#0xF                @ 0805239A 13E4     
add     r4,r4,r0                @ 0805239C 1824     
mov     r0,r4                @ 0805239E 1C20     
blh      0x8058A60                @ 080523A0 F006FB5E 
lsl     r0,r0,#0x10                @ 080523A4 0400     
asr     r0,r0,#0x10                @ 080523A6 1400 
mov r1, #0x50    
strh     r0,[r6,r1]                @ 080523A8 6530    
  @ mov r0, #0
  @ str r0, [r6, #0x54] 
  @ str r0, [r6, #0x54]
mov r1, #0x4c
ldrh     r1,[r6,r1]                @ 080523AA 6CF1     
cmp     r1,r0                @ 080523AC 4281     
ble     Heal                @ 080523AE DD07     
mov     r0,#0x1                @ 080523B0 2001     
neg     r0,r0                @ 080523B2 4240     
b       Hurt                @ 080523B4 E005     
.ltorg 
Heal:   
mov     r0,#0x1                @ 080523C0 2001    
Hurt: 
mov r1, #0x48
strh     r0,[r6,r1]                @ 080523C2 64B0 

@now update the attacker
ldr     r4,=#0x203E152                @ 08052366 4C15     
ldr     r0,[r6,#0x5c]                @ 08052368 6E30     
blh      0x805A16C                @ 0805236A F007FEFF 
lsl     r0,r0,#0x1                @ 0805236E 0040     
add     r0,r0,r4                @ 08052370 1900     
mov     r1,#0x0                @ 08052372 2100     
ldsh    r5,[r0,r1]                @ 08052374 5E45     
add     r4,r5,#1                @ 08052376 1C6C     
lsl     r4,r4,#0x10                @ 08052378 0424     
lsr     r4,r4,#0x10                @ 0805237A 0C24     
ldr     r0,[r6,#0x5c]                @ 0805237C 6E30     
blh      0x805A16C                @ 0805237E F007FEF5 
lsl     r5,r5,#0x1                @ 08052382 006D     
add     r5,r5,r0                @ 08052384 182D     
mov     r0,r5                @ 08052386 1C28     
blh      0x8058A60                @ 08052388 F006FB6A 
lsl     r0,r0,#0x10                @ 0805238C 0400     
asr     r0,r0,#0x10                @ 0805238E 1400     
mov r1, #0x4e
strh     r0,[r6,r1]                @ 08052390 64F0     
ldr     r0,[r6,#0x5c]                @ 08052392 6E30     
blh      0x805A16C                @ 08052394 F007FEEA 
lsl     r4,r4,#0x10                @ 08052398 0424     
asr     r4,r4,#0xF                @ 0805239A 13E4     
add     r4,r4,r0                @ 0805239C 1824     
mov     r0,r4                @ 0805239E 1C20     
blh      0x8058A60                @ 080523A0 F006FB5E 
lsl     r0,r0,#0x10                @ 080523A4 0400     
asr     r0,r0,#0x10                @ 080523A6 1400 
mov r1, #0x52    
strh     r0,[r6,r1]                @ 080523A8 6530     
mov r1, #0x4e
ldrh     r1,[r6,r1]                @ 080523AA 6CF1     
cmp     r1,r0                @ 080523AC 4281     
ble     Heal2                @ 080523AE DD07     
mov     r0,#0x1                @ 080523B0 2001     
neg     r0,r0                @ 080523B2 4240     
b       Hurt2                @ 080523B4 E005     
.ltorg 
Heal2:   
mov     r0,#0x1                @ 080523C0 2001    
Hurt2: 
mov r1, #0x4a
strh     r0,[r6,r1]                @ 080523C2 64B0 



mov     r1,#0x0                @ 080523C4 2100     
strh    r1,[r6,#0x2C]                @ 080523C6 85B1  
mov r0, #0x4c   
ldrh     r0,[r6,r0]                @ 080523C8 6CF0     
strh    r0,[r6,#0x2E]                @ 080523CA 85F0
mov r0, #0x4e
ldrh r0, [r6,r0]
strh r0, [r6, #0x30]     
str     r1,[r6,#0x54]                @ 080523CC 6571     
str     r1,[r6,#0x58]                @ 080523CE 65B1  
  str r7, [r6, #0x64]   
  ldr r0,[r6,#0x5c]  
blh      0x805A16C                @ 080523D2 F007FECB 
ldr     r1,=0x2017780                @ 080523D6 4904     
lsl     r0,r0,#0x1                @ 080523D8 0040     
add     r0,r0,r1                @ 080523DA 1840     
  mov r1,#0x2 
strh    r1,[r0]                @ 080523DE 8001     
pop     {r4-r7}                @ 080523E0 BC70     
pop     {r0}                @ 080523E2 BC01     
bx      r0                @ 080523E4 4700     
