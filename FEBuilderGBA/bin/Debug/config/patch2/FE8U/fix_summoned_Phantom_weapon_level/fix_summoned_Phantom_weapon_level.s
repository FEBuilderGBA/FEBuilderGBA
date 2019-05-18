@
@Change the weapon level rules of the summoned Phantom
@Allocate weapon levels you own, not the summons level.
@
@召喚された亡霊戦士の武器レベルの規則を変更します。
@召喚者のレベルではなく、保有する武器レベルを割り振るようにします。
@
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@Hook 0x0807AF6C FE8U

.thumb
ldr  r1, =0x03004E50    @ 操作キャラのワークメモリへのポインタ
ldr  r0, [r1, #0x0]     @ CurrentUnit->RamUnit
ldrb r0, [r0, #0x8]     @ CurrentUnit->RamUnit->Lv
strb r0, [r5, #0x8]     @ Unit->Lv = CurrentUnit->RamUnit->Lv
mov  r0, #0xff
strb r0, [r5, #0x9]     @ Unit->Exp = 0xFF

ldrb r0, [r5,#0x1E]     @ Get Weapon ID
blh 0x080177B0          @ GetROMItemStructPtr FE8U
ldrb r1, [r0,#0x7]      @ Item->Attribute
cmp  r1, #0x08          @ if (Item->Attribute >= 0x08) { goto quit; }
bge  quit               @ quit

ldrb r2, [r0,#0x1c]     @ Item->WeaponLevel

mov  r0, r5             @ Unit->WeaponLevel + Item->Attribute
add  r0, #0x28
add  r0, r1

strb r2,[r0]            @ StoreLevel

quit:
ldr  r3,=0x0807AFC0+1
bx   r3
