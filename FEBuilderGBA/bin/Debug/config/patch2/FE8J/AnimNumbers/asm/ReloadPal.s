@ Hooked at 0x6C6C0.
@ Re-loads the Miss/NoDamage palette.
@ r0 holds AISSubjectID.
.thumb

push  {r14}

@ Vanilla, arg to StartEkrSubAnimeEmulator.
str   r2, [sp, #0x4]

mov   r1, r0
@ldr   r0, =0x8802884	@Blue Palette	@{U}
ldr   r0, =0x8856DDC	@Blue Palette	@{J}
lsl   r2, r1, #0x5
add   r0, r2
sub   r1, #0x1
neg   r1, r1
lsl   r1, #0x5
ldr   r2, =0x20228A8+0x2A0	@gPaletteBuffer+0x2A0	@{U}	@{J}
add   r1, r2
mov   r2, #0x8
swi   #0xC              @ CpuFastSet

@ldr   r3, =0x8001F94|1 @EnablePaletteSync	@{U}
ldr   r3, =0x8001EE4|1 @EnablePaletteSync	@{J}
bl    GOTO_R3

@ Vanilla stuff overwritten by hook.
@ These prepare args to a StartEkrSubAnimeEmulator call.
mov   r1, #0x2
ldsh  r0, [r5, r1]
mov   r3, #0x4
ldsh  r1, [r5, r3]
sub   r1, #0x28

pop   {r3}
GOTO_R3:
bx    r3
