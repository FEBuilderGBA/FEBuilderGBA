@Hook 0803182c	@{J}
@Hook 080318E0	@{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {r4, r5, r6, r7, lr}
sub sp, #0x8

mov r4 ,r0             @EnemyClass
add r4, #0x2c          @EnemyClass->WeaponLevel

ldr r5, =0x0203A8EC    @ArenaData@闘技場の一時データ	@{J}
@ldr r5, =0x0203A8F0    @ArenaData@闘技場の一時データ	@{U}
ldrb r5, [r5, #0xd]    @ArenaData@闘技場の一時データ.playerWpnType

mov r6, sp
mov r1, #0x00
str r1, [r6, #0x0]
str r1, [r6, #0x4]

mov r7, #0x0  @MaxCount

mov r3, #0x0

Loop:
cmp r3, #0x8
bge Break

cmp  r3, #0x4   @4=Staff
beq  Continue   @Skip Staff Level

ldrb r0, [r4, r3]
cmp  r0, #0x0
beq  Continue   @Can not Use the Weapon

@プレイヤーの武器に反撃できる武器を探す
cmp  r5, #0x2   @0=Sword 1=Lance 2=Axs
ble  MeleeAttack

cmp  r5, #0x3   @3=Bow
beq  BowAttack

MagicAttack:
b StoreWeapon

MeleeAttack:
cmp  r3, #0x3   @3=Bow
beq  Continue   @Skip Bow  剣、槍、斧は、弓に反撃できぬ
b    StoreWeapon

BowAttack:
cmp  r3, #0x2   @2=Axs
ble  Continue   @Skip Melee  弓には剣、槍、斧は反撃できぬ
b    StoreWeapon

StoreWeapon:
strb r3, [r6, r7]
add  r7, #0x01

Continue:
add r3, #0x01
b   Loop

Break:

@利用可能な武器リストから、ランダム選出
mov r0 ,r7
blh 0x08000c58   @NextRN_N	@{J}
@blh 0x08000c80   @NextRN_N	@{U}

ldrb r0, [r6, r0]

add sp, #0x8
pop {r4, r5, r6, r7}
pop {r1}
bx r1
