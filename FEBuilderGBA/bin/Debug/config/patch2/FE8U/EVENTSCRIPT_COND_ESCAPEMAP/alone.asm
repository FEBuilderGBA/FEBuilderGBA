.thumb
LDR r2, =0x0202BE94 @FE8U (Unit@味方[1].ROMユニットポインタ )
MOV r3, #0x0

loop:
LDR r0, [r2]
CMP r0, #0x0
BEQ alone

	LDR r1, [r2, #0xC ]
	MOV r0, #0x4
	AND r0 ,r1
	BNE next

		LSL r0 ,r1 ,#0xF
		BMI next
			MOV r0, #0x8
			AND r1 ,r0
			BEQ term

				ADD r2, #0x48

next:
			ADD r3, #0x1
			CMP r3, #0x3E
			BLE loop
alone:
		MOV r0, #0x1
		B exit

term:
MOV r0, #0x0

exit:
MOV r8, r8
LDR r1, =0x030004B8   @FE8U (MemorySlot00 常に0にする必要がある ) r1=UnitForm
STR r0, [r1, #0x30]   @ MemorySlot0C 主に処理の結果が返されます 
MOV r0, #0x0
BX LR
