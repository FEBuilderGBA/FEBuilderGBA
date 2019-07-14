.thumb
@draws the stat screen
.include "mss_defs.s"


page_start

@draw str
    draw_textID_at 13, 3, textID=0x4fe
    draw_str_bar_at 16, 3
@Draw skl
draw_textID_at 13, 7, textID=0x4EC
@draw spd
draw_textID_at 13, 9, textID=0x4ED

rescue_check @r0 = 10 if true, 0 if false
cmp r0, #0
beq NoRescue

  @halved if Rescue
  draw_skl_reduced_bar_at 16, 7
  draw_spd_reduced_bar_at 16, 9
b RescueCheckEnd
.ltorg
NoRescue:
  draw_skl_bar_at 16, 7
  draw_spd_bar_at 16, 9
RescueCheckEnd:

draw_textID_at 13, 11, textID=0x4ee @luck
draw_luck_bar_at 16, 11

draw_textID_at 13, 13, textID=0x4ef @def
draw_def_bar_at 16, 13

draw_textID_at 13, 15, textID=0x4f0 @res/mag
draw_res_bar_at 16, 15

draw_textID_at 13, 5, textID=0x4ff @res/mag
draw_res_bar_at 16, 5

NextColumn:
draw_textID_at 21, 3, textID=0x4f6 @move
draw_move_bar_at 24, 3

draw_textID_at 21, 5, textID=0x4f7 @con
draw_con_bar_at 24, 5


draw_textID_at 21, 7, textID=0x4f8 @aid
draw_number_at 25, 7, 0x80189B8, 2 @aid getter
draw_aid_icon_at 26, 7

draw_trv_text_at 21, 9

draw_textID_at 21, 11, textID=0x4f1 @affin

draw_status_text_at 21, 13

draw_affinity_icon_at 24, 11

@draw_talk_text_at 21, 15

b continue

.align 2, 0

continue:
blh DrawBWLNumbers

page_end
.ltorg
.align

.include "Get Talkee.asm"