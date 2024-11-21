using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Documents;

namespace ModdersAssistant
{
    public static class ProgramData
    {
        public static bool isDebugBuild {
            get {
                #if DEBUG
                    return true;
                #else
                    return false;
                #endif
            }
        }
        public static bool isRuntime => MainWindow.current != null;
        public static bool runUnitTests = true;
        public static bool logDebugMessages = true;
        public static string programName = "Modder's Assistant";
        public static bool safeToSave = false;
        public static bool doneGameFolderWarning = false;

        public static int programVersion = 1;
        public static int majorVersion = 0;
        public static int minorVersion = 0;

        public static int programWidth = 1600;
        public static int programHeight = 900;

        public static string versionText => $"{programVersion}.{majorVersion}.{minorVersion}"
;
        public static class FilePaths {
            public static string rootFolder {
                get {
                    if (!isDebugBuild) {
                        return $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ModdersAssistantData";
                    }
                    else {
                        return $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ModdersAssistantDataDebug";
                    }
                }
            }

            // Folders
            public static string resourcesFolder = $"{rootFolder}\\Resources";
            public static string dataFolder = $"{rootFolder}\\Data";
            public static string logsFolder = $"{rootFolder}\\Logs";
            public static string crashReportsFolder = $"{rootFolder}\\CrashReports";

            // Files
            public static string logFile = $"{logsFolder}\\ModdersAssistant.log";
            public static string settingsFile = $"{dataFolder}\\Settings.json";
            public static string projectsSaveFile = $"{dataFolder}\\ProjectsSaveFile.json";
            public static string ticketsSaveFile = $"{dataFolder}\\TicketsSaveFile.json";

            public static string installationTemplate = $"{dataFolder}\\InstallationTemplate.txt";
            public static string disclaimerTemplate = $"{dataFolder}\\DisclaimerTemplate.txt";
            public static string discordMessageTemplate = $"{dataFolder}\\DiscordMessageTemplate.txt";

            // Public Functions

            public static void CreateFolderStructure() {
                List<string> folders = new List<string> {
                    rootFolder,
                    resourcesFolder,
                    $"{resourcesFolder}\\ControlBox",
                    $"{resourcesFolder}\\GUI",
                    dataFolder,
                    logsFolder,
                    crashReportsFolder
                };

                foreach (string folder in folders) {
                    Directory.CreateDirectory(folder);
                }
            }

            public static void GenerateResources() {
                
                // ControlBox
                GenerateSettingsSVG();
                GenerateMoveSVG();
                GenerateMinimiseSVG();
                GenerateCloseSVG();

                // GUI
                GenerateUpSVG();
                GenerateDownSVG();
                GenerateInfoSVG();
                GenerateWarningSVG();
                GenerateErrorSVG();
                GenerateAddSVG();
                GenerateEditSVG();
                GenerateFolderSVG();
                GenerateDllSVG();
                GenerateJsonSVG();
                GenerateMarkdownSVG();
                GeneratePngSVG();
                GenerateBinSVG();

                MainWindow.current.controlBox.RefreshIcons();
            }

            // Private Functions

            private static void GenerateSVG(string name, string svg) {
                string path = $"{resourcesFolder}/{name}.svg";
                if (File.Exists(path)) return;
                File.WriteAllText(path, svg);
            }

            // Control Box SVGs

