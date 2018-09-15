﻿using System;
using System.Windows.Forms;

namespace Sledge.BspEditor.Components
{
    public interface IMapDocumentControl : IDisposable
    {
        bool IsFocused { get; }
        string Type { get; }
        Control Control { get; }
        string GetSerialisedSettings();
        void SetSerialisedSettings(string settings);
    }
}