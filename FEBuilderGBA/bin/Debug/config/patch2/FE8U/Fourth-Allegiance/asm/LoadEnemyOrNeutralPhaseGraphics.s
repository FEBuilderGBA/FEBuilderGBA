.thumb
.org 0x0

.set NeutralPhaseBannerPalette, NeutralPhaseBannerGfx+4

cmp r0, #0x80
bne NeutralPhase @ If not enemy phase, then neutral phase

EnemyPhase:
ldr r0,=0x859FA4C
ldr r1, =0x6002800
ldr r3, =0x8012F51
mov r14, r3
.short 0xF800
ldr r4, =0x85A0068
mov r0, r4
mov r1, #0xA0
mov r2, #0x20
ldr r3, =0x8000DB9
mov r14, r3
.short 0xF800
mov r1, #0x90
lsl r1, #2
mov r0, r4
mov r2, #0x20
ldr r3, =0x8000DB9
mov r14, r3
.short 0xF800
b End

NeutralPhase:
ldr r0, NeutralPhaseBannerGfx
ldr r1, =0x6002800
ldr r3, =0x8012F51
mov r14, r3
.short 0xF800
ldr r4, NeutralPhaseBannerPalette
mov r0, r4
mov r1, #0xA0
mov r2, #0x20
ldr r3, =0x8000DB9
mov r14, r3
.short 0xF800
mov r1, #0x90
lsl r1, #2
mov r0, r4
mov r2, #0x20
ldr r3, =0x8000DB9
mov r14, r3
.short 0xF800



End:
pop {r4}
pop {r0}
bx r0

.align
.pool
NeutralPhaseBannerGfx:
