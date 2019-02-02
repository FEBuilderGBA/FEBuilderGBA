using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEBuilderGBA
{
    public class GBAMemory
    {
        public const uint ROM_ADDR = 0x8000000;
        public const int ROM_MAX_LENGTH = 0x2000000;
        public const uint BIOS_ADDR = 0;
        public const int BIOS_MAX_LENGTH = 0x4000;
        public const uint EWRAM_ADDR = 0x2000000;
        public const int EWRAM_MAX_LENGTH = 0x40000;
        public const uint IWRAM_ADDR = 0x3000000;
        public const int IWRAM_MAX_LENGTH = 0x8000;
        public const uint IOREG_ADDR = 0x4000000;
        public const int IOREG_MAX_LENGTH = 0x400;
        public const uint PALRAM_ADDR = 0x5000000;
        public const int PALRAM_MAX_LENGTH = 0x400;
        public const uint VRAM_ADDR = 0x6000000;
        public const int VRAM_MAX_LENGTH = 0x18000;
        public const uint OAM_ADDR = 0x7000000;
        public const int OAM_MAX_LENGTH = 0x400;
        public const uint SRAM_ADDR = 0xE000000;
        public const int SRAM_MAX_LENGTH = 0x10000;
    }
}
