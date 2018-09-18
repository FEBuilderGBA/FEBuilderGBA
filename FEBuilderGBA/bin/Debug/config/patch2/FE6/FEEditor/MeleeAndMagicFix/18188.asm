;
;from FEditorAdv
;
@thumb
@org $08018188
;
;@r0=char data ptr
push	{lr}
bl		$08016958
lsl		r0,r0,#0x18
lsr		r0,r0,#0x13
ldr		r1,=$0x08016410
ldr		r1,[r1]
add		r0,	r0,	r1	;@Indexing of item array complete
ldrb	r0,	[r0,#0x08]	;@
mov		r1,	#0x02		;@"Use magic animation?"
and		r0,	r1		;@
lsr		r0,	r0,	#0x01	;@r0 == 1 or 0, guaranteed
pop		{pc}			;@
