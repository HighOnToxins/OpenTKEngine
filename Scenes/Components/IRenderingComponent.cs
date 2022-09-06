using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenTKMiniEngine.Scenes.Components;

public interface IRenderingComponent {

	public void RenderUpdate(FrameEventArgs obj, GameWindow window);

	public void Render();

	public void Unload();

}
