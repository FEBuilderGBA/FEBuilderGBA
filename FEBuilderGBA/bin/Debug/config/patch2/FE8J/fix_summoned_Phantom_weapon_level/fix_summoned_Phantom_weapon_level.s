@
@Change the weapon level rules of the summoned Phantom
@Allocate weapon levels you own, not the summons level.
@
@召喚された亡霊戦士の武器レベルの規則を変更します。
@召喚者のレベルではなく、保有する武器レベルを割り振るようにします。
@

.thumb
.equ origin, 0x0807D296 @ FE8J
ldrb r0, [r4,#0x1E]     @ Get Weapon ID
BL 0x08017558           @ GetROMItemStructPtr FE8U
ldrb r1, [r0,#0x7]      @ Item->Attribute
cmp  r1, #0x08          @ if (Item->Attribute >= 0x08) { goto quit; }
bge  0x0807D2E8         @ quit

ldrb r2, [r0,#0x1c]     @ Item->WeaponLevel

mov  r0, r4             @ Unit->WeaponLevel + Item->Attribute
add  r0, #0x28
add  r0, r1

strb r2,[r0]            @ StoreLevel
b    0x0807D2E8         @ quit
