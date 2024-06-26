#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ImageToAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Texture2D image;
    private Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        image.wrapMode = TextureWrapMode.Clamp;
        image.filterMode = FilterMode.Point;
        var id = image.name;
        var path = "Assets/Generated/" + id + "/";
        (new System.IO.FileInfo(path)).Directory.Create(); // If the directory already exists, this method does nothing.
        AssetDatabase.Refresh();
        AssetImporter assetImporter = AssetImporter.GetAtPath("Assets/Generated/" + id);
        assetImporter.assetBundleName = id;
        assetImporter.SaveAndReimport();
        Rect[] regions = defineRegions();
        sprites = MakeMultiSprite(image, 48, regions);
        Sprite[] walkUpSprites = new Sprite[9];
        for (int i = 0; i < 9; i++)
        {
            walkUpSprites[i] = sprites[i + (9 * 0)];
        }
        Sprite[] walkLeftSprites = new Sprite[9];
        for (int i = 0; i < 9; i++)
        {
            walkLeftSprites[i] = sprites[i + (9 * 1)];
        }
        Sprite[] walkDownSprites = new Sprite[9];
        for (int i = 0; i < 9; i++)
        {
            walkDownSprites[i] = sprites[i + (9 * 2)];
        }
        Sprite[] walkRightSprites = new Sprite[9];
        for (int i = 0; i < 9; i++)
        {
            walkRightSprites[i] = sprites[i + (9 * 3)];
        }
        Sprite[] idleUp = new Sprite[1];
        idleUp[0] = sprites[0];
        Sprite[] idleLeft = new Sprite[1];
        idleLeft[0] = sprites[9];
        Sprite[] idleDown = new Sprite[1];
        idleDown[0] = sprites[18];
        Sprite[] idleRight = new Sprite[1];
        idleRight[0] = sprites[27];
        var spritesList = new List<KeyValuePair<string, Sprite[]>>
            {
                new KeyValuePair<string, Sprite[]>("Walking_Up", walkUpSprites),
                new KeyValuePair<string, Sprite[]>("Walking_Front", walkDownSprites),
                new KeyValuePair<string, Sprite[]>("Walking_Left", walkLeftSprites),
                new KeyValuePair<string, Sprite[]>("Walking_Right",walkRightSprites),
                new KeyValuePair<string, Sprite[]>("Idle_Down",idleDown),
                new KeyValuePair<string, Sprite[]>("Idle_Left",idleLeft),
                new KeyValuePair<string, Sprite[]>("Idle_Right", idleRight),
                new KeyValuePair<string, Sprite[]>("Idle_Up", idleUp)

            };

        // For testing purposes, also write to a file in the project folder
        for (int i = 0; i < sprites.Length; i++)
        {
            AssetDatabase.CreateAsset(sprites[i], path + "sprite" + i + ".asset");
        }

        //System.IO.File.WriteAllBytes(path+"spritesheet.png", bytes);
        var animations = createAnimationClip(spritesList, path);
        AnimatorOverrideController aoc = OverrideAnimator(animations);
        AssetDatabase.CreateAsset(aoc, path + "aoc.overrideController");
    }

    private List<AnimationClip> createAnimationClip(List<KeyValuePair<string, Sprite[]>> spritesList, string path)
    {
        var animations = new List<AnimationClip>();
        foreach (var sprite in spritesList)
        {
            var sprites = sprite.Value;
            AnimationClip animationClip = new AnimationClip();
            animationClip.frameRate = 12; // Set your desired frame rate
            ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[sprites.Length];

            // Calculate time based on frame rate
            float frameRate = animationClip.frameRate;
            float totalTime = sprites.Length / frameRate;

            // Add keyframes for each sprite
            for (int i = 0; i < sprites.Length; i++)
            {
                float time = i / frameRate; // Adjusted time calculation

                // Create ObjectReferenceKeyframe for each sprite
                ObjectReferenceKeyframe spriteKeyframe = new ObjectReferenceKeyframe();
                spriteKeyframe.time = time;
                spriteKeyframe.value = sprites[i];

                keyframes[i] = spriteKeyframe;
            }

            // Create an animation curve using the keyframes
            AnimationUtility.SetObjectReferenceCurve(animationClip, new EditorCurveBinding
            {
                type = typeof(SpriteRenderer),
                path = "",
                propertyName = "m_Sprite"
            }, keyframes);

            // Save the AnimationClip
            SetLooping(animationClip, true);
            AssetDatabase.CreateAsset(animationClip, path + sprite.Key + ".anim");

            animations.Add(animationClip);
        }
        return animations;
    }

    private AnimatorOverrideController OverrideAnimator(List<AnimationClip> animations)
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        foreach (var a in aoc.animationClips)
        {
            foreach (var animationClip in animations)
            {
                if (animationClip.name.Contains(a.name))
                {
                    anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, animationClip));
                }
            }

        }
        aoc.ApplyOverrides(anims);
        return aoc;
    }

    public static void SetLooping(AnimationClip clip, bool looping)
    {
        var settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = looping;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        // None of these let us avoid the need to restart Unity.
        //EditorUtility.SetDirty(clip);
        //AssetDatabase.SaveAssets();

        //var path = AssetDatabase.GetAssetPath(clip);
        //AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    }

    private Rect[] defineRegions()
    {
        Rect[] regions = new Rect[36];
        //walk up
        for (int i = 0; i < 9; i++)
        {
            regions[i] = new Rect(i * 64, 768, 64, 64);
        }
        //walk left
        for (int i = 0; i < 9; i++)
        {
            regions[i + 9] = new Rect(i * 64, 702, 64, 64);
        }
        //walk down
        for (int i = 0; i < 9; i++)
        {
            regions[i + 18] = new Rect(i * 64, 640, 64, 64);
        }
        //walk right
        for (int i = 0; i < 9; i++)
        {
            regions[i + 27] = new Rect(i * 64, 576, 64, 64);
        }
        return regions;
    }

    public Sprite[] MakeMultiSprite(
                 Texture2D spritesheet,
                 float pixelsPerUnit,
                 params Rect[] regions)
    {

        var sprites = new Sprite[regions.Length];

        for (int i = 0; i < sprites.Length; i++)
            sprites[i] = Sprite.Create(
                           spritesheet,
                           regions[i],
                           new Vector2(0.5f, 0.5f),
                           pixelsPerUnit);

        return sprites;
    }

    public Sprite[] MakeTextureArray(
             Texture2D spritesheet,
             float pixelsPerUnit,
             params Rect[] regions)
    {

        var sprites = new Sprite[regions.Length];

        for (int i = 0; i < sprites.Length; i++)
            sprites[i] = Sprite.Create(
                           spritesheet,
                           regions[i],
                           new Vector2(0.5f, 0.5f),
                           pixelsPerUnit);

        return sprites;
    }
}
#endif