            private static void GenerateSettingsSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<svg
   xmlns:dc=""http://purl.org/dc/elements/1.1/""
   xmlns:cc=""http://creativecommons.org/ns#""
   xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#""
   xmlns:svg=""http://www.w3.org/2000/svg""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   id=""Layer_1""
   enable-background=""new 0 0 512 512""
   height=""512""
   viewBox=""0 0 512 512""
   width=""512""
   version=""1.1""
   sodipodi:docname=""Settings.svg""
   inkscape:version=""1.0.2-2 (e86c870879, 2021-01-15)"">
  <metadata
     id=""metadata11"">
    <rdf:RDF>
      <cc:Work
         rdf:about="""">
        <dc:format>image/svg+xml</dc:format>
        <dc:type
           rdf:resource=""http://purl.org/dc/dcmitype/StillImage"" />
      </cc:Work>
    </rdf:RDF>
  </metadata>
  <defs
     id=""defs9"" />
  <sodipodi:namedview
     pagecolor=""#ffffff""
     bordercolor=""#666666""
     borderopacity=""1""
     objecttolerance=""10""
     gridtolerance=""10""
     guidetolerance=""10""
     inkscape:pageopacity=""0""
     inkscape:pageshadow=""2""
     inkscape:window-width=""1920""
     inkscape:window-height=""1017""
     id=""namedview7""
     showgrid=""false""
     inkscape:zoom=""1.6953125""
     inkscape:cx=""256""
     inkscape:cy=""256""
     inkscape:window-x=""-8""
     inkscape:window-y=""-8""
     inkscape:window-maximized=""1""
     inkscape:current-layer=""Layer_1"" />
  <path
     d=""m272.066 512h-32.133c-25.989 0-47.134-21.144-47.134-47.133v-10.871c-11.049-3.53-21.784-7.986-32.097-13.323l-7.704 7.704c-18.659 18.682-48.548 18.134-66.665-.007l-22.711-22.71c-18.149-18.129-18.671-48.008.006-66.665l7.698-7.698c-5.337-10.313-9.792-21.046-13.323-32.097h-10.87c-25.988 0-47.133-21.144-47.133-47.133v-32.134c0-25.989 21.145-47.133 47.134-47.133h10.87c3.531-11.05 7.986-21.784 13.323-32.097l-7.704-7.703c-18.666-18.646-18.151-48.528.006-66.665l22.713-22.712c18.159-18.184 48.041-18.638 66.664.006l7.697 7.697c10.313-5.336 21.048-9.792 32.097-13.323v-10.87c0-25.989 21.144-47.133 47.134-47.133h32.133c25.989 0 47.133 21.144 47.133 47.133v10.871c11.049 3.53 21.784 7.986 32.097 13.323l7.704-7.704c18.659-18.682 48.548-18.134 66.665.007l22.711 22.71c18.149 18.129 18.671 48.008-.006 66.665l-7.698 7.698c5.337 10.313 9.792 21.046 13.323 32.097h10.87c25.989 0 47.134 21.144 47.134 47.133v32.134c0 25.989-21.145 47.133-47.134 47.133h-10.87c-3.531 11.05-7.986 21.784-13.323 32.097l7.704 7.704c18.666 18.646 18.151 48.528-.006 66.665l-22.713 22.712c-18.159 18.184-48.041 18.638-66.664-.006l-7.697-7.697c-10.313 5.336-21.048 9.792-32.097 13.323v10.871c0 25.987-21.144 47.131-47.134 47.131zm-106.349-102.83c14.327 8.473 29.747 14.874 45.831 19.025 6.624 1.709 11.252 7.683 11.252 14.524v22.148c0 9.447 7.687 17.133 17.134 17.133h32.133c9.447 0 17.134-7.686 17.134-17.133v-22.148c0-6.841 4.628-12.815 11.252-14.524 16.084-4.151 31.504-10.552 45.831-19.025 5.895-3.486 13.4-2.538 18.243 2.305l15.688 15.689c6.764 6.772 17.626 6.615 24.224.007l22.727-22.726c6.582-6.574 6.802-17.438.006-24.225l-15.695-15.695c-4.842-4.842-5.79-12.348-2.305-18.242 8.473-14.326 14.873-29.746 19.024-45.831 1.71-6.624 7.684-11.251 14.524-11.251h22.147c9.447 0 17.134-7.686 17.134-17.133v-32.134c0-9.447-7.687-17.133-17.134-17.133h-22.147c-6.841 0-12.814-4.628-14.524-11.251-4.151-16.085-10.552-31.505-19.024-45.831-3.485-5.894-2.537-13.4 2.305-18.242l15.689-15.689c6.782-6.774 6.605-17.634.006-24.225l-22.725-22.725c-6.587-6.596-17.451-6.789-24.225-.006l-15.694 15.695c-4.842 4.843-12.35 5.791-18.243 2.305-14.327-8.473-29.747-14.874-45.831-19.025-6.624-1.709-11.252-7.683-11.252-14.524v-22.15c0-9.447-7.687-17.133-17.134-17.133h-32.133c-9.447 0-17.134 7.686-17.134 17.133v22.148c0 6.841-4.628 12.815-11.252 14.524-16.084 4.151-31.504 10.552-45.831 19.025-5.896 3.485-13.401 2.537-18.243-2.305l-15.688-15.689c-6.764-6.772-17.627-6.615-24.224-.007l-22.727 22.726c-6.582 6.574-6.802 17.437-.006 24.225l15.695 15.695c4.842 4.842 5.79 12.348 2.305 18.242-8.473 14.326-14.873 29.746-19.024 45.831-1.71 6.624-7.684 11.251-14.524 11.251h-22.148c-9.447.001-17.134 7.687-17.134 17.134v32.134c0 9.447 7.687 17.133 17.134 17.133h22.147c6.841 0 12.814 4.628 14.524 11.251 4.151 16.085 10.552 31.505 19.024 45.831 3.485 5.894 2.537 13.4-2.305 18.242l-15.689 15.689c-6.782 6.774-6.605 17.634-.006 24.225l22.725 22.725c6.587 6.596 17.451 6.789 24.225.006l15.694-15.695c3.568-3.567 10.991-6.594 18.244-2.304z""
     id=""path2""
     style=""fill:#ffffff;fill-opacity:1"" />
  <path
     d=""m256 367.4c-61.427 0-111.4-49.974-111.4-111.4s49.973-111.4 111.4-111.4 111.4 49.974 111.4 111.4-49.973 111.4-111.4 111.4zm0-192.8c-44.885 0-81.4 36.516-81.4 81.4s36.516 81.4 81.4 81.4 81.4-36.516 81.4-81.4-36.515-81.4-81.4-81.4z""
     id=""path4""
     style=""fill:#ffffff;fill-opacity:1"" />
</svg>
";
                GenerateSVG("ControlBox/Settings", svg);
            }

            private static void GenerateMoveSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<svg
   xmlns:dc=""http://purl.org/dc/elements/1.1/""
   xmlns:cc=""http://creativecommons.org/ns#""
   xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#""
   xmlns:svg=""http://www.w3.org/2000/svg""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   version=""1.1""
   id=""Layer_1""
   x=""0px""
   y=""0px""
   viewBox=""0 0 492.009 492.009""
   style=""enable-background:new 0 0 492.009 492.009;""
   xml:space=""preserve""
   sodipodi:docname=""Move.svg""
   inkscape:version=""1.0.2-2 (e86c870879, 2021-01-15)""><metadata
   id=""metadata67""><rdf:RDF><cc:Work
       rdf:about=""""><dc:format>image/svg+xml</dc:format><dc:type
         rdf:resource=""http://purl.org/dc/dcmitype/StillImage"" /></cc:Work></rdf:RDF></metadata><defs
   id=""defs65"" /><sodipodi:namedview
   pagecolor=""#ffffff""
   bordercolor=""#666666""
   borderopacity=""1""
   objecttolerance=""10""
   gridtolerance=""10""
   guidetolerance=""10""
   inkscape:pageopacity=""0""
   inkscape:pageshadow=""2""
   inkscape:window-width=""1920""
   inkscape:window-height=""1017""
   id=""namedview63""
   showgrid=""false""
   inkscape:zoom=""1.7641954""
   inkscape:cx=""246.0045""
   inkscape:cy=""246.0045""
   inkscape:window-x=""-8""
   inkscape:window-y=""-8""
   inkscape:window-maximized=""1""
   inkscape:current-layer=""Layer_1"" />
<g
   id=""g6""
   style=""fill:#ffffff;fill-opacity:1"">
	<g
   id=""g4""
   style=""fill:#ffffff;fill-opacity:1"">
		<path
   d=""M314.343,62.977L255.399,4.033c-2.672-2.672-6.236-4.04-9.92-4.032c-3.752-0.036-7.396,1.36-10.068,4.032l-57.728,57.728    c-5.408,5.408-5.408,14.2,0,19.604l7.444,7.444c5.22,5.22,14.332,5.22,19.556,0l22.1-22.148v81.388    c0,0.248,0.144,0.452,0.188,0.684c0.6,7.092,6.548,12.704,13.8,12.704h10.52c7.644,0,13.928-6.208,13.928-13.852v-9.088    c0-0.04,0-0.068,0-0.1V67.869l22.108,22.152c5.408,5.408,14.18,5.408,19.584,0l7.432-7.436    C319.751,77.173,319.751,68.377,314.343,62.977z""
   id=""path2""
   style=""fill:#ffffff;fill-opacity:1"" />
	</g>
</g>
<g
   id=""g12""
   style=""fill:#ffffff;fill-opacity:1"">
	<g
   id=""g10""
   style=""fill:#ffffff;fill-opacity:1"">
		<path
   d=""M314.335,409.437l-7.44-7.456c-5.22-5.228-14.336-5.228-19.564,0l-22.108,22.152v-70.216c0-0.04,0-0.064,0-0.1v-9.088    c0-7.648-6.288-14.16-13.924-14.16h-10.528c-7.244,0-13.192,5.756-13.796,12.856c-0.044,0.236-0.188,0.596-0.188,0.84v81.084    l-22.1-22.148c-5.224-5.224-14.356-5.224-19.58,0l-7.44,7.444c-5.4,5.404-5.392,14.2,0.016,19.608l57.732,57.724    c2.604,2.612,6.08,4.032,9.668,4.032h0.52c3.716,0,7.184-1.416,9.792-4.032l58.94-58.94    C319.743,423.633,319.743,414.841,314.335,409.437z""
   id=""path8""
   style=""fill:#ffffff;fill-opacity:1"" />
	</g>
</g>
<g
   id=""g18""
   style=""fill:#ffffff;fill-opacity:1"">
	<g
   id=""g16""
   style=""fill:#ffffff;fill-opacity:1"">
		<path
   d=""M147.251,226.781l-1.184,0h-7.948c-0.028,0-0.056,0-0.088,0h-69.88l22.152-22.032c2.612-2.608,4.048-6.032,4.048-9.74    c0-3.712-1.436-7.164-4.048-9.768l-7.444-7.428c-5.408-5.408-14.204-5.4-19.604,0.008l-58.944,58.94    c-2.672,2.668-4.1,6.248-4.028,9.92c-0.076,3.82,1.356,7.396,4.028,10.068l57.728,57.732c2.704,2.704,6.252,4.056,9.804,4.056    s7.1-1.352,9.804-4.056l7.44-7.44c2.612-2.608,4.052-6.092,4.052-9.8c0-3.712-1.436-7.232-4.052-9.836l-22.144-22.184h80.728    c0.244,0,0.644-0.06,0.876-0.104c7.096-0.6,12.892-6.468,12.892-13.716v-10.536C161.439,233.229,154.895,226.781,147.251,226.781z    ""
   id=""path14""
   style=""fill:#ffffff;fill-opacity:1"" />
	</g>
</g>
<g
   id=""g24""
   style=""fill:#ffffff;fill-opacity:1"">
	<g
   id=""g22""
   style=""fill:#ffffff;fill-opacity:1"">
		<path
   d=""M487.695,236.765l-58.944-58.936c-5.404-5.408-14.2-5.408-19.604,0l-7.436,7.444c-2.612,2.604-4.052,6.088-4.052,9.796    c0,3.712,1.436,7.072,4.052,9.68l22.148,22.032h-70.328c-0.036,0-0.064,0-0.096,0h-9.084c-7.644,0-13.78,6.444-13.78,14.084    v10.536c0,7.248,5.564,13.108,12.664,13.712c0.236,0.048,0.408,0.108,0.648,0.108h81.188l-22.156,22.18    c-2.608,2.604-4.048,6.116-4.048,9.816c0,3.716,1.436,7.208,4.048,9.816l7.448,7.444c2.7,2.704,6.248,4.06,9.8,4.06    s7.096-1.352,9.8-4.056l57.736-57.732c2.664-2.664,4.092-6.244,4.028-9.92C491.787,243.009,490.359,239.429,487.695,236.765z""
   id=""path20""
   style=""fill:#ffffff;fill-opacity:1"" />
	</g>
</g>
<g
   id=""g30""
   style=""fill:#ffffff;fill-opacity:1"">
	<g
   id=""g28""
   style=""fill:#ffffff;fill-opacity:1"">
		<path
   d=""M246.011,207.541c-21.204,0-38.456,17.252-38.456,38.46c0,21.204,17.252,38.46,38.456,38.46    c21.204,0,38.46-17.256,38.46-38.46C284.471,224.793,267.215,207.541,246.011,207.541z""
   id=""path26""
   style=""fill:#ffffff;fill-opacity:1"" />
	</g>
</g>
<g
   id=""g32""
   style=""fill:#ffffff;fill-opacity:1"">
</g>
<g
   id=""g34""
   style=""fill:#ffffff;fill-opacity:1"">
</g>
<g
   id=""g36""
   style=""fill:#ffffff;fill-opacity:1"">
</g>
<g
   id=""g38""
   style=""fill:#ffffff;fill-opacity:1"">
</g>
<g
   id=""g40""
   style=""fill:#ffffff;fill-opacity:1"">
</g>
<g
   id=""g42""
   style=""fill:#ffffff;fill-opacity:1"">
</g>
<g
   id=""g44""
   style=""fill:#ffffff;fill-opacity:1"">
</g>
<g
   id=""g46""
   style=""fill:#ffffff;fill-opacity:1"">
</g>
<g
   id=""g48""
   style=""fill:#ffffff;fill-opacity:1"">
</g>
<g
   id=""g50""
   style=""fill:#ffffff;fill-opacity:1"">
</g>
<g
   id=""g52""
   style=""fill:#ffffff;fill-opacity:1"">
</g>
<g
   id=""g54""
   style=""fill:#ffffff;fill-opacity:1"">
</g>
<g
   id=""g56""
   style=""fill:#ffffff;fill-opacity:1"">
</g>
<g
   id=""g58""
   style=""fill:#ffffff;fill-opacity:1"">
</g>
<g
   id=""g60""
   style=""fill:#ffffff;fill-opacity:1"">
</g>
</svg>
";
                GenerateSVG("ControlBox/Move", svg);
            }

