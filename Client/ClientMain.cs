using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace LoginTransition.Client
{
    public class ClientMain : BaseScript
    {
        private const float CloudOpacity = 0.01f;
        private const bool MuteSound = true;

        public ClientMain()
        {
            Main();
        }

        [Tick]
        public Task OnTick()
        {
            return Task.FromResult(0);
        }

        private static void SoundToggle(bool state)
        {
            if (state)
            {
                StartAudioScene("MP_LEADERBOARD_SCENE");
            }
            else
            {
                StopAudioScene("MP_LEADERBOARD_SCENE");
            }
        }

        private static void Initialize()
        {
            SetManualShutdownLoadingScreenNui(true);
            SoundToggle(MuteSound);

            if (!IsPlayerSwitchInProgress())
            {
                SwitchOutPlayer(PlayerPedId(), 0, 1);
            }
        }

        private static void ClearScreen()
        {
            SetCloudHatOpacity(CloudOpacity);
            HideHudAndRadarThisFrame();
            
            SetDrawOrigin(0.0f, 0.0f, 0.0f, 0);
        }
        
        private static async void Main()
        {
            Initialize();

            while (GetPlayerSwitchState() != 5)
            {
                await Delay(0);
                ClearScreen();
            }

            ShutdownLoadingScreen();

            ClearScreen();
            await Delay(0);
            DoScreenFadeOut(0);

            ShutdownLoadingScreenNui();

            ClearScreen();
            await Delay(0);
            ClearScreen();
            DoScreenFadeIn(500);

            while (!IsScreenFadedIn())
            {
                await Delay(0);
                ClearScreen();
            }

            var gameTimer = GetGameTimer();

            SoundToggle(false);
            
            while (true)
            {
                ClearScreen();
                await Delay(0);

                if (GetGameTimer() - gameTimer <= 5000) continue;
                
                SwitchInPlayer(PlayerPedId());
                ClearScreen();

                while (GetPlayerSwitchState() != 12)
                {
                    await Delay(0);
                    ClearScreen();
                }

                break;
            }

            ClearDrawOrigin();
        }
    }
}