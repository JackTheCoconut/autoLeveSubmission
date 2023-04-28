using System;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using System.Runtime.CompilerServices;
using autoLeveSubmission.Windows;
using Dalamud.Game.Gui;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using ImGuiNET;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using Lumina;
using GameObject = Dalamud.Game.ClientState.Objects.Types.GameObject;


namespace autoLeveSubmission
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "autoLeveSubmission";
        private const string CommandName = "/autoleve";


        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("autoLeveSubmission");

        private ConfigWindow ConfigWindow { get; init; }
        private MainWindow MainWindow { get; init; }

        public static int i = 0;

        public TargetManager TargetManager { get; init; }

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager, TargetManager targetManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;
            TargetManager = targetManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            ConfigWindow = new ConfigWindow(this);
            MainWindow = new MainWindow(this, goatImage);
            
            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(MainWindow);

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
            
        }

        
        public string PrintTargetID()
        {
            //0x7ff7ceb9b380;
            if (TargetManager != null)
            {
                return TargetManager.Target.DataId.ToString();
            }
            else
            {
                return "NULL";
            }
        }

        public GameObject FindTarget(ref TargetManager targetManager)
        {
            if (targetManager.Target.DataId == 1037263)
                return targetManager.Target.TargetObject;
            return null;
        }
        public void ChangeTarget(ref TargetManager targetManager)
        {
            
        }

        public void FocusOnTarget()
        {
            TargetManager.FocusTarget = TargetManager.SoftTarget;
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            
            ConfigWindow.Dispose();
            MainWindow.Dispose();
            
            this.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            MainWindow.IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }

        
        
    }
}
