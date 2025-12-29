using UnityEditor;
using UnityEngine;
using CHAL.Data;

[CustomEditor(typeof(HeroXPConfig))]
/// <summary>
/// Custom editor for configuring Hero XP settings.
/// </summary>
public class HeroXpConfigEditor : Editor
{  
    private const string PrefKey_WaveSec_L1 = "HeroXpConfigEditor.WaveSec_L1";
    private const string PrefKey_WaveSec_L60 = "HeroXpConfigEditor.WaveSec_L60";
    private const string PrefKey_WaveSec_L90 = "HeroXpConfigEditor.WaveSec_L90";

    private static float s_waveSecL1 = 10f;
    private static float s_waveSecL60 = 120f;
    private static float s_waveSecL90 = 200f;

/// <summary>
/// Draws the custom inspector GUI for the HeroXPConfig object.
/// </summary>
    public override void OnInspectorGUI()
    {
        var cfg = (HeroXPConfig)target;

        // Standardfelder (LevelCap, baseXpPerStandardWave, wavesRequiredPerLevel, wavesCurve)
        DrawDefaultInspector();

        EditorGUILayout.Space(12);
        EditorGUILayout.LabelField("XP Curve (Level -> Required Waves)", EditorStyles.boldLabel);

        // Kurve im Inspector anzeigen + editierbar
        // Hinweis: CurveField ist float-basiert; wir backen später in int[].
        EditorGUI.BeginChangeCheck();

        // Viewport: X=1..LevelCap-1, Y=auto (du kannst auch fixen, z.B. 0..200)
        int maxY = 0;

        // Max aus Array
        if (cfg.wavesRequiredPerLevel != null)
        {
            for (int i = 0; i < cfg.wavesRequiredPerLevel.Length; i++)
                maxY = Mathf.Max(maxY, cfg.wavesRequiredPerLevel[i]);
        }

        // Max aus Curve (falls du Punkte hochziehst, bevor gebaked wird)
        if (cfg.wavesCurve != null && cfg.wavesCurve.length > 0)
        {
            var keys = cfg.wavesCurve.keys;
            for (int i = 0; i < keys.Length; i++)
                maxY = Mathf.Max(maxY, Mathf.CeilToInt(keys[i].value));
        }

        // padding + fallback
        maxY = Mathf.Max(1, maxY);
        int paddedMaxY = Mathf.Max(1, Mathf.CeilToInt(maxY * 1.1f)); // +10% Luft

        var viewRect = new Rect(
            0,                                // xMin
            -1,                                // yMin
            Mathf.Max(1, cfg.LevelCap),    // xRange
            paddedMaxY                         // yRange
        );

        var rect = GUILayoutUtility.GetRect(10, 250, GUILayout.ExpandWidth(true));


        cfg.wavesCurve = EditorGUI.CurveField(
            rect,
            GUIContent.none,
            cfg.wavesCurve,
            Color.green,
            viewRect
        );

        if (EditorGUI.EndChangeCheck())
        {
            // Optional: live backen, damit Array sofort synchron ist.
            BakeCurveToArray(cfg);
            EditorUtility.SetDirty(cfg);
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Rebuild Curve from Array"))
        {
            RebuildCurveFromArray(cfg);
            EditorUtility.SetDirty(cfg);
        }

        if (GUILayout.Button("Bake Curve to Array"))
        {
            BakeCurveToArray(cfg);
            EditorUtility.SetDirty(cfg);
        }
        EditorGUILayout.EndHorizontal();

        // Optional: Quick sanity
        if (cfg.wavesRequiredPerLevel == null || cfg.wavesRequiredPerLevel.Length < cfg.LevelCap)
        {
            EditorGUILayout.HelpBox(
                "wavesRequiredPerLevel should have at least LevelCap entries (Level 1->2 at index 0 ...).",
                MessageType.Warning
            );
        }

        EditorGUILayout.Space(12);
        EditorGUILayout.LabelField("Cumulative Progress", EditorStyles.boldLabel);

        EnsureArraySize(cfg);

        // Build curves
        BuildCumulativeCurves_TimeBy3Points(cfg, s_waveSecL1, s_waveSecL60, s_waveSecL90, out var cumulativeWavesCurve, out var cumulativeTimeCurve, out int maxCumWaves, out float maxCumTime);

        // Draw cumulative waves curve (read-only)
        EditorGUILayout.LabelField("Cumulative Waves to Level N", EditorStyles.miniBoldLabel);
        DrawReadOnlyCurve(
            cumulativeWavesCurve,
            cfg.LevelCap,
            maxCumWaves,
            height: 180
        );

        EditorGUILayout.Space(12);
        // Small numeric summary (super hilfreich fürs Balancing)
        DrawSummary(cfg);

        if (s_waveSecL1 < 0f)
        {
            s_waveSecL1 = EditorPrefs.GetFloat(PrefKey_WaveSec_L1, 10f);
            s_waveSecL60 = EditorPrefs.GetFloat(PrefKey_WaveSec_L60, 120f);
            s_waveSecL90 = EditorPrefs.GetFloat(PrefKey_WaveSec_L90, 200f);
        }

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.LabelField("Estimated Wave Duration (seconds)", EditorStyles.miniBoldLabel);
        s_waveSecL1 = EditorGUILayout.FloatField("Level 1", s_waveSecL1);
        s_waveSecL60 = EditorGUILayout.FloatField("Level 60", s_waveSecL60);
        s_waveSecL90 = EditorGUILayout.FloatField("Level 90", s_waveSecL90);

        // Basic clamps (no nonsense)
        s_waveSecL1 = Mathf.Max(1f, s_waveSecL1);
        s_waveSecL60 = Mathf.Max(1f, s_waveSecL60);
        s_waveSecL90 = Mathf.Max(1f, s_waveSecL90);

        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetFloat(PrefKey_WaveSec_L1, s_waveSecL1);
            EditorPrefs.SetFloat(PrefKey_WaveSec_L60, s_waveSecL60);
            EditorPrefs.SetFloat(PrefKey_WaveSec_L90, s_waveSecL90);
        }

        // Draw cumulative time curve (read-only)
        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("Cumulative Time to Level N (abstract units)", EditorStyles.miniBoldLabel);

        // Time max for view rect
        int maxTimeInt = Mathf.Max(1, Mathf.CeilToInt(maxCumTime));

        float halfTimeLevel = FindHalfTimeLevel(cumulativeTimeCurve, cfg.LevelCap);

        DrawReadOnlyCurveWithVerticalMarker(
            cumulativeTimeCurve,
            cfg.LevelCap,
            maxTimeInt,
            height: 180,
            markerLevel: halfTimeLevel,
            markerLabel: $"½ time: {halfTimeLevel}"
        );

    }

