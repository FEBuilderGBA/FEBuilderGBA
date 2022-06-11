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
        public ToolROMRebuildFreeArea(uint freeAreaMinimumSize, uint freeAreaStartAddress, string appendFreeAreaFilename)
        {
            this.FreeAreaMinimumSize = freeAreaMinimumSize;
            this.FreeAreaStartAddress = freeAreaStartAddress;
            this.AppendFreeAreaFilename = appendFreeAreaFilename;
        }

        uint FreeAreaMinimumSize = 2048;
        uint FreeAreaStartAddress = 0x1000000;
        string AppendFreeAreaFilename = "";
        uint FreeAreaPadding = 0;

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

            Dictionary<uint, bool> knownDic = AsmMapFile.MakeKnownListToDic(knownList);
            MakeFreeDataList(RecycleFreeAreaList, knownDic, FreeAreaMinimumSize + FreeAreaPadding + FreeAreaPadding, data, RebuildAddress, useMap);

            for (int i = 0; i < this.RecycleFreeAreaList.Count; )
            {
                Address p = this.RecycleFreeAreaList[i];

                //頭としっぽはくれてやれ
                if (p.Length <= FreeAreaPadding*2)
                {
                    this.RecycleFreeAreaList.RemoveAt(i);
                    continue;
                }
                p.ResizeAddress(p.Addr + FreeAreaPadding, p.Length - FreeAreaPadding - FreeAreaPadding);
                i++;
            }

            AppendFreeArea(RebuildAddress, this.AppendFreeAreaFilename);

            RecycleFreeAreaList.Sort((a, b) => { return ((int)a.Length) - ((int)b.Length); });
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

        void AppendFreeArea(uint RebuildAddress,string appendFreeAreaFilename)
        {
            if (!File.Exists(appendFreeAreaFilename))
            {
                return ;
            }
            try
            {
                using (StreamReader reader = File.OpenText(appendFreeAreaFilename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.IndexOf("//FREEAREA:") < 0)
                        {
                            continue;
                        }

                        string[] sp = line.Split(' ');
                        if (sp.Length < 5)
                        {
                            continue;
                        }
                        uint start = U.atoh(sp[0]);
                        uint end = U.atoh(sp[2]);
                        AppendCustomFreeList(RecycleFreeAreaList,RebuildAddress, start, end);
                    }
                }
            }
            catch (Exception)
            {
                return;
            }

        }
        
        void AppendCustomFreeList(List<Address> list,uint RebuildAddress, uint start, uint end)
        {
            start = U.toOffset(start);
            if (!U.isPadding4(start))
            {//4で割り切れないの補正
                start = U.SubPadding4(start) + 4;
            }

            end = U.toOffset(end);
            end = U.SubPadding4(end);
            if (start <= 0x100 || end <= 0x100 || end <= start)
            {//アドレスが変
                return;
            }
            if (start >= RebuildAddress || end >= RebuildAddress)
            {//リビルドアドレスより上なのて無視
                return;
            }
            if (end - start >= FreeAreaMinimumSize)
            {//自動探索する領域よりも大きいから無視しておいた方が無難
                return;
            }

            uint d = Program.ROM.u32(start);
            if (!(d == 0x0 || d == 0xFFFFFFFF))
            {//空いてない
                return;
            }

            d = Program.ROM.u32(end - 4);
            if (!(d == 0x0 || d == 0xFFFFFFFF))
            {//空いてない
                //終端が割り込まれるのはよくあることなので4バイト削る
                end -= 4;
                if (end <= start + 4)
                {//小さすぎる
                    return;
                }
                d = Program.ROM.u32(end - 4);
                if (!(d == 0x0 || d == 0xFFFFFFFF))
                {//それでも空いてない
                    //もう一回削る
                    end -= 4;
                    if (end <= start + 4)
                    {//小さすぎる
                        return;
                    }
                    d = Program.ROM.u32(end - 4);
                    if (!(d == 0x0 || d == 0xFFFFFFFF))
                    {//それでも空いてないなら無理
                        return;
                    }
                }
            }
            if (end - start <= 0xF)
            {//あまりに小さすぎる
                return;
            }
            
            foreach (Address a in list)
            {
                uint AddrEnd = a.Addr + a.Length;
                if (a.Addr >= start && AddrEnd <= start && AddrEnd <= end)
                {//既に知ってる
                    return;
                }
            }

            if (InputFormRef.DoEvents(null, "AppendCustomFreeList " + U.ToHexString(start))) return;
            FEBuilderGBA.Address.AddAddress(list
                , start
                , end - start
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
