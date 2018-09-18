@ボタンを押していると、戦闘アニメを倍速再生
@ Orignal https://github.com/FireEmblemUniverse/SelfRandomizingRom-fe8/blob/920248ac5deecda17fae4f2709feb3f0d166d411/src/randomizer_a.c#L345

@Call 4D78
@r0 temp
@r1 duration
@r3 work
@r4 AISPointer[]

.thumb

LDR r3, =0x02024CC0 @ FE8J (KeyStatusBuffer@KeyStatusBuffer.FirstTickDelay )
LDRH r2, [r3, #0x4] @      (KeyStatusBuffer@KeyStatusBuffer.Current )
MOV r3, #0x2
AND r3 ,r2
BNE Term              @Bボタンが押されている場合、プラマイゼロでdurationの値をそのまま格納する
                      @これを自動的に最適化したコンパイラは賢い!! 偉い!!

    LSL r2 ,r2 ,#0x1F
    BMI Next2
                      @どのボタンも押されていない場合は、 duration - 1 をする
        SUB r1,r1, #0x1  @ duration - 1
        B Term      
    Next2:            @Aボタンが押されている
                      @可読性を重視してここに移動する
        mov r1,#0x00  @即終了
Term:
STRH r1, [r4, #0x6]   @ AISPointer[3] = r1

                      @ ジャンプコードが入らないので、cmpの部分も侵食する
                      @ 侵食するコードの再送
lsl r0 ,r0 ,#0x10
CMP r0, #0x0          @フラグ判定だけやる
                      @LDR と BXではフラグは変わらないので
LDR r3, =0x08004D80+1 @ FE8J bneの部分に戻します
bx  r3
