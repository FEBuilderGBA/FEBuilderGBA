/* HQ-Mixer rev 4.0 created by ipatix (c) 2021
 * licensed under GPLv3, see LICENSE.txt for details */

    /**********************
     * CONFIGURATION AREA *
     **********************/
                           @for FE8U
    .equ    hq_buffer_ptr, 0x03002C60   @ <-- set this to an IWRAM address where you want your high quality mix buffer to be
    .equ    POKE_CHN_INIT, 0                        @ <-- set to '1' for pokemon games, '0' for other games
    .equ    ENABLE_STEREO, 1                        @ <-- TODO actually implement, not functional yet
    .equ    ENABLE_REVERB, 0                        @ <-- if you want faster code or don't like reverb, set this to '0', set to '1' otherwise
    .equ    ENABLE_DMA, 1                           @ <-- Using DMA produces smaller code and has better performance. Disable it if your case does not allow to use DMA.

    /*****************
     * END OF CONFIG *
     *****************/

    /* NO USER SERVICABLE CODE BELOW HERE! YOU HAVE BEEN WARNED */

    /* globals */
    .global SoundMainRAM

    .equ    DMA_BUFFER_SIZE, 0x630

    .equ    FRAME_LENGTH_5734, 0x60
    .equ    FRAME_LENGTH_7884, 0x84             @ THIS MODE IS NOT SUPPORTED BY THIS ENGINE BECAUSE IT DOESN'T USE AN 8 ALIGNED BUFFER LENGTH
    .equ    FRAME_LENGTH_10512, 0xB0
    .equ    FRAME_LENGTH_13379, 0xE0            @ DEFAULT
    .equ    FRAME_LENGTH_15768, 0x108
    .equ    FRAME_LENGTH_18157, 0x130
    .equ    FRAME_LENGTH_21024, 0x160
    .equ    FRAME_LENGTH_26758, 0x1C0
    .equ    FRAME_LENGTH_31536, 0x210
    .equ    FRAME_LENGTH_36314, 0x260
    .equ    FRAME_LENGTH_40137, 0x2A0
    .equ    FRAME_LENGTH_42048, 0x2C0

    /* stack variables */
    .equ    ARG_FRAME_LENGTH, 0x0               @ Number of samples per frame/buffer
    .equ    ARG_REMAIN_CHN, 0x4                 @ temporary to count down the channels to process
    .equ    ARG_BUFFER_POS, 0x8                 @ stores the current output buffer pointer
    .equ    ARG_LOOP_START_POS, 0xC             @ stores wave loop start position in channel loop
    .equ    ARG_LOOP_LENGTH, 0x10               @   ''    ''   ''  end position
    .equ    ARG_BUFFER_POS_INDEX_HINT, 0x14     @ if this value is == 2, then this is the last buffer before wraparound
    .equ    ARG_PCM_STRUCT, 0x18                @ pointer to engine the main work area

    /* channel struct */
    .equ    CHN_STATUS, 0x0                     @ [byte] channel status bitfield
    .equ    CHN_MODE, 0x1                       @ [byte] channel mode bitfield
    .equ    CHN_VOL_1, 0x2                      @ [byte] volume right
    .equ    CHN_VOL_2, 0x3                      @ [byte] volume left
    .equ    CHN_ATTACK, 0x4                     @ [byte] wave attack summand
    .equ    CHN_DECAY, 0x5                      @ [byte] wave decay factor
    .equ    CHN_SUSTAIN, 0x6                    @ [byte] wave sustain level
    .equ    CHN_RELEASE, 0x7                    @ [byte] wave release factor
    .equ    CHN_ADSR_LEVEL, 0x9                 @ [byte] current envelope level
    .equ    CHN_FINAL_VOL_1, 0xA                @ [byte] not used anymore!
    .equ    CHN_FINAL_VOL_2, 0xB                @ [byte] not used anymore!
    .equ    CHN_ECHO_VOL, 0xC                   @ [byte] pseudo echo volume
    .equ    CHN_ECHO_REMAIN, 0xD                @ [byte] pseudo echo length
    .equ    CHN_SAMPLE_COUNTDOWN, 0x18          @ [word] sample countdown in mixing loop
    .equ    CHN_FINE_POSITION, 0x1C             @ [word] inter sample position (23 bits)
    .equ    CHN_FREQUENCY, 0x20                 @ [word] sample rate (in Hz)
    .equ    CHN_WAVE_OFFSET, 0x24               @ [word] wave header pointer
    .equ    CHN_POSITION_ABS, 0x28              @ [word] points to the current position in the wave data (relative offset for compressed samples)
    .equ    CHN_SAMPLE_STOR, 0x3F               @ [byte] contains the previously loaded sample from the linear interpolation

    /* wave header struct */
    .equ    WAVE_TYPE, 0x0                      @ [byte] 0x0 = 8 bit pcm, 0x1 = pokemon dpcm
    .equ    WAVE_LOOP_FLAG, 0x3                 @ [byte] 0x0 = oneshot; 0x40 = looped
    .equ    WAVE_FREQ, 0x4                      @ [word] pitch adjustment value = mid-C samplerate * 1024
    .equ    WAVE_LOOP_START, 0x8                @ [word] loop start position
    .equ    WAVE_LENGTH, 0xC                    @ [word] loop end / wave end position
    .equ    WAVE_DATA, 0x10                     @ [byte array] actual wave data

    /* pulse wave synth configuration offset */
    .equ    SYNTH_TYPE, 0x1                     @ [byte]
    .equ    SYNTH_BASE_WAVE_DUTY, 0x2           @ [byte]
    .equ    SYNTH_WIDTH_CHANGE_1, 0x3           @ [byte]
    .equ    SYNTH_MOD_AMOUNT, 0x4               @ [byte]
    .equ    SYNTH_WIDTH_CHANGE_2, 0x5           @ [byte]

    /* CHN_STATUS flags - 0x0 = OFF */
    .equ    FLAG_CHN_INIT, 0x80                 @ [bit] write this value to init a channel
    .equ    FLAG_CHN_RELEASE, 0x40              @ [bit] write this value to release (fade out) the channel
    .equ    FLAG_CHN_LOOP, 0x10                 @ [bit] loop (yes/no)
    .equ    FLAG_CHN_ECHO, 0x4                  @ [bit] echo phase
    .equ    FLAG_CHN_ATTACK, 0x3                @ [bit] attack phase
    .equ    FLAG_CHN_DECAY, 0x2                 @ [bit] decay phase
    .equ    FLAG_CHN_SUSTAIN, 0x1               @ [bit] sustain phase

    /* CHN_MODE flags */
    .equ    MODE_FIXED_FREQ, 0x8                @ [bit] set to disable resampling (i.e. playback with output rate)
    .equ    MODE_REVERSE, 0x10                  @ [bit] set to reverse sample playback
    .equ    MODE_COMP, 0x20                     @ [bit] is wave being played compressed
    .equ    MODE_SYNTH, 0x40                    @ [bit] channel is a synth channel

    .equ    MODE_FLGSH_SIGN_REVERSE, 27         @ shift by n bits to get the reverse flag into SIGN

    /* variables of the engine work area */
    .equ    VAR_REVERB, 0x5                     @ [byte] 0-127 = reverb level
    .equ    VAR_MAX_CHN, 0x6                    @ [byte] maximum channels to process
    .equ    VAR_MASTER_VOL, 0x7                 @ [byte] PCM master volume
    .equ    VAR_EXT_NOISE_SHAPE_LEFT, 0xE       @ [byte] normally unused, used here for noise shaping
    .equ    VAR_EXT_NOISE_SHAPE_RIGHT, 0xF      @ [byte] normally unused, used here for noise shaping
    .equ    VAR_DEF_PITCH_FAC, 0x18             @ [word] this value get's multiplied with the samplerate for the inter sample distance
    .equ    VAR_FIRST_CHN, 0x50                 @ [CHN struct] relative offset to channel array
    .equ    VAR_PCM_BUFFER, 0x350

    /* just some more defines */
    .equ    REG_DMA3_SRC, 0x040000D4
    .equ    ARM_OP_LEN, 0x4

    /* extensions */
    .equ    BDPCM_BLK_STRIDE, 0x21
    .equ    BDPCM_BLK_SIZE, 0x40
    .equ    BDPCM_BLK_SIZE_MASK, 0x3F
    .equ    BDPCM_BLK_SIZE_SHIFT, 0x6

    .thumb
    .align  2

SoundMainRAM:
    /* load Reverb level and check if we need to apply it */
    STR     R4, [SP, #ARG_BUFFER_POS_INDEX_HINT]
    /*
     * okay, before the actual mixing starts
     * the volume and envelope calculation takes place
     */
    MOV     R4, R8  @ R4 = buffer length
    /*
     * this stroes the buffer length to a backup location
     */
    STR     R4, [SP, #ARG_FRAME_LENGTH]
    /* init channel loop */
    LDR     R4, [SP, #ARG_PCM_STRUCT]           @ R4 = main work area pointer
    LDR     R0, [R4, #VAR_DEF_PITCH_FAC]        @ R0 = samplingrate pitch factor
    MOV     R12, R0
    LDRB    R0, [R4, #VAR_MAX_CHN]
    ADD     R4, #VAR_FIRST_CHN                  @ R4 = Base channel Offset (Channel #0)

C_channel_state_loop:
    /* this is the main channel processing loop */
    STR     R0, [SP, #ARG_REMAIN_CHN]
    LDR     R3, [R4, #CHN_WAVE_OFFSET]
    LDRB    R6, [R4, #CHN_STATUS]           @ R6 will hold the channel status
    MOVS    R0, #0xC7                       @ check if any of the channel status flags is set
    TST     R0, R6                          @ check if none of the flags is set
    BEQ     C_skip_channel
    /* check channel flags */
    LSL     R0, R6, #25                     @ shift over the FLAG_CHN_INIT to CARRY
    BCC     C_adsr_echo_check               @ continue with normal channel procedure
    /* check leftmost bit */
    BMI     C_stop_channel                  @ FLAG_CHN_INIT | FLAG_CHN_RELEASE -> stop directly
    /* channel init procedure */
    MOVS    R6, #FLAG_CHN_ATTACK
    /* enabled compression if sample flag is set */
    MOVS    R0, R3                          @ R0 = CHN_WAVE_OFFSET
    ADD     R0, #WAVE_DATA                  @ R0 = wave data offset
    LDR     R2, [R3, #WAVE_LENGTH]
    CMP     R2, #0
    BEQ     C_channel_init_synth
    LDRB    R5, [R3, #WAVE_TYPE]
    LSL     R5, R5, #31
    LDRB    R5, [R4, #CHN_MODE]
    BMI     C_channel_init_comp
    LSL     R5, R5, #27                     @ shift MODE_REVERSE flag to SIGN
    BMI     C_channel_init_noncomp_reverse
    /* Pokemon games seem to init channels differently than other m4a games */
C_channel_init_noncomp_forward:
.if POKE_CHN_INIT==0
.else
    LDR     R1, [R4, #CHN_SAMPLE_COUNTDOWN]
    ADD     R0, R1
    SUB     R2, R1
.endif
    B       C_channel_init_check_loop
C_channel_init_synth:
    MOV     R5, #MODE_SYNTH
    STRB    R5, [R4, #CHN_MODE]
    LDRB    R1, [R3, #(WAVE_DATA + SYNTH_TYPE)]
    CMP     R1, #2
    BNE     C_channel_init_check_loop
    /* start triangular synth wave at 90 degree phase
     * to avoid a pop sound at the start of the wave */
    MOV     R5, #0x40
    LSL     R5, #24
    STR     R5, [R4, #CHN_FINE_POSITION]
    MOV     R5, #0
    B       C_channel_init_check_loop_no_fine_pos
C_channel_init_noncomp_reverse:
.if POKE_CHN_INIT==0
    ADD     R0, R2
.else
    ADD     R0, R2
    LDR     R1, [R4, #CHN_SAMPLE_COUNTDOWN]
    SUB     R0, R1
    SUB     R2, R1
.endif
    B       C_channel_init_check_loop
C_channel_init_comp:
    MOV     R0, #MODE_COMP
    ORR     R5, R0
    STRB    R5, [R4, #CHN_MODE]
    LSL     R5, R5, #27                     @ shift MODE_REVERSE flag to SIGN
    BMI     C_channel_init_comp_reverse
C_channel_init_comp_forward:
.if POKE_CHN_INIT==0
    MOV     R0, #0
.else
    LDR     R0, [R4, #CHN_SAMPLE_COUNTDOWN]
    SUB     R2, R0
.endif
    B       C_channel_init_check_loop
C_channel_init_comp_reverse:
.if POKE_CHN_INIT==0
    MOV     R0, R2
.else
    LDR     R1, [R4, #CHN_SAMPLE_COUNTDOWN]
    SUB     R2, R1
    MOV     R0, R2
.endif
C_channel_init_check_loop:
    MOVS    R5, #0                          @ initial envelope = #0
    STR     R5, [R4, #CHN_FINE_POSITION]
C_channel_init_check_loop_no_fine_pos:
    STR     R0, [R4, #CHN_POSITION_ABS]
    STR     R2, [R4, #CHN_SAMPLE_COUNTDOWN]
    STRB    R5, [R4, #CHN_ADSR_LEVEL]
    MOV     R2, #CHN_SAMPLE_STOR            @ offset is too large to be used in one instruction
    STRB    R5, [R4, R2]
    /* enabled loop if required */
    LDRB    R2, [R3, #WAVE_LOOP_FLAG]
    LSR     R0, R2, #6
    BEQ     C_adsr_attack
    /* loop enabled here */
    ADD     R6, #FLAG_CHN_LOOP
    B       C_adsr_attack

C_adsr_echo_check:
    /* this is the normal ADSR procedure without init */
    LDRB    R5, [R4, #CHN_ADSR_LEVEL]
    LSL     R0, R6, #29                     @ FLAG_CHN_ECHO --> bit 31 (sign bit)
    BPL     C_adsr_release_check
    /* pseudo echo handler */
    LDRB    R0, [R4, #CHN_ECHO_REMAIN]
    SUB     R0, #1
    STRB    R0, [R4, #CHN_ECHO_REMAIN]
    BHI     C_channel_vol_calc              @ continue normal if channel is still on

C_stop_channel:
    MOVS    R0, #0
    STRB    R0, [R4, #CHN_STATUS]

C_skip_channel:
    /* go to end of the channel loop */
    B       C_end_channel_state_loop

C_adsr_release_check:
    LSL     R0, R6, #25                      @ FLAG_CHN_RELEASE --> bit 31 (sign bit)
    BPL     C_adsr_decay_check
    /* release handler */
    LDRB    R0, [R4, #CHN_RELEASE]
    MUL     R5, R5, R0
    LSR     R5, #8
    BLE     C_adsr_released
    /* pseudo echo init handler */
    LDRB    R0, [R4, #CHN_ECHO_VOL]
    CMP     R5, R0
    BHI     C_channel_vol_calc

C_adsr_released:
    /* if volume released to #0 */
    LDRB    R5, [R4, #CHN_ECHO_VOL]
    CMP     R5, #0
    BEQ     C_stop_channel
    /* pseudo echo volume handler */
    MOVS    R0, #FLAG_CHN_ECHO
    ORR     R6, R0                          @ set the echo flag
    B       C_adsr_save_and_finalize

C_adsr_decay_check:
    /* check if decay is active */
    MOVS    R2, #(FLAG_CHN_DECAY+FLAG_CHN_SUSTAIN)
    AND     R2, R6
    CMP     R2, #FLAG_CHN_DECAY
    BNE     C_adsr_attack_check             @ decay not active yet
    /* decay handler */
    LDRB    R0, [R4, #CHN_DECAY]
    MUL     R5, R5, R0
    LSR     R5, R5, #8
    LDRB    R0, [R4, #CHN_SUSTAIN]
    CMP     R5, R0
    BHI     C_channel_vol_calc              @ sample didn't decay yet
    /* sustain handler */
    MOVS    R5, R0                          @ current level = sustain level
    BEQ     C_adsr_released                 @ sustain level #0 --> branch
    /* step to next phase otherweise */
    B       C_adsr_next_state

C_adsr_attack_check:
    /* attack handler */
    CMP     R2, #FLAG_CHN_ATTACK
    BNE     C_channel_vol_calc              @ if it isn't in attack attack phase, it has to be in sustain (keep vol) --> branch

C_adsr_attack:
    /* apply attack summand */
    LDRB    R0, [R4, #CHN_ATTACK]
    ADD     R5, R0
    CMP     R5, #0xFF
    BLO     C_adsr_save_and_finalize
    /* cap attack at 0xFF */
    MOVS    R5, #0xFF

C_adsr_next_state:
    /* switch to next adsr phase */
    SUB     R6, #1

C_adsr_save_and_finalize:
    /* store channel status */
    STRB    R6, [R4, #CHN_STATUS]

C_channel_vol_calc:
    /* store the calculated ADSR level */
    STRB    R5, [R4, #CHN_ADSR_LEVEL]
    /* apply master volume */
    LDR     R0, [SP, #ARG_PCM_STRUCT]
    LDRB    R0, [R0, #VAR_MASTER_VOL]
    ADD     R0, #1
    MUL     R5, R0
    /* left side volume */
    LDRB    R0, [R4, #CHN_VOL_2]
    MUL     R0, R5
    LSR     R0, #13
    MOV     R10, R0                         @ R10 = left volume
    /* right side volume */
    LDRB    R0, [R4, #CHN_VOL_1]
    MUL     R0, R5
    LSR     R0, #13
    MOV     R11, R0                         @ R11 = right volume
    /*
     * Now we get closer to actual mixing:
     * For looped samples some additional operations are required
     */
    MOVS    R0, #FLAG_CHN_LOOP
    AND     R0, R6
    BEQ     C_sample_loop_setup_skip
    /* loop setup handler */
    ADD     R3, #WAVE_LOOP_START
    LDMIA   R3!, {R0, R1}                   @ R0 = loop start, R1 = loop end
    LDRB    R2, [R4, #CHN_MODE]
    LSL     R2, R2, #MODE_FLGSH_SIGN_REVERSE
    BCS     C_sample_loop_setup_comp
    ADD     R3, R0                          @ R3 = loop start position (absolute)
    B       C_sample_loop_setup_finish
C_sample_loop_setup_comp:
    MOV     R3, R0
C_sample_loop_setup_finish:
    STR     R3, [SP, #ARG_LOOP_START_POS]
    SUB     R0, R1, R0

C_sample_loop_setup_skip:
    /* do the rest of the setup */
    STR     R0, [SP, #ARG_LOOP_LENGTH]      @ if loop is off --> R0 = 0x0
    LDR     R5, hq_buffer_literal
    LDR     R2, [R4, #CHN_SAMPLE_COUNTDOWN]
    LDR     R3, [R4, #CHN_POSITION_ABS]
    LDRB    R0, [R4, #CHN_MODE]
    ADR     R1, C_mixing_setup
    BX      R1

    .align  2
hq_buffer_literal:
    .word   hq_buffer_ptr

    .arm
    .align  2

    /* register usage:
     * R0:  scratch
     * R1:  scratch
     * R2:  sample countdown
     * R3:  sample pointer
     * R4:  sample step
     * R5:  mixing buffer
     * R6:  sampleval base
     * R7:  sample interpos
     * R8:  frame count
     * R9:  scratch
     * R10: scratch
     * R11: volume
     * R12: sampval diff
     * LR:  scratch */
C_mixing_setup:
    /* frequency and mixing loading routine */
    LDRSB   R6, [R4, #CHN_SAMPLE_STOR]
    LDR     R8, [SP, #ARG_FRAME_LENGTH]
    ORRS    R11, R11, R10, LSL#16           @ R11 = 00LL00RR
    BEQ     C_mixing_epilogue               @ volume #0 --> branch and skip channel processing
    /* normal processing otherwise */
    TST     R0, #(MODE_COMP|MODE_REVERSE)
    BNE     C_mixing_setup_comp_rev
    TST     R0, #MODE_FIXED_FREQ
    BNE     C_setup_fixed_freq_mixing
C_mixing_setup_comp_rev:
    STMFD   SP!, {R4, R9, R12}
    ADD     R4, R4, #CHN_FINE_POSITION
    LDMIA   R4, {R7, LR}                    @ R7 = Fine Position, LR = Frequency
    MUL     R4, LR, R12                     @ R4 = inter sample steps = output rate factor * samplerate
    TST     R0, #MODE_SYNTH
    BNE     C_setup_synth
    /*
     * Mixing goes with volume ranges 0-127
     * They come in 0-255 --> divide by 2
     */
    MOVS    R11, R11, LSR#1
    ADC     R11, R11, #0x8000
    BIC     R11, R11, #0x8000
    MOV     R1, R7                          @ R1 = inter sample position
    /*
     * There is 2 different mixing codepaths for uncompressed data
     *  path 1: fast mixing, but doesn't supports loop or stop
     *  path 2: not so fast but supports sample loops / stop
     * This checks if there is enough samples aviable for path 1.
     * important: R0 is expected to be #0
     */
    SUB     R10, SP, #0x8
    TST     R0, #MODE_FIXED_FREQ
    MOVNE   R4, #0x800000
    MOVS    R0, R0, LSL#(MODE_FLGSH_SIGN_REVERSE)
    UMLAL   R1, R0, R4, R8
    MOV     R1, R1, LSR#23
    ORR     R0, R1, R0, LSL#9
    BCS     C_data_load_comp
    BMI     C_data_load_uncomp_rev
    B       C_data_load_uncomp_for

/* registers:
 * R9: src address (relative to start address)
 * R0: dst address (on stack)
 * R12: delta_lookup_table */
F_decode_compressed:
    STMFD   SP!, {R3, LR}
    MOV     LR, #BDPCM_BLK_SIZE
    LDRB    R2, [R9], #1
    LDRB    R3, [R9], #1
    B       C_bdpcm_decoder_loop_entry

C_bdpcm_decoder_loop:
    LDRB    R3, [R9], #1
    LDRB    R2, [R12, R3, LSR#4]
    ADD     R2, R1, R2
    AND     R3, R3, #0xF
C_bdpcm_decoder_loop_entry:
    LDRB    R1, [R12, R3]
    ADD     R1, R1, R2
bdpcm_instructions:
    NOP
    NOP
    SUBS    LR, #2
    BGT     C_bdpcm_decoder_loop
    LDMFD   SP!, {R3, PC}

bdpcm_instruction_resource_for:
    STRB    R2, [R0], #1
    STRB    R1, [R0], #1
bdpcm_instruction_resource_rev:
    STRB    R2, [R0, #-1]!
    STRB    R1, [R0, #-1]!

delta_lookup_table:
    .byte    0, 1, 4, 9, 16, 25, 36, 49, -64, -49, -36, -25, -16, -9, -4, -1
stack_boundary_literal:
    .word    0x03007900

C_data_load_comp:
    ADRPL   R9, bdpcm_instruction_resource_for
    ADRMI   R9, bdpcm_instruction_resource_rev
    LDMIA   R9, {R12, LR}
    ADR     R9, bdpcm_instructions
    STMIA   R9, {R12, LR}
    ADR     R12, delta_lookup_table
    BMI     C_data_load_comp_rev
C_data_load_comp_for:
    /* TODO having loop support for forward samples would be nice */
    /* LR = end_of_last_block */
    ADD     LR, R3, R0
    ADD     LR, #(1+(BDPCM_BLK_SIZE-1))             @ -1 for alignment, +1 because we need an extra sample for interpolation
    BIC     LR, #BDPCM_BLK_SIZE_MASK
    /* R9 = start_of_first_block >> 6 */
    MOV     R9, R3, LSR#BDPCM_BLK_SIZE_SHIFT
    /* R8 = num_samples */
    SUB     R8, LR, R9, LSL#BDPCM_BLK_SIZE_SHIFT
    /* check if stack would overflow */
    LDR     R1, stack_boundary_literal
    ADD     R1, R8
    CMP     R1, SP
    BHS     C_end_mixing
    /* --- */
    ADD     R1, R3, R0
    SUBS    R0, R2, R0
    STMFD   SP!, {R0, R1}
    SUB     SP, R8
    BGT     C_data_load_comp_for_calc_pos
    /* locate end of sample data block */
    ADD     R1, R3, R2
    /* ugly workaround for unaligned samples */
    ADD     R1, R1, #BDPCM_BLK_SIZE_MASK
    BIC     R1, R1, #BDPCM_BLK_SIZE_MASK
    SUB     R1, LR, R1
    SUB     R8, R1
    ADD     R0, SP, R8
    BL      F_clear_mem
C_data_load_comp_for_calc_pos:
    AND     R3, R3, #BDPCM_BLK_SIZE_MASK
    MOV     R0, SP
C_data_load_comp_decode:
    LDR     R2, [R10, #8]           @ load chn_ptr from previous STMFD
    @ zero flag should be only set when leaving from F_clear_mem (R1 = 0)
    STREQB  R1, [R2, #CHN_STATUS]
    LDR     R2, [R2, #CHN_WAVE_OFFSET]
    ADD     R2, #WAVE_DATA
    MOV     R1, #BDPCM_BLK_STRIDE
    MLA     R9, R1, R9, R2
C_data_load_comp_loop:
    BL      F_decode_compressed
    SUBS    R8, #BDPCM_BLK_SIZE
    BGT     C_data_load_comp_loop
    B       C_select_highspeed_codepath_vla_r3

C_data_load_comp_rev:
    /* LR = end_of_last_block */
    ADD     LR, R3, #(BDPCM_BLK_SIZE-1)
    BIC     LR, #BDPCM_BLK_SIZE_MASK
    /* R9 = start_of_first_block >> 6 */
    SUB     R9, R3, R0
    SUB     R9, #1  @ one extra sample for LERP
    MOV     R9, R9, LSR#BDPCM_BLK_SIZE_SHIFT
    /* R8 = num_samples */
    SUB     R8, LR, R9, LSL#BDPCM_BLK_SIZE_SHIFT
    /* check if stack would overflow */
    LDR     LR, stack_boundary_literal
    ADD     LR, R8
    CMP     LR, SP
    BHS     C_end_mixing
    /* --- */
    SUB     LR, R3, R0
    SUBS    R0, R2, R0
    STMFD   SP!, {R0, LR}
    MOV     R0, SP
    SUB     SP, R8
    BGT     C_data_load_comp_rev_calc_pos
    SUB     R1, R3, R2
    SUB     R1, R1, R9, LSL#BDPCM_BLK_SIZE_SHIFT
    SUB     R8, R1
    ADD     R0, SP, R8
    BL      F_clear_mem
C_data_load_comp_rev_calc_pos:
    RSB     R3, R3, #0
    AND     R3, R3, #BDPCM_BLK_SIZE_MASK
    B       C_data_load_comp_decode

C_data_load_uncomp_rev:
    /* LR = end_of_last_block */
    ADD     LR, R3, #0x3
    BIC     LR, #0x3
    /* R9 = start_of_first_block */
    SUB     R9, R3, R0
    SUB     R9, #1
    BIC     R9, #0x3
    /* R8 = num_samples */
    SUB     R8, LR, R9
    /* check if stack would overflow */
    LDR     R1, stack_boundary_literal
    ADD     R1, R8
    CMP     R1, SP
    BHS     C_end_mixing
    /* --- */
    SUB     R1, R3, R0
    SUBS    R0, R2, R0
    STMFD   SP!, {R0, R1}
    MOV     R0, SP
    SUB     SP, R8
    BGT     C_data_load_uncomp_rev_loop
    SUB     R1, R3, R2
    SUB     R1, R9
    SUB     R8, R1
    ADD     R0, SP, R8
    BL      F_clear_mem
    LDR     R2, [R10, #8]           @ load chn_ptr from previous STMFD
    @ R1 should be zero here
    STRB    R1, [R2, #CHN_STATUS]
C_data_load_uncomp_rev_loop:
    LDMIA   R9!, {R1}
    EOR     R2, R1, R1, ROR#16
    MOV     R2, R2, LSR#8
    BIC     R2, R2, #0xFF00
    EOR     R1, R2, R1, ROR#8
    STMDB   R0!, {R1}
    SUBS    R8, #4
    BGT     C_data_load_uncomp_rev_loop
    RSB     R3, R3, #0
    B       C_select_highspeed_codepath_vla_r3_and3

C_data_load_uncomp_for:
    CMP     R2, R0                          @ actual comparison
    BLE     C_unbuffered_mixing       @ if not enough samples are available for path 1 --> branch
    /*
     * This is the mixer path 1.
     * The interesting thing here is that the code will
     * buffer enough samples on stack if enough space
     * on stack is available (or goes over the limit of 0x400 bytes)
     */
    SUB     R2, R2, R0
    LDR     R9, stack_boundary_literal
    ADD     R9, R0
    CMP     R9, SP
    ADD     R9, R3, R0
    /*
     * R2 = remaining samples after processing
     * R9 = final sample position
     * SP = original stack location
     * These values will get reloaded after channel processing
     * due to the lack of registers.
     */
    STMFD   SP!, {R2, R9}
    CMPLO   R0, #0x400                      @ > 0x400 bytes --> read directly from ROM rather than buffered
    BHS     C_select_highspeed_codepath

    BIC     R1, R3, #3
    ADD     R0, R0, #7
.if ENABLE_DMA==1
    /*
     * The code below inits the DMA to read word aligned
     * samples from ROM to stack
     */
    MOV     R9, #0x04000000
    ADD     R9, #0x000000D4
    MOV     R0, R0, LSR#2
    SUB     SP, SP, R0, LSL#2
    ORR     LR, R0, #0x84000000
    STMIA   R9, {R1, SP, LR}                @ actually starts the DMA
.else
    /*
     * This alternative path doesn't use DMA but copies with CPU instead
     */
    BIC     R0, R0, #0x3
    SUB     SP, SP, R0
    MOV     LR, SP
    STMFD   SP!, {R3-R10}
    ANDS    R10, R0, #0xE0
    RSB     R10, R10, #0xF0
    ADD     PC, PC, R10, LSR#2
C_copy_loop:
    LDMIA   R1!, {R3-R10}
    STMIA   LR!, {R3-R10}
    LDMIA   R1!, {R3-R10}
    STMIA   LR!, {R3-R10}
    LDMIA   R1!, {R3-R10}
    STMIA   LR!, {R3-R10}
    LDMIA   R1!, {R3-R10}
    STMIA   LR!, {R3-R10}
    LDMIA   R1!, {R3-R10}
    STMIA   LR!, {R3-R10}
    LDMIA   R1!, {R3-R10}
    STMIA   LR!, {R3-R10}
    LDMIA   R1!, {R3-R10}
    STMIA   LR!, {R3-R10}
    LDMIA   R1!, {R3-R10}
    STMIA   LR!, {R3-R10}
    SUBS    R0, #0x100
    BPL     C_copy_loop
    ANDS    R0, R0, #0x1C
    BEQ     C_copy_end
C_copy_loop_rest:
    LDMIA   R1!, {R3}
    STMIA   LR!, {R3}
    SUBS    R0, #0x4
    BGT     C_copy_loop_rest
C_copy_end:
    LDMFD   SP!, {R3-R10}
.endif
C_select_highspeed_codepath_vla_r3_and3:
    AND     R3, R3, #3
C_select_highspeed_codepath_vla_r3:
    ADD     R3, R3, SP
C_select_highspeed_codepath:
    STMFD   SP!, {R10}                      @ save original SP for VLA
    /*
     * This code decides which piece of code to load
     * depending on playback-rate / default-rate ratio.
     * Modes > 1.0 run with different volume levels.
     * R4 = inter sample step
     */
    ADR     R0, high_speed_code_resource    @ loads the base pointer of the code
    SUBS    R4, R4, #0x800000
    MOVPL   R11, R11, LSL#1                 @  if >= 1.0*   0-127 --> 0-254 volume level
    ADDPL   R0, R0, #(ARM_OP_LEN*6)         @               6 instructions further
    SUBPLS  R4, R4, #0x800000               @  if >= 2.0*
    ADDPL   R0, R0, #(ARM_OP_LEN*6)
    ADDPL   R4, R4, #0x800000
    LDR     R2, previous_fast_code
    CMP     R0, R2                          @ code doesn't need to be reloaded if it's already in place
    BEQ     C_skip_fast_mixing_creation
    /* This loads the needed code to RAM */
    STR     R0, previous_fast_code
    LDMIA   R0, {R0-R2, R8-R10}             @ load 6 opcodes
    ADR     LR, fast_mixing_instructions

C_fast_mixing_creation_loop:
    /* paste code to destination, see below for patterns */
    STMIA   LR, {R0, R1}
    ADD     LR, LR, #(ARM_OP_LEN*38)
    STMIA   LR, {R0, R1}
    SUB     LR, LR, #(ARM_OP_LEN*35)
    STMIA   LR, {R2, R8-R10}
    ADD     LR, LR, #(ARM_OP_LEN*38)
    STMIA   LR, {R2, R8-R10}
    SUB     LR, LR, #(ARM_OP_LEN*32)
    ADDS    R5, R5, #0x40000000         @ do that for 4 blocks
    BCC     C_fast_mixing_creation_loop

C_skip_fast_mixing_creation:
    LDR     R8, [SP]                        @ restore R8 with the frame length
    LDR     R8, [R8, #(ARG_FRAME_LENGTH + 0x8 + 0xC)]
    MOV     R2, #0xFF000000                 @ load the fine position overflow bitmask
    LDRSB   R12, [R3]
    SUB     R12, R12, R6
C_fast_mixing_loop:
    /* This is the actual processing and interpolation code loop; NOPs will be replaced by the code above */
    LDMIA   R5, {R0, R1, R10, LR}       @ load 4 stereo samples to Registers
    MUL     R9, R7, R12
fast_mixing_instructions:
    NOP                                 @ Block #1
    NOP
    MLANE   R0, R11, R9, R0
    NOP
    NOP
    NOP
    NOP
    BIC     R7, R7, R2, ASR#1
    MULNE   R9, R7, R12
    NOP                                 @ Block #2
    NOP
    MLANE   R1, R11, R9, R1
    NOP
    NOP
    NOP
    NOP
    BIC     R7, R7, R2, ASR#1
    MULNE   R9, R7, R12
    NOP                                 @ Block #3
    NOP
    MLANE   R10, R11, R9, R10
    NOP
    NOP
    NOP
    NOP
    BIC     R7, R7, R2, ASR#1
    MULNE   R9, R7, R12
    NOP                                 @ Block #4
    NOP
    MLANE   LR, R11, R9, LR
    NOP
    NOP
    NOP
    NOP
    BIC     R7, R7, R2, ASR#1
    STMIA   R5!, {R0, R1, R10, LR}      @ write 4 stereo samples

    LDMIA   R5, {R0, R1, R10, LR}       @ load the next 4 stereo samples
    MULNE   R9, R7, R12
    NOP                                 @ Block #1
    NOP
    MLANE   R0, R11, R9, R0
    NOP
    NOP
    NOP
    NOP
    BIC     R7, R7, R2, ASR#1
    MULNE   R9, R7, R12
    NOP                                 @ Block #2
    NOP
    MLANE   R1, R11, R9, R1
    NOP
    NOP
    NOP
    NOP
    BIC     R7, R7, R2, ASR#1
    MULNE   R9, R7, R12
    NOP                                 @ Block #3
    NOP
    MLANE   R10, R11, R9, R10
    NOP
    NOP
    NOP
    NOP
    BIC     R7, R7, R2, ASR#1
    MULNE   R9, R7, R12
    NOP                                 @ Block #4
    NOP
    MLANE   LR, R11, R9, LR
    NOP
    NOP
    NOP
    NOP
    BIC     R7, R7, R2, ASR#1
    STMIA   R5!, {R0, R1, R10, LR}      @ write 4 stereo samples
    SUBS    R8, R8, #8
    BGT     C_fast_mixing_loop
    /* restore previously saved values */
    LDMFD   SP, {SP}                        @ reload original stack pointer from VLA
C_skip_fast_mixing:
    LDMFD   SP!, {R2, R3}
    B       C_end_mixing

/* Various variables for the cached mixer */

    .align    2
previous_fast_code:
    .word   0x0 /* mark as invalid initially */

/* Those instructions below are used by the high speed loop self modifying code */
high_speed_code_resource:
    /* Block for Mix Freq < 1.0 * Output Frequency */
    MOV     R9, R9, ASR#22
    ADDS    R9, R9, R6, LSL#1
    ADDS    R7, R7, R4
    ADDPL   R6, R12, R6
    LDRPLSB R12, [R3, #1]!
    SUBPLS  R12, R12, R6

    /* Block for Mix Freq > 1.0 AND < 2.0 * Output Frequency */
    ADDS    R9, R6, R9, ASR#23
    ADD     R6, R12, R6
    ADDS    R7, R7, R4
    LDRPLSB R6, [R3, #1]!
    LDRSB   R12, [R3, #1]!
    SUBS    R12, R12, R6

    /* Block for Mix Freq > 2.0 * Output Frequency */
    ADDS    R9, R6, R9, ASR#23
    ADD     R7, R7, R4
    ADD     R3, R3, R7, LSR#23
    LDRSB   R6, [R3]
    LDRSB   R12, [R3, #1]!
    SUBS    R12, R12, R6

/* incase a loop or end occurs during mixing, this code is used */
C_unbuffered_mixing:
    LDRSB   R12, [R3]
    SUB     R12, R12, R6
    ADD     R5, R5, R8, LSL#2               @ R5 = End of HQ buffer

/* This below is the unbuffered mixing loop. R6 = base sample, R12 diff to next */
C_unbuffered_mixing_loop:

    MUL     R9, R7, R12
    MOV     R9, R9, ASR#22
    ADDS    R9, R9, R6, LSL#1
    LDRNE   R0, [R5, -R8, LSL#2]
    MLANE   R0, R11, R9, R0
    STRNE   R0, [R5, -R8, LSL#2]
    ADD     R7, R7, R4
    MOVS    R9, R7, LSR#23
    BEQ     C_unbuffered_mixing_skip_load   @ skip the mixing load if it isn't required

    SUBS    R2, R2, R9
    BLE     C_unbuffered_mixing_loop_or_end
C_unbuffered_mixing_loop_continue:
    SUBS    R9, R9, #1
    ADDEQ   R6, R12, R6
    LDRNESB R6, [R3, R9]!
    LDRSB   R12, [R3, #1]!
    SUB     R12, R12, R6
    BIC     R7, R7, #0x3F800000

C_unbuffered_mixing_skip_load:
    SUBS    R8, R8, #1                      @ reduce the sample count for the buffer by #1
    BGT     C_unbuffered_mixing_loop

C_end_mixing:
    LDMFD   SP!, {R4, R9, R12}
    STR     R7, [R4, #CHN_FINE_POSITION]
    STRB    R6, [R4, #CHN_SAMPLE_STOR]
    B       C_mixing_end_store

C_unbuffered_mixing_loop_or_end:
    /* This loads the loop information end loops incase it should */
    LDR     R0, [SP, #(ARG_LOOP_LENGTH+0xC)]
    CMP     R0, #0                          @ check if loop is enabled; if Loop is enabled R6 is != 0
    SUBNE   R3, R3, R0
    ADDNE   R2, R2, R0
    BNE     C_unbuffered_mixing_loop_continue
    LDMFD   SP!, {R4, R9, R12}
    B       C_mixing_end_and_stop_channel   @ R0 == 0 (if this branches)

C_fixed_mixing_loop_or_end:
    LDR     R2, [SP, #ARG_LOOP_LENGTH+0x8]
    MOVS    R0, R2                          @ copy it to R6 and check whether loop is disabled
    LDRNE   R3, [SP, #ARG_LOOP_START_POS+0x8]
    BNE     C_fixed_mixing_loop_continue

    LDMFD   SP!, {R4, R9}

C_mixing_end_and_stop_channel:
    STRB    R0, [R4]                        @ update channel flag with chn halt
    B       C_mixing_epilogue

/* These are used for the fixed freq mixer */
fixed_mixing_code_resource:
    MOVS    R6, R10, LSL#24
    MOVS    R6, R6, ASR#24
    MOVS    R6, R10, LSL#16
    MOVS    R6, R6, ASR#24
    MOVS    R6, R10, LSL#8
    MOVS    R6, R6, ASR#24
    MOVS    R6, R10, ASR#24
    LDMIA   R3!, {R10}                          @ load chunk of samples
    MOVS    R6, R10, LSL#24
    MOVS    R6, R6, ASR#24
    MOVS    R6, R10, LSL#16
    MOVS    R6, R6, ASR#24
    MOVS    R6, R10, LSL#8
    MOVS    R6, R6, ASR#24

C_setup_fixed_freq_mixing:
    STMFD   SP!, {R4, R9}

C_fixed_mixing_length_check:
    MOV     LR, R2                          @ sample countdown
    CMP     R2, R8
    MOVGT   LR, R8                          @ min(buffer_size, sample_countdown)
    SUB     LR, LR, #1
    MOVS    LR, LR, LSR#2
    BEQ     C_fixed_mixing_process_rest     @ <= 3 samples to process

    SUB     R8, R8, LR, LSL#2               @ subtract the amount of samples we need to process from the buffer length
    SUB     R2, R2, LR, LSL#2               @ subtract the amount of samples we need to process from the remaining samples
    ADR     R1, fixed_mixing_instructions
    ADR     R0, fixed_mixing_code_resource
    MOV     R9, R3, LSL#30
    ADD     R0, R0, R9, LSR#27              @ alignment * 8 + resource offset = new resource offset
    LDMIA   R0!, {R6, R7, R9, R10}          @ load and write instructions
    STMIA   R1, {R6, R7}
    ADD     R1, R1, #0xC
    STMIA   R1, {R9, R10}
    ADD     R1, R1, #0xC
    LDMIA   R0, {R6, R7, R9, R10}
    STMIA   R1, {R6, R7}
    ADD     R1, R1, #0xC
    STMIA   R1, {R9, R10}
    LDMIA   R3!, {R10}                      @ load 4 samples from ROM

C_fixed_mixing_loop:
    LDMIA    R5, {R0, R1, R7, R9}       @ load 4 samples from hq buffer

fixed_mixing_instructions:
    NOP
    NOP
    MLANE   R0, R11, R6, R0             @ add new sample if neccessary
    NOP
    NOP
    MLANE   R1, R11, R6, R1
    NOP
    NOP
    MLANE   R7, R11, R6, R7
    NOP
    NOP
    MLANE   R9, R11, R6, R9
    STMIA   R5!, {R0, R1, R7, R9}       @ write samples to the mixing buffer
    SUBS    LR, LR, #1
    BNE     C_fixed_mixing_loop

    SUB     R3, R3, #4                      @ we'll need to load this block again, so rewind a bit

C_fixed_mixing_process_rest:
    MOV     R1, #4                          @ repeat the loop #4 times to completley get rid of alignment errors

C_fixed_mixing_unaligned_loop:
    LDR     R0, [R5]
    LDRSB   R6, [R3], #1
    MLA     R0, R11, R6, R0
    STR     R0, [R5], #4
    SUBS    R2, R2, #1
    BEQ     C_fixed_mixing_loop_or_end
C_fixed_mixing_loop_continue:
    SUBS    R1, R1, #1
    BGT     C_fixed_mixing_unaligned_loop

    SUBS    R8, R8, #4
    BGT     C_fixed_mixing_length_check     @ repeat the mixing procedure until the buffer is filled

    LDMFD   SP!, {R4, R9}

C_mixing_end_store:
    STR     R2, [R4, #CHN_SAMPLE_COUNTDOWN]
    STR     R3, [R4, #CHN_POSITION_ABS]

C_mixing_epilogue:
    ADR     R0, (C_end_channel_state_loop+1)
    BX      R0

    .thumb

C_end_channel_state_loop:
    LDR     R0, [SP, #ARG_REMAIN_CHN]
    SUB     R0, #1
    BLE     C_main_mixer_return

    ADD     R4, #0x40
    B       C_channel_state_loop

C_main_mixer_return:
    LDR     R3, [SP, #ARG_PCM_STRUCT]
    LDRB    R4, [R3, #VAR_EXT_NOISE_SHAPE_LEFT]
    LSL     R4, R4, #16
    LDRB    R5, [R3, #VAR_EXT_NOISE_SHAPE_RIGHT]
    LSL     R5, R5, #16
.if ENABLE_REVERB==1
    LDRB    R2, [R3, #VAR_REVERB]
    LSR     R2, R2, #2
    LDR     R1, [SP, #ARG_BUFFER_POS_INDEX_HINT]
    CMP     R1, #2
.else
    MOV     R2, #0
    MOV     R3, #0
.endif
    ADR     R0, C_downsampler
    BX      R0

    .arm
    .align  2

C_downsampler:
    LDR     R8, [SP, #ARG_FRAME_LENGTH]
    LDR     R9, [SP, #ARG_BUFFER_POS]
.if ENABLE_REVERB==1
    ORR     R2, R2, R2, LSL#16
    MOVNE   R3, R8
    ADDEQ   R3, R3, #VAR_PCM_BUFFER
    SUBEQ   R3, R3, R9
.endif
    LDR     R10, hq_buffer_literal
    MOV     R11, #0xFF00
    MOV     LR, #0xC0000000

C_downsampler_loop:
    LDMIA   R10, {R0, R1}
    ADD     R12, R4, R0         @ left sample #1
    ADDS    R4, R12, R12
    EORVS   R12, LR, R4, ASR#31
    AND     R4, R12, #0x007F0000
    AND     R6, R11, R12, LSR#15

    ADD     R12, R5, R0, LSL#16 @ right sample #1
    ADDS    R5, R12, R12
    EORVS   R12, LR, R5, ASR#31
    AND     R5, R12, #0x007F0000
    AND     R7, R11, R12, LSR#15

    ADD     R12, R4, R1         @ left sample #2
    ADDS    R4, R12, R12
    EORVS   R12, LR, R4, ASR#31
    AND     R4, R12, #0x007F0000
    AND     R12, R11, R12, LSR#15
    ORR     R6, R12, R6, LSR#8

    ADD     R12, R5, R1, LSL#16 @ right sample #2
    ADDS    R5, R12, R12
    EORVS   R12, LR, R5, ASR#31
    AND     R5, R12, #0x007F0000
    AND     R12, R11, R12, LSR#15
    ORR     R7, R12, R7, LSR#8

.if ENABLE_REVERB==1
    LDRSH   R12, [R9, R3]!

    MOV     R1, R12, ASR#8
    MOV     R12, R12, LSL#24
    MOV     R0, R12, ASR#24

    ADD     R9, R9, #DMA_BUFFER_SIZE    @ \ LDRSH  R12, [R9, #0x630]!
    LDRSH   R12, [R9]                   @ / is unfortunately not a valid instruction

    ADD     R1, R1, R12, ASR#8
    MOV     R12, R12, LSL#24
    ADD     R0, R0, R12, ASR#24

    LDRSH   R12, [R9, -R3]!

    ADD     R1, R1, R12, ASR#8
    MOV     R12, R12, LSL#24
    ADD     R0, R0, R12, ASR#24

    STRH    R6, [R9]                    @ \ STRH  R6, [R9], #-0x630
    SUB     R9, R9, #DMA_BUFFER_SIZE    @ / is unfortunately not a valid instruction
    LDRSH   R12, [R9]
    STRH    R7, [R9], #2

    ADD     R1, R1, R12, ASR#8
    MOV     R12, R12, LSL#24
    ADD     R0, R0, R12, ASR#24

    MUL     R1, R2, R1
    MUL     R0, R2, R0

    STMIA   R10!, {R0, R1}
.else /* if ENABLE_REVERB==0 */
    MOV     R0, #DMA_BUFFER_SIZE
    STRH    R6, [R9, R0]
    STRH    R7, [R9], #2

    STMIA   R10!, {R2, R3}
.endif
    SUBS    R8, #2
    BGT     C_downsampler_loop

    ADR     R0, (C_downsampler_return+1)
    BX      R0

    .pool

    .align  1
    .thumb

C_downsampler_return:
    LDR     R0, [SP, #ARG_PCM_STRUCT]
    LSR     R4, #16
    STRB    R4, [R0, #VAR_EXT_NOISE_SHAPE_LEFT]
    LSR     R5, #16
    STRB    R5, [R0, #VAR_EXT_NOISE_SHAPE_RIGHT]
    LDR     R3, =0x68736D53                     @ this is used to indicate the interrupt handler the rendering was finished properly
    STR     R3, [R0]
    ADD     SP, SP, #0x1C
    POP     {R0-R7}
    MOV     R8, R0
    MOV     R9, R1
    MOV     R10, R2
    MOV     R11, R3
    POP     {PC}

    .pool

    .arm
    .align  2

C_setup_synth:
    LDRB    R12, [R3, #SYNTH_TYPE]
    CMP     R12, #0
    BNE     C_check_synth_saw

    /* modulating pulse wave */
    LDRB    R6, [R3, #SYNTH_WIDTH_CHANGE_1]
    ADD     R2, R2, R6, LSL#24
    LDRB    R6, [R3, #SYNTH_WIDTH_CHANGE_2]
    ADDS    R6, R2, R6, LSL#24
    MVNMI   R6, R6
    MOV     R10, R6, LSR#8
    LDRB    R1, [R3, #SYNTH_MOD_AMOUNT]
    LDRB    R0, [R3, #SYNTH_BASE_WAVE_DUTY]
    MOV     R0, R0, LSL#24
    MLA     R6, R10, R1, R0                 @ calculate the final duty cycle with the offset, and intensity * rotating duty cycle amount
    STMFD   SP!, {R2, R3, R9, R12}

C_synth_pulse_loop:
    LDMIA   R5, {R0-R3, R9, R10, R12, LR} @ load 8 samples
    CMP     R7, R6                      @ Block #1
    ADDLO   R0, R0, R11, LSL#6
    SUBHS   R0, R0, R11, LSL#6
    ADDS    R7, R7, R4, LSL#3
    CMP     R7, R6                      @ Block #2
    ADDLO   R1, R1, R11, LSL#6
    SUBHS   R1, R1, R11, LSL#6
    ADDS    R7, R7, R4, LSL#3
    CMP     R7, R6                      @ Block #3
    ADDLO   R2, R2, R11, LSL#6
    SUBHS   R2, R2, R11, LSL#6
    ADDS    R7, R7, R4, LSL#3
    CMP     R7, R6                      @ Block #4
    ADDLO   R3, R3, R11, LSL#6
    SUBHS   R3, R3, R11, LSL#6
    ADDS    R7, R7, R4, LSL#3
    CMP     R7, R6                      @ Block #5
    ADDLO   R9, R9, R11, LSL#6
    SUBHS   R9, R9, R11, LSL#6
    ADDS    R7, R7, R4, LSL#3
    CMP     R7, R6                      @ Block #6
    ADDLO   R10, R10, R11, LSL#6
    SUBHS   R10, R10, R11, LSL#6
    ADDS    R7, R7, R4, LSL#3
    CMP     R7, R6                      @ Block #7
    ADDLO   R12, R12, R11, LSL#6
    SUBHS   R12, R12, R11, LSL#6
    ADDS    R7, R7, R4, LSL#3
    CMP     R7, R6                      @ Block #8
    ADDLO   LR, LR, R11, LSL#6
    SUBHS   LR, LR, R11, LSL#6
    ADDS    R7, R7, R4, LSL#3

    STMIA   R5!, {R0-R3, R9, R10, R12, LR} @ write 8 samples
    SUBS    R8, R8, #8
    BGT     C_synth_pulse_loop

    LDMFD   SP!, {R2, R3, R9, R12}
    B       C_end_mixing

C_check_synth_saw:
    /*
     * This is actually not a true saw wave
     * but looks pretty similar
     * (has a jump in the middle of the wave)
     */
    SUBS    R12, R12, #1
    BNE     C_synth_triangle

    MOV     R6, #0x300
    MOV     R11, R11, LSR#1
    BIC     R11, R11, #0xFF00
    MOV     R12, #0x70

C_synth_saw_loop:

    LDMIA   R5, {R0, R1, R10, LR}       @ load 4 samples from memory
    ADDS    R7, R7, R4, LSL#3           @ Block #1 (some oscillator type code)
    RSB     R9, R12, R7, LSR#24
    MOV     R6, R7, LSL#1
    SUB     R9, R9, R6, LSR#27
    ADDS    R2, R9, R2, ASR#1
    MLANE   R0, R11, R2, R0

    ADDS    R7, R7, R4, LSL#3           @ Block #2
    RSB     R9, R12, R7, LSR#24
    MOV     R6, R7, LSL#1
    SUB     R9, R9, R6, LSR#27
    ADDS    R2, R9, R2, ASR#1
    MLANE   R1, R11, R2, R1

    ADDS    R7, R7, R4, LSL#3           @ Block #3
    RSB     R9, R12, R7, LSR#24
    MOV     R6, R7, LSL#1
    SUB     R9, R9, R6, LSR#27
    ADDS    R2, R9, R2, ASR#1
    MLANE   R10, R11, R2, R10

    ADDS    R7, R7, R4, LSL#3           @ Block #4
    RSB     R9, R12, R7, LSR#24
    MOV     R6, R7, LSL#1
    SUB     R9, R9, R6, LSR#27
    ADDS    R2, R9, R2, ASR#1
    MLANE   LR, R11, R2, LR

    STMIA   R5!, {R0, R1, R10, LR}
    SUBS    R8, R8, #4
    BGT     C_synth_saw_loop

    B       C_end_mixing

C_synth_triangle:
    MOV     R6, #0x80
    MOV     R12, #0x180

C_synth_triangle_loop:
    LDMIA   R5, {R0, R1, R10, LR}       @ load samples from work buffer
    ADDS    R7, R7, R4, LSL#3           @ Block #1
    RSBPL   R9, R6, R7, ASR#23
    SUBMI   R9, R12, R7, LSR#23
    MLA     R0, R11, R9, R0

    ADDS    R7, R7, R4, LSL#3           @ Block #2
    RSBPL   R9, R6, R7, ASR#23
    SUBMI   R9, R12, R7, LSR#23
    MLA     R1, R11, R9, R1

    ADDS    R7, R7, R4, LSL#3           @ Block #3
    RSBPL   R9, R6, R7, ASR#23
    SUBMI   R9, R12, R7, LSR#23
    MLA     R10, R11, R9, R10

    ADDS    R7, R7, R4, LSL#3           @ Block #4
    RSBPL   R9, R6, R7, ASR#23
    SUBMI   R9, R12, R7, LSR#23
    MLA     LR, R11, R9, LR

    STMIA   R5!, {R0, R1, R10, LR}
    SUBS    R8, R8, #4                  @ subtract #4 from the remainging samples
    BGT     C_synth_triangle_loop

    B       C_end_mixing

/* R0: base addr
 * R1: len in bytes */
F_clear_mem:
    STMFD   SP!, {R0, R2-R5, LR}
    MOV     R2, #0
    MOV     R3, #0
    MOV     R4, #0
    MOV     R5, #0
    AND     LR, R1, #0x30
    RSB     LR, LR, #0x30
    ADD     PC, PC, LR, LSR#2
C_clear_loop:
    STMIA   R0!, {R2-R5}
    STMIA   R0!, {R2-R5}
    STMIA   R0!, {R2-R5}
    STMIA   R0!, {R2-R5}
    SUBS    R1, R1, #0x40
    BPL     C_clear_loop
    ANDS    R1, R1, #0xC
    LDMEQFD SP!, {R0, R2-R5, PC}
C_clear_loop_rest:
    STMIA   R0!, {R2}
    SUBS    R1, R1, #4
    BGT     C_clear_loop_rest
    LDMFD   SP!, {R0, R2-R5, PC}
