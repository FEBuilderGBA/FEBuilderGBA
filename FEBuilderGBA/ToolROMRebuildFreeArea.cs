using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Reflection;

namespace FEBuilderGBA
{
    //リビルドしない領域にあるフリーエリア 再利用する場合に利用する.
    class ToolROMRebuildFreeArea
    {
        public ToolROMRebuildFreeArea( )
        {
        }

        const int FREEAREA_BLOCK_SIZE = 1024;

        List<Address> RecycleFreeAreaList = new List<Address>();
        public void MakeFreeAreaList(byte[] data, uint RebuildAddress, Dictionary<uint, uint> useMap)
        {
            List<Address> knownList = U.MakeAllStructPointersList(false);
            List<DisassemblerTrumb.LDRPointer> ldrmap = DisassemblerTrumb.MakeLDRMap(Program.ROM.Data, 0x100);
            U.AppendAllASMStructPointersList(knownList
                , ldrmap
                , isPatchInstallOnly: true
                , isPatchPointerOnly: false
                , isPatchStructOnly: false
                , isPatchStoreSymbol: true
                , isUseOtherGraphics: true
                , isUseOAMSP: true
                );

            Dictionary<uint, bool> knownDic = MakeKnownListToDic(knownList);
            MakeFreeDataList(RecycleFreeAreaList, knownDic, FREEAREA_BLOCK_SIZE+16+16, data, RebuildAddress, useMap);

            for (int i = 0; i < this.RecycleFreeAreaList.Count; i++)
            {
                Address p = this.RecycleFreeAreaList[i];

                //頭としっぽはくれてやれ
                if (p.Length < 32)
                {
                    p.ResizeAddress(p.Addr, 0);
                }
                else
                {
                    p.ResizeAddress(p.Addr + 16, p.Length - 16 - 16);
                }

            }
        }
        Dictionary<uint,bool> MakeKnownListToDic(List<Address> knownList)
        {
            Dictionary<uint,bool> ret = new Dictionary<uint,bool>();
            foreach(Address a in knownList)
            {
                if (a.DataType == Address.DataTypeEnum.FFor00)
                {
                    continue;
                }

                uint addr = U.toOffset(a.Addr);

                ret[addr] = true;
                for (uint i = 0; i < a.Length; i += (FREEAREA_BLOCK_SIZE / 2))
                {
                    ret[addr + i] = true;
                }

                if (a.Pointer != U.NOT_FOUND)
                {
                    addr = U.toOffset(a.Pointer);
                    ret[addr + 0] = true;
                    ret[addr + 1] = true;
                    ret[addr + 2] = true;
                    ret[addr + 3] = true;
                }

            }
            return ret;
        }

        //フリー領域と思われる部分を検出.
        void MakeFreeDataList(List<Address> list
            , Dictionary<uint, bool> knownDic
            , uint needSize, byte[] data
            , uint length, Dictionary<uint, uint> useMap)
        {
            uint addr = U.Padding4(Program.ROM.RomInfo.compress_image_borderline_address());
            for (; addr < length; addr += 4)
            {
                byte filldata;
                if (data[addr] == 0x00 || data[addr] == 0xFF)
                {
                    if (useMap.ContainsKey(addr)
                        || knownDic.ContainsKey(addr))
                    {
                        continue;
                    }
                    filldata = data[addr];

                    uint start = addr;
                    addr++;
                    for (; ; addr++)
                    {
                        if (addr >= length)
                        {
                            uint matchsize = addr - start;
                            if (matchsize >= needSize)
                            {
                                if (InputFormRef.DoEvents(null, "MakeFreeDataList " + U.ToHexString(addr))) return;
                                FEBuilderGBA.Address.AddAddress(list
                                    , start
                                    , matchsize
                                    , U.NOT_FOUND
                                    , ""
                                    , Address.DataTypeEnum.FFor00);
                            }
                            break;
                        }
                        if (data[addr] != filldata
                            ||  useMap.ContainsKey(addr)
                            ||  knownDic.ContainsKey(addr))
                        {
                            uint matchsize = addr - start;
                            if (matchsize >= needSize)
                            {
                                if (InputFormRef.DoEvents(null, "MakeFreeDataList " + U.ToHexString(addr))) return;
                                FEBuilderGBA.Address.AddAddress(list
                                    , start
                                    , matchsize
                                    , U.NOT_FOUND
                                    , ""
                                    , Address.DataTypeEnum.FFor00);
                            }
                            break;
                        }
                    }

                    addr = U.Padding4(addr);
                }
            }
        }

        
        
        //空き領域から割り当てができるか?
        public uint CanAllocFreeArea(uint needSize)
        {
            //4の倍数に格納する
            needSize = U.Padding4(needSize);

            for (int i = 0; i < this.RecycleFreeAreaList.Count; i++)
            {
                Address p = this.RecycleFreeAreaList[i];
                if (p.Length >= needSize)
                {
                    uint use_addr = U.Padding4(p.Addr);

                    uint endaddr = U.Padding4(use_addr + needSize);
                    uint length = U.Sub(p.Length, (endaddr - use_addr));

                    p.ResizeAddress(endaddr, length);
                    if (p.Length < 4)
                    {//もう空きがない.
                        this.RecycleFreeAreaList.RemoveAt(i);
                    }

                    return use_addr;
                }
            }
            //割当不可能
            return U.NOT_FOUND;
        }
    }
}
