.thumb
.include "mss_defs.s"

page_start

mov r0, r8
blh      MagCheck
cmp     r0,#0x0
beq     NotMag
draw_weapon_rank_at 1, 1, Anima, 0
draw_weapon_rank_at 1, 3, Light, 1
draw_weapon_rank_at 9, 1, Dark, 2
draw_weapon_rank_at 9, 3, Staff, 3
b       EndRanks
.ltorg

NotMag:
draw_weapon_rank_at 1, 1, Sword, 0
draw_weapon_rank_at 1, 3, Lance, 1
draw_weapon_rank_at 9, 1, Axe, 2
draw_weapon_rank_at 9, 3, Bow, 3

EndRanks:

blh      DrawSupports

page_end
