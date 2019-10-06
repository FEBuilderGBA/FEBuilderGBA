

@ created by ~ipatix~
@ revision 2.1

    /* globals */
	.global	main_mixer
	.global	main_mixer_end

    /* game code definitions */
	.equ	GAME_BPED, 0
	.equ	GAME_BPEE, 1
	.equ	GAME_BPRE, 2
	.equ	GAME_KWJ6, 3
	.equ	GAME_AE7E, 4
	.equ	GAME_BPRD, 5
	.equ	GAME_BE8E, 6

    /* SELECT USED GAME HERE */
	.equ	USED_GAME, GAME_BE8E		@ CHOOSE YOUR GAME

	.equ	FRAME_LENGTH_5734, 0x60
	.equ	FRAME_LENGTH_7884, 0x84	    @ THIS MODE IS NOT SUPPORTED BY THIS ENGINE BECAUSE IT DOESN'T USE AN 8 ALIGNED BUFFER LENGTH
	.equ	FRAME_LENGTH_10512, 0xB0
	.equ	FRAME_LENGTH_13379, 0xE0	@ DEFAULT
	.equ	FRAME_LENGTH_15768, 0x108
	.equ	FRAME_LENGTH_18157, 0x130
	.equ	FRAME_LENGTH_21024, 0x160
	.equ	FRAME_LENGTH_26758, 0x1C0
	.equ	FRAME_LENGTH_31536, 0x210
	.equ	FRAME_LENGTH_36314, 0x260
	.equ	FRAME_LENGTH_40137, 0x2A0
	.equ	FRAME_LENGTH_42048, 0x2C0

	.equ	DECODER_BUFFER_BPE, 0x03001300
	.equ	DECODER_BUFFER_BPR, 0x03002088
	.equ	DECODER_BUFFER_KWJ, 0x03005800

	.equ	BUFFER_IRAM_BPE, 0x03001AA8
	.equ	BUFFER_IRAM_BPR, 0x030028E0
	.equ	BUFFER_IRAM_KWJ, 0x03005840
	.equ	BUFFER_IRAM_AE7, 0x03006D60	@ PUT THE WORKBUFFER ADDRESS FOR FIRE EMBLEM HERE!!!
    .equ    BUFFER_IRAM_BE8, 0x03007510

    /* stack variables */
	.equ	ARG_FRAME_LENGTH, 0x0       @ TODO actually use this variable
	.equ	ARG_REMAIN_CHN, 0x4         @ This is the channel count variable    
	.equ	ARG_BUFFER_POS, 0x8         @ stores the current output buffer pointer
	.equ	ARG_LOOP_START_POS, 0xC     @ stores wave loop start position in channel loop
	.equ	ARG_LOOP_LENGTH, 0x10       @   ''    ''   ''  end position
@   .equ    ARG_UKNOWN, 0x14 
	.equ	ARG_VAR_AREA, 0x18          @ pointer to engine the main work area

    /* channel struct */
	.equ	CHN_STATUS, 0x0             @ [byte] channel status bitfield
	.equ	CHN_MODE, 0x1               @ [byte] channel mode bitfield
	.equ	CHN_VOL_1, 0x2              @ [byte] volume right
	.equ	CHN_VOL_2, 0x3              @ [byte] volume left
	.equ	CHN_ATTACK, 0x4             @ [byte] wave attack summand
	.equ	CHN_DECAY, 0x5              @ [byte] wave decay factor
	.equ	CHN_SUSTAIN, 0x6            @ [byte] wave sustain level
	.equ	CHN_RELEASE, 0x7            @ [byte] wave release factor
	.equ	CHN_ADSR_LEVEL, 0x9         @ [byte] current envelope level
	.equ	CHN_FINAL_VOL_1, 0xA		@ [byte] not used anymore!
	.equ	CHN_FINAL_VOL_2, 0xB		@ [byte] not used anymore!
	.equ	CHN_ECHO_VOL, 0xC           @ [byte] pseudo echo volume
	.equ	CHN_ECHO_REMAIN, 0xD        @ [byte] pseudo echo length
	.equ	CHN_POSITION_REL, 0x18		@ [word] sample countdown in mixing loop
	.equ	CHN_FINE_POSITION, 0x1C     @ [word] inter sample position (23 bits)
	.equ	CHN_FREQUENCY, 0x20         @ [word] sample rate (in Hz)
	.equ	CHN_WAVE_OFFSET, 0x24       @ [word] wave header pointer
	.equ	CHN_POSITION_ABS, 0x28		@ [word] points to the current position in the wave data (relative offset for compressed samples)
	.equ	CHN_BLOCK_COUNT, 0x3C       @ [word] only used for compressed samples: contains the value of the block that is currently decoded

    /* wave header struct */
	.equ	WAVE_LOOP_FLAG, 0x3         @ [byte] 0x0 = oneshot; 0x40 = looped
	.equ	WAVE_FREQ, 0x4              @ [word] pitch adjustment value = mid-C samplerate * 1024
	.equ	WAVE_LOOP_START, 0x8        @ [word] loop start position
	.equ	WAVE_LENGTH, 0xC            @ [word] loop end / wave end position
    .equ    WAVE_DATA, 0x10             @ [byte array] actual wave data

    /* pulse wave synth configuration offset */
	.equ	SYNTH_BASE_WAVE_DUTY, 0x1   @ [byte]
	.equ	SYNTH_WIDTH_CHANGE_1, 0x2   @ [byte]
	.equ	SYNTH_MOD_AMOUNT, 0x3       @ [byte]
	.equ	SYNTH_WIDTH_CHANGE_2, 0x4   @ [byte]

    /* CHN_STATUS flags - 0x0 = OFF */
	.equ	FLAG_CHN_INIT, 0x80         @ [bit] write this value to init a channel
	.equ	FLAG_CHN_RELEASE, 0x40      @ [bit] write this value to release (fade out) the channel
	.equ	FLAG_CHN_COMP, 0x20         @ [bit] is wave being played compressed (yes/no)
	.equ	FLAG_CHN_LOOP, 0x10         @ [bit] loop (yes/no)
	.equ	FLAG_CHN_ECHO, 0x4          @ [bit] echo phase
	.equ	FLAG_CHN_ATTACK, 0x3        @ [bit] attack phase
	.equ	FLAG_CHN_DECAY, 0x2         @ [bit] decay phase
	.equ	FLAG_CHN_SUSTAIN, 0x1       @ [bit] sustain phase

    /* CHN_MODE flags */
	.equ	MODE_FIXED_FREQ, 0x8        @ [bit] set to disable resampling (i.e. playback with output rate)
	.equ	MODE_REVERSE, 0x10          @ [bit] set to reverse sample playback
	.equ	MODE_COMP, 0x30             @ [bit] is wave being played compressed or reversed (TODO: rename flag)
	.equ	MODE_SYNTH, 0x40            @ [bit] READ ONLY, indicates synthzied output

    /* variables of the engine work area */
	.equ	VAR_REVERB, 0x5             @ [byte] 0-127 = reverb level
	.equ	VAR_MAX_CHN, 0x6            @ [byte] maximum channels to process
	.equ	VAR_MASTER_VOL, 0x7         @ [byte] PCM master volume
	.equ	VAR_DEF_PITCH_FAC, 0x18     @ [word] this value get's multiplied with the samplerate for the inter sample distance
	.equ	VAR_FIRST_CHN, 0x50         @ [CHN struct] relative offset to channel array

    /* just some more defines */
	.equ	REG_DMA3_SRC, 0x040000D4
    .equ    ARM_OP_LEN, 0x4

@#######################################
@*********** GAME CONFIGS **************
@ add the game's name above to the ASM .equ-s before creating new configs
@#######################################


@*********** IF GERMAN POKEMON EMERALD
.if USED_GAME==GAME_BPED

	.equ	hq_buffer, BUFFER_IRAM_BPE
	.equ	decoder_buffer_target, DECODER_BUFFER_BPE
	.equ	ALLOW_PAUSE, 1
	.equ	DMA_FIX, 1
	.equ	ENABLE_DECOMPRESSION, 1
	.equ	PREVENT_CLIP, 1

.endif
@*********** IF ENGLISH POKEMON FIRE RED
.if USED_GAME==GAME_BPRD

	.equ	hq_buffer, BUFFER_IRAM_BPR
	.equ	decoder_buffer_target, DECODER_BUFFER_BPR
	.equ	ALLOW_PAUSE, 1
	.equ	DMA_FIX, 1
	.equ	ENABLE_DECOMPRESSION, 1
	.equ	PREVENT_CLIP, 1

.endif
@*********** IF ENGLISH POKEMON EMERALD
.if USED_GAME==GAME_BPEE

	.equ	hq_buffer, BUFFER_IRAM_BPE
	.equ	decoder_buffer_target, DECODER_BUFFER_BPE
	.equ	ALLOW_PAUSE, 1
	.equ	DMA_FIX, 1
	.equ	ENABLE_DECOMPRESSION, 1
	.equ	PREVENT_CLIP, 1

.endif
@*********** IF ENGLISH POKEMON FIRE RED
.if USED_GAME==GAME_BPRE

	.equ	hq_buffer, BUFFER_IRAM_BPR
	.equ	decoder_buffer_target, DECODER_BUFFER_BPR
	.equ	ALLOW_PAUSE, 1
	.equ	DMA_FIX, 1
	.equ	ENABLE_DECOMPRESSION, 1
	.equ	PREVENT_CLIP, 1

.endif
@*********** IF KAWAs JUKEBOX 2006
.if USED_GAME==GAME_KWJ6

	.equ	hq_buffer, BUFFER_IRAM_KWJ
	.equ	decoder_buffer_target, DECODER_BUFFER_KWJ
	.equ	ALLOW_PAUSE, 0
	.equ	DMA_FIX, 0
	.equ	ENABLE_DECOMPRESSION, 0
	.equ	PREVENT_CLIP, 1

.endif
@*********** IF US FIRE EMBLEM (7)
.if USED_GAME==GAME_AE7E

	.equ	hq_buffer, BUFFER_IRAM_AE7
	.equ	ALLOW_PAUSE, 0
	.equ	DMA_FIX, 0
	.equ	ENABLE_DECOMPRESSION, 0
	.equ	PREVENT_CLIP, 0

.endif
@*********** IF US FIRE EMBLEM : THE SACRED STONES
.if USED_GAME==GAME_BE8E

	@ FE8U support by circleseverywhere

	.equ	hq_buffer, BUFFER_IRAM_BE8
	.equ	ALLOW_PAUSE, 0
	.equ	DMA_FIX, 0
	.equ	ENABLE_DECOMPRESSION, 0
	.equ	PREVENT_CLIP, 0

.endif
@***********

	.thumb

main_mixer:
    /* load Reverb level and check if we need to apply it */
    LDRB	R3, [R0, #VAR_REVERB]
    LSR	R3, R3, #2
    BEQ	clear_buffer

    ADR	R1, do_reverb
    BX	R1

	.align	2
	.arm

do_reverb:

    /* 
     * reverb is calculated by the following: new_sample = old_sample * reverb_level / 127
     * note that reverb is mono (both sides get mixed together)
     * 
     * reverb get's applied to the frame we are currently looking at and the one after that
     * the magic below simply calculateds the pointer for the one after the current one
     */

    CMP	R4, #2
    ADDEQ	R7, R0, #0x350
    ADDNE	R7, R5, R8
    MOV	R4, R8
    ORR	R3, R3, R3, LSL#16			
    STMFD	SP!, {R8, LR}
    LDR	LR, hq_buffer_label

reverb_loop:
        /* This loop does the reverb processing */
        LDRSB	R0, [R5, R6]
        LDRSB	R1, [R5], #1
        LDRSB	R2, [R7, R6]
        LDRSB	R8, [R7], #1
        LDRSB	R9, [R5, R6]
        LDRSB	R10, [R5], #1
        LDRSB	R11, [R7, R6]
        LDRSB	R12, [R7], #1
        ADD	R0, R0, R1
        ADD	R0, R0, R2
        ADDS	R0, R0, R8
        ADDMI	R0, R0, #0x4
        ADD	R1, R9, R10
        ADD	R1, R1, R11
        ADDS	R1, R1, R12
        ADDMI	R1, R1, #0x4
        MUL	R0, R3, R0
        MUL	R1, R3, R1
        STMIA	LR!, {R0, R1}
        SUBS	R4, R4, #2
        BGT	reverb_loop
        /* end of loop */
    LDMFD	SP!, {R8, LR}
    ADR	R0, (adsr_setup+1)
    BX	R0

	.thumb

clear_buffer:
    /* Incase reverb is disabled the buffer get's set to zero */
    LDR	R3, hq_buffer_label
    MOV	R1, R8
    MOV	R4, #0
    MOV	R5, #0
    MOV	R6, #0
    MOV	R7, #0
    /*
     * Setting the buffer to zero happens in a very efficient loop
     * Depending on the alignment of the buffer length, twice or quadruple the amount of bytes
     * get cleared at once
     */
    LSR	R1, #3
    BCC	clear_buffer_align_8

    STMIA	R3!, {R4, R5, R6, R7}

clear_buffer_align_8:

    LSR	R1, #1
    BCC	clear_buffer_align_16

    STMIA	R3!, {R4, R5, R6, R7}
    STMIA	R3!, {R4, R5, R6, R7}

clear_buffer_align_16:
        /* This repeats until the buffer has been cleared */
        STMIA	R3!, {R4, R5, R6, R7}
        STMIA	R3!, {R4, R5, R6, R7}
        STMIA	R3!, {R4, R5, R6, R7}
        STMIA	R3!, {R4, R5, R6, R7}
        SUB	    R1, #1
        BGT	    clear_buffer_align_16
        /* loop end */
adsr_setup:
    /*
     * okay, before the actual mixing starts
     * the volume and envelope calculation happens
     */
    MOV R4, R8  @ R4 = buffer length
    /* this buffers the buffer length to a backup location
     * TODO: Move this variable to stack
     */
    ADR	R0, hq_buffer_length_label
    STR	R4, [R0]
    /* init channel loop */
    LDR	R4, [SP, #ARG_VAR_AREA]	        @ R4 = main work area pointer
    LDR	R0, [R4, #VAR_DEF_PITCH_FAC]	@ R0 = samplingrate pitch factor
    MOV	R12, R0					        @ --> R12
    LDRB R0, [R4, #VAR_MAX_CHN]		    @ load MAX channels to R0
    ADD	R4, #VAR_FIRST_CHN  			@ R4 = Base channel Offset (Channel #0)

mixer_entry:
        /* this is the main channel processing loop */
        STR	R0, [SP, #ARG_REMAIN_CHN]		
        LDR	R3, [R4, #CHN_WAVE_OFFSET]
        LDRB R6, [R4, #CHN_STATUS]
        MOVS R0, #0xC7					@ check if any of the channel status flags is set
        TST	R0, R6						@ check if none of the flags is set
        BEQ return_channel_null 		@ skip channel
        /* check channel flags */
        LSL	R0, R6, #25 				@ shift over the FLAG_CHN_INIT to CARRY
        BCC	adsr_echo_check				@ continue with normal channel procedure
        /* check leftmost bit */
        BMI	stop_channel_handler		@ if the channel is initiated but on release it gets turned off immediatley
        /* channel init procedure */
        MOVS R6, #FLAG_CHN_ATTACK		@ set the channel status to ATTACK
        MOVS R0, R3						@ R0 = CHN_WAVE_OFFSET
        ADD	R0, #WAVE_DATA				@ R0 = wave data offset

        /* Pokemon games seem to init channels differently than other m4a games */
    .if ALLOW_PAUSE==0
        STR	R0, [R4, #CHN_POSITION_ABS]
        LDR	R0, [R3, #WAVE_LENGTH]
        STR	R0, [R4, #CHN_POSITION_REL] 
    .else
        LDR	R1, [R4, #CHN_POSITION_REL]
        ADD	R0, R0, R1
        STR	R0, [R4, #CHN_POSITION_ABS]
        LDR	R0, [R3, #WAVE_LENGTH]
        SUB	R0, R0, R1
        STR	R0, [R4, #CHN_POSITION_REL]
    .endif

        MOVS R5, #0						@ initial envelope = #0
        STRB R5, [R4, #CHN_ADSR_LEVEL]
        STR	R5, [R4, #CHN_FINE_POSITION]
        LDRB R2, [R3, #WAVE_LOOP_FLAG]
        LSR	R0, R2, #6
        BEQ	adsr_attack_handler         @ if loop disabled --> branch
        /* loop enabled here */
        MOVS R0, #FLAG_CHN_LOOP	
        ORR	R6, R0      				@ update channel status
        B adsr_attack_handler

adsr_echo_check:
        /* this is the normal ADSR procedure without init */
        LDRB R5, [R4, #CHN_ADSR_LEVEL]
        LSL	R0, R6, #29				    @ echo flag --> bit 31
        BPL	adsr_release_check			@ PL == false
        /* pseudo echo handler */
        LDRB R0, [R4, #CHN_ECHO_REMAIN]
        SUB	R0, #1
        STRB R0, [R4, #CHN_ECHO_REMAIN]
        BHI	channel_vol_calc			@ if echo still on --> branch

stop_channel_handler:

        MOVS R0, #0
        STRB R0, [R4, #CHN_STATUS]

return_channel_null:
        /* go to end of the channel loop */
        B check_remain_channels

adsr_release_check:
        LSL	R0, R6, #25					@ bit 31 = release bit
        BPL	adsr_decay_check			@ if release == 0 --> branch
        /* release handler */
        LDRB R0, [R4, #CHN_RELEASE]
        @SUB R0, #0xFF                  @ linear decay; TODO make option for triggering it
        @SUB R0, #1
        @ADD R5, R5, R0
        MUL	R5, R5, R0	            	@ default release algorithm
        LSR	R5, R5, #8
        @BMI adsr_released_handler      @ part of linear decay
        BEQ	adsr_released_handler	    @ release gone down to #0 --> branch
        /* pseudo echo init handler */
        LDRB R0, [R4, #CHN_ECHO_VOL]
        CMP	R5, R0
        BHI	channel_vol_calc            @ if release still above echo level --> branch

adsr_released_handler:
        /* if volume released to #0 */
        LDRB R5, [R4, #CHN_ECHO_VOL]    @ TODO: replace with MOV R5, R0
        CMP	R5, #0
        BEQ	stop_channel_handler        @ if pseudo echo vol = 0 --> branch
        /* pseudo echo volume handler */
        MOVS R0, #FLAG_CHN_ECHO
        ORR	R6, R0						@ set the echo flag
        B adsr_update_status

adsr_decay_check:
        /* check if decay is active */
        MOVS R2, #3
        AND	R2, R6                      @ seperate phase status bits
        CMP	R2, #FLAG_CHN_DECAY
        BNE	adsr_attack_check			@ decay not active --> branch
        /* decay handler */
        LDRB R0, [R4, #CHN_DECAY]
        MUL	R5, R0
        LSR	R5, R5, #8
        LDRB R0, [R4, #CHN_SUSTAIN]
        CMP	R5, R0
        BHI	channel_vol_calc		    @ sample didn't decay yet --> branch
        /* sustain handler */
        MOVS R5, R0						@ current level = sustain level
        BEQ	adsr_released_handler       @ sustain level #0 --> branch
        /* step to next phase otherweise */
        B adsr_switchto_next

adsr_attack_check:
        /* attack handler */
        CMP	R2, #FLAG_CHN_ATTACK
        BNE	channel_vol_calc			@ if it isn't in attack attack phase, it has to be in sustain (no adsr change needed) --> branch

adsr_attack_handler:
        /* apply attack summand */
        LDRB R0, [R4, #CHN_ATTACK]
        ADD	R5, R5, R0
        CMP	R5, #0xFF
        BCC	adsr_update_status
        /* cap attack at 0xFF */
        MOVS R5, #0xFF
 
adsr_switchto_next:
        /* switch to next adsr phase */
        SUB	R6, #1

adsr_update_status:
        /* store channel status */
        STRB R6, [R4, #CHN_STATUS]

channel_vol_calc:
        /* store the calculated ADSR level */
        STRB R5, [R4, #CHN_ADSR_LEVEL]
        /* apply master volume */
        LDR	R0, [SP, #ARG_VAR_AREA]
        LDRB R0, [R0, #VAR_MASTER_VOL]
        ADD	R0, #1
        MUL	R5, R0, R5
        /* left side volume */
        LDRB R0, [R4, #CHN_VOL_2]
        MUL	R0, R5
        LSR	R0, R0, #13
        MOV	R10, R0                     @ R10 = left volume
        /* right side volume */
        LDRB R0, [R4, #CHN_VOL_1]
        MUL	R0, R5
        LSR	R0, R0, #13
        MOV	R11, R0						@ R11 = right volume
        /*
         * Now we get closer to actual mixing:
         * For looped samples some additional operations are required
         */
        MOVS R0, #FLAG_CHN_LOOP
        AND	R0, R6
        BEQ	mixing_loop_setup				@ TODO: This label should rather be called "skip_loop_setup"
        /* loop setup handler */
        ADD	R3, #WAVE_LOOP_START
        LDMIA R3!, {R0, R1}					@ R0 = loop start, R1 = loop end
        ADD	R3, R0, R3					    @ R3 = loop start position (absolute)
        STR	R3, [SP, #ARG_LOOP_START_POS]	@ backup loop start
        SUB	R0, R1, R0

mixing_loop_setup:
        /* do the rest of the setup */
        STR	R0, [SP, #ARG_LOOP_LENGTH]		@ if loop is off --> R0 = 0x0
        LDR	R5, hq_buffer_label
        LDR	R2, [R4, #CHN_POSITION_REL]		@ remaining samples for channel
        LDR	R3, [R4, #CHN_POSITION_ABS]		@ current stream position (abs)
        LDRB R0, [R4, #CHN_MODE]
        ADR	R1, mixing_arm_setup
        BX R1

	.align	2
hq_buffer_label:
	.word	hq_buffer
hq_buffer_length_label:     @ TODO: Replace with variable on stack
	.word	0xFFFFFFFF

	.arm
mixing_arm_setup:
        /* frequency and mixing loading routine */
        LDR	R8, hq_buffer_length_label
        ORRS R11, R10, R11, LSL#16		    @ R11 = 00RR00LL
        BEQ	switchto_thumb					@ volume #0 --> branch and skip channel processing
        /* normal processing otherwise */
        TST R0, #MODE_FIXED_FREQ
        BNE	fixed_mixing_setup
        TST R0, #MODE_COMP
        BNE special_mixing	                @ compressed? --> branch
        /* same here */
        STMFD SP!, {R4, R9, R12}
        /*
         * This mixer supports 4 different kind of synthesized sounds
         * They are triggered when the loop end = 0
         * This get's checked below
         */
        MOVS R2, R2
        ORREQ R0, R0, #MODE_SYNTH
        STREQB R0, [R4, #CHN_MODE]
        ADD	R4, R4, #CHN_FINE_POSITION
        LDMIA R4, {R7, LR}					@ R7 = Fine Position, LR = Frequency
        MUL	R4, R12, LR					    @ R4 = inter sample steps = output rate factor * samplerate
        /* now the first samples get loaded */
        LDRSB R6, [R3], #1
        LDRSB R12, [R3]
        TST	R0, #MODE_SYNTH
        BNE	init_synth
        /* incase no synth mode should be used, code contiues here */
        SUB	R12, R12, R6					@ R12 = DELTA
        /*
         * Mixing goes with volume ranges 0-127
         * They come in 0-255 --> divide by 2
         */
        MOVS R11, R11, LSR#1
        ADC	R11, R11, #0x8000
        BIC	R11, R11, #0xFF00
        MOV	R1, R7	    					@ R1 = inter sample position
        /*
         * There is 2 different mixing codepaths for uncompressed data
         *  path 1: fast mixing, but doesn't supports loop or stop
         *  path 2: not so fast but supports sample loops / stop
         * This checks if there is enough samples aviable for path 1.
         * important: R0 is expected to be #0
         */
        UMLAL R1, R0, R4, R8
        MOV	R1, R1, LSR#23
        ORR	R0, R1, R0, LSL#9
        CMP	R2, R0						    @ actual comparison
        BLE	split_sample_loading			@ if not enough samples are available for path 1 --> branch
        /* 
         * This is the mixer path 1.
         * The interesting thing here is that the code will
         * buffer enough samples on stack if enough space
         * on stack is available (or goes over the limit of 0x400 bytes)
         */
        SUB	R2, R2, R0
        LDR	R10, stack_capacity
        ADD	R10, R10, R0
        CMP	R10, SP
        ADD	R10, R3, R0
        ADR	R9, custom_stack_3
        /*
         * R2 = remaining samples
         * R10 = final sample position
         * SP = original stack location
         * These values will get reloaded after channel processing
         * due to the lack of registers.
         */
        STMIA	R9, {R2, R10, SP}
        CMPCC	R0, #0x400                  @ > 0x400 bytes --> read directly from ROM rather than buffered
        BCS	select_mixing_mode              @ TODO rename
        /*
         * The code below inits the DMA to read word aligned
         * samples from ROM to stack
         */
        BIC	R1, R3, #3
        MOV	R9, #0x04000000
        ADD	R9, R9, #0xD4
        ADD	R0, R0, #7
        MOV	R0, R0, LSR#2
        SUB SP, SP, R0, LSL#2
        AND	R3, R3, #3
        ADD	R3, R3, SP
        ORR	LR, R0, #0x84000000
        STMIA R9, {R1, SP, LR}              @ actually starts the DMA

        /* Somehow is neccesary for some games not to break */
    .if DMA_FIX==1
        MOV	R0, #0
        MOV	R1, R0
        MOV	R2, R1
        STMIA R9, {R0, R1, R2}
    .endif

select_mixing_mode:
        /*
         * This code decides which piece of code to load
         * depending on playback-rate / default-rate ratio.
         * Modes > 1.0 run with different volume levels.
         */
        SUBS R4, R4, #0x800000
        MOVPL R11, R11, LSL#1
        ADR	R0, math_resources				@ loads the base pointer of the code
        ADDPL R0, R0, #(ARM_OP_LEN*6)       @ 6 instructions further
        SUBPLS R4, R4, #0x800000
        ADDPL R0, R0, #(ARM_OP_LEN*6)
        ADDPL R4, R4, #0x800000				@ TODO how does restoring for > 2.0 ratios work?
        LDR	R2, function_pointer
        CMP	R0, R2						    @ code doesn't need to be reloaded if it's already in place
        BEQ	mixing_init
        /* This loads the needed code to RAM */
        STR	R0, function_pointer
        LDMIA R0, {R0-R2, R8-R10}			@ load 6 opcodes
        ADR	LR, runtime_created_routine

create_routine_loop:
            /* paste code to destination, see below for patterns */
            STMIA	LR, {R0, R1}
            ADD	LR, LR, #0x98
            STMIA	LR, {R0, R1}
            SUB	LR, LR, #0x8C
            STMIA	LR, {R2, R8-R10}
            ADD	LR, LR, #0x98
            STMIA	LR, {R2, R8-R10}
            SUB	LR, LR, #0x80
            ADDS	R5, R5, #0x40000000	    @ do that for 4 blocks
            BCC	create_routine_loop

        LDR	R8, hq_buffer_length_label

mixing_init:
        MOV	R2, #0xFF000000					@ load the fine position overflow bitmask
mixing_loop:
        /* This is the actual processing and interpolation code loop; NOPs will be replaced by the code above */
            LDMIA R5, {R0, R1, R10, LR}	@ load 4 stereo samples to Registers
            MUL	R9, R7, R12
runtime_created_routine:
            NOP							@ Block #1
            NOP
            MLANE R0, R11, R9, R0
            NOP
            NOP
            NOP
            NOP
            BIC	R7, R7, R2, ASR#1
            MULNE	R9, R7, R12
            NOP							@ Block #2
            NOP
            MLANE R1, R11, R9, R1
            NOP
            NOP
            NOP
            NOP
            BIC	R7, R7, R2, ASR#1
            MULNE R9, R7, R12
            NOP							@ Block #3
            NOP
            MLANE R10, R11, R9, R10
            NOP
            NOP
            NOP
            NOP
            BIC	R7, R7, R2, ASR#1
            MULNE R9, R7, R12
            NOP							@ Block #4
            NOP
            MLANE LR, R11, R9, LR
            NOP
            NOP
            NOP
            NOP
            BIC	R7, R7, R2, ASR#1
            STMIA R5!, {R0, R1, R10, LR}	@ write 4 stereo samples
            
            LDMIA R5, {R0, R1, R10, LR}	    @ load the next 4 stereo samples
            MULNE R9, R7, R12	
            NOP							@ Block #1
            NOP
            MLANE R0, R11, R9, R0
            NOP
            NOP
            NOP
            NOP
            BIC	R7, R7, R2, ASR#1
            MULNE R9, R7, R12
            NOP							@ Block #2
            NOP
            MLANE R1, R11, R9, R1
            NOP
            NOP
            NOP
            NOP
            BIC	R7, R7, R2, ASR#1
            MULNE R9, R7, R12
            NOP							@ Block #3
            NOP
            MLANE R10, R11, R9, R10
            NOP
            NOP
            NOP
            NOP
            BIC	R7, R7, R2, ASR#1
            MULNE R9, R7, R12
            NOP							@ Block #4
            NOP
            MLANE LR, R11, R9, LR
            NOP
            NOP
            NOP
            NOP
            BIC	R7, R7, R2, ASR#1
            STMIA R5!, {R0, R1, R10, LR}	@ write 4 stereo samples
            SUBS R8, R8, #8					@ subtract 8 from the sample count
            BGT	mixing_loop
        /* restore previously saved values */
        ADR	R12, custom_stack_3
        LDMIA R12, {R2, R3, SP}
        B mixing_end_func

@ work variables

	.align	2
custom_stack_3:
	.word	0x0, 0x0, 0x0
stack_capacity:
	.word	0x03007910
function_pointer:
	.word	0x0

@ math resources, not directly used

math_resources:

MOV	R9, R9, ASR#22					@ Frequency Lower than default Frequency
ADDS	R9, R9, R6, LSL#1
ADDS	R7, R7, R4
ADDPL	R6, R12, R6
LDRPLSB	R12, [R3, #1]!
SUBPLS	R12, R12, R6

ADDS	R9, R6, R9, ASR#23				@ Frequency < 2x && Frequency > default frequency
ADD	R6, R12, R6
ADDS	R7, R7, R4
LDRPLSB	R6, [R3, #1]!
LDRSB	R12, [R3, #1]!
SUBS	R12, R12, R6

ADDS	R9, R6, R9, ASR#23				@ Frequency >= 2x higher than default Frequency
ADD	R7, R7, R4
ADD	R3, R3, R7, LSR#23
LDRSB	R6, [R3]
LDRSB	R12, [R3, #1]!
SUBS	R12, R12, R6

split_sample_loading:

ADD	R5, R5, R8, LSL#2				@ R5 = End of HQ buffer

uncached_mixing_loop:

MUL	R9, R7, R12					@ calc interpolated DELTA
MOV	R9, R9, ASR#22					@ scale down the DELTA
ADDS	R9, R9, R6, LSL#1				@ Add to Base Sample (upscaled to 8 bits again)
LDRNE	R0, [R5, -R8, LSL#2]				@ load sample from buffer
MLANE	R0, R11, R9, R0					@ add it to the buffer sample
STRNE	R0, [R5, -R8, LSL#2]				@ write the sample
ADD	R7, R7, R4					@ add the step size to the fine position
MOVS	R9, R7, LSR#23					@ write the overflow amount to R9
BEQ	uncached_mixing_load_skip			@ skip the mixing load if it isn't required

SUBS	R2, R2, R7, LSR#23				@ remove the overflow count from the remaning samples
BLLE	loop_end_sub					@ if the loop end is reached call the loop handler
SUBS	R9, R9, #1					@ remove #1 from the overflow count
ADDEQ	R6, R12, R6					@ new base sample is previous sample + DELTA
@RETURN LOCATION FROM LOOP HANDLER
LDRNESB	R6, [R3, R9]!					@ load new sample
LDRSB	R12, [R3, #1]!					@ load the delta sample (always required)
SUB	R12, R12, R6					@ calc new DELTA
BIC	R7, R7, #0x3F800000				@ clear the overflow from the fine position by using the bitmask

uncached_mixing_load_skip:

SUBS	R8, R8, #1					@ reduce the sample count for the buffer by #1
BGT	uncached_mixing_loop

mixing_end_func:

SUB	R3, R3, #1					@ reduce sample pointer by #1 (???)
LDMFD	SP!, {R4, R9, R12}				@ pop values from stack
STR	R7, [R4, #CHN_FINE_POSITION]			@ store the fine position
B	store_coarse_sample_pos				@ jump over to code to store coarse channel position

loop_end_sub:

ADD	R3, SP, #ARG_LOOP_START_POS+0xC			@ prepare sample loop start loading and lopo length loading (0xC due to the pushed stack pointer)
LDMIA	R3, {R3, R6}					@ R3 = Loop Start; R6 = Loop Length
CMP	R6, #0						@ check if loop is enabled; if Loop is enabled R6 is != 0
RSBNE	R9, R2, #0					@ the sample overflow from the resampling needs to get subtracted so the remaining samples is slightly less
ADDNE	R2, R6, R2					@ R2 = add the loop length
ADDNE	PC, LR, #8					@ return from the subroutine to 2 instructions after the actual return location
LDMFD	SP!, {R4, R9, R12}				@ restore registers from stack
B	update_channel_status

fixed_freq_loop_end_handler:

LDR	R2, [SP, #ARG_LOOP_LENGTH+0x8]			@ load the loop length value
MOVS	R6, R2						@ copy it to R6 and check if loop is disabled
LDRNE	R3, [SP, #ARG_LOOP_START_POS+0x8]		@ reset the sample pointer to the loop start position
BXNE	LR						@ if it loops return to mixing function, if it doesn't go on and end mixing

LDMFD	SP!, {R4, R9}

update_channel_status:

STRB	R6, [R4]					@ if loop ist disabled R6 = 0 and we can disable the channel by writing R6 to R4 (channel area)
B	switchto_thumb					@ switch to thumb

fixed_math_resource:	@ not exectued, used to create mixing function

MOVS	R6, R10, LSL#24
MOVS	R6, R6, ASR#24
MOVS	R6, R10, LSL#16
MOVS	R6, R6, ASR#24
MOVS	R6, R10, LSL#8
MOVS	R6, R6, ASR#24
MOVS	R6, R10, ASR#24
LDMIA	R3!, {R10}					@ load chunk of samples
MOVS	R6, R10, LSL#24
MOVS	R6, R6, ASR#24
MOVS	R6, R10, LSL#16
MOVS	R6, R6, ASR#24
MOVS	R6, R10, LSL#8
MOVS	R6, R6, ASR#24
LDMFD	SP!, {R4, R9, R12}

fixed_mixing_setup:

STMFD	SP!, {R4, R9}					@ backup the channel pointer and 

fixed_mixing_check_length:

MOV	LR, R2						@ move absolute sample position to LR
CMP	R2, R8						@ 
MOVGT	LR, R8						@ if there is less samples than the buffer to process write the smaller sample amount to LR
SUB	LR, LR, #1					@ shorten samples to process by #1
MOVS	LR, LR, LSR#2					@ calculate the amount of words to process (-1/4)
BEQ	fixed_mixing_process_unaligned			@ process the unaligned samples if there is <= 3 samples to process

SUB	R8, R8, LR, LSL#2				@ subtract the amount of samples we need to process from the buffer length
SUB	R2, R2, LR, LSL#2				@ subtract the amount of samples we need to process from the remaining samples
ADR	R1, fixed_mixing_custom_routine
ADR	R0, fixed_math_resource				@ load the 2 pointers to create function (@R0) by instructions from R1
MOV	R9, R3, LSL#30					@ move sample alignment bits to the leftmost position
ADD	R0, R0, R9, LSR#27				@ alignment * 8 + resource offset = new resource offset
LDMIA	R0!, {R6, R7, R9, R10}				@ load 4 instructions
STMIA	R1, {R6, R7}					@ write the 1st 2 instructions
ADD	R1, R1, #0xC					@ move label pointer over to the next slot
STMIA	R1, {R9, R10}					@ write 2nd block
ADD	R1, R1, #0xC					@ move label pointer to next block
LDMIA	R0, {R6, R7, R9, R10}				@ load instructions for block #3 and #4
STMIA	R1, {R6, R7}					@ write block #3
ADD	R1, R1, #0xC					@ ...
STMIA	R1, {R9, R10}					@ write block #4
LDMIA	R3!, {R10}					@ write read 4 samples from ROM

fixed_mixing_loop:

LDMIA	R5, {R0, R1, R7, R9}				@ load 4 samples from hq buffer

fixed_mixing_custom_routine:

NOP
NOP
MLANE	R0, R11, R6, R0					@ add new sample if neccessary
NOP
NOP
MLANE	R1, R11, R6, R1
NOP
NOP
MLANE	R7, R11, R6, R7
NOP
NOP
MLANE	R9, R11, R6, R9
STMIA	R5!, {R0, R1, R7, R9}				@ write the samples to the work area buffer
SUBS	LR, LR, #1					@ countdown the sample blocks to process
BNE	fixed_mixing_loop				@ if the end wasn't reached yet, repeat the loop

SUB	R3, R3, #4					@ reduce sample position by #4, we'll need to load the samples again

fixed_mixing_process_unaligned:

MOV	R1, #4						@ we need to repeat the loop #4 times to completley get rid of alignment errors

fixed_mixing_unaligned_loop:

LDR	R0, [R5]					@ load sample from buffer
LDRSB	R6, [R3], #1					@ load sample from ROM ro R6
MLA	R0, R11, R6, R0					@ write the sample to the buffer
STR	R0, [R5], #4
SUBS	R2, R2, #1					@ reduce alignment error by #1
BLEQ	fixed_freq_loop_end_handler
SUBS	R1, R1, #1
BGT	fixed_mixing_unaligned_loop			@ repeat the loop #4 times

SUBS	R8, R8, #4					@ reduce the sample amount we wrote to the buffer by #1
BGT	fixed_mixing_check_length			@ go up to repeat the mixing procedure until the buffer is filled

LDMFD	SP!, {R4, R9}					@ pop registers from stack

store_coarse_sample_pos:

STR	R2, [R4, #CHN_POSITION_REL]			@ store relative and absolute sample position
STR	R3, [R4, #CHN_POSITION_ABS]			

switchto_thumb:

ADR	R0, (check_remain_channels+1)			@ load the label offset and switch to thumb
BX	R0

	.thumb

check_remain_channels:

LDR	R0, [SP, #ARG_REMAIN_CHN]			@ load the remaining channels
SUB	R0, #1						@ reduce the amount by #1
BLE	mixer_return					@ end the mixing when finished processing all channels

ADD	R4, #0x40
B	mixer_entry

mixer_return:

ADR	R0, downsampler
BX	R0

downsampler_return:

LDR	R0, [SP, #ARG_VAR_AREA]			@ load the main var area to R0
LDR	R3, mixer_finished_status		@ load some status indication value to R3
STR	R3, [R0]				@ store this value to the main var area
ADD	SP, SP, #0x1C
POP	{R0-R7}
MOV	R8, R0
MOV	R9, R1
MOV	R10, R2
MOV	R11, R3
POP	{R3}
BX	R3

	.align	2

mixer_finished_status:
	.word	0x68736D53

	.arm

downsampler:

LDR	R10, hq_buffer_label
LDR	R9, [SP, #ARG_BUFFER_POS]
LDR	R8, hq_buffer_length_label
MOV	R11, #0xFF
.if PREVENT_CLIP==1

MOV	R12, #0xFFFFFFFF
MOV	R12, R12, LSL#14
MOV	R7, #0x630

downsampler_loop:

LDRSH	R2, [R10], #2
LDRSH	R0, [R10], #2
LDRSH	R3, [R10], #2
LDRSH	R1, [R10], #2

CMP	R0, #0x4000
MOVGE	R0, #0x3F80
CMP	R0, #-0x4000
MOVLT	R0, R12

CMP	R1, #0x4000
MOVGE	R1, #0x3F80
CMP	R1, #-0x4000
MOVLT	R1, R12

CMP	R2, #0x4000
MOVGE	R2, #0x3F80
CMP	R2, #-0x4000
MOVLT	R2, R12

CMP	R3, #0x4000
MOVGE	R3, #0x3F80
CMP	R3, #-0x4000
MOVLT	R3, R12

AND	R0, R11, R0, ASR#7
AND	R1, R11, R1, ASR#7
AND	R2, R11, R2, ASR#7
AND	R3, R11, R3, ASR#7

ORR	R2, R2, R3, LSL#8
ORR	R0, R0, R1, LSL#8

STRH	R2, [R9, R7]
STRH	R0, [R9], #2

SUBS	R8, #2
BGT	downsampler_loop

.else
downsampler_loop:

LDRH	R4, [R10], #2
LDRH	R0, [R10], #2
LDRH	R5, [R10], #2
LDRH	R1, [R10], #2
LDRH	R6, [R10], #2
LDRH	R2, [R10], #2
LDRH	R7, [R10], #2
LDRH	R3, [R10], #2

AND	R0, R11, R0, LSR#7
AND	R1, R11, R1, LSR#7
AND	R2, R11, R2, LSR#7
AND	R3, R11, R3, LSR#7
AND	R4, R11, R4, LSR#7
AND	R5, R11, R5, LSR#7
AND	R6, R11, R6, LSR#7
AND	R7, R11, R7, LSR#7

ORR	R4, R4, R5, LSL#8
ORR	R4, R4, R6, LSL#16
ORR	R4, R4, R7, LSL#24

ORR	R0, R0, R1, LSL#8
ORR	R0, R0, R2, LSL#16
ORR	R0, R0, R3, LSL#24

STR	R4, [R9, #0x630]
STR	R0, [R9], #4

SUBS	R8, #4
BGT	downsampler_loop

.endif

ADR	R0, (downsampler_return+1)
BX	R0

	.align	2

init_synth:

CMP	R12, #0		@ $030057C4
BNE	check_synth_type

LDRB	R6, [R3, #SYNTH_WIDTH_CHANGE_1]			@ for saw wave -> 0xF0 (base duty cycle change)
ADD	R2, R2, R6, LSL#24				@ add it to the current synt
LDRB	R6, [R3, #SYNTH_WIDTH_CHANGE_2]			@ for saw wave -> 0x80 (base duty cycle change #2)
ADDS	R6, R2, R6, LSL#24				@ add this to the synth state aswell but keep the old value in R2 and put the new one in R6
MVNMI	R6, R6	 					@ negate if duty cycle is > 50%
MOV	R10, R6, LSR#8					@ dividide the final duty cycle by 8 to R10
LDRB	R1, [R3, #SYNTH_MOD_AMOUNT]			@ for saw wave -> 0xE0
LDRB	R0, [R3, #SYNTH_BASE_WAVE_DUTY]			@ for saw wave -> 0x10 (base duty cycle offset)
MOV	R0, R0, LSL#24					@ convert it to a usable duty cycle
MLA	R6, R10, R1, R0					@ calculate the final duty cycle with the offset, and intensity * rotating duty cycle amount
STMFD	SP!, {R2, R3, R9, R12}

synth_type_0_loop:

LDMIA	R5, {R0-R3, R9, R10, R12, LR}			@ load 8 samples
CMP	R7, R6						@ Block #1
ADDCC	R0, R0, R11, LSL#6
SUBCS	R0, R0, R11, LSL#6
ADDS	R7, R7, R4, LSL#3
CMP	R7, R6						@ Block #2
ADDCC	R1, R1, R11, LSL#6
SUBCS	R1, R1, R11, LSL#6
ADDS	R7, R7, R4, LSL#3
CMP	R7, R6						@ Block #3
ADDCC	R2, R2, R11, LSL#6
SUBCS	R2, R2, R11, LSL#6
ADDS	R7, R7, R4, LSL#3
CMP	R7, R6						@ Block #4
ADDCC	R3, R3, R11, LSL#6
SUBCS	R3, R3, R11, LSL#6
ADDS	R7, R7, R4, LSL#3
CMP	R7, R6						@ Block #5
ADDCC	R9, R9, R11, LSL#6
SUBCS	R9, R9, R11, LSL#6
ADDS	R7, R7, R4, LSL#3
CMP	R7, R6						@ Block #6
ADDCC	R10, R10, R11, LSL#6
SUBCS	R10, R10, R11, LSL#6
ADDS	R7, R7, R4, LSL#3
CMP	R7, R6						@ Block #7
ADDCC	R12, R12, R11, LSL#6
SUBCS	R12, R12, R11, LSL#6
ADDS	R7, R7, R4, LSL#3
CMP	R7, R6						@ Block #8
ADDCC	LR, LR, R11, LSL#6
SUBCS	LR, LR, R11, LSL#6
ADDS	R7, R7, R4, LSL#3

STMIA	R5!, {R0-R3, R9, R10, R12, LR}			@ write 8 samples
SUBS	R8, R8, #8					@ remove #8 from sample count
BGT	synth_type_0_loop

LDMFD	SP!, {R2, R3, R9, R12}
B	mixing_end_func

check_synth_type:

SUBS	R12, R12, #1					@ remove #1 from the synth type byte and check if it's #0
BNE	synth_type_2					@ if it still isn't it's synth type 2 (smooth pan flute)

MOV	R6, #0x300					@ R6 = 0x300
MOV	R11, R11, LSR#1					@ halve the volume
BIC	R11, R11, #0xFF00				@ clear bad bits from division
MOV	R12, #0x70					@ R12 = 0x70

synth_type_1_loop:

LDMIA	R5, {R0, R1, R10, LR}				@ load 4 samples from memory
ADDS	R7, R7, R4, LSL#3				@ Block #1 (some oscillator type code)
RSB	R9, R12, R7, LSR#24
MOV	R6, R7, LSL#1
SUB	R9, R9, R6, LSR#27
ADDS	R2, R9, R2, ASR#1
MLANE	R0, R11, R2, R0

ADDS	R7, R7, R4, LSL#3				@ Block #2
RSB	R9, R12, R7, LSR#24
MOV	R6, R7, LSL#1
SUB	R9, R9, R6, LSR#27
ADDS	R2, R9, R2, ASR#1
MLANE	R1, R11, R2, R1

ADDS	R7, R7, R4, LSL#3				@ Block #3
RSB	R9, R12, R7, LSR#24
MOV	R6, R7, LSL#1
SUB	R9, R9, R6, LSR#27
ADDS	R2, R9, R2, ASR#1
MLANE	R10, R11, R2, R10

ADDS	R7, R7, R4, LSL#3				@ Block #4
RSB	R9, R12, R7, LSR#24
MOV	R6, R7, LSL#1
SUB	R9, R9, R6, LSR#27
ADDS	R2, R9, R2, ASR#1
MLANE	LR, R11, R2, LR

STMIA	R5!, {R0, R1, R10, LR}
SUBS	R8, R8, #4
BGT	synth_type_1_loop

B	mixing_end_func					@ goto end

synth_type_2:

MOV	R6, #0x80					@ write base values to the registers
MOV	R12, #0x180

synth_type_2_loop:

LDMIA	R5, {R0, R1, R10, LR}				@ load samples from work buffer
ADDS	R7, R7, R4, LSL#3				@ Block #1
RSBPL	R9, R6, R7, ASR#23
SUBMI	R9, R12, R7, LSR#23
MLA	R0, R11, R9, R0

ADDS	R7, R7, R4, LSL#3				@ Block #2
RSBPL	R9, R6, R7, ASR#23
SUBMI	R9, R12, R7, LSR#23
MLA	R1, R11, R9, R1

ADDS	R7, R7, R4, LSL#3				@ Block #3
RSBPL	R9, R6, R7, ASR#23
SUBMI	R9, R12, R7, LSR#23
MLA	R10, R11, R9, R10

ADDS	R7, R7, R4, LSL#3				@ Block #4
RSBPL	R9, R6, R7, ASR#23
SUBMI	R9, R12, R7, LSR#23
MLA	LR, R11, R9, LR

STMIA	R5!, {R0, R1, R10, LR}				@ store the samples back to the buffer
SUBS	R8, R8, #4					@ subtract #4 from the remainging samples
BGT	synth_type_2_loop

B	mixing_end_func

@****************** SPECIAL MIXING ******************@
.if ENABLE_DECOMPRESSION==1
special_mixing:		@ $03006BF8

LDR	R6, [R4, #CHN_WAVE_OFFSET]		@ load the wave header offset to R6
LDRB	R0, [R4]
TST	R0, #FLAG_CHN_COMP			@ check if the channel is initialized
BNE	setup_compressed_mixing_frequency	@ skip the setup procedure if it's running in compressed mode already

ORR	R0, R0, #FLAG_CHN_COMP			@ enable the flag in the channel status
STRB	R0, [R4]				@ store the channel status
LDRB	R0, [R4, #CHN_MODE]			@ load the channel mode byte
TST	R0, #MODE_REVERSE			@ check if reverse mode is not enabled

BEQ	determine_compression			@ if Reverse Mode isn't enabled we can directly check if the sample has to get decoded

LDR	R1, [R6, #WAVE_LENGTH]			@ load the amount of samples
ADD	R1, R1, R6, LSL#1			@ do some start position calculation (???)
ADD	R1, R1, #0x20
SUB	R3, R1, R3
STR	R3, [R4, #CHN_POSITION_ABS]		@ store the final seek position

determine_compression:

LDRH	R0, [R6]				@ load the compression flag from the sample header
CMP	R0, #0					@ check if the compression is not enabled
BEQ	setup_compressed_mixing_frequency	@ skip the compression handler

SUB	R3, R3, R6				@ calc initial position
SUB	R3, R3, #0x10
STR	R3, [R4, #CHN_POSITION_ABS]		@ store the inital position (relative, not absolute)

setup_compressed_mixing_frequency:

STMFD	SP!, {R4, R9, R12}

MOVS	R11, R11, LSR#1				@ divide master volume by 2
ADC	R11, R11, #0x8000
BIC	R11, R11, #0xFF00

LDR	R7, [R4, #CHN_FINE_POSITION]		@ load the fine position
LDR	R1, [R4, #CHN_FREQUENCY]		@ load the channel frequency
LDRB	R0, [R4, #CHN_MODE]			@ load the channel mode again
TST	R0, #MODE_FIXED_FREQ			@ check if fixed frequency mode is enabled
MOVNE	R1, #0x800000				@ ### SAMPLE STEP FREQUENCY CHANGED TO R7
MULEQ	R1, R12, R1				@ default rate factor * frequency = sample steps

ADD	R5, R5, R8, LSL#2			@ set the buffer pointer to the end of the channel

LDRH	R0, [R6]				@ load the codec type
CMP	R0, #0					@ check if compression is disabled
BEQ	uncompressed_mixing_reverse_check

MOV	R0, #0xFF000000				@ set the current decoding block to "something very high" so that the first block always gets decoded
STR	R0, [R4, #CHN_BLOCK_COUNT]		@ write the last decoded block into the channel vars
LDRB	R0, [R4, #CHN_MODE]			@ check again if reverse mode is enabled
TST	R0, #MODE_REVERSE			@ test if reverse mode is enabled
BNE	compressed_mixing_reverse_init		@ check again of reverse mixing is enabled

BL	bdpcm_decoder				@ load a sample from the stream to R12
MOV	R6, R12					@ move the base sample to R6
ADD	R3, R3, #1				@ increase stream position by #1
BL	bdpcm_decoder				@ load the delta sample and calculate delta value
SUB	R12, R12, R6

@***** MIXING LOOP REGISTER USAGE ***********@
@ R0:	Sample to modify from buffer
@ R1:	sample steps		(MOVED FROM R4)
@ R2:	remaining samples before loop/end
@ R3:	sample position
@ R4:	channel pointer
@ R5:	pointer to the end of buffer
@ R6:	Base sample
@ R7:	fine position
@ R8:	remaining samples for current buffer
@ R9:	interpolated sample
@ R10:	not used
@ R11:	volume
@ R12:	Delta Sample
@ LR:	not used
@********************************************@

compressed_mixing_loop:

MUL	R9, R7, R12				@ delta sample * fine position = interpolated DELTA
MOV	R9, R9, ASR#22				@ scale down the sample
ADDS	R9, R9, R6, LSL#1			@ double the base sample and add it to the interpolated downscaled DELTA
LDRNE	R0, [R5, -R8, LSL#2]			@ if the sample is NOT 0 load the sample from buffer and store the calulated value
MLANE	R0, R11, R9, R0				@ add the sample to the buffer sample and apply volume
STRNE	R0, [R5, -R8, LSL#2]			@ store the sample if it's not Zero
ADD	R7, R7, R1				@ ### changed from R4 to R1
MOVS	R9, R7, LSR#23				@ check if there is new samples to load

BEQ	compressed_mixing_load_skip		@ no new samples need to be loaded

SUBS	R2, R2, R7, LSR#23			@ remove the sample overflow from the remaining samples
BLLE	loop_end_sub				@ call the loop/ending handler if the countdown reached zero or something negative
SUBS	R9, R9, #1				@ check if only one sample has to get loaded
ADDEQ	R6, R12, R6				@ if this is the case we can calculate the new base sample
BEQ	compressed_mixing_base_load_skip

ADD	R3, R3, R9				@ these opcodes are equivalent to LDRNESB R6, [R3, R9]!
BL	bdpcm_decoder
MOV	R6, R12

compressed_mixing_base_load_skip:

ADD	R3, R3, #1					@ equivalent to LDRSB	R12, [R3, #1]!
BL	bdpcm_decoder
SUB	R12, R12, R6
BIC	R7, R7, #0x3F800000			@ clear the overflow bits by using the according bitmask

compressed_mixing_load_skip:

SUBS	R8, R8, #1				@ remove #1 from the remaining samples
BGT	compressed_mixing_loop

@SUB	R3, R3, #1				@ sample pointer -1 (???); ALREADY DONE BY mixing_end_func
B	mixing_end_func




compressed_mixing_reverse_init:

SUB	R3, R3, #1				@ subtract one from the reverse playback location initially
BL	bdpcm_decoder				@ fetch a sample from stream
MOV	R6, R12					@ bdpcm_decoder returns base sample in R12 --> R6
SUB	R3, R3, #1				@ seek one sample further backwards
BL	bdpcm_decoder				@ detch the DELTA sample
SUB	R12, R12, R6				@ calc the Delta value

compressed_mixing_reverse_loop:

MUL	R9, R7, R12				@ delta sample * fine position = interpolated DELTA
MOV	R9, R9, ASR#22				@ scale down the sample
ADDS	R9, R9, R6, LSL#1			@ double the base sample and add it to the interpolated downscaled DELTA
LDRNE	R0, [R5, -R8, LSL#2]			@ if the sample is NOT 0 load the sample from buffer and store the calulated value
MLANE	R0, R11, R9, R0				@ add the sample to the buffer sample and apply volume
STRNE	R0, [R5, -R8, LSL#2]			@ store the sample if it's not Zero
ADD	R7, R7, R1				@ ### changed from R4 to R1
MOVS	R9, R7, LSR#23				@ check if there is new samples to load

BEQ	compressed_mixing_reverse_load_skip	@ skip sample loading if we don't need to load new samples from ROM

SUBS	R2, R2, R7, LSR#23			@ remove the overflowed samples from the remaining samples
BLLE	loop_end_sub				@ if the sample playback finished go to end handler

SUBS	R9, R9, #1				@ remove sample overflow count by #1
ADDEQ	R6, R12, R6				@ make the previous delta sample the new base sample if only #1 sample needs to get loaded
BEQ	compressed_mixing_reverse_base_load_skip @skip base sample loading

SUB	R3, R3, R9				@
BL	bdpcm_decoder				@
MOV	R6, R12					@

compressed_mixing_reverse_base_load_skip:

SUB	R3, R3, #1
BL	bdpcm_decoder
SUB	R12, R12, R6				@ load next samples???
BIC	R7, R7, #0x3F800000			@ clear overflow bits

compressed_mixing_reverse_load_skip:

SUBS	R8, R8, #1
BGT	compressed_mixing_reverse_loop

@ADD	R3, R3, #2				@ ???, copied from original code
ADD	R3, R3, #3

B	mixing_end_func


uncompressed_mixing_reverse_check:

LDRB	R0, [R4, #1]				@ load the channel mode		=$03006D84
TST	R0, #MODE_REVERSE			@ check if reverse mode is even enabled
BEQ	mixing_end_func				@ skip the channel if the mode is "akward"

LDRSB	R6, [R3, #-1]!				@ load first negative sample
LDRSB	R12, [R3, #-1]				@ load the DELTA sample
SUB	R12, R12, R6				@ calculate DELTA

reverse_mixing_loop:

MUL	R9, R7, R12				@ delta sample * fine position = interpolated DELTA
MOV	R9, R9, ASR#22				@ scale down the sample
ADDS	R9, R9, R6, LSL#1			@ double the base sample and add it to the interpolated downscaled DELTA
LDRNE	R0, [R5, -R8, LSL#2]			@ if the sample is NOT 0 load the sample from buffer and store the calulated value
MLANE	R0, R11, R9, R0				@ add the sample to the buffer sample and apply volume
STRNE	R0, [R5, -R8, LSL#2]			@ store the sample if it's not Zero
ADD	R7, R7, R1				@ ### changed from R4 to R1
MOVS	R9, R7, LSR#23				@ check if there is new samples to load

BEQ	reverse_mixing_load_skip

SUBS	R2, R2, R7, LSR#23			@ blablabla, all same as above
BLLE	loop_end_sub

MOVS	R9, R9					@ check if sample 
ADDEQ	R6, R12, R6
LDRNESB	R6, [R3, -R9]!
LDRSB	R12, [R3, #-1]				@ load samples dependent on conditions
SUB	R12, R12, R6
BIC	R7, R7, #0x3F800000			@ cut off overflow count to get new fine position

reverse_mixing_load_skip:

SUBS	R8, R8, #1				@ remaining samples -1
BGT	reverse_mixing_loop			@ continue lopo if there is still samples to process

@ADD	R3, R3, #1				@ copied from original code (???)
ADD	R3, R3, #2				@ =$03006DE8

B	mixing_end_func

@**************** SPECIAL MIXING END ****************@

@************** SPECIAL MIXING LOOPING **************@

compressed_loop_end_sub:




@************ SPECIAL MIXING LOOPING END ************@

@****************** BDPCM DEOCODER ******************@

bdpcm_decoder:				@ RETURNS SAMPLE FROM POSITION XXX in R12

STMFD	SP!, {R0, R2, R5-R7, LR}		@ push registers to make them free to use: R0, R2, R5, R6, R7, LR
MOV	R0, R3, LSR#6				@ shift the relative position over to clip of every but the block offset
LDR	R12, [R4, #CHN_BLOCK_COUNT]		@ check if the current sample position is at the beginning of the current block
CMP	R0, R12
BEQ	bdpcm_decoder_return

STR	R0, [R4, #CHN_BLOCK_COUNT]		@ store the block position to Channel Vars
MOV	R12, #0x21				@ load decoding byte count to R1 (1 Block = 0x21 Bytes)
MUL	R2, R12, R0				@ multiply the block count with the block length to calc actual byte position of current block
LDR	R12, [R4, #CHN_WAVE_OFFSET]		@ load the wave data offset to R1
ADD	R2, R2, R12				@ add the wave data offset and 0x10 to get the actual position in ROM
ADD	R2, R2, #0x10				@ 
LDR	R5, decoder_buffer			@ load the decoder buffer pointer to R5
ADR	R6, delta_lookup_table			@ load the lookup table pointer to R6
MOV	R7, #0x40				@ load the block sample count (0x40) to R7
LDRB	LR, [R2], #1				@ load the first byte & sample from the wave data to LR (each block starts with a signed 8 bit pcm sample) LDRSB not necessary due to the 24 high bits being cut off anyway
STRB	LR, [R5], #1				@ write the sample to the decoder buffer
LDRB	R12, [R2], #1				@ load the next 2 samples to R1 (to get decoded) --- LSBits is decoded first and MSBits last
B	bdpcm_decoder_lsb

bdpcm_decoder_msb:

LDRB	R12, [R2], #1				@ load the next 2 samples to get decoded
MOV	R0, R12, LSR#4				@ seperate the current samples' bits
LDRSB	R0, [R6, R0]				@ load the differential value from the lookup table
ADD	LR, LR, R0				@ add the decoded value to the previous sample value to calc the current samples' level
STRB	LR, [R5], #1				@ write the output sample to the decoder buffer and increment buffer pointer

bdpcm_decoder_lsb:

AND	R0, R12, #0xF				@ seperate the 4 LSBits
LDRSB	R0, [R6, R0]				@ but the 4 bit value into the lookup table and save the result to R0
ADD	LR, LR, R0				@ add the value from the lookup table to the previous value to calc the new one
STRB	LR, [R5], #1				@ store the decoded sample to the decoding buffer
SUBS	R7, R7, #2				@ decrease the block sample counter by 2 (2 samples each byte) and check if it is still above 0
BGT	bdpcm_decoder_msb			@ if there is still samples to decode jump to the MSBits

bdpcm_decoder_return:

LDR	R5, decoder_buffer			@ reload the decompressor buffer offset to R5
AND	R0, R3, #0x3F				@ cut off the main position bits to read data from short buffer
LDRSB	R12, [R5, R0]				@ read the decoded sample from buffer
LDMFD	SP!, {R0, R2, R5-R7, PC}		@ pop registers and return to the compressed sample mixer

@**************** END BDPCM DECODER *****************@

decoder_buffer:
	.word	decoder_buffer_target
delta_lookup_table:
	.byte	0x0, 0x1, 0x4, 0x9, 0x10, 0x19, 0x24, 0x31, 0xC0, 0xCF, 0xDC, 0xE7, 0xF0, 0xF7, 0xFC, 0xFF
.endif

main_mixer_end:

	.end

