.thumb
.include "mss_defs.s"

page_start

@ mov r0, r8
@ blh      MagCheck
@ cmp     r0,#0x0
@ beq     NotMag
draw_weapon_rank_at 9, 1, Anima, 0
draw_weapon_rank_at 9, 3, Light, 1
draw_weapon_rank_at 9, 5, Dark, 2
draw_weapon_rank_at 9, 7, Staff, 3
@ b       EndRanks
@ .ltorg

@ NotMag:
draw_weapon_rank_at 1, 1, Sword, 4
draw_weapon_rank_at 1, 3, Lance, 5
draw_weapon_rank_at 1, 5, Axe, 6
draw_weapon_rank_at 1, 7, Bow, 7

@ EndRanks:

@ blh      DrawSupports

page_end
