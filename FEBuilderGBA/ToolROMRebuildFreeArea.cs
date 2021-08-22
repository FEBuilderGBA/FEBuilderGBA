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
        public ToolROMRebuildFreeArea(uint freeAreaMinimumSize, uint freeAreaStartAddress )
        {
            uint FreeAreaMinimumSize = freeAreaMinimumSize;
            uint FreeAreaStartAddress = freeAreaStartAddress;
        }

        uint FreeAreaMinimumSize;
        uint FreeAreaStartAddress;

        List<Address> RecycleFreeAreaList = new List<Address>();
        public void MakeFreeAreaList(byte[] data, uint RebuildAddress, Dictionary<uint, uint> useMap)
        {
            List<Address> knownList = U.MakeAllStructPointersList(false);
            List<DisassemblerTrumb.LDRPointer> ldrmap = Program.AsmMapFileAsmCache.GetLDRMapCache();
            U.AppendAllASMStructPointersList(knownList
                , ldrmap
                , isPatchInstallOnly: true
                , isPatchPointerOnly: false
                , isPatchStructOnly: false
                , isUseOtherGraphics: true
                , isUseOAMSP: true
                );
            MoveToFreeSapceForm.AppendSkillSystemsSanctuary(knownList);

            Dictionary<uint, bool> knownDic = MakeKnownListToDic(knownList);
            MakeFreeDataList(RecycleFreeAreaList, knownDic, FreeAreaMinimumSize + 16 + 16, data, RebuildAddress, useMap);

            for (int i = 0; i < this.RecycleFreeAreaList.Count; )
            {
                Address p = this.RecycleFreeAreaList[i];

                //頭としっぽはくれてやれ
                if (p.Length < 64)
                {
                    this.RecycleFreeAreaList.RemoveAt(i);
                    continue;
                }
                p.ResizeAddress(p.Addr + 16, p.Length - 16 - 16);
                i++;
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

                addr = U.Padding4(addr);
                for (uint i = 0; i < a.Length; i += 4)
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
            uint addr = FreeAreaStartAddress;
            for (; addr < length; addr += 4)
            {
                byte filldata;
                if (!(data[addr] == 0x00 || data[addr] == 0xFF))
                {
                    continue;
                }
                if (useMap.ContainsKey(addr)
                    || knownDic.ContainsKey(addr))
                {
                    continue;
                }
                uint checkData = Program.ROM.u32(addr);
                if (!(checkData == 0x00 || checkData == 0xFFFFFFFF))
                {
                    continue;
                }

                filldata = data[addr];

                uint start = addr;
                addr++;
                for (; addr < length ; addr++)
                {
                    uint a = data[addr];
                    bool foundUseMap = useMap.ContainsKey(addr);
                    bool foundKnownDic = knownDic.ContainsKey(addr);
                    if (a != filldata
                        || foundUseMap
                        || foundKnownDic)
                    {
                        uint matchsize = addr - start;
                        if (matchsize >= needSize)
                        {
                            Log.Debug("MakeFreeDataList" , U.ToHexString(start) , matchsize.ToString());
                            AppendList(list, start, matchsize,data);
                        }
                        break;
                    }
                }

                addr = U.Padding4(addr);
            }
        }
        void AppendList(List<Address> list ,uint start,uint matchsize,byte[] data)
        {
            uint checkData = U.u32(data,start);
            if (!(checkData == 0x00 || checkData == 0xFFFFFFFF))
            {
                Debug.Assert(false);
                return;
            }

            if (InputFormRef.DoEvents(null, "MakeFreeDataList " + U.ToHexString(start))) return;
            FEBuilderGBA.Address.AddAddress(list
                , start
                , matchsize
                , U.NOT_FOUND
                , ""
                , Address.DataTypeEnum.FFor00);
        }

        
        
        //空き領域から割り当てができるか?
        public uint CanAllocFreeArea(uint needSize, uint current_addr)
        {
            //4の倍数に格納する
            needSize = U.Padding4(needSize);

            for (int i = 0; i < this.RecycleFreeAreaList.Count; i++)
            {
                Address p = this.RecycleFreeAreaList[i];
                if (p.Length < needSize)
                {
                    continue;
                }

                uint use_addr = U.Padding4(p.Addr);
                if (current_addr < use_addr)
                {//非拡張領域を処理しているときに、
                 //現在処理している領域より先の領域を割り当ててはいけない。
                 //あとあと、上書きされるかもしれないので
                    continue;
                }

                uint endaddr = U.Padding4(use_addr + needSize);
                uint length = U.Sub(p.Length, (endaddr - use_addr));

                p.ResizeAddress(endaddr, length);
                if (p.Length < 4)
                {//もう空きがない.
                    this.RecycleFreeAreaList.RemoveAt(i);
                }

                return use_addr;
            }
            //割当不可能
            return U.NOT_FOUND;
        }
    }
}
