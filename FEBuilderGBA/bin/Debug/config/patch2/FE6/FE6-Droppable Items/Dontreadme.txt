Assemble FE6-Droppable Items EA.txt with Event Assembler.
This hack makes droppable items a thing in FE6.
Enemies that steal items or loot chests will have their droppable flag (0x1000 in 0xC of the ram unit pointer) set.
You can set it in your UNIT data by setting 0x40 in AI byte 4. So Guard tile + drop last item would be 0x40+0x20 = 0x60. This is identical to a hack made by Vennobennu for FE7.
If you find any issues, contact Tequila on FEU.