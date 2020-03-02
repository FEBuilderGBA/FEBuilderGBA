.thumb
OptionByte2 = 0x0202BD2D

    push {r7}
    sub sp, #0x10
    mov r7, sp

    ldr r0, =OptionByte2
    ldrb r0, [r0]
    mov r1, #0x80
    tst r0, r1
    bne return

    bl Always

    bl Selecting

return:
    add sp, #0x10
    pop {r7}
mov r0, #0x30
ldrb r0, [r4,r0]
lsl r0, #0x1C
lsr r0, #0x1C
ldr r1, =0x08027660
mov pc, r1

Always:
        push {lr}
        bl CameraCacheFunc        @カメラ位置をキャッシュ
        cmp r0, #0
        beq falseAlways
        bl HpBars
        bl Pinned
    falseAlways:
        pop {pc}

Selecting:
        push {r6, lr}
        bl IsSelecting
        cmp r0, #0
        beq endSelect

        bl CheckCache
        cmp r0, #1
        beq trueCahce
    falseCache:
        bl CacheFunc
        b endSelect

    trueCahce:
        bl DisplayOtherIcons
    endSelect:
        pop {r6, pc}

CacheFunc:
        push {lr}
        mov r6, #0

        bl IsDanger
        orr r6, r0
        bl CanTalk
        lsl r0, #2
        orr r6, r0

        ldr r0, WarningCache
        add r0, #2
        ldrb r1, [r4,#0xB]
        strb r6, [r0, r1]       @WriteToCache
        pop {pc}

IsDanger:
        push {lr}
        ldr r0, =OptionByte2
        ldrb r0, [r0]
        mov r1, #0x10
        tst r0, r1
        bne falseDanger

        ldrb r0, [r4, #0xB]
        mov r1, #0x80
        tst r0, r1
        beq falseDanger

        bl JudgeDanger
        .short 0xE000

    falseDanger:
        mov r0, #0
        pop {pc}


BATTLE_SIZE = (128)

JudgeDanger:
        push {lr}
        sub sp, #BATTLE_SIZE

        mov r0, #15
        ldr r1, =0x03004df0
        ldr r1, [r1]
        mov r2, r4
        bl CheckXY
        cmp r0, #0
        beq notDanger

        ldr r0, =0x0203a4e8
        ldr r1, =0x03004df0
        ldr r1, [r1]

        ldrb r2, [r0, #0x0B]
        ldrb r3, [r1, #0x0B]
        cmp r2, r3
        beq dontNeed
        bl SetInitDataR1toR0
    dontNeed:
        mov r0, sp
        mov r1, r4
        bl SetInitDataR1toR0

        mov r0, sp
        ldr r1, =0x0203a4e8
        bl CALC_TRIANGLE_FUNC

        mov r0, sp
        ldr r1, =0x0203a4e8
        bl CalcAttackFunc

        ldr r0, =0x0203a4e8
        mov r1, sp
        bl CalcDefenseFunc

        ldr r0, LITE_MODE
        cmp r0, #1
        beq skipVersus

        mov r0, sp
        ldr r1, =0x0203a4e8
        bl CALC_VERSUS_FUNC

        ldr r0, =0x0203a4e8
        mov r1, sp
        bl CALC_VERSUS_FUNC
    skipVersus:
        mov r0, #90
        add r0, sp
        ldrh r0, [r0]

        ldr r3, =0x0203a4e8
        mov r1, #92
        ldrh r1, [r3, r1]
        sub r0, r1
        ble notDanger
        ldrb r1, [r3, #19]  @現在HP
        cmp r1, r0
        ble trueDanger      @即死する

        mov r2, #10
        mul r0, r2
        ldrb r1, [r3, #18]  @最大HP
        mov r2, #8
        mul r1, r2
        swi #6

        cmp r0, #1
        blt notDanger       @HP8割以上
    trueDanger:
        mov r0, #1
        .short 0xE000
    notDanger:
        mov r0, #0
        add sp, #BATTLE_SIZE
        pop {pc}


UNIT_SIZE = (60)

SetInitDataR1toR0:
        push {r0, lr}
        mov r2, #UNIT_SIZE
        bl MEMCPY_R1toR0_FUNC
        ldr r0, [sp]
        bl Initialize
        pop {r0, pc}

Initialize:
        push {r0, lr}

        bl HitPointFunc
        strb r0, [r1, #18]

        ldr r0, [sp]
        bl StrongFunc
        ldr r1, [sp]
        strb r0, [r1, #20]

        ldr r0, [sp]
        bl DefenseFunc
        ldr r1, [sp]
        strb r0, [r1, #23]

        ldr r0, [sp]
        bl ResistFunc
        ldr r1, [sp]
        strb r0, [r1, #24]

        ldr r0, [sp]
        bl MagicFuncIfNeed

        ldr r0, [sp]
        bl EquipmentFunc

        ldr r0, [sp]
        ldr r1, =0x0203a4e8
        cmp r0, r1
        bne jumpInit            @自キャラ以外は不要
        bl CALC_TERRAIN_FUNC
    jumpInit:
        pop {r0, pc}

MagicFuncIfNeed:
        push {lr}
        ldr r3, =0x08018ecc     @magic_func
        ldr r1, [r3]
        ldr r2, =0x468F4900
        cmp r1, r2
        bne notMagic

        mov lr, r3
        .short 0xF800
        strb r0, [r1, #26]
    notMagic:
        pop {pc}

EquipmentFunc:
        push {r4, r5, r6, lr}
        mov r6, r0

        mov r2, #0
        mov r1, #83
        strb r2, [r6, r1]       @すくみ初期化
        mov r1, #84
        strb r2, [r6, r1]       @すくみ初期化

        bl $080168d0
        ldr r1, =0x0802a894
        mov pc, r1

$080168d0:
    ldr r1, =0x080168d0
    mov pc, r1
$08017314:
    ldr r1, =0x08017314
    mov pc, r1


CalcAttackFunc:
        push {r4, r5, lr}
        mov r4, r0
        mov r5, r1

        ldr r2, =0x0802aa28
        mov lr, r2
        .short 0xF800

        ldr r0, LITE_MODE
        cmp r0, #1
        bne continueCalcAtk
        pop {r4, r5, pc}
    continueCalcAtk:
        mov r0, r4
        ldr r2, =0x0802acc4
        mov lr, r2
        .short 0xF800

        mov r0, r4
        ldr r2, =0x0802ad00
        mov lr, r2
        .short 0xF800

        ldr r0, =0x0802a90c
        mov pc, r0
CalcDefenseFunc:
        push {r4, r5, lr}
        mov r4, r0
        mov r5, r1

        ldr r2, =0x0802a9b0
        mov lr, r2
        .short 0xF800

        ldr r0, LITE_MODE
        cmp r0, #1
        bne continueCalcDef
        pop {r4, r5, pc}
    continueCalcDef:
        mov r0, r4
        ldr r2, =0x0802ad00
        mov lr, r2
        .short 0xF800

        ldr r0, =0x0802a90c
        mov pc, r0


HitPointFunc:
    ldr r2, =0x08018ea4
    mov pc, r2
StrongFunc:
    ldr r2, =0x08018ec4
    mov pc, r2
DefenseFunc:
    ldr r2, =0x08018f64
    mov pc, r2
ResistFunc:
    ldr r2, =0x08018f84
    mov pc, r2

CALC_TERRAIN_FUNC:
    ldr r2, =0x0802a648
    mov pc, r2

CALC_TRIANGLE_FUNC:
    ldr r2, =0x0802c6f8
    mov pc, r2
@CALC_ALL_FUNC:
@    ldr r2, =0x0802a8c8
@    mov pc, r2
CALC_VERSUS_FUNC:
    ldr r2, =0x0802ad3c
    mov pc, r2

MEMCPY_R1toR0_FUNC:
    ldr r3, =0x080d6908
    mov pc, r3

CanTalk:
        push {lr}
        ldr r0, =0x03004df0
        ldr r0, [r0]
        ldr r0, [r0]
        ldrb r0, [r0, #4]
        ldr r1, [r4]
        ldrb r1, [r1, #4]
        bl TALK_CHECK
        pop {pc}

TALK_CHECK:
    ldr r2, =0x08086240
    mov pc, r2

CheckCache:
        ldr r0, WarningCache
        ldrb r1, [r0]
        cmp r1, #2
        beq completedCache       @キャッシュ済み
        ldrb r1, [r0, #1]
        ldrb r2, [r4, #0xB]
        cmp r2, r1
        ble completeCache      @キャッシュ完了
    @未キャッシュ
        strb r2, [r0, #1]   @スタートの部隊IDストア
        mov r1, #1
        strb r1, [r0]
        mov r0, #0
        b endCache
    completeCache:
        mov r1, #2
        strb r1, [r0]
    completedCache:
        mov r0, #1
    endCache:
        bx lr

DisplayOtherIcons:
        push {lr}
        ldr    r6, WarningCache
        add    r6,#2
        ldrb   r0,[r4,#0xB]
        ldrb   r6,[r6,r0]
        cmp    r6,#0
        beq    GoBack               @nothing to display
    DisplayEffective:
        mov    r0,#0b0001
        tst    r0,r6
        beq    DisplayCrit
        mov    r0,#0x00             @IconSelect
        bl     Draw_Warning_Sign
    DisplayCrit:
        mov    r0,#0b0010
        tst    r0,r6
        beq    DisplayTalk
        mov    r0,#0x00             @IconSelect
        bl     Draw_Warning_Sign
    DisplayTalk:
        mov    r0, #0b0100
        tst    r0,r6
        beq    GoBack
        mov    r0,#0x08             @IconSelect
        bl     Draw_Warning_Sign
    GoBack:
        pop {pc}


IsSelecting:
        ldr r2, =0x03004df0
        ldr r2, [r2]
        cmp r2, #0
        beq notSelecting

        ldrb r0, [r2, #0xB]
        mov r1, #0xC0
        tst r0, r1
        bne notSelecting

        ldrb r0, [r2, #0xC]
        mov r1, #1
        tst r0, r1
        beq notSelecting

        mov r0, #1
        bx lr
    notSelecting:
        ldr r1, WarningCache
        mov r0, #0
        str r0, [r1]        @ClearCache
        bx lr



x_coord = 0x10
y_coord = 0x11
CameraStuff = 0x0202BCAC
Xvalue = (0x0201)

CameraCacheFunc:
        push {lr}
        mov r0, #0
        str r0, [r7,#0x0]

        mov      r1,#x_coord
        ldsb  r1,[r4,r1]
        lsl      r1,#4
        ldr      r2,=CameraStuff
        mov      r3,#0xC
        ldsh  r0,[r2,r3]           @camera x
        sub      r3,r1,r0          @r3= x - camera_x
        mov      r0,#y_coord
        ldsb  r0,[r4,r0]
        lsl      r0,#4
        mov      r1,#0xE
        ldsh  r1,[r2,r1]           @camera y
        sub      r2,r0,r1          @r2 = y - camera_y
        mov      r1,r3
        add      r1,#0x10
        mov      r0,#0x80
        lsl      r0,#1
        cmp      r1,r0
        bhi      falseScreen         @x is either >0x100 or <0, so out of range
        mov      r0,r2
        add      r0,#0x10
        cmp      r0,#0xB0
        bhi      falseScreen         @y is either >0xB0 or <0, so out of range

        str      r3,[r7,#0x4]         @sp+4 = x - x'
        str      r2,[r7,#0x8]         @sp+8 = y - y'
@Find out whether we even need to display an hp bar
        mov r0, #1
        str r0, [r7,#0x0]
        .short 0xE000
    falseScreen:
        mov r0, #0
        pop {pc}

HpBars:
        push {lr}

        ldrb r0, [r4, #18]  @
        ldrb r2, [r4, #19]  @現在HP
        cmp r2, r0
        bge falseHPBars @HP最大
        mov r1, r0
        sub r0, r2
        mov r2, #11
        mul r0, r2
        swi #6      @damage*11(r0)/maxHP(r1)
        lsl r2, r0, #2
        ldr r3, HPFramePointers
        ldr r2, [r3, r2]

        ldr r0, [r7, #4]
        ldr r3, = Xvalue
        add r0, r3
        sub r3, #2      @Create 0x1FF
        and r0, r3

        ldr r1, [r7, #8]
        add r1, #0xFB
        mov r3, #0xFF
        and r1, r3

        ldr r3, =0x08002B08
        mov lr, r3
        mov r3, #0
        .short 0xF800
    falseHPBars:
        pop {pc}

Pinned:
        push {lr}
        ldrb r0, [r4, #11]
        lsr r0, r0, #6
        beq falsePin
        mov r0, #0x3B
        ldrb r0, [r4, r0]
        lsl r0, r0, #24
        bpl falsePin

        ldr r2, PinFramePointers

        ldr r0, [r7, #4]
        ldr r3, = Xvalue
        add r0, r3
        sub r3, #2      @Create 0x1FF
        and r0, r3

        ldr r1, [r7, #8]
        add r1, #0xFB
        mov r3, #0xFF
        and r1, r3

        ldr r3, =0x08002B08
        mov lr, r3
        mov r3, #0
        .short 0xF800
    falsePin:
        pop {pc}

WRAMDisplay = 0x08002B08

Draw_Warning_Sign:
@r0=thing to determine what we're drawing, r1=sp (to retrieve x and y stuff)
        push {lr}
        ldr r2, [r7, #0]
        cmp r2, #0
        beq FinishedDrawing

        ldr r2, WS_FrameData
        add r2, r0

        ldr    r1, =0x0201
        ldr    r0,[r7,#4]        @x-x'
        add    r0,r1
        add    r0,#0xB              @tweak for x coordinate, whatever that means
        sub    r1,#2
        and    r0,r1

        ldr    r1,[r7,#8]        @y-y'
        add    r1,#0xEE          @y coordinate tweak?
        mov    r3,#0xFF
        and    r1,r3

        ldr    r3, =WRAMDisplay
        mov    lr, r3
        mov    r3,#0
        .short 0xF800
FinishedDrawing:
        pop {pc}

CheckXY:
@r1とr2がr0マス以内に居るならr0=TRUE
@同座標ならTRUE
@
        push {r0}
        ldrb r0, [r1, #16]
        ldrb r3, [r2, #16]
        sub r3, r0, r3
        bge jump1CheckXY
        neg r3, r3  @絶対値取得
    jump1CheckXY:

        ldrb r1, [r1, #17]
        ldrb r2, [r2, #17]
        sub r2, r1, r2
        bge jump2CheckXY
        neg r2, r2  @絶対値取得
    jump2CheckXY:

        add r2, r2, r3
        pop {r0}
        cmp r2, r0
        bgt falseCheckXY    @r0マス以内に居ない
        mov r0, #1
        bx lr
    falseCheckXY:
        mov r0, #0
        bx lr

LITE_MODE = addr+0
HPFramePointers = addr+4
PinFramePointers = addr+8
WarningCache = addr+12
WS_FrameData = addr+16

.align
.ltorg
addr:
