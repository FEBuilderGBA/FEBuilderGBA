@thumb
@org	$080946fc
	cmp	r2, #6		;–{—ˆ‚Í5
	bls	$08094702
	b	$08094d38
	lsl	r0, r2, #2
	mov	r1, r15
	add	r1, #0x10
	ldr	r0, [r0, r1]
	mov	pc, r0
@dcd $0200e060
@dcd $0200d6e0
@dcd $0200e098
@dcd $08094734
@dcd $08094898
@dcd $0809495c
@dcd $08094a92
@dcd $08094ba8
@dcd $08094cd8