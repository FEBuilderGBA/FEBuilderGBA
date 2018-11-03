.thumb
@Branching from 0x08058158
@r0 ok
@r1 ok
@r2 ok
@r3 ok.
@r4  reserved.
@r5  reserved.
@r6  reserved.
@r7  reserved.
cmp r7, #0x0
beq Something_of_left
	mov r3, r10
	bl   find_anime
	mov r5,r0    @Set animation pointer on left
Something_of_left:
mov r6, r9
cmp r6, #0x0
beq exit
    ldr r3,[sp, #0x18]
    bl   find_anime
    str r0,[sp, #0x1c]    @Set animation pointer on right

exit:
ldr r0,=0x08058171
mov pc,r0

@@find Animation
find_anime:

ldr  r0, [r3, #0x4]  @get Class Struct
ldrb r2, [r0, #0x4]  @Class->Class_ID

ldr  r0, [r3, #0x0]  @get Unit Struct
ldrb r0, [r0, #0x4]  @Unit->Unit_ID

lsl  r2,r2,#0x8
orr  r2,r0           @Class->Class_ID << 8 | Unit->Unit_ID

@Search the table
ldr  r1,CustomAnimeTable
loop_search_table:
ldrh r0,[r1, #0x0]		 @ read custom animation for class_id<<8 | unitid
cmp  r0,#0x00        @ term data
beq  not_found
cmp  r0,r2           @ check class_id and unit_id
beq  found
add  r1,#0x8         @ next table
b    loop_search_table

found:
ldr r0,[r1, #0x4]    @I found a dedicated animation pointer.
b   store_and_exit

not_found:
ldr r0, [r3, #0x4]
ldr r0, [r0, #0x34]  @Default Class animation

store_and_exit:
mov	r2,lr
bx	r2

.ltorg
CustomAnimeTable:
