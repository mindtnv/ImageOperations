using System.Drawing;

namespace ImageOperations.Effects
{
    public interface IEffect
    {
        Image Emit(Image source);
    }
}