    private static void DrawReadOnlyCurve(AnimationCurve curve, int levelCap, int maxY, float height)
    {
        maxY = Mathf.Max(1, maxY);
        int paddedMaxY = Mathf.Max(1, Mathf.CeilToInt(maxY * 1.1f));

        var viewRect = new Rect(
            1,                              // xMin
            0,                              // yMin
            Mathf.Max(1, levelCap),         // xRange
            paddedMaxY                      // yRange
        );

        var rect = GUILayoutUtility.GetRect(10f, height, GUILayout.ExpandWidth(true));
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUI.CurveField(
                rect,
                GUIContent.none,
                curve,
                Color.cyan,
                viewRect
            );
        }
    }

    private static float EstimatedWaveSecondsBy3Points(int level, float secL1, float secL60, float secL90)
    {
        level = Mathf.Max(1, level);

        // Piecewise linear:
        // 1..60 : L1 -> L60
        // 60..90: L60 -> L90
        // 90+   : clamp at L90 (keine weitere Eskalation)
        if (level <= 60)
        {
            float t = (level - 1) / 59f; // 1..60 => 0..1
            return Mathf.Lerp(secL1, secL60, t);
        }
        if (level <= 90)
        {
            float t = (level - 60) / 30f; // 60..90 => 0..1
            return Mathf.Lerp(secL60, secL90, t);
        }
        return secL90;
    }

    private static void BuildCumulativeCurves_TimeBy3Points(
        HeroXPConfig cfg,
        float secL1,
        float secL60,
        float secL90,
        out AnimationCurve cumulativeWavesCurve,
        out AnimationCurve cumulativeTimeCurve,
        out int maxCumWaves,
        out float maxCumTime
    )
    {
        int cap = Mathf.Max(1, cfg.LevelCap);
        EnsureArraySize(cfg);

        var waveKeys = new Keyframe[cap];
        var timeKeys = new Keyframe[cap];

        int cumWaves = 0;
        float cumTime = 0f;

        maxCumWaves = 0;
        maxCumTime = 0f;

        for (int level = 1; level <= cap; level++)
        {
            waveKeys[level - 1] = new Keyframe(level, cumWaves);
            timeKeys[level - 1] = new Keyframe(level, cumTime);

            maxCumWaves = Mathf.Max(maxCumWaves, cumWaves);
            maxCumTime = Mathf.Max(maxCumTime, cumTime);

            if (level <= cap - 1)
            {
                int wavesForStep = Mathf.Max(0, cfg.wavesRequiredPerLevel[level - 1]);
                float secPerWave = EstimatedWaveSecondsBy3Points(level, secL1, secL60, secL90);

                cumWaves += wavesForStep;
                cumTime += wavesForStep * secPerWave;
            }
        }

        cumulativeWavesCurve = new AnimationCurve(waveKeys);
        cumulativeTimeCurve = new AnimationCurve(timeKeys);

        for (int i = 0; i < cumulativeWavesCurve.length; i++)
            cumulativeWavesCurve.SmoothTangents(i, 0f);

        for (int i = 0; i < cumulativeTimeCurve.length; i++)
            cumulativeTimeCurve.SmoothTangents(i, 0f);
    }

    private static float SumTime_TimeBy3Points(
        HeroXPConfig cfg,
        int startLevel,
        int endLevel,
        float secL1,
        float secL60,
        float secL90
    )
    {
        EnsureArraySize(cfg);
        int cap = Mathf.Max(1, cfg.LevelCap);

        startLevel = Mathf.Clamp(startLevel, 1, cap);
        endLevel = Mathf.Clamp(endLevel, 1, cap);
        if (endLevel <= startLevel) return 0f;

        float sum = 0f;
        for (int lvl = startLevel; lvl < endLevel; lvl++)
        {
            int wavesForStep = Mathf.Max(0, cfg.wavesRequiredPerLevel[lvl - 1]);
            float secPerWave = EstimatedWaveSecondsBy3Points(lvl, secL1, secL60, secL90);
            sum += wavesForStep * secPerWave;
        }
        return sum;
    }

    private static void DrawSummary(HeroXPConfig cfg)
    {
        int cap = Mathf.Max(1, cfg.LevelCap);

        int lvlA = 80;
        int lvlB = 100;

        lvlA = Mathf.Clamp(lvlA, 1, cap);
        lvlB = Mathf.Clamp(lvlB, 1, cap);

        int wavesToA = SumWaves(cfg, 1, lvlA);
        int wavesToB = SumWaves(cfg, 1, lvlB);
        int tailWaves = SumWaves(cfg, lvlA, lvlB);

        float timeToA = SumTime_TimeBy3Points(cfg, 1, lvlA, s_waveSecL1, s_waveSecL60, s_waveSecL90) /60/60;
        float timeToB = SumTime_TimeBy3Points(cfg, 1, lvlB, s_waveSecL1, s_waveSecL60, s_waveSecL90) /60 /60;
        float tailTime = SumTime_TimeBy3Points(cfg, lvlA, lvlB, s_waveSecL1, s_waveSecL60, s_waveSecL90) /60 /60;

        EditorGUILayout.HelpBox(
            $"Waves: 1→{lvlA}: {wavesToA} | 1→{lvlB}: {wavesToB} | {lvlA}→{lvlB}: {tailWaves}\n" +
            $"Time (abstract): 1→{lvlA}: {timeToA:0.0}h |  {lvlA}→{lvlB}: {tailTime:0.0}h | 1→{lvlB}: {timeToB:0.0}h \n" +
            $"Tail ratio (time): {(timeToA > 0f ? (tailTime / timeToA) : 0f):0.00}x of 1→{lvlA}",
            MessageType.Info
        );
    }

    private static int SumWaves(HeroXPConfig cfg, int startLevel, int endLevel)
    {
        EnsureArraySize(cfg);
        int cap = Mathf.Max(1, cfg.LevelCap);

        startLevel = Mathf.Clamp(startLevel, 1, cap);
        endLevel = Mathf.Clamp(endLevel, 1, cap);

        if (endLevel <= startLevel) return 0;

        int sum = 0;
        for (int lvl = startLevel; lvl < endLevel; lvl++)
        {
            // lvl -> lvl+1 maps to index lvl-1
            sum += Mathf.Max(0, cfg.wavesRequiredPerLevel[lvl - 1]);
        }
        return sum;
    }

    private static float FindHalfTimeLevel(AnimationCurve cumulativeTimeCurve, int levelCap)
    {
        if (cumulativeTimeCurve == null || cumulativeTimeCurve.length == 0)
            return Mathf.Clamp(levelCap * 0.5f, 1f, levelCap);

        float total = cumulativeTimeCurve.Evaluate(levelCap);
        float half = total * 0.5f;

        // Wir gehen durch Keys (monoton steigend) und suchen den ersten >= half
        var keys = cumulativeTimeCurve.keys;
        for (int i = 1; i < keys.Length; i++)
        {
            float prevV = keys[i - 1].value;
            float currV = keys[i].value;

            if (currV >= half)
            {
                float prevL = keys[i - 1].time;
                float currL = keys[i].time;

                float denom = (currV - prevV);
                if (Mathf.Abs(denom) < 1e-6f)
                    return Mathf.Clamp(currL, 1f, levelCap);

                float t = (half - prevV) / denom; // 0..1
                return Mathf.Clamp(Mathf.Lerp(prevL, currL, t), 1f, levelCap);
            }
        }

        return levelCap;
    }

    private static void DrawReadOnlyCurveWithVerticalMarker(
        AnimationCurve curve,
        int levelCap,
        int maxY,
        float height,
        float markerLevel,
        string markerLabel
    )
    {
        maxY = Mathf.Max(1, maxY);
        int paddedMaxY = Mathf.Max(1, Mathf.CeilToInt(maxY * 1.1f));

        // gleiche View wie vorher, nur wir brauchen xMin/xRange auch fürs Mapping
        float xMin = 1f;
        float xRange = Mathf.Max(1, levelCap);

        var viewRect = new Rect(
            xMin,                   // xMin
            0f,                     // yMin
            xRange,                 // xRange
            paddedMaxY              // yRange
        );

        var rect = GUILayoutUtility.GetRect(10f, height, GUILayout.ExpandWidth(true));

        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUI.CurveField(
                rect,
                GUIContent.none,
                curve,
                Color.cyan,
                viewRect
            );
        }

        // Overlay: senkrechte Linie bei markerLevel
        markerLevel = Mathf.Clamp(markerLevel, xMin, xMin + xRange);

        float nx = (markerLevel - xMin) / xRange;     // 0..1
        float px = rect.x + nx * rect.width;

        Handles.BeginGUI();
        var old = Handles.color;
        Handles.color = new Color(1f, 1f, 1f, 0.6f); // leicht transparent
        Handles.DrawLine(new Vector3(px, rect.y), new Vector3(px, rect.yMax));
        Handles.color = old;
        Handles.EndGUI();

        // Label (optional)
        if (!string.IsNullOrEmpty(markerLabel))
        {
            var labelRect = new Rect(px + 4f, rect.y + 2f, 80f, 18f);
            GUI.Label(labelRect, markerLabel, EditorStyles.miniLabel);
        }
    }


    private static void RebuildCurveFromArray(HeroXPConfig cfg)
    {
        EnsureArraySize(cfg);

        int maxLevel = Mathf.Max(1, cfg.LevelCap);
        int points = Mathf.Max(0, maxLevel - 1); // L1->2 ... L99->100
        var keys = new Keyframe[points];

        for (int lvl = 1; lvl <= points; lvl++)
        {
            int waves = Mathf.Max(0, cfg.wavesRequiredPerLevel[lvl - 1]);
            keys[lvl - 1] = new Keyframe(lvl, waves);
        }

        cfg.wavesCurve = new AnimationCurve(keys);
        // Optional: Smooth tangents
        for (int i = 0; i < cfg.wavesCurve.length; i++)
            cfg.wavesCurve.SmoothTangents(i, 0f);
    }

    private static void BakeCurveToArray(HeroXPConfig cfg)
    {
        EnsureArraySize(cfg);

        int maxLevel = Mathf.Max(1, cfg.LevelCap);
        int points = Mathf.Max(0, maxLevel - 1);

        // Wir ignorieren X-Positions-Drift und sampeln strikt bei integer Levels.
        for (int lvl = 1; lvl <= points; lvl++)
        {
            float y = cfg.wavesCurve != null ? cfg.wavesCurve.Evaluate(lvl) : 0f;
            int waves = Mathf.Max(0, Mathf.RoundToInt(y));
            cfg.wavesRequiredPerLevel[lvl - 1] = waves;
        }
    }

    private static void EnsureArraySize(HeroXPConfig cfg)
    {
        int need = Mathf.Max(1, cfg.LevelCap);
        if (cfg.wavesRequiredPerLevel == null || cfg.wavesRequiredPerLevel.Length < need)
        {
            var old = cfg.wavesRequiredPerLevel ?? new int[0];
            var arr = new int[need];
            for (int i = 0; i < Mathf.Min(old.Length, arr.Length); i++)
                arr[i] = old[i];
            cfg.wavesRequiredPerLevel = arr;
        }

        if (cfg.wavesCurve == null || cfg.wavesCurve.length == 0)
        {
            // Initiale Kurve aus Array bauen
            // (damit nicht leer)
            int points = Mathf.Max(0, need - 1);
            var keys = new Keyframe[points];
            for (int lvl = 1; lvl <= points; lvl++)
                keys[lvl - 1] = new Keyframe(lvl, Mathf.Max(0, cfg.wavesRequiredPerLevel[lvl - 1]));
            cfg.wavesCurve = new AnimationCurve(keys);
        }
    }

}
