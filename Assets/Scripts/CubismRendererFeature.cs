using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CubismRendererFeature : ScriptableRendererFeature
{
    class CubismPass : ScriptableRenderPass
    {
        public Material material;
        public int tiles = 64;
        public float jitter = 0.35f;
        public float rotation = 1.2f;
        public float colorNoise = 0.15f;
        public float seed = 0f;

        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTex;

        public CubismPass()
        {
            tempTex.Init("_TempCubismTex");
        }

        public void Setup(RenderTargetIdentifier src)
        {
            source = src;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null) return;

            CommandBuffer cmd = CommandBufferPool.Get("CubismEffect");

            material.SetFloat("_TileCount", tiles);
            material.SetFloat("_Jitter", jitter);
            material.SetFloat("_Rotation", rotation);
            material.SetFloat("_ColorNoise", colorNoise);
            material.SetFloat("_Seed", seed);

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            cmd.GetTemporaryRT(tempTex.id, opaqueDesc);
            Blit(cmd, source, tempTex.Identifier(), material, 0);
            Blit(cmd, tempTex.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (cmd == null) return;
            cmd.ReleaseTemporaryRT(tempTex.id);
        }
    }

    [System.Serializable]
    public class CubismSettings
    {
        public Material material;
        [Range(4, 512)] public int tiles = 64;
        [Range(0f, 1f)] public float jitter = 0.35f;
        [Range(0f, 6.28318f)] public float rotation = 1.2f;
        [Range(0f, 1f)] public float colorNoise = 0.15f;
        public float seed = 0f;
    }

    public CubismSettings settings = new CubismSettings();
    CubismPass pass;

    public override void Create()
    {
        pass = new CubismPass
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents,
            material = settings.material,
            tiles = settings.tiles,
            jitter = settings.jitter,
            rotation = settings.rotation,
            colorNoise = settings.colorNoise,
            seed = settings.seed
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.material == null) return;
        pass.material = settings.material;
        pass.tiles = settings.tiles;
        pass.jitter = settings.jitter;
        pass.rotation = settings.rotation;
        pass.colorNoise = settings.colorNoise;
        pass.seed = settings.seed;

        pass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(pass);
    }
}
