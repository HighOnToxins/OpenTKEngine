
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenTKEngine.Scenes.Components;

public interface ILogicComponent {

	public void Update(FrameEventArgs obj, GameWindow window);

	public void Unload();

}
