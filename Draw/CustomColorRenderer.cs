using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

public class MultiBrush : ComObject
{
    public MultiBrush(Brush foreground, Brush highlight)
    {
        ForegroundBrush = foreground;
        HighlightBrush = highlight;
    }

    public Brush ForegroundBrush;
    public Brush HighlightBrush;
}

public class CustomColorRenderer : SharpDX.DirectWrite.TextRendererBase
{
    private RenderTarget renderTarget;
    private MultiBrush defaultBrush;

    public void AssignResources(RenderTarget renderTarget, MultiBrush defaultBrush)
    {
        this.renderTarget = renderTarget;
        this.defaultBrush = defaultBrush;
    }

    private MultiBrush sb;
    public override Result DrawGlyphRun(object clientDrawingContext, float baselineOriginX, float baselineOriginY, MeasuringMode measuringMode, GlyphRun glyphRun, GlyphRunDescription glyphRunDescription, ComObject clientDrawingEffect)
    {
        if (clientDrawingEffect != null && clientDrawingEffect is MultiBrush)
        {
            sb = (MultiBrush)clientDrawingEffect;
        }
        else
        {
            sb = defaultBrush;
        }

        try
        {
            this.renderTarget.DrawGlyphRun(new Vector2(baselineOriginX - 1f, baselineOriginY - 1f), glyphRun, sb.HighlightBrush, measuringMode);
            //// render shadow 1 px away
            this.renderTarget.DrawGlyphRun(new Vector2(baselineOriginX + 1f, baselineOriginY + 1f), glyphRun, sb.HighlightBrush, measuringMode);

            // render main text
            this.renderTarget.DrawGlyphRun(new Vector2(baselineOriginX, baselineOriginY), glyphRun, sb.ForegroundBrush, measuringMode);
            return Result.Ok;
        }
        catch
        {
            return Result.Fail;
        }
    }
}