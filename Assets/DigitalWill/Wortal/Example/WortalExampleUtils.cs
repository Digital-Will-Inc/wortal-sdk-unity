using System;
using UnityEngine;

namespace DigitalWill.WortalExample
{
	/// <summary>
	/// Utility methods for the Wortal SDK example.
	/// </summary>
	public static class WortalExampleUtils
	{
        private static Texture2D _logo;

        ////////////////////////////////////////////////////////
        // The ContextPayload requires a data URL for a base64 encoded image.
        ////////////////////////////////////////////////////////

        public static string GetImage()
        {
            if (_logo == null)
            {
                _logo = Resources.Load<Texture2D>("wortal_logo");
                _logo = _logo.Decompress();
            }
            byte[] bytes = _logo.EncodeToPNG();
            string base64 = Convert.ToBase64String(bytes);
            return "data:image/png;base64," + base64;
        }

        ////////////////////////////////////////////////////////
        // It is necessary to decompress the Texture2D before calling EncodeTo() on it, as Unity does not support
        // compresses textures in these methods.
        ////////////////////////////////////////////////////////

        public static Texture2D Decompress(this Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                source.width,
                source.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
    }
}
