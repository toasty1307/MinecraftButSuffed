namespace MinecraftButScuffed.Rendering;

public enum DrawOrder : int
{
    SkyBox = 0,
    World = 1,
    PostProcessing = 2,
    Gui = 3,
    Overlay = 4,
}