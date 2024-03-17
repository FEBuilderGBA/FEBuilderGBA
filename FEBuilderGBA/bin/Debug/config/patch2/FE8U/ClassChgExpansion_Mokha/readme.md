# ClassChg List Expansion

- Date 2023.08.05
- Author Mokha

Expand selectable classes during branched promotion to **SIX**

<div align=center>

![image](gfx/ClassChgExpansion.png)

<div align=left>

## Custom Usage

### Vanila table
Vanilla table of 2 selectable classes has been retained. You can also config on old table to diy branched promotion classes.

## Expansion
At the same time, there are two list for promotion expansion:

1. `gPromoJidLutExpa`

    A `0x100 * 4` linear table according to class index.
    You can config 4 additional class to promote.

    You can also config this table by FEBuilder's patch, **./patches/PATCH_ClassChgExpandList.txt**

2. `gClassChgExpaMods`

    A table for promotion with higher degrees of freedom. A unit can promote to `ClassChgExpaMod::jid_promo` if:

    - The configed class index is matched
    - The configed character index is matched
    - The configed item is used for promotion
    - The configed event flag is set

    You can config this table by FEBuilder's patch, **./patches/PATCH_ClassChgExpandModulearList.txt**

### Trainees
As for auto-promotion for trainee class, there is also a reworked table for configuration. `gpTraineesRe`, with two element `jid` and `level`.

    You can also config this table by FEBuilder's patch, **./patches/PATCH_ClassChgExpandTrainee.txt**

## Custom Build

If you want to modify on source code, please refer to [CHAX template](https://github.com/MokhaLeee/fe8-chax-template), note that the C-Lib use the [ver.2023.07.31](https://github.com/MokhaLeee/FE-CLib-Mokha/releases/tag/3.0).

## Wizardry notes

You can also write your own jid-getter function and add the function to the list `gGetClasschgListFuncs`.

```c
/**
 * r0: unit struct
 * r1: protion item
 * r2: out buffer
 * r3: length of the out-buffer, DO NOT overflow!
 *
 * return: amount of the classes to promote
 */
typedef int (* GetClasschgListFunc_t)(struct Unit * unit, u16 item, u8 * out, int len);
extern const GetClasschgListFunc_t gGetClasschgListFuncs[];
```