            private static void GenerateMinimiseSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<svg
   width=""512""
   height=""512""
   viewBox=""0 0 512 512""
   version=""1.1""
   id=""svg8""
   inkscape:version=""1.1.1 (3bf5ae0d25, 2021-09-20)""
   sodipodi:docname=""Minimise.svg""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:svg=""http://www.w3.org/2000/svg""
   xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#""
   xmlns:cc=""http://creativecommons.org/ns#""
   xmlns:dc=""http://purl.org/dc/elements/1.1/"">
  <defs
     id=""defs2"" />
  <sodipodi:namedview
     id=""base""
     pagecolor=""#232323""
     bordercolor=""#666666""
     borderopacity=""1.0""
     inkscape:pageopacity=""0""
     inkscape:pageshadow=""2""
     inkscape:zoom=""1.4""
     inkscape:cx=""215.35714""
     inkscape:cy=""300.35714""
     inkscape:document-units=""px""
     inkscape:current-layer=""layer1""
     showgrid=""false""
     units=""px""
     inkscape:window-width=""1920""
     inkscape:window-height=""1009""
     inkscape:window-x=""-8""
     inkscape:window-y=""-8""
     inkscape:window-maximized=""1""
     inkscape:document-rotation=""0""
     inkscape:pagecheckerboard=""0"" />
  <metadata
     id=""metadata5"">
    <rdf:RDF>
      <cc:Work
         rdf:about="""">
        <dc:format>image/svg+xml</dc:format>
        <dc:type
           rdf:resource=""http://purl.org/dc/dcmitype/StillImage"" />
      </cc:Work>
    </rdf:RDF>
  </metadata>
  <g
     inkscape:label=""Layer 1""
     inkscape:groupmode=""layer""
     id=""layer1""
     transform=""translate(0,-161.53332)"">
    <rect
       style=""fill:#ffffff;stroke:none;stroke-width:9.4084;stroke-linecap:round;paint-order:fill markers stroke""
       id=""rect1221""
       width=""512""
       height=""51.200001""
       x=""0""
       y=""391.93332""
       ry=""25.6"" />
  </g>
</svg>
";
                GenerateSVG("ControlBox/Minimise", svg);
            }

            private static void GenerateCloseSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<svg
   width=""512""
   height=""512""
   viewBox=""0 0 512 512""
   version=""1.1""
   id=""svg8""
   inkscape:version=""1.1.1 (3bf5ae0d25, 2021-09-20)""
   sodipodi:docname=""Close.svg""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:svg=""http://www.w3.org/2000/svg""
   xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#""
   xmlns:cc=""http://creativecommons.org/ns#""
   xmlns:dc=""http://purl.org/dc/elements/1.1/"">
  <defs
     id=""defs2"" />
  <sodipodi:namedview
     id=""base""
     pagecolor=""#232323""
     bordercolor=""#666666""
     borderopacity=""1.0""
     inkscape:pageopacity=""0""
     inkscape:pageshadow=""2""
     inkscape:zoom=""0.7""
     inkscape:cx=""-73.571429""
     inkscape:cy=""480.71429""
     inkscape:document-units=""px""
     inkscape:current-layer=""layer1""
     showgrid=""false""
     units=""px""
     inkscape:window-width=""1920""
     inkscape:window-height=""1009""
     inkscape:window-x=""-8""
     inkscape:window-y=""-8""
     inkscape:window-maximized=""1""
     inkscape:document-rotation=""0""
     inkscape:pagecheckerboard=""0"" />
  <metadata
     id=""metadata5"">
    <rdf:RDF>
      <cc:Work
         rdf:about="""">
        <dc:format>image/svg+xml</dc:format>
        <dc:type
           rdf:resource=""http://purl.org/dc/dcmitype/StillImage"" />
      </cc:Work>
    </rdf:RDF>
  </metadata>
  <g
     inkscape:label=""Layer 1""
     inkscape:groupmode=""layer""
     id=""layer1""
     transform=""translate(0,-161.53332)"">
    <g
       id=""g1431""
       transform=""matrix(0.96010753,0.96010753,-0.96010753,0.96010753,411.08936,-229.1311)"">
      <rect
         style=""fill:#ffffff;stroke:none;stroke-width:9.4084;stroke-linecap:round;paint-order:fill markers stroke""
         id=""rect1221""
         width=""512""
         height=""51.200001""
         x=""0""
         y=""391.93332""
         ry=""25.6"" />
      <rect
         style=""fill:#ffffff;stroke:none;stroke-width:9.4084;stroke-linecap:round;paint-order:fill markers stroke""
         id=""rect1221-5""
         width=""512""
         height=""51.200001""
         x=""161.53333""
         y=""-281.60001""
         ry=""25.6""
         transform=""rotate(90)"" />
    </g>
  </g>
</svg>
";
                GenerateSVG("ControlBox/Close", svg);
            }

            // GUI SVGs

            private static void GenerateUpSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<!-- Created with Inkscape (http://www.inkscape.org/) -->

<svg
   width=""512""
   height=""512""
   viewBox=""0 0 512 512""
   version=""1.1""
   id=""svg5""
   inkscape:version=""1.1.1 (3bf5ae0d25, 2021-09-20)""
   sodipodi:docname=""Up.svg""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:svg=""http://www.w3.org/2000/svg"">
  <sodipodi:namedview
     id=""namedview7""
     pagecolor=""#505050""
     bordercolor=""#eeeeee""
     borderopacity=""1""
     inkscape:pageshadow=""0""
     inkscape:pageopacity=""0""
     inkscape:pagecheckerboard=""0""
     inkscape:document-units=""px""
     showgrid=""false""
     units=""px""
     showguides=""true""
     inkscape:guide-bbox=""true""
     inkscape:zoom=""0.83007813""
     inkscape:cx=""-268.04706""
     inkscape:cy=""313.82588""
     inkscape:window-width=""2560""
     inkscape:window-height=""1009""
     inkscape:window-x=""1912""
     inkscape:window-y=""-8""
     inkscape:window-maximized=""1""
     inkscape:current-layer=""layer1"">
    <sodipodi:guide
       position=""0,346.95529""
       orientation=""-1,0""
       id=""guide925""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""256,496.33882""
       orientation=""-1,0""
       id=""guide927""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""512,420.44235""
       orientation=""-1,0""
       id=""guide929""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""-524.04706,512""
       orientation=""0,1""
       id=""guide931""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""-357.79765,0""
       orientation=""0,1""
       id=""guide933""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
  </sodipodi:namedview>
  <defs
     id=""defs2"" />
  <g
     inkscape:label=""Layer 1""
     inkscape:groupmode=""layer""
     id=""layer1"">
    <path
       style=""fill:#ffffff;stroke:none;stroke-width:1px;stroke-linecap:butt;stroke-linejoin:miter;stroke-opacity:1;fill-opacity:1""
       d=""M 0,512 256,0 512,512 H 0""
       id=""path968"" />
  </g>
</svg>
";
                GenerateSVG("GUI/Up", svg);
            }

            private static void GenerateDownSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<!-- Created with Inkscape (http://www.inkscape.org/) -->

<svg
   width=""512""
   height=""512""
   viewBox=""0 0 512 512""
   version=""1.1""
   id=""svg5""
   inkscape:version=""1.1.1 (3bf5ae0d25, 2021-09-20)""
   sodipodi:docname=""Down.svg""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:svg=""http://www.w3.org/2000/svg"">
  <sodipodi:namedview
     id=""namedview7""
     pagecolor=""#505050""
     bordercolor=""#eeeeee""
     borderopacity=""1""
     inkscape:pageshadow=""0""
     inkscape:pageopacity=""0""
     inkscape:pagecheckerboard=""0""
     inkscape:document-units=""px""
     showgrid=""false""
     units=""px""
     showguides=""true""
     inkscape:guide-bbox=""true""
     inkscape:zoom=""0.83007813""
     inkscape:cx=""-268.04706""
     inkscape:cy=""313.82588""
     inkscape:window-width=""2560""
     inkscape:window-height=""1009""
     inkscape:window-x=""1912""
     inkscape:window-y=""-8""
     inkscape:window-maximized=""1""
     inkscape:current-layer=""layer1"">
    <sodipodi:guide
       position=""0,346.95529""
       orientation=""-1,0""
       id=""guide925""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""256,496.33882""
       orientation=""-1,0""
       id=""guide927""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""512,420.44235""
       orientation=""-1,0""
       id=""guide929""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""-524.04706,512""
       orientation=""0,1""
       id=""guide931""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""-357.79765,0""
       orientation=""0,1""
       id=""guide933""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
  </sodipodi:namedview>
  <defs
     id=""defs2"" />
  <g
     inkscape:label=""Layer 1""
     inkscape:groupmode=""layer""
     id=""layer1"">
    <path
       style=""fill:#ffffff;fill-opacity:1;stroke:none;stroke-width:1px;stroke-linecap:butt;stroke-linejoin:miter;stroke-opacity:1""
       d=""M 0,0 256,512 512,0 H 0""
       id=""path968"" />
  </g>
</svg>
";
                GenerateSVG("GUI/Down", svg);
            }

