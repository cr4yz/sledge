﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sledge.Common;
using Sledge.DataStructures.MapObjects;
using Sledge.Editor.History;
using Sledge.Editor.Properties;
using Sledge.Graphics.Helpers;
using Sledge.Settings;
using Sledge.UI;

namespace Sledge.Editor.Tools
{
    /// <summary>
    /// The decal tool creates a decal on any face that is clicked in the 3D viewport.
    /// The decal will be created with the current texture in the texture toolbar.
    /// </summary>
    class DecalTool : BaseTool
    {
        public DecalTool()
        {
            Usage = ToolUsage.View3D;
        }

        public override Image GetIcon()
        {
            return Resources.Tool_Decal;
        }

        public override string GetName()
        {
            return "Decal Tool";
        }

        public override void MouseDown(ViewportBase viewport, MouseEventArgs e)
        {
            var vp = viewport as Viewport3D;
            if (vp == null) return;

            // Get the ray that is cast from the clicked point along the viewport frustrum
            var ray = vp.CastRayFromScreen(e.X, e.Y);

            // Grab all the elements that intersect with the ray
            var hits = Document.Map.WorldSpawn.GetAllNodesIntersectingWith(ray);

            // Sort the list of intersecting elements by distance from ray origin and grab the first hit
            var hit = hits
                .Select(x => new {Item = x, Intersection = x.GetIntersectionPoint(ray)})
                .Where(x => x.Intersection != null)
                .OrderBy(x => (x.Intersection - ray.Start).VectorMagnitude())
                .FirstOrDefault();

            if (hit == null) return; // Nothing was clicked

            var textureName = "{TARGET"; // TODO texture toolbar
            var decal = new Entity(Document.Map.IDGenerator.GetNextObjectID())
                            {
                                EntityData = new EntityData
                                                 {
                                                     Name = "infodecal"
                                                 },
                                ClassName = "infodecal",
                                Colour = Colour.GetRandomBrushColour(),
                                Decal = TextureHelper.Get(textureName),
                                Origin = hit.Intersection
                            };
            decal.EntityData.Properties.Add(new Property {Key = "texture", Value = textureName});

            // Log the history in the undo stack
            var hc = new HistoryCreate("Apply decal", new[] {decal});
            Document.History.AddHistoryItem(hc);

            // Add the decal and update the viewports
            decal.Parent = Document.Map.WorldSpawn;
            Document.Map.WorldSpawn.Children.Add(decal);
            decal.UpdateBoundingBox();
            Document.UpdateDisplayLists();
        }

        public override void MouseEnter(ViewportBase viewport, EventArgs e)
        {
            // 
        }

        public override void MouseLeave(ViewportBase viewport, EventArgs e)
        {
            // 
        }

        public override void MouseUp(ViewportBase viewport, MouseEventArgs e)
        {
            // 
        }

        public override void MouseWheel(ViewportBase viewport, MouseEventArgs e)
        {
            // 
        }

        public override void MouseMove(ViewportBase viewport, MouseEventArgs e)
        {
            // 
        }

        public override void KeyPress(ViewportBase viewport, KeyPressEventArgs e)
        {
            // 
        }

        public override void KeyDown(ViewportBase viewport, KeyEventArgs e)
        {
            // 
        }

        public override void KeyUp(ViewportBase viewport, KeyEventArgs e)
        {
            // 
        }

        public override void UpdateFrame(ViewportBase viewport)
        {
            // 
        }

        public override void Render(ViewportBase viewport)
        {
            // 
        }

        public override HotkeyInterceptResult InterceptHotkey(HotkeysMediator hotkeyMessage)
        {
            return HotkeyInterceptResult.Continue;
        }
    }
}