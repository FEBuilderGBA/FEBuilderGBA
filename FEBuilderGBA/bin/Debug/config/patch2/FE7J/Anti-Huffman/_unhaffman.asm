;// Original source and concept by Hextator
;// Rewritten by Nintenlord
;// for FE7J Import by 7743

@thumb

;@org $08004364	;FE7U
@org $08004240	;FE7J
textLoader
	push {lr}
	lsr r2, r0, #31
	cmp r2, #0
	beq textLoader_compressed

textLoader_uncompressed
;	bl $08012c7c	;FE7U
	bl $08013334	;FE7J
	b textLoader_end

textLoader_compressed
	ldr r2, [pc, #8]
	ldr r2, [r2, #0]
;	bl $080bfc54	;FE7U
	bl $080C0798	;FE7J

textLoader_end
	pop {pc}

;@dcd $03003940	;FE7U
@dcd $03003860	;FE7J


;@org $08012c6c	;FE7U
@org $08013324	;FE7J
	ldr r1, [pc, #8]
	add r0, r5, #0
;	bl $08012C9C	;FE7U
	bl $08013354	;FE7J
	str r5, [r6, #0]
;	b $08012c92		;FE7U
	b $0801334A		;FE7J

;@dcd $0202a5b4	;FE7U
@dcd $0202a5b0	;FE7J
;//08012c7c-8e are free: 12 opcodes	;FE7U
;//08013334-46 are free: 12 opcodes	;FE7J

;@org $08012c7c	;FE7U
@org $08013334	;FE7J

main
	mov r3, #0x80
	lsl r3, r3, #24
	sub r0, r0, r3
	
main_loop
	ldrb r2, [r0, #0]
	strb r2, [r1, #0]
	add r1, #1
	add r0, #1
	cmp r2, #0
	bne main_loop

main_Cend
	bx lr
