using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FEBuilderGBA
{
    public class EventTemplateImpl
    {
        public EventTemplateImpl()
        {
        }

        uint MapID;
        EventScriptInnerControl CurrentControl;
        public void Init(uint mapid, EventScriptInnerControl currentControl)
        {
            this.MapID = mapid;
            this.CurrentControl = currentControl;
        }
        public void LoadTemplate()
        {
            this.EventTemplateList = new List<EventTemplate>();

            string configFilename = U.ConfigDataFilename("template_list_event_");
            if (!File.Exists(configFilename))
            {
                return;
            }
            string[] lines = File.ReadAllLines(configFilename);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (U.OtherLangLine(line))
                {
                    continue;
                }
                string[] sp = line.Split('\t');
                if (sp.Length < 2)
                {
                    continue;
                }
                EventTemplate et = new EventTemplate();
                et.Filename = sp[0];
                et.Info = sp[1];

                this.EventTemplateList.Add(et);
            }
        }

        public class EventTemplate
        {
            public string Filename;
            public string Info;
        }

        List<EventTemplate> EventTemplateList;

        public EventTemplate GetTemplate(int index)
        {
            if (index < 0 || index >= EventTemplateList.Count)
            {
                return null;
            }
            return EventTemplateList[index];
        }
        public List<EventTemplate> GetTemplateAll()
        {
            return EventTemplateList;
        }

        public const uint INVALIDATE_UNIT_POINTER = 0xFFFFFF;
        static string ToPointerToString(uint addr)
        {
            if (addr == U.NOT_FOUND)
            {
                addr = EventUnitForm.INVALIDATE_UNIT_POINTER;
            }
            return U.ToHexString8(U.ChangeEndian32(U.toPointer(addr)));
        }
        static string ToUShortToString(uint addr)
        {
            return U.ToHexString8(U.ChangeEndian32(addr)).Substring(0, 4);
        }



        public List<EventScript.OneCode> GetCodes(EventTemplate et)
        {
            List<EventScript.OneCode>  Codes = new List<EventScript.OneCode>();

            string fullfilename = Path.Combine(Program.BaseDirectory, "config", "data", et.Filename);
            if (!File.Exists(fullfilename))
            {
                return Codes;
            }

            string XXXXXXXX = null;
            string YYYYYYYY = null;
            if (et.Filename.IndexOf("template_event_CALL_END_EVENT") >= 0)
            {
                XXXXXXXX = ToPointerToString(EventCondForm.GetEndEvent(this.MapID));
            }
            else if (et.Filename.IndexOf("template_event_PREPARATION") >= 0)
            {
                XXXXXXXX = ToPointerToString(EventCondForm.GetPlayerUnits(this.MapID));
                YYYYYYYY = ToPointerToString(EventCondForm.GetEnemyUnits(this.MapID));
            }
            else if (et.Filename.IndexOf("_COND_") >= 0)
            {
                uint labelX = GetUnuseLabelID(0x9000);
                XXXXXXXX = ToUShortToString(labelX);

                uint labelY = GetUnuseLabelID(labelX + 1);
                YYYYYYYY = ToUShortToString(labelY);
            }


            byte[] bin = EventScriptInnerControl.ConverteventTextToBin(fullfilename
                , EventScriptInnerControl.TermCode.NoTerm
                , XXXXXXXX, YYYYYYYY);
            uint addr = 0;
            uint limit = (uint)bin.Length;
            while (addr < limit)
            {
                EventScript.OneCode code = Program.EventScript.DisAseemble(bin, addr);
                Codes.Add(code);
                addr += (uint)code.Script.Size;
            }
            return Codes;
        }

        uint GetUnuseLabelID(uint startID)
        {
            for (uint id = startID; id < 0xffff; id++)
            {
                if (!this.CurrentControl.IsUseLabelID(id))
                {
                    return id;
                }
            }
            return 0xffff;
        }
    }
}
