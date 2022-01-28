using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using TRexGame.Entities;
using TRexGame.Graphics;
using TRexGame.System;
using TRexGame.Extentions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace TRexGame
{

    public class TRexRunnerGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private const string ASSET_NAME_SPRITESHEET = "TrexSpritesheet";
        private const string ASSET_GUY_RUNNING_SPRITESHEET = "Running_guy_spritesheet";
        private const string ASSET_OFFICE_FURNITURE_SPRITESHEET = "Office_Furniture_SpriteSheet";
        private const string ASSET_CANNONS_SPRITESHEET = "CannonsSpriteSheet";
        private const string ASSET_NAME_SFX_HIT = "hit";
        private const string ASSET_NAME_SFX_SCORE_REACHED = "score-reached";
        private const string ASSET_NAME_SFX_BUTTON_PRESS = "button-press";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS = "Korporaciju_Patrankos";
        private const string ASSET_NAME_CANNON_SHOT = "Cannon_Shot";
        private const string ASSET_NAME_CROWD_CHANTING = "Crowd_Chanting";

        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND = "Korp_Patrank_Game_Background_v1";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V2 = "Korp_Patrank_Game_Background_v2";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V3 = "Korp_Patrank_Game_Background_v3";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V4 = "Korp_Patrank_Game_Background_v4";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V5 = "Korp_Patrank_Game_Background_v5";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V6 = "Korp_Patrank_Game_Background_v6";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V7 = "Korp_Patrank_Game_Background_v7";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V8 = "Korp_Patrank_Game_Background_v8";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V9 = "Korp_Patrank_Game_Background_v9";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V10 = "Korp_Patrank_Game_Background_v10";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V11 = "Korp_Patrank_Game_Background_v11";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V12 = "Korp_Patrank_Game_Background_v12";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V13 = "Korp_Patrank_Game_Background_v13";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V14 = "Korp_Patrank_Game_Background_v14";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V15 = "Korp_Patrank_Game_Background_v15";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V16 = "Korp_Patrank_Game_Background_v16";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V17 = "Korp_Patrank_Game_Background_v17";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V18 = "Korp_Patrank_Game_Background_v18";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V19 = "Korp_Patrank_Game_Background_v19";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V20 = "Korp_Patrank_Game_Background_v20";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V21 = "Korp_Patrank_Game_Background_v21";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V22 = "Korp_Patrank_Game_Background_v22";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V23 = "Korp_Patrank_Game_Background_v23";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V24 = "Korp_Patrank_Game_Background_v24";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V25 = "Korp_Patrank_Game_Background_v25";

        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON = "Korp_Patrank_Game Background_cannon_game_v1";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V2 = "Korp_Patrank_Game Background_cannon_game_v2";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V3 = "Korp_Patrank_Game Background_cannon_game_v3";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V4 = "Korp_Patrank_Game Background_cannon_game_v4";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V5 = "Korp_Patrank_Game Background_cannon_game_v5";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V6 = "Korp_Patrank_Game Background_cannon_game_v6";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V7 = "Korp_Patrank_Game Background_cannon_game_v7";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V8 = "Korp_Patrank_Game Background_cannon_game_v8";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V9 = "Korp_Patrank_Game Background_cannon_game_v9";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V10 = "Korp_Patrank_Game Background_cannon_game_v10";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V11 = "Korp_Patrank_Game Background_cannon_game_v11";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V12 = "Korp_Patrank_Game Background_cannon_game_v12";
        
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_GAME_V1 = "Korp_Patrank_Game_Background_cannon_game_chanting_v1";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_GAME_V2 = "Korp_Patrank_Game_Background_cannon_game_chanting_v2";

        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V1 = "End_Anim_Korp_Patrank_Game_Background_cannon_v1";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V2 = "End_Anim_Korp_Patrank_Game_Background_cannon_v2";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V3 = "End_Anim_Korp_Patrank_Game_Background_cannon_v3";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V4 = "End_Anim_Korp_Patrank_Game_Background_cannon_v4";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V5 = "End_Anim_Korp_Patrank_Game_Background_cannon_v5";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V6 = "End_Anim_Korp_Patrank_Game_Background_cannon_v6";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V7 = "End_Anim_Korp_Patrank_Game_Background_cannon_v7";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V8 = "End_Anim_Korp_Patrank_Game_Background_cannon_v8";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V9 = "End_Anim_Korp_Patrank_Game_Background_cannon_v9";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V10 = "End_Anim_Korp_Patrank_Game_Background_cannon_v10";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V11 = "End_Anim_Korp_Patrank_Game_Background_cannon_v11";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V12 = "End_Anim_Korp_Patrank_Game_Background_cannon_v12";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V13 = "End_Anim_Korp_Patrank_Game_Background_cannon_v13";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V14 = "End_Anim_Korp_Patrank_Game_Background_cannon_v14";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V15 = "End_Anim_Korp_Patrank_Game_Background_cannon_v15";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V16 = "End_Anim_Korp_Patrank_Game_Background_cannon_v16";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V17 = "End_Anim_Korp_Patrank_Game_Background_cannon_v17";
        private const string ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V18 = "End_Anim_Korp_Patrank_Game_Background_cannon_v18";


        public const int GAME_WINDOW_WIDTH = 600;
        public const int GAME_WINDOW_HEIGHT = 150;

        public const int TREX_START_POS_Y = GAME_WINDOW_HEIGHT - 16;
        public const int TREX_START_POS_X = 1;

        private const int SCORE_BOARD_POS_X = GAME_WINDOW_WIDTH - 130;
        private const int SCORE_BOARD_POS_Y = 10;

        public const int NUMBER_3_SPRITE_POS_X = 685;
        public const int NUMBER_2_SPRITE_POS_X = 675;
        public const int NUMBER_1_SPRITE_POS_X = 665;
        public const int NUMBER_0_SPRITE_POS_X = 655;
        public const int NUMBER_SPRITE_POS_Y = 2;
        public const int NUMBER_SPRITE_WIDTH = 9;
        public const int NUMBER_SPRITE_HEIGHT = 11;


        private const float FADE_IN_ANIMATION_SPEED = 820f;
        private const string SAVE_FILE_NAME = "Save.dat";
        private SoundEffect _sfxHit;
        private SoundEffect _sfxButtonPress;
        private SoundEffect _sfxScoreReached;
        private SoundEffect _korporacijuPatrankos;
        private SoundEffect _cannonShot;
        private SoundEffect _crowdChanting;
        public float _korporacijuPatrankosSongElapsed { get; set; } = 0f;

        private Texture2D _spriteSheetTexture;
        private Texture2D _runningGuySheetTexture;
        private Texture2D _officeFurnitureSheetTexture;
        private Texture2D _cannonsSheetTexture;


        private Texture2D _korpPatrankosBackground;
        private Texture2D _korpPatrankosBackgroundv2;
        private Texture2D _korpPatrankosBackgroundv3;
        private Texture2D _korpPatrankosBackgroundv4;
        private Texture2D _korpPatrankosBackgroundv5;
        private Texture2D _korpPatrankosBackgroundv6;
        private Texture2D _korpPatrankosBackgroundv7;
        private Texture2D _korpPatrankosBackgroundv8;
        private Texture2D _korpPatrankosBackgroundv9;
        private Texture2D _korpPatrankosBackgroundv10;
        private Texture2D _korpPatrankosBackgroundv11;
        private Texture2D _korpPatrankosBackgroundv12;
        private Texture2D _korpPatrankosBackgroundv13;
        private Texture2D _korpPatrankosBackgroundv14;
        private Texture2D _korpPatrankosBackgroundv15;
        private Texture2D _korpPatrankosBackgroundv16;
        private Texture2D _korpPatrankosBackgroundv17;
        private Texture2D _korpPatrankosBackgroundv18;
        private Texture2D _korpPatrankosBackgroundv19;
        private Texture2D _korpPatrankosBackgroundv20;
        private Texture2D _korpPatrankosBackgroundv21;
        private Texture2D _korpPatrankosBackgroundv22;
        private Texture2D _korpPatrankosBackgroundv23;
        private Texture2D _korpPatrankosBackgroundv24;
        private Texture2D _korpPatrankosBackgroundv25;
        private List<Texture2D> _korpBackgrounds;

        private Texture2D _korpPatrankosBeginCannonBackground;
        private Texture2D _korpPatrankosBeginCannonBackgroundv2;
        private Texture2D _korpPatrankosBeginCannonBackgroundv3;
        private Texture2D _korpPatrankosBeginCannonBackgroundv4;
        private Texture2D _korpPatrankosBeginCannonBackgroundv5;
        private Texture2D _korpPatrankosBeginCannonBackgroundv6;
        private Texture2D _korpPatrankosBeginCannonBackgroundv7;
        private Texture2D _korpPatrankosBeginCannonBackgroundv8;
        private Texture2D _korpPatrankosBeginCannonBackgroundv9;
        private Texture2D _korpPatrankosBeginCannonBackgroundv10;
        private Texture2D _korpPatrankosBeginCannonBackgroundv11;
        private Texture2D _korpPatrankosBeginCannonBackgroundv12;
        private List<Texture2D> _korpBackgroundsBeginCannon;

        private Texture2D _korpPatrankosCannonGameBackgroundv1;
        private Texture2D _korpPatrankosCannonGameBackgroundv2;
        private List<Texture2D> _korpBackgroundsCannonGame;

        private Texture2D _korpPatrankosCannonGameEndBackgroundv1;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv2;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv3;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv4;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv5;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv6;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv7;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv8;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv9;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv10;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv11;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv12;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv13;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv14;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv15;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv16;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv17;
        private Texture2D _korpPatrankosCannonGameEndBackgroundv18;
        private List<Texture2D> _korpBackgroundsCannonGameEnd;

        private Texture2D _fadeInTexture;
        private Texture2D _invertedSpriteSheet;

        private float _fadeInTexturePosX;

        private TRex _trex;
        private InputController _inputController;
        private EntityManager _entityManager;
        private GroundManager _groundManager;
        private BackgroundManager _backgroundManager;
        private ObstacleManager _obstacleManager;
        private SkyManager _skyManager;
        private GameOverScreen _gameOverScreen;

        private ScoreBoard _scoreboard;

        private KeyboardState _previousKeyboardState;

        private SpriteAnimation _countDownAnimation;
        private Sprite _numberFinishSprite;
        private SoundEffectInstance _sei;

        private DateTime _highscoreDate;
        public GameState State { get; private set; }

        public TRexRunnerGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _entityManager = new EntityManager();
            State = GameState.Initial;
            _fadeInTexturePosX = TRex.COLLISION_SPRITE_WIDTH;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            _graphics.PreferredBackBufferHeight = GAME_WINDOW_HEIGHT;
            _graphics.PreferredBackBufferWidth = GAME_WINDOW_WIDTH;
            _graphics.ApplyChanges();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _sfxButtonPress = Content.Load<SoundEffect>(ASSET_NAME_SFX_BUTTON_PRESS);
            _sfxHit = Content.Load<SoundEffect>(ASSET_NAME_SFX_HIT);
            _sfxScoreReached = Content.Load<SoundEffect>(ASSET_NAME_SFX_SCORE_REACHED);
            _korporacijuPatrankos = Content.Load<SoundEffect>(ASSET_NAME_KORPORACIJU_PATRANKOS);
            _sei = _korporacijuPatrankos.CreateInstance();
            _cannonShot = Content.Load<SoundEffect>(ASSET_NAME_CANNON_SHOT);
            _crowdChanting = Content.Load<SoundEffect>(ASSET_NAME_CROWD_CHANTING);

            _spriteSheetTexture = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET);
            _runningGuySheetTexture = Content.Load<Texture2D>(ASSET_GUY_RUNNING_SPRITESHEET);
            _officeFurnitureSheetTexture = Content.Load<Texture2D>(ASSET_OFFICE_FURNITURE_SPRITESHEET);
            _cannonsSheetTexture = Content.Load<Texture2D>(ASSET_CANNONS_SPRITESHEET);

            _korpPatrankosBackground = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND);
            _korpPatrankosBackgroundv2 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V2);
            _korpPatrankosBackgroundv3 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V3);
            _korpPatrankosBackgroundv4 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V4);
            _korpPatrankosBackgroundv5 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V5);
            _korpPatrankosBackgroundv6 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V6);
            _korpPatrankosBackgroundv7 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V7);
            _korpPatrankosBackgroundv8 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V8);
            _korpPatrankosBackgroundv9 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V9);
            _korpPatrankosBackgroundv10 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V10);
            _korpPatrankosBackgroundv11 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V11);
            _korpPatrankosBackgroundv12 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V12);
            _korpPatrankosBackgroundv13 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V13);
            _korpPatrankosBackgroundv14 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V14);
            _korpPatrankosBackgroundv15 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V15);
            _korpPatrankosBackgroundv16 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V16);
            _korpPatrankosBackgroundv17 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V17);
            _korpPatrankosBackgroundv18 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V18);
            _korpPatrankosBackgroundv19 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V19);
            _korpPatrankosBackgroundv20 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V20);
            _korpPatrankosBackgroundv21 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V21);
            _korpPatrankosBackgroundv22 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V22);
            _korpPatrankosBackgroundv23 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V23);
            _korpPatrankosBackgroundv24 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V24);
            _korpPatrankosBackgroundv25 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_V25);
            _korpBackgrounds = new List<Texture2D>();
            _korpBackgrounds.Add(_korpPatrankosBackground);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv2);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv3);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv4);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv5);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv6);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv7);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv8);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv9);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv10);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv11);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv12);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv13);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv14);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv15);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv16);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv17);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv18);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv19);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv20);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv21);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv22);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv23);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv24);
            _korpBackgrounds.Add(_korpPatrankosBackgroundv25);

            _korpPatrankosBeginCannonBackground = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON);
            _korpPatrankosBeginCannonBackgroundv2 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V2);
            _korpPatrankosBeginCannonBackgroundv3 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V3);
            _korpPatrankosBeginCannonBackgroundv4 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V4);
            _korpPatrankosBeginCannonBackgroundv5 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V5);
            _korpPatrankosBeginCannonBackgroundv6 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V6);
            _korpPatrankosBeginCannonBackgroundv7 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V7);
            _korpPatrankosBeginCannonBackgroundv8 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V8);
            _korpPatrankosBeginCannonBackgroundv9 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V9);
            _korpPatrankosBeginCannonBackgroundv10 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V10);
            _korpPatrankosBeginCannonBackgroundv11 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V11);
            _korpPatrankosBeginCannonBackgroundv12 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_BEGIN_CANNON_V12);
            _korpBackgroundsBeginCannon = new List<Texture2D>();
            _korpBackgroundsBeginCannon.Add(_korpPatrankosBeginCannonBackground);
            _korpBackgroundsBeginCannon.Add(_korpPatrankosBeginCannonBackgroundv2);
            _korpBackgroundsBeginCannon.Add(_korpPatrankosBeginCannonBackgroundv3);
            _korpBackgroundsBeginCannon.Add(_korpPatrankosBeginCannonBackgroundv4);
            _korpBackgroundsBeginCannon.Add(_korpPatrankosBeginCannonBackgroundv5);
            _korpBackgroundsBeginCannon.Add(_korpPatrankosBeginCannonBackgroundv6);
            _korpBackgroundsBeginCannon.Add(_korpPatrankosBeginCannonBackgroundv7);
            _korpBackgroundsBeginCannon.Add(_korpPatrankosBeginCannonBackgroundv8);
            _korpBackgroundsBeginCannon.Add(_korpPatrankosBeginCannonBackgroundv9);
            _korpBackgroundsBeginCannon.Add(_korpPatrankosBeginCannonBackgroundv10);
            _korpBackgroundsBeginCannon.Add(_korpPatrankosBeginCannonBackgroundv11);
            _korpBackgroundsBeginCannon.Add(_korpPatrankosBeginCannonBackgroundv12);

            _korpPatrankosCannonGameBackgroundv1 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_GAME_V1);
            _korpPatrankosCannonGameBackgroundv2 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_GAME_V2);
            _korpBackgroundsCannonGame = new List<Texture2D>();
            _korpBackgroundsCannonGame.Add(_korpPatrankosCannonGameBackgroundv1);
            _korpBackgroundsCannonGame.Add(_korpPatrankosCannonGameBackgroundv2);

            _korpPatrankosCannonGameEndBackgroundv1 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V1);
            _korpPatrankosCannonGameEndBackgroundv2 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V2);
            _korpPatrankosCannonGameEndBackgroundv3 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V3);
            _korpPatrankosCannonGameEndBackgroundv4 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V4);
            _korpPatrankosCannonGameEndBackgroundv5 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V5);
            _korpPatrankosCannonGameEndBackgroundv6 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V6);
            _korpPatrankosCannonGameEndBackgroundv7 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V7);
            _korpPatrankosCannonGameEndBackgroundv8 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V8);
            _korpPatrankosCannonGameEndBackgroundv9 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V9);
            _korpPatrankosCannonGameEndBackgroundv10 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V10);
            _korpPatrankosCannonGameEndBackgroundv11 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V11);
            _korpPatrankosCannonGameEndBackgroundv12 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V12);
            _korpPatrankosCannonGameEndBackgroundv13 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V13);
            _korpPatrankosCannonGameEndBackgroundv14 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V14);
            _korpPatrankosCannonGameEndBackgroundv15 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V15);
            _korpPatrankosCannonGameEndBackgroundv16 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V16);
            _korpPatrankosCannonGameEndBackgroundv17 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V17);
            _korpPatrankosCannonGameEndBackgroundv18 = Content.Load<Texture2D>(ASSET_NAME_KORPORACIJU_PATRANKOS_BACKGROUND_CANNON_END_V18);
            _korpBackgroundsCannonGameEnd = new List<Texture2D>();
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv1);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv2);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv3);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv4);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv5);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv6);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv7);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv8);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv9);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv10);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv11);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv12);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv13);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv14);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv15);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv16);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv17);
            _korpBackgroundsCannonGameEnd.Add(_korpPatrankosCannonGameEndBackgroundv18);

            _invertedSpriteSheet = _spriteSheetTexture.InvertColors(Color.Transparent);

            // Pass width and height
            _fadeInTexture = new Texture2D(GraphicsDevice, 1, 1);
            _fadeInTexture.SetData(new Color[] { Color.White });

            // Here we initialize TRex object and pass the texture and position where TRex needs to be drawn
            // we also set state to Idle and idel Animation to play = true
            _trex = new TRex(_runningGuySheetTexture, new Vector2(TREX_START_POS_X, TREX_START_POS_Y - TRex.COLLISION_SPRITE_HEIGHT), _sfxButtonPress);
            _trex.DrawOrder = 10;
            // we have to pass here a mehtod with a matching signature which takes an object and eventArgs objects as paramether
            // list and doesn't retun anything. we are subscribing to the event
            _trex.JumpComplete += trex_JumpComplete;
            _trex.Died += trex_Died;

            _scoreboard = new ScoreBoard(_spriteSheetTexture, new Vector2(SCORE_BOARD_POS_X, SCORE_BOARD_POS_Y), _trex, _sfxScoreReached);
            //_scoreboard.Score = 0;
            //_scoreboard.HighScore = 1;

            _inputController = new InputController(_trex);
            _backgroundManager = new BackgroundManager(_korpBackgrounds, _korpBackgroundsBeginCannon, _korpBackgroundsCannonGame, _korpBackgroundsCannonGameEnd, _entityManager, _trex);
            _obstacleManager = new ObstacleManager(_spriteSheetTexture, _cannonsSheetTexture, _officeFurnitureSheetTexture, _entityManager, _trex, _scoreboard, _backgroundManager, this, _cannonShot, _crowdChanting);
            _skyManager = new SkyManager(_trex, _spriteSheetTexture, _invertedSpriteSheet, _entityManager, _scoreboard);

            _groundManager = new GroundManager(_spriteSheetTexture, _entityManager, _trex);
            

            _gameOverScreen = new GameOverScreen(_spriteSheetTexture, this);
            _gameOverScreen.Position = new Vector2(GAME_WINDOW_WIDTH / 2 - GameOverScreen.GAME_OVER_SPRITE_WIDTH / 2, GAME_WINDOW_HEIGHT / 2 - 30);

            _entityManager.AddEntity(_trex);
            _entityManager.AddEntity(_groundManager);
            _entityManager.AddEntity(_backgroundManager);
            _entityManager.AddEntity(_scoreboard);
            _entityManager.AddEntity(_obstacleManager);
            _entityManager.AddEntity(_gameOverScreen);
            _entityManager.AddEntity(_skyManager);

            // for testing purpose
            _groundManager.Initialize();
            _backgroundManager.Initialize();

            LoadSaveState();

            _countDownAnimation = new SpriteAnimation();
            CreateCountDownAnimation();
            _countDownAnimation.Play();
            PlaySong();
            
        }

        private void CreateCountDownAnimation()
        {
            _countDownAnimation.ShouldLoop = false;
            Sprite numberThreeSprite = new Sprite(_spriteSheetTexture, NUMBER_3_SPRITE_POS_X, NUMBER_SPRITE_POS_Y, NUMBER_SPRITE_WIDTH, NUMBER_SPRITE_HEIGHT);
            Sprite numberTwoSprite = new Sprite(_spriteSheetTexture, NUMBER_2_SPRITE_POS_X, NUMBER_SPRITE_POS_Y, NUMBER_SPRITE_WIDTH, NUMBER_SPRITE_HEIGHT);
            Sprite numberOneSprite = new Sprite(_spriteSheetTexture, NUMBER_1_SPRITE_POS_X, NUMBER_SPRITE_POS_Y, NUMBER_SPRITE_WIDTH, NUMBER_SPRITE_HEIGHT);
            Sprite numberZeroSprite = new Sprite(_spriteSheetTexture, NUMBER_0_SPRITE_POS_X, NUMBER_SPRITE_POS_Y, NUMBER_SPRITE_WIDTH, NUMBER_SPRITE_HEIGHT);
            _numberFinishSprite = new Sprite(_spriteSheetTexture, NUMBER_0_SPRITE_POS_X, NUMBER_SPRITE_POS_Y, NUMBER_SPRITE_WIDTH, NUMBER_SPRITE_HEIGHT);

            // the first frame would start at 0 seconds. _runSpriteOne was initiated in constructor
            _countDownAnimation.AddFrame(numberThreeSprite, 0);
            // second frame is 1/20f._RunSpriteTwo was initiated in constructor.
            _countDownAnimation.AddFrame(numberTwoSprite, 1f);
            // another frame to indicate the end of the animation or in other word, how long animation should last
            _countDownAnimation.AddFrame(numberOneSprite, 2.5f);
            _countDownAnimation.AddFrame(numberZeroSprite, 4f);
            _countDownAnimation.AddFrame(_numberFinishSprite, 5.5f);
            _countDownAnimation.AddFrame(_numberFinishSprite, 7f);

        }

        private void trex_Died(object sender, EventArgs e)
        {
            State = GameState.GameOver;
            _obstacleManager.IsEnabled = false;
            _gameOverScreen.IsEnabled = true;
            
            _sfxHit.Play();
            StopSong();
            _korporacijuPatrankosSongElapsed = 0;
            
            if(_scoreboard.DisplayScore > _scoreboard.HighScore)
            {
                Debug.WriteLine("New Highscore set: " + _scoreboard.DisplayScore);
                _scoreboard.HighScore = _scoreboard.DisplayScore;
                _highscoreDate = DateTime.Now;
                
                SaveGame();
            }
        }

        private void trex_JumpComplete(object sender, EventArgs e)
        {
            if (State == GameState.Transition)
            {
                State = GameState.Playing;
                _trex.Initialize();
                _obstacleManager.IsEnabled = true;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);

            KeyboardState keyboardState = Keyboard.GetState();
            if (State == GameState.CountDown && _countDownAnimation.FrameCount != 0)
            {
                _trex.Speed = 0;
                _inputController.ProcessControls(gameTime);
            }
            else if (State == GameState.CountDown && _countDownAnimation.FrameCount == 0)
            {
                _trex.Speed = TRex.START_SPEED;
                State = GameState.Playing;
                
            }
            // Controls only work if game is in playing state
            if (State == GameState.Playing)
                _inputController.ProcessControls(gameTime);
            else if (State == GameState.Transition)
                _fadeInTexturePosX += (float) gameTime.ElapsedGameTime.TotalSeconds * FADE_IN_ANIMATION_SPEED;
            else if (State == GameState.Initial && _countDownAnimation.FrameCount == 0)
            {
                bool isStartKeyPressed = keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Space);
                bool wasStartKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Up) || _previousKeyboardState.IsKeyDown(Keys.Space);

                StartGame();
                if (isStartKeyPressed && !wasStartKeyPressed)
                {
                        
                }       
            }


            _countDownAnimation.Update(gameTime);

            // with this, if play animation = true, we update the PlaybackProgress by using elapsed time since last "update" call
            // If PlaybackProgress is more than our last frame's timestamp, Playback progress is reset. The frames we added when declaring
            // _trex instance are still the same with same timestamps and since PlaybackProgress is reset, it is < last frame timestamp, so 
            // our animation can start again
            _entityManager.Update(gameTime);

            _previousKeyboardState = keyboardState;

            // So that we know at which timestamp we are in the song
            if (_sei.State == SoundState.Playing)
            {
                _korporacijuPatrankosSongElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _korporacijuPatrankosSongElapsed = 0;
            }


        }

        protected override void Draw(GameTime gameTime)
        {
            if (_skyManager == null)
                GraphicsDevice.Clear(Color.White);
            else
                GraphicsDevice.Clear(_skyManager.ClearColor);
           
            // Begin() method below prepares stuff to be drawned
            _spriteBatch.Begin();
            // We will acumulate all Sprites (floor, Trex, clounds...) to one batch and draw that batch, rather than drawing each single
            // individual sprite. Draw() method sends the instruction to graphics card that draws sprites on screen.However, it is first
            // placed into a queue

            // here we draw the currentframe's sprite. How do we know which frame is current? we take the latest frame's timestamp which is 
            // still < PlaybackProgress. 
            _entityManager.Draw(_spriteBatch, gameTime);

            // Draw white rectangle over sprites if game is not yet Playing
            if (State == GameState.Initial || State == GameState.Transition)
            {
                _spriteBatch.Draw(
                    _fadeInTexture,
                    new Rectangle((int)Math.Round(_fadeInTexturePosX), 0, GAME_WINDOW_WIDTH, GAME_WINDOW_HEIGHT),
                    Color.White);
            }

            if (_countDownAnimation.FrameCount > 0)
            {
                _countDownAnimation.Draw(_spriteBatch, new Vector2(GAME_WINDOW_WIDTH/2, GAME_WINDOW_HEIGHT/3));

                if (_countDownAnimation.CurrentFrame.Sprite == _numberFinishSprite)
                {
                    _countDownAnimation.Clear();
                }
            }
            

            // when End() method is called, all queued sprite draws are finnaly drawn by the graphics card
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool StartGame()
        {
            if (State != GameState.Initial)
                return false;

            _scoreboard.Score = 0;
            State = GameState.Transition;
            // why we need dis?
            _trex.BeginJump();

            return true;

        }

        public bool Replay()
        {
            if (State != GameState.GameOver)
                return false;
            State = GameState.CountDown;

            _korporacijuPatrankosSongElapsed = 0;
            PlaySong();
            

            _scoreboard.Score = 0;
            _trex.Initialize();
            _obstacleManager.Reset();
            _obstacleManager.IsEnabled = true;
            _gameOverScreen.IsEnabled = false;
            _groundManager.Initialize();
            _inputController.BlockInputTemporarily();
            CreateCountDownAnimation();
            _countDownAnimation.Play();
            _countDownAnimation.ResetPlaybackProgress();
            _backgroundManager.Initialize();

            return true;
        }

        public void SaveGame()
        {
            // same as using saveState.HighScore = _scoreboard.HighScore;
            SaveState saveState = new SaveState()
            {
                HighScore = _scoreboard.HighScore,
                HighscoreDate = _highscoreDate
            };

            // saving to harddisk
            try
            {
                // async soon as the socre of using block is left, the dispose method will automatically be called
                using(FileStream fileStream = new FileStream(SAVE_FILE_NAME, FileMode.Create))
                {
                    // serialize our instance of saveState and write that into filestream by using binary formater
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, saveState);
                }

            }
            catch(Exception ex)
            {
                Debug.WriteLine("An error occured while saving the game: " + ex.Message);
            }
        }

        public void LoadSaveState()
        {

            try
            {
                // async soon as the socre of using block is left, the dispose method will automatically be called
                using (FileStream fileStream = new FileStream(SAVE_FILE_NAME, FileMode.OpenOrCreate))
                {
                    // deserialize our instance of saveState and write that into filestream by using binary formater
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    SaveState saveState = binaryFormatter.Deserialize(fileStream) as SaveState;
                
                    if(saveState != null && _scoreboard != null)
                    {
                        _scoreboard.HighScore = saveState.HighScore;
                        _highscoreDate = saveState.HighscoreDate;
                    }

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occured while loading the game: " + ex.Message);
            }

        }

        public void PlaySong()
        {
            _sei.Play();
            _sei.Volume = 0.8f;
        }

        public void StopSong()
        {
            _sei.Stop();
        }
    }

    
}
