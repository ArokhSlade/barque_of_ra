using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
[RequireComponent(typeof(RawImage))]
public class UIWaveScroller : MonoBehaviour
{
    [Header("Scroll")]
    [SerializeField] float speedTilesPerSecond = 0.05f;
    [SerializeField] bool reverse = false;

    [Header("Tiling")]
    [Min(0.01f)][SerializeField] float tilesX = 1f;
    [SerializeField] bool autoSetWrapRepeat = true;

    [Header("Editor Mode")]
    [SerializeField] bool forceRepaintInEditMode = true; // wichtig für flüssiges Playback

    RawImage img;

    #if UNITY_EDITOR
    double lastEditorTime;
    #endif

    void OnEnable()
    {
        img = GetComponent<RawImage>();
        ApplyWrapMode();
        ApplyTiling();

        #if UNITY_EDITOR
        lastEditorTime = EditorApplication.timeSinceStartup;
        EditorApplication.update += EditorTick;
        #endif
    }

    void OnDisable()
    {
        #if UNITY_EDITOR
        EditorApplication.update -= EditorTick;
        #endif
    }

    void Update()
    {
        if (!Application.isPlaying) return;
        Tick(Time.unscaledDeltaTime);
    }

    #if UNITY_EDITOR
    void EditorTick()
    {
        if (Application.isPlaying) return;

        var now = EditorApplication.timeSinceStartup;
        float dt = Mathf.Min(0.05f, (float)(now - lastEditorTime)); // clamp für gleichmäßiges Timing
        lastEditorTime = now;

        Tick(dt);

        // UI als „dirty“ markieren, damit Canvas neu baut
        if (img)
        {
            img.SetVerticesDirty();
            img.SetMaterialDirty();
        }

        if (forceRepaintInEditMode)
        {
            // Game/Scene Views aktiv neu zeichnen
            EditorApplication.QueuePlayerLoopUpdate();
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            SceneView.RepaintAll();
        }
    }
    #endif

    void Tick(float dt)
    {
        if (!img) return;
        var r = img.uvRect;
        float dir = reverse ? -1f : 1f;
        r.x = Repeat01(r.x + dir * speedTilesPerSecond * dt);
        img.uvRect = r;
    }

    void OnValidate()
    {
        if (!img) img = GetComponent<RawImage>();
        ApplyWrapMode();
        ApplyTiling();

        #if UNITY_EDITOR
        // sofortiger Refresh bei Inspector‑Änderungen
        if (!Application.isPlaying && forceRepaintInEditMode)
        {
            EditorApplication.QueuePlayerLoopUpdate();
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }
        #endif
    }

    void ApplyTiling()
    {
        if (!img) return;
        var r = img.uvRect;
        r.width = Mathf.Max(0.01f, tilesX);
        img.uvRect = r;
    }

    void ApplyWrapMode()
    {
        if (!autoSetWrapRepeat || !img || !img.texture) return;
        img.texture.wrapMode = TextureWrapMode.Mirror;
    }

    static float Repeat01(float x) => x - Mathf.Floor(x);
}
