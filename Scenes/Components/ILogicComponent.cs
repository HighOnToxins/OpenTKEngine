
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenTKMiniEngine.Scenes.Components;

public interface ILogicComponent {

	public void Update(FrameEventArgs obj, GameWindow window);

	public void Unload();

}