            private static void GenerateInfoSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<!-- Created with Inkscape (http://www.inkscape.org/) -->

<svg
   width=""512""
   height=""512""
   viewBox=""0 0 512 512""
   version=""1.1""
   id=""svg5""
   inkscape:version=""1.1.1 (3bf5ae0d25, 2021-09-20)""
   sodipodi:docname=""Info.svg""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:svg=""http://www.w3.org/2000/svg"">
  <sodipodi:namedview
     id=""namedview7""
     pagecolor=""#505050""
     bordercolor=""#eeeeee""
     borderopacity=""1""
     inkscape:pageshadow=""0""
     inkscape:pageopacity=""0""
     inkscape:pagecheckerboard=""0""
     inkscape:document-units=""px""
     showgrid=""false""
     units=""px""
     showguides=""true""
     inkscape:guide-bbox=""true""
     inkscape:zoom=""1.138""
     inkscape:cx=""482.86467""
     inkscape:cy=""235.94025""
     inkscape:window-width=""2560""
     inkscape:window-height=""1009""
     inkscape:window-x=""-8""
     inkscape:window-y=""-8""
     inkscape:window-maximized=""1""
     inkscape:current-layer=""layer1"">
    <sodipodi:guide
       position=""0,45.980582""
       orientation=""-1,0""
       id=""guide989""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""-18.640777,0""
       orientation=""0,1""
       id=""guide991""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""512,0""
       orientation=""-1,0""
       id=""guide1069""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""256,535.72584""
       orientation=""-1,0""
       id=""guide1109""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""231.10721,443.405""
       orientation=""0,1""
       id=""guide1111""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
  </sodipodi:namedview>
  <defs
     id=""defs2"" />
  <g
     inkscape:label=""Layer 1""
     inkscape:groupmode=""layer""
     id=""layer1"">
    <path
       style=""fill:none;fill-opacity:1;stroke:#ffffff;stroke-width:7.91742;stroke-linecap:butt;stroke-linejoin:miter;stroke-miterlimit:4;stroke-dasharray:none;stroke-opacity:1""
       d=""M 6.8566886,473.74379 256,42.214922 505.14331,473.74379 Z""
       id=""path854""
       sodipodi:nodetypes=""cccc"" />
    <text
       xml:space=""preserve""
       style=""font-style:normal;font-weight:normal;font-size:421.78px;line-height:1.25;font-family:sans-serif;fill:#ffffff;fill-opacity:1;stroke:#ffffff;stroke-width:10.5445;stroke-opacity:1""
       x=""198.12881""
       y=""442.35748""
       id=""text2077""><tspan
         sodipodi:role=""line""
         id=""tspan2075""
         x=""198.12881""
         y=""442.35748""
         style=""fill:#ffffff;fill-opacity:1;stroke:#ffffff;stroke-width:10.5445;stroke-opacity:1"">i</tspan></text>
  </g>
</svg>
";
                GenerateSVG("GUI/Info", svg);
            }

            private static void GenerateWarningSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<!-- Created with Inkscape (http://www.inkscape.org/) -->

<svg
   width=""512""
   height=""512""
   viewBox=""0 0 512 512""
   version=""1.1""
   id=""svg5""
   inkscape:version=""1.1.1 (3bf5ae0d25, 2021-09-20)""
   sodipodi:docname=""Warning.svg""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:svg=""http://www.w3.org/2000/svg"">
  <sodipodi:namedview
     id=""namedview7""
     pagecolor=""#505050""
     bordercolor=""#eeeeee""
     borderopacity=""1""
     inkscape:pageshadow=""0""
     inkscape:pageopacity=""0""
     inkscape:pagecheckerboard=""0""
     inkscape:document-units=""px""
     showgrid=""false""
     units=""px""
     showguides=""true""
     inkscape:guide-bbox=""true""
     inkscape:zoom=""1.138""
     inkscape:cx=""482.86467""
     inkscape:cy=""235.94025""
     inkscape:window-width=""2560""
     inkscape:window-height=""1009""
     inkscape:window-x=""-8""
     inkscape:window-y=""-8""
     inkscape:window-maximized=""1""
     inkscape:current-layer=""layer1"">
    <sodipodi:guide
       position=""0,45.980582""
       orientation=""-1,0""
       id=""guide989""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""-18.640777,0""
       orientation=""0,1""
       id=""guide991""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""512,0""
       orientation=""-1,0""
       id=""guide1069""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""256,535.72584""
       orientation=""-1,0""
       id=""guide1109""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""231.10721,443.405""
       orientation=""0,1""
       id=""guide1111""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
  </sodipodi:namedview>
  <defs
     id=""defs2"" />
  <g
     inkscape:label=""Layer 1""
     inkscape:groupmode=""layer""
     id=""layer1"">
    <path
       style=""fill:none;fill-opacity:1;stroke:#fff00f;stroke-width:7.91742;stroke-linecap:butt;stroke-linejoin:miter;stroke-miterlimit:4;stroke-dasharray:none;stroke-opacity:1""
       d=""M 6.8566878,473.74379 256,42.214924 505.14331,473.74379 Z""
       id=""path854""
       sodipodi:nodetypes=""cccc"" />
    <text
       xml:space=""preserve""
       style=""font-style:normal;font-weight:normal;font-size:421.78px;line-height:1.25;font-family:sans-serif;fill:#fff00f;fill-opacity:1;stroke:#fff00f;stroke-width:10.5445;stroke-opacity:1""
       x=""-313.87119""
       y=""-133.64253""
       id=""text2077""
       transform=""scale(-1)""><tspan
         sodipodi:role=""line""
         id=""tspan2075""
         x=""-313.87119""
         y=""-133.64253""
         style=""fill:#fff00f;fill-opacity:1;stroke:#fff00f;stroke-width:10.5445;stroke-opacity:1"">i</tspan></text>
  </g>
</svg>
";
                GenerateSVG("GUI/Warning", svg);
            }

