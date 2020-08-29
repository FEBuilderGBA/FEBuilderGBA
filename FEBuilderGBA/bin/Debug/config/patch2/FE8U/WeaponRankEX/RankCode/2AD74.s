.thumb

cmp r0, #0xe
ble Cont2AD
	mov r0, #0xe	
Cont2AD:		
ldr r2,=lMaxRank
ldrb r2, [r2]
cmp r0, r2
bls Jump2AD	
	add r1, r4
	add r1, #0x60
	ldrh r0, [r1]
	add r0, #0x5	
	strh r0, [r1]
	add r1, #0x6
	ldrh r0, [r1]
	add r0, #0x5
	strh r0, [r1]
Jump2AD:
pop {r4}
ldr r3,=0x0802ad8d
bx r3