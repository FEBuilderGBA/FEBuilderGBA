using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace FEBuilderGBA
{
    sealed class ROMFE0 : ROMFEINFO
    {
        public ROMFE0(ROM rom)
    	{
    		VersionToFilename = "NAZO";
    		TitleToFilename = "NAZO";
            OverwriteROMConstants(rom);
        }
    };

}
    