            private static void GenerateErrorSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<!-- Created with Inkscape (http://www.inkscape.org/) -->

<svg
   width=""512""
   height=""512""
   viewBox=""0 0 512 512""
   version=""1.1""
   id=""svg5""
   inkscape:version=""1.1.1 (3bf5ae0d25, 2021-09-20)""
   sodipodi:docname=""Error.svg""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns:xlink=""http://www.w3.org/1999/xlink""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:svg=""http://www.w3.org/2000/svg"">
  <sodipodi:namedview
     id=""namedview7""
     pagecolor=""#505050""
     bordercolor=""#eeeeee""
     borderopacity=""1""
     inkscape:pageshadow=""0""
     inkscape:pageopacity=""0""
     inkscape:pagecheckerboard=""0""
     inkscape:document-units=""px""
     showgrid=""false""
     units=""px""
     showguides=""true""
     inkscape:guide-bbox=""true""
     inkscape:zoom=""1.138""
     inkscape:cx=""481.98595""
     inkscape:cy=""235.94025""
     inkscape:window-width=""2560""
     inkscape:window-height=""1009""
     inkscape:window-x=""-8""
     inkscape:window-y=""-8""
     inkscape:window-maximized=""1""
     inkscape:current-layer=""layer1"">
    <sodipodi:guide
       position=""0,45.980582""
       orientation=""-1,0""
       id=""guide989""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""-18.640777,0""
       orientation=""0,1""
       id=""guide991""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""512,0""
       orientation=""-1,0""
       id=""guide1069""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""256,535.72584""
       orientation=""-1,0""
       id=""guide1109""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
    <sodipodi:guide
       position=""231.10721,443.405""
       orientation=""0,1""
       id=""guide1111""
       inkscape:label=""""
       inkscape:locked=""false""
       inkscape:color=""rgb(0,0,255)"" />
  </sodipodi:namedview>
  <defs
     id=""defs2"">
    <linearGradient
       inkscape:collect=""always""
       id=""linearGradient7077"">
      <stop
         style=""stop-color:#ff0000;stop-opacity:1;""
         offset=""0""
         id=""stop7073"" />
      <stop
         style=""stop-color:#ff0000;stop-opacity:0;""
         offset=""1""
         id=""stop7075"" />
    </linearGradient>
    <linearGradient
       inkscape:collect=""always""
       xlink:href=""#linearGradient7077""
       id=""linearGradient7079""
       x1=""-283.10266""
       y1=""-288.00001""
       x2=""-228.89734""
       y2=""-288.00001""
       gradientUnits=""userSpaceOnUse"" />
  </defs>
  <g
     inkscape:label=""Layer 1""
     inkscape:groupmode=""layer""
     id=""layer1"">
    <path
       style=""fill:none;fill-opacity:1;stroke:#ff0000;stroke-width:7.91742;stroke-linecap:butt;stroke-linejoin:miter;stroke-miterlimit:4;stroke-dasharray:none;stroke-opacity:1""
       d=""M 6.8566886,473.74379 256,42.214922 505.14331,473.74379 Z""
       id=""path854""
       sodipodi:nodetypes=""cccc"" />
    <text
       xml:space=""preserve""
       style=""font-style:normal;font-weight:normal;font-size:421.78px;line-height:1.25;font-family:sans-serif;fill:#ff0000;fill-opacity:1;stroke:#ff0000;stroke-width:10.5445;stroke-opacity:1""
       x=""-313.87119""
       y=""-133.64253""
       id=""text2077""
       transform=""scale(-1)""><tspan
         sodipodi:role=""line""
         id=""tspan2075""
         x=""-313.87119""
         y=""-133.64253""
         style=""fill:#ff0000;fill-opacity:1;stroke:#ff0000;stroke-width:10.5445;stroke-opacity:1"">i</tspan></text>
  </g>
</svg>
";
                GenerateSVG("GUI/Error", svg);
            }

            private static void GenerateAddSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<!-- Created with Inkscape (http://www.inkscape.org/) -->

<svg
   width=""512""
   height=""512""
   viewBox=""0 0 512 512""
   version=""1.1""
   id=""svg5""
   inkscape:version=""1.1.1 (3bf5ae0d25, 2021-09-20)""
   sodipodi:docname=""Add.svg""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:svg=""http://www.w3.org/2000/svg"">
  <sodipodi:namedview
     id=""namedview7""
     pagecolor=""#505050""
     bordercolor=""#eeeeee""
     borderopacity=""1""
     inkscape:pageshadow=""0""
     inkscape:pageopacity=""0""
     inkscape:pagecheckerboard=""0""
     inkscape:document-units=""px""
     showgrid=""false""
     units=""px""
     inkscape:zoom=""0.83007813""
     inkscape:cx=""46.381176""
     inkscape:cy=""382.49412""
     inkscape:window-width=""2560""
     inkscape:window-height=""1009""
     inkscape:window-x=""1912""
     inkscape:window-y=""-8""
     inkscape:window-maximized=""1""
     inkscape:current-layer=""layer1"" />
  <defs
     id=""defs2"" />
  <g
     inkscape:label=""Layer 1""
     inkscape:groupmode=""layer""
     id=""layer1"">
    <path
       style=""fill:none;stroke:#ffffff;stroke-width:29.5442;stroke-linecap:round;stroke-linejoin:miter;stroke-miterlimit:4;stroke-dasharray:none;stroke-opacity:1;fill-opacity:1""
       d=""M 256,14.772096 V 497.2279""
       id=""path2030"" />
    <path
       style=""fill:none;stroke:#ffffff;stroke-width:29.5442;stroke-linecap:round;stroke-linejoin:miter;stroke-miterlimit:4;stroke-dasharray:none;stroke-opacity:1;fill-opacity:1""
       d=""M 497.2279,256 H 14.772097""
       id=""path2030-0"" />
  </g>
</svg>
";
                GenerateSVG("GUI/Add", svg);
            }

            private static void GenerateEditSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<!-- Uploaded to: SVG Repo, www.svgrepo.com, Generator: SVG Repo Mixer Tools -->

<svg
   width=""800px""
   height=""800px""
   viewBox=""0 0 24 24""
   fill=""none""
   version=""1.1""
   id=""svg4""
   sodipodi:docname=""532977-Slash-pencil.svg""
   inkscape:version=""1.1.1 (3bf5ae0d25, 2021-09-20)""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:svg=""http://www.w3.org/2000/svg"">
  <defs
     id=""defs8"" />
  <sodipodi:namedview
     id=""namedview6""
     pagecolor=""#505050""
     bordercolor=""#eeeeee""
     borderopacity=""1""
     inkscape:pageshadow=""0""
     inkscape:pageopacity=""0""
     inkscape:pagecheckerboard=""0""
     showgrid=""false""
     inkscape:zoom=""0.75130096""
     inkscape:cx=""350.72496""
     inkscape:cy=""473.17922""
     inkscape:window-width=""2560""
     inkscape:window-height=""1009""
     inkscape:window-x=""1912""
     inkscape:window-y=""-8""
     inkscape:window-maximized=""1""
     inkscape:current-layer=""svg4"" />
  <path
     d=""M9.65661 17L6.99975 17L6.99975 14M6.10235 14.8974L17.4107 3.58902C18.1918 2.80797 19.4581 2.80797 20.2392 3.58902C21.0202 4.37007 21.0202 5.6364 20.2392 6.41745L8.764 17.8926C8.22794 18.4287 7.95992 18.6967 7.6632 18.9271C7.39965 19.1318 7.11947 19.3142 6.8256 19.4723C6.49475 19.6503 6.14115 19.7868 5.43395 20.0599L3 20.9998L3.78312 18.6501C4.05039 17.8483 4.18403 17.4473 4.3699 17.0729C4.53497 16.7404 4.73054 16.424 4.95409 16.1276C5.20582 15.7939 5.50466 15.4951 6.10235 14.8974Z""
     stroke=""#000000""
     stroke-width=""2""
     stroke-linecap=""round""
     stroke-linejoin=""round""
     id=""path2""
     style=""stroke:#ffffff;stroke-opacity:1"" />
</svg>
";
                GenerateSVG("GUI/Edit", svg);
            }

            private static void GenerateFolderSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<!-- Uploaded to: SVG Repo, www.svgrepo.com, Generator: SVG Repo Mixer Tools -->

<svg
   width=""800px""
   height=""800px""
   viewBox=""0 0 32 32""
   version=""1.1""
   id=""svg11""
   sodipodi:docname=""522121-Slash-folder-2.svg""
   inkscape:version=""1.1.1 (3bf5ae0d25, 2021-09-20)""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:svg=""http://www.w3.org/2000/svg""
   xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#""
   xmlns:cc=""http://creativecommons.org/ns#""
   xmlns:dc=""http://purl.org/dc/elements/1.1/""
   xmlns:sketch=""http://www.bohemiancoding.com/sketch/ns"">
  <sodipodi:namedview
     id=""namedview13""
     pagecolor=""#505050""
     bordercolor=""#eeeeee""
     borderopacity=""1""
     inkscape:pageshadow=""0""
     inkscape:pageopacity=""0""
     inkscape:pagecheckerboard=""0""
     showgrid=""false""
     inkscape:zoom=""0.53125""
     inkscape:cx=""134.58824""
     inkscape:cy=""501.64706""
     inkscape:window-width=""2560""
     inkscape:window-height=""1009""
     inkscape:window-x=""1912""
     inkscape:window-y=""-8""
     inkscape:window-maximized=""1""
     inkscape:current-layer=""svg11"" />
  <title
     id=""title2"">folder 2</title>
  <desc
     id=""desc4"">Created with Sketch Beta.</desc>
  <defs
     id=""defs6"" />
  <g
     id=""Icon-Set-Filled""
     sketch:type=""MSLayerGroup""
     transform=""translate(-362,-153)""
     fill=""#000000""
     style=""fill-rule:evenodd;stroke:none;stroke-width:1;fill:#ffffff;fill-opacity:1"">
    <path
       d=""m 390,157 h -14 c 0,-2.209 -1.791,-4 -4,-4 h -6 c -2.209,0 -4,1.791 -4,4 v 6 h 32 v -2 c 0,-2.209 -1.791,-4 -4,-4 z m -28,24 c 0,2.209 1.791,4 4,4 h 24 c 2.209,0 4,-1.791 4,-4 v -16 h -32 z""
       id=""folder-2""
       sketch:type=""MSShapeGroup""
       style=""fill:#ffffff;fill-opacity:1"" />
  </g>
  <metadata
     id=""metadata830"">
    <rdf:RDF>
      <cc:Work
         rdf:about="""">
        <dc:title>folder 2</dc:title>
      </cc:Work>
    </rdf:RDF>
  </metadata>
</svg>
";
                GenerateSVG("GUI/Folder", svg);
            }

