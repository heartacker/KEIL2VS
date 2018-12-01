using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace KEIL2VS.ProjGen
{
    class Keil2vsGen
    {

        private XDocument document = new XDocument();
        private XmlDocument xmlDoc = new XmlDocument();
        //private FileInfo[] fileInfos;
        //private Infos.ProjectInfo projectIno;
        //private Infos.Config config;
        //private Infos.UprojInfo[] uprojInfo;
        //private readonly Infos.PreStr preStr = new Infos.PreStr();

        public string GetFullPath(string basePath, string targetPat)
        {
            var baseUri = new Uri(basePath);
            var uri = new Uri(baseUri, targetPat);
            return uri.ToString().Replace("/", "\\").Replace("file:\\\\\\", "");
        }
        public string GetRelativePath(string basePath, string targetPath)
        {
            var uri = new Uri(basePath);
            var uri2 = new Uri(targetPath);
            return uri.MakeRelativeUri(uri2).ToString().Replace("/", "\\");
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002C94 File Offset: 0x00000E94
        public string MDK_IncludePathRead(string doc, string targetName)
        {
            if (doc == "")
            {
                return "";
            }
            if (targetName == "")
            {
                return "";
            }
            xmlDoc.Load(doc);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(".//Targets/Target");
            foreach (object obj in xmlNodeList)
            {
                var xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName").InnerText == targetName)
                {
                    return xmlNode.SelectSingleNode(".//VariousControls/IncludePath").InnerText;
                }
            }
            return null;
        }

        public string[] MDK_GroupRead(string doc, string targetName)
        {
            if (doc == "")
            {
                return null;
            }
            if (targetName == "")
            {
                return null;
            }
            xmlDoc.Load(doc);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(".//Targets/Target");
            foreach (object obj in xmlNodeList)
            {
                var xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName").InnerText != targetName) continue;
                XmlNodeList xmlNodeList2 = xmlNode.SelectNodes(".//Groups/*");
                string[] array = new string[xmlNodeList2.Count];
                var num = 0;
                foreach (object obj2 in xmlNodeList2)
                {
                    var xmlNode2 = (XmlNode)obj2;
                    array[num] = xmlNode2.SelectSingleNode("./GroupName").InnerText;
                    num++;
                }
                return array;
            }
            return null;
        }

        public string[] MDK_TargetRead(string Doc)
        {
            if (Doc == "")
            {
                return null;
            }
            xmlDoc.Load(Doc);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(".//Targets/*");
            string[] array = new string[xmlNodeList.Count];
            var num = 0;
            foreach (object obj in xmlNodeList)
            {
                var xmlNode = (XmlNode)obj;
                array[num] = xmlNode.SelectSingleNode("./TargetName").InnerText;
                num++;
            }
            return array;
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002BD0 File Offset: 0x00000DD0
        public string MDK_DefineRead(string doc, string targetName)
        {
            if (doc == "")
            {
                return "";
            }
            if (targetName == "")
            {
                return "";
            }
            xmlDoc.Load(doc);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(".//Targets/Target");
            foreach (object obj in xmlNodeList)
            {
                var xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName").InnerText != targetName) continue;
                var defineString = xmlNode.SelectSingleNode(".//VariousControls/Define").InnerText;
                if (!defineString.EndsWith(","))
                {
                    defineString += ";";
                }
                defineString = defineString.Replace(',', ';');
                return defineString;
            }
            return null;
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002E88 File Offset: 0x00001088
        public string[] MDK_SrcRead(string doc, string targetName, string Group)
        {
            if (doc == "")
            {
                return null;
            }
            if (Group == "")
            {
                return null;
            }
            xmlDoc.Load(doc);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(".//Targets/Target");
            if (xmlNodeList == null) return null;
            foreach (object obj in xmlNodeList)
            {
                var xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName")?.InnerText != targetName) continue;
                XmlNodeList xmlNodeList2 = xmlNode.SelectNodes(".//Groups/Group");
                if (xmlNodeList2 == null) continue;
                foreach (object obj2 in xmlNodeList2)
                {
                    var xmlNode2 = (XmlNode)obj2;
                    if (xmlNode2.SelectSingleNode("./GroupName")?.InnerText != Group) continue;
                    XmlNodeList xmlNodeList3 = xmlNode2.SelectNodes("./Files/File");
                    string[] array = new string[xmlNodeList3.Count];
                    var num = 0;
                    foreach (object obj3 in xmlNodeList3)
                    {
                        var xmlNode3 = (XmlNode)obj3;
                        var innerText = xmlNode3.SelectSingleNode("./FilePath").InnerText;
                        array[num] = innerText;
                        num++;
                    }

                    return array;
                }
            }

            return null;
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00003058 File Offset: 0x00001258
        public bool MDK_CheckProject(string docName, ref Infos.PreStr preStr)
        {
            if (docName == "") return false;
            bool isuproj;
            try
            {
                xmlDoc.Load(docName);
                XmlNode xmlNode = xmlDoc.SelectSingleNode(".//Header");
                if (xmlNode == null)
                {
                    isuproj = false;
                }
                else if (xmlNode.InnerText == "### uVision Project, (C) Keil Software")
                {
                    isuproj = true;
                }
                else
                {
                    isuproj = false;
                }
            }
            catch
            {
                MessageBox.Show(docName + preStr.NotUvProj, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                isuproj = false;
            }
            return isuproj;
        }



        public string MDK_TargetStatusRead(string doc, string targetName)
        {
            if (doc == "")
            {
                return "";
            }
            if (targetName == "")
            {
                return "";
            }
            xmlDoc.Load(doc);
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(".//Targets/Target");
            foreach (object obj in xmlNodeList)
            {
                var xmlNode = (XmlNode)obj;
                if (xmlNode.SelectSingleNode("./TargetName").InnerText != targetName) continue;
                var str = "";
                str = str + "Device: " + xmlNode.SelectSingleNode(".//Device").InnerText + "\r\n";
                str = str + "Error: " + xmlNode.SelectSingleNode(".//TargetStatus/Error").InnerText + "\r\n";
                str = str + "ListingPath: " + xmlNode.SelectSingleNode(".//TargetCommonOption/ListingPath").InnerText + "\r\n";
                str = str + "OutputDirectory: " + xmlNode.SelectSingleNode(".//TargetCommonOption/OutputDirectory").InnerText + "\r\n";
                str = str + "Define: " + xmlNode.SelectSingleNode(".//VariousControls/Define").InnerText + "\r\n";
                str += "IncludePath: \r\n";
                var text = xmlNode.SelectSingleNode(".//VariousControls/IncludePath").InnerText;
                text = text.Replace("..\\", "**\\");
                text = text.Replace(".\\", "..\\");
                text = text.Replace("**\\", "..\\");
                text = text.Replace(";", ";\r\n");
                return str + text;
            }
            return null;
        }

        public void VC_vcxproj_Create(string docName, IReadOnlyList<string> targets,
                                        ref Infos.ProjectInfo projectIno, ref Infos.PreStr preStr,
                                        ref Infos.Config config)
        {
            if (docName == "")
            {
                return;
            }
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var xElProject = new XElement(ns + "Project", new XAttribute("DefaultTargets", "Build"), new XAttribute("ToolsVersion", "15.0"));
            var xEl_ItemGroup = new XElement(ns + "ItemGroup", new XAttribute("Label", "ProjectConfigurations"));
            foreach (var targetName in targets)
            {
                xEl_ItemGroup.Add(new XElement(ns + "ProjectConfiguration",
                    new XAttribute("Include", "Target|Win32".Replace("Target", targetName)),
                    new XElement(ns + "Configuration", targetName),
                    new XElement(ns + "Platform", "Win32")));
            }
            xElProject.Add(xEl_ItemGroup);
            var xEl_PropertyGroup = new XElement(ns + "PropertyGroup",
                new XAttribute("Label", "Globals"),
                new XElement(ns + "ProjectGuid", Guid.NewGuid().ToString("B")),
                new XElement(ns + "Keyword", "MakeFileProj"),
                new XElement(ns + "WindowsTargetPlatformVersion", "10.0.17134.0"));
            xElProject.Add(xEl_PropertyGroup);
            xElProject.Add(new XElement(ns + "Import",
                new XAttribute("Project", "$(VCTargetsPath)\\Microsoft.Cpp.Default.props")));
            foreach (var newValue in targets)
            {
                xElProject.Add(new XElement(ns + "PropertyGroup",
                    new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", newValue)),
                    new XAttribute("Label", "Configuration"),
                    new XElement(ns + "ConfigurationType", "Makefile"),
                    new XElement(ns + "UseDebugLibraries", "true"),
                    new XElement(ns + "PlatformToolset", "v141")));
            }
            xElProject.Add(new XElement(ns + "Import",
                new XAttribute("Project", "$(VCTargetsPath)\\Microsoft.Cpp.props")),
                new XElement(ns + "ImportGroup",
                new XAttribute("Label", "ExtensionSettings")));
            foreach (var newValue2 in targets)
            {
                xElProject.Add(new XElement(ns + "ImportGroup",
                    new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", newValue2)),
                    new XAttribute("Label", "PropertySheets"),
                    new XElement(ns + "Import",
                    new XAttribute("Project", "$(UserRootDir)\\Microsoft.Cpp.$(Platform).user.props"),
                    new XAttribute("Condition", "exists('$(UserRootDir)\\Microsoft.Cpp.$(Platform).user.props')"),
                    new XAttribute("Label", "LocalAppDataPlatform"))));
            }
            xElProject.Add(new XElement(ns + "PropertyGroup", new XAttribute("Label", "UserMacros")));
            foreach (var oneTarget in targets)
            {
                var includepathTemp = MDK_IncludePathRead(projectIno.MdkProjectFile, oneTarget);
                string[] array = includepathTemp.Split(';');
                string keilIncludePath = null;
                foreach (var targetPat in array)
                {
                    var fullPath = GetFullPath(projectIno.MdkProjectPath, targetPat);
                    keilIncludePath = keilIncludePath + GetRelativePath(projectIno.VcProjectPath, fullPath) + ";";
                }
                //for (var m = 0; m < array.Length; m++)
                //{
                //    var fullPath = GetFullPath(_projectIno.MDK_Project_Path, array[m]);
                //    keilIncludePath = keilIncludePath + GetRelativePath(_projectIno.VCProject_Path, fullPath) + ";";
                //}
                keilIncludePath += config.UV4IncPath + ";";

                xElProject.Add(new XElement(ns + "PropertyGroup",
                    new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", oneTarget)),
                    new XElement(ns + "NMakeOutput", "Template.bin".Replace("Template", projectIno.ProjectName)),
                    new XElement(ns + "NMakePreprocessorDefinitions", preStr.PredefineKeil + config.PreDefine + MDK_DefineRead(projectIno.MdkProjectFile, oneTarget)),
                    new XElement(ns + "IncludePath", keilIncludePath),
                    new XElement(ns + "NMakeBuildCommandLine", projectIno.NMakeBuildCommandLine.Replace("Target", oneTarget)),
                    new XElement(ns + "LibraryPath", @"$(VC_LibraryPath_x86);$(WindowsSDK_LibraryPath_x86);$(NETFXKitsDir)Lib\um\x86;" + config.UV4LibPath + ";")));
            }
            xElProject.Add(new XElement(ns + "ItemDefinitionGroup", ""));
            string[] array2 = MDK_GroupRead(projectIno.MdkProjectFile, targets[0]);
            var xElNone = new XElement(ns + "ItemGroup", "");
            var xElClCompile = new XElement(ns + "ItemGroup", "");
            foreach (var group in array2)
            {
                string[] array4 = MDK_SrcRead(projectIno.MdkProjectFile, targets[0], group);
                foreach (var targetPat in array4)
                {
                    var text5 = GetFullPath(projectIno.MdkProjectPath, targetPat);
                    text5 = GetRelativePath(projectIno.VcProjectPath, text5);
                    if (text5.EndsWith(".c"))
                    {
                        xElClCompile.Add(new XElement(ns + "ClCompile", new XAttribute("Include", text5)));
                    }
                    else
                    {
                        xElNone.Add(new XElement(ns + "None", new XAttribute("Include", text5)));
                    }
                }
            }
            xElNone.Add(new XElement(ns + "None", new XAttribute("Include", "Readme.txt")));
            xElProject.Add(xElNone);
            xElProject.Add(xElClCompile);
            xElProject.Add(new XElement(ns + "Import", new XAttribute("Project", "$(VCTargetsPath)\\Microsoft.Cpp.targets")));
            xElProject.Add(new XElement(ns + "ImportGroup", new XAttribute("Label", "ExtensionTargets"), ""));
            xElProject.Save(docName);
        }

        public void VC_Filters_Create(string docName, string[] targets, ref Infos.ProjectInfo projectIno)
        {
            if (docName == "")
            {
                return;
            }
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var xelement = new XElement(ns + "Project", new XAttribute("DefaultTargets", "Build"), new XAttribute("ToolsVersion", "4.0"));
            var xElItemGroup1 = new XElement(ns + "ItemGroup", "");
            const string sourceFiles = "资源文件";
            xElItemGroup1.Add(
                new XElement(ns + "Filter", new XAttribute("Include", sourceFiles),
                new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B")),
                new XElement(ns + "Extensions", "cpp;c;cc;cxx;def;odl;idl;hpj;bat;asm;asmx")),
                new XElement(ns + "Filter", new XAttribute("Include", "include"),
                new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B")),
                new XElement(ns + "Extensions", "h;hpp;hxx;hm;inl;inc;xsd")),
                new XElement(ns + "Filter", new XAttribute("Include", "项目说明"),
                new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B")),
                new XElement(ns + "Extensions", "txt")));
            string[] groups = MDK_GroupRead(projectIno.MdkProjectFile, projectIno.MdkTarget);
            foreach (var str in groups)
            {
                xElItemGroup1.Add(new XElement(ns + "Filter", new XAttribute("Include", sourceFiles + "\\" + str),
                    new XElement(ns + "UniqueIdentifier", Guid.NewGuid().ToString("B"))));
            }
            xelement.Add(xElItemGroup1);
            xElItemGroup1 = new XElement(ns + "ItemGroup", "");
            var xElItemGroup2 = new XElement(ns + "ItemGroup", "");
            foreach (var group in groups)
            {
                string[] srcInThisGroup = MDK_SrcRead(projectIno.MdkProjectFile, targets[0], group);
                foreach (var srcPath in srcInThisGroup)
                {
                    var srFullPath = GetFullPath(projectIno.MdkProjectPath, srcPath);
                    srFullPath = GetRelativePath(projectIno.VcProjectPath, srFullPath);
                    if (srFullPath.EndsWith(".c"))
                    {
                        xElItemGroup2.Add(new XElement(ns + "ClCompile",
                            new XAttribute("Include", srFullPath),
                            new XElement(ns + "Filter", sourceFiles + "\\" + group)));
                    }
                    else
                    {
                        xElItemGroup1.Add(new XElement(ns + "None",
                            new XAttribute("Include", srFullPath),
                            new XElement(ns + "Filter", sourceFiles + "\\" + group)));
                    }
                }
            }
            xElItemGroup1.Add(new XElement(ns + "None",
                new XAttribute("Include", "Readme.txt"),
                new XElement(ns + "Filter", sourceFiles)));
            xelement.Add(xElItemGroup1);
            xelement.Add(xElItemGroup2);
            xelement.Save(docName);
        }
        public void VC_Create_UserFile(string docName, string Debugcmd, string workingDirectory, string[] targets, ref Infos.ProjectInfo projectIno)
        {
            if (docName == "")
            {
                return;
            }
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var xEl_proj = new XElement(ns + "Project", new XAttribute("ToolsVersion", "4.0"));
            foreach (var newValue in targets)
            {
                xEl_proj.Add(new XElement(ns + "PropertyGroup",
                    new XAttribute("Condition", "'$(Configuration)|$(Platform)'=='Target|Win32'".Replace("Target", newValue)),
                    new XElement(ns + "LocalDebuggerCommand", $"\"{projectIno.UV4Path}\" "),
                    new XElement(ns + "LocalDebuggerCommandArguments", Debugcmd.Replace("Target", newValue)),
                    new XElement(ns + "LocalDebuggerWorkingDirectory", workingDirectory),
                    new XElement(ns + "DebuggerFlavor", "WindowsLocalDebugger"))
                    );
            }
            xEl_proj.Save(docName);
        }

        public void VC_Creat_readme(string docName, string projectName, ref Infos.PreStr preStr)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("========================================================================\r\n");
            stringBuilder.Append("    生成文件项目：Template 项目概述\r\n");
            stringBuilder.Append("========================================================================\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("本文件概要介绍组成 Template 项目的每个文件的内容。\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("Template.sln\r\n");
            stringBuilder.Append("    这是Template项目的解决方案文件\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("Template.vcxproj\r\n");
            stringBuilder.Append("    这是Template项目的主项目文件\r\n");
            stringBuilder.Append("    其中包含了这个项目中的各个Target，\r\n");
            stringBuilder.Append("    以及Include Path、所有源文件的路径、编译命令。\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("Template.vcxproj.filters\r\n");
            stringBuilder.Append("    这是Template项目的项目筛选器文件。\r\n");
            stringBuilder.Append("    它包含了这个项目中的所有源文件分组及源文件的路径。\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("Template.vcxproj.user\r\n");
            stringBuilder.Append("    这是Template项目的 用户文件，\r\n");
            stringBuilder.Append("    它包含了这个项目中的各个Target的 Debug命令。\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("以上文件由MDK Project  To Visual Studio Project 工具读取 Keil uVision4\r\n");
            stringBuilder.Append("的项目文件：Template.uvproj 中的设定，按照Visual Studio 2017\r\n");
            stringBuilder.Append("中VC++ “生成文件项目” 的模板文件来生成的，如有疑问，请看MSDN~\r\n");
            stringBuilder.Append("\r\n");
            DateTime dt = DateTime.Now;
            stringBuilder.Append(dt + "\r\n");
            stringBuilder.Append(preStr.Author + "\r\n");
            stringBuilder.Append("/////////////////////////////////////////////////////////////////////////////\r\n");
            stringBuilder = stringBuilder.Replace("Template", projectName);
            FileStream fileStream = File.OpenWrite(docName);
            byte[] bytes = new UTF8Encoding(true).GetBytes(stringBuilder.ToString());
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }

        public void VC_Creat_Sln(string docName, string projectName, string[] targets)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Microsoft Visual Studio Solution File, Format Version 12.00\r\n");
            stringBuilder.Append("# Visual Studio 15\r\n");
            stringBuilder.Append("VisualStudioVersion = 15.0.27703.2018\r\n");
            stringBuilder.Append("MinimumVisualStudioVersion = 10.0.40219.1\r\n");
            stringBuilder.Append("Project(\"{" + Guid.NewGuid().ToString("B") + "}\") = \"Template\", \"Template.vcxproj\", \"{" + Guid.NewGuid().ToString("B") + "}\"");
            stringBuilder.Append("\r\nEndProject\r\n");
            stringBuilder.Append("Global\r\n");
            stringBuilder.Append("\tGlobalSection(SolutionConfigurationPlatforms) = preSolution\r\n");
            foreach (var newValue in targets)
            {
                var text = "\t\tDebug|Win32 = Debug|Win32\r\n";
                text = text.Replace("Debug", newValue);
                stringBuilder.Append(text);
            }
            stringBuilder.Append("\tEndGlobalSection\r\n");
            stringBuilder.Append("\tGlobalSection(ProjectConfigurationPlatforms) = postSolution\r\n");
            var newValue2 = Guid.NewGuid().ToString("B");
            foreach (var newValue3 in targets)
            {
                var text2 = "\t\tGUID.Debug|Win32.ActiveCfg = Debug|Win32\r\n";
                var text3 = "\t\tGUID.Debug|Win32.Build.0 = Debug|Win32\r\n";
                text2 = text2.Replace("GUID", newValue2);
                text2 = text2.Replace("Debug", newValue3);
                text3 = text3.Replace("GUID", newValue2);
                text3 = text3.Replace("Debug", newValue3);
                stringBuilder.Append(text2);
                stringBuilder.Append(text3);
            }
            stringBuilder.Append("\tEndGlobalSection\r\n");
            stringBuilder.Append("\t\tGlobalSection(SolutionProperties) = preSolution\r\n");
            stringBuilder.Append("\t\tHideSolutionNode = FALSE\r\n");
            stringBuilder.Append("\tEndGlobalSection\r\n");
            stringBuilder.Append("\tGlobalSection(ExtensibilityGlobals) = postSolution\r\n");
            stringBuilder.Append("\t\tSolutionGuid = {" + Guid.NewGuid().ToString("B") + "}\r\n");
            stringBuilder.Append("\tEndGlobalSection\r\n");
            stringBuilder.Append("EndGlobal\r\n");
            stringBuilder = stringBuilder.Replace("Template", projectName);
            FileStream fileStream = File.OpenWrite(docName);
            byte[] bytes = new UTF8Encoding(true).GetBytes(stringBuilder.ToString());
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
            stringBuilder.Clear();
        }

    }
}
