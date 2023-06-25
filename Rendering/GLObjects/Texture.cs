
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using StbImageSharp;

namespace OpenTKEngine.Rendering.GLObjects;

public sealed class Texture: GLObject
{
    private readonly TextureHandle textureHandle;

    public Texture(string path)
    {
        StbImage.stbi_set_flip_vertically_on_load(1);
        ImageResult image;

        using(Stream stream = File.OpenRead(path))
        {
            image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        }

        textureHandle = GL.GenTexture();
        SetUnit(TextureUnit.Texture0);
        GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

        //TODO: Add the ability to change all these settings.
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        GL.GenerateMipmap(TextureTarget.Texture2d);
    }

    public override ObjectIdentifier Identifier => ObjectIdentifier.Texture;

    protected override uint Handle => (uint) textureHandle.Handle;

    public void SetUnit(TextureUnit textureUnit)
    {
        GL.ActiveTexture(textureUnit);
        Bind();
    }

    public override void Bind()
    {
        GL.BindTexture(TextureTarget.Texture2d, textureHandle);
    }

    public override void Dispose()
    {
        GL.DeleteTexture(textureHandle);
    }
}