            private static void GenerateDllSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<!-- Uploaded to: SVG Repo, www.svgrepo.com, Generator: SVG Repo Mixer Tools -->

<svg
   fill=""#000000""
   version=""1.1""
   id=""Capa_1""
   width=""800px""
   height=""800px""
   viewBox=""0 0 550.801 550.801""
   xml:space=""preserve""
   sodipodi:docname=""126981-Slash-dll-file-format-variant.svg""
   inkscape:version=""1.1.1 (3bf5ae0d25, 2021-09-20)""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:svg=""http://www.w3.org/2000/svg""><defs
   id=""defs17"" /><sodipodi:namedview
   id=""namedview15""
   pagecolor=""#505050""
   bordercolor=""#eeeeee""
   borderopacity=""1""
   inkscape:pageshadow=""0""
   inkscape:pageopacity=""0""
   inkscape:pagecheckerboard=""0""
   showgrid=""false""
   inkscape:zoom=""0.53125""
   inkscape:cx=""-208""
   inkscape:cy=""554.35294""
   inkscape:window-width=""2560""
   inkscape:window-height=""1009""
   inkscape:window-x=""1912""
   inkscape:window-y=""-8""
   inkscape:window-maximized=""1""
   inkscape:current-layer=""Capa_1"" />
<g
   id=""g12""
   style=""fill:#ffffff;fill-opacity:1"">
	<g
   id=""g10""
   style=""fill:#ffffff;fill-opacity:1"">
		<path
   d=""M171.439,410.062c-6.605,0-10.887,0.58-13.402,1.16v85.726c2.521,0.58,6.603,0.58,10.302,0.58    c26.823,0.195,44.307-14.576,44.307-45.868C212.847,424.449,196.91,410.062,171.439,410.062z""
   id=""path2""
   style=""fill:#ffffff;fill-opacity:1"" />
		<path
   d=""M475.095,131.992c-0.032-2.526-0.833-5.021-2.568-6.993L366.324,3.694c-0.021-0.034-0.053-0.045-0.084-0.076    c-0.633-0.707-1.36-1.29-2.141-1.804c-0.232-0.15-0.465-0.285-0.707-0.422c-0.686-0.366-1.393-0.67-2.131-0.892    c-0.2-0.058-0.379-0.14-0.58-0.192C359.87,0.114,359.047,0,358.203,0H97.2C85.292,0,75.6,9.693,75.6,21.601v507.6    c0,11.913,9.692,21.601,21.6,21.601H453.6c11.918,0,21.601-9.688,21.601-21.601V133.202    C475.2,132.796,475.137,132.398,475.095,131.992z M222.373,503.751c-13.809,11.464-34.794,16.906-60.455,16.906    c-15.356,0-26.238-0.97-33.626-1.94V390.034c10.887-1.74,25.073-2.721,40.047-2.721c24.877,0,41.009,4.472,53.644,13.995    c13.608,10.115,22.162,26.241,22.162,49.376C244.139,475.76,235.008,493.057,222.373,503.751z M345.432,519.297h-81.836V388.294    h29.734v106.123h52.096v24.88H345.432z M444.762,519.297h-81.833V388.294h29.742v106.123h52.091V519.297z M97.2,366.752V21.601    h250.203v110.515c0,5.961,4.831,10.8,10.8,10.8H453.6l0.011,223.836H97.2z""
   id=""path4""
   style=""fill:#ffffff;fill-opacity:1"" />
		<path
   d=""M301.725,245.015c1.382-0.546,2.484-1.619,3.027-2.911c0.538-1.292,0.522-2.829-0.042-4.103    c-1.698-3.878-0.823-8.448,2.141-11.417l7.899-7.899c1.994-1.973,4.557-3.04,7.235-3.04h1.856l2.12,0.814    c1.646,0.717,3.037,0.659,4.287,0.14c1.323-0.551,2.399-1.65,2.905-2.958c1.498-3.758,4.926-6.157,8.87-6.426v-7.362    c0-0.765-0.501-1.49-1.224-1.785c-7.788-3.046-14.096-9.213-17.297-16.936c-3.206-7.739-3.116-16.577,0.254-24.234    c0.306-0.693,0.137-1.556-0.396-2.107l-16.69-16.674c-0.918-0.923-1.599-0.633-2.12-0.412c-7.589,3.359-16.485,3.478-24.211,0.253    c-7.723-3.18-13.9-9.503-16.951-17.303c-0.279-0.706-0.983-1.197-1.764-1.197h-23.578c-0.789,0-1.5,0.491-1.783,1.197    c-3.04,7.8-9.212,14.123-16.938,17.303c-7.702,3.225-16.58,3.127-24.242-0.253c-0.48-0.195-1.166-0.511-2.101,0.412l-16.69,16.674    c-0.541,0.551-0.702,1.414-0.377,2.123c3.351,7.662,3.449,16.479,0.253,24.218c-3.204,7.723-9.511,13.885-17.318,16.936    c-0.728,0.295-1.205,0.98-1.205,1.785v23.593c0,0.783,0.478,1.469,1.205,1.767c7.808,3.046,14.099,9.213,17.318,16.933    c3.195,7.739,3.098,16.58-0.269,24.242c-0.309,0.702-0.143,1.551,0.393,2.102l16.69,16.688c0.936,0.928,1.595,0.611,2.117,0.379    c3.915-1.708,8.095-2.594,12.397-2.594c4.09,0,8.058,0.785,11.815,2.356c7.739,3.201,13.911,9.509,16.951,17.303    c0.282,0.706,1,1.202,1.783,1.202h23.578c0.793,0,1.484-0.475,1.764-1.202c3.045-7.794,9.213-14.102,16.951-17.303    c7.536-3.111,16.079-3.085,23.567-0.016c0.105-0.991,0.358-1.982,0.77-2.932c0.597-1.34,0.612-2.88,0.074-4.188    c-0.538-1.276-1.64-2.357-2.953-2.871c-3.997-1.604-6.528-5.345-6.528-9.563v-11.172    C295.26,250.338,297.791,246.592,301.725,245.015z M249.816,257.5c-25.289,0-45.853-20.569-45.853-45.863    c0-25.278,20.569-45.884,45.853-45.884c25.31,0,45.887,20.601,45.887,45.884C295.703,236.931,275.12,257.5,249.816,257.5z""
   id=""path6""
   style=""fill:#ffffff;fill-opacity:1"" />
		<path
   d=""M384.381,245.695c-1.508-3.668-1.466-7.853,0.121-11.48c0.143-0.33,0.068-0.735-0.185-0.999l-7.899-7.9    c-0.438-0.438-0.77-0.298-1.013-0.195c-3.597,1.587-7.815,1.653-11.464,0.124c-3.676-1.511-6.598-4.503-8.037-8.203    c-0.132-0.335-0.464-0.567-0.833-0.567h-11.169c-0.364,0-0.718,0.232-0.844,0.567c-1.435,3.699-4.367,6.692-8.016,8.203    c-3.655,1.529-7.863,1.474-11.486-0.124c-0.227-0.093-0.559-0.237-1.002,0.195l-7.91,7.9c-0.253,0.264-0.332,0.675-0.189,1.01    c1.613,3.628,1.64,7.802,0.121,11.47c-1.514,3.654-4.498,6.576-8.189,8.023c-0.348,0.134-0.585,0.464-0.585,0.849v11.172    c0,0.367,0.237,0.691,0.585,0.833c3.691,1.445,6.676,4.361,8.189,8.024c1.514,3.667,1.482,7.86-0.121,11.488    c-0.137,0.332-0.063,0.733,0.189,0.992l7.91,7.91c0.443,0.438,0.765,0.295,1.002,0.179c1.867-0.812,3.834-1.233,5.88-1.233    c1.925,0,3.813,0.374,5.59,1.117c3.665,1.52,6.608,4.504,8.032,8.2c0.126,0.338,0.479,0.564,0.844,0.564h11.169    c0.369,0,0.701-0.227,0.833-0.564c1.445-3.696,4.366-6.681,8.037-8.2c3.659-1.519,7.857-1.466,11.475,0.116    c0.242,0.116,0.564,0.243,1.002-0.179l7.899-7.91c0.253-0.259,0.338-0.66,0.185-0.992c-1.593-3.628-1.629-7.82-0.121-11.488    c1.519-3.663,4.504-6.579,8.211-8.024c0.332-0.142,0.564-0.466,0.564-0.833v-11.172c0-0.366-0.232-0.714-0.564-0.849    C388.885,252.271,385.899,249.35,384.381,245.695z M349.481,281.876c-11.992,0-21.716-9.742-21.716-21.731    c0-11.977,9.729-21.734,21.716-21.734c11.991,0,21.731,9.764,21.731,21.734C371.213,272.134,361.463,281.876,349.481,281.876z""
   id=""path8""
   style=""fill:#ffffff;fill-opacity:1"" />
	</g>
</g>
</svg>
";
                GenerateSVG("GUI/DLL", svg);
            }

            private static void GenerateJsonSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<!-- Uploaded to: SVG Repo, www.svgrepo.com, Generator: SVG Repo Mixer Tools -->

<svg
   fill=""#000000""
   height=""800px""
   width=""800px""
   version=""1.1""
   id=""Capa_1""
   viewBox=""0 0 58 58""
   xml:space=""preserve""
   sodipodi:docname=""154781-Slash-json-file.svg""
   inkscape:version=""1.1.1 (3bf5ae0d25, 2021-09-20)""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:svg=""http://www.w3.org/2000/svg""><defs
   id=""defs13"" /><sodipodi:namedview
   id=""namedview11""
   pagecolor=""#505050""
   bordercolor=""#eeeeee""
   borderopacity=""1""
   inkscape:pageshadow=""0""
   inkscape:pageopacity=""0""
   inkscape:pagecheckerboard=""0""
   showgrid=""false""
   inkscape:zoom=""1.0625""
   inkscape:cx=""399.52941""
   inkscape:cy=""400.47059""
   inkscape:window-width=""2560""
   inkscape:window-height=""1009""
   inkscape:window-x=""1912""
   inkscape:window-y=""-8""
   inkscape:window-maximized=""1""
   inkscape:current-layer=""Capa_1"" />
