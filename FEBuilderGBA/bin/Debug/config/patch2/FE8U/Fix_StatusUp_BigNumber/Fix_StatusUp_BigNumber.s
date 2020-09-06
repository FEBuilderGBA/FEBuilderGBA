@Call 0808133C	{J}
@Call 0807EFFC	{U}
.thumb

cmp r2, #11  @11以上だと表示が乱れるので、+マークだけにする
bge Overflow

mov r3, #11
neg r3, r3
cmp r2, r3  @-11以上だと表示が乱れるので、-マークだけにする
ble Underflow
b   Exit

Overflow:
mov r2, #17	@どういうわけか、0x11(17)にすると、+マークだけになる
b   Exit

Underflow:
mov r2, #17	@どういうわけか、0x11(17)にすると、-マークだけになる
neg r2, r2
@b   Exit

Exit:
@壊すコードの再送
mov r10, r0
mov r8, r1
mov r4 ,r2
mov r9, r4
@ldr r3,=0x08081344|1	@{J}
ldr r3,=0x0807F004|1	@{U}
bx r3
