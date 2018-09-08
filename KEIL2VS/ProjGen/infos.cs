using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace KEIL2VS.ProjGen
{
    class Infos
    {
        public struct UprojInfo
        {
            public string FileName;
            public string FileFullname;
        }

        public struct ProjectInfo
        {
            public string UV4Path;
            public string MdkProjectPath;
            public string MdkProjectFile;
            public string MdkTarget;
            public string ProjectName;
            public string IncludePath;
            public string VcProjectPath;
            public string VcxprojName;
            public string VcFiltersName;
            public string VcUserFileName;
            public string NMakePreprocessorDefinitions;
            public string NMakeBuildCommandLine;
            public string NMakeCleanCommandLine;
            public string LocalDebuggerCommandArguments;
            public string LocalDebuggerWorkingDirectory;
            public string CuruProjectFileDir;
        }
        public struct Config
        {
            public string ToolName;
            public string ToolsVersion;
            public string UV4Path;
            public string DocName;
            public string UV4IncPath;
            public string UV4LibPath;
            public string PreDefine;
        }

        public struct PreStr
        {
            public string Configfilename;
            public string FristUse;
            public string ToolsVersion;
            public string PredefineKeil;
            public string Predefine;
            public string NoKeil;
            public string RegPath;
            public string NotUvProj;
            public string Notkeilexe;
            public string Author;
            public string NameSp;
            public string FindUprojInThisFolder;
            public string Selectnewfolder;

            public string ApplicationStartpath { get; set; }
            public string Sourepretext { get; set; }
        }
    }
}