<g
   id=""g8""
   style=""fill:#ffffff;fill-opacity:1"">
	<path
   d=""M33.655,45.988c-0.232-0.31-0.497-0.533-0.793-0.67s-0.608-0.205-0.937-0.205c-0.337,0-0.658,0.063-0.964,0.191   s-0.579,0.344-0.82,0.649s-0.431,0.699-0.567,1.183c-0.137,0.483-0.21,1.075-0.219,1.777c0.009,0.684,0.08,1.267,0.212,1.75   s0.314,0.877,0.547,1.183s0.497,0.528,0.793,0.67s0.608,0.212,0.937,0.212c0.337,0,0.658-0.066,0.964-0.198s0.579-0.349,0.82-0.649   s0.431-0.695,0.567-1.183s0.21-1.082,0.219-1.784c-0.009-0.684-0.08-1.265-0.212-1.743S33.888,46.298,33.655,45.988z""
   id=""path2""
   style=""fill:#ffffff;fill-opacity:1"" />
	<path
   d=""M51.5,39V13.978c0-0.766-0.092-1.333-0.55-1.792L39.313,0.55C38.964,0.201,38.48,0,37.985,0H8.963   C7.777,0,6.5,0.916,6.5,2.926V39H51.5z M29.5,33c0,0.552-0.447,1-1,1s-1-0.448-1-1v-3c0-0.552,0.447-1,1-1s1,0.448,1,1V33z    M37.5,3.391c0-0.458,0.553-0.687,0.877-0.363l10.095,10.095C48.796,13.447,48.567,14,48.109,14H37.5V3.391z M36.5,24v-4   c0-0.551-0.448-1-1-1c-0.553,0-1-0.448-1-1s0.447-1,1-1c1.654,0,3,1.346,3,3v4c0,1.103,0.897,2,2,2c0.553,0,1,0.448,1,1   s-0.447,1-1,1c-1.103,0-2,0.897-2,2v4c0,1.654-1.346,3-3,3c-0.553,0-1-0.448-1-1s0.447-1,1-1c0.552,0,1-0.449,1-1v-4   c0-1.2,0.542-2.266,1.382-3C37.042,26.266,36.5,25.2,36.5,24z M28.5,22c0.828,0,1.5,0.672,1.5,1.5S29.328,25,28.5,25   c-0.828,0-1.5-0.672-1.5-1.5S27.672,22,28.5,22z M16.5,26c1.103,0,2-0.897,2-2v-4c0-1.654,1.346-3,3-3c0.553,0,1,0.448,1,1   s-0.447,1-1,1c-0.552,0-1,0.449-1,1v4c0,1.2-0.542,2.266-1.382,3c0.84,0.734,1.382,1.8,1.382,3v4c0,0.551,0.448,1,1,1   c0.553,0,1,0.448,1,1s-0.447,1-1,1c-1.654,0-3-1.346-3-3v-4c0-1.103-0.897-2-2-2c-0.553,0-1-0.448-1-1S15.947,26,16.5,26z""
   id=""path4""
   style=""fill:#ffffff;fill-opacity:1"" />
	<path
   d=""M6.5,41v15c0,1.009,1.22,2,2.463,2h40.074c1.243,0,2.463-0.991,2.463-2V41H6.5z M18.021,51.566   c0,0.474-0.087,0.873-0.26,1.196s-0.405,0.583-0.697,0.779s-0.627,0.333-1.005,0.41c-0.378,0.077-0.768,0.116-1.169,0.116   c-0.2,0-0.436-0.021-0.704-0.062s-0.547-0.104-0.834-0.191s-0.563-0.185-0.827-0.294s-0.487-0.232-0.67-0.369l0.697-1.107   c0.091,0.063,0.221,0.13,0.39,0.198s0.354,0.132,0.554,0.191s0.41,0.111,0.629,0.157s0.424,0.068,0.615,0.068   c0.483,0,0.868-0.094,1.155-0.28s0.439-0.504,0.458-0.95v-7.711h1.668V51.566z M25.958,52.298c-0.15,0.342-0.362,0.643-0.636,0.902   s-0.61,0.467-1.012,0.622s-0.856,0.232-1.367,0.232c-0.219,0-0.444-0.012-0.677-0.034s-0.467-0.062-0.704-0.116   c-0.237-0.055-0.463-0.13-0.677-0.226s-0.398-0.212-0.554-0.349l0.287-1.176c0.128,0.073,0.289,0.144,0.485,0.212   s0.398,0.132,0.608,0.191s0.419,0.107,0.629,0.144s0.405,0.055,0.588,0.055c0.556,0,0.982-0.13,1.278-0.39s0.444-0.645,0.444-1.155   c0-0.31-0.104-0.574-0.314-0.793s-0.472-0.417-0.786-0.595s-0.654-0.355-1.019-0.533s-0.706-0.388-1.025-0.629   s-0.583-0.526-0.793-0.854s-0.314-0.738-0.314-1.23c0-0.446,0.082-0.843,0.246-1.189s0.385-0.641,0.663-0.882   s0.602-0.426,0.971-0.554s0.759-0.191,1.169-0.191c0.419,0,0.843,0.039,1.271,0.116s0.774,0.203,1.039,0.376   c-0.055,0.118-0.118,0.248-0.191,0.39s-0.142,0.273-0.205,0.396s-0.118,0.226-0.164,0.308s-0.073,0.128-0.082,0.137   c-0.055-0.027-0.116-0.063-0.185-0.109s-0.166-0.091-0.294-0.137s-0.296-0.077-0.506-0.096s-0.479-0.014-0.807,0.014   c-0.183,0.019-0.355,0.07-0.52,0.157s-0.31,0.193-0.438,0.321s-0.228,0.271-0.301,0.431s-0.109,0.313-0.109,0.458   c0,0.364,0.104,0.658,0.314,0.882s0.47,0.419,0.779,0.588s0.647,0.333,1.012,0.492s0.704,0.354,1.019,0.581   s0.576,0.513,0.786,0.854s0.314,0.781,0.314,1.319C26.184,51.603,26.108,51.956,25.958,52.298z M35.761,51.156   c-0.214,0.647-0.511,1.185-0.889,1.613s-0.82,0.752-1.326,0.971s-1.06,0.328-1.661,0.328s-1.155-0.109-1.661-0.328   s-0.948-0.542-1.326-0.971s-0.675-0.966-0.889-1.613s-0.321-1.395-0.321-2.242s0.107-1.593,0.321-2.235s0.511-1.178,0.889-1.606   s0.82-0.754,1.326-0.978s1.06-0.335,1.661-0.335s1.155,0.111,1.661,0.335s0.948,0.549,1.326,0.978s0.675,0.964,0.889,1.606   s0.321,1.388,0.321,2.235S35.975,50.509,35.761,51.156z M45.68,54h-1.668l-3.951-6.945V54h-1.668V43.924h1.668l3.951,6.945v-6.945   h1.668V54z""
   id=""path6""
   style=""fill:#ffffff;fill-opacity:1"" />
</g>
</svg>
";
                GenerateSVG("GUI/Json", svg);
            }

            private static void GenerateMarkdownSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<!-- Uploaded to: SVG Repo, www.svgrepo.com, Generator: SVG Repo Mixer Tools -->

<svg
   fill=""#000000""
   width=""800px""
   height=""800px""
   viewBox=""0 0 1024 1024""
   class=""icon""
   version=""1.1""
   id=""svg4""
   sodipodi:docname=""332064-Slash-file-markdown.svg""
   inkscape:version=""1.1.1 (3bf5ae0d25, 2021-09-20)""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:svg=""http://www.w3.org/2000/svg"">
  <defs
     id=""defs8"" />
  <sodipodi:namedview
     id=""namedview6""
     pagecolor=""#505050""
     bordercolor=""#eeeeee""
     borderopacity=""1""
     inkscape:pageshadow=""0""
     inkscape:pageopacity=""0""
     inkscape:pagecheckerboard=""0""
     showgrid=""false""
     inkscape:zoom=""1.0625""
     inkscape:cx=""399.52941""
     inkscape:cy=""400.47059""
     inkscape:window-width=""2560""
     inkscape:window-height=""1009""
     inkscape:window-x=""1912""
     inkscape:window-y=""-8""
     inkscape:window-maximized=""1""
     inkscape:current-layer=""svg4"" />
  <path
     d=""M854.6 288.7c6 6 9.4 14.1 9.4 22.6V928c0 17.7-14.3 32-32 32H192c-17.7 0-32-14.3-32-32V96c0-17.7 14.3-32 32-32h424.7c8.5 0 16.7 3.4 22.7 9.4l215.2 215.3zM790.2 326L602 137.8V326h188.2zM426.13 600.93l59.11 132.97a16 16 0 0 0 14.62 9.5h24.06a16 16 0 0 0 14.63-9.51l59.1-133.35V758a16 16 0 0 0 16.01 16H641a16 16 0 0 0 16-16V486a16 16 0 0 0-16-16h-34.75a16 16 0 0 0-14.67 9.62L512.1 662.2l-79.48-182.59a16 16 0 0 0-14.67-9.61H383a16 16 0 0 0-16 16v272a16 16 0 0 0 16 16h27.13a16 16 0 0 0 16-16V600.93z""
     id=""path2""
     style=""fill:#ffffff;fill-opacity:1"" />
</svg>
";
                GenerateSVG("GUI/Markdown", svg);
            }

            private static void GeneratePngSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<!-- Uploaded to: SVG Repo, www.svgrepo.com, Generator: SVG Repo Mixer Tools -->

