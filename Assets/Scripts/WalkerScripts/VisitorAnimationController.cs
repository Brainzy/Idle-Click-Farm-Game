using DG.Tweening;
using UnityEngine;

namespace WalkerScripts
{
    public class VisitorAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator myAnimator;
        [SerializeField] private string walkAnimName;
        [SerializeField] private string idleAnimName;
        [SerializeField] private string celebrateAnimName;
        [SerializeField] private string waveAnimName;
        [SerializeField] private float minRandomWaveTime = 3;
        [SerializeField] private float maxRandomWaveTime = 5;
        [SerializeField] private WalkerOnBezierPath walkerOnBezierPath;
        [SerializeField] private Vector3 lookAtCameraRotation;

        private float celebrateAnimDuration;
        private float waveAnimDuration;
        private float waveRandomTimer;
        private float celebrateTimer;
        private float waveTimer;
        
        private enum AnimationState
        {
           Walk,
           Celebrate,
           Wave,
           Idle
        }

        private AnimationState animationState;
        
        private void UpdateAnimClipTimes()
        {
            AnimationClip[] clips = myAnimator.runtimeAnimatorController.animationClips;
            foreach(AnimationClip clip in clips)
            {
                switch(clip.name)
                {
                    case "Waving":
                        waveAnimDuration = clip.length;
                        waveTimer = waveAnimDuration;
                        break;
                    case "Celebrate":
                        celebrateAnimDuration = clip.length;
                        celebrateTimer = celebrateAnimDuration;
                        break;
                }
            }
        }

        private void Start()
        {
            UpdateAnimClipTimes();
        }

        private void UpdateAnimState(AnimationState newState)
        {
            switch (newState)
            {
                case AnimationState.Walk:
                    animationState = AnimationState.Walk;
                    break;
                case AnimationState.Celebrate:
                    animationState = AnimationState.Celebrate;
                    celebrateTimer = celebrateAnimDuration;
                    break;
                case AnimationState.Wave:
                    animationState = AnimationState.Wave;
                    waveTimer = waveAnimDuration;
                    break;
                case AnimationState.Idle:
                    animationState = AnimationState.Idle;
                    //transform.rotation = Quaternion.Euler(lookAtCameraRotation);
                    transform.DORotate(lookAtCameraRotation, 1f);
                    waveRandomTimer = Random.Range(minRandomWaveTime, maxRandomWaveTime);
                    break;
            }
        }

        private void Update()
        {
            if (animationState == AnimationState.Idle)
            {
                waveRandomTimer -= Time.deltaTime;
                if (waveRandomTimer <= 0)
                {
                    PlayWaveAnimation();
                }
            }
            if (animationState == AnimationState.Wave)
            {
                waveTimer -= Time.deltaTime;
                if (waveTimer <= 0)
                {
                    PlayIdleAnimation();
                }
            }
            if (animationState == AnimationState.Celebrate)
            {
                celebrateTimer -= Time.deltaTime;
                if (celebrateTimer <= 0)
                {
                    walkerOnBezierPath.FinishedCelebrateAnimation();
                    PlayWalkAnimation();
                }
            }
        }

        public void PlayWalkAnimation()
        {
            UpdateAnimState(AnimationState.Walk);
            myAnimator.Play(walkAnimName);
        }

        public void PlayIdleAnimation()
        {
            UpdateAnimState(AnimationState.Idle);
            myAnimator.Play(idleAnimName);
        }

        public void PlayCelebrateAnimation()
        {
            UpdateAnimState(AnimationState.Celebrate);
            myAnimator.Play(celebrateAnimName);
        }

        private void PlayWaveAnimation()
        {
            UpdateAnimState(AnimationState.Wave);
            myAnimator.Play(waveAnimName);
        }
        

    }
}
