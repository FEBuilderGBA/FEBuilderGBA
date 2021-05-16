using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FEBuilderGBA
{
    class RAM : IDisposable
    {
        //http://www.codingvision.net/security/c-how-to-scan-a-process-memory
        // REQUIRED CONSTS
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int MEM_COMMIT  = 0x00001000;
        const int MEM_PRIVATE = 0x00020000;
        const int PAGE_READWRITE = 0x04;
        const int PROCESS_VM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008; //書き込むには、これも必要
        const int PROCESS_ACCESS_ALL = 0x001F0FFF;



        // REQUIRED METHODS
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        static extern unsafe bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, uint lpBaseAddress, byte[] lpBuffer, uint dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, uint lpBaseAddress, byte[] lpBuffer, uint dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        // REQUIRED STRUCTS
        public struct MEMORY_BASIC_INFORMATION
        {
            public uint BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public uint RegionSize;
            public int State;
            public int Protect;
            public int lType;
        }

        public struct SYSTEM_INFO
        {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }

        Process Process;
        IntPtr ProcessHandle;
        public byte[] Memory02 { get; private set; }
        MEMORY_BASIC_INFORMATION MemBasicInfo02;
        uint AppnedOffset02;

        public byte[] Memory03 { get; private set; }
        MEMORY_BASIC_INFORMATION MemBasicInfo03;
        uint AppnedOffset03;


        public void Dispose()
        {
            DisConnect();
        }

        public uint u32(uint pointer)
        {
            uint addr = pointer;
            if (U.is_02RAMPointer(addr))
            {
                addr = addr - 0x02000000 + this.AppnedOffset02;
                if (addr + 4 > this.Memory02.Length)
                {
                    return 0;
                }
                return U.u32(this.Memory02, addr);
            }
            if (U.is_03RAMPointer(addr))
            {
                addr = addr - 0x03000000 + this.AppnedOffset03;
                if (addr + 4 > this.Memory03.Length)
                {
                    return 0;
                }
                return U.u32(this.Memory03, addr);
            }
            Debug.Assert(false);
            return 0;
        }
        public uint u16(uint pointer)
        {
            uint addr = pointer;
            if (U.is_02RAMPointer(addr))
            {
                addr = addr - 0x02000000 + this.AppnedOffset02;
                if (addr + 2 > this.Memory02.Length)
                {
                    return 0;
                }
                return U.u16(this.Memory02, addr);
            }
            if (U.is_03RAMPointer(addr))
            {
                addr = addr - 0x03000000 + this.AppnedOffset03;
                if (addr + 2 > this.Memory03.Length)
                {
                    return 0;
                }
                return U.u16(this.Memory03, addr);
            }
            Debug.Assert(false);
            return 0;
        }
        public uint u8(uint pointer)
        {
            uint addr = pointer;
            if (U.is_02RAMPointer(addr))
            {
                addr = addr - 0x02000000 + this.AppnedOffset02;
                if (addr + 1 > this.Memory02.Length)
                {
                    return 0;
                }
                return U.u8(this.Memory02, addr);
            }
            if (U.is_03RAMPointer(addr))
            {
                addr = addr - 0x03000000 + this.AppnedOffset03;
                if (addr + 1 > this.Memory03.Length)
                {
                    return 0;
                }
                return U.u8(this.Memory03, addr);
            }
            Debug.Assert(false);
            return 0;
        }
        public uint u4(uint pointer, bool isHigh)
        {
            uint addr = pointer;
            if (U.is_02RAMPointer(addr))
            {
                addr = addr - 0x02000000 + this.AppnedOffset02;
                if (addr + 1 > this.Memory02.Length)
                {
                    return 0;
                }
                return U.u4(this.Memory02, addr, isHigh);
            }
            if (U.is_03RAMPointer(addr))
            {
                addr = addr - 0x03000000 + this.AppnedOffset03;
                if (addr + 1 > this.Memory03.Length)
                {
                    return 0;
                }
                return U.u4(this.Memory03, addr, isHigh);
            }
            Debug.Assert(false);
            return 0;
        }
        public byte[] getBinaryData(uint pointer, int count)
        {
            if (count < 0)
            {
                R.Error("RAM.getBinaryData pointer:{0} count:{1}" , U.To0xHexString(pointer), count );
                return new byte[0];
            }
            return getBinaryData(pointer,(uint)count);
        }

        public byte[] getBinaryData(uint pointer, uint count)
        {
            uint addr = pointer;
            if (U.is_02RAMPointer(addr))
            {
                addr = addr - 0x02000000 + this.AppnedOffset02;
                if (addr + 4 > this.Memory02.Length)
                {
                    return new byte[count];
                }
                return U.getBinaryData(this.Memory02, addr, count);
            }
            if (U.is_03RAMPointer(addr))
            {
                addr = addr - 0x03000000 + this.AppnedOffset03;
                if (addr + 4 > this.Memory03.Length)
                {
                    return new byte[count];
                }
                return U.getBinaryData(this.Memory03, addr, count);
            }
            if (addr == 0 || count == 0)
            {
                return new byte[0];
            }
            Debug.Assert(false);
            return new byte[count];
        }
        public uint strlen(uint addr)
        {
            return getBlockDataCount(addr, 1, (i, p , mem) =>
            {
                return (mem[p] != '\0');
            });
        }

        public uint getBlockDataCount(uint addr, uint blocksize, Func<int, uint,byte[], bool> is_data_exists_callback)
        {
            if (addr == 0 || addr == U.NOT_FOUND)
            {
                return 0;
            }

            if (U.is_02RAMPointer(addr))
            {
                addr = addr - 0x02000000 + this.AppnedOffset02;
                if (addr + 4 > this.Memory02.Length)
                {
                    return 0;
                }
                uint length = (uint)this.Memory02.Length;
                int i = 0;
                while (addr + blocksize < length)
                {
                    if (!is_data_exists_callback(i, addr,this.Memory02))
                    {
                        return (uint)i;
                    }
                    addr += blocksize;
                    i++;
                }
                return (uint)i;
            }
            if (U.is_03RAMPointer(addr))
            {
                addr = addr - 0x03000000 + this.AppnedOffset03;
                if (addr + 4 > this.Memory03.Length)
                {
                    return 0;
                }
                uint length = (uint)this.Memory03.Length;
                int i = 0;
                while (addr + blocksize < length)
                {
                    if (!is_data_exists_callback(i, addr, this.Memory03))
                    {
                        return (uint)i;
                    }
                    addr += blocksize;
                    i++;
                }
                return (uint)i;
            }

            //            R.Error("警告:データが途中で終わってしまいました。 addr:{0} data.Length:{1} countI:{2}", U.ToHexString(addr), U.ToHexString(length), i);
            //            Debug.Assert(false);
            return 0;
        }
        public uint Grep0x02(byte[] need, uint blocksize)
        {
            uint addr = U.Grep(this.Memory02, need, this.AppnedOffset02, 0, blocksize);
            if (addr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            addr = addr + 0x02000000 - this.AppnedOffset02;
            return addr;
        }
        public uint Grep0x03(byte[] need, uint blocksize)
        {
            uint addr = U.Grep(this.Memory03, need, this.AppnedOffset03, 0, blocksize);
            if (addr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            addr = addr + 0x03000000 - this.AppnedOffset03;
            return addr;
        }
        public uint GrepPatternMatch0x02(byte[] need, bool[] mask, uint blocksize = 4)
        {
            uint addr = U.GrepPatternMatch(this.Memory02, need,mask, this.AppnedOffset02, 0, blocksize);
            if (addr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            addr = addr + 0x02000000 - this.AppnedOffset02;
            return addr;
        }
        public uint GrepPatternMatch0x03(byte[] need, bool[] mask, uint blocksize = 4)
        {
            uint addr = U.GrepPatternMatch(this.Memory03, need, mask, this.AppnedOffset03, 0, blocksize);
            if (addr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            addr = addr + 0x03000000 - this.AppnedOffset03;
            return addr;
        }

        public void write_u32(uint pointer, uint data)
        {
            uint addr = pointer;
            if (U.is_02RAMPointer(addr))
            {
                addr = addr - 0x02000000 + this.AppnedOffset02;
                if (addr + 4 > this.Memory02.Length)
                {
                    return ;
                }
                U.write_u32(this.Memory02, addr , data);
                WriteMemory02(addr, data, 4);
                return;
            }
            if (U.is_03RAMPointer(addr))
            {
                addr = addr - 0x03000000 + this.AppnedOffset03;
                if (addr + 4 > this.Memory03.Length)
                {
                    return ;
                }
                U.write_u32(this.Memory03, addr, data);
                WriteMemory03(addr, data, 4);
                return;
            }
            Debug.Assert(false);
            return ;
        }
        public void write_u16(uint pointer,uint data)
        {
            uint addr = pointer;
            if (U.is_02RAMPointer(addr))
            {
                addr = addr - 0x02000000 + this.AppnedOffset02;
                if (addr + 2 > this.Memory02.Length)
                {
                    return ;
                }
                U.write_u16(this.Memory02, addr, data);
                WriteMemory02(addr, data, 2);
                return;
            }
            if (U.is_03RAMPointer(addr))
            {
                addr = addr - 0x03000000 + this.AppnedOffset03;
                if (addr + 2 > this.Memory03.Length)
                {
                    return ;
                }
                U.write_u16(this.Memory03, addr, data);
                WriteMemory03(addr, data, 2);
                return;
            }
            Debug.Assert(false);
            return ;
        }
        public void write_u8(uint pointer,uint data)
        {
            uint addr = pointer;
            if (U.is_02RAMPointer(addr))
            {
                addr = addr - 0x02000000 + this.AppnedOffset02;
                if (addr + 1 > this.Memory02.Length)
                {
                    return ;
                }
                U.write_u8(this.Memory02, addr, data);
                WriteMemory02(addr, data, 1);
                return;
            }
            if (U.is_03RAMPointer(addr))
            {
                addr = addr - 0x03000000 + this.AppnedOffset03;
                if (addr + 1 > this.Memory03.Length)
                {
                    return ;
                }
                U.write_u8(this.Memory03, addr, data);
                WriteMemory03(addr, data, 1);
                return;
            }
            Debug.Assert(false);
            return;
        }
        public void write_u4(uint pointer, uint data, bool isHigh)
        {
            uint addr = pointer;
            if (U.is_02RAMPointer(addr))
            {
                addr = addr - 0x02000000 + this.AppnedOffset02;
                if (addr + 1 > this.Memory02.Length)
                {
                    return;
                }
                U.write_u4(this.Memory02, addr, data, isHigh);
                WriteMemory02(addr, data, 1);
                return;
            }
            if (U.is_03RAMPointer(addr))
            {
                addr = addr - 0x03000000 + this.AppnedOffset03;
                if (addr + 1 > this.Memory03.Length)
                {
                    return;
                }
                U.write_u4(this.Memory03, addr, data, isHigh);
                WriteMemory03(addr, data, 1);
                return;
            }
            Debug.Assert(false);
            return;
        }
        public void write_range(uint addr, byte[] write_data)
        {
            if (U.is_02RAMPointer(addr))
            {
                addr = addr - 0x02000000 + this.AppnedOffset02;
                if (addr + 1 > this.Memory02.Length)
                {
                    return;
                }
                U.write_range(this.Memory02, addr, write_data);
                WriteMemory02(addr, write_data, (uint)write_data.Length);
                return;
            }
            if (U.is_03RAMPointer(addr))
            {
                addr = addr - 0x03000000 + this.AppnedOffset03;
                if (addr + 1 > this.Memory03.Length)
                {
                    return;
                }
                U.write_range(this.Memory02, addr, write_data);
                WriteMemory03(addr, write_data, (uint)write_data.Length);
                return;
            }
        }


        public void DisConnect()
        {
            if (ProcessHandle != IntPtr.Zero)
            {
                CloseHandle(ProcessHandle);
                ProcessHandle = IntPtr.Zero;
                MemBasicInfo02.RegionSize = 0;
                MemBasicInfo03.RegionSize = 0;
            }
        }

        public bool IsConnect()
        {
            if (ProcessHandle == IntPtr.Zero)
            {
                return false;
            }
            if (MemBasicInfo02.RegionSize <= 0)
            {
                return false;
            }
            if (MemBasicInfo03.RegionSize <= 0)
            {
                return false;
            }
            if (Memory02.Length <= 0)
            {
                return false;
            }
            if (Memory03.Length <= 0)
            {
                return false;
            }
            return true;
        }

        bool Connect()
        {
            DisConnect();

            string romname = U.MakeFilename("emulator");
            Process process = Program.UpdateWatcher.GetRunning(romname);
            if (process == null)
            {
                this.ErrorMessage = R._("エミュレータが動作していません。");
                return false;
            }
            this.Process = process;
            try
            {
                return SearchMemory();
            }
            catch (OverflowException)
            {
//                R.Error(R.ExceptionToString(e));
                this.ErrorMessage = R._("OverflowException! 32bitバージョンのエミュレータを利用していますか?");
                return false;
            }
            catch (Exception e)
            {
                R.Error(R.ExceptionToString(e));
                this.ErrorMessage = e.ToString();
                return false;
            }
        }

        bool SearchMemory()
        {
            // getting minimum & maximum address
            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            IntPtr PROCS_min_address = sys_info.minimumApplicationAddress;
            IntPtr PROCS_max_address = sys_info.maximumApplicationAddress;

            // saving the values as long ints so I won't have to do a lot of casts later
            ulong PROCS_min_address_l = (ulong)PROCS_min_address;
            ulong PROCS_max_address_l = (ulong)PROCS_max_address;

            // opening the process with desired access level
            this.ProcessHandle = OpenProcess(
                  PROCESS_QUERY_INFORMATION  //メモリ一覧の取得権限
                | PROCESS_VM_READ            //読込権限
                | PROCESS_VM_WRITE | PROCESS_VM_OPERATION //書き込み権限
                , false, this.Process.Id);
            if (this.ProcessHandle == IntPtr.Zero)
            {
                this.ErrorMessage = R._("OpenProcess APIが失敗ました。");
                return false;
            }

            // this will store any information we get from VirtualQueryEx()
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();

            while (PROCS_min_address_l < PROCS_max_address_l)
            {
                // 28 = sizeof(MEMORY_BASIC_INFORMATION)
                int ret = VirtualQueryEx(this.ProcessHandle, PROCS_min_address, out mem_basic_info, 28);
                if (ret <= 0)
                {
                    break;
                }

                // if this memory chunk is accessible
                if (mem_basic_info.Protect == PAGE_READWRITE
                    && mem_basic_info.State == MEM_COMMIT
                    && mem_basic_info.lType == MEM_PRIVATE
                    )
                {
                    if (this.MemBasicInfo02.RegionSize <= 0)
                    {
                        Parse02(this.ProcessHandle, mem_basic_info);
                    }
                    if (this.MemBasicInfo03.RegionSize <= 0)
                    {
                        Parse03(this.ProcessHandle, mem_basic_info);
                    }
                }

                // move to the next memory chunk
                PROCS_min_address_l += (ulong)mem_basic_info.RegionSize;
                PROCS_min_address = (IntPtr)PROCS_min_address_l;
            }

            if (! IsConnect())
            {
                CloseHandle(ProcessHandle);
                ProcessHandle = IntPtr.Zero;
                if (MemBasicInfo02.RegionSize <= 0 || Memory02.Length <= 0)
                {
                    if (MemBasicInfo03.RegionSize <= 0 || Memory03.Length <= 0)
                    {
                        this.ErrorMessage = R._("読込むメモリを特定できませんでした。(Page02,Page03)");
                    }
                    else
                    {
                        this.ErrorMessage = R._("読込むメモリを特定できませんでした。(Page02)");
                    }
                }
                else
                {
                    this.ErrorMessage = R._("読込むメモリを特定できませんでした。(Page03)");
                }

#if DEBUG
//                DebugDumpMemory();
#endif
                return false;
            }
            return true;
        }

#if DEBUG
        void DebugDumpMemory()
        {
            // getting minimum & maximum address
            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            IntPtr PROCS_min_address = sys_info.minimumApplicationAddress;
            IntPtr PROCS_max_address = sys_info.maximumApplicationAddress;

            // saving the values as long ints so I won't have to do a lot of casts later
            ulong PROCS_min_address_l = (ulong)PROCS_min_address;
            ulong PROCS_max_address_l = (ulong)PROCS_max_address;

            // opening the process with desired access level
            IntPtr processHandle = OpenProcess(
                  PROCESS_QUERY_INFORMATION  //メモリ一覧の取得権限
                | PROCESS_VM_READ            //読込権限
                | PROCESS_VM_WRITE | PROCESS_VM_OPERATION //書き込み権限
                , false, this.Process.Id);
            if (processHandle == IntPtr.Zero)
            {
                return ;
            }

            // this will store any information we get from VirtualQueryEx()
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();

            while (PROCS_min_address_l < PROCS_max_address_l)
            {
                // 28 = sizeof(MEMORY_BASIC_INFORMATION)
                int ret = VirtualQueryEx(processHandle, PROCS_min_address, out mem_basic_info, 28);
                if (ret <= 0)
                {
                    break;
                }

                // if this memory chunk is accessible
                if (mem_basic_info.Protect == PAGE_READWRITE
                    && mem_basic_info.State == MEM_COMMIT
                    && mem_basic_info.lType == MEM_PRIVATE
                    )
                {
                    DebugDump(processHandle, mem_basic_info);
                }

                // move to the next memory chunk
                PROCS_min_address_l += (ulong)mem_basic_info.RegionSize;
                PROCS_min_address = (IntPtr)PROCS_min_address_l;
            }
            CloseHandle(processHandle);
            processHandle = IntPtr.Zero;
        }
        void DebugDump(IntPtr processHandle
            , MEMORY_BASIC_INFORMATION mem_basic_info
            )
        {
            int bytesRead = 0;  // number of bytes read with ReadProcessMemory
            byte[] buffer = new byte[mem_basic_info.RegionSize];
            ReadProcessMemory((int)processHandle, mem_basic_info.BaseAddress, buffer, mem_basic_info.RegionSize, ref bytesRead);

            U.WriteAllBytes("_debug_dump_"+U.ToHexString(mem_basic_info.BaseAddress) + ".bin", buffer);
        }
#endif

        const uint PAGE02_SIZE = 0x40000;
        const uint PAGE03_SIZE = 0x8000;

        const uint PAGE03_ADDRESS_OFFSET = 0x40000;
        const uint PAGE03_ADDRESS_END_OFFSET = 0x40000 + 0x8000;

        bool Parse02(IntPtr processHandle
            , MEMORY_BASIC_INFORMATION mem_basic_info
        )
        {
            if (mem_basic_info.RegionSize < PAGE02_SIZE)
            {
                return false;
            }

            int bytesRead = 0;  // number of bytes read with ReadProcessMemory
            byte[] buffer = new byte[mem_basic_info.RegionSize];
            ReadProcessMemory((int)processHandle, mem_basic_info.BaseAddress, buffer, mem_basic_info.RegionSize, ref bytesRead);
            if (bytesRead < PAGE02_SIZE)
            {
                return false;
            }

            uint procs_game_main = Program.ROM.RomInfo.workmemory_procs_game_main_address() - 0x02000000;
            uint rom_procs_game_main = Program.ROM.RomInfo.procs_game_main_address();

            uint procs_forest_address = Program.ROM.RomInfo.workmemory_procs_forest_address() - 0x02000000;
            
            uint append_offset = 0;

            uint limit = ((uint)bytesRead) - procs_game_main - (0x4);

            bool match = false;
            for (; append_offset < limit; append_offset += 4)
            {
                uint addr = append_offset + procs_game_main;
                if (addr + 3 >= buffer.Length)
                {
                    continue;
                }
                if (buffer[addr + 3] != 0x8)
                {
                    continue;
                }
                uint pointer = U.u32(buffer, addr);
                if (pointer != rom_procs_game_main)
                {
                    continue;
                }

                //FE6ではこれだけで誤判定するので追加の情報を付与する.
                addr = append_offset + procs_forest_address;
                if (addr+3 >= buffer.Length)
                {
                    continue;
                }
                pointer = U.u32(buffer, addr);
                if (!U.is_ROMorRAMPointerOrNULL(pointer))
                {
                    continue;
                }
                addr = append_offset + procs_forest_address + 4;
                if (addr+3 >= buffer.Length)
                {
                    continue;
                }
                pointer = U.u32(buffer, addr);
                if (!U.is_ROMorRAMPointerOrNULL(pointer))
                {
                    continue;
                }

                match = true;
                break;
            }
            if (match == false)
            {
                return false;
            }
            this.Memory02 = buffer;
            this.MemBasicInfo02 = mem_basic_info;
            this.AppnedOffset02 = append_offset;
            
            return true;
        }

        bool Parse03(IntPtr processHandle
            , MEMORY_BASIC_INFORMATION mem_basic_info
        )
        {
            if (mem_basic_info.RegionSize < PAGE03_SIZE)
            {
                return false;
            }

            int bytesRead = 0;  // number of bytes read with ReadProcessMemory
            byte[] buffer = new byte[mem_basic_info.RegionSize];
            ReadProcessMemory((int)processHandle, mem_basic_info.BaseAddress, buffer, mem_basic_info.RegionSize, ref bytesRead);
            if (bytesRead < PAGE03_SIZE)
            {
                return false;
            }

            uint user_stack_base = Program.ROM.RomInfo.workmemory_user_stack_base_address() - 0x03000000;
            uint function_fe_main_return_address = Program.ROM.RomInfo.function_fe_main_return_address();

            uint control_unit_address = Program.ROM.RomInfo.workmemory_control_unit_address() - 0x03000000;

            uint append_offset = 0;

            uint limit = ((uint)bytesRead) - user_stack_base - 4;
            bool match = false;

            for (; append_offset < limit; append_offset += 4)
            {
                uint addr = append_offset + user_stack_base;
                if (addr >= buffer.Length)
                {
                    continue;
                }
                if (buffer[addr + 3] != 0x8)
                {
                    continue;
                }
                uint main_function = U.u32(buffer, addr);
                if (main_function != function_fe_main_return_address)
                {
                    continue;
                }

                //誤判定をさけるためにプレイヤ操作ユニット領域を確認する
                addr = append_offset + control_unit_address;
                if (addr >= buffer.Length)
                {
                    continue;
                }
                uint pointer = U.u32(buffer, addr);
                if (!U.is_ROMorRAMPointerOrNULL(pointer))
                {
                    continue;
                }
                

                match = true;
                break;
            }
            if (match == false)
            {
                return false;
            }
            this.Memory03 = buffer;
            this.MemBasicInfo03 = mem_basic_info;
            this.AppnedOffset03 = append_offset;
            return true;
        }

        public bool UpdateMemory()
        {
            if (!this.IsConnect())
            {
                return this.Connect();
            }
            return ReadMemory02() && ReadMemory03();
        }
        bool ReadMemory02()
        {
            Debug.Assert(MemBasicInfo02.RegionSize > 0);

            int bytesRead = 0;  // number of bytes read with ReadProcessMemory
            byte[] buffer = new byte[MemBasicInfo02.RegionSize];
            ReadProcessMemory((int)this.ProcessHandle, MemBasicInfo02.BaseAddress, buffer, MemBasicInfo02.RegionSize, ref bytesRead);

            if (bytesRead <= 0 || bytesRead < MemBasicInfo02.RegionSize)
            {
                this.ErrorMessage = R._("Page02を読めませんでした。bytesRead:{0},MemBasicInfo02.RegionSize:{1},"
                    , bytesRead, MemBasicInfo02.RegionSize);
                this.DisConnect();
                return this.Connect();
            }

            //必須の値があるかどうかチェックする.
            uint procs_game_main = Program.ROM.RomInfo.workmemory_procs_game_main_address() - 0x02000000;
            uint rom_procs_game_main = Program.ROM.RomInfo.procs_game_main_address();
            uint addr = this.AppnedOffset02 + procs_game_main;
            uint data = U.u32(buffer, addr);
            if (data != rom_procs_game_main)
            {
                this.ErrorMessage = R._("Page02に必須の値がありません。procs_game_main:{0},data:{1},rom_procs_game_main:{2},"
                    , U.To0xHexString(procs_game_main), U.To0xHexString(data), U.To0xHexString(rom_procs_game_main));
                this.DisConnect();
                return this.Connect();
            }

            this.Memory02 = buffer;
            return true;
        }
        bool ReadMemory03()
        {
            Debug.Assert(MemBasicInfo03.RegionSize > 0);

            int bytesRead = 0;  // number of bytes read with ReadProcessMemory
            byte[] buffer = new byte[MemBasicInfo03.RegionSize];
            ReadProcessMemory((int)this.ProcessHandle, MemBasicInfo03.BaseAddress, buffer, MemBasicInfo03.RegionSize, ref bytesRead);

            if (bytesRead <= 0 || bytesRead < MemBasicInfo03.RegionSize)
            {
                this.ErrorMessage = R._("Page03を読めませんでした。bytesRead:{0},MemBasicInfo03.RegionSize:{1},"
                    , bytesRead, MemBasicInfo03.RegionSize);
                this.DisConnect();
                return this.Connect();
            }

            //必須の値があるかどうかチェックする.
            uint user_stack_base = Program.ROM.RomInfo.workmemory_user_stack_base_address() - 0x03000000;
            uint function_fe_main_return_address = Program.ROM.RomInfo.function_fe_main_return_address();
            uint addr = this.AppnedOffset03 + user_stack_base;
            uint data = U.u32(buffer, addr);
            if (data != function_fe_main_return_address)
            {
                this.ErrorMessage = R._("Page03に必須の値がありません。user_stack_base:{0},data:{1},function_fe_main_return_address:{2},"
                    , U.To0xHexString(user_stack_base), U.To0xHexString(data), U.To0xHexString(function_fe_main_return_address));
                this.DisConnect();
                return this.Connect();
            }

            this.Memory03 = buffer;
            return true;
        }

        void WriteMemory02(uint addr,uint data , uint size)
        {
            Debug.Assert(MemBasicInfo02.RegionSize > 0);
            Debug.Assert(size == 1 || size == 2 || size == 4);
            int bytesWrite = 0;  // number of bytes Write
            byte[] buffer = new byte[size];
            if (size == 4)
            {
                U.write_u32(buffer, 0, data);
            }
            else if (size == 2)
            {
                U.write_u16(buffer, 0, data);
            }
            else
            {
                size = 1;
                U.write_u8(buffer, 0, data);
            }
            uint writeAddr = (uint)(MemBasicInfo02.BaseAddress + addr);
            WriteProcessMemory((int)this.ProcessHandle, writeAddr, buffer, size, ref bytesWrite);

            if (bytesWrite <= 0 || bytesWrite < size)
            {
                this.ErrorMessage = R._("Page02に書き込めません。bytesWrite:{0},size:{1}"
                    , bytesWrite, size);
                this.DisConnect();
                return ;
            }

            return;
        }
        void WriteMemory03(uint addr, uint data, uint size)
        {
            Debug.Assert(MemBasicInfo03.RegionSize > 0);
            Debug.Assert(size == 1 || size == 2 || size == 4);
            int bytesWrite = 0;  // number of bytes Write
            byte[] buffer = new byte[size];
            if (size == 4)
            {
                U.write_u32(buffer, 0, data);
            }
            else if (size == 2)
            {
                U.write_u16(buffer, 0, data);
            }
            else
            {
                size = 1;
                U.write_u8(buffer, 0, data);
            }
            uint writeAddr = (uint)(MemBasicInfo03.BaseAddress + addr);
            WriteProcessMemory((int)this.ProcessHandle, writeAddr, buffer, (uint)size, ref bytesWrite);

            if (bytesWrite <= 0 || bytesWrite < size)
            {
                this.ErrorMessage = R._("Page03に書き込めません。bytesWrite:{0},size:{1}"
                    , bytesWrite, size);
                this.DisConnect();
                return;
            }

            return;
        }
        void WriteMemory02(uint addr, byte[] data, uint size)
        {
            Debug.Assert(MemBasicInfo02.RegionSize > 0);
            int bytesWrite = 0;  // number of bytes Write
            uint writeAddr = (uint)(MemBasicInfo02.BaseAddress + addr);
            WriteProcessMemory((int)this.ProcessHandle, writeAddr, data, size, ref bytesWrite);

            if (bytesWrite <= 0 || bytesWrite < size)
            {
                this.ErrorMessage = R._("Page02に書き込めません。bytesWrite:{0},size:{1}"
                    , bytesWrite, size);
                this.DisConnect();
                return;
            }

            return;
        }
        void WriteMemory03(uint addr, byte[] data, uint size)
        {
            Debug.Assert(MemBasicInfo03.RegionSize > 0);
            int bytesWrite = 0;  // number of bytes Write
            uint writeAddr = (uint)(MemBasicInfo03.BaseAddress + addr);
            WriteProcessMemory((int)this.ProcessHandle, writeAddr, data, (uint)size, ref bytesWrite);

            if (bytesWrite <= 0 || bytesWrite < size)
            {
                this.ErrorMessage = R._("Page03に書き込めません。bytesWrite:{0},size:{1}"
                    , bytesWrite, size);
                this.DisConnect();
                return;
            }

            return;
        }
        string ErrorMessage;
        public string GetErrorMessage()
        {
            return this.ErrorMessage;
        }

        public void DumpMemory0x02000000(string filename)
        {
            U.WriteAllBytes(filename, U.subrange(this.Memory02, this.AppnedOffset02, this.AppnedOffset02 + PAGE02_SIZE));
        }
        public void DumpMemory0x03000000(string filename)
        {
            U.WriteAllBytes(filename, U.subrange(this.Memory03, this.AppnedOffset03, this.AppnedOffset03 + PAGE03_SIZE));
        }

        public RAM()
        {
            this.Memory02 = new byte[0];
            this.Memory03 = new byte[0];
        }
    }
}