<svg
   fill=""#000000""
   height=""800px""
   width=""800px""
   version=""1.1""
   id=""Capa_1""
   viewBox=""0 0 58 58""
   xml:space=""preserve""
   sodipodi:docname=""165931-Slash-png.svg""
   inkscape:version=""1.1.1 (3bf5ae0d25, 2021-09-20)""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:svg=""http://www.w3.org/2000/svg""><defs
   id=""defs17"" /><sodipodi:namedview
   id=""namedview15""
   pagecolor=""#505050""
   bordercolor=""#eeeeee""
   borderopacity=""1""
   inkscape:pageshadow=""0""
   inkscape:pageopacity=""0""
   inkscape:pagecheckerboard=""0""
   showgrid=""false""
   inkscape:zoom=""1.0625""
   inkscape:cx=""399.52941""
   inkscape:cy=""400.47059""
   inkscape:window-width=""2560""
   inkscape:window-height=""1009""
   inkscape:window-x=""1912""
   inkscape:window-y=""-8""
   inkscape:window-maximized=""1""
   inkscape:current-layer=""Capa_1"" />
<g
   id=""g12""
   style=""fill:#ffffff;fill-opacity:1"">
	<path
   d=""M50.95,12.187L39.313,0.55C38.964,0.201,38.48,0,37.985,0H8.963C7.777,0,6.5,0.916,6.5,2.926V39h0.633l16.736-14.245   c0.397-0.337,0.986-0.314,1.355,0.055l4.743,4.743l9.795-10.727c0.181-0.198,0.434-0.315,0.703-0.325   c0.282-0.009,0.53,0.09,0.725,0.274l10,9.5l0.311,0.297V13.978C51.5,13.212,51.408,12.645,50.95,12.187z M16.5,23.638   c-3.071,0-5.569-2.498-5.569-5.569S13.429,12.5,16.5,12.5s5.569,2.498,5.569,5.569S19.571,23.638,16.5,23.638z M48.109,14H37.5   V3.391c0-0.458,0.553-0.687,0.877-0.363l10.095,10.095C48.796,13.447,48.567,14,48.109,14z""
   id=""path2""
   style=""fill:#ffffff;fill-opacity:1"" />
	<path
   d=""M31.383,30.969l4.807,4.807c0.391,0.391,0.391,1.023,0,1.414s-1.023,0.391-1.414,0L24.462,26.876L10.218,39H51.5v-7.636   L40.551,20.928L31.383,30.969z""
   id=""path4""
   style=""fill:#ffffff;fill-opacity:1"" />
	<path
   d=""M20.823,49.058c0.196-0.068,0.376-0.18,0.54-0.335s0.296-0.371,0.396-0.649c0.1-0.278,0.15-0.622,0.15-1.032   c0-0.164-0.023-0.354-0.068-0.567c-0.046-0.214-0.139-0.419-0.28-0.615c-0.142-0.196-0.34-0.36-0.595-0.492   c-0.255-0.132-0.593-0.198-1.012-0.198h-1.23v3.992h1.504C20.429,49.16,20.627,49.126,20.823,49.058z""
   id=""path6""
   style=""fill:#ffffff;fill-opacity:1"" />
	<path
   d=""M16.5,14.5c-1.968,0-3.569,1.601-3.569,3.569s1.601,3.569,3.569,3.569s3.569-1.601,3.569-3.569S18.468,14.5,16.5,14.5z""
   id=""path8""
   style=""fill:#ffffff;fill-opacity:1"" />
	<path
   d=""M7.85,41H6.5v1.08V43v13c0,1.009,1.22,2,2.463,2h40.074c1.243,0,2.463-0.991,2.463-2V41h-43H7.85z M35.302,46.679   c0.214-0.643,0.51-1.178,0.889-1.606c0.378-0.429,0.822-0.754,1.333-0.978c0.51-0.224,1.062-0.335,1.654-0.335   c0.547,0,1.057,0.091,1.531,0.273c0.474,0.183,0.897,0.456,1.271,0.82l-1.135,1.012c-0.219-0.265-0.47-0.456-0.752-0.574   c-0.283-0.118-0.574-0.178-0.875-0.178c-0.337,0-0.659,0.063-0.964,0.191c-0.306,0.128-0.579,0.344-0.82,0.649   c-0.242,0.306-0.431,0.699-0.567,1.183s-0.21,1.075-0.219,1.777c0.009,0.684,0.08,1.276,0.212,1.777   c0.132,0.501,0.314,0.911,0.547,1.23s0.497,0.556,0.793,0.711c0.296,0.155,0.608,0.232,0.937,0.232c0.1,0,0.234-0.007,0.403-0.021   c0.168-0.014,0.337-0.036,0.506-0.068c0.168-0.032,0.33-0.075,0.485-0.13c0.155-0.055,0.269-0.132,0.342-0.232v-2.488h-1.709   v-1.121H42.5v3.896c-0.21,0.265-0.444,0.48-0.704,0.649s-0.533,0.308-0.82,0.417S40.392,53.954,40.087,54   c-0.306,0.046-0.608,0.068-0.909,0.068c-0.602,0-1.155-0.109-1.661-0.328s-0.948-0.542-1.326-0.971   c-0.378-0.429-0.675-0.966-0.889-1.613c-0.214-0.647-0.321-1.395-0.321-2.242S35.087,47.321,35.302,46.679z M25.369,43.924h1.668   l3.951,6.945v-6.945h1.668V54h-1.668l-3.951-6.945V54h-1.668V43.924z M17.084,43.924h2.898c0.428,0,0.852,0.068,1.271,0.205   c0.419,0.137,0.795,0.342,1.128,0.615c0.333,0.273,0.602,0.604,0.807,0.991s0.308,0.822,0.308,1.306   c0,0.511-0.087,0.973-0.26,1.388c-0.173,0.415-0.415,0.764-0.725,1.046c-0.31,0.282-0.684,0.501-1.121,0.656   s-0.921,0.232-1.449,0.232h-1.217V54h-1.641V43.924z""
   id=""path10""
   style=""fill:#ffffff;fill-opacity:1"" />
</g>
</svg>
";
                GenerateSVG("GUI/PNG", svg);
            }

            private static void GenerateBinSVG() {
                string svg = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<!-- Uploaded to: SVG Repo, www.svgrepo.com, Generator: SVG Repo Mixer Tools -->

<svg
   width=""800px""
   height=""800px""
   viewBox=""-0.5 0 19 19""
   version=""1.1""
   id=""svg10""
   sodipodi:docname=""433921-Slash-bin.svg""
   inkscape:version=""1.1.1 (3bf5ae0d25, 2021-09-20)""
   xmlns:inkscape=""http://www.inkscape.org/namespaces/inkscape""
   xmlns:sodipodi=""http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd""
   xmlns=""http://www.w3.org/2000/svg""
   xmlns:svg=""http://www.w3.org/2000/svg""
   xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#""
   xmlns:cc=""http://creativecommons.org/ns#""
   xmlns:dc=""http://purl.org/dc/elements/1.1/""
   xmlns:sketch=""http://www.bohemiancoding.com/sketch/ns"">
  <sodipodi:namedview
     id=""namedview12""
     pagecolor=""#505050""
     bordercolor=""#eeeeee""
     borderopacity=""1""
     inkscape:pageshadow=""0""
     inkscape:pageopacity=""0""
     inkscape:pagecheckerboard=""0""
     showgrid=""false""
     inkscape:zoom=""1.0625""
     inkscape:cx=""399.52941""
     inkscape:cy=""400.47059""
     inkscape:window-width=""2560""
     inkscape:window-height=""1009""
     inkscape:window-x=""1912""
     inkscape:window-y=""-8""
     inkscape:window-maximized=""1""
     inkscape:current-layer=""svg10"" />
  <title
     id=""title2"">icon/18/icon-delete</title>
  <desc
     id=""desc4"">Created with Sketch.</desc>
  <defs
     id=""defs6"" />
  <g
     id=""out""
     stroke=""none""
     stroke-width=""1""
     fill=""none""
     fill-rule=""evenodd""
     sketch:type=""MSPage""
     style=""fill:#ffffff;fill-opacity:1"">
    <path
       d=""M4.91666667,14.8888889 C4.91666667,15.3571429 5.60416667,16 6.0625,16 L12.9375,16 C13.3958333,16 14.0833333,15.3571429 14.0833333,14.8888889 L14.0833333,6 L4.91666667,6 L4.91666667,14.8888889 L4.91666667,14.8888889 L4.91666667,14.8888889 Z M15,3.46500003 L12.5555556,3.46500003 L11.3333333,2 L7.66666667,2 L6.44444444,3.46500003 L4,3.46500003 L4,4.93000007 L15,4.93000007 L15,3.46500003 L15,3.46500003 L15,3.46500003 Z""
       id=""path""
       fill=""#000000""
       sketch:type=""MSShapeGroup""
       style=""fill:#ffffff;fill-opacity:1"" />
  </g>
  <metadata
     id=""metadata829"">
    <rdf:RDF>
      <cc:Work
         rdf:about="""">
        <dc:title>icon/18/icon-delete</dc:title>
      </cc:Work>
    </rdf:RDF>
  </metadata>
</svg>
";
                GenerateSVG("GUI/Bin", svg);
            }
        }
    }
}
