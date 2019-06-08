.thumb
@draws the stat screen
.include "mss_defs.s"
.set SkillGetter, IconGraphic+4
.set SkillTester, SkillGetter+4
.set SaviorID, SkillTester+4
.set CelerityID, SaviorID+4
.set SS_SkillsText, CelerityID+4
.set SS_TalkText, SS_SkillsText+4

page_start

@draw str or mag
  mov r0, r8
  blh     MagCheck      @r0 = 1 if mag should show
  cmp     r0,#0x0       
  beq     NotMag        
    @draw Mag at 13, 3. colour defaults to yellow.
    draw_textID_at 13, 3, textID=0x4ff
    draw_str_bar_at 16, 3
    b       MagStrDone    
  NotMag:
    @draw Str at 13, 3
    draw_textID_at 13, 3, textID=0x4fe
    draw_str_bar_at 16, 3
  MagStrDone:

draw_textID_at 13, 5, textID=0x4EC @skl
draw_textID_at 13, 7, textID=0x4ED @spd

@ rescue_check
@ cmp r0, #0
@ beq NoRescue
@   @halved if Rescue
@   mov r0, r8
@   @check for savior
@   .set saviorloc, (SaviorID - . - 6)
@   ldr r1, =saviorloc
@   add r1, pc
@   ldr r1, [r1]
@   .set skilltestloc, (SkillTester - . - 6) 
@   ldr r2, =skilltestloc
@   add r2, pc
@   ldr r2, [r2]
@   mov lr, r2
@   .short 0xf800
@   cmp r0, #0
@   bne NoRescue
@   draw_skl_reduced_bar_at 16, 5
@   draw_spd_reduced_bar_at 16, 7
@ b RescueCheckEnd
b NoRescue
.ltorg
NoRescue:
  draw_skl_bar_at 16, 5
  draw_spd_bar_at 16, 7
RescueCheckEnd:

draw_textID_at 13, 9, textID=0x4ee @luck
draw_luck_bar_at 16, 9

draw_textID_at 13, 11, textID=0x4ef @def
draw_def_bar_at 16, 11

draw_textID_at 13, 13, textID=0x4f0 @res
draw_res_bar_at 16, 13

draw_textID_at 13, 15, 0x4f6 @move
draw_move_bar_with_getter_at 16, 15

draw_textID_at 13, 17, textID=0x4f7 @con
draw_con_bar_with_getter_at 16, 17


draw_textID_at 21, 3, textID=0x4f8 @aid
draw_number_at 25, 3, 0x80189B8, 2 @aid getter
draw_aid_icon_at 26, 3

draw_trv_text_at 21, 5

draw_textID_at 21, 7, textID=0x4f1 @affin

draw_affinity_icon_at 24, 7

draw_status_text_at 21, 9

.set ss_talkloc, (SS_TalkText - . - 6)
  ldr r0, =ss_talkloc
  add r0, pc
  ldr r0, [r0]
draw_talk_text_at 21, 11

.set ss_skillloc, (SS_SkillsText - . - 6)
  ldr r0, =ss_skillloc
  add r0, pc
  ldr r0, [r0]
draw_textID_at 23, 13, colour=White @skills

b skipliterals
.ltorg
skipliterals:

mov r0, r8
ldr r1, SkillGetter
mov lr, r1
.short 0xf800 @skills now stored in the skills buffer

mov r6, r0
ldrb r0, [r6] 
cmp r0, #0
beq SkillEnd
draw_skill_icon_at 21, 15

ldrb r0, [r6,#1]
cmp r0, #0
beq SkillEnd
draw_skill_icon_at 24, 15

ldrb r0, [r6, #2]
cmp r0, #0
beq SkillEnd
draw_skill_icon_at 27, 15

ldrb r0, [r6, #3]
cmp r0, #0
beq SkillEnd
draw_skill_icon_at 21, 17

ldrb r0, [r6, #4]
cmp r0, #0
beq SkillEnd
draw_skill_icon_at 24, 17

ldrb r0, [r6, #5]
cmp r0, #0
beq SkillEnd
draw_skill_icon_at 27, 17

SkillEnd:

@ draw_textID_at 13, 15, textID=0x4f6 @move
@ draw_move_bar_at 16, 15

@blh DrawBWLNumbers

page_end

.ltorg

.include "Get Talkee.asm"

.include "alternateicondraw.s"
@POIN SkillIcons at the end here
@POIN SkillGetter after that
@POIN SkillTester after THAT
@WORD SaviorID lol
