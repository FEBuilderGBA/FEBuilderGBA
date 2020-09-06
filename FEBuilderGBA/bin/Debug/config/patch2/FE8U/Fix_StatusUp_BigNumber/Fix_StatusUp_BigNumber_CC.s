@Call 080770B4	{J}
@Call 08074C7C	{U}
.thumb


cmp r0, #11  @11以上だと表示が乱れるので、+マークだけにする
bge Overflow

mov r3, #11
neg r3, r3
cmp r0, r3  @-11以上だと表示が乱れるので、-マークだけにする
ble Underflow
b   Exit

Overflow:
mov r0, #17	@どういうわけか、0x11(17)にすると、+マークだけになる
b   Exit

Underflow:
mov r0, #17	@どういうわけか、0x11(17)にすると、-マークだけになる
neg r0, r0
@b   Exit

Exit:
@壊すコードの再送
@mov r7, r9
mov r6, r8
push {r6,r7}
mov r7 ,r0
mov r6 ,r1
@ldr r3,=0x080770BC|1	@{J}
ldr r3,=0x08074C84|1	@{U}
bx r3
