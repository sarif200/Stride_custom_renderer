
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Rendering;
using Stride.Rendering.Compositing;
using Stride.Graphics;


namespace custom_renderer
{
    public class custom_renderer : SceneRendererBase
    {
        private DelegateSceneRenderer renderer;
        private Buffer VertexBuffer;
        private static VertexDeclaration VertexDeclaration = VertexPositionNormalTexture.Layout;
        private MutablePipelineState pipelineState;
        private DynamicEffectInstance myCustomShader;
        public Matrix WorldMatrix = Matrix.Identity;
        //private Matrix viewprojection = new Matrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);


        protected override void InitializeCore()
        {
            base.InitializeCore();
            myCustomShader = new DynamicEffectInstance("test_shader1");
            myCustomShader.Initialize(Context.Services);
            pipelineState = new MutablePipelineState(Context.GraphicsDevice);
            pipelineState.State.SetDefaults();
            pipelineState.State.InputElements = VertexDeclaration.CreateInputElements();
            pipelineState.State.PrimitiveType = PrimitiveType.TriangleStrip;
            pipelineState.State.BlendState = BlendStates.Default;
            pipelineState.State.RasterizerState.CullMode = CullMode.None;

        }
        protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
        {
            var graphicsDevice = drawContext.GraphicsDevice;
            var graphicsContext = drawContext.GraphicsContext;
            var commandList = drawContext.GraphicsContext.CommandList;
            myCustomShader.UpdateEffect(graphicsDevice);
            var normal = new Vector3(1, 0, 0);
            VertexBuffer = Buffer.New(graphicsDevice, new[] { new VertexPositionNormalColor(new Vector3(-0.5f, -0.5f, 0.5f), normal, Color.Red), new VertexPositionNormalColor(new Vector3(0, 0.4f, -0.5f), normal, Color.Red), new VertexPositionNormalColor(new Vector3(10, 10f, 0f), normal, Color.Red), }, BufferFlags.VertexBuffer, GraphicsResourceUsage.Immutable);



            drawContext.CommandList.Clear(drawContext.CommandList.RenderTarget, Color.Green);
            drawContext.CommandList.Clear(drawContext.CommandList.DepthStencilBuffer, DepthStencilClearOptions.DepthBuffer);

            myCustomShader.Parameters.Set(TransformationKeys.WorldViewProjection, WorldMatrix);
            myCustomShader.Parameters.Set(test_shader1Keys.Color, Color.Red);

            pipelineState.State.RootSignature = myCustomShader.RootSignature;
            pipelineState.State.EffectBytecode = myCustomShader.Effect.Bytecode;
            pipelineState.State.Output.CaptureState(commandList);
            pipelineState.Update();
            ;
            commandList.SetPipelineState(pipelineState.CurrentState);

            myCustomShader.Apply(graphicsContext);

            commandList.SetVertexBuffer(0, VertexBuffer, 0, VertexDeclaration.VertexStride);
            commandList.Draw(3);
        }

    }
}