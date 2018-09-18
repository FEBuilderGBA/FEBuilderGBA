;マップ変化が実行されたらフラグを立てる. by 7743
;
;r3 マップ変化アドレス store to r8
;r4 store to r9
;
;
;
;
;@org	$0802C282	//FE7J
;@org	$0802BDC2	//FE7U
@thumb

ldrb r0,[r3,0x05]
cmp r0,0x00
beq exit	;00なら設定なし

ldr r1,=$0807A0B4 ;フラグを立てる関数	//FE7J
;ldr r1,=$080798E4 ;フラグを立てる関数	//FE7U
mov	r14, r1
@dcw	$F800

exit
;壊すコードを再送
pop {r3,r4}
mov r8, r3
mov r9, r4
pop {r4,r5,r6,r7}
pop {r0}
bx r0       ;ついでだから、 bx r0 で自動的に戻ろう。そっちが楽だ。
