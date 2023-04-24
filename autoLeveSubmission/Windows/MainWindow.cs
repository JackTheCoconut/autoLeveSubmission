﻿using System;
using System.Numerics;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;


namespace autoLeveSubmission.Windows;


public class MainWindow : Window, IDisposable
{
    private TextureWrap GoatImage;
    private Plugin Plugin;

    public MainWindow(Plugin plugin, TextureWrap goatImage) : base(
        "My Amazing Window", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.GoatImage = goatImage;
        this.Plugin = plugin;
    }

    public void Dispose()
    {
        this.GoatImage.Dispose();
    }

    
    public override void Draw()
    {
       
        ImGui.Text($"The random config bool is {this.Plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");
        
        
        

        ImGui.Text("----------------------------");
        try
        {
            ImGui.Text("i'm trying");
            Plugin.PrintTargetID();
        }
        catch(Exception e)
        {
            Plugin.i += 1;
            ImGui.Text(Plugin.i.ToString());
            ImGui.Text(e.Message);
        }
        ImGui.Text("----------------------------");


        ImGui.Spacing();
        ImGui.Text("Have a goat:");
        ImGui.Indent(55);
        ImGui.Image(this.GoatImage.ImGuiHandle, new Vector2(this.GoatImage.Width, this.GoatImage.Height));
        ImGui.Unindent(55);
        
        //testing
        
    }
}
